using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UDPManager : MonoBehaviour
{
    public string IpAddress;
    public int ConnectPort;
    public string RecvStr;

    private Socket UdpSocket;
    private EndPoint ClientEnd;
    private IPEndPoint IpEnd;
    private string SendStr;
    private byte[] RecvData = new byte[1024];
    private byte[] SendData = new byte[1024];
    private int RecvLen;
    private Thread ConnectThread;

    private long TimeNow;
    private float IntervalTime;
    private List<string> SignList = new List<string>();
    private bool AsSameSign;

    private float signal_delay;

    List<EndPoint> ClientList;


    private string IpAddressStr;
    public Text text1;
    public Text text2;
    public GUIStyle GUIstyle;

    public InputField IpAddressInputField;
    public InputField ConnectPortInputField;

    // Use this for initialization
    void Start()
    {
        //IpAddress = "192.168.1.70";
        //ConnectPort = 7400;

        RecvStr = "";
        //InitSocket(); //在这里初始化server


        ClientList = new List<EndPoint>();
       
    }
    public void Check() {
        IpAddress = IpAddressInputField.text;
        ConnectPort = int.Parse(ConnectPortInputField.text);
        InitSocket(); //在这里初始化server
    }
    //初始化
    void InitSocket()
    {
        IpEnd = new IPEndPoint(IPAddress.Parse(IpAddress), ConnectPort);
        UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        UdpSocket.Bind(IpEnd);
        //定义客户端
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        ClientEnd = (EndPoint)sender;
        
        //开启一个线程连接
        ConnectThread = new Thread(new ThreadStart(SocketReceive));
        ConnectThread.Start();


    }
    void SocketSend(string sendStr)
    {

        if (ClientList.Count != 0)
        {
            for (int i = 0; i < ClientList.Count; i++)
            {
                SendData = new byte[1024];
                SendData = Encoding.UTF8.GetBytes(sendStr + i);
                UdpSocket.SendTo(SendData, SendData.Length, SocketFlags.None, ClientList[i]);

            }
        }
        //UdpSocket.SendTo(SendData, SendData.Length, SocketFlags.None, ClientEnd);

        //UdpSocket.SendTo(SendData, SendData.Length, SocketFlags.None, new IPEndPoint(IPAddress.Parse("192.168.1.255"), ConnectPort));
    }
    //服务器接收
    void SocketReceive()
    {
        while (true)
        {
            RecvData = new byte[1024];
            RecvLen = UdpSocket.ReceiveFrom(RecvData, ref ClientEnd);
            //将客户端地址记录到客服端列表

            RecvStr = Encoding.UTF8.GetString(RecvData, 0, RecvLen);
            
            IpAddressStr = ClientEnd.ToString();
            if (RecvStr == "Successful")
            {
                if (ClientList.Count != 0)
                {
                    bool same = false;
                    for (int i = 0; i < ClientList.Count; i++)
                    {
                        if ((ClientEnd + "") == (ClientList[i] + ""))
                        {
                            same = true;
                            break;
                        }
                    }
                    if (!same)
                    {
                        ClientList.Add(ClientEnd);
                        SendData = new byte[1024];
                        SendData = Encoding.UTF8.GetBytes("Successful");
                        UdpSocket.SendTo(SendData, SendData.Length, SocketFlags.None, ClientEnd);
                    }
                }
                else
                {
                    ClientList.Add(ClientEnd);
                    SendData = new byte[1024];
                    SendData = Encoding.UTF8.GetBytes("Successful");
                    UdpSocket.SendTo(SendData, SendData.Length, SocketFlags.None, ClientEnd);
                }
                for (int i = 0; i < ClientList.Count; i++)
                {
                    Debug.Log("Client IP Addreee " + i + " ==  " + ClientList[i]);
                }

            }
            //SocketSend("服务器发来的数据：" + recvStr);

            //信号过滤，避免短时间内接收到大量相同的信号
            if (signal_delay != 0)
            {
                this.IntervalTime = (DateTime.Now.Ticks - this.TimeNow) / 10000;
                this.IntervalTime = this.IntervalTime / 1000;

                if (this.IntervalTime < signal_delay)
                {
                    for (int i = 0; i < this.SignList.Count; i++)
                    {
                        if (this.SignList[i] == RecvStr)
                        {
                            this.AsSameSign = true;
                        }
                    }
                    if (!this.AsSameSign)
                    {
                        this.SignList.Add(RecvStr);
                        for (int j = 0; j < this.SignList.Count; j++)
                        {
                            //print("???????" + this.SignList[j]);
                        }

                    }
                }
                else
                {
                    this.AsSameSign = false;
                    this.SignList.Clear();
                    //this.SignList = null;
                    this.SignList.Add(RecvStr);
                    for (int i = 0; i < this.SignList.Count; i++)
                    {
                        print("===" + this.SignList[i]);
                    }

                    this.TimeNow = DateTime.Now.Ticks;
                }
            }

        }
    }
    private void Update()
    {
        text1.text = RecvStr;
        text2.text = IpAddressStr;
    }
    public void SendSocket()
    {
        SocketSend("Server:"+System.DateTime.Now);
    }
    //连接关闭
    void SocketQuit()
    {
        //关闭线程
        if (ConnectThread != null)
        {
            ConnectThread.Interrupt();
            ConnectThread.Abort();
        }
        //最后关闭socket
        if (UdpSocket != null)
            UdpSocket.Close();
        Debug.LogWarning("断开连接");
    }



    void OnApplicationQuit()
    {
        SocketQuit();
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 100), GetLocalIPAddress(), GUIstyle);
    }

    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                // hintText.text = ip.ToString();
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
}
