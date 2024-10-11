using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TurnStart()
    {

    }

    void TurnEnd()
    {

    }

    void NextTurn()
    {

    }

	private void Reset()
	{
		
	}

	public void QueueEntities(EntitySO[] players, EntitySO[] enemies)
	{
        EntitySO[] tempOrder = new EntitySO[players.Length + enemies.Length];

        foreach (EntitySO entity in players)
        {
            if (entity != null)
            {
                 tempOrder = tempOrder.OrderByDescending((entity) => entity.GetSpeed()).ToArray();
            }
        }
	}
}
