using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : MonoBehaviour
{
	public IRobotController thisRobot;
	public IRobotController followTarget;
	public float minFollowDistance = 3;
	public float breakingDistance = 3.5f;

	Transform followTransform;
	Transform thisTransform;


	// gizmo data
	Vector3 forwardGizmo;
	Vector3 targetGizmo;

	void Awake ()
	{
		if ( followTarget == null )
			enabled = false;
		AnimationEvent e;
	}

	void Start ()
	{
		followTransform = followTarget.robotBody;
		thisTransform = thisRobot.robotBody;
		thisRobot.camera.enabled = false;
	}

	void LateUpdate ()
	{
		Vector3 toTarget = followTransform.position - thisTransform.position;
		toTarget.y = 0;
		float distance = toTarget.magnitude;
		toTarget.Normalize ();
		if ( distance > minFollowDistance )
		{
//			toTarget = toTarget.normalized;
			Vector3 forward = thisTransform.forward;
			forward.y = 0;
			forward = forward.normalized;
			float angleToTarget = Vector3.Angle ( forward, toTarget );
			Vector3 localForward = thisTransform.InverseTransformDirection ( toTarget );
			Vector3 localTargetPos = thisTransform.InverseTransformPoint ( followTransform.position );
			float angleRatio = Mathf.Clamp01 ( angleToTarget / 90 );

			if ( angleToTarget > 0.1f )
			{
				if ( localTargetPos.x > 0 )
					thisRobot.Rotate ( angleRatio );
				else
				if ( localTargetPos.x < 0 )
					thisRobot.Rotate ( -angleRatio );
			}

//			if ( localTargetPos.z > 0 )
			if ( distance > 10 )
				thisRobot.Move ( ( distance + 1 - 10 ) * ( 1 - angleRatio ) ); // lerp walk to run speed
			else
//				if ( distance <= breakingDistance )
				thisRobot.Move ( Mathf.Clamp01 ( Mathf.InverseLerp ( minFollowDistance, breakingDistance, distance ) ) - angleRatio );
//				thisRobot.Move ( 1 - angleRatio );
//			else
//			if ( localTargetPos.z < 0 )
//				thisRobot.Move ( -1 );
			
//			thisRobot.Rotate ( angle );
//			if ( angleToTarget > 0.1f )
//				thisRobot.Rotate ( Mathf.Min ( angleToTarget, thisRobot.hRotateSpeed ) * Time.deltaTime * Mathf.Sign ( localForward.x ) );
//			Debug.Log ( "angle is " + angleToTarget + " localforward x is " + localForward.x );
//			Debug.DrawRay ( thisTransform.position + Vector3.up, forward, Color.blue );
//			Debug.DrawRay ( thisTransform.position + Vector3.up, toTarget, Color.red );
			forwardGizmo = forward;
			targetGizmo = toTarget;
		}
	}

	#if UNITY_EDITOR
	void OnDrawGizmosSelected ()
	{
		if ( !Application.isPlaying || !enabled )
			return;
		Gizmos.color = Color.blue;
		Vector3 pos = thisTransform.position + Vector3.up;
		Gizmos.DrawRay ( pos, forwardGizmo );
		Gizmos.color = Color.red;
		Gizmos.DrawRay ( pos, targetGizmo );
	}
	#endif
}