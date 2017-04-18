/// <summary>
/// Implementation from
/// Windows.Foundation.UniversalApiContract, Version=4.0.0.0
/// </summary>

namespace Plugin.NetStandardStorage.Abstractions.Types
{
    /// <summary>
    /// Specifies what to do if a file or folder with the specified name already exists
    /// in the current folder when you copy, move, or rename a file or folder.
    /// </summary>
    public enum NameCollisionOption
    {
        /// <summary>
        /// Automatically append a number to the base of the specified name if the file or
        /// folder already exists.
        /// </summary>
        GenerateUniqueName = 0,

        /// <summary>
        /// Replace the existing item if the file or folder already exists.
        /// </summary>
        ReplaceExisting = 1,

        /// <summary>
        /// Raise an exception of type **System.Exception** if the file or folder already
        /// exists.
        /// </summary>
        FailIfExists = 2,
    }
}
