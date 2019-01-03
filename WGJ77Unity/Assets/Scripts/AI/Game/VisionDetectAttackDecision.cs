using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Decisions/VisionDetectAttack" )]
public class VisionDetectAttackDecision : AI_Decision
{
    public override bool Decide( AI_StateController p_controller )
    {
        bool targetVisible = Look( p_controller as StateController );
        return targetVisible;
    }

    private bool Look( StateController p_controller )
    {
        RaycastHit hit;

        if ( p_controller.m_gameEnemyStats.m_visionDetectionRange > 0 )
            Debug.DrawRay( p_controller.m_agentEyesTransform.position, p_controller.m_agentEyesTransform.forward.normalized * p_controller.m_gameEnemyStats.m_visionDetectionRange, Color.green );

        if ( Physics.SphereCast( p_controller.m_agentEyesTransform.position, p_controller.m_gameEnemyStats.m_visionDetectionSphereCastRadius,
            p_controller.m_agentEyesTransform.forward, out hit, p_controller.m_gameEnemyStats.m_visionDetectionRange )
            && hit.collider.CompareTag( "Player" ) )
        {
            p_controller.m_fleeTarget = hit.transform;
            return true;
        }
        else
        {
            return false;
        }

    }
}
