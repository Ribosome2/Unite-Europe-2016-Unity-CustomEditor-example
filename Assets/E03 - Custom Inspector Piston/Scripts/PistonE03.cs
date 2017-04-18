using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PistonE03 : MonoBehaviour 
{
    public float Speed;
    public Vector3 AddForceWhenHittingPlayer;

    //We are hiding this in the inspector because we want to draw our own custom
    //inspector for it.
    [HideInInspector]
    public List<PistonState> States = new List<PistonState>();

    Transform m_Visuals;
    Transform Visuals
    {
        get
        {
            if( m_Visuals == null )
            {
                m_Visuals = transform.GetChild( 0 );
            }

            return m_Visuals;
        }
    }

    Vector3 m_VisualsTargetPosition;
    float m_MovingSpeed;
    Vector3 m_MovingDirection;

    void Awake()
    {
        m_VisualsTargetPosition = Visuals.transform.localPosition;
    }

    void Update()
    {
        UpdateVisualsPosition();
    }

    //Simply move the visuals to a previously designated target position
    void UpdateVisualsPosition()
    {
        //Using Vector3.Lerp instead of Vector3.MoveTowards to get a smoother decceleration. Both are valid options though
        Vector3 newPosition = Vector3.Lerp( Visuals.transform.localPosition, m_VisualsTargetPosition, Speed * Time.deltaTime );

        //Storing the movement speed so we can use it as a force indicator later
        m_MovingSpeed = Vector3.Distance( Visuals.transform.localPosition, newPosition ) / Time.deltaTime;

        if( m_MovingSpeed == 0 )
        {
            m_MovingDirection = Vector3.zero;
        }
        else
        {
            m_MovingDirection = ( newPosition - Visuals.transform.localPosition ).normalized;
        }

        Visuals.transform.localPosition = newPosition;
    }

    //This method tries to find the given state in our list of states and sets the
    //defined position of that state as the target position for our object
    public void SetState( string state )
    {
        for( int i = 0; i < States.Count; ++i )
        {
            if( States[ i ].Name == state )
            {
                SetTargetPosition( States[ i ].Position );
                return;
            }
        }

        Debug.LogWarning( "Couldn't find PistonState '" + state + "'" );
    }

    public float GetMovingSpeed()
    {
        return m_MovingSpeed;
    }

    public Vector3 GetMovingDirection()
    {
        return m_MovingDirection;
    }

    void SetTargetPosition( float x, float y, float z )
    {
        SetTargetPosition( new Vector3( x, y, z ) );
    }

    void SetTargetPosition( Vector3 position )
    {
        m_VisualsTargetPosition = position;

        if( Application.isPlaying == false )
        {
            Visuals.transform.position = m_VisualsTargetPosition;
        }
    }
}
