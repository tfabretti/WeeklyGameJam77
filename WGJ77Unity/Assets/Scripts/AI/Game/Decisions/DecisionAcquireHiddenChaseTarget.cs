using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Decisions/AcquireHiddenChaseTarget" )]
public class DecisionAcquireHiddenChaseTarget : AI_Decision
{
	public override bool Decide( AI_StateController p_controller )
    {
        bool targetVisible = Look( p_controller as StateController );
        return targetVisible;
    }

	// Picks a random target colliding with a sphere of the wanted range
    private bool Look( StateController p_controller )
    {
		if ( p_controller.m_chaseTarget != null )
			return false;

		EnemyStats gameStats = p_controller.m_gameEnemyStats;

		int layerMaskWithoutWorld = 1 << LayerMask.NameToLayer( "World" );
		layerMaskWithoutWorld = ~layerMaskWithoutWorld;

		int collidingCount = Physics.OverlapSphereNonAlloc( p_controller.m_agentEyesTransform.position, gameStats.m_hiddenTargetAcquisitionRange, p_controller.m_collidersArrayForTests, layerMaskWithoutWorld );

		for ( int i = 0 ; i < collidingCount ; ++i )
		{
			StateController otherStateController = p_controller.m_collidersArrayForTests[i].transform.GetComponent<StateController>();
			// If the collided object hasn't a StateController or is in the same team : array removal
			if ( otherStateController == null || ( otherStateController.m_gameEnemyStats.m_teams.m_teamMask & gameStats.m_teams.m_teamMask ) != 0 )
				p_controller.m_collidersArrayForTests[i--] = p_controller.m_collidersArrayForTests[--collidingCount];
		}

		// A random target is picked
		if ( collidingCount > 0 )
		{
			p_controller.m_chaseTarget = p_controller.m_collidersArrayForTests[Random.Range( 0, collidingCount - 1 )].transform;
			return true;
		}

		return false;
	}
}
