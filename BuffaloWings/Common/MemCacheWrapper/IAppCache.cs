namespace Microsoft.Dldw.BuffaloWings.Common.ApplicationCache
{
    public interface IAppCache
    {
        /// <summary>
        /// Gets an object from the cache using the specified key.
        /// </summary>
        /// <param name="key">The unique Value that is used to identify the object in the cache.</param>
        /// <returns>The object that was cached by using the specified key. Null is returned if the key does not exist.</returns>
        object Get(string key);

        /// <summary>
        /// Adds or replaces an object in the specified region and namespace.
        /// </summary>
        /// <param name="key">The unique Value that is used to identify the object in the region.</param>
        /// <param name="value">The object to add or replace.</param>
        /// <param name="cacheModel">The cache configuration to cache this object.</param>
        void Set(string key, object value, CacheModel cacheModel);

        /// <summary>
        /// Removes an object from the cache in a region.
        /// </summary>
        /// <param name="key">The unique Value that is used to identify the object in the region and namespace.</param>
        /// <returns>True if the object is removed; otherwise, false.</returns>
        object Remove(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsKey(string key);

        /// <summary>
        /// It should be called at the time of releasing resources
        /// </summary>
        void Uninitailze();
    }
}
