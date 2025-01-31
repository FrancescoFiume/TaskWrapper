using System.Net;
using System.Text;


namespace TaskWrapper.API;

public class Router
{
    private readonly Dictionary<string, Dictionary<string, Func<HttpListenerContext, Task>>> routes = new();

    public Router()
    {
        routes["GET"] = new Dictionary<string, Func<HttpListenerContext, Task>>();
        routes["POST"] = new Dictionary<string, Func<HttpListenerContext, Task>>();
    }
    
    public void AddRoute(string method, string path, Func<HttpListenerContext, Task> handler)
    {
        if (routes.ContainsKey(method))
        {
            routes[method][path] = handler;
        }
        else
        {
            throw new Exception("Metodo HTTP non supportato");
        }
    }
    
    public async Task HandleRequest(HttpListenerContext context)
    {
        string method = context.Request.HttpMethod;
        string path = context.Request.Url.AbsolutePath;

        if (routes.ContainsKey(method) && routes[method].ContainsKey(path))
        {
            // Invoca il handler per questa route
            await routes[method][path](context);
        }
        else
        {
            // Se la route non è trovata, restituisci un errore 404
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            byte[] buffer = Encoding.UTF8.GetBytes("404 - Not Found");
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }
    }
    
}