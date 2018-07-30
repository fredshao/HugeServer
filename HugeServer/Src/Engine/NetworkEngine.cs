using System;
using System.Collections.Generic;
using System.Text;

public class NetworkEngine
{
    private static NetworkEngine _inst = null;
    public static NetworkEngine Inst {
        get {
            if(_inst == null)
            {
                _inst = new NetworkEngine();
            }
            return _inst;
        }
    }

    public void InitEngine()
    {

    }

    public void StartEngine()
    {

    }
}
