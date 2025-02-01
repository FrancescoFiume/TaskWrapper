using System.Net;
using System.Text;

namespace TaskWrapper.API;

public class WebServer
{
    private HttpListener _listener;
    private readonly Wrapper _wrapper;
    private readonly Router _router;





    public WebServer(Wrapper wrapper, Router router)
    {
        _wrapper = wrapper;
        _router = router;
    }


    public void Serve(int port=8800)
    {
        Task.Run(async () =>
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{port}/");
            _listener.Start();
            _router.AddRoute("GET","/creationTime/", CreationDatetimeAsync);
            _router.AddRoute("GET","/startTime/", StartDatetimeAsync);
            _router.AddRoute("GET","/log/", LogAsync);

            while (true)
            {
                HttpListenerContext context = await _listener.GetContextAsync();
                
                await _router.HandleRequest(context);
            }
        });
       
        
    }
    
    
    public async Task CreationDatetimeAsync(HttpListenerContext context)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(_wrapper.CreationTime.ToString());
        context.Response.ContentLength64 = buffer.Length;
        context.Response.ContentType = "text/html";
        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        context.Response.OutputStream.Close();
    }
    public async Task StartDatetimeAsync(HttpListenerContext context)
    {
        byte[] buffer;
        if (_wrapper.StartTime != null)
        {
            buffer = Encoding.UTF8.GetBytes(_wrapper.CreationTime.ToString());
        }
        else
        {
            buffer = Encoding.UTF8.GetBytes("Task Hasn't Started");
        }
        context.Response.ContentLength64 = buffer.Length;
        context.Response.ContentType = "text/html";
        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        context.Response.OutputStream.Close();
    }
    
    public async Task LogAsync(HttpListenerContext context)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(_wrapper.Log());
        context.Response.ContentLength64 = buffer.Length;
        context.Response.ContentType = "text/html";
        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        context.Response.OutputStream.Close();
    }
}