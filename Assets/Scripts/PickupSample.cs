using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSample : MonoBehaviour
{
	public Collider Collider;
	public MeshRenderer Renderer;

	public Vector3 GetCenterPosition ()
	{
		return Collider.bounds.center;
	}
}