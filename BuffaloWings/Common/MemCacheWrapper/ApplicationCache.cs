namespace Microsoft.Dldw.BuffaloWings.Common.ApplicationCache
{
    using System;
    using System.Globalization;
    using System.Runtime.Caching;


    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public sealed class ApplicationCache : IAppCache
    {
        private string Region { get; set; }

        private readonly MemoryCache memoryCache;

        private bool cacheDisposed;

        public ApplicationCache( string region )
        {
            if (string.IsNullOrWhiteSpace(region))
                throw new ArgumentNullException(region);

            Region = region;

            this.memoryCache = new MemoryCache(region);
        }

        #region IAppCache Implementation

        /// <summary>
        /// It will be used to get element from the cache
        /// </summary>
        /// <param name="key"> Key that points to the object in the cache </param>
        /// <returns> Returns the object pointing by the key</returns>
        public object Get(string key)
        {
            var cacheKey = BuildCacheKey(key, Region);

            var value = this.memoryCache.Get(cacheKey);

            return value;
        }

        /// <summary>
        /// It will be used to set the objects in cache
        /// </summary>
        /// <param name="key"> Key that points to the object in the cache </param>
        /// <param name="value"> value to be stored</param>
        /// <param name="cacheModel">Cache configurations (Duration, time duration for cache)</param>
        public void Set(string key, object value, CacheModel cacheModel)
        {
            string cacheKey = BuildCacheKey(key, Region);

            DateInterval unit = cacheModel.CacheDuration.Unit;

            long duration = cacheModel.CacheDuration.Value * Duration.UnitToScale(unit);

            duration = duration / Duration.UnitToScale(DateInterval.Second);

            var cachePolicy = new CacheItemPolicy();

            if (cacheModel.CacheExpirationPolicy == CacheModel.ExpirationPolicy.Absolute)
                cachePolicy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(duration));

            else if (cacheModel.CacheExpirationPolicy == CacheModel.ExpirationPolicy.Sliding)
                
                cachePolicy.SlidingExpiration = new TimeSpan(0, 0, (int)duration);

            this.memoryCache.Set(cacheKey, value, cachePolicy);

        }

        /// <summary>
        /// It will be used to remove element from the cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Remove(string key)
        {
            string cacheKey = BuildCacheKey(key, Region);

            var value = this.memoryCache.Remove(cacheKey);

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            var cacheKey = BuildCacheKey(key, Region);
            return this.memoryCache.Contains(cacheKey);
        }

        public void Uninitailze()
        {
            if( cacheDisposed || this.memoryCache == null )
            {
                return;
            }
            this.memoryCache.Dispose();
            this.cacheDisposed = true;
        }

        #endregion 
        /// <summary>
        /// It will generate a Unique key based on the region
        /// </summary>
        /// <param name="key"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        private string BuildCacheKey(string key, string region)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(key);


            return string.Format(CultureInfo.InvariantCulture, "{0}://{1}", region, key);
        }

        ~ApplicationCache()
        {
            Uninitailze();
        }

    }

}
