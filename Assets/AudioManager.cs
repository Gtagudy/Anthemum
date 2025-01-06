using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource ambience;

	private void Start()
	{
		if(backgroundMusic != null)
			backgroundMusic.Play();
		if(ambience != null)
			ambience.Play();
	}
}
