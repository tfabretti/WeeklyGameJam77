using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;


public class Damager2D : Damager
{
	public Vector2 offset = new Vector2( 1.5f, 1f );
	public Vector2 size = new Vector2( 2.5f, 1f );

	protected ContactFilter2D m_AttackContactFilter;
	protected Collider2D[] m_AttackOverlapResults = new Collider2D[10];
	protected Collider2D m_LastHit;

	void Awake()
	{
		m_AttackContactFilter.layerMask = hittableLayers;
		m_AttackContactFilter.useLayerMask = true;
		m_AttackContactFilter.useTriggers = canHitTriggers;
	}

	void FixedUpdate()
	{
		if ( !m_CanDamage )
			return;

		Vector2 scale = m_DamagerTransform.lossyScale;

		Vector2 facingOffset = Vector2.Scale( offset, scale );
		if ( offsetBasedOnSpriteFacing && spriteRenderer != null && spriteRenderer.armature.flipX != m_SpriteOriginallyFlipped )
			facingOffset = new Vector2( -offset.x * scale.x, offset.y * scale.y );

		Vector2 scaledSize = Vector2.Scale( size, scale );

		Vector2 pointA = (Vector2)m_DamagerTransform.position + facingOffset - scaledSize * 0.5f;
		Vector2 pointB = pointA + scaledSize;

		int hitCount = Physics2D.OverlapArea( pointA, pointB, m_AttackContactFilter, m_AttackOverlapResults );

		for ( int i = 0 ; i < hitCount ; i++ )
		{
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
[CustomEditor( typeof( Damager2D ) )]
public class Damager2DEditor : Editor
{
	static BoxBoundsHandle s_BoxBoundsHandle = new BoxBoundsHandle();
	static Color s_EnabledColor = Color.green + Color.grey;

	void OnSceneGUI()
	{
		Damager2D damager = (Damager2D)target;

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