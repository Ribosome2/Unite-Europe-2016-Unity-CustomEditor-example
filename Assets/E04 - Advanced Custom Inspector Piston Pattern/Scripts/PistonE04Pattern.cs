using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This component allows the user to create a time based movement pattern for the piston
[RequireComponent( typeof( PistonE03 ) )]
public class PistonE04Pattern : MonoBehaviour 
{
    public float DelayPatternAtBeginning;
    public List<PistonStatePattern> Pattern = new List<PistonStatePattern>();

    PistonE03 m_Piston;
    int m_CurrentPatternIndex = -1;
    float m_CurrentTimeInPatternEntry;

    void Awake()
    {
        m_Piston = GetComponent<PistonE03>();
    }

    void Update () 
    {
        UpdateTimeInCurrentPatternEntry();
        UpdatePattern();        
    }

    void UpdateTimeInCurrentPatternEntry()
    {
        m_CurrentTimeInPatternEntry += Time.deltaTime;
    }

    void UpdatePattern()
    {
        if( IsCurrentPatternEntryFinished() == true )
        {
            GotoNextPatternIndex();
            ExecutePatternEntry();
            ResetCurrentTimeInPatternEntry();
        }
    }

    void ResetCurrentTimeInPatternEntry()
    {
        m_CurrentTimeInPatternEntry = 0;
    }

    void ExecutePatternEntry()
    {
        m_Piston.SetState( Pattern[ m_CurrentPatternIndex ].Name );
    }

    void GotoNextPatternIndex()
    {
        m_CurrentPatternIndex++;

        if( m_CurrentPatternIndex >= Pattern.Count )
        {
            m_CurrentPatternIndex = 0;
        }
    }

    bool IsCurrentPatternEntryFinished()
    {
        float timeInCurrentPatternEntry = DelayPatternAtBeginning;

        if( m_CurrentPatternIndex >= 0 )
        {
            timeInCurrentPatternEntry = Pattern[ m_CurrentPatternIndex ].DelayAfterwards;
        }

        return m_CurrentTimeInPatternEntry >= timeInCurrentPatternEntry;
    }
}
