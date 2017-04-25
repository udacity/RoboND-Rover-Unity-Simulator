using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimConstraint : MonoBehaviour
{
	public Transform aimTarget;
	public bool useAxis = true;
	public Vector3 localAxis = Vector3.up;
	[Tooltip ("Turn speed in degrees/sec. Use 0 for instant aim")]
	public float aimSpeed;
	public bool useLimits;
	public float minAngle;
	public float maxAngle;
	public bool maintainOffset;

	Quaternion initialRotation;
	Quaternion qInitialOffset;
	Quaternion initialLocalRotation;
	Vector3 initialOffset;
	Vector3 localOffset;

	Vector3 offsetAxis;
	float offsetAngle;

	void Start ()
	{
		initialLocalRotation = transform.localRotation;
//		initialRotation = transform.rotation;
		localAxis = localAxis.normalized;
		localOffset = Vector3.zero;
		offsetAngle = 0;
		if ( maintainOffset )
		{
			Vector3 upAxis = transform.TransformDirection ( localAxis );
			Vector3 toTarget = ( aimTarget.position - transform.position ).normalized;
			if ( useAxis )
				toTarget = Vector3.ProjectOnPlane ( toTarget, upAxis ).normalized;
//			localOffset = transform.InverseTransformDirection ( transform.forward - Vector3.forward );
			localOffset = transform.InverseTransformDirection ( toTarget.normalized - transform.forward );
//			localOffset = transform.InverseTransformDirection ( transform.forward - toTarget );
//			initialOffset = ( ( transform.position + transform.forward ) - ( transform.position + toTarget ).normalized );//.normalized;
//			initialOffset = ( transform.forward - toTarget.normalized );//.normalized;
			qInitialOffset = Quaternion.FromToRotation ( toTarget, transform.forward );
			Debug.DrawRay ( transform.position, transform.forward, Color.blue, 5 );
			Debug.DrawRay ( transform.position, toTarget, Color.magenta, 5 );
			Debug.DrawRay ( transform.position, qInitialOffset * transform.up, Color.cyan, 5 );
			Debug.DrawRay ( transform.position, transform.up, Color.green, 5 );
//			Debug.DrawLine ( aimTarget.position, aimTarget.position + , Color.cyan, 5 );
//			Debug.DrawLine ( transform.position + toTarget, transform.position + toTarget + initialOffset, Color.green, 5 );

		} else
			qInitialOffset = Quaternion.identity;
//			initialOffset = Vector3.zero;
//			initialOffset = Quaternion.identity;
	}

	void LateUpdate ()
	{
//		transform.localRotation = initialLocalRotation;
		Vector3 upAxis = transform.TransformDirection ( localAxis );
		Vector3 toTarget = aimTarget.position - transform.position;
		Vector3 offset = transform.TransformDirection ( localOffset );
		Debug.DrawRay ( transform.position, toTarget, Color.gray );
		Debug.DrawRay ( transform.position, offset, Color.white );
//		toTarget = toTarget + offset;
		if ( useAxis )
		{
			toTarget = Vector3.ProjectOnPlane ( toTarget, upAxis ).normalized;
			offset = Vector3.ProjectOnPlane ( offset, upAxis ).normalized;
		}
//		Quaternion targetRot = Quaternion.LookRotation ( toTarget - offset );
		Quaternion q = Quaternion.AngleAxis ( offsetAngle, useAxis ? upAxis : offsetAxis );
		Quaternion targetRot = Quaternion.LookRotation ( toTarget );
		Quaternion qOffset = maintainOffset ? Quaternion.LookRotation ( offset ) : Quaternion.identity;
//		Quaternion qOffset = maintainOffset ? Quaternion.LookRotation ( initialOffset ) : Quaternion.identity;
//		Debug.DrawRay ( transform.position, upAxis, Color.green );
//		Debug.DrawRay ( transform.position, toTarget, Color.magenta );
		Debug.DrawRay ( transform.position, transform.forward, Color.blue );

//		transform.rotation = qInitialOffset * targetRot * initialLocalRotation;
		if ( maintainOffset )
			transform.rotation = qInitialOffset * targetRot;
//			transform.localRotation = initialLocalRotation * targetRot * qInitialOffset;
//			transform.localRotation = qOffset * targetRot * initialLocalRotation;
//			transform.localRotation = qInitialOffset * targetRot * initialLocalRotation;
//			transform.localRotation = q;// * targetRot;
//			transform.localRotation = initialLocalRotation;
		else
			transform.rotation = targetRot;
	}
}