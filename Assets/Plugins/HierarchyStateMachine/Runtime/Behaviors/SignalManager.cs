using System;
using System.Collections.Generic;
using UnityEngine;
using EVENT = UltEvents.UltEvent;

namespace SLS.StateMachineH.V5
{
    [RequireComponent(typeof(StateMachine))]
    public class SignalManager : StateBehavior
    {
        public SignalSet globalSignals;

        public EVENT this[string name] => globalSignals[name];

        public bool FireSignal(string name)
        {
            if(Machine.CurrentState.TryGetComponent(out SignalNode signalNode) && signalNode.FireSignal(name)) return true;
            else if (globalSignals.ContainsKey(name))
            {
                globalSignals[name]?.InvokeSafe();
                return true;
            }
            return false;
            
        }


    }
}
