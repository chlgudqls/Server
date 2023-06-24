﻿using System;
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
        static bool _stop = false;

        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작");
            while (!_stop)
            {

            }
            Console.WriteLine("쓰레드 종료");
        }
        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain);
            t.Start();

            Thread.Sleep(1000);

            _stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기중");
        }
    }
}
