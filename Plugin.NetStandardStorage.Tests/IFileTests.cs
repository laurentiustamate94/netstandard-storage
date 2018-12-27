using NUnit.Framework;
using Plugin.NetStandardStorage.Tests.Fakes;
using Plugin.NetStandardStorage.Abstractions.Interfaces;
using System;
using Plugin.NetStandardStorage.Abstractions.Types;
using System.IO;

namespace Plugin.NetStandardStorage.Tests
{
    [TestFixture]
    public class IFileTests
    {
        private FakeFileSystem fileSystem;

        private IFolder RootFolder => fileSystem?.LocalStorage;

        #region Setup and Teardown

        [SetUp]
        public void SetupEnvironment()
        {
            fileSystem = new FakeFileSystem();
        }

        [TearDown]
        public void TearDownEnvironment()
        {
            fileSystem = null;
        }

        #endregion

        #region Tests

        [Test]
        public void Exists_InRootFolder_Null_Parameters_ShouldThrowException()
        {
            Assert.Inconclusive("Should throw ArgumentOutOfRangeException instead of FormatException");
            Assert.That(() => RootFolder.CheckFileExists(null), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Exists_InRootFolder_StringEmpty_Parameters_ShouldThrowException()
        {
            Assert.Inconclusive("Should throw ArgumentOutOfRangeException instead of FormatException");
            Assert.That(() => RootFolder.CheckFileExists(string.Empty), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Create_twice_than_check_exists_in_RootFolder_ShouldThrowException()
        {
            var fileName = "a.txt";
            Assert.That(() =>
            {
                RootFolder.CreateFile(fileName, CreationCollisionOption.FailIfExists);
                RootFolder.CreateFile(fileName, CreationCollisionOption.FailIfExists);
            }, Throws.TypeOf<IOException>());
        }

        [Test]
        public void Create_twice_than_check_exists_in_RootFolder_ShouldNotThrowException()
        {
            var fileName = "a.txt";

            RootFolder.CreateFile(fileName, CreationCollisionOption.OpenIfExists);
            RootFolder.CreateFile(fileName, CreationCollisionOption.OpenIfExists);

        }

        [Test]
        public void Exists_InRootFolder()
        {
            var fileName = "a.txt";
            var exists = RootFolder.CheckFileExists(fileName);
            if (exists)
            {
                RootFolder.GetFile(fileName).Delete();
            }

            RootFolder.CreateFile(fileName, CreationCollisionOption.FailIfExists);

            Assert.IsTrue(RootFolder.CheckFileExists(fileName));
        }

        #endregion
    }
}
