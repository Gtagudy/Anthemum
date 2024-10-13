using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Int Events")]
public class IntGameEvent : ScriptableObject
{
    public UnityAction<int> actionRaise;
    public void Raise(int value)
    {
        actionRaise?.Invoke(value);
    }
}
