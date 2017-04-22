using System;
using System.Collections.Generic;
using System.IO;
using Plugin.NetStandardStorage.Abstractions.Interfaces;
using Plugin.NetStandardStorage.Abstractions.Types;

namespace Plugin.NetStandardStorage.Implementations
{
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

            System.IO.File.Delete(this.FullPath);
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

                if (System.IO.File.Exists(candidatePath))
                {
                    if (option == NameCollisionOption.GenerateUniqueName)
                    {
                        continue;
                    }
                    else if (option == NameCollisionOption.ReplaceExisting)
                    {
                        System.IO.File.Delete(candidatePath);
                    }
                    else if (option == NameCollisionOption.FailIfExists)
                    {
                        throw new IOException("File already exists at path: " + candidatePath);
                    }
                    else
                    {
                        throw new ArgumentException("Incorrect NameCollisionOption value: " + option);
                    }
                }

                System.IO.File.Move(this.FullPath, candidatePath);

                this._fullPath = candidatePath;
                this._name = candidateName;

                break;
            }
        }

        public Stream Open(FileAccess fileAccess)
        {
            if (fileAccess == FileAccess.Read)
            {
                return System.IO.File.OpenRead(this.FullPath);
            }
            else if (fileAccess == FileAccess.Write)
            {
                return System.IO.File.OpenWrite(this.FullPath);
            }
            else if (fileAccess == FileAccess.ReadWrite)
            {
                return System.IO.File.Open(this.FullPath, FileMode.Open, FileAccess.ReadWrite);
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
            System.IO.File.WriteAllBytes(this.FullPath, bytes);
        }

        public void WriteAllLines(IEnumerable<string> contents)
        {
            System.IO.File.WriteAllLines(this.FullPath, contents);
        }

        public void WriteAllText(string contents)
        {
            System.IO.File.WriteAllText(this.FullPath, contents);
        }
    }
}
