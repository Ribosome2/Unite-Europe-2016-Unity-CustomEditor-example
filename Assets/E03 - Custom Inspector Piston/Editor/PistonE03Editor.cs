using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

[CustomEditor( typeof( PistonE03 ) )]
public class PistonE03Editor : Editor 
{
    PistonE03 m_Target;

    public override void OnInspectorGUI()
    {
        m_Target = (PistonE03)target;

        DrawDefaultInspector();
        DrawStatesInspector();        
    }

    //Draw a beautiful and useful custom inspector for our states array
    void DrawStatesInspector()
    {
        GUILayout.Space( 5 );
        GUILayout.Label( "States", EditorStyles.boldLabel );

        for( int i = 0; i < m_Target.States.Count; ++i )
        {
            DrawState( i );
        }

        DrawAddStateButton();
    }

    void DrawState( int index )
    {
        if( index < 0 || index >= m_Target.States.Count )
        {
            return;
        }

        //Find the States variable in our serializedObject. serializedObject is a special way to access and modify the variables
        //of our component to which Unity added a lot of helper functionality. For example, if you modify values though the
        //serializedObject instead of modifying the component directly, Unity automatically creates Undo and Redo functionality
        //for you
        SerializedProperty listIterator = serializedObject.FindProperty( "States" );

        GUILayout.BeginHorizontal();
        {
            //If this object is an instantiated prefab, we want to mirror Unitys default inspector behaviour where
            //variables that have been modified (but not applied) are drawn in bold text
            if( listIterator.isInstantiatedPrefab == true )
            {
                //The SetBoldDefaultFont functionality is usually hidden from us but we can use some tricks to
                //access the method anyways. See the implementation of our own EditorGUIHelper.SetBoldDefaultFont
                //for more info
                EditorGUIHelper.SetBoldDefaultFont( listIterator.GetArrayElementAtIndex( index ).prefabOverride );
            }
            
            GUILayout.Label( "Name", EditorStyles.label, GUILayout.Width( 50 ) );

            //BeginChangeCheck() is a useful way to see if an inspector variable was changed between
            //BeginChangeCheck() and EndChangeCheck()
            EditorGUI.BeginChangeCheck();
            string newName = GUILayout.TextField( m_Target.States[ index ].Name, GUILayout.Width( 120 ) );
            Vector3 newPosition = EditorGUILayout.Vector3Field( "", m_Target.States[ index ].Position );

            //If a variable was modified, EndChangeCheck() returns true so we can do neccessary behaviour like storing the changes
            if( EditorGUI.EndChangeCheck() )
            {
                //Create an Undo/Redo step for this modification
                Undo.RecordObject( m_Target, "Modify State" );

                m_Target.States[ index ].Name = newName;
                m_Target.States[ index ].Position = newPosition;

                //Whenever you modify a component variable directly without using serializedObject you need to tell
                //Unity that this component has changed so the values are saved next time the user saves the project.
                EditorUtility.SetDirty( m_Target );
            }

            EditorGUIHelper.SetBoldDefaultFont( false );

            if( GUILayout.Button( "Remove" ) )
            {
                EditorApplication.Beep();

                //DisplayDialog is a very useful method to create a simple popup check. You can use this to simply display information to the user 
                //that something has changed which the user has to confirm by clicking OK, or you can even ask simple Yes, No questions to, 
                //like in this example, ask if the user really wants to delete something
                if( EditorUtility.DisplayDialog( "Really?", "Do you really want to remove the state '" + m_Target.States[ index ].Name + "'?", "Yes", "No" ) == true )
                {
                    Undo.RecordObject( m_Target, "Delete State" );
                    m_Target.States.RemoveAt( index );
                    EditorUtility.SetDirty( m_Target );
                }
            }
        }
        GUILayout.EndHorizontal();
    }

    void DrawAddStateButton()
    {
        if( GUILayout.Button( "Add new State", GUILayout.Height( 30 ) ) )
        {
            //Create an Undo/Redo step for this modification
            Undo.RecordObject( m_Target, "Add new State" );

            m_Target.States.Add( new PistonState { Name = "New State" } );

            //Whenever you modify a component variable directly without using serializedObject you need to tell
            //Unity that this component has changed so the values are saved next time the user saves the project.
            EditorUtility.SetDirty( m_Target );
        }
    }
}
