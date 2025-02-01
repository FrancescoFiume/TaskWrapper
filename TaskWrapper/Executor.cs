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
        startInfo.FileName = new ExtensionExtractor(Path).Extract();
        if (startInfo.FileName.Contains("dotnet"))
        {
            if (Arguments != "")
            {
                startInfo.Arguments = $"run --project {Path} {Arguments}"; 
            }
            else
            {
                startInfo.Arguments = $"run --project {Path}";
            }
        }
        else
        {
            if (Arguments != "")
            {
                startInfo.Arguments = $"{Path} {Arguments}"; 
            }
            else
            {
                startInfo.Arguments = $"{Path}";
            } 
        }
        startInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Path);
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
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

    public async Task<bool> Run()
    {
        try
        {
            process.Start();
            Logging.Enqueue("Il processo è partito.");
            process.BeginOutputReadLine(); 
            process.BeginErrorReadLine();

            
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                Logging.Enqueue($"Il processo è terminato con errore. Codice di uscita: {process.ExitCode}");
            }
            else
            {
                Logging.Enqueue("Il processo è terminato correttamente.");
            }
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