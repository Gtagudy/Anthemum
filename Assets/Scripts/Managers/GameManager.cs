using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
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

    public int[,] grid = new int[5,5];
    CombatSceneSO gameScene;

    CombatManager combatManager;
    UIManager uiManager;
    ChangeScene change;

    [SerializeField] public CombatEntity MainCharacter;
    [SerializeField] public CombatEntity MainEnemy;

	[SerializeField] public TextMeshProUGUI TitleText;
	[SerializeField] public Button StartBTN;
	[SerializeField] public Button QuitBTN;
	[SerializeField] public Canvas TitleUI;

    [SerializeField] private Image BlackScreen;

	private bool startedCombat = false;
    private bool inTitleScreen = false;
    private bool inWorld = false;

    float storedSpeed;
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
                if(runs >= 5)
                {
                    gameState = GameState.Title; break;
                }
                if (!inWorld)
                {
                    MainCharacter.GetComponent<Character>().UpdateSpeed(storedSpeed);
                    inWorld = true;
                    MainCharacter.gameObject.SetActive(true);
                }

                break;

            case GameState.Pause:

            case GameState.Combat:
                if (!startedCombat)
                {
                    inWorld = false;
                    runs++;
                    int chosenLevel = UnityEngine.Random.Range(0, CombatScenes.Length - 1);

					if (CombatScenes[chosenLevel] != null)
                    {
                        startedCombat = true;
                        Debug.Log("Combat!");
                        CombatSceneSO chosenScene = CombatScenes[chosenLevel];
                        

                        combatManager.StartCombat(chosenScene);
                    } else
                    {
                        chosenLevel = UnityEngine.Random.Range(0, CombatScenes.Length - 1);
                    }
                }
                break;
        }
    }

	public void MoveToWorld()
	{
        if(clickSounds.Length > 0)
		{
			chosenClick.clip = clickSounds[UnityEngine.Random.Range(0, clickSounds.Length - 1)];
			chosenClick.Play();
			gameState = GameState.World;
			TitleUI.gameObject.SetActive(false);

		}
		//TitleUI.GetComponent<TMP_EditorPanelUI>
	}

	public void Joever()
    {
        if (clickSounds.Length > 0)
        {
            chosenClick.clip = clickSounds[UnityEngine.Random.Range(0, clickSounds.Length)];
            chosenClick.Play();

            Application.Quit();
        }
    }
    public AudioClip[] GetClickSounds()
    {
        return clickSounds;
    }
    public void StartCombat()
    {
        
        gameState = GameState.Combat;
    }
}
