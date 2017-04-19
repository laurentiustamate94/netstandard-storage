using System;
using Plugin.NetStandardStorage.Abstractions.Interfaces;
using Plugin.NetStandardStorage.Implementations;

namespace Plugin.NetStandardStorage
{
    public sealed class NetStandardStorage
    {
        private static Lazy<IFileSystem> _fileSystem
            = new Lazy<IFileSystem>(() => new FileSystem(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static IFileSystem FileSystem
        {
            get
            {
                return _fileSystem.Value;
            }
        }

        private NetStandardStorage()
        {

        }
    }
}
