using UnityEngine;
using UnityEditor;
using System.Collections;

//EditorWindow is another useful Editor Class. This way you can define your own windows
//that can be moved and docked just like any default Unity windows
public class PreviewPlaybackWindow : EditorWindow 
{
    //Create a Menu Item so we can open this window
    [MenuItem( "Window/Preview Playback Window" )]
    static void OpenPreviewPlaybackWindow()
    {
        EditorWindow.GetWindow<PreviewPlaybackWindow>( false, "Playback" );
    }

    float m_PlaybackModifier;
    float m_LastTime;

    void OnEnable()
    {
        //This update callback is called 30 times per second in the editor. Basically it's 
        //an Update() function you can use at edit time
        EditorApplication.update -= OnUpdate;
        EditorApplication.update += OnUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
    }

    void OnUpdate()
    {
        if( m_PlaybackModifier != 0f )
        {
            PreviewTime.Time += ( Time.realtimeSinceStartup - m_LastTime ) * m_PlaybackModifier;

            //If the preview time changes, make sure you Repaint this window so you can see it immediately. Otherwise Unity
            //will only call Repaint if it determines the window needs to be redrawn. For example if you move it
            Repaint();

            //Since we are previewing data in the SceneView we also want to make sure it is updated each time the preview has changed
            SceneView.RepaintAll();
        }

        m_LastTime = Time.realtimeSinceStartup;
    }

    void OnGUI()
    {
        float seconds = Mathf.Floor( PreviewTime.Time % 60 );
        float minutes = Mathf.Floor( PreviewTime.Time / 60 );

        GUILayout.Label( "Preview Time: " + minutes + ":" + seconds.ToString( "00" ) );
        GUILayout.Label( "Playback Speed: " + m_PlaybackModifier );

        GUILayout.BeginHorizontal();
        {
            if( GUILayout.Button( "|<", GUILayout.Height( 30 ) ) )
            {
                PreviewTime.Time = 0f;
                SceneView.RepaintAll();
            }

            if( GUILayout.Button( "<<", GUILayout.Height( 30 ) ) )
            {
                m_PlaybackModifier = -5f;
            }

            if( GUILayout.Button( "<", GUILayout.Height( 30 ) ) )
            {
                m_PlaybackModifier = -1f;
            }

            if( GUILayout.Button( "||", GUILayout.Height( 30 ) ) )
            {
                m_PlaybackModifier = 0f;
            }

            if( GUILayout.Button( ">", GUILayout.Height( 30 ) ) )
            {
                m_PlaybackModifier = 1f;
            }

            if( GUILayout.Button( ">>", GUILayout.Height( 30 ) ) )
            {
                m_PlaybackModifier = 5f;
            }
        }
        GUILayout.EndHorizontal();
    }
}
