using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{
    public float Speed;
    public float DistanceSpeedModifier;

    [Space( 10 )]
    public float MaximumHeight;
    public float MinimumHeight;

    [Header( "Safe Frame" )]
    [Range( 0f, 1f )]
    public float SafeFrameTop;
    [Range( 0f, 1f )]
    public float SafeFrameBottom;

    GameObject m_FollowTarget;


    //I use this pattern alot so I don't have to worry about getting a component first in the script
    //Since we are using this in the editor no Awake() or similar functions are called where we could
    //store the component in m_Camera. This way we simply get it the first time we are trying to access it
    Camera m_Camera;
    Camera Camera
    {
        get
        {
            if( m_Camera == null )
            {
                m_Camera = GetComponent<Camera>();
            }

            return m_Camera;
        }
    }

    void Start()
    {
        m_FollowTarget = GameObject.FindWithTag( "Player" );
    }

    void LateUpdate()
    {
        if( m_FollowTarget == null )
        {
            return;
        }

        UpdateHeightPosition();
    }

    //Here we check where the player is on screen and if we need to change the camera position to keep
    //the marble on screen
    void UpdateHeightPosition()
    {
        Vector3 position = transform.position;

        //Convert the marbles position to viewport coordinates of the camera. Viewport coordinates go from
        //0 to 1 horizontally and vertically. So if the marble is dead center of the screen, the viewport
        //coordinates are 0.5, 0.5. If the marble is outside of the screen the coordinates are either
        //smaller than zero or bigger than 1
        Vector3 targetViewportPoint = Camera.WorldToViewportPoint( m_FollowTarget.transform.position );

        //Check if the marble is above our top safe frame line
        if( targetViewportPoint.y > SafeFrameTop )
        {
            float distance = Mathf.Abs( targetViewportPoint.y - SafeFrameTop );

            position.y += ( Speed + distance * DistanceSpeedModifier ) * Time.deltaTime;
        }
        //and check if it's below our bottom safe frame line
        else if( targetViewportPoint.y < SafeFrameBottom )
        {
            float distance = Mathf.Abs( targetViewportPoint.y - SafeFrameBottom );

            position.y -= ( Speed + distance * DistanceSpeedModifier ) * Time.deltaTime;
        }

        //Make sure the Y position of our camera is never above or below the limits we define
        position.y = Mathf.Clamp( position.y, MinimumHeight, MaximumHeight );

        transform.position = position;
    }

    void OnDrawGizmos()
    {
        DrawHeightGizmos();
        DrawSafeFrameGizmos();
    }

    //Here we draw gizmos to show the Maximum and Minimum height limits of the camera
    void DrawHeightGizmos()
    {
        Vector3 maximumHeightPosition = new Vector3( transform.position.x, MaximumHeight, transform.position.z );
        Vector3 minimumHeightPosition = new Vector3( transform.position.x, MinimumHeight, transform.position.z );

        Gizmos.color = new Color( 0f, 1f, 1f, 1f );
        Gizmos.DrawLine( maximumHeightPosition, minimumHeightPosition );
        Gizmos.DrawWireCube( maximumHeightPosition, new Vector3( 0.5f, 0.01f, 0.5f ) );
        Gizmos.DrawWireCube( minimumHeightPosition, new Vector3( 0.5f, 0.01f, 0.5f ) );

        Gizmos.color = new Color( 0f, 1f, 1f, 0.4f );
        Gizmos.DrawCube( maximumHeightPosition, new Vector3( 0.5f, 0.01f, 0.5f ) );
        Gizmos.DrawCube( minimumHeightPosition, new Vector3( 0.5f, 0.01f, 0.5f ) );
    }

    //This method draws the safe frame lines so we can visualize them in the game view
    void DrawSafeFrameGizmos()
    {
        //Again, using viewport coordinates to find world positions relative to our camera view
        Vector3 corner1 = Camera.ViewportToWorldPoint( new Vector3( 0f, SafeFrameTop, 1f ) );
        Vector3 corner2 = Camera.ViewportToWorldPoint( new Vector3( 1f, SafeFrameTop, 1f ) );
        Vector3 corner3 = Camera.ViewportToWorldPoint( new Vector3( 0f, SafeFrameBottom, 1f ) );
        Vector3 corner4 = Camera.ViewportToWorldPoint( new Vector3( 1f, SafeFrameBottom, 1f ) );

        Gizmos.color = Color.green;
        Gizmos.DrawLine( corner1, corner2 );
        Gizmos.DrawLine( corner3, corner4 );
        
    }
}
