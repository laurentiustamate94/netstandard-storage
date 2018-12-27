using System;

namespace Plugin.NetStandardStorage
{
    public class NotIInitializedException : Exception
    {
        public NotIInitializedException()
        : base("Plugin.NetStandardStorage has no initialized yet. Please call CrossStorage.Init() first.")
        {

        }
    }
}
