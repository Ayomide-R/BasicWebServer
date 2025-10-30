using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Generic;

public static class Server
{
    private static HttpListener? listener;
    private static Router? router;

    public static void Start()
    {
        string websitePath = Path.Combine(Directory.GetCurrentDirectory(), "Website");
        router = new Router(websitePath);
        
        
        router.AddRoute(new Route() { Verb = "GET", Path = "/submit", Action = MyCustomHandler });
        router.AddRoute(new Route() { Verb = "POST", Path = "/submit", Action = MyCustomHandler });

        router.AddRoute(new Route() 
        { 
            Verb = "GET", 
            Path = "/admin", 
            Action = MyProtectedHandler,
            RequiresAuthorization = true 
        });
        
        router.AddRoute(new Route() { Verb = "GET", Path = "/login", Action = MyLoginHandler });
        
        listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");

        listener.Start();
        Console.WriteLine("Server started. Listening on http://localhost:8080/");
        Console.WriteLine($"Serving content from: {websitePath}");

        StartSessionCleanup(); 

        Task.Run(() => RunServer());
    }
    private static async void RunServer()
    {
        while (listener != null && listener.IsListening)
        {
            try
            {
                HttpListenerContext context = await listener.GetContextAsync();
                HandleConnection(context);
            }
            catch (HttpListenerException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling request: {ex}");
            }
        }
    }
    private static byte[] MyCustomHandler(HttpListenerContext context)
    {
        string responseText;
        string sessionInfo = $"Session ID: {context.Request.Cookies["SessionID"]?.Value ?? "N/A"}";

        if (context.Request.HttpMethod == "GET")
        {
        
            responseText = $@"
                <html>
                <head>
                    <title>POST Test</title>
                    <style>
                        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f0f2f5; color: #333; margin: 0; display: flex; justify-content: center; align-items: center; min-height: 100vh; }}
                        .container {{ background: #ffffff; padding: 40px; border-radius: 15px; box-shadow: 0 8px 16px rgba(0,0,0,0.1); text-align: center; }}
                        h1 {{ color: #4a7dff; border-bottom: none; margin-bottom: 25px; }}
                        form {{ margin-top: 20px; }}
                        input[type='text'] {{ padding: 12px; margin-bottom: 15px; border: 1px solid #ddd; border-radius: 8px; width: 250px; transition: border-color 0.3s; }}
                        input[type='text']:focus {{ border-color: #4a7dff; outline: none; }}
                        input[type='submit'] {{ background-color: #28a745; color: white; padding: 12px 25px; border: none; border-radius: 8px; cursor: pointer; font-size: 1.05em; transition: background-color 0.3s, transform 0.1s; }}
                        input[type='submit']:hover {{ background-color: #1e7e34; transform: translateY(-2px); }}
                        p {{ color: #777; font-size: 0.9em; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Submit Data</h1>
                        <p>{sessionInfo}</p>
                        <form method='POST' action='/submit'>
                            <label for='username'>Username:</label><br><input type='text' id='username' name='username'><br>
                            <label for='message'>Message:</label><br><input type='text' id='message' name='message'><br><br>
                            <input type='submit' value='Simulate Login'>
                        </form>
                    </div>
                </body>
                </html>";
        }
        else if (context.Request.HttpMethod == "POST")
        {
            Dictionary<string, string> data = RequestUtils.GetPostData(context.Request);
            
            string username = data.GetValueOrDefault("username", "NONE");
            string message = data.GetValueOrDefault("message", "NONE");

            Session? session = SessionManager.GetSession(context.Request.Cookies["SessionID"]?.Value ?? "");
            
            if (session != null)
            {
                session.Data["Username"] = username;
                session.Data["IsLoggedIn"] = true; 
            }
            
            responseText = $@"
                <html>
                <head>
                    <title>POST Result</title>
                    <style>
                        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f0f2f5; color: #333; margin: 0; display: flex; justify-content: center; align-items: center; min-height: 100vh; }}
                        .container {{ background: #ffffff; padding: 40px; border-radius: 15px; box-shadow: 0 8px 16px rgba(0,0,0,0.1); text-align: center; }}
                        h1 {{ color: #28a745; border-bottom: none; margin-bottom: 25px; }}
                        p {{ font-size: 1.05em; margin: 8px 0; }}
                        b {{ color: #000; }}
                        a {{ color: #4a7dff; text-decoration: none; font-weight: bold; }}
                        a:hover {{ text-decoration: underline; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Data Received and Logged In!</h1>
                        <p>Username: <b>{username}</b></p>
                        <p>Message: <b>{message}</b></p>
                        <p style='color: #28a745; font-weight: bold;'>Authorization Flag Set: True</p> 
                        <p><a href='/admin'>Go to Protected Page (/admin)</a></p>
                    </div>
                </body>
                </html>";
        }
        else
        {
            context.Response.StatusCode = 405; 
            responseText = "Method Not Allowed";
        }
        
        return Encoding.UTF8.GetBytes(responseText);
    }
    
    private static byte[] MyProtectedHandler(HttpListenerContext context)
    {
        string responseText = $@"
            <html>
            <head>
                <title>Admin Area</title>
                <style>
                    body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #e9f5e9; color: #1e4d2b; margin: 0; display: flex; justify-content: center; align-items: center; min-height: 100vh; }}
                    .container {{ background: #ffffff; padding: 50px; border-radius: 20px; box-shadow: 0 10px 20px rgba(0, 128, 0, 0.2); text-align: center; }}
                    h1 {{ color: #28a745; border-bottom: 3px solid #28a745; padding-bottom: 15px; margin-bottom: 20px; }}
                    p {{ font-size: 1.3em; color: #333; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Welcome, Authorized User!</h1>
                    <p>You can see this because Roy verified your session.</p>
                </div>
            </body>
            </html>";
        
        return Encoding.UTF8.GetBytes(responseText);
    }

    private static byte[] MyLoginHandler(HttpListenerContext context)
    {
        string responseText = $@"
            <html>
            <head>
                <title>Login Required</title>
                <style>
                    body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f0f2f5; color: #333; margin: 0; display: flex; justify-content: center; align-items: center; min-height: 100vh; }}
                    .container {{ background: #ffffff; padding: 40px; border-radius: 15px; box-shadow: 0 8px 16px rgba(255, 0, 0, 0.2); text-align: center; }}
                    h1 {{ color: #cc0000; margin-bottom: 20px; }}
                    p {{ font-size: 1.1em; margin: 10px 0; }}
                    a {{ color: #4a7dff; text-decoration: none; font-weight: bold; }}
                    a:hover {{ text-decoration: underline; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Access Denied: Please Log In</h1>
                    <p>You were redirected here because the page you tried to access (/admin) requires Roy authorization.</p>
                    <p>Click <a href='/submit'>here</a> to submit data and then try /admin again.</p>
                </div>
            </body>
            </html>";
        
        return Encoding.UTF8.GetBytes(responseText);
    }

    private static void HandleConnection(HttpListenerContext context)
    {
        string? sessionId = context.Request.Cookies["SessionID"]?.Value;
        Session? session = sessionId != null ? SessionManager.GetSession(sessionId) : null;

        if (session == null)
        {
            session = SessionManager.CreateNewSession();
            context.Response.SetCookie(new Cookie("SessionID", session.Id)
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                Path = "/",
            });
        }

        if (router == null)
        {
            byte[] buffer = Encoding.UTF8.GetBytes("Server error: router not initialized");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
            return;
        }

        string contentType;
        byte[] fileContent = router.Route(context, out contentType);

        context.Response.ContentType = contentType;
        context.Response.ContentLength64 = fileContent.Length;

        context.Response.OutputStream.Write(fileContent, 0, fileContent.Length);
        context.Response.OutputStream.Close();

        Console.WriteLine($"Handled request for: {context.Request.Url} | Session: {session.Id}");
    }
    
    private static void StartSessionCleanup()
    {
        int cleanupIntervalMs = 300000;

        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(cleanupIntervalMs);
                SessionManager.CleanExpiredSessions();
            }
        });
    }
}