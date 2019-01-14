using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Actions/Wander" )]
public class ActionWander : AI_Action
{
    public override void Act( AI_StateController p_controller )
    {
        Wander( p_controller as StateController );
    }

	// Wanders in a random direction while moving toward the chase target (updates offsetChase) 
	private void Wander( StateController p_controller )
    {
		EnemyStats gameStats = p_controller.m_gameEnemyStats;

		// Should the direction be readjusted ?
		float currentCycle = Mathf.Repeat( p_controller.m_stateTimeElapsed, gameStats.m_hiddenTargetReadjustmentDelay );
		float previousCycle = Mathf.Repeat( p_controller.m_stateTimeElapsed - Time.deltaTime, gameStats.m_hiddenTargetReadjustmentDelay );
		// If the delay is up : readjust
		if ( p_controller.m_stateTimeElapsed == Time.deltaTime || previousCycle > currentCycle )
		{
			Vector3 forward = p_controller.m_navMeshAgent.velocity;
			if ( forward == Vector3.zero )
			{
				float randomDirectionAngle = Random.Range( 0f, 360f );
				forward = Quaternion.Euler( 0, randomDirectionAngle, 0 ) * Vector3.forward;
			}
			else
				forward = forward.normalized;

			float fRandomAngle = Random.Range( -135f, 135f );
			Vector3 wantedDirection = Quaternion.Euler( 0, fRandomAngle, 0 ) * forward * gameStats.m_hiddenTargetAcquisitionRange;
			p_controller.m_navMeshAgent.destination = p_controller.transform.position + wantedDirection;
			p_controller.m_navMeshAgent.speed = p_controller.m_gameEnemyStats.m_moveSpeed;
			p_controller.m_navMeshAgent.isStopped = false;
		}
	}

}
