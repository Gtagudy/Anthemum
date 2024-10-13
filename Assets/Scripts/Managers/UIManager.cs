using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public IntGameEvent changeHealth;
    /*
    The UI Manage is a manager made along with the GameManager. The UI will even begin at Title,
    working throughout the game in both the World and the Combat
     */

    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject CommandPanel;

    [SerializeField] TextMeshProUGUI StateMachine;
    [SerializeField] TextMeshProUGUI EntityTurn;
    [SerializeField] Button SkipButton;

    [SerializeField] Scrollbar[] healthBars;

	internal void CreateHealthBars(EntitySO entity)
	{
        /*entity.GetTransform();
        Instantiate(healthBars[0]);
        healthBars[]*/
	}

	void Awake()
    {
        changeHealth.RegisterListeners(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
