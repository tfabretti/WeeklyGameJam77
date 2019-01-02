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

    void Update()
    {
        if ( !m_isAIActive )
            return;

        m_currentState.UpdateState( this );
    }

    void OnDrawGizmos()
    {
        if ( m_currentState != null && m_agentEyesTransform != null )
        {
            Gizmos.color = m_currentState.m_sceneGizmoColor;
            Gizmos.DrawWireSphere( m_agentEyesTransform.position, m_enemyStats.lookSphereCastRadius );
        }
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