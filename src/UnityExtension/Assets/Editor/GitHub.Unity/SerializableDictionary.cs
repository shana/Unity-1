using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (Name == "ChangesView")
            {
                LogHelper.Debug("Start OnBeforeSerialize Count:{0}", this.Count);
                LogHelper.Debug("Start OnBeforeSerialize keys:values {0}:{1}", keys.Count, values.Count);
            }

            keys.Clear();
            values.Clear();
            foreach (var pair in this)
            {
                if (Name == "ChangesView")
                {
                    LogHelper.Debug("Serialize Item: {0}", pair.Key);
                }

                keys.Add(pair.Key);
                values.Add(pair.Value);
            }

            if (Name == "ChangesView")
            {
                LogHelper.Debug("End OnBeforeSerialize Count:{0}", this.Count);
                LogHelper.Debug("End OnBeforeSerialize keys:values {0}:{1}", keys.Count, values.Count);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            if (Name == "ChangesView")
            {
                LogHelper.Debug("Start OnAfterDeserialize Count:{0}", this.Count);
                LogHelper.Debug("Start OnAfterDeserialize keys:values {0}:{1}", keys.Count, values.Count);
            }

            this.Clear();

            if (keys.Count != values.Count)
            {
                throw new SerializationException(
                    string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.",
                        keys.Count, values.Count));
            }

            for (var i = 0; i < keys.Count; i++)
            {
                this.Add(keys[i], values[i]);
            }

            if (Name == "ChangesView")
            {
                LogHelper.Debug("End OnAfterDeserialize Count:{0}", this.Count);
                LogHelper.Debug("End OnAfterDeserialize keys:values {0}:{1}", keys.Count, values.Count);
            }
        }
    }
}
