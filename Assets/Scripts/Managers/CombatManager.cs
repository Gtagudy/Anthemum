using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    TurnManager turnManager;
	public void StartCombat(EntitySO[] players, EntitySO[] enemies)
	{
        Debug.Log("We now starting combat!");
        turnManager.QueueEntities(players, enemies);

	}

    


	// Start is called before the first frame update
	void Awake()
    {
        turnManager = GetComponent<TurnManager>();
        //actionManager = GetComponent<ActionManager>();
        //entityManager = GetComponent<EntityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
