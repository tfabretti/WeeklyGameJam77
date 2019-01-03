using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_EnemyStats : ScriptableObject
{
    public float m_moveSpeed = 1;

    public float m_visionDetectionRange = 40f;
	public float m_visionDetectionSphereCastRadius = 1f;
}