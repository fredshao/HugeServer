using System;
using System.Collections.Generic;
using System.Text;

public class DBEngine
{
    private static DBEngine _inst = null;
    public static DBEngine Inst {
        get {
            if(_inst == null)
            {
                _inst = new DBEngine();
            }

            return _inst;
        }
    }

    public void StartEngine()
    {

    }
}
