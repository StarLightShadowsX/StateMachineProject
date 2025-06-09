using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ULT_EVENTS
using EVENT = UltEvents.UltEvent;
#else
using EVENT = UnityEngine.Events.UnityEvent;
#endif

namespace SLS.StateMachineH.V5
{
    [Serializable]
    public class SignalSet : Dictionary<string, EVENT>, ISerializationCallbackReceiver
    {
        [SerializeField] internal List<KeyValuePair> serializedList = new();

        [Serializable]
        public struct KeyValuePair
        {
            public string key;
            public EVENT value;
            public KeyValuePair(string key, EVENT value)
            {
                this.key = key;
                this.value = value;
            }
        }


        public SignalSet() : base()
        {
            serializedList = new();
        }
        public SignalSet(int capacity) : base(capacity)
        {
            serializedList = new(capacity);
        }

        public void OnBeforeSerialize()
        {
            serializedList.Clear();
            foreach (var kvp in this)           
                serializedList.Add(new (kvp.Key, kvp.Value));
        }

        public void OnAfterDeserialize()
        {
            base.Clear();
            HashSet<string> uniqueNames = new();

            for (int i = 0; i < serializedList.Count; i++)
            {
                if (!string.IsNullOrEmpty(serializedList[i].key) && serializedList[i].value != null && !uniqueNames.Contains(serializedList[i].key))
                {
                    base.Add(serializedList[i].key, serializedList[i].value);
                    uniqueNames.Add(serializedList[i].key);
                }
                else
                {
                    Debug.LogWarning($"SignalSet deserialization warning: Invalid key or event at index {i}.");
                }
            }
        }

        public new EVENT this[string key]
        {
            get => base[key];
            set
            {
                base[key] = value;
                bool anyEntryWasFound = false;
                for (int i = 0; i < serializedList.Count; i++)
                {
                    KeyValuePair kvp = serializedList[i];
                    if (!((object)key == kvp.key || key.Equals(kvp.key))) continue;
                    anyEntryWasFound = true;
                    kvp.value = value;
                    serializedList[i] = kvp;
                }

                if (!anyEntryWasFound)
                    serializedList.Add(new(key, value));
                OnBeforeSerialize();
            }
        }

        public new void Add(string key, EVENT value)
        {
            base.Add(key, value);
            serializedList.Add(new(key, value));
            OnAfterDeserialize();
        }

        public void AddNew()
        {
            base.Add("New", new());
            serializedList.Add(new("New", new()));
            OnAfterDeserialize();
        }


        public new void Clear()
        {
            base.Clear();
            serializedList.Clear();
            OnAfterDeserialize();
        }

        public new bool Remove(string key)
        {
            if (TryGetValue(key, out var value))
            {
                base.Remove(key);
                serializedList.Remove(new(key, value));
                OnAfterDeserialize();
                return true;
            }

            return false;
        }

        public new bool TryAdd(string key, EVENT value)
        {
            if (base.TryAdd(key, value))
            {
                serializedList.Add(new(key, value));
                OnAfterDeserialize();
                return true;
            }

            return false;
        }

    }
}
