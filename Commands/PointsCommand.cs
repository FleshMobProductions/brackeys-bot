﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Linq;
using Discord.WebSocket;

namespace BrackeysBot.Commands
{
    public class PointsCommand : ModuleBase
    {
        private readonly CommandService commands;

        public PointsCommand(CommandService commands)
        {
            this.commands = commands;
        }

        [Command("points")]
        public async Task DisplayPointsSelf ()
        {
            var user = Context.User;
            int karma = BrackeysBot.Karma.GetKarma(user);
            await ReplyAsync($"{ user.Mention }, you have { karma } karma.");
        }

        [Command("points")]
        public async Task DisplayPointsUser (SocketGuildUser user)
        {
            int remainingSeconds;
            if (KarmaTable.CheckPointsUserCooldownExpired(Context.User, out remainingSeconds))
            {
                int total = BrackeysBot.Karma.GetKarma(user);
                string pointsDisplay = $"{ total } point{ (total != 1 ? "s" : "") }";
                await ReplyAsync($"{ user.Username } has { pointsDisplay }.");

                KarmaTable.AddPointsUserCooldown(Context.User);
            }
            else
            {
                string displaySeconds = $"{ remainingSeconds } second{ (remainingSeconds != 1 ? "s" : "") }";
                await ReplyAsync($"{ Context.User.Mention }, please wait { displaySeconds } before using that command again.");
            }
        }
    }
}