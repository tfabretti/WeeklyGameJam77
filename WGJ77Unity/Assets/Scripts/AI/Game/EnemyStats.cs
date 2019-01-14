using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/EnemyStats")]
public class EnemyStats : AI_EnemyStats
{
	public float m_attackRange = 1f;
	public float m_attackRate = 1f;

	[Tooltip( "Used to find a target to steer to when looking for an attack target" )]
	public float m_hiddenTargetAcquisitionRange = 100f;
	[Tooltip( "Used to steer to the hidden target with an angle error" )]
	public float m_hiddenTargetDirectionAngleError = 45f;
	[Tooltip( "Used to readjust the trajectory to the hidden taget" )]
	public float m_hiddenTargetReadjustmentDelay = 3f;

	[System.Serializable]
	public struct FleeParam
	{
		public float m_fleeAngle;
	}
	public FleeParam m_fleeParam;
}