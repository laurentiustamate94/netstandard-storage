using System;
using System.Collections.Generic;
using System.IO;
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
    public class File : IFile
    {
        private string _name;
        private string _fullPath;

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

        public File(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                throw new FormatException("Null or emtpy *fullPath* argument not allowed !");
            }

            this._fullPath = fullPath;
            this._name = Path.GetFileName(fullPath);
        }

        public void Delete()
        {
            if (!System.IO.File.Exists(this.FullPath))
            {
                throw new FileNotFoundException("The file was not found at the specified path: " + this.FullPath);
            }

            Task.Run(() => this.DeleteAsync())
                .Wait();
        }

        private async Task DeleteAsync()
        {
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(this.FullPath);

            await file.DeleteAsync();
        }

        public void Move(string newPath, NameCollisionOption option = NameCollisionOption.ReplaceExisting)
        {
            if (string.IsNullOrEmpty(newPath))
            {
                throw new FormatException("Null or emtpy *newPath* argument not allowed !");
            }

            var newDirectory = Path.GetDirectoryName(newPath);
            var newName = Path.GetFileName(newPath);

            var counter = 1;
            var candidateName = newName;

            while (true)
            {
                if (counter > 1)
                {
                    candidateName = String.Format(
                        "{0} ({1}){2}",
                        Path.GetFileNameWithoutExtension(newName),
                        counter,
                        Path.GetExtension(newName));
                }

                var candidatePath = Path.Combine(newDirectory, candidateName);

                var existsFile = System.IO.File.Exists(candidatePath);
                var existsDirectory = Directory.Exists(candidatePath);

                if (existsFile || existsDirectory)
                {
                    if (option == NameCollisionOption.GenerateUniqueName)
                    {
                        continue;
                    }
                    else if (option == NameCollisionOption.ReplaceExisting)
                    {
                        if (existsDirectory)
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
                    else if (option == NameCollisionOption.FailIfExists)
                    {
                        if (existsDirectory)
                        {
                            throw new IOException("Folder already exists with same name at path: " + candidatePath);
                        }
                        else
                        {
                            throw new IOException("File already exists at path: " + candidatePath);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Incorrect NameCollisionOption value: " + option);
                    }
                }

                Task.Run(() => MoveAsync(newPath, candidateName))
                    .Wait();

                this._fullPath = candidatePath;
                this._name = candidateName;

                break;
            }
        }

        private async Task MoveAsync(string newPath, string candidateName)
        {
            var folderPath = Path.GetDirectoryName(newPath);
            var folder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(folderPath);
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(this.FullPath);

            await file.MoveAsync(folder, candidateName);
        }

        public Stream Open(FileAccess fileAccess)
        {
            var task = Task.Run(() => OpenAsync(fileAccess));
            
            task.Wait();

            return task.Result;
        }

        private async Task<Stream> OpenAsync(FileAccess fileAccess)
        {
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(this.FullPath);

            if (fileAccess == FileAccess.Read)
            {
                return await file.OpenStreamForReadAsync();
            }
            else if (fileAccess == FileAccess.Write)
            {
                return await file.OpenStreamForWriteAsync();
            }
            else if (fileAccess == FileAccess.ReadWrite)
            {
                var randomAccessStream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                return randomAccessStream.AsStream();
            }

            throw new ArgumentException("Incorrect FileAccess value: " + fileAccess);
        }

        public void Rename(string newName, NameCollisionOption option = NameCollisionOption.FailIfExists)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new FormatException("Null or emtpy *newName* argument not allowed !");
            }

            var newPath = Path.Combine(Path.GetDirectoryName(this.FullPath), newName);

            Move(newPath, option);
        }

        public void WriteAllBytes(byte[] bytes)
        {
            Task.Run(() => WriteAllBytesAsync(bytes))
                .Wait();
        }

        private async Task WriteAllBytesAsync(byte[] bytes)
        {
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(this.FullPath);

            await Windows.Storage.FileIO.WriteBytesAsync(file, bytes);
        }

        public void WriteAllLines(IEnumerable<string> contents)
        {
            Task.Run(() => WriteAllLinesAsync(contents))
                .Wait();
        }

        private async Task WriteAllLinesAsync(IEnumerable<string> contents)
        {
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(this.FullPath);

            await Windows.Storage.FileIO.WriteLinesAsync(file, contents);
        }

        public void WriteAllText(string contents)
        {
            Task.Run(() => WriteAllTextAsync(contents))
                .Wait();
        }

        private async Task WriteAllTextAsync(string contents)
        {
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(this.FullPath);

            await Windows.Storage.FileIO.WriteTextAsync(file, contents);
        }
    }
}
