using UnityEngine;
using System.Collections;

public class VictoryZone : MonoBehaviour
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

    GUIStyle m_WinLabelStyle;
    GUIStyle WinLabelStyle
    {
        get
        {
            if( m_WinLabelStyle == null )
            {
                m_WinLabelStyle = new GUIStyle();
                m_WinLabelStyle.alignment = TextAnchor.MiddleCenter;
                m_WinLabelStyle.fontSize = 50;
                m_WinLabelStyle.fontStyle = FontStyle.Bold;
                m_WinLabelStyle.normal.textColor = Color.white;
            }

            return m_WinLabelStyle;
        }
    }

    bool m_ShowYouWinText = false;

    void OnTriggerEnter( Collider collider )
    {
        if( collider.tag == "Player" )
        {
            StartCoroutine( VictoryRoutine( collider ) );
        }
    }

    IEnumerator VictoryRoutine( Collider playerCollider )
    {
        Marble marble = playerCollider.GetComponent<Marble>();
        marble.EnableMovement = false;

        m_ShowYouWinText = true;

        yield return new WaitForSeconds( 2f );

        ReloadCurrentLevel();
    }

    void ReloadCurrentLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene( UnityEngine.SceneManagement.SceneManager.GetActiveScene().name );
    }

    //OnDrawGizmos() is a magic function that is called by Unity when the Editors SceneView is rendered.
    //This way we can draw Debug Data right into the SceneView
    void OnDrawGizmos()
    {
        Gizmos.color = new Color( 0f, 1f, 0f, 1f );
        Gizmos.DrawWireCube( transform.position + BoxCollider.center, BoxCollider.size );

        Gizmos.color = new Color( 0f, 1f, 0f, 0.3f );
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

    void OnGUI()
    {
        if( m_ShowYouWinText == true )
        {
            GUI.color = Color.black;
            GUI.Label( new Rect( -1f, -1f, Screen.width, Screen.height ), "You Win!", WinLabelStyle );
            GUI.Label( new Rect( -1f, 1f, Screen.width, Screen.height ), "You Win!", WinLabelStyle );
            GUI.Label( new Rect( 1f, -1f, Screen.width, Screen.height ), "You Win!", WinLabelStyle );
            GUI.Label( new Rect( 1f, 1f, Screen.width, Screen.height ), "You Win!", WinLabelStyle );

            GUI.color = Color.white;
            GUI.Label( new Rect( 0f, 0f, Screen.width, Screen.height ), "You Win!", WinLabelStyle );
        }
    }
}
