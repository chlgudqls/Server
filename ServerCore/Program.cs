using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Lcck
    {
        // bool <- 커널 - 아랫단계에서의 관리자 // 아랫단계라서 오래걸린다
        // 커널 새로운단어 나옴 
        // 톨게이트
        AutoResetEvent _available = new AutoResetEvent(true); // 누구나 들어올수있는상태
        public void Acquire()
        {
            _available.WaitOne(); // 입장시도 // 문도 자동으로 닫음
        }
        public void Release()
        {
            _available.Set(); // 문개방 true 로 다시켜줌
        }
    }

    class Program
    {
        static int _num = 0;
        static Lcck _lock = new Lcck();

        static void Thread_1()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }
        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(_num);
        }
    }
}
