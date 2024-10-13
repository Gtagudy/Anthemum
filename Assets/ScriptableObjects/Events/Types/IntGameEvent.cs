using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Int Events")]
public class IntGameEvent : ScriptableObject
{
    private List<IntListener> listeners = new List<IntListener>();

    public void RegisterListeners(IntListener listener)
    {
        listeners.Add(listener);
    }

    public void unRegisterListeners(IntListener listener)
    {
        listeners.Remove(listener);
    }

    public void ClearListeners()
    {
        listeners.Clear();
    }

    public void Raise(int value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Raise(value);
        }
    }
}
