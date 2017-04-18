using UnityEngine;
using UnityEditor;
using System.Collections;

public class PreviewTime 
{
    public static float Time
    {
        get
        {
            if( Application.isPlaying == true )
            {
                return UnityEngine.Time.timeSinceLevelLoad;
            }

            //EditorPrefs is the same as PlayerPrefs but it only works in the editor
            //This way you can store variables persistantly even if you close the editor
            return EditorPrefs.GetFloat( "PreviewTime", 0f );
        }
        set
        {
            EditorPrefs.SetFloat( "PreviewTime", value );
        }
    }
}
