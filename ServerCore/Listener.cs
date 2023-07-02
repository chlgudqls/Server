using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    class Listener
    {
        Socket _listenSocket;
        Action<Socket> _OnAcceptHandler;

        public void Init(IPEndPoint endPoint, Action<Socket> OnAcceptHandler)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _OnAcceptHandler += OnAcceptHandler;

            // 문지기 교육
            _listenSocket.Bind(endPoint);

            // 영업 시작
            // backlog : 최대 대기수
            _listenSocket.Listen(10);

            // 그게 아니면 여기서 콜백방식으로 complte에 집어넣는다
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            // 언제든 요청이왔으면 콜백방식으로 호출된다
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptComplted);
            RegisterAccept(args);
        }

        // 등록
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            // 계속해서 재사용하고 있기 때문에 한번돌때마다 널로 밀어줌
            args.AcceptSocket = null;

            bool pending = _listenSocket.AcceptAsync(args);

            if (!pending)
                // 운좋게 실행하는 동시에 클라이언트가 접속요청이와서 눈깜짝할사이 팬딩없이 완료됐다
                // 막바로 호출 
                OnAcceptComplted(null, args);
        }

        // 처리
        // 그래서 어떤식으로든  여기에 들어오게된다
        void OnAcceptComplted(object sender, SocketAsyncEventArgs args)
        {
            // 에러없이 잘 처리됐다면
            if (args.SocketError == SocketError.Success)
            {
                _OnAcceptHandler.Invoke(args.AcceptSocket);
            }
            else
                Console.WriteLine(args.SocketError.ToString());

            // 끝난후 다음 아이를위해서 또한번 등록해줌
            RegisterAccept(args);
        }

        public Socket Accept()
        {
            // 싱크 앞에 A붙으면 부정적의미

            // 손님을 입장시킨다.
            return _listenSocket.Accept(); // 세션의 소켓 대리인    // 블로킹함
        }
    }
}
