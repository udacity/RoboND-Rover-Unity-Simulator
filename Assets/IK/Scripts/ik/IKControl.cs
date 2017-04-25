using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKControl : MonoBehaviour 
{
	public Transform forearm;
	public Transform hand;
	public Transform target;

	public float transition = 1;
	public float elbowAngle;

	Transform armIK;
	Transform armRotation;

	float upperArmLength;
	float forearmLength;
	float armLength;

	void Start ()
	{
		GameObject armIKGameObject = new GameObject ( "Arm IK" );
		armIK = armIKGameObject.transform;
		armIK.parent = transform;
		GameObject armRotationGameObject = new GameObject ( "Arm Rotation" );
		armRotation = armRotationGameObject.transform;
		armRotation.parent = armIK;
		upperArmLength = Vector3.Distance ( transform.position, forearm.position );
		forearmLength = Vector3.Distance ( forearm.position, hand.position );
		armLength = upperArmLength + forearmLength;
	}

	void LateUpdate()
	{
		//Store rotation before IK.
		Quaternion storeUpperArmRotation = transform.rotation;
		Quaternion storeForearmRotation = forearm.rotation;

		//Upper Arm looks target.
		armIK.position = transform.position;
		armIK.LookAt ( forearm );
		armRotation.position = transform.position;
		armRotation.rotation = transform.rotation;
		armIK.LookAt ( target );
		transform.rotation = armRotation.rotation;

		//Upper Arm IK angle.
		float targetDistance = Vector3.Distance ( transform.position, target.position );
		targetDistance = Mathf.Min ( targetDistance, armLength - 0.00001f );
		float adjacent = ( ( upperArmLength * upperArmLength ) - ( forearmLength * forearmLength ) + ( targetDistance * targetDistance ) ) / ( 2 * targetDistance );
		float angle = Mathf.Acos ( adjacent / upperArmLength ) * Mathf.Rad2Deg;
		transform.RotateAround ( transform.position, transform.forward, -angle );

		//Forearm looks target.
		armIK.position = forearm.position;
		armIK.LookAt ( hand );
		armRotation.position = forearm.position;
		armRotation.rotation = forearm.rotation;
		armIK.LookAt ( target );
		forearm.rotation = armRotation.rotation;

		//Elbow angle.
		transform.RotateAround ( transform.position, target.position - transform.position, elbowAngle );

		//Transition IK rotations with animation rotation.
		transition = Mathf.Clamp01 ( transition );
		transform.rotation = Quaternion.Slerp ( storeUpperArmRotation, transform.rotation, transition );
		forearm.rotation = Quaternion.Slerp ( storeForearmRotation, forearm.rotation, transition );
	}
}