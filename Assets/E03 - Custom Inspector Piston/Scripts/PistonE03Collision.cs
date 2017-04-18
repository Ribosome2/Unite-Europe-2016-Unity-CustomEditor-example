using UnityEngine;
using System.Collections;

//Helper class that defines what happens if the marble collides with this
public class PistonE03Collision : MonoBehaviour 
{
    public float HitForce;

    PistonE03 m_Piston;

    void Awake()
    {
        m_Piston = GetComponentInParent<PistonE03>();
    }

    void OnCollisionStay( Collision collision )
    {
        if( collision.collider.tag != "Player" )
        {
            return;
        }

        if( m_Piston.GetMovingSpeed() < 0.001f )
        {
            return;
        }

        if( Vector3.Dot( collision.contacts[ 0 ].normal, m_Piston.GetMovingDirection() ) > -0.5f )
        {
            return;
        }

        collision.rigidbody.velocity = m_Piston.GetMovingDirection() * HitForce + m_Piston.AddForceWhenHittingPlayer;
    }
}
