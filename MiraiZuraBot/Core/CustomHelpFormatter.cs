﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Services.LanguageService;
using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MiraiZuraBot.Core
{
    class CustomHelpFormatter : BaseHelpFormatter
    {
        private LanguageService _languageService;
        private Translator _translator;
        private Translator.Language _lang;
        private string _commandName;
        private string _commandDescription;
        private List<string> _aliases;
        private List<string> _parameters;
        private Dictionary<string, List<string>> _subCommands;

        private const string Color = "#5588EE";

        public CustomHelpFormatter(CommandContext ctx, LanguageService languageService, Translator translator) : base(ctx)
        {
            _languageService = languageService;
            _translator = translator;
            _lang = _languageService.GetServerLanguage(ctx.Guild.Id);
            _aliases = new List<string>();
            _parameters = new List<string>();
            _subCommands = new Dictionary<string, List<string>>();            
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            _commandDescription = _translator.GetString(_lang, "helpCommandNoDescription");

            foreach (var attribute in command.CustomAttributes)
            {
                if (attribute is CommandLangAttribute comm)
                {
                    _commandName = comm.GetCommand(_lang);
                }
                else if (attribute is DescriptionLangAttribute desc)
                {
                    _commandDescription = desc.GetDescription(_lang);
                }
                else if (attribute is AliasLangAttribute al)
                {
                    var aliases = al.GetAliases(_lang);
                    foreach (var alias in aliases)
                    {
                        _aliases.Add($"`{alias}`");
                    }
                }
            }

            if (command.Overloads.Count > 0)
            {
                foreach (var argument in command.Overloads[0].Arguments)
                {
                    var argumentBuilder = new StringBuilder();
                    string argumentName = argument.Name;
                    string argumentDesc = null;

                    foreach (var attribute in argument.CustomAttributes)
                    {
                        if (attribute is DescriptionLangAttribute desc)
                        {
                            argumentDesc = desc.GetDescription(_lang);
                        }
                        else if (attribute is ParameterLangAttribute param)
                        {
                            argumentName = param.GetParameterName(_lang);
                        }
                    }

                        argumentBuilder.Append($"`{argumentName}`: {argumentDesc}");

                    if (argument.DefaultValue != null)
                    {
                        argumentBuilder.Append($" {_translator.GetString(_lang, "helpDefaultValue")}: {argument.DefaultValue}");
                    }

                    _parameters.Add(argumentBuilder.ToString());
                }
            }
            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyTypes = assembly.GetTypes();

            foreach (var type in assemblyTypes)
            {
                var attributes = type.GetCustomAttributes();
                var commandGroupAttribute = attributes.FirstOrDefault(p => p is GroupLangAttribute);

                if (commandGroupAttribute != null)
                {
                    var groupAttribute = (GroupLangAttribute)attributes.First(p => p is GroupLangAttribute);
                    var commandHandlers = type.GetMethods();

                    foreach (var method in commandHandlers)
                    {
                        var methodAttributes = method.GetCustomAttributes();
                        var commandAttribute = (CommandLangAttribute)methodAttributes.FirstOrDefault(p => p is CommandLangAttribute);

                        if (commandAttribute != null)
                        {
                            if (!_subCommands.ContainsKey(groupAttribute.GetGroup(_lang)))
                            {
                                _subCommands.Add(groupAttribute.GetGroup(_lang), new List<string>());
                            }

                            _subCommands[groupAttribute.GetGroup(_lang)].Add($"`{commandAttribute.GetCommand(_lang)}`");
                        }

                        _subCommands[groupAttribute.GetGroup(_lang)] = _subCommands[groupAttribute.GetGroup(_lang)].OrderBy(p => p).ToList();
                    }
                }
            }

            return this;
        }

        public override CommandHelpMessage Build()
        {
            var embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Color)
            };

            return _commandName == null ? BuildGeneralHelp(embed) : BuildCommandHelp(embed);
        }

        private CommandHelpMessage BuildGeneralHelp(DiscordEmbedBuilder embed)
        {
            embed.AddField(_translator.GetString(_lang, "help"), string.Format(_translator.GetString(_lang, "helpDescription"), Bot.configJson.CommandPrefix));

            var orderedSubCommands = _subCommands.OrderBy(p => p.Key).ToList();
            foreach (var group in orderedSubCommands)
            {
                embed.AddField(group.Key, string.Join(", ", group.Value));
            }

            return new CommandHelpMessage(string.Empty, embed);
        }

        private CommandHelpMessage BuildCommandHelp(DiscordEmbedBuilder embed)
        {
            embed.AddField(_commandName, _commandDescription);

            if (_aliases.Count > 0) embed.AddField(_translator.GetString(_lang, "helpAliases"), string.Join(", ", _aliases));
            if (_parameters.Count > 0) embed.AddField(_translator.GetString(_lang, "helpParameters"), string.Join("\r\n", _parameters));

            return new CommandHelpMessage(string.Empty, embed);
        }
    }
}
