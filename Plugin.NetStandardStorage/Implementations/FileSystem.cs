using System;
using System.IO;
using Plugin.NetStandardStorage.Abstractions.Interfaces;

namespace Plugin.NetStandardStorage.Implementations
{
    public class FileSystem : IFileSystem
    {
        public virtual IFolder LocalStorage
        {
            get
            {
                var tempPath = Path.GetTempPath();

                if (IsAndroidStorage(tempPath))
                {
                    // TODO:
                    //var localStorage = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    //return new Folder(localStorage, canDelete: false);
                    return GetAndroidFolder(tempPath, "files");
                }
                else if (IsIosStorage(tempPath))
                {
                    // TODO:
                    //var localStorage = Foundation.NSFileManager.DefaultManager.GetUrls(Foundation.NSSearchPathDirectory.LibraryDirectory, Foundation.NSSearchPathDomain.User)[0];
                    //return new Folder(localStorage.Path, canDelete: false);
                    return GetIosFolder(tempPath, "Library");
                }
                else if (IsUwpStorage(tempPath))
                {
                    // TODO:
                    //return new Folder(Windows.Storage.ApplicationData.Current.LocalFolder.Path, canDelete: false);
                    return GetUwpFolder(tempPath, "LocalState");
                }

                throw new PlatformNotSupportedException();
            }
        }

        public virtual IFolder RoamingStorage
        {
            get
            {
                var tempPath = Path.GetTempPath();

                if (IsAndroidStorage(tempPath))
                {
                    return null;
                    // TODO:
                    //throw new System.PlatformNotSupportedException("The Roaming folder is not supported in Android !");
                }
                else if (IsIosStorage(tempPath))
                {
                    return null;
                    // TODO:
                    //throw new System.PlatformNotSupportedException("The Roaming folder is not supported in iOS !");
                }
                else if (IsUwpStorage(tempPath))
                {
                    return GetUwpFolder(tempPath, "RoamingState");
                    // TODO:
                    //return new Folder(Windows.Storage.ApplicationData.Current.RoamingFolder.Path, canDelete: false);
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
            path += $"/{name}";

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
            path += $"/{name}";

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
            path += $@"\{name}";

            return new Folder(path, canDelete: false);
        }

        #endregion
    }
}
