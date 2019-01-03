using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : AI_StateController
{
	public EnemyStats m_gameEnemyStats;

    public Transform m_chaseTarget;

    [HideInInspector] public NavMeshAgent navMeshAgent;
	[HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;

	void Awake () 
	{
        m_enemyStats = m_gameEnemyStats;
		navMeshAgent = GetComponent<NavMeshAgent> ();
        m_isAIActive = true;

    }

	public void SetupAI(bool p_IsAIActive)
	{
		m_isAIActive = p_IsAIActive;
        if ( navMeshAgent )
            navMeshAgent.enabled = m_isAIActive;
	}
}