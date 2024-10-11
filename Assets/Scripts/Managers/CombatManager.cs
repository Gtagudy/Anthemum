using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

	public void StartCombat(EntitySO[] players, EntitySO[] enemies)
	{
        TurnManager turnManager = new TurnManager();
        turnManager.QueueEntities(players, enemies);
	}



	// Start is called before the first frame update
	void Start()
    {
        //actionManager = GetComponent<ActionManager>();
        //entityManager = GetComponent<EntityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
