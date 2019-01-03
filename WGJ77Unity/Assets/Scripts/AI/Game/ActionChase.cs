using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Actions/Chase" )]
public class ActionChase : AI_Action
{
    public override void Act( AI_StateController p_controller )
    {
        Chase( p_controller as StateController );
    }

    private void Chase( StateController p_controller )
    {
        if ( p_controller.m_chaseTarget != null )
        {
            p_controller.m_navMeshAgent.destination = p_controller.m_chaseTarget.position;
            p_controller.m_navMeshAgent.speed = p_controller.m_gameEnemyStats.m_moveSpeed;
            p_controller.m_navMeshAgent.isStopped = false;
        }
    }

}
