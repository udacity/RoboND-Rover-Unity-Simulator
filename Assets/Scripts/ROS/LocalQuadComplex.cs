using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalQuadComplex : MonoBehaviour
{
	public ComplexQuadController quad;

	void LateUpdate ()
	{
		Vector3 input = new Vector3 ( Input.GetAxis ( "Horizontal" ), Input.GetAxis ( "Thrust" ), Input.GetAxis ( "Vertical" ) );

		float thrust = input.y;
//		float thrust = quad.thrustForce * input.y / 4;// * Time.deltaTime;
//		Vector3 upThrust = transform.up * thrust;
		float forwardTiltMultiplier = input.z > 0 ? Mathf.Lerp ( 1f, 0.8f, input.z ) : 1;
		float backwardTiltMultiplier = input.z < 0 ? Mathf.Lerp ( 1f, 0.8f, -input.z ) : 1;
		float rightTiltMultiplier = input.x > 0 ? Mathf.Lerp ( 1f, 0.8f, input.x ) : 1;
		float leftTiltMultiplier = input.x < 0 ? Mathf.Lerp ( 1f, 0.8f, -input.x ) : 1;

//		rb.AddForceAtPosition ( upThrust * forwardTiltMultiplier * leftTiltMultiplier, frontLeftRotor.position, ForceMode.Acceleration );
//		rb.AddForceAtPosition ( upThrust * forwardTiltMultiplier * rightTiltMultiplier, frontRightRotor.position, ForceMode.Acceleration );
//		rb.AddForceAtPosition ( upThrust * backwardTiltMultiplier * leftTiltMultiplier, rearLeftRotor.position, ForceMode.Acceleration );
//		rb.AddForceAtPosition ( upThrust * backwardTiltMultiplier * rightTiltMultiplier, rearRightRotor.position, ForceMode.Acceleration );

		quad.Steer (
			thrust * forwardTiltMultiplier * leftTiltMultiplier,
			thrust * forwardTiltMultiplier * rightTiltMultiplier,
			thrust * backwardTiltMultiplier * leftTiltMultiplier,
			thrust * backwardTiltMultiplier * rightTiltMultiplier
		);
	}
}