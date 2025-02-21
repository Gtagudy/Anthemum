using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Animator animator;

	private bool readyToMove = false;

	public void FadeToMainWorld()
	{
		animator.SetTrigger("FadeOut");
	}	

	public void OnFadeComplete()
	{
		SceneManager.LoadScene(0);
	}
}
