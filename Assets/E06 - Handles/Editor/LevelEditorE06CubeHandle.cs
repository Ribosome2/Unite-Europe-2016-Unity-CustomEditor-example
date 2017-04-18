using UnityEngine;
using UnityEditor;
using System.Collections;

//[InitializeOnLoad] makes sure that the constructor of this class is called in the editor
//whenever the editor assembly is being loaded (for example on start or on recompile)
[InitializeOnLoad]
public class LevelEditorE06CubeHandle : Editor 
{
    public static Vector3 CurrentHandlePosition = Vector3.zero;
    public static bool IsMouseInValidArea = false;

    static Vector3 m_OldHandlePosition = Vector3.zero;

    static LevelEditorE06CubeHandle()
    {
        //The OnSceneGUI delegate is called every time the SceneView is redrawn and allows you
        //to draw GUI elements into the SceneView to create in editor functionality
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    static void OnSceneGUI( SceneView sceneView )
    {
        if( IsInCorrectLevel() == false )
        {
            return;
        }

        bool isLevelEditorEnabled = EditorPrefs.GetBool( "IsLevelEditorEnabled", true );

        //Ignore this. I am using this because when the scene GameE06 is opened we haven't yet defined any On/Off buttons
        //for the cube handles. That comes later in E07. This way we are forcing the cube handles state to On in this scene
        {
            if( UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "GameE06" )
            {
                isLevelEditorEnabled = true;
            }
        }

        if( isLevelEditorEnabled == false )
        {
            return;
        }

        UpdateHandlePosition();
        UpdateIsMouseInValidArea( sceneView.position );
        UpdateRepaint();

        DrawCubeDrawPreview();
    }

    //I will use this type of function in many different classes. Basically this is useful to 
    //be able to draw different types of the editor only when you are in the correct scene so we
    //can have an easy to follow progression of the editor while hoping between the different scenes
    static bool IsInCorrectLevel()
    {
        return UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "GameE06"
            || UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "GameE07"
            || UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "GameE08"
            || UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "GameE09";
    }

    static void UpdateIsMouseInValidArea( Rect sceneViewRect )
    {
        //Make sure the cube handle is only drawn when the mouse is within a position that we want
        //In this case we simply hide the cube cursor when the mouse is hovering over custom GUI elements in the lower
        //are of the sceneView which we will create in E07
        bool isInValidArea = Event.current.mousePosition.y < sceneViewRect.height - 35;

        if( isInValidArea != IsMouseInValidArea )
        {
            IsMouseInValidArea = isInValidArea;
            SceneView.RepaintAll();
        }
    }

    static void UpdateHandlePosition()
    {
        if( Event.current == null )
        {
            return;
        }

        Vector2 mousePosition = new Vector2( Event.current.mousePosition.x, Event.current.mousePosition.y );

        Ray ray = HandleUtility.GUIPointToWorldRay( mousePosition );
        RaycastHit hit;

        if( Physics.Raycast( ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer( "Level" ) ) == true )
        {
            Vector3 offset = Vector3.zero;

            if( EditorPrefs.GetBool( "SelectBlockNextToMousePosition", true ) == true )
            {
                offset = hit.normal;
            }

            CurrentHandlePosition.x = Mathf.Floor( hit.point.x - hit.normal.x * 0.001f + offset.x );
            CurrentHandlePosition.y = Mathf.Floor( hit.point.y - hit.normal.y * 0.001f + offset.y );
            CurrentHandlePosition.z = Mathf.Floor( hit.point.z - hit.normal.z * 0.001f + offset.z );

            CurrentHandlePosition += new Vector3( 0.5f, 0.5f, 0.5f );
        }
    }

    static void UpdateRepaint()
    {
        //If the cube handle position has changed, repaint the scene
        if( CurrentHandlePosition != m_OldHandlePosition )
        {
            SceneView.RepaintAll();
            m_OldHandlePosition = CurrentHandlePosition;
        }
    }

    static void DrawCubeDrawPreview()
    {
        if( IsMouseInValidArea == false )
        {
            return;
        }

        Handles.color = new Color( EditorPrefs.GetFloat( "CubeHandleColorR", 1f ), EditorPrefs.GetFloat( "CubeHandleColorG", 1f ), EditorPrefs.GetFloat( "CubeHandleColorB", 0f ) );

        DrawHandlesCube( CurrentHandlePosition );
    }

    static void DrawHandlesCube( Vector3 center )
    {
        Vector3 p1 = center + Vector3.up * 0.5f + Vector3.right * 0.5f + Vector3.forward * 0.5f;
        Vector3 p2 = center + Vector3.up * 0.5f + Vector3.right * 0.5f - Vector3.forward * 0.5f;
        Vector3 p3 = center + Vector3.up * 0.5f - Vector3.right * 0.5f - Vector3.forward * 0.5f;
        Vector3 p4 = center + Vector3.up * 0.5f - Vector3.right * 0.5f + Vector3.forward * 0.5f;

        Vector3 p5 = center - Vector3.up * 0.5f + Vector3.right * 0.5f + Vector3.forward * 0.5f;
        Vector3 p6 = center - Vector3.up * 0.5f + Vector3.right * 0.5f - Vector3.forward * 0.5f;
        Vector3 p7 = center - Vector3.up * 0.5f - Vector3.right * 0.5f - Vector3.forward * 0.5f;
        Vector3 p8 = center - Vector3.up * 0.5f - Vector3.right * 0.5f + Vector3.forward * 0.5f;

        //You can use Handles to draw 3d objects into the SceneView. If defined properly the
        //user can even interact with the handles. For example Unitys move tool is implemented using Handles
        //However here we simply draw a cube that the 3D position the mouse is pointing to
        Handles.DrawLine( p1, p2 );
        Handles.DrawLine( p2, p3 );
        Handles.DrawLine( p3, p4 );
        Handles.DrawLine( p4, p1 );

        Handles.DrawLine( p5, p6 );
        Handles.DrawLine( p6, p7 );
        Handles.DrawLine( p7, p8 );
        Handles.DrawLine( p8, p5 );

        Handles.DrawLine( p1, p5 );
        Handles.DrawLine( p2, p6 );
        Handles.DrawLine( p3, p7 );   
        Handles.DrawLine( p4, p8 );
    }
}
