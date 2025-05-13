using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class EntityButton : MonoBehaviour
{
	[SerializeField] CombatEntity combatEntity;

	internal void UpdateEntity(CombatEntity combatEntity)
	{
		this.combatEntity = combatEntity;
		this.GetComponentInChildren<TextMeshProUGUI>().text = combatEntity.name;
	}
	public CombatEntity GetEntity()
	{
		return combatEntity;
	}
}
