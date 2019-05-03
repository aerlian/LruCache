using System;

namespace Main
{
    public class LruCacheException : Exception
    {
        public LruCacheException(string message) : base(message)
        {
        }
    }
}
