using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

//CanEditMultipleObjects tells Unity that you designed you custom inspector in such a way that
//when multiple objects of the same type are selected they can all be edited at the same time.
//If you use serializedObject Unity does this automatically for you. If you are not using
//serializedObject but, for example, you access the component directly by using the "target"
//variable, you should change your implementation to use the "targets" array instead which contains
//all selected components. The "target" variable only accesses the first component
[CanEditMultipleObjects]
[CustomEditor( typeof( PistonE04Pattern ) )]
public class PistonE04PatternEditor : Editor 
{
    ReorderableList m_List;
    PistonE03 m_Piston;

    //This is called whenever the custom inspector is opened (ie, the gameobject with a PistonE04Pattern component is selected)
    void OnEnable()
    {
        if( target == null )
        {
            return;
        }

        FindPistonComponent();
        CreateReorderableList();
        SetupReoirderableListHeaderDrawer();
        SetupReorderableListElementDrawer();
        SetupReorderableListOnAddDropdownCallback();
    }

    void FindPistonComponent()
    {
        m_Piston = ( target as PistonE04Pattern ).GetComponent<PistonE03>();
    }

    void CreateReorderableList()
    {
        //A ReorderableList is a very fancy implementation to inspect array type variables. Notice that we are
        //using UnityEditorInternal; at the very top. ReorderableList is not usually exposed to the public yet
        //and everything placed in UnityEditorInternal may change until Unity thinks it is ready. But the
        //ReorderableList is so useful that I wanted to demonstrate how to use it here
        //
        //If you want to learn more about ReorderableLists, check out this blog post:
        //http://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/
        m_List = new ReorderableList(
                        serializedObject,
                        serializedObject.FindProperty( "Pattern" ),
                        true, true, true, true );
    }

    void SetupReoirderableListHeaderDrawer()
    {
        //The ReorderableList has a bunch of callbacks that you can override to modify how the list is being drawn
        //We are using drawHeaderCallback to write the table headers "State" and "Delay" for the following list
        //Each callback receives a Rect variable which contains the position where this element is being drawn
        //so you should make sure you use this variable to position your objects relative to this
        m_List.drawHeaderCallback = 
            ( Rect rect ) =>
        {
            EditorGUI.LabelField( 
                new Rect( rect.x, rect.y, rect.width - 60, rect.height ), 
                "State" );
            EditorGUI.LabelField(
                new Rect( rect.x + rect.width - 60, rect.y, 60, rect.height ),
                "Delay" );
        };
    }

    void SetupReorderableListElementDrawer()
    {
        //drawElementCallback defines how each entry to this list should be drawn
        //Again, make sure everything you draw is relative to the Rect variable you receive in this callback
        m_List.drawElementCallback =
            ( Rect rect, int index, bool isActive, bool isFocused ) =>
        {
            var element = m_List.serializedProperty.GetArrayElementAtIndex( index );
            rect.y += 2;

            float delayWidth = 60;
            float nameWidth = rect.width - delayWidth;

            EditorGUI.PropertyField(
                new Rect( rect.x, rect.y, nameWidth - 5, EditorGUIUtility.singleLineHeight ),
                element.FindPropertyRelative( "Name" ), GUIContent.none );

            EditorGUI.PropertyField(
                new Rect( rect.x + nameWidth, rect.y, delayWidth, EditorGUIUtility.singleLineHeight ),
                element.FindPropertyRelative( "DelayAfterwards" ), GUIContent.none );
        };
    }

    void SetupReorderableListOnAddDropdownCallback()
    {
        //onAddDropdownCallback defines what happens when you click the [+] Button at the bottom of the list
        //In our case we want to show a dropdown menu that gives the user predefined options what States they
        //can add to our list.
        m_List.onAddDropdownCallback = 
            ( Rect buttonRect, ReorderableList l ) =>
        {
            if( m_Piston.States == null || m_Piston.States.Count == 0 )
            {
                EditorApplication.Beep();
                EditorUtility.DisplayDialog( "Error", "You don't have any states defined in the PistonE03 component", "Ok" );
                return;
            }

            var menu = new GenericMenu();

            foreach( PistonState state in m_Piston.States )
            {
                menu.AddItem( new GUIContent( state.Name ),
                              false,
                              OnReorderableListAddDropdownClick,
                              state );
            }

            menu.ShowAsContext();
        };
    }

    //This callback is called when the user selects an element in the Add Element dropdown menu
    void OnReorderableListAddDropdownClick( object target ) 
    {
        PistonState state = (PistonState)target;

        int index = m_List.serializedProperty.arraySize;
        m_List.serializedProperty.arraySize++;
        m_List.index = index;

        SerializedProperty element = m_List.serializedProperty.GetArrayElementAtIndex( index );
        element.FindPropertyRelative( "Name" ).stringValue = state.Name;
        element.FindPropertyRelative( "DelayAfterwards" ).floatValue = 0f;

        serializedObject.ApplyModifiedProperties();
    }  

    public override void OnInspectorGUI()
    {
        GUILayout.Space( 5 );

        EditorGUILayout.PropertyField( serializedObject.FindProperty( "DelayPatternAtBeginning" ) );

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();

        m_List.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}