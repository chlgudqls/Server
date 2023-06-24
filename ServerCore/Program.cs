using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);
            // 먹통의 단점극복 일감을 지정
            for (int i = 0; i < 5; i++)
            {
                Task t = new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning); // 오래걸릴것이다 추가해주면 5명제한두고 5명모두일시켜도? 아래 일을 해줌
                t.Start();
            }
            // 최소1명 최대5명을 고용해서 사용하고 
            // 아마도 최대직원이5명   5명모두가 다돌고있으면 아래의 스레드가 실행되지못한다 직원제한 뒀기때문
           
            //for (int i = 0; i < 5; i++) // 5명을 돌린거임
            //    ThreadPool.QueueUserWorkItem(obj => { while (true) { } });

            // Thread가 정직원이면 ThreadPool은 단기알바  백그라운도로돌아가는 스레드
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
            while (true)
            {

            }
        }
    }
}
