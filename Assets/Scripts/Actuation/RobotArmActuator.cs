using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmActuator : MonoBehaviour
{
	public Transform baseTransform;
	public Transform shoulder;
	public Transform upperArm;
	public Transform elbow;
	public Transform forearm;
	public Transform wrist;
	public Transform target;
	public PointConstraint handConstraint;
	public Transform foldedPosition;
	public Transform targetPosition;

	public float rotationSpeed = 360;
	public float upperArmMinAngle = 10;
	public float upperArmMaxAngle = 60;
	public float elbowMinAngle = 25;
	public float elbowMaxAngle = 150;

	float upperArmLength;
	float forearmLength;
	float upperArmEuler;
	float elbowEuler;

	public System.Action onArrive;
	bool announceArrive;
	float timeNotMoved;
	Vector3 lastWristPos;

	Vector3 initialFoldedPosition;
	Vector3 initialTargetPosition;


	void Awake ()
	{
		upperArmLength = ( elbow.position - upperArm.position ).magnitude;
		forearmLength = ( wrist.position - elbow.position ).magnitude;
		elbowEuler = 150;
		handConstraint.weight = 0;
		upperArm.localEulerAngles = new Vector3 ( 10, 0, 0 );
		upperArmEuler = 10;
		initialFoldedPosition = foldedPosition.localPosition;
		initialTargetPosition = targetPosition.localPosition;
	}

	void LateUpdate ()
	{
		if ( target == null )
			return;

		bool didMove = false;

		// shoulder
		Vector3 toTarget = ( target.position - shoulder.position ).normalized;
		toTarget = Vector3.ProjectOnPlane ( toTarget, Vector3.up ).normalized;
		Vector3 up = Vector3.up;
		Quaternion targetRot = Quaternion.LookRotation ( toTarget, up );
		shoulder.rotation = Quaternion.RotateTowards ( shoulder.rotation, targetRot, rotationSpeed * Time.deltaTime );
		Debug.DrawLine ( shoulder.position, shoulder.position + toTarget, Color.blue );

		// turn to target first
		if ( Quaternion.Angle ( shoulder.rotation, targetRot ) > 0.1f )
			return;

		// use the law of cosines to find the angles we need
		// law says c² = a² + b² - 2ab*cos(C) where C opposite c
		// so C would be....
		// 2ab*cos(C) = a² + b² - c²
		// cos(C) = (a² + b² - c²) / 2ab
		// and C = Acos ( (a² + b² - c²) / 2ab )


		toTarget = target.position - upperArm.position;
		toTarget = Vector3.ProjectOnPlane ( toTarget, upperArm.right );

		float a = upperArmLength;
		float b = forearmLength; // let alpha opposite b = upper arm angle
		float c = toTarget.magnitude; // let gamma opposite c = forearm angle
		float alpha = Mathf.Acos ( ( c * c + a * a - b * b ) / ( 2 * a * c ) ) * Mathf.Rad2Deg;
		if ( alpha == 0 || float.IsNaN ( alpha ) )
			alpha = 0.1f;
//
		alpha = Vector3.Angle ( Vector3.up, toTarget ) - alpha;
		alpha = Mathf.Clamp ( alpha, upperArmMinAngle, upperArmMaxAngle );
		upperArmEuler = Mathf.MoveTowards ( upperArmEuler, alpha, rotationSpeed * Time.deltaTime );
		if ( upperArmEuler > upperArmMaxAngle || upperArmEuler < upperArmMinAngle )
			Debug.Log ( "alpha: " + alpha + " uae: " + upperArmEuler );
		Vector3 euler = new Vector3 ( upperArmEuler, 0, 0 );
		upperArm.localEulerAngles = euler;

//		// upper arm
//		alpha = Mathf.Clamp ( 90 - alpha, upperArmMinAngle, upperArmMaxAngle );
//		Quaternion q = Quaternion.AngleAxis ( alpha, upperArm.right );
//		targetRot = Quaternion.LookRotation ( toTarget, upperArm.up );
//		targetRot = q * targetRot;
//		upperArm.rotation = Quaternion.RotateTowards ( upperArm.rotation, targetRot, rotationSpeed * Time.deltaTime );
//		Vector3 euler = upperArm.localEulerAngles;
//		euler.x = Mathf.Clamp ( euler.x, upperArmMinAngle, upperArmMaxAngle );
//		euler.y = euler.z = 0;
//		upperArm.localEulerAngles = euler;
		Debug.DrawRay ( upperArm.position, upperArm.forward, Color.blue );
		Debug.DrawRay ( upperArm.position, toTarget, Color.cyan );

		// elbow
		toTarget = target.position - elbow.position;
		toTarget = Vector3.ProjectOnPlane ( toTarget, elbow.right );
		float gamma = Vector3.Angle ( -upperArm.up, toTarget );
		gamma = Mathf.Clamp ( gamma, elbowMinAngle, elbowMaxAngle );
		elbowEuler = Mathf.MoveTowards ( elbowEuler, 180 - gamma, rotationSpeed * Time.deltaTime );
		euler = new Vector3 ( elbowEuler, 0, 0 );
		elbow.localEulerAngles = euler;
		Debug.DrawRay ( elbow.position, toTarget, Color.magenta );
		Debug.DrawRay ( elbow.position, elbow.up, Color.green );
		Debug.DrawRay ( elbow.position, elbow.right, Color.red );
		Debug.DrawRay ( elbow.position, elbow.forward, Color.blue );

		// piston?

		// wrist
		toTarget = shoulder.forward;
//		toTarget = ( target.position - wrist.position ).normalized;
//		if ( toTarget == Vector3.zero )
//			toTarget = wrist.forward;
//		toTarget = Vector3.ProjectOnPlane ( toTarget, Vector3.up ).normalized;
		targetRot = Quaternion.LookRotation ( toTarget );
		wrist.rotation = Quaternion.RotateTowards ( wrist.rotation, targetRot, rotationSpeed * Time.deltaTime );

		if ( wrist.position == lastWristPos )
			timeNotMoved += Time.deltaTime;
		else
			timeNotMoved = 0;
		lastWristPos = wrist.position;

		if ( announceArrive )
		{
			if ( timeNotMoved >= 0.5f )
//			if ( didMove && ( target.position - wrist.position ).sqrMagnitude < 0.01f )
//				didMove = false;
//			if ( !didMove )
			{
				announceArrive = false;
				if ( onArrive != null )
					onArrive ();
			}
		}
	}

	public void Fold (bool announce = false)
	{
		handConstraint.weight = 0;
		announceArrive = announce;
		timeNotMoved = 0;
	}

	public void Unfold (bool announce = false)
	{
		handConstraint.weight = 1;
		announceArrive = announce;
		timeNotMoved = 0;
	}

	public void MoveToTarget (float time = 2, bool backwards = false)
	{
		StartCoroutine ( MoveTo ( time, backwards ) );
	}

	IEnumerator MoveTo (float time, bool backwards = false)
	{
		yield return null;
		float t = backwards ? 1 : 0;
		float target = 1 - t;
		float spd = 1f / time;
		while ( t < time )
		{
			handConstraint.weight = Mathf.MoveTowards ( handConstraint.weight, target, spd * Time.deltaTime );
//			handConstraint.weight += Time.deltaTime * spd;
			if ( handConstraint.weight >= 1 || handConstraint.weight <= 0 )
				break;
			t += Time.deltaTime;
			yield return null;
		}
		handConstraint.weight = target;
		if ( onArrive != null )
			onArrive ();
	}

	public void SetTarget (int target, Vector3 position)
	{
		if ( target == 0 )
			foldedPosition.position = position;
		else
			targetPosition.position = position;
	}

	public void MoveTarget (Vector3 position, float time = 3)
	{
		StartCoroutine ( MoveTo ( position, time ) );
	}

	IEnumerator MoveTo (Vector3 position, float time)
	{
		yield return null;
		float t = 0;
		float spd = 1f / time;
		float dist = ( position - targetPosition.position ).magnitude;
		Vector3 start = targetPosition.position;
		while ( t < 1f )
		{
			targetPosition.position = Vector3.Slerp ( start, position, t );
//			targetPosition.position = Vector3.MoveTowards ( targetPosition.position, position, dist * spd * Time.deltaTime );
			if ( targetPosition.position == position )
				break;
			t += Time.deltaTime * spd;
			yield return null;
		}

		targetPosition.position = position;
//		if ( onArrive != null )
//			onArrive ();
	}

	public void ResetPosition ()
	{
		StartCoroutine ( _Reset () );
	}

	IEnumerator _Reset ()
	{
		handConstraint.weight = 0;
		yield return new WaitForSeconds ( 1.5f );
		foldedPosition.localPosition = initialFoldedPosition;
		yield return new WaitForSeconds ( 1.5f );
		targetPosition.localPosition = initialTargetPosition;
	}
}