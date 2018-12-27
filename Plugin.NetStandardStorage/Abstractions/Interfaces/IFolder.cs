using System.Collections.Generic;
using Plugin.NetStandardStorage.Abstractions.Types;

namespace Plugin.NetStandardStorage.Abstractions.Interfaces
{
    /// <summary>
    /// Manages folders and their contents and provides information about them.
    /// </summary>
    public interface IFolder
    {
        /// <summary>
        /// The name of the folder.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The full path of the folder.
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Creates a new file with the specified name in the current folder.
        /// </summary>
        /// <param name="name">The name of the new file to create in the current folder.</param>
        /// <param name="option">One of the enumeration values that determines how to
        /// handle the collision if a file with the specified *name* 
        /// already exists in the current folder.</param>
        /// <returns>When this method completes, it returns a File</returns>
        IFile CreateFile(string name, CreationCollisionOption option);

        /// <summary>
        /// Gets the file with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <returns>When this method completes, it returns a File</returns>
        IFile GetFile(string name);

        /// <summary>
        /// Gets the files in the current folder.
        /// </summary>
        /// <returns>When this method completes, it returns a list of Files</returns>
        IList<IFile> GetFiles();

        /// <summary>
        /// Determines whether the given file exists in the current folder.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <returns>true if *name* refers to an existing file; 
        /// false if the file does not exist or an error occurs 
        /// when trying to determine if the specified file exists.</returns>
        bool CheckFileExists(string name);

        /// <summary>
        /// Creates a new folder with the specified name in the current folder.
        /// </summary>
        /// <param name="name">The name of the new folder to create in the current folder.</param>
        /// <param name="option">One of the enumeration values that determines how to
        /// handle the collision if a folder with the specified *name* 
        /// already exists in the current folder.</param>
        /// <returns>When this method completes, it returns a Folder</returns>
        IFolder CreateFolder(string name, CreationCollisionOption option);

        /// <summary>
        /// Gets the folder with the specified name from the current folder.
        /// </summary>
        /// <param name="name">The name of the folder.</param>
        /// <returns>When this method completes, it returns a Folder</returns>
        IFolder GetFolder(string name);

        /// <summary>
        /// Gets the folders in the current folder.
        /// </summary>
        /// <returns>When this method completes, it returns a list of Folders</returns>
        IList<IFolder> GetFolders();

        /// <summary>
        /// Determines whether the given folder exists in the current folder.
        /// </summary>
        /// <param name="name">The name of the folder.</param>
        /// <returns>true if *name* refers to an existing folder; 
        /// false if the folder does not exist or an error occurs 
        /// when trying to determine if the specified folder exists.</returns>
        bool CheckFolderExists(string name);

        /// <summary>
        /// Deletes the current folder.
        /// </summary>
        void Delete();

        /// <summary>
        /// Deletes the current folder with all of its content.
        /// Warning: Recurse delete.
        /// </summary>
        void DeleteRecursively();
    }
}
