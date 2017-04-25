using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalQuadSimple : MonoBehaviour
{
	public SimpleQuadController quad;

	void LateUpdate ()
	{
		Vector3 input = new Vector3 ( Input.GetAxis ( "Horizontal" ), Input.GetAxis ( "Thrust" ), Input.GetAxis ( "Vertical" ) );

		quad.Steer ( input.y, input.z, input.x );
	}
}