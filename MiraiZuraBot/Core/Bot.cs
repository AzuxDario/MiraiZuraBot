using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Exceptions;
using DSharpPlus.Net.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Handlers.EmojiHandlers;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Helpers.SchoolidoluHelper;
using MiraiZuraBot.Helpers.TimeHelper;
using MiraiZuraBot.Services.AnnouncementService;
using MiraiZuraBot.Services.EmojiService;
using MiraiZuraBot.Services.LanguageService;
using MiraiZuraBot.Services.RandomMessagesService;
using MiraiZuraBot.Services.RolesService;
using MiraiZuraBot.Services.SchoolidoluService;
using MiraiZuraBot.Services.TriviaService;
using MiraiZuraBot.Translators;
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
#if DEBUG
        public static readonly string botname = "Mirai Zura Test";
#else
        public static readonly string botname = "Mirai Zura";
#endif

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
        private LanguageService _languageService;
        private Translator _translator;

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
            };

            _translator = new Translator();
            _languageService = new LanguageService(_translator);

            DiscordClient = new DiscordClient(connectionConfig);
            DiscordClient.MessageCreated += DiscordClient_MessageCreatedAsync;
            DiscordClient.MessageUpdated += DiscordClient_MessageUpdatedAsync;
            DiscordClient.MessageReactionAdded += DiscordClient_MessageReactionAddedAsync;


            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new[] { configJson.CommandPrefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                CaseSensitive = false,
                IgnoreExtraArguments = true,
                Services = BuildDependencies()
            };

            _commands = DiscordClient.UseCommandsNext(commandsConfig);
            _commands.SetHelpFormatter<CustomHelpFormatter>();
            _commands.CommandExecuted += Commands_CommandExecuted;
            _commands.CommandErrored += Commands_CommandErrored;
            RegisterCommands();

            await DiscordClient.ConnectAsync();
        }

        private ServiceProvider BuildDependencies()
        {
            return new ServiceCollection()

            // Singletons
            .AddSingleton(_translator)

            // Helpers
            .AddScoped<SchoolidoluHelper>()
            .AddScoped<TimeHelper>()

            // Services
            .AddScoped<AssignRolesService>()
            .AddScoped<BirthdaysService>()
            .AddScoped<EmojiCounterService>()
            .AddScoped<LanguageService>()
            .AddScoped<RandomMessageService>()
            .AddScoped<SchoolidoluService>()
            .AddScoped<TriviaService>()

            .BuildServiceProvider();
        }

        private async Task DiscordClient_MessageCreatedAsync(DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            if (e.Channel.IsPrivate == false)
            {
                try
                {
                    EmojiCounterHandler emojiCounterHandler = new EmojiCounterHandler();
                    await emojiCounterHandler.CountEmojiInMessage(e.Message);
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
                    EmojiAddHandler emojiAddHanlder = new EmojiAddHandler();
                    await emojiAddHanlder.AddEmojiOnServer(e.Guild, e.Message);
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
                    EmojiCounterHandler emojiCounterHandler = new EmojiCounterHandler();
                    await emojiCounterHandler.CountEmojiInMessage(e.Message);
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
                    EmojiCounterHandler emojiCounterHandler = new EmojiCounterHandler();
                    await emojiCounterHandler.CountEmojiReaction(e.User, e.Emoji, e.Channel);
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
                if (attributes.Any(p => p.GetType() == typeof(GroupLangAttribute)))
                {
                    var genericRegisterCommandMethod = registerCommandsMethod.MakeGenericMethod(type);
                    genericRegisterCommandMethod.Invoke(_commands, null);
                }
            }
        }

        private Task Commands_CommandExecuted(CommandExecutionEventArgs e)
        {
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Info, botname, $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'", DateTime.Now);

            return Task.FromResult(0);
        }

        private async Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Error, botname, $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}.", DateTime.Now);

            var lang = _languageService.GetServerLanguage(e.Context.Guild.Id);

            switch (e.Exception)
            {
                case Checks​Failed​Exception ex:
                    {
                        StringBuilder messageToSend = new StringBuilder();
                        messageToSend.Append(_translator.GetString(lang, "errorNotEnoughPermissions")).AppendLine();

                        var failedChecks = ex.FailedChecks;
                        foreach(var failedCheck in failedChecks)
                        {
                            if (failedCheck is RequireBotPermissionsAttribute failBot)
                            {
                                messageToSend.Append(_translator.GetString(lang, "errorINeed"));
                                messageToSend.Append(": ");
                                messageToSend.Append(failBot.Permissions.ToPermissionString());
                                messageToSend.AppendLine();
                            }
                            else if (failedCheck is RequireUserPermissionsAttribute failUser)
                            {
                                messageToSend.Append(_translator.GetString(lang, "errorYouNeed"));
                                messageToSend.Append(": ");
                                messageToSend.Append(failUser.Permissions.ToPermissionString());
                                messageToSend.AppendLine();
                            }
                            else if (failedCheck is RequireOwnerAttribute)
                            {
                                messageToSend.Append(_translator.GetString(lang, "errorOnlyMyOwner"));
                                messageToSend.AppendLine();
                            }
                        }

                        await PostEmbedHelper.PostEmbed(e.Context, _translator.GetString(lang, "error"), messageToSend.ToString());
                        break;
                    }
                case UnauthorizedException _:
                    {
                        await e.Context.Member.SendMessageAsync(_translator.GetString(lang, "errorNotEnoughPermissions"));
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
