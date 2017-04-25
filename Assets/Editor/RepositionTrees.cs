using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RepositionTrees : MonoBehaviour 
{
	[MenuItem ("Utility/Align trees to terrain", false, 0)]
	static void Reposition ()
	{
		Transform[] objects;
		LayerMask groundMask = LayerMask.NameToLayer ( "Ground" );
		foreach ( GameObject go in Selection.gameObjects )
		{
			objects = go.GetComponentsInChildren<Transform> ();
			foreach ( Transform t in objects )
			{
				Ray ray = new Ray ( t.position + Vector3.up * 50, -Vector3.up );
				RaycastHit hit;
				Debug.DrawLine ( t.position, t.position-Vector3.up * 100, Color.red, 5 );
				if ( Physics.Raycast ( ray, out hit, 100, 1 << groundMask.value ) )
				{
//					Debug.Log ( hit.collider.name );
					t.position = hit.point;
//					t.rotation = Quaternion.identity;
					Vector3 tangent = Vector3.Cross ( t.right, hit.normal );
					t.rotation = Quaternion.LookRotation ( tangent );
//					t.rotation = Quaternion.LookRotation ( Vector3.forward );
//					t.rotation = Quaternion.LookRotation ( Vector3.up );
//					t.rotation = Quaternion.LookRotation ( hit.normal );
				}
//				} else
//					Debug.Log ( "no hit" );
			}
		}
	}
}