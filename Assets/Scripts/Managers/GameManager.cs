using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    /*
        The point of the GameManager here is to, of course, control the game. We will use this class to handle
    how the world will work, including handing things over to the CombatManager. We will be communicating with the UI Manager here
    as well.
    
    */
    public GameState gameState = GameState.Title;

    [SerializeField] CombatSceneSO[] CombatScenes;
    [SerializeField] CombatSceneSO[] OriginalScenes;

    [SerializeField] CombatSceneSO currentCombatScene;

    public int[,] grid = new int[5,5];
    CombatSceneSO gameScene;

    CombatManager combatManager;
    UIManager uiManager;
    ChangeScene change;

    [SerializeField] public CombatEntity MainCharacter;
    [SerializeField] public CombatEntity MainEnemy;

    //[SerializeField]

    [SerializeField] private Image BlackScreen;

	private bool startedCombat = false;
    private bool inTitleScreen = false;
    private bool inWorld = false;

    //float storedSpeed;
    int runs = 0;

    [SerializeField] public AudioClip[] clickSounds;
    [SerializeField] public AudioSource chosenClick;

    // Start is called before the first frame update
    void Awake()
    {
        combatManager = GetComponent<CombatManager>();
        uiManager = GetComponent<UIManager>();

        CombatScenes = OriginalScenes;
        change = GetComponent<ChangeScene>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameState)
        {
            case GameState.Title:
                if(!inTitleScreen)
                {
                    inTitleScreen = true;
                    Debug.Log("Title");
                    //uiManager.PlayTitle(TitleText, StartBTN, QuitBTN);
                }
                break;

            case GameState.World:

				startedCombat = false;
                if (!inWorld)
                {
                    //MainCharacter.GetComponent<Character>().UpdateSpeed(storedSpeed);
                    inWorld = true;
                    MainCharacter.gameObject.SetActive(true);
                }

                break;

            case GameState.Pause:
				MainCharacter.GetComponent<Character>().UpdateSpeed(0);
                break;

			case GameState.Combat:

				if (!startedCombat)
                {
				    MainCharacter.GetComponent<Character>().UpdateSpeed(0);
                    inWorld = false;
                    runs++;

                    startedCombat = true;
                    combatManager.StartCombat(currentCombatScene);

                    /*int chosenLevel = UnityEngine.Random.Range(0, CombatScenes.Length - 1);

					if (CombatScenes[chosenLevel] != null)
                    {
                        Debug.Log("Combat!");
                        CombatSceneSO chosenScene = CombatScenes[chosenLevel];
                        

                        combatManager.StartCombat(chosenScene);
                    } else
                    {
                        chosenLevel = UnityEngine.Random.Range(0, CombatScenes.Length - 1);
                    }*/
                }
                break;
        }
    }

    public void PauseGame()
    {

    }


	public void ExitGame()
    {
        if(Application.isEditor)
        {
            EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
    }
    public AudioClip[] GetClickSounds()
    {
        return clickSounds;
    }
    public void StartCombat(CombatSceneSO combatSceneSO)
    {
        currentCombatScene = combatSceneSO;

        gameState = GameState.Combat;
    }
}
