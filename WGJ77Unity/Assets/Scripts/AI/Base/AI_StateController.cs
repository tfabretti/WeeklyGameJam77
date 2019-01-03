using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AI_StateController : MonoBehaviour {

    public AI_State m_currentState;
    public AI_State m_remainState;

    public Transform m_agentEyesTransform;

    protected AI_EnemyStats m_enemyStats;
    protected bool m_isAIActive = false;

    [HideInInspector] public float m_stateTimeElapsed;
    [HideInInspector] public NavMeshAgent m_navMeshAgent;

    void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_isAIActive = true;
    }

    void Update()
    {
        if ( !m_isAIActive )
            return;

        m_currentState.UpdateState( this );
    }

    void OnDrawGizmos()
    {
        if ( m_currentState != null && m_agentEyesTransform != null && m_enemyStats != null )
        {
            Gizmos.color = m_currentState.m_sceneGizmoColor;
            Gizmos.DrawWireSphere( m_agentEyesTransform.position, m_enemyStats.m_visionDetectionSphereCastRadius );
        }
    }

    public void SetupAI( bool p_IsAIActive )
    {
        m_isAIActive = p_IsAIActive;
        if ( m_navMeshAgent )
            m_navMeshAgent.enabled = m_isAIActive;
    }

    public void TransitionToState( AI_State nextState )
    {
        if ( nextState != m_remainState )
        {
            m_currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed( float duration )
    {
        m_stateTimeElapsed += Time.deltaTime;
        return ( m_stateTimeElapsed >= duration );
    }

    private void OnExitState()
    {
        m_stateTimeElapsed = 0;
    }
}