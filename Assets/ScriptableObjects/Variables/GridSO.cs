using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GridSO
{
	[SerializeField] EntitySO entityHere;
	[SerializeField] bool obstructionHere;

	[SerializeField] GameObject GameObject;

	[SerializeField] Tuple<int, int> gridPosition;
}
