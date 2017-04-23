using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Plugin.NetStandardStorage.Abstractions.Interfaces;
using Plugin.NetStandardStorage.Abstractions.Types;

namespace Plugin.NetStandardStorage.Implementations
{
    /// <summary>
    /// TODO Will need to be removed after netstandard2.0
    /// 
    /// Apparently does not fully support System.IO for the netstandard1.4
    /// </summary>
    public class Folder : IFolder
    {
        private string _name;
        private string _fullPath;
        private bool _canDelete;

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public string FullPath
        {
            get
            {
                return this._fullPath;
            }
        }

        public Folder(string fullPath, bool canDelete = true)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new FormatException("Null or emtpy *fullPath* argument not allowed !");
            }

            this._fullPath = fullPath;
            this._name = Path.GetFileName(fullPath);

            this._canDelete = canDelete;
        }

        public IFile CreateFile(string name, CreationCollisionOption option)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FormatException("Null or emtpy *name* argument not allowed !");
            }

            var counter = 1;
            var candidateName = name;

            while (true)
            {
                if (counter > 1)
                {
                    candidateName = String.Format(
                        "{0} ({1}){2}",
                        Path.GetFileNameWithoutExtension(name),
                        counter,
                        Path.GetExtension(name));
                }

                var candidatePath = Path.Combine(this.FullPath, candidateName);

                var existsFile = this.CheckFileExists(candidatePath);
                var existsFolder = this.CheckFolderExists(candidatePath);

                if (existsFolder || existsFile)
                {
                    if (option == CreationCollisionOption.GenerateUniqueName)
                    {
                        continue;
                    }
                    else if (option == CreationCollisionOption.ReplaceExisting)
                    {
                        if (existsFile)
                        {
                            new File(candidatePath)
                                .Delete();
                        }
                        else
                        {
                            new Folder(candidatePath)
                                .Delete();
                        }
                    }
                    else if (option == CreationCollisionOption.FailIfExists)
                    {
                        if (existsFolder)
                        {
                            throw new IOException("Folder already exists with same name at path: " + candidatePath);
                        }
                        else
                        {
                            throw new IOException("File already exists at path: " + candidatePath);
                        }
                    }
                    else if (option == CreationCollisionOption.OpenIfExists)
                    {
                        return new File(candidatePath);
                    }
                    else
                    {
                        throw new ArgumentException("Incorrect CreationCollisionOption value: " + option);
                    }
                }

                Task.Run(() => this.CreateFileAsync(candidateName))
                    .Wait();

                return new File(candidatePath);
            }
        }

        private async Task CreateFileAsync(string candidateName)
        {
            var folder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(this.FullPath);

            await folder.CreateFileAsync(candidateName);
        }

        public IFile GetFile(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FormatException("Null or emtpy *name* argument not allowed !");
            }

            var path = Path.Combine(this.FullPath, name);

            if (!System.IO.File.Exists(path))
            {
                throw new FileNotFoundException("The file was not found at the specified path: " + path);
            }

            return new File(path);
        }

        public IList<IFile> GetFiles()
        {
            return Directory.GetFiles(this.FullPath)
                .Select(path => new File(path))
                .ToList<IFile>()
                .AsReadOnly();
        }

        public IFolder CreateFolder(string name, CreationCollisionOption option)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FormatException("Null or emtpy *name* argument not allowed !");
            }

            var counter = 1;
            var candidateName = name;

            while (true)
            {
                if (counter > 1)
                {
                    candidateName = String.Format(
                        "{0} ({1})",
                        name,
                        counter);
                }

                var candidatePath = Path.Combine(this.FullPath, candidateName);

                var existsFile = this.CheckFileExists(candidatePath);
                var existsFolder = this.CheckFolderExists(candidatePath);

                if (existsFolder || existsFile)
                {
                    if (option == CreationCollisionOption.GenerateUniqueName)
                    {
                        continue;
                    }
                    else if (option == CreationCollisionOption.ReplaceExisting)
                    {
                        if (existsFile)
                        {
                            new File(candidatePath)
                                .Delete();
                        }
                        else
                        {
                            new Folder(candidatePath)
                                .Delete();
                        }
                    }
                    else if (option == CreationCollisionOption.FailIfExists)
                    {
                        if (existsFile)
                        {
                            throw new IOException("File already exists with same name at path: " + candidatePath);
                        }
                        else
                        {
                            throw new IOException("Folder already exists at path: " + candidatePath);
                        }
                    }
                    else if (option == CreationCollisionOption.OpenIfExists)
                    {
                        return new Folder(candidatePath);
                    }
                    else
                    {
                        throw new ArgumentException("Incorrect CreationCollisionOption value: " + option);
                    }
                }

                Task.Run(() => this.CreateFolderAsync(candidateName))
                    .Wait();

                return new Folder(candidatePath);
            }
        }

        private async Task CreateFolderAsync(string candidateName)
        {
            var folder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(this.FullPath);

            await folder.CreateFolderAsync(candidateName);
        }

        public IFolder GetFolder(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FormatException("Null or emtpy *name* argument not allowed !");
            }

            var path = Path.Combine(this.FullPath, name);

            if (!CheckFolderExists(path))
            {
                throw new DirectoryNotFoundException("The folder was not found at the specified path: " + path);
            }

            return new Folder(path);
        }

        public IList<IFolder> GetFolders()
        {
            return Directory.GetDirectories(this.FullPath)
                .Select(path => new Folder(path))
                .ToList<IFolder>()
                .AsReadOnly();
        }

        public bool CheckFileExists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public bool CheckFolderExists(string path)
        {
            return Directory.Exists(path);
        }

        public void Delete()
        {
            if (!this._canDelete)
            {
                throw new IOException("The root folder can't be deleted !");
            }

            if (CheckFolderExists(this.FullPath))
            {
                Task.Run(() => DeleteAsync())
                    .Wait();
            }
        }

        private async Task DeleteAsync()
        {
            var file = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(this.FullPath);

            await file.DeleteAsync();
        }
    }
}
