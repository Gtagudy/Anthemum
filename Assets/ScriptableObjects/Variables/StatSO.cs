using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObject/Stats")]
public class StatSO : ScriptableObject
{
	public int health;
	[SerializeField] int originalHealth;

	public int attack;

	public int defense;

	[SerializeField] public int speed;
}