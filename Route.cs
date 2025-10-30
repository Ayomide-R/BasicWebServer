using System.Net;
using System.Text;

public delegate byte[] RouteAction(HttpListenerContext context);

public class Route
{
   
    public string Verb { get; set; } = string.Empty; 
    public string Path { get; set; } = string.Empty; 
    public RouteAction Action { get; set; } = delegate { return Array.Empty<byte>(); };

    public bool RequiresAuthorization { get; set; } = false; 
}