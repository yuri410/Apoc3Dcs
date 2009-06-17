﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualBicycle.Input
{
    public class WirelessInputProcessor : InputProcessor, IDisposable
    {
        const char HandlebarDataTagC = 'H';
        const char WheelDataTagC = 'W';
        const char ResetDataTagC = 'R';
        const char ButtonDataTagC = 'S';
        const char DataRequestTagC = 'D';
        const char IdentifierTagC = 'V';

        enum DataType : int
        {
            Unknown = 0,
            HandlebarDataTag = (byte)HandlebarDataTagC,
            WheelDataTag = (byte)WheelDataTagC,
            ResetDataTag = (byte)ResetDataTagC,
            ButtonDataTag = (byte)ButtonDataTagC,
            DataRequestTag = (byte)DataRequestTagC
        }

        struct DataTrunk
        {
            public DataType type;

            public byte byte1;
            public byte byte2;

            public DataTrunk(byte a, byte b, byte c)
            {
                this.type = (DataType)a;
                this.byte1 = b;
                this.byte2 = c;
            }
        }

        public override void Update(float dt)
        {
            throw new NotImplementedException();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
