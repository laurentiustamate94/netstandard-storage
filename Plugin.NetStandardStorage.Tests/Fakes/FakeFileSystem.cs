using Plugin.NetStandardStorage.Abstractions.Interfaces;
using Plugin.NetStandardStorage.Implementations;

namespace Plugin.NetStandardStorage.Tests.Fakes
{
    public sealed class FakeFileSystem : FileSystem
    {
        private string TestDirectory => NUnit.Framework.TestContext.CurrentContext.TestDirectory;

        public override IFolder LocalStorage
            => new Folder(TestDirectory);

        public override IFolder RoamingStorage
            => new Folder(TestDirectory);
    }
}
