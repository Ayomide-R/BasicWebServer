using System;
using System.Collections.Generic;

public static class SessionManager
{
    private static Dictionary<string, Session> ActiveSessions = new Dictionary<string, Session>();

    private static readonly TimeSpan SessionTimeout = TimeSpan.FromMinutes(20);

    public static Session CreateNewSession()
    {
        string newId = Guid.NewGuid().ToString();
        Session session = new Session(newId);

        ActiveSessions[newId] = session; 

        Console.WriteLine($"[Session] Created new session: {newId}");
        return session;
    }

    public static Session? GetSession(string id)
    {
        if (ActiveSessions.TryGetValue(id, out Session? session) && session != null)
        {
            session.LastActivity = DateTime.UtcNow;
            return session;
        }

        return null;
    }
    
      public static void CleanExpiredSessions()
    {
        var expiredKeys = new List<string>();
        DateTime cutoff = DateTime.UtcNow.Subtract(SessionTimeout);
        int cleanedCount = 0;

        lock (ActiveSessions) 
        {
            foreach (var kvp in ActiveSessions)
            {
                if (kvp.Value.LastActivity < cutoff)
                {
                    expiredKeys.Add(kvp.Key);
                }
            }

            foreach (string key in expiredKeys)
            {
                ActiveSessions.Remove(key);
                cleanedCount++;
            }
        }

        if (cleanedCount > 0)
        {
            Console.WriteLine($"[Session] Cleaned up {cleanedCount} expired sessions.");
        }
    }
}