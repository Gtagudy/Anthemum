using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    /*
        The point of the GameManager here is to, of course, control the game. We will use this class to handle
    how the world will work, including handing things over to the CombatManager. We will be communicating with the UI Manager here
    as well.
    
    */
    GameState gameState = GameState.Combat;

    [SerializeField] CombatEntity[] Players;
    [SerializeField] CombatEntity[] Enemies;

    CombatManager combatManager;

    private bool startedCombat = false;

    // Start is called before the first frame update
    void Awake()
    {
        combatManager = GetComponent<CombatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameState)
        {
            case GameState.Combat:
                if (!startedCombat)
                {
                    startedCombat = true;
                    Debug.Log("Combat!");
                    combatManager.StartCombat(Players, Enemies);
                }
                break;
        }
    }
}
