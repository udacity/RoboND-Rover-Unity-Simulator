using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCamera : MonoBehaviour
{
	public float moveSpeed = 25;
	public float rotationSpeed = 180;
	public float sprintFactor = 4;

	Rigidbody rb;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void LateUpdate ()
	{
		Vector3 move = new Vector3 ( Input.GetAxis ( "Horizontal" ), Input.GetAxis ( "Climb_Descend" ), Input.GetAxis ( "Vertical" ) );
		Vector2 rotation = new Vector2 ( Input.GetAxis ( "Mouse X" ), Input.GetAxis ( "Mouse Y" ) );


		if ( Input.GetKey ( KeyCode.LeftShift ) || Input.GetKey ( KeyCode.RightShift ) )
			move *= sprintFactor;
		move = transform.TransformDirection ( move );
		rb.velocity = move * moveSpeed;
//		rb.MovePosition ( transform.position + move * moveSpeed * Time.deltaTime );
//		rb.AddForce ( move * moveSpeed * Time.deltaTime, ForceMode.Acceleration );
//		transform.Translate ( move * moveSpeed * Time.deltaTime, Space.Self );
		transform.Rotate ( Vector3.up * rotation.x * rotationSpeed * Time.deltaTime, Space.World );
		transform.Rotate ( Vector3.right * -rotation.y * rotationSpeed * Time.deltaTime, Space.Self );

		if ( Input.GetKeyDown ( KeyCode.Escape ) )
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if ( Input.GetMouseButtonDown ( 0 ) )
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
}