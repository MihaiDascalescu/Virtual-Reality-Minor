using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineScripts
{
    public abstract class BaseState
    {
        public virtual void OnEnterState() {}
        public abstract Type Tick();
        public virtual void OnExitState() {}
    }
}
