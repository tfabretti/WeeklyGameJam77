using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/EnemyStats")]
public class EnemyStats : AI_EnemyStats
{
	public float moveSpeed = 1;

	public float attackRange = 1f;
	public float attackRate = 1f;
	public float attackForce = 15f;
	public int attackDamage = 50;

	public float searchDuration = 4f;
	public float searchingTurnSpeed = 120f;
}