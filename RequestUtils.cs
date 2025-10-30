using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

public static class RequestUtils
{
    public static Dictionary<string, string> GetPostData(HttpListenerRequest request)
    {
        var postData = new Dictionary<string, string>();
        
        if (request.HasEntityBody)
        {
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                string rawData = reader.ReadToEnd();
                
                string[] pairs = rawData.Split('&');

                foreach (string pair in pairs)
                {
                    if (string.IsNullOrEmpty(pair)) continue;
        
                    string[] parts = pair.Split('=');
                    
                    if (parts.Length == 2)
                    {

                        string key = WebUtility.UrlDecode(parts[0]);
                        string value = WebUtility.UrlDecode(parts[1]);
                        postData[key] = value;
                    }
                }
            }
        }

        return postData;
    }
}