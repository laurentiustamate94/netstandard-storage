using System;
using Plugin.NetStandardStorage.Abstractions.Interfaces;
using Plugin.NetStandardStorage.Implementations;

namespace Plugin.NetStandardStorage
{
    public static class CrossStorage
    {
        private static readonly Lazy<IFileSystem> fileSystem
            = new Lazy<IFileSystem>(CreateFileSystem, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static IFileSystem FileSystem
            => fileSystem.Value;

        private static IFileSystem CreateFileSystem()
        {
            return new FileSystem();
        }
    }
}
