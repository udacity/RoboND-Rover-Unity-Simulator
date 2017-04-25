using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointConstraint : MonoBehaviour
{
	public Transform target1;
	public Transform target2;
	[Range (0, 1)]
	public float weight;
	public bool maintainOffset;

	Vector3 offset1;
	Vector3 offset2;

	void Start ()
	{
		if ( target2 == null )
			target2 = target1;
		if ( maintainOffset )
		{
			offset1 = target1.position - transform.position;
			offset2 = target2.position - transform.position;

		} else
			offset1 = offset2 = Vector3.zero;
	}

	void LateUpdate ()
	{
		weight = Mathf.Clamp01 ( weight );
		Vector3 pos1 = target1.position - offset1;
		Vector3 pos2 = target2.position - offset2;
		Vector3 targetPos = Vector3.Lerp ( pos1, pos2, weight );
		transform.position = targetPos;
	}
}