namespace FitnessTracker.Tests;

using System.Net.Mime;
using FitnessTracker;

public class FileManagerTests
{
    FileManager fileManager;
    string testFileName;

    public FileManagerTests()
    {
        // before each test:
        testFileName = "testfile.txt";
        File.Delete(testFileName); 
        fileManager = new FileManager();
    }

    [Fact]
    public void Test_AppendLine_AppendsLineWithNewline()
    {
        fileManager.AppendLine(testFileName, "Test String");
        var content =File.ReadAllText(testFileName);
        Assert.Equal("Test String" + Environment.NewLine, content);
    }

    [Fact]
    public void Test_AppendLine_AppendsTwoLines()
    {
        fileManager.AppendLine(testFileName, "First Line");
        fileManager.AppendLine(testFileName, "Second Line");
        var content = File.ReadAllText(testFileName);
        Assert.Equal("First Line" + Environment.NewLine + "Second Line" + Environment.NewLine, content);   
    }

    [Fact]
    public void Test_ReadAllLines_ReturnsAllLines()
    {
        fileManager.AppendLine(testFileName, "First Line");
        fileManager.AppendLine(testFileName, "Second Line");
        var lines = fileManager.ReadAllLines(testFileName);
        Assert.Equal(2, lines.Length);
        Assert.Equal("First Line", lines[0]);
        Assert.Equal("Second Line", lines[1]);
    }

    [Fact]
    public void Test_EnsureFileExists_CreatesFileIfMissing()
    {
        var lines = fileManager.ReadAllLines(testFileName);
        Assert.True(File.Exists(testFileName));
        Assert.Empty(lines);
    }
}