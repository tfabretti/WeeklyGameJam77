using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;


public class Damager3D : Damager
{
	public Vector3 offset = new Vector3( 2.5f, 1f, 0f );
	public Vector3 size = new Vector3( 2.5f, 1f, 2.5f );

	protected Collider[] m_AttackOverlapResults = new Collider[10];
	protected Collider m_LastHit;

	void FixedUpdate()
	{
		if ( !m_CanDamage )
			return;

		Vector3 scale = m_DamagerTransform.lossyScale;

		Vector3 facingOffset = Vector3.Scale( offset, scale );
		if ( offsetBasedOnSpriteFacing && spriteRenderer != null && spriteRenderer.armature.flipX != m_SpriteOriginallyFlipped )
			facingOffset = new Vector2( -offset.x * scale.x, offset.y * scale.y );

		Vector3 scaledSize = Vector3.Scale( size, scale );

		Vector3 boxCenter = m_DamagerTransform.position + facingOffset;
		Vector3 boxHalfExtents = scaledSize * 0.5f;

		LayerMask ignoreLayers = ~hittableLayers;
		int hitCount = Physics.OverlapBoxNonAlloc( boxCenter, boxHalfExtents, m_AttackOverlapResults, Quaternion.Euler( 0, 0, 0 ), hittableLayers );

		for ( int i = 0 ; i < hitCount ; i++ )
		{
			// Self test
			if ( m_AttackOverlapResults[i].gameObject == gameObject )
				return;

			// Team test
			StateController otherStateController = m_AttackOverlapResults[i].transform.GetComponent<StateController>();
			if ( m_StateController != null && otherStateController != null
				&& ( otherStateController.m_gameEnemyStats.m_teams.m_teamMask & m_StateController.m_gameEnemyStats.m_teams.m_teamMask ) != 0 )
				continue;

			m_LastHit = m_AttackOverlapResults[i];
			Damageable damageable = m_LastHit.GetComponent<Damageable>();

			if ( damageable )
			{
				OnDamageableHit.Invoke( this, damageable );
				damageable.TakeDamage( this, ignoreInvincibility );
				if ( disableDamageAfterHit )
					DisableDamage();
			}
			else
			{
				OnNonDamageableHit.Invoke( this );
			}
		}
	}
}


#if UNITY_EDITOR
[CustomEditor( typeof( Damager3D ) )]
public class Damager3DEditor : Editor
{
	static BoxBoundsHandle s_BoxBoundsHandle = new BoxBoundsHandle();
	static Color s_EnabledColor = Color.green + Color.grey;

	void OnSceneGUI()
	{
		Damager3D damager = (Damager3D)target;

		if ( !damager.enabled )
			return;

		Matrix4x4 handleMatrix = damager.transform.localToWorldMatrix;
		handleMatrix.SetRow( 0, Vector4.Scale( handleMatrix.GetRow( 0 ), new Vector4( 1f, 1f, 0f, 1f ) ) );
		handleMatrix.SetRow( 1, Vector4.Scale( handleMatrix.GetRow( 1 ), new Vector4( 1f, 1f, 0f, 1f ) ) );
		handleMatrix.SetRow( 2, new Vector4( 0f, 0f, 1f, damager.transform.position.z ) );
		using ( new Handles.DrawingScope( handleMatrix ) )
		{
			s_BoxBoundsHandle.center = damager.offset;
			s_BoxBoundsHandle.size = damager.size;

			s_BoxBoundsHandle.SetColor( s_EnabledColor );
			EditorGUI.BeginChangeCheck();
			s_BoxBoundsHandle.DrawHandle();
			if ( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject( damager, "Modify Damager" );

				damager.size = s_BoxBoundsHandle.size;
				damager.offset = s_BoxBoundsHandle.center;
			}
		}
	}
}
#endif