using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GitHub.Logging;
using UnityEngine;

namespace GitHub.Unity
{
    //http://answers.unity3d.com/answers/809221/view.html

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();
        [SerializeField] public string Name;
        [SerializeField] public string id = Guid.NewGuid().ToString();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            if (Name == "folders")
            {
                LogHelper.Debug("OnBeforeDeserialize: {0}", id);
            }

            keys.Clear();
            values.Clear();
            foreach (var pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            if (Name == "folders")
            {
                LogHelper.Debug("OnAfterDeserialize: {0}", id);
            }

            Clear();

            if (keys.Count != values.Count)
            {
                throw new SerializationException(
                    string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.",
                        keys.Count, values.Count));
            }

            for (var i = 0; i < keys.Count; i++)
            {
                Add(keys[i], values[i]);
            }
        }
    }
}
