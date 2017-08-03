using System;

namespace SimpleHttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing,Please Wait a few Second");

            Server myServer = new Server();
            //输出初始化结果
            if (myServer.Initialze()==1)
            {
                Console.WriteLine(
                    "Initialization is Complete,Server is running at port "
                    + myServer.ServerPort);
            }
            else
            {
                Console.WriteLine("Initializtion failed,Please Restart!");
            }
            
            Console.ReadKey();
        }
    }
}