using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CreateAssetMenu( menuName = "PluggableAI/TeamsDefinition" )]
[System.Serializable]
public class AI_TeamsDefinition : ScriptableObject
{
	public string[] m_TeamsArray;
}


[CustomEditor( typeof( AI_TeamsDefinition ) )]
public class AI_TeamsEdtior : Editor
{
	AI_TeamsDefinition m_teamsData;

    public void OnEnable()
    {
        m_teamsData = target as AI_TeamsDefinition;
    }

    public override void OnInspectorGUI()
    {
		// Show default inspector property editor
		DrawDefaultInspector();

		// Builds an enum from the string array of teams
		if ( GUILayout.Button( "Build Enum" ) )
        {
            using ( StreamWriter streamWriter = new StreamWriter( "Assets/Scripts/AI/Game/AI_TeamsEnum.cs" ) )
            {
                streamWriter.WriteLine( "public enum AI_TeamsEnum" );
                streamWriter.WriteLine( "{" );
                for ( int i = 0 ; i < m_teamsData.m_TeamsArray.Length ; i++ )
                {
                    if ( m_teamsData.m_TeamsArray[i]  != "" )
                        streamWriter.WriteLine( "\t" + m_teamsData.m_TeamsArray[i] + " = 1 << " + i +"," );
                }
                streamWriter.WriteLine( "}" );
            }
            AssetDatabase.Refresh();
        }
    }

}



[System.Serializable]
public class AI_TeamMask
{
	public AI_TeamsDefinition m_teamsDefinition;
	public int m_teamMask;
}


[CustomPropertyDrawer( typeof( AI_TeamMask ), true )]
public class AI_TeamMaskDrawer : PropertyDrawer
{
	private bool m_showInfo = true;

	// Is power of two ?
	bool IsPowerOfTwo( int p_int )
	{
		return ( p_int & ( p_int - 1 ) ) == 0;
	}

	string GetTeamMaskLabelName( SerializedProperty p_teamsArray, SerializedProperty p_teamMask )
	{
		if ( p_teamMask.intValue == 0 || p_teamsArray == null || p_teamsArray.arraySize == 0 )
			return "None";

		if ( p_teamMask.intValue == ~0 )
			return "All";

		if ( IsPowerOfTwo( p_teamMask.intValue ) )
		{
			if ( p_teamMask.intValue == 1 )
				return p_teamsArray.GetArrayElementAtIndex(0).stringValue;
			else
			{
				int arrayIndex = Mathf.RoundToInt( Mathf.Sqrt( p_teamMask.intValue ) ); // 2 = idx 1, 4 = idx 2...
				if ( arrayIndex < p_teamsArray.arraySize )
					return p_teamsArray.GetArrayElementAtIndex(arrayIndex).stringValue;
			}
		}

		return "Mixed...";
	}

	static void SetTeamMaskNone( object data )
	{
		var teamMask = (SerializedProperty)data;
		teamMask.serializedObject.Update();
		teamMask.intValue = 0;
		teamMask.serializedObject.ApplyModifiedProperties();
	}

	static void SetTeamMaskAll( object data )
	{
		var teamMask = (SerializedProperty)data;
		teamMask.serializedObject.Update();
		teamMask.intValue = ~0;
		teamMask.serializedObject.ApplyModifiedProperties();
	}

	static void ToggleTeamMaskItem( object userData )
	{
		var args = (object[])userData;
		var teamMask = (SerializedProperty)args[0];
		var teamTypeID = (int)args[1];
		var value = (bool)args[2];

		teamMask.serializedObject.Update();

		if ( value )
			teamMask.intValue = teamMask.intValue | ( 1 << teamTypeID );
		else
			teamMask.intValue = teamMask.intValue & ~( 1 << teamTypeID );

		teamMask.serializedObject.ApplyModifiedProperties();
	}

#if UNITY_EDITOR
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
	{
		SerializedProperty teamMaskProperty = property.FindPropertyRelative( "m_teamMask" );

		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty( position, label, property );

		// Draw label
		position.height = EditorGUIUtility.singleLineHeight;
		m_showInfo = EditorGUI.Foldout( position, m_showInfo, label, true );

		if ( m_showInfo )
		{
			// Make children fields be indented
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = indent + 1;

			SerializedProperty teamsDefProp = property.FindPropertyRelative( "m_teamsDefinition" );
			// Calculate rects
			float currentHeight = position.y + EditorGUIUtility.singleLineHeight;
			Rect teamsDefRect = new Rect( position.x, currentHeight, position.width, EditorGUIUtility.singleLineHeight );
			EditorGUI.PropertyField( teamsDefRect, teamsDefProp );

			currentHeight += EditorGUIUtility.singleLineHeight;
			Rect teamMaskRect = new Rect( position.x, currentHeight, position.width, EditorGUIUtility.singleLineHeight );
			currentHeight += EditorGUIUtility.singleLineHeight;

			SerializedProperty teamsArrayProp = null;
			if ( teamsDefProp.objectReferenceValue != null )
			{
				SerializedObject teamsDefSerializedObject = new SerializedObject( teamsDefProp.objectReferenceValue );
				teamsArrayProp = teamsDefSerializedObject.FindProperty( "m_TeamsArray" );
			}


			// Contents of the dropdown box.
			string popupContent = "";
			popupContent = GetTeamMaskLabelName( teamsArrayProp, teamMaskProperty );

			var content = new GUIContent( popupContent );
			var popupRect = GUILayoutUtility.GetRect( content, EditorStyles.popup );
			popupRect.y = teamMaskRect.y;

			EditorGUI.BeginProperty( teamMaskRect, GUIContent.none, teamMaskProperty );
			popupRect = EditorGUI.PrefixLabel( popupRect, 0, new GUIContent( "Teams" ) );
			bool pressed = GUI.Button( popupRect, content, EditorStyles.popup );

			if ( pressed )
			{
				var showNone = teamMaskProperty.intValue == 0;
				var showAll = teamMaskProperty.intValue == ~0;

				var menu = new GenericMenu();
				menu.AddItem( new GUIContent( "None" ), showNone, SetTeamMaskNone, teamMaskProperty );
				menu.AddItem( new GUIContent( "All" ), showAll, SetTeamMaskAll, teamMaskProperty );
				menu.AddSeparator( "" );

				if ( teamsArrayProp != null )
				{
					var count = teamsArrayProp.arraySize;
					for ( var i = 0 ; i < count ; ++i )
					{
						var showSelected = ( teamMaskProperty.intValue & 1 << i ) != 0;
						var userData = new object[] { teamMaskProperty, i, !showSelected };
						menu.AddItem( new GUIContent( teamsArrayProp.GetArrayElementAtIndex( i ).stringValue ), showSelected, ToggleTeamMaskItem, userData );
					}
				}

				menu.DropDown( popupRect );
			}

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;
		}

		EditorGUI.EndProperty();

		if ( property.serializedObject != null )
			property.serializedObject.ApplyModifiedProperties();

		int caca = 3;
	}

	public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
	{
		return m_showInfo ? EditorGUIUtility.singleLineHeight * 2 : EditorGUIUtility.singleLineHeight;
	}
#endif
}