﻿using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace ProjectOrigin
{
    /// <summary>Connection class for connecting the bot to Discord and handling all necessary events. All input
    /// the bot receives comes through here.</summary>
    public class Connection
    {
        private readonly DiscordLogger _logger;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        /// <summary>Connection constructor to set up a logger, client, and command service</summary>
        /// <param name="logger">The logger logs all commands. By default this prints the log to the console.</param>
        /// <param name="client">The client with all configs already set up.</param>
        /// <param name="commands">The command service with all configs already set up.</param>
        public Connection(DiscordLogger logger, DiscordSocketClient client, CommandService commands)
        {
            _logger = logger;
            _client = client;
            _commands = commands;
        }

        /// <summary>Connects the bot to Discord and assigns events to event handling methods.</summary>
        /// <param name="config">The bot's config for default values and the token.</param>
        public async Task ConnectAsync(BotConfig config)
        {
            _client.Log += _logger.Log;

            await _client.LoginAsync(TokenType.Bot, config.Token);
            await _client.StartAsync();

            // Set the client to static classes
            MessageHandler.SetClient(_client);
            ClientAccess.SetClient(_client);

            // Assign events to their handlers
            _client.Ready += ClientReady;

            _client.JoinedGuild += HandleGuildJoin;

            _client.MessageReceived += MessageRecieved;

            _client.ReactionAdded += ReactionReceived;

            _commands.CommandExecuted += CommandExecutedAsync;

            _client.SlashCommandExecuted += SlashCommandHandler;

            _client.ButtonExecuted += ButtonHandler;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            await Task.Delay(-1);
        }

        /// <summary>Event handler for when the bot finishes connecting.</summary>
        private async Task ClientReady()
        {
            // Delete ALL Slash commands- to be used to fix mistakes
            /*
            IReadOnlyCollection<SocketApplicationCommand> commands = await _client.GetGlobalApplicationCommandsAsync();
            foreach (SocketApplicationCommand cmd in commands)
            {
                await cmd.DeleteAsync();
            }
            */

            // Build all slash commands
            await _client.CreateGlobalApplicationCommandAsync(new SlashCommandBuilder
            {
                Name = "menu",
                Description = "Open the Main Menu"
            }.Build());
        }

        /// <summary>Event handler for when the bot joins a new guild.</summary>
        /// <param name="guild">The guild that was joined.</param>
        private async Task HandleGuildJoin(SocketGuild guild)
        {
            await guild.DefaultChannel.SendMessageAsync("Hello! I am MonBot. By default, my command prefix is **!** \nIf you would like to change this or other settings, type **!settings**. \nIf you would like to learn more about me and what I do, use **!info**. \nTo create a character, type **!startadventure**. \nTo create a town, type **!foundtown**.");
        }

        /// <summary>Event handler for when a new message appears anywhere.</summary>
        /// <param name="messageParam">The sent message.</param>
        private async Task MessageRecieved(SocketMessage messageParam)
        {
            try
            {
                //Don't process the command if it was a system message
                var message = messageParam as SocketUserMessage;
                if (message == null) return;

                //Create a number to track where the prefix ends and the command begins
                int argPos = 0;

                //If the user who sent that message is expecting input, parse the message for inputs.
                var user = UserHandler.GetUser(message.Author.Id);
                if (user.ExpectedInput != -1)
                {
                    var con = new SocketCommandContext(_client, message);
                    await MessageHandler.ParseExpectedInput(message, user, con);
                }

                // Determine if the message is a command based on the prefix and make sure no bots trigger commands
                if (!(message.HasCharPrefix('!', ref argPos) ||
                    message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                    message.Author.IsBot)
                    return;

                // Create a WebSocket-based command context based on the message
                var context = new SocketCommandContext(_client, message);

                //Update user's info CHANGE DM VALUE
                UserHandler.UpdateUserInfo(context.User.Id, 0, context.User.Username, context.User.Mention, context.User.GetAvatarUrl());

                // Execute the command with the command context we just
                // created, along with the service provider for precondition checks.
                await _commands.ExecuteAsync(context, argPos, services: null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>Event handler for when a reaction is added anywhere.</summary>
        /// <param name="cacheMessage"></param>
        /// <param name="channel"></param>
        /// <param name="reaction"></param>
        public async Task ReactionReceived(Cacheable<IUserMessage, ulong> cacheMessage, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            try
            {
                if (reaction.User.Value.IsBot)
                    return;

                //TO BE REMOVED- SAVE TOWNS EVERY 5-10 MINS INSTEAD. CREATE SAFE SHUTDOWN METHOD THAT SAVES TOWNS/USERS ETC.
                TownHandler.SaveTowns();

                var message = await cacheMessage.GetOrDownloadAsync();
                var user = UserHandler.GetUser(reaction.UserId);
                //Console.WriteLine($"Cache {cacheMessage.Id}\nMessage {message.Id}\nReaction: {reaction.MessageId}");

                if (user.ReactionMessages.ContainsKey(message.Id))
                {
                    await EmoteCommands.ParseEmote(user, message, reaction);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>Event handler for when a command is executed.</summary>
        /// <param name="command"></param>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // if a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified)
            {
                Console.WriteLine($"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
                return;
            }


            // log success to the console and exit this method
            if (result.IsSuccess)
            {
                Console.WriteLine($"Command [{command.Value.Name}] executed for -> [{context.User.Username}]");
                return;
            }

            // failure scenario, lets let the user know
            await context.Channel.SendMessageAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!");
        }

        /// <summary>Event handler for when a slash command is executed.</summary>
        /// <param name="cmd">The command that was executed.</param>
        public async Task SlashCommandHandler(SocketSlashCommand cmd)
        {
            try 
            {
                switch (cmd.Data.Name)
                {
                    case "menu":
                        await SlashCommands.MainMenu(cmd);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>Event handler for when a button is pressed.</summary>
        /// <param name="comp">The button component that was pressed.</param>
        public async Task ButtonHandler(SocketMessageComponent comp)
        {
            try
            {
                switch (comp.Data.CustomId)
                {
                    case "MainMenuLocation":
                        await ButtonCommands.RespondNotImplemented(comp, "LOCATION");
                        break;
                    case "MainMenuParty":
                        await ButtonCommands.PartyMenu(comp);
                        break;
                    case "MainMenuBag":
                        await ButtonCommands.RespondNotImplemented(comp, "BAG");
                        break;
                    case "MainMenuDex":
                        await ButtonCommands.RespondNotImplemented(comp, "DEX");
                        break;
                    case "MainMenuTeam":
                        await ButtonCommands.RespondNotImplemented(comp, "TEAM");
                        break;
                    case "MainMenuPvP":
                        await ButtonCommands.RespondNotImplemented(comp, "PVP");
                        break;
                    case "MainMenuSettings":
                        await ButtonCommands.RespondNotImplemented(comp, "SETTINGS");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }
    }

}