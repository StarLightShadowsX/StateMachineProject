using System;
using System.Collections.Generic;
using UnityEngine;

#if ULT_EVENTS
using EVENT = UltEvents.UltEvent;
#else
using EVENT = UnityEngine.Events.UnityEvent;
#endif

namespace SLS.StateMachineH.V5
{
    [RequireComponent(typeof(StateMachine))]
    public class SignalManager : StateBehavior
    {
        public SignalSet globalSignals = new();

        public EVENT this[string name] => globalSignals[name];

        public bool FireSignal(string name)
        {
            if(Machine.CurrentState.TryGetComponent(out SignalNode signalNode) && signalNode.FireSignal(name)) return true;
            else if (globalSignals.ContainsKey(name))
            {
                globalSignals[name]?.Invoke();
                return true;
            }
            return false;
            
        }


    }
}
