using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : CharacterMovements
{
    [HideInInspector] public NavMeshAgent m_navMeshAgent;

	void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
    }

	protected override void DoMovements()
	{
		if ( !m_navMeshAgent || !m_animator )
            return;

		DragonBones.ColorTransform newColor = new DragonBones.ColorTransform();
		newColor.redOffset = 255;
		newColor.redMultiplier = 2;
		newColor.greenOffset = 1;
		newColor.blueOffset = 10000;
		m_animator.color = newColor;

		Turn( m_navMeshAgent.desiredVelocity );
		Animate( m_navMeshAgent.desiredVelocity );
    }

	// Turn in the right direction
	void Turn( Vector3 p_v3DesiredVolcity )
    {
        if ( p_v3DesiredVolcity.x > 0 )
            m_animator.armature.flipX = true;
        else if ( p_v3DesiredVolcity.x < 0 )
            m_animator.armature.flipX = false;
    }

    // Animation
    void Animate( Vector3 p_v3DesiredVolcity )
    {
		if ( m_animator == null )
			return;

        string animationToPlay = m_animationIdle;
        if ( p_v3DesiredVolcity.x != 0 || p_v3DesiredVolcity.z != 0 )
            animationToPlay = m_animationMove;
        
        if ( m_animator.animation.lastAnimationName != animationToPlay )
            m_animator.animation.Play( animationToPlay );
    }
}
