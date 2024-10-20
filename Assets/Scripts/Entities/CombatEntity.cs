using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatEntity : MonoBehaviour
{
    [SerializeField] EntitySO entity;
    public IntGameEvent healthChange;
	[SerializeField] Scrollbar Health;
    [SerializeField] GameObject myTurn;
    private bool isMyTurn = false;

	// Start is called before the first frame update
	void Awake()
    {
        
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


}
