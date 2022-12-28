using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectOrigin
{
    /// <summary>Static class for handling users- such as retrieving and storing user information.</summary>
    public static class UserHandler
    {
        /// <summary>The filepath for user data.</summary>
        public static readonly string filepath;
        /// <summary>The list of users currently loaded into memory.</summary>
        private static Dictionary<ulong, UserAccount> _dic;
        /// <summary>The JsonStorage object used for storing and retrieving data from user files.</summary>
        private static JsonStorage _jsonStorage;

        /// <summary>Static constructor for setting filepath, dictionary, and storage</summary>
        static UserHandler()
        {
            //Access JsonStorage to load user list into memory 
            filepath = "Project/Data/UserData/UserList";

            _dic = new Dictionary<ulong, UserAccount>();
            _jsonStorage = new JsonStorage();
        }

        /// <summary>Get user with the ID specified in ContextIds.</summary>
        /// <param name="ids">The ContextIds that contains the user's ID.</param>
        /// <returns>Returns the user with the specified user ID.</returns>
        public static UserAccount GetUser(ContextIds ids)
        {
            return GetUser(ids.UserId);
        }

        /// <summary>Get user with the specified ID.</summary>
        /// <param name="id">A valid user ID.</param>
        /// <returns>Returns the user with the specified ID.</returns>
        public static UserAccount GetUser(ulong id)
        {
            // If the user is currently loaded, return it
            if (IsUserLoaded(id))
                return _dic[id];
            else
            {
                // Otherwise, check if the user exists in file storage. If it is, load it and return.
                if (_jsonStorage.DoesJsonExist(GetUserFilepath(id)))
                {
                    _dic.Add(id, _jsonStorage.RestoreObject<UserAccount>(GetUserFilepath(id)));
                }
                // Otherwise, if it does not exist in file storage, create it
                else
                {
                    UserAccount acc = CreateNewUser(id);
                }

                return _dic[id];
            }
        }

        /// <summary>Create a new user with the specified ID.</summary>
        /// <param name="id">A valid user ID.</param>
        /// <returns>Returns the user that was created.</returns>
        public static UserAccount CreateNewUser(ulong id)
        {
            Console.WriteLine($"Creating new user with ID: {id}");
            // Use ClientAccess to access important user data, such as their mention and username.
            var user = ClientAccess.GetUser(id);

            UserAccount acc = new UserAccount(true)
            {
                UserId = id,
                Name = user.Username,
                Mention = user.Mention
            };

            // Add the new user to storage in-memory and save them to the file storage.
            _dic.Add(id, acc);
            SaveUser(acc);

            return acc;
        }

        /// <summary>Update important values for a specified user.</summary>
        /// <param name="id">A valid user ID.</param>
        /// <param name="dm">The user's Direct Message Channel ID.</param>
        /// <param name="name">The user's name.</param>
        /// <param name="mention">The user's mention tag.</param>
        /// <param name="avatar">The user's avatar URL.</param>
        public static void UpdateUserInfo(ulong id, ulong dm, string name, string mention, string avatar)
        {
            var user = GetUser(id);
            user.DmId = dm;
            user.Name = name;
            user.Mention = mention;
            if (user.HasCharacter)
                user.Char.Mention = mention;
            user.AvatarUrl = avatar;
            SaveUsers();
        }

        /// <summary>Save all users to file storage that are currently loaded in the dictionary.</summary>
        public static void SaveUsers()
        {
            foreach (KeyValuePair<ulong, UserAccount> kvp in _dic)
            {
                SaveUser(kvp.Value);
            }
        }

        /// <summary>Save a specified user to file storage.</summary>
        /// <param name="acc">The user to save.</param>
        public static void SaveUser(UserAccount acc)
        {
            _jsonStorage.StoreObject(acc, GetUserFilepath(acc.UserId));
        }

        /// <summary>Checks if the user has a character.</summary>
        /// <param name="ids">The ContextIds that contain the user's id.</param>
        /// <exception cref="InvalidCharacterStateException">Throws InvalidCharacterStateException if the user does not have a character.</exception>
        public static void CharacterExists(ContextIds ids)
        {
            var user = GetUser(ids.UserId);
            if (!user.HasCharacter)
            {
                // Removing functionality of MessageHandler- message is now sent through the thrown exception in the catch statement.
                // await MessageHandler.CharacterMissing(ids);
                throw new InvalidCharacterStateException($"{user.Mention}, you do not have a character! You can create one using the \"startadventure\" command.");
            }
        }

        /// <summary>Checks if another user has a character from the perspective of a user.</summary>
        /// <param name="executingUser">The user who is executing a command on another user.</param>
        /// <param name="otherUser">The user who is being checked for a character.</param>
        /// <exception cref="InvalidCharacterStateException">Throws InvalidCharacterStateException if the other user does not have a character.</exception>
        public static void OtherCharacterExists(UserAccount executingUser, UserAccount otherUser)
        {
            if (!otherUser.HasCharacter)
            {
                // Removing functionality of MessageHandler- message is now sent through the thrown exception in the catch statement.
                // await MessageHandler.OtherCharacterMissing(ids);
                throw new InvalidCharacterStateException($"{executingUser.Mention}, that user does not have a character!");
            }
        }

        /// <summary>Checks whether the user's character is in a valid location.</summary>
        /// <param name="ids">The ContextIds that contain the user's id and guild ID.</param>
        /// <exception cref="InvalidCharacterStateException">Throws InvalidCharacterStateException if the user's character is in a different location.</exception>
        public static void ValidCharacterLocation(ContextIds ids)
        {
            var user = GetUser(ids.UserId);
            if (user.Char.CurrentGuildId != ids.GuildId)
            {
                // Removing functionality of MessageHandler- message is now sent through the thrown exception in the catch statement.
                //await MessageHandler.InvalidCharacterLocation(ids);
                throw new InvalidCharacterStateException($"{user.Mention} you must be in this location to use commands here! Your character is currently at {user.Char.CurrentGuildName}.");
            }
        }

        /// <summary>Checks if another user has a character in a valid location from the perspective of a user.</summary>
        /// <param name="ids">The ContextIds that contain the executing user's id and guild ID.</param>
        /// <param name="otherUser">The user who's character is being checked.</param>
        /// <exception cref="InvalidCharacterStateException">Throws InvalidCharacterStateException if the user's character is in a different location.</exception>
        public static void OtherCharacterLocation(ContextIds ids, UserAccount otherUser)
        {
            var user = GetUser(ids.UserId);
            if (otherUser.Char.CurrentGuildId != ids.GuildId)
            {
                // Removing functionality of MessageHandler- message is now sent through the thrown exception in the catch statement.
                //await MessageHandler.InvalidOtherCharacterLocation(ids, otherUser);
                throw new InvalidCharacterStateException($"{user.Mention} that player is not in this location! They are currently at {otherUser.Char.CurrentGuildName}.");
            }
        }

        /// <summary>Checks if the user's character is in combat.</summary>
        /// <param name="ids">The ContextIds that contain the executing user's id.</param>
        /// <exception cref="InvalidCharacterStateException">Throws InvalidCharacterStateException if the user's character is not in combat.</exception>
        public static void CharacterInCombat(ContextIds ids)
        {
            var user = GetUser(ids.UserId);
            if (!user.Char.InCombat)
            {
                //await MessageHandler.NotInCombat(ids);
                throw new InvalidCharacterStateException($"{user.Mention}, you are not in combat right now!");
            }
        }

        /// <summary>Checks if a user with the given ID exists.</summary>
        /// <param name="id">A valid User ID.</param>
        public static bool IsUserLoaded(ulong id)
        {
            return _dic.ContainsKey(id);
        }

        /// <summary>Gets the filepath for a specified user.</summary>
        /// <param name="id">A valid user ID.</param>
        /// <returns>Returns the filepath for the user.</returns>
        public static string GetUserFilepath(ulong id)
        {
            return $"{filepath}\\{id}\\{id}";
        }
    }
}