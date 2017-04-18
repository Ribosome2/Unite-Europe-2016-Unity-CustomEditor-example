using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;

public class OpenSceneMenuItem 
{
    [MenuItem( "Open Scene/E01 - Basic Gizmos" )]
    public static void OpenE01()
    {
        OpenScene( "GameE01" );
    }

    [MenuItem( "Open Scene/E02 - Custom Inspector Camera" )]
    public static void OpenE02()
    {
        OpenScene( "GameE02" );
    }

    [MenuItem( "Open Scene/E03 - Custom Inspector Piston" )]
    public static void OpenE03()
    {
        OpenScene( "GameE03" );
    }

    [MenuItem( "Open Scene/E04 - Advanced Custom Inspector Piston Pattern" )]
    public static void OpenE04()
    {
        OpenScene( "GameE04" );
    }

    [MenuItem( "Open Scene/E05 - EditorWindow" )]
    public static void OpenE05()
    {
        OpenScene( "GameE05" );
    }

    [MenuItem( "Open Scene/E06 - Handles" )]
    public static void OpenE06()
    {
        OpenScene( "GameE06" );
    }

    [MenuItem( "Open Scene/E07 - Handles GUI" )]
    public static void OpenE07()
    {
        OpenScene( "GameE07" );
    }

    [MenuItem( "Open Scene/E08 - Add and Remove Objects" )]
    public static void OpenE08()
    {
        OpenScene( "GameE08" );
    }

    [MenuItem( "Open Scene/E09 - Scriptable Object" )]
    public static void OpenE09()
    {
        OpenScene( "GameE09" );
    }

    static void OpenScene( string name )
    {
        if( EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == true )
        {
            EditorSceneManager.OpenScene( "Assets/" + name + ".unity" );
        }
    }
}
