using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Events;


public class Damager : MonoBehaviour
{
	[System.Serializable]
	public class DamagableEvent : UnityEvent<Damager, Damageable>
	{
	}

	[System.Serializable]
	public class NonDamagableEvent : UnityEvent<Damager>
	{
	}

	public int damage = 1;
	[Tooltip( "If this is set, the offset x will be changed base on the sprite flipX setting. e.g. Allow to make the damager alway forward in the direction of sprite" )]
	public bool offsetBasedOnSpriteFacing = true;
	[Tooltip( "SpriteRenderer used to read the flipX value used by offset Based OnSprite Facing" )]
	public DragonBones.UnityArmatureComponent spriteRenderer;
	[Tooltip( "If disabled, damager ignore trigger when casting for damage" )]
	public bool canHitTriggers = true;
	public bool disableDamageAfterHit = false;
	[Tooltip( "If set, an invincible damageable hit will still get the onHit message (but won't loose any life)" )]
	public bool ignoreInvincibility = false;
	public LayerMask hittableLayers;
	public AI_TeamMask hittableTeams;
	public DamagableEvent OnDamageableHit;
	public NonDamagableEvent OnNonDamageableHit;

	protected bool m_SpriteOriginallyFlipped;
	protected bool m_CanDamage = false;
	protected Transform m_DamagerTransform;
	protected StateController m_StateController;

	void Start()
	{
		if ( offsetBasedOnSpriteFacing && spriteRenderer != null )
			m_SpriteOriginallyFlipped = spriteRenderer.armature.flipX;

		m_DamagerTransform = transform;
		m_StateController = GetComponent<StateController>();
	}

	public void EnableDamage()
	{
		m_CanDamage = true;
	}

	public void DisableDamage()
	{
		m_CanDamage = false;
	}
}


#if UNITY_EDITOR
[CustomEditor( typeof( Damager ) )]
public class DamagerEditor : Editor
{
	SerializedProperty m_DamageProp;
	SerializedProperty m_OffsetBasedOnSpriteFacingProp;
	SerializedProperty m_SpriteRendererProp;
	SerializedProperty m_CanHitTriggersProp;
	SerializedProperty m_IgnoreInvincibilityProp;
	SerializedProperty m_HittableLayersProp;
	SerializedProperty m_HittableTeamsProp;
	SerializedProperty m_OnDamageableHitProp;
	SerializedProperty m_OnNonDamageableHitProp;

	void OnEnable()
	{
		m_DamageProp = serializedObject.FindProperty( "damage" );
		m_OffsetBasedOnSpriteFacingProp = serializedObject.FindProperty( "offsetBasedOnSpriteFacing" );
		m_SpriteRendererProp = serializedObject.FindProperty( "spriteRenderer" );
		m_CanHitTriggersProp = serializedObject.FindProperty( "canHitTriggers" );
		m_IgnoreInvincibilityProp = serializedObject.FindProperty( "ignoreInvincibility" );
		m_HittableLayersProp = serializedObject.FindProperty( "hittableLayers" );
		m_HittableTeamsProp = serializedObject.FindProperty( "hittableTeams" );
		m_OnDamageableHitProp = serializedObject.FindProperty( "OnDamageableHit" );
		m_OnNonDamageableHitProp = serializedObject.FindProperty( "OnNonDamageableHit" );
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField( m_DamageProp );
		EditorGUILayout.PropertyField( m_OffsetBasedOnSpriteFacingProp );
		if ( m_OffsetBasedOnSpriteFacingProp.boolValue )
			EditorGUILayout.PropertyField( m_SpriteRendererProp );
		EditorGUILayout.PropertyField( m_CanHitTriggersProp );
		EditorGUILayout.PropertyField( m_IgnoreInvincibilityProp );
		EditorGUILayout.PropertyField( m_HittableLayersProp );
		EditorGUILayout.PropertyField( m_HittableLayersProp );
		EditorGUILayout.PropertyField( m_OnDamageableHitProp );
		EditorGUILayout.PropertyField( m_HittableTeamsProp );

		serializedObject.ApplyModifiedProperties();
	}

}
#endif