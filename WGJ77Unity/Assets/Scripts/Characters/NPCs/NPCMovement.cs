using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public string m_animationIdle = "";
    public string m_animationMove = "";

    private DragonBones.UnityArmatureComponent m_animator = null; // Reference to the animator

    [HideInInspector] public NavMeshAgent m_navMeshAgent;

    void Awake()
    {
        Rigidbody rigidbodyBody = GetComponent<Rigidbody>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();

        Transform mesh = transform.Find( "Mesh" );
        if ( mesh )
            m_animator = mesh.GetComponent<DragonBones.UnityArmatureComponent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ( !m_navMeshAgent )
            return;

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
        string animationToPlay = m_animationIdle;
        if ( p_v3DesiredVolcity.x != 0 || p_v3DesiredVolcity.z != 0 )
            animationToPlay = m_animationMove;
        
        if ( m_animator.animation.lastAnimationName != animationToPlay )
            m_animator.animation.Play( animationToPlay );
    }
}
