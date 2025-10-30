using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq; 
using System.Text;

public class Router
{
    public string WebsitePath { get; set; }
    private List<Route> customRoutes = new List<Route>();

    public Router(string websitePath)
    {
        WebsitePath = websitePath;
    }

    public void AddRoute(Route route)
    {
        customRoutes.Add(route);
        Console.WriteLine($"[Router] Registered route: {route.Verb} {route.Path}");
    }

    public byte[] Route(HttpListenerContext context, out string contentType)
    {
        string urlPath = context.Request.Url?.AbsolutePath ?? "/";
        string verb = context.Request.HttpMethod;
        
        Route? matchedRoute = customRoutes.SingleOrDefault(r =>
            r.Verb.Equals(verb, StringComparison.OrdinalIgnoreCase) &&
            r.Path.Equals(urlPath, StringComparison.OrdinalIgnoreCase));

        if (matchedRoute != null)
        {
            if (matchedRoute.RequiresAuthorization)
            {
                string sessionId = context.Request.Cookies["SessionID"]?.Value ?? string.Empty;
                Session? session = SessionManager.GetSession(sessionId); 
                
                if (session == null || !session.Data.ContainsKey("IsLoggedIn") || (bool)session.Data["IsLoggedIn"] == false)
                {
                    context.Response.Redirect("/login"); 
                    contentType = "text/html";
                    context.Response.StatusCode = 302; 
                    return Encoding.UTF8.GetBytes("Redirecting to login..."); 
                }
            }
            
            contentType = "text/html"; 
            return matchedRoute.Action(context);
        }
        string fileName = urlPath.TrimStart('/');

        if (string.IsNullOrEmpty(fileName))
        {
            fileName = "index.html";
        }

        string fullPath = Path.Combine(WebsitePath, fileName);
        
        if (fileName.EndsWith(".html")) { contentType = "text/html"; }
        else if (fileName.EndsWith(".css")) { contentType = "text/css"; }
        else if (fileName.EndsWith(".ico")) { contentType = "image/x-icon"; }
        else { contentType = "application/octet-stream"; }

        if (File.Exists(fullPath))
        {
            return File.ReadAllBytes(fullPath);
        }
        else
        {
            context.Response.StatusCode = 404;
            
            string notFoundPath = Path.Combine(WebsitePath, "404.html");

            if (File.Exists(notFoundPath))
            {
                contentType = "text/html";
                return File.ReadAllBytes(notFoundPath);
            }
            else
            {
                contentType = "text/plain";
                return Encoding.UTF8.GetBytes($"404 Not Found: {urlPath}");
            }
        }
    }
}