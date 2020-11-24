using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MiraiZuraBot.Attributes;
using MiraiZuraBot.Helpers;
using MiraiZuraBot.Services.LanguageService;
using MiraiZuraBot.Services.RolesService;
using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiZuraBot.Commands.RolesCommands
{
    [GroupLang("Role", "Roles")]
    class AssignRolesCommand : BaseCommandModule
    {
        private AssignRolesService _assignRolesService;
        private LanguageService _languageService;
        private Translator _translator;

        public AssignRolesCommand(AssignRolesService assignRolesService, LanguageService languageService, Translator translator)
        {
            _assignRolesService = assignRolesService;
            _languageService = languageService;
            _translator = translator;
        }

        [Command("pokazRole")]
        [Aliases("showRoles")]
        [CommandLang("pokazRole", "showRoles")]
        [DescriptionLang("Pokazuje role, które można przydzielić sobie na tym serwerze.", "Shows the roles that you can assign yourself on this server.")]
        public async Task ShowRoles(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var assignRoles = _assignRolesService.GetRoles(ctx.Guild.Id);

            if(assignRoles.Count == 0)
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleRolesOnServer"), _translator.GetString(lang, "roleNoRolesOnServer"));
            }
            else
            {
                // Get server roles.
                var serverRoles = ctx.Guild.Roles;

                List<DiscordRole> discordRoles = new List<DiscordRole>();
                foreach (ulong roleId in assignRoles)
                {
                    discordRoles.Add(serverRoles.Where(p => p.Value.Id == roleId).FirstOrDefault().Value);
                }

                List<DiscordRole> sortedRoles = discordRoles.OrderBy(o => o.Name).ToList();
                await PostLongMessageHelper.PostLongMessage(ctx, sortedRoles.Select(p => p.Name).ToList(), _translator.GetString(lang, "roleRolesOnServer"), ", ");
            } 
        }

        [Command("nadajRole")]
        [Aliases("assignRole")]
        [CommandLang("nadajRole", "assignRole")]
        [DescriptionLang("Dodaje tobie role z listy ról.", "Assign role to you from the role list.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task GiveRole(CommandContext ctx, [DescriptionLang("Pełna nazwa roli", "Full role name"), ParameterLang("Rola", "Role"), RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var serverRoles = ctx.Guild.Roles;
            var role = serverRoles.Select(p => p).Where(q => q.Value.Name == message).FirstOrDefault();
            
            if(role.Value == null)
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleDoesntExist"));
                return;
            }

            /*if (HasUserRole(ctx.Member, role.Value))
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleAlreadyHas"));
                return;
            }*/

            if (_assignRolesService.IsRoleOnList(role.Value.Id))
            {
                if (!CanBotModifyThisRole(role.Value, ctx.Guild.CurrentMember.Roles.ToList()))
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleBotRolesTooLowToGrant"));
                    return;
                }

                await ctx.Member.GrantRoleAsync(role.Value, _translator.GetString(lang, "roleGrantedServerLog"));
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleGranted"));
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleNotOnList"));
            }
        }

        [Command("odbierzRole")]
        [Aliases("removeRole")]
        [CommandLang("odbierzRole", "removeRole")]
        [DescriptionLang("Odbiera tobie role z listy ról.", "Remove role from you from the role list.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task RemoveRole(CommandContext ctx, [DescriptionLang("Pełna nazwa roli", "Full role name"), ParameterLang("Rola", "Role"), RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var serverRoles = ctx.Guild.Roles;
            var role = serverRoles.Select(p => p).Where(q => q.Value.Name == message).FirstOrDefault();

            if (role.Value == null)
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleDoesntExist"));
                return;
            }

            /*if (!HasUserRole(ctx.Member, role.Value))
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleDoesntHas"));
                return;
            }*/

            if (_assignRolesService.IsRoleOnList(role.Value.Id))
            {
                if (!CanBotModifyThisRole(role.Value, ctx.Guild.CurrentMember.Roles.ToList()))
                {
                    await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleBotRolesTooLowToRemove"));
                    return;
                }
                await ctx.Member.RevokeRoleAsync(role.Value, _translator.GetString(lang, "roleRemovedServerLog"));
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleRemoved"));
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleNotOnList"));
            }
        }


        [Command("dodajRole")]
        [Aliases("addRole")]
        [CommandLang("dodajRole", "addRole")]
        [DescriptionLang("Dodaje rolę do listy roli jakie mogą sobie przydzielać członkowie serwera.", "Adds a role to the role list that server members can assign.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task AddRole(CommandContext ctx, [DescriptionLang("Pełna nazwa roli", "Full role name"), ParameterLang("Rola", "Role"), RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var serverRoles = ctx.Guild.Roles;
            var role = serverRoles.Select(p => p).Where(q => q.Value.Name == message).FirstOrDefault();

            if (role.Value == null)
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleDoesntExist"));
                return;
            }

            if (_assignRolesService.IsRoleOnList(role.Value.Id))
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleAlreadyOnList"));
                return;
            }

            // User who triggered is owner, we can add role without problem or user who triggered isn't owner, we need to check if role is lower than the highest role he has
            var userTheHighestRolePosition = GetTheHighestRolePosition(ctx.Member.Roles.ToList());
            if (ctx.User == ctx.Guild.Owner || role.Value.Position < userTheHighestRolePosition)
            {
                // Add role to database
                _assignRolesService.AddRoleToDatabase(ctx.Guild.Id, role.Value.Id);
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleAddedToList"));
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleCantAddToList"));
            }
        }

        [Command("usunRole")]
        [Aliases("deleteRole")]
        [CommandLang("usunRole", "deleteRole")]
        [DescriptionLang("Usuwa rolę z listy roli jakie mogą sobie przydzielać członkowie serwera.", "Removes a role from the role list that can be assigned by server members.")]
        [RequireBotPermissions(DSharpPlus.Permissions.ManageRoles)]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageRoles)]
        public async Task DeleteRole(CommandContext ctx, [DescriptionLang("Pełna nazwa roli", "Full role name"), ParameterLang("Rola", "Role"), RemainingText] string message)
        {
            await ctx.TriggerTypingAsync();

            var lang = _languageService.GetServerLanguage(ctx.Guild.Id);

            var serverRoles = ctx.Guild.Roles;
            var role = serverRoles.Select(p => p).Where(q => q.Value.Name == message).FirstOrDefault();

            if (role.Value == null)
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleDoesntExist"));
                return;
            }

            if (!_assignRolesService.IsRoleOnList(role.Value.Id))
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleNotOnList"));
                return;
            }

            // User who triggered is owner, we can add role without problem or user who triggered isn't owner, we need to check if role is lower than the highest role he has
            var userTheHighestRolePosition = GetTheHighestRolePosition(ctx.Member.Roles.ToList());
            if (ctx.User == ctx.Guild.Owner || role.Value.Position < userTheHighestRolePosition)
            {
                // Add role to database
                _assignRolesService.RemoveRoleFromDatabase(ctx.Guild.Id, role.Value.Id);
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleRemovedFromList"));
            }
            else
            {
                await PostEmbedHelper.PostEmbed(ctx, _translator.GetString(lang, "roleSystem"), _translator.GetString(lang, "roleCantRemoveFromList"));
            }
        }

        private bool HasUserRole(DiscordMember member, DiscordRole role)
        {
            foreach (var memberRole in member.Roles)
            {
                if (memberRole == role)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CanBotModifyThisRole(DiscordRole role, List<DiscordRole> botRoles)
        {
            int highestBotRole = GetTheHighestRolePosition(botRoles);
            if(role.Position < highestBotRole)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int GetTheHighestRolePosition(List<DiscordRole> roles)
        {
            int position = 0;
            foreach(var role in roles)
            {
                if(role.Position > position)
                {
                    position = role.Position;
                }
            }

            return position;
        }
    }
}
