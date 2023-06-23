using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerCore
{
    class Program
    {
        static void MainThread(object state)
        {
            for (int i = 0; i < 5; i++)
                Console.WriteLine("Hello Thread!");
        }
        static void Main(string[] args)
        {
            // Thread가 정직원이면 ThreadPool은 단기알바
            ThreadPool.QueueUserWorkItem(MainThread);
            //// 실행될 메인함수를 넣어줘야된다
            //Thread t = new Thread(MainThread);
            //t.Name = "Test Thread";
            //// 백그라운드 실행이라서 메인종료되면 같이 종료
            //t.IsBackground = true;
            //t.Start();

            //Console.WriteLine("Waiting for Thread!");
            //t.Join();

            //Console.WriteLine("Hello World!");
        }
    }
}
