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
                var tempPath = Path.GetTempPath();

                if (IsAndroidStorage(tempPath))
                {
                    return GetAndroidFolder(tempPath, "files");
                }
                else if (IsIosStorage(tempPath))
                {
                    return GetIosFolder(tempPath, "Library");
                }
                else if (IsUwpStorage(tempPath))
                {
                    return GetUwpFolder(tempPath, "LocalState");
                }

                throw new PlatformNotSupportedException();
            }
        }

        public IFolder RoamingStorage
        {
            get
            {
                var tempPath = Path.GetTempPath();

                if (IsAndroidStorage(tempPath))
                {
                    return null;
                }
                else if (IsIosStorage(tempPath))
                {
                    return null;
                }
                else if (IsUwpStorage(tempPath))
                {
                    return GetUwpFolder(tempPath, "RoamingState");
                }

                throw new PlatformNotSupportedException();
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

        #region ANDROID

        private bool IsAndroidStorage(string tempPath)
        {
            return tempPath.Contains("/data/user");
        }

        private IFolder GetAndroidFolder(string tempPath, string name)
        {
            var path = tempPath.Replace("/cache/", "");
            path += string.Format("/{0}", name);

            return new Folder(path, canDelete: false);
        }

        #endregion

        #region IOS

        private bool IsIosStorage(string tempPath)
        {
            return tempPath.Contains("Data/Application");
        }

        private IFolder GetIosFolder(string tempPath, string name)
        {
            var path = tempPath.Replace("/tmp/", "");
            path += string.Format("/{0}", name);

            return new Folder(path, canDelete: false);
        }

        #endregion

        #region UWP

        private bool IsUwpStorage(string tempPath)
        {
            return tempPath.Contains(@"AppData\Local\Packages");
        }

        private IFolder GetUwpFolder(string tempPath, string name)
        {
            var path = tempPath.Replace(@"\AC\Temp\", "");
            path += string.Format(@"\{0}", name);

            return new Folder(path, canDelete: false);
        }

        #endregion
    }
}
