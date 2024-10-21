using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EntityButton : MonoBehaviour
{
	[SerializeField] CombatEntity combatEntity;

	internal void UpdateEntity(CombatEntity combatEntity)
	{
		this.combatEntity = combatEntity;
	}
	public CombatEntity GetEntity()
	{
		return combatEntity;
	}
}
