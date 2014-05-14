namespace Microsoft.Dldw.BuffaloWings.Common.ApplicationCache
{
    public class CacheModel
    {
        
        public Duration CacheDuration { get; private set; }

        public ExpirationPolicy CacheExpirationPolicy { get; private set; }

        public CacheModel(Duration duration, ExpirationPolicy policy = ExpirationPolicy.Absolute)
        {
            CacheDuration = duration;
            CacheExpirationPolicy = policy;
        }


        public enum ExpirationPolicy
        {

            /// <summary>
            /// 
            /// </summary>
            Absolute = 0,

            /// <summary>
            /// 
            /// </summary>
            Sliding = 1
        }
    }
}
