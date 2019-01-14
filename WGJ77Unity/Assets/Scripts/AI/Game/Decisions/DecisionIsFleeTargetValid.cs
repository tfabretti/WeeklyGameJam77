using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Decisions/IsFleeTargetValid" )]
public class DecisionIsFleeTargetValid : AI_Decision
{
    public override bool Decide( AI_StateController p_controller )
    {
        bool targetIsValid = IsValid( p_controller as StateController );
        return targetIsValid;
    }

    private bool IsValid( StateController p_controller )
    {
		Transform fleeTarget = p_controller.m_fleeTarget;
		p_controller.m_chaseTarget = null;
		Vector3 offsetFleeTarget = p_controller.m_offsetFleeTarget;
		p_controller.m_offsetFleeTarget = Vector3.zero;

		if ( fleeTarget == null )
			return false;

		// Target is inactive
		if ( fleeTarget.gameObject.activeInHierarchy == false )
			return false;

		// Target is in the same team
		StateController otherStateController = fleeTarget.GetComponent<StateController>();
		if ( otherStateController == null || ( otherStateController.m_gameEnemyStats.m_teams.m_teamMask & p_controller.m_gameEnemyStats.m_teams.m_teamMask ) != 0 )
			return false;

		// Target is too far
		Vector3 targetToAgent = p_controller.transform.position - fleeTarget.position;
		if ( targetToAgent.magnitude > p_controller.m_gameEnemyStats.m_vision.m_range * 1.5f )
			return false;

		p_controller.m_fleeTarget = fleeTarget;
		p_controller.m_offsetFleeTarget = offsetFleeTarget;
		return true;
	}
}
