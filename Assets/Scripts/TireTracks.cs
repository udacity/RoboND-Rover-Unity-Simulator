using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireTracks : MonoBehaviour
{
	public WheelCollider wheelCollider;
	public Material material;
//	public Vector3 forward = Vector3.forward;
	public float width = 0.2f;
	public float trackDistance = 0.05f;
	public int trackCount = 200;

	MeshFilter filter;
	MeshRenderer rend;
	Mesh mesh;

	LayerMask groundMask;
	Vector3 forward;
	Vector3 right;
	Vector3 lastPosition;
	float groundDistance;

	List<Vector3> verts;
	List<int> triangles;
	List<Vector2> uvs;
	int curCount;

	void Awake ()
	{
		GameObject tracks = new GameObject ( "Tracks" );
//		tracks.transform.SetParent ( transform, false );
		filter = tracks.AddComponent<MeshFilter> ();
		mesh = new Mesh ();
		mesh.name = "Tracks";
		filter.mesh = mesh;
		rend = tracks.AddComponent<MeshRenderer> ();
		rend.material = material;
//		right = Vector3.Cross ( forward, Vector3.up );
		groundMask = LayerMask.GetMask ( "Ground", "Obstacles" );
		forward = transform.forward;
		right = transform.right;
		verts = new List<Vector3> ();
		triangles = new List<int> ();
		uvs = new List<Vector2> ();
	}

	void Update ()
	{
		if ( !wheelCollider.isGrounded )
			return;
		// check the track distance every update
		float sqDistance = ( transform.position - lastPosition ).sqrMagnitude;
		if ( sqDistance > trackDistance * trackDistance )
		{
			// find position on ground
			Ray ray = new Ray ( transform.position, Vector3.down );
			RaycastHit hit;
			if ( Physics.Raycast ( ray, out hit, 1, groundMask.value ) )
			{
				forward = Vector3.Cross ( Vector3.up, -transform.right );
				Vector3 point = hit.point;
				point.y += 0.01f;
				Vector3[] newVerts = new Vector3[] {
//					-forward * width - transform.right * width,
//					-forward * width + transform.right * width,
//					forward * width + transform.right * width,
//					forward * width - transform.right * width
					point - forward * width - transform.right * width,
					point - forward * width + transform.right * width,
					point + forward * width + transform.right * width,
					point + forward * width - transform.right * width
				};
				int count = verts.Count;
				verts.AddRange ( newVerts );
				Vector2[] newUVs = new Vector2[] {
					Vector2.zero,
					Vector2.right,
					Vector2.one,
					Vector2.up
				};
				uvs.AddRange ( newUVs );

				int[] tris = new int[] {
					count, count + 2, count + 1,
					count, count + 3, count + 2
				};
				triangles.AddRange ( tris );
				mesh.Clear ();
				mesh.SetVertices ( verts );
				mesh.SetUVs ( 0, uvs );
				mesh.SetTriangles ( triangles, 0, true );
//				Debug.DrawLine ( hit.point, hit.point + forward * width/2, Color.green, 2 );
//				Debug.DrawLine ( hit.point, hit.point + transform.right * width/2, Color.magenta, 2 );
				curCount++;
			}

			lastPosition = transform.position;

			// every 50 extra tracks, clear some out. using 50 as a buffer so we're not removing from the list every track
			if ( curCount > trackCount + 50 )
			{
//				Debug.Log ( "cur " + curCount + " track " + trackCount );
//				Debug.Log ( "4c-t-1 is " + 4 * ( curCount - trackCount - 1 ) );
				verts = verts.GetRange ( 4 * ( curCount - trackCount - 1 ), 4 * trackCount );
//				Debug.Log ( "vert count " + verts.Count );
				uvs = uvs.GetRange ( 4 * ( curCount - trackCount - 1 ), 4 * trackCount );
//				Debug.Log ( "uv count " + uvs.Count );
				triangles.Clear ();
				for ( int i = 0; i < trackCount; i++ )
					triangles.AddRange ( new int[] {
						4*i, 4*i + 2, 4*i + 1,
						4*i, 4*i + 3, 4*i + 2
					} );
//				Debug.Log ( "tri count " + triangles.Count );
				curCount = trackCount;

			} else
			if ( curCount > trackCount )
			{
				// until when we reach the desired count, start hiding tracks to make them disappear one by one
				for ( int i = 1; i < 7; i++ )
				{
					int tri = 6 * ( curCount - trackCount ) - i;
					triangles [ tri ] = 0;
				}
			}
		}
	}

	#if UNITY_EDITOR
//	void OnDrawGizmosSelected ()
//	{
//		if ( UnityEditor.SceneView.currentDrawingSceneView == null || UnityEditor.SceneView.currentDrawingSceneView.camera == null )
//			return;
//		
//		Gizmos.color = Color.blue;
//		Gizmos.DrawRay ( transform.position, forward );
//		Gizmos.color = Color.red;
//		Gizmos.DrawRay ( transform.position, transform.right );
//	}
	#endif
}