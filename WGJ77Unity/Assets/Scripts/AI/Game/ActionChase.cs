using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Actions/Chase" )]
public class ActionChase : AI_Action
{
    public override void Act( AI_StateController p_controller )
    {
        Chase( ( StateController)p_controller );
    }

    private void Chase( StateController p_controller )
    {
        p_controller.navMeshAgent.destination = p_controller.m_chaseTarget.position;
        p_controller.navMeshAgent.isStopped = false;
    }

}
