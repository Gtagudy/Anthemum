using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    ActionManager actionManager;
    UIManager uiManager;
	public void CheckEntity(EntitySO dequeue)
	{
		if(dequeue != null)
        {
            if (dequeue.isPlayer == true)
            {

                Debug.Log("Welcome player!");
                actionManager.ResolvePlayer(dequeue);
            }
            else
            {
                Debug.Log("Ohh your the enemy!!");
                actionManager.ResolveEnemy(dequeue);
            }
        }
	}

	// Start is called before the first frame update
	void Awake()
    {
        actionManager = GetComponent<ActionManager>();
        uiManager = GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
