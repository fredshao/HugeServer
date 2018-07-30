using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

public class SessionManager
{
    private ConcurrentDictionary<int, Session> topSessions = new ConcurrentDictionary<int, Session>();
    private ConcurrentDictionary<int, Session> downSessions = new ConcurrentDictionary<int, Session>();


    public void AddTopSession(Session _session)
    {
        if(!topSessions.TryAdd(_session.id, _session))
        {
            Debug.LogError("添加TopSession失败: " + _session.id);
        }


    }
}
