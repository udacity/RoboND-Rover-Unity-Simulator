using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects ;

public class RoverController : IRobotController
{
	Vector3 GroundVelocity { get { return new Vector3 ( rb.velocity.x, 0, rb.velocity.z ); } }
	Vector3 VerticalVelocity { get { return new Vector3 ( 0, rb.velocity.y, 0 ); } }
	public override float Zoom { get { return cameraDefaultFOV / camera.fieldOfView; } }

	public Transform flatbed;
	public Rigidbody rb;
	public WheelCollider[] wheels;
	public Transform[] wheelMeshes;
	public Transform robotArmShoulder;
	public Transform robotGrabPoint;
	public SphereCollider cameraCollider;
	public RobotArmActuator armActuator;
	public List<Transform> storedLocations;

	public float cameraMinAngle;
	public float cameraMaxAngle;
	public float cameraDefaultFOV = 60;
	public float cameraMinFOV;
	public float cameraMaxFOV;

	public float maxReverseSpeed = 2f;
	public float acceleration = 2.5f;
	public float motorTorque;
	public float brakeTorque;
	public float maxSteering = 25;
	public float downForce = 50;

	int curCamera;

	DepthOfField depthOfField;
	Quaternion[] localRotations = new Quaternion[4];
	float lastMoveInput;
	float lastBrakeInput;
	[SerializeField]
	float lastAngle;
	float lastSteerTime;
	float lastSteerInput;
	float targetYaw;
	float fixedTurnSpeed;
	float wheelAngle;

	bool isMotorInput;
	bool isSteeringInput;
	bool isPickingUp;
	bool grabbingObject;
	bool isTurningInPlace;
	bool isFixedTurning;
	System.Action<PickupSample> pickupCallback;

	// gizmo drawing vars
	Vector3 point1, point2;
	float radius;
	Dictionary<GameObject, Vector3> finalPositions = new Dictionary<GameObject, Vector3> ();

	void Awake ()
	{
		recordTime = 1f / recordRate;
		actualCamera.SetParent ( fpsPosition );
		rb.centerOfMass = new Vector3 ( 0, -1, 0 );
		for ( int i = 0; i < 4; i++ )
			localRotations [ i ] = wheelMeshes [ i ].localRotation;
		depthOfField = camera.GetComponent<DepthOfField> ();
		ResetZoom ();

//		armActuator.onArrive = OnPickup;
	}

	void Start ()
	{
		armActuator.Fold ( false );
	}

	void FixedUpdate ()
	{
		if ( !isSteeringInput )
		{
			lastAngle = 0;
			lastSteerInput = 0;
		}

		for (int i = 0; i < 4; i++)
		{
			Quaternion quat;
			Vector3 position;
			wheels [ i ].GetWorldPose ( out position, out quat );
			wheelMeshes [ i ].position = position;
			wheelMeshes [ i ].rotation = quat * localRotations [ i ];
			if ( isTurningInPlace || isFixedTurning )
			{
				float y = robotBody.eulerAngles.y * 2.5f;
				wheelMeshes [ i ].rotation *= Quaternion.Euler ( y, 0, 0 );
//				wheelMeshes [ i ].rotation *= Quaternion.Euler ( -wheelAngle, 0, 0 );
//				wheelAngle += hRotateSpeed * Time.deltaTime;
			}
		}

		if ( isMotorInput )
		{
			isTurningInPlace = false;
			wheels [ 0 ].motorTorque = wheels [ 1 ].motorTorque = wheels [ 2 ].motorTorque = wheels [ 3 ].motorTorque = lastMoveInput * motorTorque;
			wheels [ 0 ].brakeTorque = wheels [ 1 ].brakeTorque = wheels [ 2 ].brakeTorque = wheels [ 3 ].brakeTorque = lastBrakeInput * brakeTorque;
//			Debug.Log ( "lmi " + lastMoveInput + " lbi " + lastBrakeInput );
/*			if ( Vector3.Angle ( robotBody.forward * lastMoveInput, GroundVelocity ) < 90 )
			{
				wheels [ 0 ].motorTorque = wheels [ 1 ].motorTorque = wheels [ 2 ].motorTorque = wheels [ 3 ].motorTorque = lastMoveInput * motorTorque;
				wheels [ 0 ].brakeTorque = wheels [ 1 ].brakeTorque = wheels [ 2 ].brakeTorque = wheels [ 3 ].brakeTorque = lastBrakeInput * brakeTorque;
//				wheels [ 0 ].brakeTorque = wheels [ 1 ].brakeTorque = wheels [ 2 ].brakeTorque = wheels [ 3 ].brakeTorque = 0;

			} else
			{
				wheels [ 0 ].motorTorque = wheels [ 1 ].motorTorque = wheels [ 2 ].motorTorque = wheels [ 3 ].motorTorque = 0;
				wheels [ 0 ].brakeTorque = wheels [ 1 ].brakeTorque = wheels [ 2 ].brakeTorque = wheels [ 3 ].brakeTorque = Mathf.Abs ( lastMoveInput ) * brakeTorque;
			}*/
		} else
		{
			if ( isTurningInPlace )
			{
				// turning in place now just literally turns in place
//				wheels [ 0 ].motorTorque = wheels [ 1 ].motorTorque = Mathf.Abs ( lastSteerInput ) * motorTorque * 2;
//				wheels [ 2 ].motorTorque = wheels [ 3 ].motorTorque = Mathf.Abs ( lastSteerInput ) * motorTorque * 2;

//				wheels [ 0 ].motorTorque = wheels [ 1 ].motorTorque = -motorTorque * moveSpeed / 2;
//				wheels [ 2 ].motorTorque = wheels [ 3 ].motorTorque = -motorTorque * moveSpeed / 2;
//				wheels[0].motorTorque = wheels[1].motorTorque = wheels[2].motorTorque = wheels[3].motorTorque = -motorTorque * speed;
				wheels[0].brakeTorque = wheels[1].brakeTorque = wheels[2].brakeTorque = wheels[3].brakeTorque = 0;
				
			} else
			{
				wheels[0].brakeTorque = wheels[1].brakeTorque = wheels[2].brakeTorque = wheels[3].brakeTorque = 0;
				wheels[0].motorTorque = wheels[1].motorTorque = wheels[2].motorTorque = wheels[3].motorTorque = 0;
			}
		}

		float maxSpeed = lastMoveInput >= 0 ? moveSpeed : maxReverseSpeed;
		float speedPercent = GroundVelocity.magnitude / maxSpeed;
		rb.AddForce ( Vector3.down * downForce * speedPercent );
//		rb.AddForce ( -robotBody.up * downForce * speedPercent );
		if ( GroundVelocity.magnitude > maxSpeed )
		{
			Vector3 velo = GroundVelocity;
			velo = velo.normalized * maxSpeed;
			velo += VerticalVelocity;
			rb.velocity = velo;
		}

		if ( isSteeringInput && !isMotorInput )
		{
			if ( GroundVelocity.sqrMagnitude < 0.1f )
				isTurningInPlace = true;
		}

		if ( isFixedTurning )
		{
			Vector3 euler = robotBody.eulerAngles;
			euler.y = Mathf.MoveTowardsAngle ( euler.y, targetYaw, fixedTurnSpeed * Time.deltaTime );
			robotBody.eulerAngles = euler;
			wheels [ 0 ].steerAngle = wheels [ 3 ].steerAngle = 45;// * Mathf.Abs ( lastSteerInput );
			wheels [ 1 ].steerAngle = wheels [ 2 ].steerAngle = -45;// * Mathf.Abs ( lastSteerInput );
			for ( int i = 0; i < 4; i++ )
				wheelMeshes [ i ].Rotate ( Vector3.right, fixedTurnSpeed * Time.deltaTime );
//				wheelMeshes [ i ].Rotate ( Vector3.right * fixedTurnSpeed, Space.World );
			if ( euler.y == targetYaw )
			{
				isFixedTurning = false;
				rb.isKinematic = false;
				for ( int i = 0; i < 4; i++ )
					wheelMeshes [ i ].transform.localRotation = localRotations [ i ];
			}

		} else
		if ( isTurningInPlace )
		{
//			SteerAngle = lastSteerInput * hRotateSpeed;
			robotBody.Rotate ( Vector3.up * lastSteerInput * hRotateSpeed * Time.deltaTime );
			wheels [ 0 ].steerAngle = wheels [ 3 ].steerAngle = 45 * Mathf.Abs ( lastSteerInput );
			wheels [ 1 ].steerAngle = wheels [ 2 ].steerAngle = -45 * Mathf.Abs ( lastSteerInput );
			for ( int i = 0; i < 4; i++ )
				wheelMeshes [ i ].Rotate ( Vector3.right, hRotateSpeed * Time.deltaTime );
//			wheels [ 0 ].steerAngle = wheels [ 1 ].steerAngle = lastAngle * 2; //  45 * Mathf.Sign ( lastAngle );
//			wheels [ 2 ].steerAngle = wheels [ 3 ].steerAngle = -lastAngle * 2; // -45 * Mathf.Sign ( lastAngle );
		} else
		{
			speedPercent = GroundVelocity.magnitude / moveSpeed; // use the forward speed here so reverse can turn better
			float steeringLerp = Mathf.Lerp ( 5, maxSteering, 1 - speedPercent ) * lastSteerInput;
			// if we're in reverse, double the steer amount
			if ( maxSpeed == maxReverseSpeed )
				steeringLerp *= 2;
			wheels [ 0 ].steerAngle = wheels [ 1 ].steerAngle = steeringLerp; //lastAngle;
			wheels[2].steerAngle = wheels[3].steerAngle = 0;
		}
	}

	void Update ()
	{
		if ( IsRecording )// && !isPickingUp )
			GetSample ( isPickingUp );
		
		if ( !isPickingUp && !getSaveStatus () )
		{
			// check for objectives. only if not already picking one up
			// start by building a capsule of the player's size
			radius = 0.5f;
//			point1 = robotArmShoulder.position + Vector3.up * radius;
//			point2 = robotArmShoulder.position;
			point1 = robotArmShoulder.position + robotBody.forward * 0.5f + Vector3.up * radius;
			point2 = robotArmShoulder.position + robotBody.forward * 0.5f;
//			point1 = robotBody.position + robotBody.forward + Vector3.up * radius;
//			point2 = point1 + Vector3.up * ( 1 - radius * 2 );
			Debug.DrawLine ( point2, point2 + robotBody.forward * radius, Color.green );
			Debug.DrawLine ( point1, point1 + robotBody.forward * radius, Color.blue );
//			Debug.DrawLine ( point1, point1 + robotBody.right * radius, Color.red );
			// then check for a capsule ahead of the player.
			Collider[] objectives = Physics.OverlapCapsule ( point2, point1, radius, objectiveMask );
			if ( objectives != null && objectives.Length > 0 )
			{
//				Vector3 localShoulder = robotBody.InverseTransformPoint ( robotArmShoulder.position );
//				Vector3 localTarget = robotBody.InverseTransformPoint ( objectives[0].transform.position );
//				Debug.DrawLine ( robotArmShoulder.position, robotArmShoulder.position + localShoulder, Color.blue );
//				Debug.DrawLine ( robotArmShoulder.position, robotArmShoulder.position + localTarget, Color.green );
//				if ( localTarget.z > localShoulder.z )
//				if ( (objectives[0].transform.position - robotArmShoulder.position).sqrMagnitude > 0.1f )
//				{
					IsNearObjective = true;
				curObjective = objectives [ 0 ].transform.GetComponentInParent<PickupSample> ();
//				}
				if ( objectives.Length > 1 )
					Debug.Log ( "Near " + objectives.Length + " objectives." );
			} else
			{
				IsNearObjective = false;
				curObjective = null;
			}
//			Speed = GroundVelocity.magnitude;
			float spd = GroundVelocity.magnitude;
			// if moving forward
			if ( spd >= 0.01f && Vector3.Angle ( robotBody.forward, GroundVelocity ) > 90 )
				spd *= -1;
			else
			if ( spd < 0.01f )
				spd = 0;
			Speed = spd;
		} else
		{
			Speed = 0;
		}


		isSteeringInput = false;
		lastAngle = 0;
//		SteerAngle = 0;
//		ThrottleInput = 0;
		isMotorInput = false;
		lastMoveInput = 0;
		lastSteerInput = 0;
		IsTurningInPlace = isFixedTurning;
//		isTurningInPlace = false;
	}

	public override void Move (float throttle, float brake)
	{
		if ( isFixedTurning || isPickingUp )
			return;
		ThrottleInput = throttle;
		BrakeInput = brake;
		lastMoveInput = throttle;
		lastBrakeInput = brake;
//		Debug.Log ( "throt: " + lastMoveInput + " brake: " + lastBrakeInput );
		isMotorInput = throttle != 0 || brake != 0;
		if ( isMotorInput && !isPickingUp )
			armActuator.enabled = false;
	}

	public override void Move (float input)
	{
		if ( isFixedTurning || isPickingUp )
			return;
		ThrottleInput = input;
		lastMoveInput = input; // * acceleration;
		isMotorInput = input != 0;
	}

	public override void Move (Vector3 direction)
	{
//		direction.y = rb.velocity.y;
//		rb.velocity = direction;
	}

	// provide input in angle
	public override void RotateRaw (float angle)
	{
		if ( isFixedTurning || isPickingUp )
			return;

		angle = Mathf.Clamp ( angle, -maxSteering, maxSteering );
		SteerAngle = angle;
		if ( angle != 0 )
		{
			lastSteerInput = angle / maxSteering;
			lastSteerTime = Time.time;
			lastAngle = angle;
			lastAngle = Mathf.Clamp ( lastAngle, -maxSteering, maxSteering );
			isSteeringInput = true;
			if ( !isPickingUp )
				armActuator.enabled = false;
		} else
		{
			lastAngle = 0;
			SteerAngle = 0;
			isSteeringInput = false;
			isTurningInPlace = false;
		}
	}

	// provide input in percent
	public override void Rotate (float anglePercent)
	{
		if ( isFixedTurning || isPickingUp )
			return;
		anglePercent = Mathf.Clamp ( anglePercent, -1f, 1f );
		SteerAngle = anglePercent * maxSteering;
		if ( anglePercent != 0 )
		{
			lastSteerInput = anglePercent;
			lastSteerTime = Time.time;
			lastAngle = anglePercent * maxSteering;
			lastAngle = Mathf.Clamp ( lastAngle, -maxSteering, maxSteering );
			isSteeringInput = true;
			if ( !isPickingUp )
				armActuator.enabled = false;

		} else
		{
			lastAngle = 0;
			SteerAngle = 0;
			isSteeringInput = false;
			isTurningInPlace = false;
		}
	}

	public override void FixedTurn (float angle, float time = 0)
	{
		if ( Speed == 0 && !isMotorInput && !isSteeringInput && !isFixedTurning )
		{
			isFixedTurning = true;
			if ( time == 0 )
				time = angle / hRotateSpeed;
			targetYaw = robotBody.eulerAngles.y + angle;
			fixedTurnSpeed = angle / time;
			rb.isKinematic = true;
			wheelAngle = 0;
		}
	}

	public override void Stop ()
	{
		rb.velocity = Vector3.zero;
	}

	public override void RotateCamera (float horizontal, float vertical)
	{
		if ( curCamera == 0 )
		{
			cameraHAxis.Rotate ( Vector3.up * horizontal );
			Vector3 euler = cameraVAxis.localEulerAngles;
			euler.x -= vertical;
			if ( euler.x > 270 )
				euler.x -= 360;
			euler.x = Mathf.Clamp ( euler.x, cameraMinAngle, cameraMaxAngle );
			cameraVAxis.localEulerAngles = euler;
		} else
		{
			tpsPosition.RotateAround ( robotBody.position, Vector3.up, horizontal );
			tpsPosition.RotateAround ( robotBody.position, tpsPosition.right, -vertical );
			Vector3 euler = tpsPosition.localEulerAngles;
			euler.z = 0;
//			euler.y = euler.z = 0;
			tpsPosition.localEulerAngles = euler;
		}
	}

	public override void ZoomCamera (float amount)
	{
		camera.fieldOfView += amount * cameraZoomSpeed;
		camera.fieldOfView = Mathf.Clamp ( camera.fieldOfView, cameraMinFOV, cameraMaxFOV );
	}

	public override void ResetZoom ()
	{
		camera.fieldOfView = cameraDefaultFOV;
		// this has issues in standalone for some reason...
//		camera.ResetFieldOfView ();
	}

	public override void SwitchCamera ()
	{
		if ( curCamera == 0 )
		{
			curCamera = 1;
			actualCamera.SetParent ( tpsPosition, false );
			if ( depthOfField != null )
				depthOfField.focalTransform = robotBody;

		} else
		{
			curCamera = 0;
			actualCamera.SetParent ( fpsPosition, false );
			if ( depthOfField != null )
				depthOfField.focalTransform = null;
		}
	}

	public override Vector3 TransformDirection (Vector3 localDirection)
	{
		return robotBody.TransformDirection ( localDirection );
	}

	public override void PickupObjective (System.Action<PickupSample> onPickup)
	{
		// step 1: find pickup position
		// step 2: actuate arm toward pickup
		// 2a. rotate shoulder, arm, piston
		// 2b. open claw to open position
		// step 3: close claw on pickup
		rb.velocity = Vector3.zero;
		rb.isKinematic = true;
//		pickupCallback = onPickup;
		isPickingUp = true;
		IsPickingUpSample = isPickingUp;
		armActuator.enabled = true;
//		Time.timeScale = 0.2f;
//		PickupProgress = 0;
		if ( IsRecording && !getSaveStatus () )
		{
//			GetSample ( true );
			TriggerPickup ();
		}
		StartCoroutine ( DoPickup () );
	}

	IEnumerator DoPickup ()
	{
		float total = 1 + 1.5f + 1.5f + 1.5f + 1.5f + 3;
		float t = 0;
//		curObjective.Collider.enabled = false;
//		grabbingObject = false;
		// first find a new folded position facing the target
		Vector3 centerPos = armActuator.baseTransform.position;
		centerPos.y = armActuator.foldedPosition.position.y;
		float dist = ( armActuator.foldedPosition.position - centerPos ).magnitude;
		Vector3 toTarget = curObjective.GetCenterPosition () - centerPos;
		toTarget.y = 0;
		// assign that position to the folded position and the arm will turn to it
		armActuator.SetTarget ( 0, centerPos + toTarget.normalized * dist );
		t = 0;
		while ( t < 1 )
		{
//			if ( IsRecording )
//				GetSample ( true );
			yield return null;
//			PickupProgress += Time.deltaTime / total;
			t += Time.deltaTime;
		}
//		yield return new WaitForSeconds ( 2 );
		// now assign the extended position as something near the sample's position
		Ray ray = new Ray ( armActuator.foldedPosition.position, curObjective.GetCenterPosition () - armActuator.foldedPosition.position );
//		Ray ray = new Ray ( robotGrabPoint.position, curObjective.GetCenterPosition () - robotGrabPoint.position );
		RaycastHit hit;
		curObjective.Collider.Raycast ( ray, out hit, 1 );
		curObjective.Collider.enabled = false;
//		Physics.Raycast ( ray, out hit );
		armActuator.SetTarget ( 1, hit.point );
//		armActuator.SetTarget ( 1, curObjective.GetCenterPosition () );
		// and move towards that over 3 sec
		armActuator.MoveToTarget ( 1.5f );
		t = 0;
		while ( t < 1.5f )
		{
//			if ( IsRecording )
//				GetSample ( true );
			yield return null;
//			PickupProgress += Time.deltaTime / total;
			t += Time.deltaTime;
		}
//		yield return new WaitForSeconds ( 3 );
		// parent and position the sample to our vacuum attachment
		Vector3 positionDIff = curObjective.transform.position - hit.point;
		Debug.DrawLine ( hit.point, curObjective.transform.position, Color.red, 3 );
		curObjective.transform.position = robotGrabPoint.position + positionDIff;
//		curObjective.transform.position = robotGrabPoint.position;// - Vector3.up * curObjective.Collider.bounds.extents.y;
		curObjective.transform.parent = armActuator.wrist;
		// now move back to folded position over 3 sec
		armActuator.MoveToTarget ( 1.5f, true );
		t = 0;
		while ( t < 1.5f )
		{
//			if ( IsRecording )
//				GetSample ( true );
			yield return null;
//			PickupProgress += Time.deltaTime / total;
			t += Time.deltaTime;
		}
//		yield return new WaitForSeconds ( 3 );
		// find a place to put the sample
		Vector3 newPos = getSaveStatus () ? finalPositions[curObjective.gameObject] : GetPositionOnFlatbed ();
		if ( IsRecording )
			finalPositions.Add ( curObjective.gameObject, newPos );
		toTarget = newPos - centerPos;
		toTarget.y = 0;
		// move the folded position to turn the arm around
		armActuator.SetTarget ( 0, centerPos + toTarget.normalized * dist );
		t = 0;
		while ( t < 1.5f )
		{
//			if ( IsRecording )
//				GetSample ( true );
			yield return null;
//			PickupProgress += Time.deltaTime / total;
			t += Time.deltaTime;
		}
//		yield return new WaitForSeconds ( 3 );
		// extend the arm out to place the object
		positionDIff = curObjective.transform.position - armActuator.foldedPosition.position;
		armActuator.SetTarget ( 1, newPos - positionDIff );
//		armActuator.SetTarget ( 1, newPos );
		armActuator.MoveToTarget ( 1.5f );
		t = 0;
		while ( t < 1.5f )
		{
//			if ( IsRecording )
//				GetSample ( true );
			yield return null;
//			PickupProgress += Time.deltaTime / total;
			t += Time.deltaTime;
		}
//		yield return new WaitForSeconds ( 3 );
		// parent and position it to us (not to the flatbed because thatscrews up scales)
		curObjective.transform.position = newPos;
		curObjective.transform.parent = robotBody;
		// lastly reset the arm, and go back to drivable state
		armActuator.ResetPosition ();
		t = 0;
		while ( t < 3 )
		{
//			if ( IsRecording )
//				GetSample ( true );
			yield return null;
//			PickupProgress += Time.deltaTime / total;
			t += Time.deltaTime;
		}
//		yield return new WaitForSeconds ( 6 );
		isPickingUp = false;
		IsPickingUpSample = isPickingUp;
		rb.isKinematic = false;
//		PickupProgress = -1;
		if ( IsRecording )
		{
//			GetSample ( true );
			StopPickup ();
		}
		if ( !getSaveStatus () )
			ObjectiveSpawner.RemoveSample ( curObjective.gameObject );
		yield break;
	}

	Vector3 GetPositionOnFlatbed ()
	{
		if ( storedLocations.Count > 0 )
		{
			Vector3 pos = storedLocations [ 0 ].position;
			Destroy ( storedLocations [ 0 ].gameObject );
			storedLocations.RemoveAt ( 0 );
			return pos;
		}

		throw new System.NullReferenceException ( "No more positions left on the rover!" );
	}
}