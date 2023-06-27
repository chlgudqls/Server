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
        // 1.근성
        // 2.양보
        // 3.갑질

        // 상호배제
        static object _lock = new object();
        static SpinLock _lock2 = new SpinLock();
        static Mutex _lock3 = new Mutex();
        static ReaderWriterLockSlim _lock4 = new ReaderWriterLockSlim(); // 우선순위가있음 평소에는 좀넓은공용화장실 vip오면 한명만사용

        class Reward
        {

        }

        static Reward GetReadById(int id)
        {
            _lock4.EnterReadLock();

            _lock4.ExitReadLock();
            return null;
        }

        static void AddReWard(Reward reward)
        {
            _lock4.EnterWriteLock();

            _lock4.ExitWriteLock();
        }
        static void Main(string[] args)
        {

            lock (_lock)
            {

            }
            bool lockTaken = false;

            try
            {
                _lock2.Enter(ref lockTaken);
            }
            finally
            {
                if(lockTaken)
                    _lock2.Exit();
            }
            // 정상처리가 안됐을 경우대비 bool에 결과물을넣어주는방법으로 처리하기위함

        }
    }
}
