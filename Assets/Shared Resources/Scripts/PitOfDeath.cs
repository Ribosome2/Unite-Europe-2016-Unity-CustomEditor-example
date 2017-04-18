using UnityEngine;
using System.Collections;

public class PitOfDeath : MonoBehaviour
{
    BoxCollider m_BoxCollider;
    BoxCollider BoxCollider
    {
        get
        {
            if( m_BoxCollider == null )
            {
                m_BoxCollider = GetComponent<BoxCollider>();
            }

            return m_BoxCollider;
        }
    }

    void OnTriggerEnter( Collider collider )
    {
        if( collider.tag == "Player" )
        {
            Marble marble = collider.GetComponent<Marble>();

            if( marble.EnableMovement == true )
            {
                marble.ResetPosition();
            }
        }
    }

    //OnDrawGizmos() is a magic function that is called by Unity when the Editors SceneView is rendered.
    //This way we can draw Debug Data right into the SceneView
    void OnDrawGizmos()
    {
        Gizmos.color = new Color( 1f, 0f, 0f, 1f );
        Gizmos.DrawWireCube( transform.position + BoxCollider.center, BoxCollider.size );

        Gizmos.color = new Color( 1f, 0f, 0f, 0.3f );
        Gizmos.DrawCube( transform.position + BoxCollider.center, BoxCollider.size );
    }

    //OnDrawGizmosSelect() is similar to OnDrawGizmos(). However this method is used instead of OnDrawGizmos()
    //if the GameObject this script belongs to is selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color( 1f, 1f, 0f, 1f );
        Gizmos.DrawWireCube( transform.position + BoxCollider.center, BoxCollider.size );

        Gizmos.color = new Color( 1f, 1f, 0f, 0.3f );
        Gizmos.DrawCube( transform.position + BoxCollider.center, BoxCollider.size );
    }
}
