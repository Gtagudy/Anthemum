using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Events")]
public class VoidGameEvent : ScriptableObject
{
    private List<VoidListener> listeners = new List<VoidListener>();

    public void RegisterListener(VoidListener listener)
    {
        listeners.Add(listener);
    }

    public void unRegisterListeners(VoidListener listener)
    {
        listeners.Remove(listener);
    }

    public void ClearListeners()
    {
        listeners.Clear();
    }

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Raise();
        }
    }
}
