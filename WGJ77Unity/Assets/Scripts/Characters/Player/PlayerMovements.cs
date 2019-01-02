using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public float m_speed = 6f;            // The speed that the player will move at.

    Vector3 m_v3Movement;                   // The vector to store the direction of the player's movement.
    Rigidbody m_rigidbodyBody;          // Reference to the player's rigidbody.

    void Awake()
    {
        m_rigidbodyBody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw( "Horizontal" );
        float v = Input.GetAxisRaw( "Vertical" );

        // Move the player around the scene.
        Move( h, v );
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
}