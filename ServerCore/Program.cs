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

        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;

        static void Thread1()
        {
            y = 1;
            Thread.MemoryBarrier(); // 경계선
            r1 = x;
        }
        static void Thread2()
        {
            // 이 위아래를 하드웨어에서 바꿔버리는 현상 쿨하게 지멋대로 싱글스레드에선 괜찮지만
            // 멀티스레드에서 이딴식?으로하면 예상한로직이 완전히 꼬이게된다 그래서 이현상을 막기위해서 메모리베리어를 사용한다
            // 하드웨어가 지멋대로 최적화시키는걸 강제하는 방법

            x = 1;
            Thread.MemoryBarrier(); // 경계선
            r2 = y;
        }
        // 어쩃든 릴리즈모드를 사용하면 최적화를 하게되는데 컴퓨터가알아서 이상해보이는코드를 고치기때문에 무한루프에 빠질수가있다
        static void Main(string[] args)
        {
            int count = 0;
            while (true)
            {
                count++;
                x = y = r1 = r2 = 0;

                Task t1 = new Task(Thread1);
                Task t2 = new Task(Thread2);

                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                if (r1 == 0 && r2 == 0)
                    break;
            }

            Console.WriteLine($"{count}번 만에 빠져나옴");

        }
    }
}
