﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Services.LanguageService;
using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.StatisticsCommands
{
    [GroupLang("Statystyka", "Statistics")]
    class GithubCommand : BaseCommandModule
    {
        private readonly string GithubLink = "https://github.com/AzuxDario/MiraiZuraBot";
        private LanguageService _languageService;
        private Translator _translator;

        public GithubCommand(LanguageService languageService, Translator translator)
        {
            _languageService = languageService;
            _translator = translator;
        }

        [Command("github")]
        [CommandLang("github", "github")]
        [DescriptionLang("Zwraca link do repozytorium bota.", "Shows link to bot repository.")]
        public async Task Github(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            await PostEmbedHelper.PostEmbed(ctx, "Github", string.Format(_translator.GetString(lang, "githubLink"), GithubLink));
        }
    }
}
