using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatEntity : MonoBehaviour
{
    public IntGameEvent healthChange;

    [SerializeField] EntitySO entity;
	
    [SerializeField] Scrollbar Health;
    
    [SerializeField] GameObject myTurn;
	[SerializeField] GameObject MoveListDisplay;
	[SerializeField] GameObject enemyTargetDisplay;
	[SerializeField] GameObject playerTargetDisplay;
	private bool isMyTurn = false;

	// Start is called before the first frame update
	void Awake()
    {
        //Health = entity.GetHealthBar();

        MoveListDisplay = GameObject.FindGameObjectWithTag("MovePanel");
	}

    // Update is called once per frame
    void Update()
    {
        if(isMyTurn)
        {
            myTurn.SetActive(true);
        }
        else
        {
            myTurn.SetActive(false);
        }
    }

    internal GameObject GetMovesDisplay()
    {
        return MoveListDisplay.GetComponent<GameObject>();
    }
    internal EntitySO GetEntitySO()
    {
        return entity;
    }
}
