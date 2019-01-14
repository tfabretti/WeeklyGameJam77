using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class AI_Transition
{
    public AI_Decision m_decision;
	public bool m_useTrueState = true;
    public AI_State m_trueState;
	public bool m_useFalseState = true;
    public AI_State m_falseState;
	public float m_minimumDelayRetest = 0;
	public float m_maximumDelayRetest = 0;
}



[CustomPropertyDrawer( typeof( AI_Transition ) )]
class AI_TransitionDrawer : PropertyDrawer
{
	private bool m_showInfo = true;

	// Draw the property inside the given rect
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
	{
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty( position, label, property );

		// Draw label
		position.height = EditorGUIUtility.singleLineHeight;
		m_showInfo = EditorGUI.Foldout( position, m_showInfo, label, true );

		// Make children fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = indent + 1;

		if ( m_showInfo )
		{
			// Calculate rects
			float currentHeight = position.y + EditorGUIUtility.singleLineHeight;
			Rect decisionRect = new Rect( position.x, currentHeight, position.width, EditorGUIUtility.singleLineHeight );
			currentHeight += EditorGUIUtility.singleLineHeight;
			Rect useTrueStateRect = new Rect( position.x, currentHeight, EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth * 0.25f, EditorGUIUtility.singleLineHeight );
			currentHeight += EditorGUIUtility.singleLineHeight;
			Rect useFalseStateRect = new Rect( position.x, currentHeight, EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth * 0.25f, EditorGUIUtility.singleLineHeight );
			currentHeight += EditorGUIUtility.singleLineHeight;
			Rect minDelayRect = new Rect( position.x, currentHeight, position.width, EditorGUIUtility.singleLineHeight );
			currentHeight += EditorGUIUtility.singleLineHeight;
			Rect maxDelayRect = new Rect( position.x, currentHeight, position.width, EditorGUIUtility.singleLineHeight );
			currentHeight += EditorGUIUtility.singleLineHeight;

			// Draw fields - passs GUIContent.none to each so they are drawn without labels
			SerializedProperty useTrueStateProperty = property.FindPropertyRelative( "m_useTrueState" );
			SerializedProperty useFalseStateProperty = property.FindPropertyRelative( "m_useFalseState" );
			EditorGUI.PropertyField( decisionRect, property.FindPropertyRelative( "m_decision" ) );
			EditorGUI.PropertyField( useTrueStateRect, useTrueStateProperty, new GUIContent("True State") );
			EditorGUI.PropertyField( useFalseStateRect, useFalseStateProperty, new GUIContent( "False State" ) );
			EditorGUI.PropertyField( minDelayRect, property.FindPropertyRelative( "m_minimumDelayRetest" ) );
			EditorGUI.PropertyField( maxDelayRect, property.FindPropertyRelative( "m_maximumDelayRetest" ) );

			if ( useTrueStateProperty.boolValue == true )
			{
				Rect stateRect = new Rect( position.x + EditorGUIUtility.labelWidth, useTrueStateRect.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight );
				EditorGUI.PropertyField( stateRect, property.FindPropertyRelative( "m_trueState" ), GUIContent.none );
			}
			if ( useFalseStateProperty.boolValue == true )
			{
				Rect stateRect = new Rect( position.x + EditorGUIUtility.labelWidth, useFalseStateRect.y, position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight );
				EditorGUI.PropertyField( stateRect, property.FindPropertyRelative( "m_falseState" ), GUIContent.none );
			}

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;
		}

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
	{
		return m_showInfo ? EditorGUIUtility.singleLineHeight * 6 : EditorGUIUtility.singleLineHeight;
	}
}