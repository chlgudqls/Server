using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static Listener _listener = new Listener();

        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {

                Session session = new Session();
                session.Start(clientSocket);
                
                byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server  !");
                session.Send(sendBuff);

                Thread.Sleep(1000);

                session.Disconnent();
                session.Disconnent();
                //// 받는다
                //byte[] recvBuff = new byte[1024]; // 보낸게 recvBuff 여기에 저장됨
                //int recvBytes = clientSocket.Receive(recvBuff); // 몇개 짜린지 확인
                //string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes); // string으로 변환

                //Console.WriteLine($"[From Client] {recvData}");

                //// 보낸다
                //byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server  !");
                //clientSocket.Send(sendBuff);  // 여기도 안받으면 대기

                //// 쫓아낸다
                //clientSocket.Shutdown(SocketShutdown.Both);
                //clientSocket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        static void Main(string[] args)
        {
            // DNS (Domain Name Systyem)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];

            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // 문지기 TCP / UDP중 우리는 TCP사용
            // 아이피 주소를 하드코딩해서 박아놓는것보다 도메인이름으로 찾아갈수있게 유연하게 좀 융통성있게

             _listener.Init(endPoint, OnAcceptHandler);
            Console.WriteLine("Listening...");

            // while이 왜 있어야되지근데
            while (true)
            {
                //Socket clientSocket = _listener.Accept();
                ;
            }
        }
    }
}
