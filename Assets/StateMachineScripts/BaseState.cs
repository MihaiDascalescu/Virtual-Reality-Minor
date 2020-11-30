using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineScripts
{
    public abstract class BaseState
    {
        public abstract Type Tick();
    }
}
