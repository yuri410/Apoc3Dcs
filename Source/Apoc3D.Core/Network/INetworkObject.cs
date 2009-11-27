using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualBicycle.Network
{
    /// <summary>
    ///  ��ʾһ�������������Ϸ��͵Ķ���
    ///  An interface that defines the properties and methods of an object that can be sent 
    ///  over the network.
    /// </summary>
    public interface INetworkObject
    {
        /// <summary>
        ///  ��ȡ�����������ΨһID
        ///  Gets an identifier of this network object (has to be unique).
        /// </summary>
        String Identifier { get; }

        /// <summary>
        /// Gets or sets whether this network object is ready to be sent. This variable
        /// can be used for optimization to not send any objects that did not change.
        /// </summary>
        bool ReadyToSend { get; set; }

        /// <summary>
        ///  
        ///  Gets or sets whether to hold the information to be transferred.
        /// </summary>
        bool Hold { get; set; }

        /// <summary>
        /// Gets or sets whether the receiver is guaranteed to receive the information
        /// </summary>
        bool Reliable { get; set; }

        /// <summary>
        /// Gets or sets whether the receiver will receive the information in the order
        /// sent by the sender. 
        /// </summary>
        bool Ordered { get; set; }

        /// <summary>
        /// Gets or sets the frequency to send information in terms of Hz. For example,
        /// 2 Hz means send twice per second, and 60 Hz means send 60 times per second.
        /// </summary>
        int SendFrequencyInHertz { get; set; }

        /// <summary>
        /// Gets all of the information that needs to be sent over the network.
        /// </summary>
        /// <returns></returns>
        byte[] GetMessage();

        /// <summary>
        /// Interprets the information associated with this object received over the network.
        /// </summary>
        /// <param name="msg"></param>
        void InterpretMessage(byte[] msg);
    }
}
