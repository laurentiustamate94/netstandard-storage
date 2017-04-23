using System;
using System.IO;
using Plugin.NetStandardStorage.Abstractions.Interfaces;
using Windows.Storage;

namespace Plugin.NetStandardStorage.Implementations
{
    public class FileSystem : IFileSystem
    {
        public IFolder LocalStorage
        {
            get
            {
                return new Folder(ApplicationData.Current.LocalFolder.Path, canDelete: false);
            }
        }

        public IFolder RoamingStorage
        {
            get
            {
                return new Folder(ApplicationData.Current.RoamingFolder.Path, canDelete: false);
            }
        }

        public IFile GetFileFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new FormatException("Null or emtpy *path* argument not allowed !");
            }

            if (System.IO.File.Exists(path))
            {
                return new File(path);
            }

            return null;
        }

        public IFolder GetFolderFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new FormatException("Null or emtpy *path* argument not allowed !");
            }

            if (Directory.Exists(path))
            {
                return new Folder(path);
            }

            return null;
        }
    }
}
