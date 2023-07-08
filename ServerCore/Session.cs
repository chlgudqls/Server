using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Session
    {
        Socket _socket;
        int _disconnected = 0;

        object _lock = new object();

        Queue<byte[]> _sendQueue = new Queue<byte[]>();
        bool _pending = false;
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();

        public void Start(Socket socket)
        {
            _socket = socket;
            SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
            recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);

            // 추가로 넘겨주고 싶은정보
            //recvArgs.UserToken = this;
            recvArgs.SetBuffer(new byte[1024], 0, 1024);
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            RegisterRecv(recvArgs);
        }

        public void Send(byte[] sendBuff)
        {
            lock (_lock)
            {
                // 일단 데이터를 큐에 집어넣고 
                _sendQueue.Enqueue(sendBuff);
                // 전송가능할때 밀티로 동시다발적으로 겹친게 없을때
                if (!_pending)
                    RegisterSend();

            }
                
            //_socket.Send(sendBuff);
            //_sendArgs.SetBuffer(sendBuff, 0, sendBuff.Length);

            //RegisterSend();
        }

        public void Disconnent()
        {
            if (Interlocked.Exchange(ref _disconnected, 1) == 1)
                return;

            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        #region 네트워크 통신 

        void RegisterSend()
        {
            // 혹시라도 들어왔을때 스킵시키려는것 누군가 예약한건지 판별하려는것
            _pending = true;
            byte[] buff = _sendQueue.Dequeue();
            _sendArgs.SetBuffer(buff, 0, buff.Length);

            bool pending = _socket.SendAsync(_sendArgs);
            if (!pending)
                OnSendCompleted(null, _sendArgs);
        }

        // complete에서 실행하는 경우도있음
        void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        // 큐에 혹시라도 들어간경우 여기서 남은거 처리해줌
                        if (_sendQueue.Count > 0)
                            RegisterSend();
                        else // 아무도 샌드큐에 패킷을 추가하지않는다
                        // 성공 
                        _pending = false;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"OnSendCompleted Failed {e}");
                    }
                }
                else
                {
                    Disconnent();
                }
            }

        }
        void RegisterRecv(SocketAsyncEventArgs args)
        {
            bool pending = _socket.ReceiveAsync(args);
            if (!pending)
                OnRecvCompleted(null, args);
        }

        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                    //string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes); // string으로 변환
                    string recvData = Encoding.UTF8.GetString(args.Buffer, args.Offset, args.BytesTransferred); // string으로 변환
                    Console.WriteLine($"[From Client] {recvData}");
                    RegisterRecv(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"OnRecvCompleted Failed {e}");
                }
            }
            else
            {
                // TODO DIsconnect
                Disconnent();
            }
        }
        #endregion
    }
}
