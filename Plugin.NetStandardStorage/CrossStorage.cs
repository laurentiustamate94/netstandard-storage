using Plugin.NetStandardStorage.Abstractions.Interfaces;
using System;

namespace Plugin.NetStandardStorage
{
    public static class CrossStorage
    {
        private static Lazy<IFileSystem> fileSystem;

        public static IFileSystem FileSystem
            => fileSystem?.Value ?? throw new NotIInitializedException();


        public static void Init<T>()
            where T : IFileSystem, new()
        {
            if (fileSystem == null)
            {
                fileSystem = new Lazy<IFileSystem>(() => new T(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
            }
        }
    }
}
