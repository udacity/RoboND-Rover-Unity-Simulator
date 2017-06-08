using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCam : MonoBehaviour
{
	public Transform camBase;
	public Transform camTransform;

	public float moveSpeed = 5;
	public float rotationSpeed = 180;

	Quaternion baseInitialRotation;
	Vector3 baseInitialPosition;
	Vector3 camInitialPosition;

	void Awake ()
	{
		baseInitialPosition = camBase.position;
		baseInitialRotation = camBase.rotation;
		camInitialPosition = camTransform.localPosition;
	}

	void LateUpdate ()
	{
		Vector2 mouse = new Vector2 ( Input.GetAxis ( "Mouse X" ), Input.GetAxis ( "Mouse Y" ) );

		if ( Input.GetMouseButton ( 0 ) )
		{
			camBase.Rotate ( Vector3.up * mouse.x * rotationSpeed * Time.deltaTime, Space.World );
			camBase.Rotate ( Vector3.right * -mouse.y * rotationSpeed * Time.deltaTime, Space.Self );

		}
		if ( Input.GetMouseButton ( 1 ) )
		{
			Vector3 forward = Vector3.ProjectOnPlane ( camBase.forward, Vector3.up ).normalized * -mouse.y;
//			Vector3 right = Vector3.zero;
			Vector3 right = camBase.right * -mouse.x;
			camBase.Translate ( ( forward + right ) * moveSpeed * Time.deltaTime, Space.World );
		}

		if ( Input.GetKeyDown ( KeyCode.R ) )
		{
			camBase.position = baseInitialPosition;
			camBase.rotation = baseInitialRotation;
			camTransform.localPosition = camInitialPosition;
		}
	}

	void OnGUI ()
	{
		GUIStyle label = GUI.skin.label;
		label.fontSize = 24 * Screen.height / 1080;
		label.alignment = TextAnchor.UpperRight;

		Rect r = new Rect ( 0, Screen.height - 100, Screen.width - 15, 100 );
		GUI.color = Color.black;
		GUI.Label ( new Rect ( r.x + 1, r.y + 1, r.width, r.height ), "LMB - Orient Camera" );
		GUI.color = Color.white;
		GUI.Label ( r, "LMB - Orient Camera" );

		r.y += 30;
		GUI.color = Color.black;
		GUI.Label ( new Rect ( r.x + 1, r.y + 1, r.width, r.height ), "RMB - Move Camera" );
		GUI.color = Color.white;
		GUI.Label ( r, "RMB - Move Camera" );

		r.y += 30;
		GUI.color = Color.black;
		GUI.Label ( new Rect ( r.x + 1, r.y + 1, r.width, r.height ), "R - Reset Camera" );
		GUI.color = Color.white;
		GUI.Label ( r, "R - Reset Camera" );
	}
}