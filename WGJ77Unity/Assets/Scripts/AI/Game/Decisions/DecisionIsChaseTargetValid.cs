using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Decisions/IsChaseTargetValid" )]
public class DecisionIsChaseTargetValid : AI_Decision
{
    public override bool Decide( AI_StateController p_controller )
    {
        bool targetIsValid = IsValid( p_controller as StateController );
        return targetIsValid;
    }

    private bool IsValid( StateController p_controller )
    {
		Transform chaseTarget = p_controller.m_chaseTarget;
		p_controller.m_chaseTarget = null;
		Vector3 offsetChaseTarget = p_controller.m_offsetChaseTarget;
		p_controller.m_offsetChaseTarget = Vector3.zero;

		if ( chaseTarget == null )
			return false;

		// Target is inactive
		if ( chaseTarget.gameObject.activeInHierarchy == false )
			return false;

		// Target is in the same team
		StateController otherStateController = chaseTarget.GetComponent<StateController>();
		if ( otherStateController == null || ( otherStateController.m_gameEnemyStats.m_teams.m_teamMask & p_controller.m_gameEnemyStats.m_teams.m_teamMask ) != 0 )
			return false;

		p_controller.m_chaseTarget = chaseTarget;
		p_controller.m_offsetChaseTarget = offsetChaseTarget;
		return true;
	}
}
