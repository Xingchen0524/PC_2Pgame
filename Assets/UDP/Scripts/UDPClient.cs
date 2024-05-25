using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using UnityEngine.UI;


public class UDPClient : MonoBehaviour
{
    private string RecvStr;
    public string UDPClientIP;
    public int ConnectPort;
    private string ClientStr = "Successful";
    private Socket UdpSocket;
    private EndPoint ServerEnd;
    private IPEndPoint IpEnd;

    private byte[] RecvData = new byte[1024];
    private byte[] SendData = new byte[1024];
    private int RecvLen = 0;
    private Thread ConnectThread;

    public Text text1;
    public Text text2;

    private string IpAddressStr;

    public GUIStyle GUIstyle;
    public InputField IpAddressInputField;
    public InputField ConnectPortInputField;
    void Start()
    {
        //服务端的IP和端口
        //UDPClientIP = "192.168.1.48";
        //ConnectPort = 7400;

        UDPClientIP = UDPClientIP.Trim();
        //InitSocket();

        RecvStr = "";

        //SendStr.GetComponent<InputField>().onEndEdit.AddListener(End_Value);
    }
    public void End_Value(string inp)
    {
        ClientStr = inp;
    }

    public void SendInputStr()
    {
        SocketSend(ClientStr);
    }
    public void Check()
    {
        UDPClientIP = IpAddressInputField.text;
        ConnectPort = int.Parse(ConnectPortInputField.text);
        InitSocket(); //在这里初始化server
    }
    void InitSocket()
    {
        IpEnd = new IPEndPoint(IPAddress.Parse(UDPClientIP), ConnectPort);
        UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        ServerEnd = (EndPoint)sender;
        Debug.Log("Waiting");
        SocketSend(ClientStr);
        Debug.Log("Complete");
        //开启一个线程连接
        ConnectThread = new Thread(new ThreadStart(SocketReceive));
        ConnectThread.Start();

        
        
    }
    void SocketSend(string sendStr)
    {
        //清空
        SendData = new byte[1024];
        //数据转换
        SendData = Encoding.UTF8.GetBytes(sendStr);
        //发送给指定服务端
        UdpSocket.SendTo(SendData, SendData.Length, SocketFlags.None, IpEnd);
    }

    //接收服务器信息
    void SocketReceive()
    {
        
        while (true)
        {

            RecvData = new byte[1024];
            try
            {
                RecvLen = UdpSocket.ReceiveFrom(RecvData, ref ServerEnd);
            }
            catch (Exception e)
            {
            }

            Debug.Log("Message source IP address: " + ServerEnd.ToString());
            IpAddressStr = ServerEnd.ToString();
            //text2.text = SendString;
            //if (recvLen > 0)
            //{
            //    recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
            //}
            RecvStr = Encoding.UTF8.GetString(RecvData, 0, RecvLen);
            
            

        }

        
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
    }
    void OnApplicationQuit()
    {
        SocketQuit();
    }
    public void SocketSendButton()
    {
        SocketSend("Client:"+System.DateTime.Now);
    }
    
    void Update()
    {
        //ReceivedTex.text = recvStr;
        
        text1.text = RecvStr;
        text2.text = IpAddressStr;

        
        
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
