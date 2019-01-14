using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Actions/LookForATarget" )]
public class ActionLookForATarget : AI_Action
{
    public override void Act( AI_StateController p_controller )
    {
        Search( p_controller as StateController );
    }

	// Wanders in a random direction while moving toward the chase target (updates offsetChase) 
	private void Search( StateController p_controller )
    {
		EnemyStats gameStats = p_controller.m_gameEnemyStats;

		// Should the direction be readjusted ?
		float currentCycle = Mathf.Repeat( p_controller.m_stateTimeElapsed, gameStats.m_hiddenTargetReadjustmentDelay );
		float previousCycle = Mathf.Repeat( p_controller.m_stateTimeElapsed - Time.deltaTime, gameStats.m_hiddenTargetReadjustmentDelay );
		// If the delay is up : readjust
		if ( previousCycle > currentCycle )
		{
			float fRandomAngle = Random.Range( -gameStats.m_hiddenTargetDirectionAngleError, gameStats.m_hiddenTargetDirectionAngleError );
			if ( p_controller.m_chaseTarget != null )
			{
				Vector3 chaseTargetDirection = p_controller.m_chaseTarget.position - p_controller.transform.position;
				Vector3 wantedDirection = Quaternion.Euler( 0, fRandomAngle, 0 ) * chaseTargetDirection;
				Vector3 wantedDestination = p_controller.m_chaseTarget.position + wantedDirection;
				p_controller.m_offsetChaseTarget = wantedDestination - p_controller.m_chaseTarget.position;
			}
			else
			{
				Vector3 wantedDirection = Quaternion.Euler( 0, fRandomAngle, 0 ) * p_controller.m_navMeshAgent.velocity.normalized * gameStats.m_hiddenTargetAcquisitionRange;
				p_controller.m_navMeshAgent.destination = p_controller.transform.position + wantedDirection;
				p_controller.m_navMeshAgent.speed = p_controller.m_gameEnemyStats.m_moveSpeed;
				p_controller.m_navMeshAgent.isStopped = false;
			}
		}

		if ( p_controller.m_chaseTarget != null )
		{
			p_controller.m_navMeshAgent.destination = p_controller.m_chaseTarget.position + p_controller.m_offsetChaseTarget;
			p_controller.m_navMeshAgent.speed = p_controller.m_gameEnemyStats.m_moveSpeed;
			p_controller.m_navMeshAgent.isStopped = false;
		}
	}

}
