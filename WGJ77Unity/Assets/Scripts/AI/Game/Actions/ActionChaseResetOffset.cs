using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Actions/ChaseResetOffset" )]
public class ActionChaseResetOffset : AI_Action
{
    public override void Act( AI_StateController p_controller )
    {
		StateController controller = p_controller as StateController;
		controller.m_offsetChaseTarget = Vector3.zero;
	}
}
