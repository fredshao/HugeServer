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
        Console.WriteLine("Hello World!");

        Task.Factory.StartNew(() =>
        {
            DBEngine.Inst.StartEngine();
            NetworkEngine.Inst.InitEngine();
            Logic.Inst.StartLogic();
            NetworkEngine.Inst.StartEngine();
        });

        System.Console.WriteLine(DateTime.Now);

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
}
