using TaskWrapper.API;

namespace TaskWrapper;

public class Wrapper
{
    public int Id  { get; set; }
    private string TaskPath { get; }
    private string Arguments { get;  }
    public  DateTime CreationTime { get; }
    private Executor Executor { get; set; }
    private DateTime _startTime { get; set; }

    public DateTime StartTime { get { return _startTime; } }

    private WebServer WebServer { get; set; }
    protected bool Completed { get; set; } = false;

    public Wrapper(string taskPath, string arguments, int id,bool webserver = false)
    {
        TaskPath = taskPath;
        Arguments = arguments;
        Id = id;
        CreationTime = DateTime.Now;
        if (webserver)
        {
            WebServer = new WebServer(this, new Router());
            WebServer.Serve();
        }
    }

    public async Task Start()
    {
      Executor = new Executor(TaskPath, Arguments);
      _startTime = DateTime.Now;
      bool completed = await Executor.Run();
      if (completed)
      {
          Completed = true;
      }
      
    }
    
}