using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Util;
using UnityEngine;

namespace StateMachineScripts
{
    public class StateMachine : MonoBehaviour
    {
        private Dictionary<Type, BaseState> availableStates;
        public BaseState CurrentState { get; set; }

        public event Action<BaseState> StateChanged;

        public void Init(Type startingState, Dictionary<Type, BaseState> states)
        {
            Debug.Assert(states.Keys.Contains(startingState), "States dictionary does not contain the starting state!");

            SetStates(states);
            SwitchToNewState(startingState);
        }
        
        public void SetStates(Dictionary<Type, BaseState> states)
        {
            // TODO: What if new states dictionary does not contain the current state?
            availableStates = states;
        }

        private void Update()
        {
            if (availableStates.Count == 0)
            {
                return;
            }

            if (CurrentState == null)
            {
                return;
            }

            var nextState = CurrentState.Tick();

            if (nextState == null)
            {
                return;
            }

            if (nextState != CurrentState.GetType())
            {
                SwitchToNewState(nextState);
            }
        }

        public void SwitchToNewState(Type nextState)
        {
            CurrentState = availableStates[nextState];
            StateChanged?.Invoke(CurrentState);
            Debug.Log($"New state is {nextState}");
        }
    }
}
