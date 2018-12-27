using Plugin.NetStandardStorage.Abstractions.Interfaces;
using Plugin.NetStandardStorage.Implementations;

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
                return new Folder(Windows.Storage.ApplicationData.Current.LocalFolder.Path, canDelete: false);
            }
        }

        public override IFolder RoamingStorage
        {
            get
            {
                return new Folder(Windows.Storage.ApplicationData.Current.RoamingFolder.Path, canDelete: false);
            }
        }
    }
}
