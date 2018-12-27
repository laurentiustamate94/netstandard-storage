using Plugin.NetStandardStorage.Abstractions.Interfaces;
using Plugin.NetStandardStorage.Implementations;
using System;

namespace Plugin.NetStandardStorage
{
    public class FileSystem : Implementations.FileSystem
    {
        public static void Init()
        {
            CrossStorage.Init<FileSystem>();
        }

        public override IFolder LocalStorage
        {
            get
            {
                var localStorage = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                return new Folder(localStorage, canDelete: false);
            }
        }

        public override IFolder RoamingStorage
            => throw new PlatformNotSupportedException("The Roaming folder is not supported in Android !");
    }
}
