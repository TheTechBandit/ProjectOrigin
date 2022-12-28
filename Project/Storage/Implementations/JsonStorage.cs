using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.IO.Directory;

namespace ProjectOrigin
{
    /// <summary>Implementation of IDataStorage to serialize objects into .json files and deserialize them back into objects.</summary>
    public class JsonStorage : IDataStorage
    {
        /// <summary>Restores an object at the given filepath by deserializing it and then returns it.</summary>
        public T RestoreObject<T>(string key)
        {
            var file = $"{key}.json";
            if (!DoesJsonExist(key))
            {
                CreateNewFileDirectory(file);
            }

            var json = File.ReadAllText(file);

            return (T)JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        /// <summary>Stores a given object into the given directory by serializing it.</summary>
        public void StoreObject(object obj, string key)
        {
            var file = $"{key}.json";
            if (!DoesJsonExist(key))
            {
                CreateNewFileDirectory(file);
            }

            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            json = JValue.Parse(json).ToString(Formatting.Indented);
            File.WriteAllText(file, json);
        }

        public void CreateNewFileDirectory(string file)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            using (var stream = File.Create(file))
            {
                //Empty filestream
            }
        }

        public bool DoesJsonExist(string path)
        {
            return File.Exists($"{path}.json");
        }
    }
}