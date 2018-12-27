using NUnit.Framework;
using Plugin.NetStandardStorage.Tests.Fakes;
using Plugin.NetStandardStorage.Abstractions.Interfaces;
using Plugin.NetStandardStorage.Abstractions.Types;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.NetStandardStorage.Tests
{
    [TestFixture]
    public class IFolderTests
    {
        private FakeFileSystem storageManager;

        #region Setup and Teardown

        [SetUp]
        public void SetupEnvironment()
        {
            storageManager = new FakeFileSystem();
        }

        [TearDown]
        public void TearDownEnvironment()
        {
            storageManager = null;
        }

        #endregion

        #region Tests

        [Test]
        public void Exists_Simple()
        {
            var folderName = "a";

            var exists = storageManager.LocalStorage.CheckFolderExists(folderName);
            storageManager.LocalStorage.CreateFolder(folderName, CreationCollisionOption.OpenIfExists);

            Assert.IsTrue(storageManager.LocalStorage.CheckFolderExists(folderName));
        }

        [Test]
        public void Exists_BySubFolder()
        {
            var folderName = "a/b";

            var exists = storageManager.LocalStorage.CheckFolderExists(folderName);
            storageManager.LocalStorage.CreateFolder(folderName, CreationCollisionOption.OpenIfExists);

            Assert.IsTrue(storageManager.LocalStorage.CheckFolderExists(folderName));
        }

        [Test]
        public void Exists_ByDeepFolders()
        {
            var rootFolderPath = "a/b/c";
            var subFolderName = "d";

            var rootFolder = storageManager.LocalStorage.CreateFolder(rootFolderPath, CreationCollisionOption.OpenIfExists);
            Assert.IsNotNull(rootFolder);

            var subFolderPath = Path.Combine(rootFolderPath, subFolderName);
            var subFolder = storageManager.LocalStorage.CreateFolder(subFolderPath, CreationCollisionOption.OpenIfExists);
            Assert.IsNotNull(subFolder);

            var subExtists = rootFolder.CheckFolderExists(subFolderName);
            Assert.IsTrue(subExtists);
        }

        [Test]
        public void Create_two_directories_and_then_delete_them()
        {
            const string dir1 = "Dir1";
            const string dir2 = "Dir2";
            IFolder rootFolder = storageManager.LocalStorage;
            var directoriesBefore = rootFolder.GetFolders().ToArray();

            rootFolder.CreateFolder(dir1, CreationCollisionOption.OpenIfExists);
            rootFolder.CreateFolder(dir2, CreationCollisionOption.OpenIfExists);
            var directoriesAfterCreate = rootFolder.GetFolders().ToArray();

            Assert.IsTrue(directoriesAfterCreate.Any(x => x.Name == dir1));
            Assert.IsTrue(directoriesAfterCreate.Any(x => x.Name == dir2));

            rootFolder.GetFolder(dir1).Delete();
            rootFolder.GetFolder(dir2).Delete();

            var directoriesAfterDelete = rootFolder.GetFolders();
            Assert.AreEqual(directoriesBefore.Length, directoriesAfterDelete.Count);
        }

        [Test]
        public void Create__directory_and_then_delete_and_check_itself()
        {
            const string dir1 = "Dir1";
            IFolder rootFolder = storageManager.LocalStorage;

            var folder = rootFolder.CreateFolder(dir1, CreationCollisionOption.OpenIfExists);

            Assert.IsTrue(rootFolder.CheckFolderExists(folder.Name));

            folder.Delete();

            var directoriesAfterDelete = rootFolder.GetFolders();
            Assert.IsFalse(rootFolder.CheckFolderExists(folder.Name));
        }

        #endregion
    }
}
