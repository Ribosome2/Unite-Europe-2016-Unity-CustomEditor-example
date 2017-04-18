using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections;

[InitializeOnLoad]
public class LevelEditorE08AddAndRemoveObjects : Editor 
{
    static Transform m_LevelParent;
    public static Transform LevelParent
    {
        get
        {
            if( m_LevelParent == null )
            {
                GameObject go = GameObject.Find( "Level" );

                if( go != null )
                {
                    m_LevelParent = go.transform;
                }
            }

            return m_LevelParent;
        }
    }

    static LevelEditorE08AddAndRemoveObjects()
    {
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

        if( LevelEditorE07ToolsMenu.SelectedTool == 0 )
        {
            return;
        }

        //By creating a new ControlID here we can grab the mouse input to the SceneView and prevent Unitys default mouse handling from happening
        //FocusType.Passive means this control cannot receive keyboard input since we are only interested in mouse input
        int controlId = GUIUtility.GetControlID( FocusType.Passive );

        //If the left mouse is being clicked and no modifier buttons are being held
        if( Event.current.type == EventType.mouseDown &&
            Event.current.button == 0 &&
            Event.current.alt == false &&
            Event.current.shift == false &&
            Event.current.control == false )
        {
            if( LevelEditorE06CubeHandle.IsMouseInValidArea == true )
            {
                if( LevelEditorE07ToolsMenu.SelectedTool == 1 )
                {
                    //If there eraser tool is selected, erase the block at the current block handle position
                    RemoveBlock( LevelEditorE06CubeHandle.CurrentHandlePosition );
                }

                if( LevelEditorE07ToolsMenu.SelectedTool == 2 )
                {
                    //If the paint tool is selected, create a new block at the current block handle position
                    AddBlock( LevelEditorE06CubeHandle.CurrentHandlePosition );
                }
            }
        }

        //If we press escape we want to automatically deselect our own painting or erasing tools
        if( Event.current.type == EventType.keyDown &&
            Event.current.keyCode == KeyCode.Escape )
        {
            LevelEditorE07ToolsMenu.SelectedTool = 0;
        }

        //Add our controlId as default control so it is being picked instead of Unitys default SceneView behaviour
        HandleUtility.AddDefaultControl( controlId );
    }

    //Create a new basic cube at the given position
    public static void AddBlock( Vector3 position )
    {
        GameObject newCube = GameObject.CreatePrimitive( PrimitiveType.Cube );
        newCube.transform.parent = LevelParent;
        newCube.transform.position = position;
        newCube.AddComponent<BoxCollider>();
        newCube.tag = "LevelCube";
        newCube.layer = LayerMask.NameToLayer( "Level" );
        newCube.GetComponent<Renderer>().material = (Material)AssetDatabase.LoadAssetAtPath( "Assets/Shared Resources/Materials/Grid@GreenBlue.mat", typeof( Material ) );

        //Make sure a proper Undo/Redo step is created. This is a special type for newly created objects
        Undo.RegisterCreatedObjectUndo( newCube, "Add Cube" );

        //Mark the scene as dirty so it is being saved the next time the user saves
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }

    //Remove a gameobject that is close to the given position
    public static void RemoveBlock( Vector3 position )
    {
        for( int i = 0; i < LevelParent.childCount; ++i )
        {
            float distanceToBlock = Vector3.Distance( LevelParent.GetChild( i ).transform.position, position );
            if( distanceToBlock < 0.1f )
            {
                //Use Undo.DestroyObjectImmediate to destroy the object and create a proper Undo/Redo step for it
                Undo.DestroyObjectImmediate( LevelParent.GetChild( i ).gameObject );

                //Mark the scene as dirty so it is being saved the next time the user saves
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
                return;
            }
        }
    }

    //I will use this type of function in many different classes. Basically this is useful to 
    //be able to draw different types of the editor only when you are in the correct scene so we
    //can have an easy to follow progression of the editor while hoping between the different scenes
    static bool IsInCorrectLevel()
    {
        return UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "GameE08";
    }
}
