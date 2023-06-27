using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class SpinLcck
    {
        volatile int _locked = 0;

        // 들어오는 스레드? 이벤트
        public void Acquire()
        {
            // while안에 릴리즈가 들어있고 액콰이어는 대기중이고
            while (true)
            {
                // 이 이벤트가 잠겨있는게 풀리길 기다리고있는 공간
                //int original = Interlocked.Exchange(ref _locked, 1);
                //if (original == 0)
                //    break;

                // 어쨋든 스핀락을 해결하기위해서 _lccked가 0이면 1을 대입
                // 예상하는값
                int expected = 0;
                // 원하는값 알아보기 편하게 변수명지어서 대입
                int desired = 1;
                /*int original = */if(Interlocked.CompareExchange(ref _locked, desired, expected) == expected)
                //if (original == 0)
                    break;
            }
            // 릴리즈가 나감으로써 갖게됨
            //_locked = true;
        }
        // 나가는 이벤트
        public void Release()
        {
            // 나감
            _locked = 0;
        }
    }

    class Program
    {
        static int _num = 0;
        static SpinLcck _lock = new SpinLcck();

        static void Thread_1()
        {
            for (int i = 0; i < 100000; i++)
            {
                // 대기시키는거?num이 실행된이후에 릴리즈가 실행됨으로 소유권을 넘긴다
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
