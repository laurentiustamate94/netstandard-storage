using System;
using System.IO;
using Plugin.NetStandardStorage.Abstractions.Interfaces;

namespace Plugin.NetStandardStorage.Implementations
{
    public class FileSystem : IFileSystem
    {
        public IFolder LocalStorage
        {
            get
            {
                var localStorage = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                return new Folder(localStorage, canDelete: false);
            }
        }

        public IFolder RoamingStorage
        {
            get
            {
                throw new PlatformNotSupportedException("The Roaming folder is not supported in Android !");
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
