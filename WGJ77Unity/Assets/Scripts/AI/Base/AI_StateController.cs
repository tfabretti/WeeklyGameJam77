using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AI_StateController : MonoBehaviour {

    public AI_State m_currentState;

    public Transform m_agentEyesTransform;

    protected AI_EnemyStats m_enemyStats;
    protected bool m_isAIActive;

    [HideInInspector] public float m_stateTimeElapsed;
    [HideInInspector] public float m_transitionsDelayCoefficient; // Used to get a delay betwen min and max : min + (max - min) * coef
    [HideInInspector] public NavMeshAgent m_navMeshAgent;

    protected virtual void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
		SetupAI( true );
	}

	private void Start()
	{
		OnEnterState();
	}

	void Update()
    {
        if ( !m_isAIActive )
            return;

		m_stateTimeElapsed += Time.deltaTime;
		m_currentState.UpdateState( this );
    }

    void OnDrawGizmos()
    {
        if ( m_currentState != null && m_agentEyesTransform != null && m_enemyStats != null )
        {
            Gizmos.color = m_currentState.m_sceneGizmoColor;
            Gizmos.DrawWireSphere( m_agentEyesTransform.position, m_enemyStats.m_vision.m_sphereCastRadius );
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
        if ( nextState != m_currentState )
        {
			OnExitState();
			m_currentState = nextState;
			OnEnterState();
		}
    }

    public bool CheckIfCountDownElapsed( float duration )
    {
        return m_stateTimeElapsed >= duration;
    }

    private void OnEnterState()
    {
		m_transitionsDelayCoefficient = Random.Range( 0.0f, 1.0f );
		m_currentState.EnterState( this );
	}

	private void OnExitState()
	{
		m_currentState.ExitState( this );
		m_stateTimeElapsed = 0;
	}
}