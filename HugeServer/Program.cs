using System;
using System.Threading.Tasks;
using System.Threading;


/*
 * Logic: TopPackageProc - DownPackageProc
 * Logic: TopRequestBradge
 * Logic: TopSessionManagement - DownSessionManagement
 * Logic: MessageBradge
 * Engine: NetworkEngine - TCPServer - In/Out PackageQueue - SessionManagement
 * Engine: DBEngine - WriteQueue - ReadQueue
 */

class Program
{
    static void Main(string[] args)
    {
        Debug.Init();

        Task.Factory.StartNew(() =>
        {
            DBEngine.Inst.StartEngine();
            NetworkEngine.Inst.InitEngine();
            Logic.Inst.StartLogic();
            NetworkEngine.Inst.StartEngine();
        });

        RunForLoop();
    }

    private static void RunForLoop()
    {
        while (true)
        {
            Thread.Sleep(1000);
            System.Console.WriteLine("Hello");
        }
    }

    private static void TestLog()
    {
        int index = 0;
        while (true)
        {
            if(index > 100000)
            {
                break;
            }
            if(index % 2 == 0)
            {
                Debug.Log("hahahahahahahahahahahahahaha - " + index);
            }
            if(index % 3 == 0)
            {
                Debug.LogError("hahahaahahahahahahahahahahah - " + index);
            }
            if(index % 4 == 0)
            {
                Debug.LogWarning("hahahaahahahahahahahahahahah - " + index);
            }
            index += 1;
        }
    }
}
