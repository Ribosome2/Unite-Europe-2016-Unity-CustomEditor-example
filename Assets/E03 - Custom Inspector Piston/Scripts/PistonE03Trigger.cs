using UnityEngine;
using System.Collections;

//Helper class that defines what happens when the marble enters the trigger zone of this object
public class PistonE03Trigger : MonoBehaviour 
{
    public string StateOnEnter;
    public string StateOnExit;

    PistonE03 m_Piston;

    void Awake()
    {
        m_Piston = GetComponent<PistonE03>();
    }

    void OnTriggerEnter( Collider collider )
    {
        if( collider.tag == "Player" )
        {
            m_Piston.SetState( StateOnEnter );
        }
    }

    void OnTriggerExit( Collider collider )
    {
        if( collider.tag == "Player" )
        {
            m_Piston.SetState( StateOnExit );
        }
    }
}
