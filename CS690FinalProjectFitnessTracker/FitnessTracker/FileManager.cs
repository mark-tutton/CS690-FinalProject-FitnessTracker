namespace FitnessTracker;

using System.IO;

public class FileManager
{

    public FileManager()
    {
    }

    private void EnsureFileExists(string fileName)
    {
        var dir = Path.GetDirectoryName(fileName);
        if (!string.IsNullOrEmpty(dir))
        {
            Directory.CreateDirectory(dir);
        }
        if (!File.Exists(fileName))
        {
            File.Create(fileName).Close();
        }
    }
    
    // appends a line to the specified file
    public void AppendLine(string fileName, string line)
    {
        EnsureFileExists(fileName);
        File.AppendAllText(fileName, line + Environment.NewLine);
    }

    // appends data to the specified file
    public void AppendData(string fileName,string data)
    {
        EnsureFileExists(fileName);
        File.AppendAllText(fileName, data);
    }

    public void OverwriteData(string fileName, string data)
    {
        EnsureFileExists(fileName);
        File.WriteAllText(fileName, data);
    }

    // reads all lines from the specified file
    public string[] ReadAllLines(string fileName)
    {
        EnsureFileExists(fileName);
        return File.ReadAllLines(fileName);
    }
}