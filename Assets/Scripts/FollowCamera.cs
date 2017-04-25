using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	public Transform target;
	public float followDistance = 5;
	public float height = 4;

	Vector3 toTarget;
	Vector3 toCamera;
	Vector3 targetForward;

	void Awake ()
	{
		toTarget = target.InverseTransformDirection ( target.position - transform.position ).normalized;
		toCamera = target.InverseTransformDirection ( transform.position - target.position ).normalized;
		targetForward = transform.InverseTransformDirection ( target.forward ).normalized;
	}

	void LateUpdate ()
	{
		Vector3 followPoint = target.position + Vector3.up * height;
		Vector3 forward = transform.TransformDirection ( transform.up.y >= 0 ? targetForward : -targetForward ) * followDistance;
//		Vector3 forward = target.TransformDirection ( toTarget ) * followDistance;
		transform.transform.position = followPoint - forward;
		Vector3 euler = transform.eulerAngles;
		euler.y = target.eulerAngles.y;
		transform.eulerAngles = euler;
	}
}