using UnityEngine;

public class PlayerMovements : CharacterMovements
{
    public float m_speed = 6f;            // The speed that the player will move at.

	public Damager m_damager;

    Vector3 m_v3Movement;                   // The vector to store the direction of the player's movement.
    Rigidbody m_rigidbodyBody;          // Reference to the player's rigidbody.

    void Awake()
    {
        m_rigidbodyBody = GetComponent<Rigidbody>();
	}

	protected override void DoMovements()
	{
		// Store the input axes.
		float h = Input.GetAxisRaw( "Horizontal" );
        float v = Input.GetAxisRaw( "Vertical" );

		// Move the player around the scene.
		Move( h, v );

		if ( m_animator )
		{
			// Turn the player in the right direction
			Turn( h, v );

			// Animate the player
			Animate( h, v );
		}
	}

	protected override void DoNoMoveAction()
	{
		if ( !m_animator )
			return;

		Attack();
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
        if ( h < 0 )
            m_animator.armature.flipX = false;
        else if ( h > 0 )
            m_animator.armature.flipX = true;
    }

    void Animate( float h, float v )
    {
		if ( m_animator == null )
			return;

		string animationToPlay = m_animationIdle;
		if ( h != 0 || v != 0 )
			animationToPlay = m_animationMove;

		if ( m_animator.animation.lastAnimationName != animationToPlay )
			m_animator.animation.Play( animationToPlay );
	}

	void Attack()
	{
		// During attack : nothing
		if ( m_animator.animation.lastAnimationName == m_animationAttack && m_animator.animation.isPlaying == false )
		{
			EnableMovements();
			if ( m_damager != null )
				m_damager.DisableDamage();
		}

		if ( Input.GetButtonDown( m_buttonAttackName ) )
		{
			m_animator.animation.Play( m_animationAttack );
			DisableMovements();
			if ( m_damager != null )
				m_damager.EnableDamage();
		}
	}
}