namespace TaskWrapper.Tests;
using Xunit;
public class ExecutorTest
{
    [Fact]
    public async Task ShouldReturnTrue_WhenProcessCompletes()
    {
        string path = "/home/fiumef/PycharmProjects/es/file.py";
        string arguments = "";
        
        Executor executor = new Executor(path, arguments);
        bool result = await executor.Run();

        Assert.True(result);

    }
    [Fact]
    public async Task Run_ShouldReturnFalse_WhenProcessFailsToStart()
    {
        
        string path = "nonexistent.py"; 
        string arguments = "";
        var executor = new Executor(path, arguments);
        
        bool result = await  executor.Run();
        
        Assert.False(result);
    }
    [Fact]
    public void HandleOutput_ShouldAddOutputToLogging()
    {
        string path = "/home/fiumef/PycharmProjects/es/file.py";
        string arguments = "";
        var executor = new Executor(path, arguments);
        
        executor.Run();
        var logging = executor.Log();
        Assert.Contains("678", logging);
    }
    
    
}