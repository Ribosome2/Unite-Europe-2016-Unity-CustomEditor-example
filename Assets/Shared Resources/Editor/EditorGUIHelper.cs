using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

public class EditorGUIHelper 
{
    static MethodInfo boldFontMethodInfo = null;

    //Unity has a private method called EdutorGUIUtility.SetBoldDefaultFont which they use internally to
    //show modified prefab values in bold in the inspector. We are using reflection to access this method
    //so we can mirror this behaviour for our custom inspectors.
    //Warning: Using reflection like this is sneaky and can break in future versions if Unity decides to
    //move or rename this method
    public static void SetBoldDefaultFont( bool value )
    {
        if( boldFontMethodInfo == null )
        {
            boldFontMethodInfo = typeof( EditorGUIUtility ).GetMethod( "SetBoldDefaultFont", BindingFlags.Static | BindingFlags.NonPublic );
        }

        boldFontMethodInfo.Invoke( null, new[] { value as object } );
    }
}
