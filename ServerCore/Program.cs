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
        // 스레드는 각자의 스택을 가지고있으나 전역으로 선언된 스택을 돌려쓸수있다
        // volatile 최적화를 하지말아달라
        volatile static bool _stop = false;

        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작");
            while (!_stop)
            {

            }
            Console.WriteLine("쓰레드 종료");
        }

        // 어쩃든 릴리즈모드를 사용하면 최적화를 하게되는데 컴퓨터가알아서 이상해보이는코드를 고치기때문에 무한루프에 빠질수가있다
        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain);
            t.Start();

            Thread.Sleep(1000);

            _stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기중");
            // 스레드의 Join과 같은 의미 끝날때까지 기다렸다가 실행한다 위의 start가
            t.Wait();
            Console.WriteLine("종료 성공");

        }
    }
}
