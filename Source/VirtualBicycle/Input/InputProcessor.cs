﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualBicycle.Input
{
    public abstract class InputProcessor
    {
        public InputManager Manager
        {
            get;
            private set;
        }

        protected InputProcessor(InputManager mgr) 
        {
            Manager = mgr;
        }

        public abstract void Update(float dt);
    }
}
