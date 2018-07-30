using System;
using System.Collections.Generic;
using System.Text;

public class Logic
{
    private static Logic _inst = null;
    public static Logic Inst {
        get {
            if(_inst == null)
            {
                _inst = new Logic();
            }

            return _inst;
        }
    }

    public void StartLogic()
    {

    }
}
