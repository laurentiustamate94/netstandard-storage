using System;
using Plugin.NetStandardStorage.Abstractions.Interfaces;
using Plugin.NetStandardStorage.Implementations;

namespace Plugin.NetStandardStorage
{
    public sealed class CrossStorage
    {
        private static Lazy<IFileSystem> _fileSystem
            = new Lazy<IFileSystem>(() => CreateFileSystem(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static IFileSystem FileSystem
        {
            get
            {
                return _fileSystem.Value;
            }
        }

        private static IFileSystem CreateFileSystem()
        {
#if !PORTABLE
            return new FileSystem();
#endif
            throw new PlatformNotSupportedException();
        }

        private CrossStorage()
        {
        }
    }
}
