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
        Path = path;
        process = new Process();
        Arguments = arguments;
        Logging = new Queue<string>(300);
        Compile();
        
    }

    private void Compile()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        //process.StartInfo.FileName = new ExtensionExtractor(Path).Extract();
        startInfo.FileName = new ExtensionExtractor(Path).Extract();
        if (Arguments != "")
        {
            startInfo.Arguments = Arguments+" "+Path;
            //process.StartInfo.Arguments = Arguments+" "+Path;
        }
        else
        {
            startInfo.Arguments = Path;
            //process.StartInfo.Arguments = Path;
        }
        startInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Path);
        startInfo.CreateNoWindow = false;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        
        // process.StartInfo.UseShellExecute = false;
        // process.StartInfo.RedirectStandardOutput = true;
        // process.StartInfo.RedirectStandardError = true;
        // process.StartInfo.CreateNoWindow = true;
        process.StartInfo = startInfo;
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

    public bool Run()
    {
        try
        {
            process.Start();
            process.BeginOutputReadLine(); 
            process.BeginErrorReadLine();

            process.WaitForExit();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errore durante l'avvio del processo: " + ex.Message);
            return false;
        }
        
    }

    public string Log()
    {
        return string.Join("\n", Logging);
    }

}