using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Actions/Flee" )]
public class ActionFlee : AI_Action
{
    public override void Act( AI_StateController p_controller )
    {
		Flee( p_controller as StateController );
    }

    private void Flee( StateController p_controller )
    {
		if ( p_controller.m_fleeTarget == null )
			return;

		EnemyStats gameStats = p_controller.m_gameEnemyStats;

		// Should the direction be readjusted ?
		float currentCycle = Mathf.Repeat( p_controller.m_stateTimeElapsed, gameStats.m_hiddenTargetReadjustmentDelay );
		float previousCycle = Mathf.Repeat( p_controller.m_stateTimeElapsed - Time.deltaTime, gameStats.m_hiddenTargetReadjustmentDelay );
		// If the delay is up : readjust
		if ( p_controller.m_stateTimeElapsed == Time.deltaTime || previousCycle > currentCycle )
		{
			float fRandomAngle = Random.Range( -gameStats.m_hiddenTargetDirectionAngleError, gameStats.m_hiddenTargetDirectionAngleError );
			Vector3 targetToAgent = p_controller.transform.position - p_controller.m_fleeTarget.position;
			Vector3 wantedDirection = Quaternion.Euler( 0, fRandomAngle, 0 ) * targetToAgent.normalized * gameStats.m_hiddenTargetAcquisitionRange;
			p_controller.m_navMeshAgent.destination = p_controller.transform.position + wantedDirection;
			p_controller.m_navMeshAgent.speed = p_controller.m_gameEnemyStats.m_moveSpeed;
			p_controller.m_navMeshAgent.isStopped = false;
		}
    }
}
