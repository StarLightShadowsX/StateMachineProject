using UnityEngine;

#if ULT_EVENTS
using EVENT = UltEvents.UltEvent;
#else
using EVENT = UnityEngine.Events.UnityEvent;
#endif


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
                signals[signalName]?.Invoke();
                return true;
            }
            else return false;
        }
    }
}
