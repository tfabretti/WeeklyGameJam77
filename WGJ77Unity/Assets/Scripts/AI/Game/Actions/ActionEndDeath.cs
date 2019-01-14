using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Actions/EndDeath" )]
public class ActionEndDeath : AI_Action
{
    public override void Act( AI_StateController p_controller )
    {
        EndDeath( p_controller as StateController );
    }

    private void EndDeath( StateController p_controller )
    {
		if ( p_controller.m_characterMovementsScript != null )
		{
			p_controller.m_characterMovementsScript.EnableMovements();
		}
	}

}
