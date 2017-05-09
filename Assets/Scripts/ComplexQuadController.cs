using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexQuadController : MonoBehaviour
{
	public Transform frontLeftRotor;
	public Transform frontRightRotor;
	public Transform rearLeftRotor;
	public Transform rearRightRotor;

	public float moveSpeed = 10;
	public float thrustForce = 25;
	public ForceMode forceMode = ForceMode.Acceleration;

	Rigidbody rb;
	Transform[] rotors;
	float[] forces;

	void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
		rotors = new Transform[4] {
			frontLeftRotor,
			frontRightRotor,
			rearLeftRotor,
			rearRightRotor
		};
		forces = new float[4];
	}

	void LateUpdate ()
	{
		if ( Input.GetButtonDown ( "Quit" ) )
			Application.Quit ();

		if ( Input.GetButtonDown ( "Quad Reorient" ) )
		{
			ResetOrientation ();
		}
//		Vector3 input = new Vector3 ( Input.GetAxis ( "Horizontal" ), Input.GetAxis ( "Thrust" ), Input.GetAxis ( "Vertical" ) );
//
//		float thrust = thrustForce * input.y / 4 * Time.deltaTime;
//		Vector3 upThrust = transform.up * thrust;
//		float forwardTiltMultiplier = input.z > 0 ? Mathf.Lerp ( 1f, 0.8f, input.z ) : 1;
//		float backwardTiltMultiplier = input.z < 0 ? Mathf.Lerp ( 1f, 0.8f, -input.z ) : 1;
//		float rightTiltMultiplier = input.x > 0 ? Mathf.Lerp ( 1f, 0.8f, input.x ) : 1;
//		float leftTiltMultiplier = input.x < 0 ? Mathf.Lerp ( 1f, 0.8f, -input.x ) : 1;

//		rb.AddForceAtPosition ( upThrust * forwardTiltMultiplier * leftTiltMultiplier, frontLeftRotor.position, forceMode );
//		rb.AddForceAtPosition ( upThrust * forwardTiltMultiplier * rightTiltMultiplier, frontRightRotor.position, forceMode );
//		rb.AddForceAtPosition ( upThrust * backwardTiltMultiplier * leftTiltMultiplier, rearLeftRotor.position, forceMode );
//		rb.AddForceAtPosition ( upThrust * backwardTiltMultiplier * rightTiltMultiplier, rearRightRotor.position, forceMode );

		float zAngle = transform.localEulerAngles.z;
		while ( zAngle > 180 )
			zAngle -= 360;
		while ( zAngle < -360 )
			zAngle += 360;
		transform.Rotate ( Vector3.up * -zAngle * Time.deltaTime, Space.World );

//		Vector3 velo = Vector3.ClampMagnitude ( rb.velocity, moveSpeed );
//		rb.velocity = velo;
	}

	void FixedUpdate ()
	{
		Vector3 up = transform.up;
		for ( int i = 0; i < 4; i++ )
			rb.AddForceAtPosition ( up * forces [ i ] * Time.deltaTime, rotors [ i ].position, forceMode );

//		float zAngle = transform.localEulerAngles.z;
//		while ( zAngle > 180 )
//			zAngle -= 360;
//		while ( zAngle < -360 )
//			zAngle += 360;
//		transform.Rotate ( Vector3.up * -zAngle * Time.deltaTime, Space.World );

		Vector3 velo = Vector3.ClampMagnitude ( rb.velocity, moveSpeed );
		rb.velocity = velo;
	}

	// apply thrust
	public void Steer (float frontLeft, float frontRight, float rearLeft, float rearRight)
	{
		forces [ 0 ] = frontLeft * thrustForce;
		forces [ 1 ] = frontRight * thrustForce;
		forces [ 2 ] = rearLeft * thrustForce;
		forces [ 3 ] = rearRight * thrustForce;
	}

	public void ResetOrientation ()
	{
		transform.rotation = Quaternion.identity;
		rb.velocity = Vector3.zero;
	}
}