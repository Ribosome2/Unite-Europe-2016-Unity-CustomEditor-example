using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class LevelEditorE09ScriptableObject : Editor 
{
    static Transform m_LevelParent;
    static Transform LevelParent
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

    //Get or Set which Block is selected in our custom menu
    public static int SelectedBlock
    {
        get
        {
            return EditorPrefs.GetInt( "SelectedEditorBlock", 0 );
        }
        set
        {
            EditorPrefs.SetInt( "SelectedEditorBlock", value );
        }
    }

    static LevelBlocks m_LevelBlocks;

    static LevelEditorE09ScriptableObject()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        //Make sure we load our block database. Notice the path here, which means the block database has to be in this specific location so we can find it
        //LoadAssetAtPath is a great way to load an asset from the project
        m_LevelBlocks = AssetDatabase.LoadAssetAtPath<LevelBlocks>( "Assets/E09 - Scriptable Object/LevelBlocks.asset" );
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

        if( m_LevelBlocks == null )
        {
            return;
        }

        DrawCustomBlockButtons( sceneView );
        HandleLevelEditorPlacement();
    }

    static void HandleLevelEditorPlacement()
    {
        if( LevelEditorE07ToolsMenu.SelectedTool == 0 )
        {
            return;
        }

        //This method is very similar to the one in E08. Only the AddBlock function is different

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
                    LevelEditorE08AddAndRemoveObjects.RemoveBlock( LevelEditorE06CubeHandle.CurrentHandlePosition );
                }

                if( LevelEditorE07ToolsMenu.SelectedTool == 2 )
                {
                    if( SelectedBlock < m_LevelBlocks.Blocks.Count )
                    {
                        AddBlock( LevelEditorE06CubeHandle.CurrentHandlePosition, m_LevelBlocks.Blocks[ SelectedBlock ].Prefab );
                    }
                }
            }
        }

        //If we press escape we want to automatically deselect our own painting or erasing tools
        if( Event.current.type == EventType.keyDown &&
            Event.current.keyCode == KeyCode.Escape )
        {
            LevelEditorE07ToolsMenu.SelectedTool = 0;
        }

        HandleUtility.AddDefaultControl( controlId );
    }

    //Draw a list of our custom blocks on the left side of the SceneView
    static void DrawCustomBlockButtons( SceneView sceneView )
    {
        Handles.BeginGUI();

        GUI.Box( new Rect( 0, 0, 110, sceneView.position.height - 35 ), GUIContent.none, EditorStyles.textArea );

        for( int i = 0; i < m_LevelBlocks.Blocks.Count; ++i )
        {
            DrawCustomBlockButton( i, sceneView.position );
        }

        Handles.EndGUI();
    }

    static void DrawCustomBlockButton( int index, Rect sceneViewRect )
    {
        bool isActive = false;

        if( LevelEditorE07ToolsMenu.SelectedTool == 2 && index == SelectedBlock )
        {
            isActive = true;
        }

        //By passing a Prefab or GameObject into AssetPreview.GetAssetPreview you get a texture that shows this object
        Texture2D previewImage = AssetPreview.GetAssetPreview( m_LevelBlocks.Blocks[ index ].Prefab );
        GUIContent buttonContent = new GUIContent( previewImage );

        GUI.Label( new Rect( 5, index * 128 + 5, 100, 20 ), m_LevelBlocks.Blocks[ index ].Name );
        bool isToggleDown = GUI.Toggle( new Rect( 5, index * 128 + 25, 100, 100 ), isActive, buttonContent, GUI.skin.button );

        //If this button is clicked but it wasn't clicked before (ie. if the user has just pressed the button)
        if( isToggleDown == true && isActive == false )
        {
            SelectedBlock = index;
            LevelEditorE07ToolsMenu.SelectedTool = 2;
        }
    }

    public static void AddBlock( Vector3 position, GameObject prefab )
    {
        if( prefab == null )
        {
            return;
        }

        GameObject newCube = (GameObject)PrefabUtility.InstantiatePrefab( prefab );
        newCube.transform.parent = LevelParent;
        newCube.transform.position = position;

        //Make sure a proper Undo/Redo step is created. This is a special type for newly created objects
        Undo.RegisterCreatedObjectUndo( newCube, "Add " + prefab.name );

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }

    //I will use this type of function in many different classes. Basically this is useful to 
    //be able to draw different types of the editor only when you are in the correct scene so we
    //can have an easy to follow progression of the editor while hoping between the different scenes
    static bool IsInCorrectLevel()
    {
        return UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "GameE09";
    }
}
