using UnityEngine;
using UnityEditor;
using System.Collections;

//You can override how a component is drawn in the inspector. Simply define the
//[CustomEditor] attribute for a class of type "Editor"
//You need to include the UnityEditor namespace to access these and this class has
//to be located in a folder named "Editor", this way Unity knows that this is an editor script
[CustomEditor( typeof( GameCamera ) )]
public class GameCameraEditor : Editor 
{
    GameCamera m_Target;

    //Override OnInspectorGUI() to draw your own editor
    public override void OnInspectorGUI()
    {
        m_Target = (GameCamera)target;

        //DrawDefaultInspector tells Unity to draw the inspector like it would if this editor
        //class would not exist. This is usefull if you just want to add some fields to the
        //already existing editor.
        DrawDefaultInspector();
        DrawCameraHeightPreviewSlider();  
    }

    void DrawCameraHeightPreviewSlider()
    {
        GUILayout.Space( 10 );

        Vector3 cameraPosition = m_Target.transform.position;
        cameraPosition.y = EditorGUILayout.Slider( "Camera Height", cameraPosition.y, m_Target.MinimumHeight, m_Target.MaximumHeight );

        if( cameraPosition.y != m_Target.transform.position.y )
        {
            Undo.RecordObject( m_Target, "Change Camera Height" );
            m_Target.transform.position = cameraPosition;
        }
    }
}
