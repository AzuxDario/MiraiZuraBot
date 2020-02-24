using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Exceptions;
using DSharpPlus.Net.WebSocket;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Core
{
    class Bot
    {
        public struct ConfigJson
        {
            [JsonProperty("token")]
            public string Token { get; private set; }

            [JsonProperty("prefix")]
            public string CommandPrefix { get; private set; }

            [JsonProperty("developer")]
            public ulong Developer { get; private set; }
        }


        public static DiscordClient DiscordClient { get; set; }
        private CommandsNextExtension _commands { get; set; }
        public static ConfigJson configJson { get; private set; }

        public void Run()
        {
            Connect();
            SetNetworkParameters();
        }

        private async void Connect()
        {
            var json = "";
            string settingsFile;
#if DEBUG
            settingsFile = "debug.json";
#else
            settingsFile = "release.json";
#endif
            using (var fs = File.OpenRead(settingsFile))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();

            configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var connectionConfig = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true

#if DEBUG
                ,
                // For Windows 7 I'm using to test
                WebSocketClientFactory = WebSocket4NetCoreClient.CreateNew
#endif
            };

            DiscordClient = new DiscordClient(connectionConfig);
            DiscordClient.MessageCreated += DiscordClient_MessageCreatedAsync;
            DiscordClient.MessageUpdated += DiscordClient_MessageUpdatedAsync;
            DiscordClient.MessageReactionAdded += DiscordClient_MessageReactionAddedAsync;


            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new[] { configJson.CommandPrefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                CaseSensitive = false
            };

            _commands = DiscordClient.UseCommandsNext(commandsConfig);
            _commands.SetHelpFormatter<CustomHelpFormatter>();
            _commands.CommandExecuted += Commands_CommandExecuted;
            _commands.CommandErrored += Commands_CommandErrored;
            RegisterCommands();

            await DiscordClient.ConnectAsync();
        }

        private async Task DiscordClient_MessageCreatedAsync(DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            if (e.Channel.IsPrivate == false)
            {
                try
                {
                    EmojiCounterService emojiCounterService = new EmojiCounterService();
                    await emojiCounterService.CountEmojiInMessage(e.Message);
                }
                catch (Exception ie)
                {
                    Console.WriteLine("Error: Counting emoji in new message.");
                    Console.WriteLine("Exception: " + ie.Message);
                    Console.WriteLine("Inner Exception: " + ie?.InnerException?.Message);
                    Console.WriteLine("Stack trace: " + ie.StackTrace);
                }
                try
                {
                    EmojiAddService emojiAddService = new EmojiAddService();
                    await emojiAddService.AddEmojiOnServer(e.Guild, e.Message);
                }
                catch (Exception ie)
                {
                    Console.WriteLine("Error: Adding reactions.");
                    Console.WriteLine("Exception: " + ie.Message);
                    Console.WriteLine("Inner Exception: " + ie?.InnerException?.Message);
                    Console.WriteLine("Stack trace: " + ie.StackTrace);
                }
            }
        }

        private async Task DiscordClient_MessageUpdatedAsync(DSharpPlus.EventArgs.MessageUpdateEventArgs e)
        {
            if (e.Channel.IsPrivate == false)
            {
                try
                {
                    EmojiCounterService emojiCounterService = new EmojiCounterService();
                    await emojiCounterService.CountEmojiInMessage(e.Message);
                }
                catch (Exception ie)
                {
                    Console.WriteLine("Error: Counting emoji in edited message..");
                    Console.WriteLine("Exception: " + ie.Message);
                    Console.WriteLine("Inner Exception: " + ie?.InnerException?.Message);
                    Console.WriteLine("Stack trace: " + ie.StackTrace);
                }
            }
        }

        private async Task DiscordClient_MessageReactionAddedAsync(DSharpPlus.EventArgs.MessageReactionAddEventArgs e)
        {
            if (e.Channel.IsPrivate == false)
            {
                try
                {
                    EmojiCounterService emojiCounterService = new EmojiCounterService();
                    await emojiCounterService.CountEmojiReaction(e.User, e.Emoji, e.Channel);
                }
                catch (Exception ie)
                {
                    Console.WriteLine("Error: Counting emoji in reactions.");
                    Console.WriteLine("Exception: " + ie.Message);
                    Console.WriteLine("Inner Exception: " + ie?.InnerException?.Message);
                    Console.WriteLine("Stack trace: " + ie.StackTrace);
                }
            }
        }

        private void SetNetworkParameters()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private void RegisterCommands()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyTypes = assembly.GetTypes();

            var registerCommandsMethod = _commands.GetType().GetMethods()
                .FirstOrDefault(p => p.Name == "RegisterCommands" && p.IsGenericMethod);

            foreach (var type in assemblyTypes)
            {
                var attributes = type.GetCustomAttributes();
                if (attributes.Any(p => p.GetType() == typeof(CommandsGroupAttribute)))
                {
                    var genericRegisterCommandMethod = registerCommandsMethod.MakeGenericMethod(type);
                    genericRegisterCommandMethod.Invoke(_commands, null);
                }
            }
        }

        private Task Commands_CommandExecuted(CommandExecutionEventArgs e)
        {
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Info, "ExampleBot", $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'", DateTime.Now);

            return Task.FromResult(0);
        }

        private async Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Error, "ExampleBot", $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

            switch (e.Exception)
            {
                case Checks​Failed​Exception _:
                    {
                        await e.Context.Channel.SendMessageAsync("Brak wystarczających uprawnień aby dokończyć akcje.");
                        break;
                    }
                case UnauthorizedException _:
                    {
                        await e.Context.Member.SendMessageAsync("Brak wystarczających uprawnień aby dokończyć akcje.");
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

    }
}
