namespace ProjectOrigin
{
    /// <summary>Interface for creating a data storage system with methods to store and retrieve data</summary>
    public interface IDataStorage
    {
        void StoreObject(object obj, string key);

        T RestoreObject<T>(string key);
    }
}