﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualBicycle.IO
{
    public interface IAbstractFactory<T, S> where S : ResourceLocation
    {
        T CreateInstance(string file);
        T CreateInstance(S fl);

        string Type { get; }
    }
}
