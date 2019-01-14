using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Decisions/VisionDetectAttack" )]
public class DecisionVisionDetectAttack : AI_Decision
{
	public override bool Decide( AI_StateController p_controller )
    {
        bool targetVisible = Look( p_controller as StateController );
        return targetVisible;
    }

    private bool Look( StateController p_controller )
    {
		Transform newTarget = AI_Functions.VisionCastSphere( p_controller );
		if ( newTarget != null )
		{
			p_controller.m_chaseTarget = newTarget;
			return true;
		}

		int layerMaskWithoutWorld = 1 << LayerMask.NameToLayer( "World" );
		layerMaskWithoutWorld = ~layerMaskWithoutWorld;

		p_controller.m_chaseTarget = AI_Functions.VisionDetectClosestTarget( p_controller, layerMaskWithoutWorld );
		return p_controller.m_chaseTarget != null;
	}
}
