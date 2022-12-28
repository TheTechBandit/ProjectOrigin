using System;
using System.Collections.Generic;

namespace ProjectOrigin
{
    /// <summary>Implementation of IDataStorage for storing items in-memory using dictionaries.</summary>
    public class InMemoryStorage : IDataStorage
    {
        /// <summary>Dictionary used for storing data.</summary>
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        /// <summary>Copies all objects-key pairs from a given dictionary.</summary>
        public void StoreAllObjects(Dictionary<string, object> dict)
        {
            foreach (KeyValuePair<string, object> entry in dict)
            {
                StoreObject(entry.Value, entry.Key);
            }
        }

        /// <summary>Stores a given object with a given key.</summary>
        public void StoreObject(object obj, string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = obj;
                return;
            }

            _dictionary.Add(key, obj);
        }

        /// <summary>Return the object associated with the given key.</summary>
        public T RestoreObject<T>(string key)
        {
            if (!_dictionary.ContainsKey(key))
                throw new ArgumentException($"The provided key '{key}' wasn't found.");
            return (T)(_dictionary[key]);
        }

        /// <summary>Returns the amount of objects in storage.</summary>
        public int StorageLength()
        {
            return _dictionary.Count;
        }

        /// <summary>Returns the dictionary being used.</summary>
        public Dictionary<string, object> GetDict()
        {
            return _dictionary;
        }
    }
}