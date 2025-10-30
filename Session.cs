using System;
using System.Collections.Generic;

public class Session
{

    public string Id { get; private set; }

    public Dictionary<string, object> Data { get; private set; }

    public DateTime LastActivity { get; set; }

    public Session(string id)
    {
        Id = id;
        Data = new Dictionary<string, object>();
        LastActivity = DateTime.UtcNow;
    }
}