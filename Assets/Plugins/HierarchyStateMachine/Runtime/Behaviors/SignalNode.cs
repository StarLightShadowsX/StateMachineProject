using System;
using System.Collections.Generic;
using UnityEngine;
using EVENT = UltEvents.UltEvent;

namespace SLS.StateMachineH.V5
{
    [RequireComponent(typeof(IStateChild))]
    public class SignalNode : StateBehavior
    {
        public SignalSet signals;

        public EVENT this[string name] => signals[name];

        public bool FireSignal(string signalName)
        {
            if (signals.ContainsKey(signalName))
            {
                signals[signalName]?.InvokeSafe();
                return true;
            }
            else return false;
        }
    }

    public class SignalSet : Dictionary<string, EVENT> { }
}
