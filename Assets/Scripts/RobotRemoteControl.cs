using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRemoteControl : MonoBehaviour
{
	public float ThrottleInput { get; set; }
	public float BrakeInput { get; set; }
	public float SteeringAngle { get; set; }
	public float VerticalAngle { get; set; }

	public FPSRobotInput manualInput;
	public IRobotController robot;
	bool useFixedUpdate;

	void Awake ()
	{
		useFixedUpdate = robot.GetType () == typeof (RoverController);
		if ( manualInput.controllable )
		{
			enabled = false;
			return;
		}
	}

	void LateUpdate ()
	{
		if ( manualInput.controllable )
			return;
		if ( robot.IsTurningInPlace )
			return;
		float throttle = ThrottleInput;
		float brake = BrakeInput;
		float steer = SteeringAngle;
		robot.Move ( throttle, brake );
		robot.RotateRaw ( steer );
//		robot.Rotate ( steer );
	}

	public void FixedTurn (float angle, float time = 0)
	{
		robot.FixedTurn ( angle, time );
	}

	public void PickupSample ()
	{
		robot.PickupObjective ( null );
	}

	public void Stop ()
	{
		robot.Stop ();
	}

//	void FixedUpdate ()
//	{
//		if ( useFixedUpdate )
//		{
//			float throttle = ThrottleInput;
//			float steer = SteeringAngle;
//			robot.Move ( throttle );
//			robot.Rotate ( steer );
//			robot.RotateCamera ( 0, VerticalAngle );
//		}
//	}
}