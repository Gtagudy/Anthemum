using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void GoToCombat()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnToWorld()
    {
        SceneManager.LoadScene(0);
    }
}
