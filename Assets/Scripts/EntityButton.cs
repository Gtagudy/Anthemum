using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EntityButton : MonoBehaviour
{
	[SerializeField] EntitySO entitySO;

	internal void UpdateEntity(EntitySO entitySO)
	{
		this.entitySO = entitySO;
	}
	internal EntitySO GetEntity()
	{
		return entitySO;
	}
}
