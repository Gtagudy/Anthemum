using System.Collections.Generic;
using UnityEngine;

public class AbilityUIManager : MonoBehaviour
{
    [SerializeField] private Transform buttonContainer;     // Parent UI Transform for buttons
    [SerializeField] private AbilityButton buttonPrefab;    // Prefab for AbilityButton
    [SerializeField] private ActionManager actionManager;   // Reference to your ActionManager

    private List<AbilityButton> activeButtons = new();

    /// <summary>
    /// Creates and initializes ability buttons for the given player's abilities.
    /// </summary>
    public void CreateMoves(CombatEntity player, List<AbilitySO> playerAbilities)
    {
        ClearButtons();

        foreach (AbilitySO ability in playerAbilities)
        {
            AbilityButton newButton = Instantiate(buttonPrefab, buttonContainer);
            newButton.Setup(player, ability, actionManager);
            activeButtons.Add(newButton);
        }
    }

    /// <summary>
    /// Removes all active ability buttons.
    /// </summary>
    public void ClearButtons()
    {
        foreach (var btn in activeButtons)
        {
            if (btn != null)
                Destroy(btn.gameObject);
        }
        activeButtons.Clear();
    }
}