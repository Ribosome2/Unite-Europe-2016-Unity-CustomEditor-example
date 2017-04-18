using UnityEngine;
using System.Collections;

public class GameCameraE01 : MonoBehaviour
{
    public float SafeFrameTop;
    public float SafeFrameBottom;

    public float Speed;
    public float DistanceSpeedModifier;

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

    void UpdateHeightPosition()
    {
        Vector3 position = transform.position;
        Vector3 targetViewportPoint = Camera.WorldToViewportPoint( m_FollowTarget.transform.position );

        if( targetViewportPoint.y > SafeFrameTop )
        {
            float distance = Mathf.Abs( targetViewportPoint.y - SafeFrameTop );

            position.y += ( Speed + distance * DistanceSpeedModifier ) * Time.deltaTime;
        }
        else if( targetViewportPoint.y < SafeFrameBottom )
        {
            float distance = Mathf.Abs( targetViewportPoint.y - SafeFrameBottom );

            position.y -= ( Speed + distance * DistanceSpeedModifier ) * Time.deltaTime;
        }

        transform.position = position;
    }

    //OnDrawGizmos() is a magic function that is called by Unity when the Editors SceneView is rendered.
    //This way we can draw Debug Data right into the SceneView. If you press the "Gizmos" button in the
    //GameView these Gizmos are also drawn into the GameView so you can see your visual debug objects while
    //playing the game
    void OnDrawGizmos()
    {
        DrawSafeFrameGizmos();
    }

    void DrawSafeFrameGizmos()
    {
        Vector3 corner1 = Camera.ViewportToWorldPoint( new Vector3( 0f, SafeFrameTop, 1f ) );
        Vector3 corner2 = Camera.ViewportToWorldPoint( new Vector3( 1f, SafeFrameTop, 1f ) );
        Vector3 corner3 = Camera.ViewportToWorldPoint( new Vector3( 0f, SafeFrameBottom, 1f ) );
        Vector3 corner4 = Camera.ViewportToWorldPoint( new Vector3( 1f, SafeFrameBottom, 1f ) );

        Gizmos.color = Color.green;
        Gizmos.DrawLine( corner1, corner2 );
        Gizmos.DrawLine( corner3, corner4 );
        
    }
}
