using System;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural.Scripts.DataStructure
{
  [Serializable]
  public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
  {
    [SerializeField] private List<TKey> keys = new();
    [SerializeField] private List<TValue> values = new();

    public void OnBeforeSerialize()
    {
      keys.Clear();
      values.Clear();

      foreach (var kvp in this)
      {
        keys.Add(kvp.Key);
        values.Add(kvp.Value);
      }
    }

    public void OnAfterDeserialize()
    {
      Clear();

      for (var i = 0; i != Math.Min(keys.Count, values.Count); i++)
      {
        Add(keys[i], values[i]);
      }
    }
  }
}