using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float m_speed = 6f;            // The speed that the player will move at.
    public string m_animationIdle = "";
    public string m_animationMove = "";

    Vector3 m_v3Movement;                   // The vector to store the direction of the player's movement.
    Rigidbody m_rigidbodyBody;          // Reference to the player's rigidbody.
    private DragonBones.UnityArmatureComponent m_animator = null; // Reference to the animator

    void Awake()
    {
        m_rigidbodyBody = GetComponent<Rigidbody>();

        Transform mesh = transform.Find( "Mesh" );
        if ( mesh )
            m_animator = mesh.GetComponent<DragonBones.UnityArmatureComponent>();
    }


    void FixedUpdate()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw( "Horizontal" );
        float v = Input.GetAxisRaw( "Vertical" );

        // Move the player around the scene.
        Move( h, v );

        // Turn the player in the right direction
        Turn( h, v );

        // Animate the player
        Animate( h, v );
    }

    void Move( float h, float v )
    {
        // Set the movement vector based on the axis input.
        m_v3Movement.Set( h, 0f, v );

        // Normalise the movement vector and make it proportional to the speed per second.
        m_v3Movement = m_v3Movement.normalized * m_speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        m_rigidbodyBody.MovePosition( transform.position + m_v3Movement );
    }

    void Turn( float h, float v )
    {
        if ( h <= 0 )
            m_animator.armature.flipX = false;
        else
            m_animator.armature.flipX = true;
    }

    void Animate( float h, float v )
    {
        string animationToPlay = m_animationIdle;
        if ( h != 0 || v != 0 )
            animationToPlay = m_animationMove;

        if ( m_animator.animation.lastAnimationName != animationToPlay )
            m_animator.animation.Play( animationToPlay );
    }
}