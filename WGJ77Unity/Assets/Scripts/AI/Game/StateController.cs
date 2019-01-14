using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StateController : AI_StateController
{
    public EnemyStats m_gameEnemyStats;

	public DragonBones.UnityArmatureComponent m_animator = null;
	public CharacterMovements m_characterMovementsScript = null;

	[HideInInspector] public Transform m_chaseTarget;
	[HideInInspector] public Vector3 m_offsetChaseTarget;
	[HideInInspector] public Transform m_fleeTarget;
	[HideInInspector] public Vector3 m_offsetFleeTarget;

	[HideInInspector] public Collider[] m_collidersArrayForTests;


	protected override void Awake()
    {
		base.Awake();
		m_enemyStats = m_gameEnemyStats;
		m_collidersArrayForTests = new Collider[128];
		m_offsetChaseTarget = Vector3.zero;
		m_offsetFleeTarget = Vector3.zero;
	}


#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		//draw the cone of view
		Vector3 forward = m_agentEyesTransform.forward;
		if ( m_gameEnemyStats.m_vision.m_direction != 0 )
			forward = Quaternion.Euler( 0, m_gameEnemyStats.m_vision.m_direction, 0 ) * m_agentEyesTransform.forward;
		Vector3 endpoint = Quaternion.Euler( 0, m_gameEnemyStats.m_vision.m_angle * 0.5f, 0 ) * forward;

		Handles.color = new Color( 0, 1.0f, 0, 0.05f );
		Handles.DrawSolidArc( m_agentEyesTransform.position, -Vector3.up, endpoint.normalized, m_gameEnemyStats.m_vision.m_angle, m_gameEnemyStats.m_vision.m_range );

		Handles.color = new Color( 1.0f, 0, 0, 0.025f );
		Handles.DrawSolidDisc( transform.position, Vector3.up, m_gameEnemyStats.m_attackRange );
	}
#endif

}
