using System;
using System.Collections.Generic;

namespace ProjectOrigin
{
    /// <summary>Class for handling town creation, loading and saving,</summary>
    public static class TownHandler
    {
        /// <summary>The filepath where towns are stored.</summary>
        public static readonly string filepath;
        /// <summary>A dictionary filled with TownAccounts using their GuildID as a key.</summary>
        private static Dictionary<ulong, TownAccount> _dic;
        /// <summary>JsonStorage object for saving the towns to memory.</summary>
        private static JsonStorage _jsonStorage;

        /// <summary>Static constructor for TownHandler that loads all towns as soon as TownHandler is accessed.</summary>
        static TownHandler()
        {
            //Access JsonStorage to load town list into memory
            filepath = "Project\\Data\\TownData\\Towns";

            _dic = new Dictionary<ulong, TownAccount>();
            _jsonStorage = new JsonStorage();
        }

        /// <summary>Gets the town with the corresponding GuildID from a ContextId.</summary>
        /// <param name="ids">ContextIds that contains a valid guild ID.</param>
        public static TownAccount GetTown(ContextIds ids)
        {
            return GetTown(ids.GuildId);
        }

        /// <summary>Gets the town with the corresponding GuildID.</summary>
        /// <param name="id">ID of a guild</param>
        public static TownAccount GetTown(ulong id)
        {
            //If the town is currently loaded, return it
            if (IsTownLoaded(id))
                return _dic[id];
            else
            {
                //Otherwise, check if the town exists in memory. If it is, load it and return.
                if(_jsonStorage.DoesJsonExist(GetTownFilepath(id)))
                {
                    _dic.Add(id, _jsonStorage.RestoreObject<TownAccount>(GetTownFilepath(id)));
                }
                //Otherwise, if it does not exist in memory, create it and store it.
                else
                {
                    TownAccount acc = CreateNewTown(id);
                }

                return _dic[id];
            }
        }

        /// <summary>Creates a new town with the given Guild ID.</summary>
        /// <param name="id">A valid Guild ID.</param>
        public static TownAccount CreateNewTown(ulong id)
        {
            Console.WriteLine($"Creating new town with ID: {id}");

            TownAccount acc = new TownAccount(true)
            {
                GuildId = id
            };
            _dic.Add(id, acc);
            SaveTown(acc);
            return acc;
        }

        /// <summary>Saves the list of towns to memory.</summary>
        public static void SaveTowns()
        {
            foreach(KeyValuePair<ulong, TownAccount> kvp in _dic)
            {
                SaveTown(kvp.Value);
            }
        }

        /// <summary>Saves a single town to memory.</summary>
        public static void SaveTown(TownAccount acc)
        {
            _jsonStorage.StoreObject(acc, GetTownFilepath(acc.GuildId));
        }

        /// <summary>Checks if a town with the given ID exists.</summary>
        /// <param name="id">A valid Guild ID.</param>
        public static bool IsTownLoaded(ulong id)
        {
            return _dic.ContainsKey(id);
        }

        /// <summary>Gets the filepath for a particular town.</summary>
        /// <param name="id">The Guild ID for the town.</param>
        /// <returns>Returns the filepath for the town's data.</returns>
        public static string GetTownFilepath(ulong id)
        {
            return $"{filepath}\\{id}\\{id}";
        }

    }
}