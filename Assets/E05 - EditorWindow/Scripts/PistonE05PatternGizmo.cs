using UnityEngine;
using UnityEditor;
using System.Collections;

[RequireComponent( typeof( PistonE04Pattern ) )]
public class PistonE05PatternGizmo : MonoBehaviour 
{
    PistonE04Pattern m_Pattern;
    PistonE03 m_Piston;

    int m_LastPatternIndex = -1;
    int m_CurrentPatternIndex = -1;
    float m_CurrentTimeInPatternEntry;
    private int stateIndex = 0;


    //All of the functionality in this class only happens in OnDrawGizmos, which is called when the SceneView is being rendered
    void OnDrawGizmos()
    {
        //Make sure all components are referenced correctly
        SetupReferences();

        //Calculate where the piston would be at the given PreviewTime.time (our own Editor time class)
        CalculateCurrentPatternState();

        //Draw the preview gizmo at the given position
        DrawGizmo();
    }

    int FindStateIndexFromPatternIndex( int patternIndex )
    {
        if( patternIndex == -1 )
        {
            return -1;
        }

        return m_Piston.States.FindIndex( item => item.Name == m_Pattern.Pattern[ patternIndex ].Name );
    }

    void DrawGizmo()
    {
        Vector3 position = transform.position;

        //Find the actual position of the cube at any given PreviewTime.time
        if( m_CurrentPatternIndex >= 0 )
        {
            int lastStateIndex = FindStateIndexFromPatternIndex( m_LastPatternIndex );

            if( stateIndex >= 0 )
            {
                Vector3 lastStatePosition = Vector3.zero;

                if( lastStateIndex >= 0 )
                {
                    lastStatePosition = m_Piston.States[ lastStateIndex ].Position;
                }

                float approximateLerpModifier = 0.6f;

                position += Vector3.Lerp(
                        lastStatePosition,
                        m_Piston.States[ stateIndex ].Position,
                        m_CurrentTimeInPatternEntry * approximateLerpModifier * m_Piston.Speed );
            }
        }

        Gizmos.color = new Color( 1f, 1f, 0f, 1f );
        Gizmos.DrawWireCube( position, Vector3.one * 0.9f );

        Gizmos.color = new Color( 1f, 1f, 0f, 0.3f );
        Gizmos.DrawCube( position, Vector3.one * 0.9f );
    }

    void SetupReferences()
    {
        if( m_Pattern == null )
        {
            m_Pattern = GetComponent<PistonE04Pattern>();
        }

        if( m_Piston == null )
        {
            m_Piston = GetComponent<PistonE03>();
        }
    }

    void CalculateCurrentPatternState()
    {
        int oldPatternIndex = m_CurrentPatternIndex;

        m_CurrentPatternIndex = -1;
        m_CurrentTimeInPatternEntry = PreviewTime.Time;

        if( PreviewTime.Time < m_Pattern.DelayPatternAtBeginning || m_Pattern.Pattern.Count == 0 )
        {
            m_LastPatternIndex = -1;
            return;
        }

        m_CurrentPatternIndex = 0;
        m_CurrentTimeInPatternEntry -= m_Pattern.DelayPatternAtBeginning;

        //Iterate through the pattern to find the pattern entry which is being used at PreviewTime.time
        while( m_CurrentTimeInPatternEntry > m_Pattern.Pattern[ m_CurrentPatternIndex ].DelayAfterwards )
        {
            m_CurrentTimeInPatternEntry -= m_Pattern.Pattern[ m_CurrentPatternIndex ].DelayAfterwards;
            m_CurrentPatternIndex++;

            if( m_CurrentPatternIndex >= m_Pattern.Pattern.Count )
            {
                m_CurrentPatternIndex = 0;
            }
        }

        if( m_CurrentPatternIndex != oldPatternIndex )
        {
            m_LastPatternIndex = oldPatternIndex;
        }
    }
}
