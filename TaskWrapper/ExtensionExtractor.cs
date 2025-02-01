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
                return config.bashBin;
            case "csproj" :
                return config.dotnetBin;
            case "py" :
                return config.pythonBin;
            case "go":
                return config.goBin;
        }
        throw new Exception($"Extension {extension} is not supported");
    }
}