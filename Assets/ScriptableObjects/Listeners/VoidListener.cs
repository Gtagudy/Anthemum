using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoidListener : MonoBehaviour
{
    public UnityEvent react;
	public void Raise()
	{
		react?.Invoke();
	}
}
