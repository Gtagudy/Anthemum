using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
	//The game event we will keep an eye on
	public GameEvent Event;

	//What we do because the game event is raised
	public UnityEvent Response;

	public void OnEnable()
	{
		Event.RegisterListener(this);
	}
	private void OnDisable()
	{
		Event.unRegisterListeners(this);
	}
	public void OnEventRaise()
	{
		Response.Invoke();
	}
}
