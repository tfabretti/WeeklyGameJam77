using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CreateAssetMenu( menuName = "PluggableAI/Teams" )]
public class AI_Teams : ScriptableObject
{
	[SerializeField]
	public string[] m_TeamsArray;

    [SerializeField]
    public int m_teamMask = 0;
}



[CustomEditor( typeof( AI_Teams ) )]
public class AI_TeamsEdtior : Editor
{
    AI_Teams m_teamsData;
    private int selected = 0;

    SerializedProperty m_teamsArrayProperty;
    SerializedProperty m_teamMaskProperty;

    public void OnEnable()
    {
        m_teamsData = target as AI_Teams;
		m_teamsArrayProperty = serializedObject.FindProperty( "m_TeamsArray" );
        m_teamMaskProperty = serializedObject.FindProperty( "m_teamMask" );
    }

    // Is power of two ?
    bool IsPowerOfTwo( int p_int )
    {
        return ( p_int & ( p_int - 1 ) ) == 0;
    }

    string GetTeamMaskLabelName( string[] p_teamsArray, SerializedProperty p_teamMask )
    {
        if ( p_teamMask.intValue == 0 || p_teamsArray.Length == 0 )
            return "None";

        if ( p_teamMask.intValue == ~0 )
            return "All";

        if ( IsPowerOfTwo( p_teamMask.intValue ) )
        {
            if ( p_teamMask.intValue == 1 )
                return p_teamsArray[0];
            else
            {
                int arrayIndex = Mathf.RoundToInt( Mathf.Sqrt( p_teamMask.intValue ) ); // 2 = idx 1, 4 = idx 2...
                if ( arrayIndex < p_teamsArray.Length )
                    return p_teamsArray[arrayIndex];
            }
        }

        return "Mixed...";
    }

    static void SetTeamMaskNone( object data )
    {
        var teamMask = (SerializedProperty)data;
        teamMask.intValue = 0;
    }

    static void SetTeamMaskAll( object data )
    {
        var teamMask = (SerializedProperty)data;
        teamMask.intValue = ~0;
    }

    static void ToggleTeamMaskItem( object userData )
    {
        var args = (object[])userData;
        var teamMask = (SerializedProperty)args[0];
        var teamTypeID = (int)args[1];
        var value = (bool)args[2];

        if ( value )
            teamMask.intValue = teamMask.intValue | ( 1 << teamTypeID );
        else
            teamMask.intValue = teamMask.intValue & ~( 1 << teamTypeID );
    }

    public override void OnInspectorGUI()
    {
		//      // AI_Teams properties
		//      serializedObject.Update();
		//      SerializedProperty teamsArray = serializedObject.FindProperty( "m_TeamsArray" );
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField( m_teamsArrayProperty, true );
		EditorGUILayout.PropertyField( m_teamMaskProperty, true );

		if ( m_teamsData.m_TeamsArray != null )
        {
            // Contents of the dropdown box.
            string popupContent = "";

            popupContent = GetTeamMaskLabelName( m_teamsData.m_TeamsArray, m_teamMaskProperty );

            var content = new GUIContent( popupContent );
            var popupRect = GUILayoutUtility.GetRect( content, EditorStyles.popup );

            EditorGUI.BeginProperty( popupRect, GUIContent.none, m_teamMaskProperty );
            popupRect = EditorGUI.PrefixLabel( popupRect, 0, new GUIContent( "Teams" ) );
            bool pressed = GUI.Button( popupRect, content, EditorStyles.popup );

            if ( pressed )
            {
                var showNone = m_teamMaskProperty.intValue == 0;
                //int maxMaskValue = ~( ~0 << ( m_teamsData.m_TeamsArray.Length - 1 ) );
                var showAll = m_teamMaskProperty.intValue == ~0;

                var menu = new GenericMenu();
                menu.AddItem( new GUIContent( "None" ), showNone, SetTeamMaskNone, m_teamMaskProperty );
                menu.AddItem( new GUIContent( "All" ), showAll, SetTeamMaskAll, m_teamMaskProperty );
                menu.AddSeparator( "" );

                var count = m_teamsData.m_TeamsArray.Length;
                for ( var i = 0 ; i < count ; ++i )
                {
                    var showSelected = ( m_teamMaskProperty.intValue & 1 << i ) != 0;
                    var userData = new object[] { m_teamMaskProperty, i, !showSelected };
                    menu.AddItem( new GUIContent( m_teamsData.m_TeamsArray[i] ), showSelected, ToggleTeamMaskItem, userData );
                }

                menu.DropDown( popupRect );
            }

            EditorGUI.EndProperty();
        }

		if ( EditorGUI.EndChangeCheck() )
			serializedObject.ApplyModifiedProperties();

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