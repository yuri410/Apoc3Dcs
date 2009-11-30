using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Network
{
    /// <summary>
    ///  �����������¼���ί������
    ///  A callback/delegate function for server connection event
    /// </summary>
    public delegate void HandleServerConnection();

    /// <summary>
    ///  �������Ͽ������¼���ί������
    ///  A callback/delegate function for server disconnection event
    /// </summary>
    public delegate void HandleServerDisconnection();

    /// <summary>
    ///  ��ʾ�ͻ���
    ///  An interface that defines the properties and methods of a network client.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        ///  ��ȡ�����������ݵĶ˿ں�
        ///  Gets the port number used to transfer messages.
        /// </summary>
        int PortNumber { get;}

        /// <summary>
        ///  ��ȡ�ÿͻ��˵�IP��ַ
        ///  Gets the address of this client in 4 bytes.
        /// </summary>
        /// <remarks>
        /// It returns 4 bytes instead of String in order to optimize network transfer.
        /// (e.g., 192.168.0.1 will be byte[0] = (byte)192, byte[1] = (byte)168, byte[2] = (byte)0,
        /// byte[3] = (byte)1)
        /// </remarks>
        byte[] MyIPAddress { get; }

        /// <summary>
        ///  ��ȡ�������Ƿ���Ҫ���ô������
        ///  Gets or sets whether to enable encryption.
        /// </summary>
        bool EnableEncryption { get; set; }

        /// <summary>
        ///  �ͻ������ӵ����������¼�
        ///  Adds or removes an event handler for client connection event.
        /// </summary>
        event HandleServerConnection ServerConnected;

        /// <summary>
        ///  �ͻ��˴ӷ������Ͽ����¼�
        ///  Adds or removes an event handler for client disconnection event.
        /// </summary>
        event HandleServerDisconnection ServerDisconnected;

        /// <summary>
        ///  ��ȡ�ÿͻ����Ƿ��Ѿ����ӵ��˷�����
        ///  Gets whether this client is connected to a server.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        ///  
        ///  Gets or sets whether to wait for the server to start up in case the server is 
        ///  not running when client tried to connect. To set the timeout for connection
        ///  trial, set the ConnectionTrialTimeOut property. Default value is false.
        /// </summary>
        /// <see cref="ConnectionTrialTimeOut"/>
        bool WaitForServer { get; set; }

        /// <summary>
        ///  
        ///  Gets or sets the timeout in milliseconds for connection trial when the server was
        ///  not up when client was started. This property is only effective if WaitForServer
        ///  is set to true. Default value is -1 which means the client waits for infinite time. 
        /// </summary>
        /// <see cref="WaitForServer"/>
        int ConnectionTrialTimeOut { get; set; }

        /// <summary>
        ///  ͨ�����������ӵ�������
        ///  Connects to the server specified by HostName.
        /// </summary>
        void Connect();

        /// <summary>
        ///  �ӷ�����������Ϣ
        ///  Receives messages from the server.
        /// </summary>
        /// <returns>A list of received messages in array of bytes</returns>
        List<byte[]> ReceiveMessage();

        /// <summary>
        ///  �������������Ϣ
        ///  Sends a message to the server.
        /// </summary>
        /// <param name="msg">The message to be sent</param>
        /// <param name="reliable">Whether the message is guaranteed to arrive at the
        /// receiver side</param>
        /// <param name="inOrder">Whether the message arrives in order at the receiver side</param>
        void SendMessage(byte[] msg, bool reliable, bool inOrder);

        /// <summary>
        ///  �رտͻ���
        ///  Shuts down the client.
        /// </summary>
        void Shutdown();
    }
}
