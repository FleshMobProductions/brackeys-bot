﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using BrackeysBot.Commands;

namespace BrackeysBot
{
    public static class UserExtensions
    {
        public static async Task MuteAsync(this IGuildUser user, ICommandContext context)
            => await user.AddRoleAsync(GetMutedRole(context));
        public static async Task UnmuteAsync(this IGuildUser user, ICommandContext context)
            => await user.RemoveRoleAsync(GetMutedRole(context));

        private static IRole GetMutedRole(ICommandContext context)
            => context.Guild.GetRole((context as BrackeysBotContext).Configuration.MutedRoleID);

        public static string Mention(this ulong userId) 
            => $"<@{userId}>";

        public static PermissionLevel GetPermissionLevel(this IGuildUser user, ICommandContext context)
        {
            if (context is BrackeysBotContext botContext)
            {
                BotConfiguration config = botContext.Configuration;

                if (user.GuildPermissions.Has(GuildPermission.Administrator))
                    return PermissionLevel.Administrator;
                if (user.RoleIds.Contains(config.ModeratorRoleID))
                    return PermissionLevel.Moderator;
                if (user.RoleIds.Contains(config.GuruRoleID))
                    return PermissionLevel.Guru;
            }

            return PermissionLevel.Default;
        }
    }
}
