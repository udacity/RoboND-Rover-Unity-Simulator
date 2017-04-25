using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : IRobotController
{
//	public override Vector3 GroundVelocity { get { return new Vector3 ( controller.velocity.x, 0, controller.velocity.z ); } }
//	public override Vector3 VerticalVelocity { get { return new Vector3 ( 0, controller.velocity.y, 0 ); } }
//	public override float SteerAngle { get { return robotBody.eulerAngles.y; } }
	public override float Zoom { get { return cameraDefaultFOV / camera.fieldOfView; } }

//	public Transform robotBody;
//	public Transform cameraHAxis;
//	public Transform cameraVAxis;
//	public Transform fpsPosition;
//	public Transform tpsPosition;
//	public Transform actualCamera;
//	public Camera camera;
	public CharacterController controller;

	public float cameraMinAngle;
	public float cameraMaxAngle;
	public float cameraMinFOV;
	public float cameraMaxFOV;
	public float cameraDefaultFOV = 60;

	int curCamera;

	void Awake ()
	{
		actualCamera.SetParent ( fpsPosition );
		ResetZoom ();
	}

	void Update ()
	{
		Speed = new Vector3 ( controller.velocity.x, 0, controller.velocity.z ).magnitude;
	}

	public override void Move (float input)
	{
		ThrottleInput = input;
		controller.SimpleMove ( robotBody.forward * input * moveSpeed );
	}

	public override void Move (Vector3 direction)
	{
		direction *= moveSpeed;
		direction.y = Physics.gravity.y * Time.deltaTime;
		controller.Move ( direction );
	}

	public override void Rotate (float angle)
	{
		SteerAngle = angle;
		robotBody.Rotate ( Vector3.up * angle );
	}

	public override void RotateCamera (float horizontal, float vertical)
	{
//		cameraHAxis.Rotate ( Vector3.up * horizontal, Space.World );
//		cameraVAxis.Rotate ( Vector3.right * -vertical, Space.Self );
		Vector3 euler = cameraVAxis.localEulerAngles;
		euler.x -= vertical;
		if ( euler.x > 270 )
			euler.x -= 360;
		euler.x = Mathf.Clamp ( euler.x, cameraMinAngle, cameraMaxAngle );
		cameraVAxis.localEulerAngles = euler;
	}

	public override void ZoomCamera (float amount)
	{
		camera.fieldOfView += amount * cameraZoomSpeed;
		camera.fieldOfView = Mathf.Clamp ( camera.fieldOfView, cameraMinFOV, cameraMaxFOV );
	}

	public override void ResetZoom ()
	{
		camera.fieldOfView = 60;
//		camera.ResetFieldOfView ();
	}

	public override void SwitchCamera ()
	{
		if ( curCamera == 0 )
		{
			curCamera = 1;
			actualCamera.SetParent ( tpsPosition, false );
		} else
		{
			curCamera = 0;
			actualCamera.SetParent ( fpsPosition, false );
		}
	}

	public override Vector3 TransformDirection (Vector3 localDirection)
	{
		return robotBody.TransformDirection ( localDirection );
	}
}