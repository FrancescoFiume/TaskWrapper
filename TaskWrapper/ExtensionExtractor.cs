namespace TaskWrapper;

public class ExtensionExtractor
{
    private string path { get; set; }

    public ExtensionExtractor(string path)
    {
        this.path = path;
    }

    public string Extract()
    {
        string extension = path.Split('.').ToList().Last();

        switch (extension)
        {
            case "sh" :
            {
                return "bash ";
            }
            case "cs" :
                return "dotnet run ";
            case "py" :
                return "python3 ";
            case "go":
                return "go run ";
        }
        throw new Exception($"Extension {extension} is not supported");
    }
}