using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : AI_StateController
{
    public EnemyStats m_gameEnemyStats;

    [HideInInspector] public Transform m_chaseTarget;
    [HideInInspector] public Transform m_fleeTarget;

    void Awake()
    {
        m_enemyStats = m_gameEnemyStats;
    }
}