using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Plugin.NetStandardStorage.Abstractions.Interfaces;
using Plugin.NetStandardStorage.Abstractions.Types;

namespace Plugin.NetStandardStorage.Implementations
{
    public class Folder : IFolder
    {
        private readonly bool canDelete;

        public string Name { get; }

        public string FullPath { get; }

        public Folder(string fullPath, bool canDelete = true)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new FormatException("Null or emtpy *fullPath* argument not allowed !");
            }

            FullPath = fullPath;
            Name = Path.GetFileName(fullPath);

            this.canDelete = canDelete;
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

                var candidatePath = Path.Combine(FullPath, candidateName);

                var existsFile = CheckFileExists(candidatePath);
                var existsFolder = CheckFolderExists(candidatePath);

                if (existsFile || existsFolder)
                {
                    if (option == CreationCollisionOption.GenerateUniqueName)
                    {
                        continue;
                    }
                    else if (option == CreationCollisionOption.ReplaceExisting)
                    {
                        if (existsFolder)
                        {
                            new Folder(candidatePath)
                                .Delete();
                        }
                        else
                        {
                            new File(candidatePath)
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

                System.IO.File.Create(candidatePath)
                    .Dispose();

                return new File(candidatePath);
            }
        }

        public IFile GetFile(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FormatException("Null or emtpy *name* argument not allowed !");
            }

            var path = Path.Combine(FullPath, name);

            if (!CheckFileExists(path))
            {
                throw new FileNotFoundException("The file was not found at the specified path: " + path);
            }

            return new File(path);
        }

        public IList<IFile> GetFiles()
        {
            return Directory.GetFiles(FullPath)
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
                    candidateName = $"{name} ({counter})";
                }

                var candidatePath = Path.Combine(FullPath, candidateName);

                var existsFile = CheckFileExists(candidatePath);
                var existsFolder = CheckFolderExists(candidatePath);

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

                Directory.CreateDirectory(candidatePath);

                return new Folder(candidatePath);
            }
        }

        public IFolder GetFolder(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new FormatException("Null or emtpy *name* argument not allowed !");
            }

            var path = Path.Combine(FullPath, name);

            if (!CheckFolderExists(path))
            {
                throw new DirectoryNotFoundException("The folder was not found at the specified path: " + path);
            }

            return new Folder(path);
        }

        public IList<IFolder> GetFolders()
        {
            return Directory.GetDirectories(FullPath)
                .Select(path => new Folder(path))
                .ToList<IFolder>()
                .AsReadOnly();
        }

        public bool CheckFileExists(string fileName)
        {
            var path = Path.Combine(FullPath, fileName);

            return System.IO.File.Exists(path);
        }

        public bool CheckFolderExists(string folderName)
        {
            var path = Path.Combine(FullPath, folderName);

            return Directory.Exists(path);
        }

        public void Delete()
        {
            if (!canDelete)
            {
                throw new IOException("The root folder can't be deleted !");
            }

            if (CheckFolderExists(FullPath))
            {
                Directory.Delete(FullPath);
            }
        }

        public void DeleteRecursively()
        {
            if (!canDelete)
            {
                throw new IOException("The root folder can't be deleted !");
            }

            if (CheckFolderExists(FullPath))
            {
                Directory.Delete(FullPath, recursive: true);
            }
        }
    }
}
