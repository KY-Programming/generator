using KY.Core.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KY.Generator.Tests;

/// <summary>
/// Every generated file is written under a <see cref="FileLock"/>, so a lock folder that can not be created would stop
/// the whole generation
/// </summary>
[TestClass]
public class FileLockTests
{
    private static readonly string temp = Path.GetTempPath();

    [TestMethod]
    public void TheLocksLiveInTheLocalApplicationData()
    {
        string localApplicationData = FileSystem.Combine(temp, "LocalAppData", "KY-Programming", "KY-Generator");

        Assert.AreEqual(FileSystem.Combine(localApplicationData, "locks"), FileLock.GetDirectory(localApplicationData, temp));
    }

    /// <summary>
    /// Without a local application data folder the path of the generator is relative, the way
    /// <see cref="Models.GeneratorEnvironment"/> builds it from an empty folder
    /// </summary>
    [TestMethod]
    public void WithoutALocalApplicationDataFolderTheLocksLiveInTheTempFolder()
    {
        string localApplicationData = FileSystem.Combine(string.Empty, "KY-Programming", "KY-Generator");

        string directory = FileLock.GetDirectory(localApplicationData, temp);

        Assert.IsTrue(FileSystem.IsAbsolute(directory), $"{directory} would resolve into the directory of the generator");
        StringAssert.StartsWith(directory, FileSystem.Combine(temp, "KY-Programming", "KY-Generator"));
    }
}
