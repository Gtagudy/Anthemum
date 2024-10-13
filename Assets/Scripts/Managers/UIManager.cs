using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /*
    The UI Manage is a manager made along with the GameManager. The UI will even begin at Title,
    working throughout the game in both the World and the Combat
     */

    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject CommandPanel;

    [SerializeField] TextMeshProUGUI StateMachine;
    [SerializeField] TextMeshProUGUI EntityTurn;
    [SerializeField] Button SkipButton;
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
