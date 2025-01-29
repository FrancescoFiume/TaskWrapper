namespace TaskWrapper;

public class Wrapper
{
    public int Id  { get; set; }
    private string TaskPath { get; set; }
    protected DateTime CreationTime { get; set; }
    protected DateTime StartTime { get; set; }

    public Wrapper(string taskPath, int id)
    {
        TaskPath = taskPath;
        Id = id;
        CreationTime = DateTime.Now;
    }

    public void Start()
    {
       // new Executor();
    }
    
}