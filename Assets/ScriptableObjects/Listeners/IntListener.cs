﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class IntListener : MonoBehaviour
{
	public UnityAction<int> intReact;

	public void Raise(int value)
	{
		intReact?.Invoke(value);
	}
}