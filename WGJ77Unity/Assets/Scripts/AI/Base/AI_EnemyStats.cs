using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class AI_EnemyStats : ScriptableObject
{
	public AI_TeamMask m_teams; // Teams definition

	public float m_moveSpeed = 1;

	[System.Serializable]
	public struct VisionParam
	{
		[Range( 0.0f, 360.0f )]
		public float m_angle;
		[Range( 0.0f, 360.0f )]
		public float m_direction;
		public float m_range;
		[Tooltip( "If set (non-zero), a sphere will be cast in front of the agent to find a target. If not, simple vision test" )]
		public float m_sphereCastRadius;
	}
	public VisionParam m_vision;
}


[CustomEditor( typeof( AI_EnemyStats ), true )]
public class AI_EnemyStatsEditor : Editor
{
	SerializedProperty m_visionSphereCastRadius;

	public void OnEnable()
	{
		m_visionSphereCastRadius = serializedObject.FindProperty( "m_vision.m_sphereCastRadius" );
	}


#if UNITY_EDITOR
	public override void OnInspectorGUI()
	{
		if ( m_visionSphereCastRadius.floatValue < 0 )
		{
			m_visionSphereCastRadius.floatValue = 0;
			serializedObject.ApplyModifiedProperties();
		}

		// Show default inspector property editor
		DrawDefaultInspector();
	}
#endif
}