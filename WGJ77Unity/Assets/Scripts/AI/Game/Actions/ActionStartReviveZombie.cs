using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "PluggableAI/Actions/StartDeath" )]
public class ActionStartReviveZombie : AI_Action
{
	public string m_deathAnimationName;

    public override void Act( AI_StateController p_controller )
    {
		Revive( p_controller as StateController );
    }

    private void Revive( StateController p_controller )
    {
		if ( p_controller.m_characterMovementsScript != null )
		{
			p_controller.m_characterMovementsScript.DisableMovements();
		}

		if ( p_controller.m_animator != null )
		{
			// Already playing : nothing
			if ( !( p_controller.m_animator.animation.lastAnimationName == m_deathAnimationName && p_controller.m_animator.animation.isPlaying ) )
				p_controller.m_animator.animation.Play( m_deathAnimationName );
		}
	}

}
