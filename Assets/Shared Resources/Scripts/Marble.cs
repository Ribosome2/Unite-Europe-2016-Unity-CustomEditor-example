using UnityEngine;
using System.Collections;

public class Marble : MonoBehaviour 
{
    public float Force;
    public bool EnableMovement = true;
    Rigidbody m_Body;
    Vector3 m_StartPosition;
    

    void Awake() 
    {
        m_Body = GetComponent<Rigidbody>();
        m_StartPosition = transform.position;
    }

    //Using FixedUpdate because we are using Physics to move the marble
    //FixedUpdate() is called every time the physics engine updates so it is the best place
    //to interact with physics
    void FixedUpdate() 
    {
        if( EnableMovement == true )
        {
            UpdateMovement();
        }
        else
        {
            ResetPlayerForces();
        }        
    }

    //Get Horizontal and Vertical player inputs and use them as torque to roll the marble
    void UpdateMovement()
    {
        Vector3 input = new Vector3( -Input.GetAxis( "Horizontal" ), 0f, -Input.GetAxis( "Vertical" ) );

        //We are using angled up and right vectors here because the isometric view is angled at 45 degrees
        //This way, if the player presses "up", the marble moves along the level lines
        Vector3 right = new Vector3( 1f, 0, 1f );
        Vector3 up = new Vector3( -1f, 0, 1f );

        Vector3 newTorque = ( input.x * right + input.z * up ) * Force;

        m_Body.AddTorque( newTorque, ForceMode.VelocityChange );
    }

    //Force the players angular force to zero so the marble stops
    void ResetPlayerForces()
    {
        m_Body.angularVelocity = Vector3.zero;
    }

    //Reset the marble to its start position
    public void ResetPosition()
    {
        transform.position = m_StartPosition;
        m_Body.angularVelocity = Vector3.zero;
        m_Body.velocity = Vector3.zero;
    }
}
