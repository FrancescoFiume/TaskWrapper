using System.Diagnostics;

namespace TaskWrapper;

public class Executor
{
    private Process process;
    private string Path { get; set; }
    private string Arguments { get; set; }
    protected Queue<string> Logging {
        get;
        set;
    }
    

    public Executor(string path, string arguments)
    {
        process = new Process();
        Arguments = arguments;
        Logging = new Queue<string>(300);
        
    }

    private void Compile()
    {
        process.StartInfo.FileName = new ExtensionExtractor(Path).Extract();
        if (Arguments != "")
        {
        process.StartInfo.Arguments = Arguments+" "+Path;
        }
        else
        {
            process.StartInfo.Arguments = Path;
        }
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;
        process.OutputDataReceived += (sender, e) => HandleOutput(e.Data);
        process.ErrorDataReceived += (sender, e) => HandleError(e.Data);
    }

    private void HandleOutput(string output)
    {
        if (output != null)
        {
            
            if (Logging.Count >= 300)
            {
                Logging.Dequeue(); 
            }
            Logging.Enqueue(output); 
        }
        
    }
    void HandleError(string output)
    {
        if (output != null)
        {
            if (Logging.Count >= 300)
            {
                Logging.Dequeue(); 
            }
            Logging.Enqueue("Errore: " + output); 
        }
    }

}