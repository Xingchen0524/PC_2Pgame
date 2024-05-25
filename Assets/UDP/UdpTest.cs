using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UdpTest : MonoBehaviour
{
    Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    int udpPort = 5200;
    bool isRun = true;

    string broadcastMessage = "房间号|||房间名|||总人数|||人数";    // 要广播的内容

    public void CreateRoom()
    {
        udpSocket.EnableBroadcast = true;    // 权限
        Thread thread_Broadcast = new Thread(Broadcast);
        thread_Broadcast.Start();  // 启动线程进行广播
    }
    private void Broadcast()
    {
        IPAddress[] ipAddressArr = Dns.GetHostAddresses(Dns.GetHostName());  // 得到本机所有的IP地址
        
        for (int i = 0; i < ipAddressArr.Length; i++)
        {
            print(i+".." + ipAddressArr[i]);
        }
        List<string> ipPrefixList = new List<string>();   // IP地址前三个部分相同则说明处于同一局域网，所以把所有IP的前三部分存起来，放一个list中
        foreach (var item in ipAddressArr)
        {
            if (item.AddressFamily == AddressFamily.InterNetwork)    // 判断是不是IPV4
            {
                string ipPrefix = item.ToString();
                int endPointIndex = ipPrefix.LastIndexOf('.');    // 得到最后一个点的位置
                ipPrefix = ipPrefix.Remove(endPointIndex + 1);    // 移除IP的第四部分
                ipPrefixList.Add(ipPrefix);
            }
        }
        while (isRun)    // 不太清楚为什么不管把线程设置成前台线程还是后台线程，在unity编辑里结束游戏的时候，线程都不结束，所以这里使用标志变量来结束
        {
            foreach (var item in ipPrefixList)
            {
                broadcastMessage = string.Format("{0}|||{1}|||{2}|||{3}", 666, "房间1", 8, 0);    // 格式为"房间号|||房间名|||总人数|||人数"（接收的时候使用string.split解析）
                byte[] message = Encoding.UTF8.GetBytes(broadcastMessage);
                udpSocket.SendTo(message, new IPEndPoint(IPAddress.Parse(item + "255"), udpPort));// 255表示广播地址
                //print(new IPEndPoint(IPAddress.Parse(item + "255"), udpPort));
            }
            Thread.Sleep(1000);    // 每一秒广播一次
        }
        
    }
    private void OnApplicationQuit()
    {
        udpSocket.Close();
        isRun = false;
    }
    // Use this for initialization
    void Start()
    {
        CreateRoom();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
