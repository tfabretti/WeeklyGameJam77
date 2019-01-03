using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/EnemyStats")]
public class EnemyStats : AI_EnemyStats
{
	public float m_attackRange = 1f;
	public float m_attackRate = 1f;
}