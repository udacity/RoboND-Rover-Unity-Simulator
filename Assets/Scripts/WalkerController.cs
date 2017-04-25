using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerController : IRobotController
{
//	public override Vector3 GroundVelocity { get { return animator.velocity; } }
//	public override Vector3 GroundVelocity { get { return new Vector3 ( animator.velocity.x, 0, animator.velocity.z ); } }
//	public override Vector3 VerticalVelocity { get { return new Vector3 ( 0, animator.velocity.y, 0 ); } }
//	public override float SteerAngle { get { return lastSteerAngle; } }
	public override float Zoom { get { return cameraDefaultFOV / camera.fieldOfView; } }

//	public Transform robotBody;
//	public Transform cameraHAxis;
//	public Transform cameraVAxis;
//	public Transform fpsPosition;
//	public Transform tpsPosition;
//	public Transform actualCamera;
//	public Camera camera;
	public CharacterController controller;
	public Rigidbody rigidbody;
	public Animator animator;
	public Collider bodyCollider;
	public Transform head;

	public float cameraMinAngle;
	public float cameraMaxAngle;
	public float cameraMinFOV;
	public float cameraMaxFOV;
	public float cameraDefaultFOV = 60;

	public PhysicMaterial idleMaterial;
	public PhysicMaterial movingMaterial;
	public LayerMask collisionMask;
	public GameObject footprint;
	public Transform[] feet;
	public int maxFootprints = 50;

//	float lastSteerAngle;
	[SerializeField]
	float slopeAngle;
	float moveInput;
	int curCamera;
	[SerializeField]
	bool isFacingSlope;

	Vector3 point1, point2;
	float radius;
	bool isPickingUp;

	Queue<GameObject> footprints;
	Transform footprintParent;
	System.Action<PickupSample> pickupCallback;

	void Awake ()
	{
		actualCamera.SetParent ( fpsPosition );
		ResetZoom ();
		footprints = new Queue<GameObject> ();
		footprintParent = new GameObject ( "Footprints" ).transform;
	}

	void Update ()
	{
		// check for slope angle ahead
		Vector3 kneePoint = robotBody.position + Vector3.up * 0.4f;
		float rayDistance = 1;
//		Ray ray = new Ray ( kneePoint, Vector3.down );
		Ray ray = new Ray ( kneePoint, ( robotBody.forward * Mathf.Sign ( moveInput ) - Vector3.up ).normalized );
		RaycastHit hit;
		if ( Physics.Raycast ( ray, out hit, rayDistance, 1 << collisionMask.value ) )
		{
			slopeAngle = Vector3.Angle ( hit.normal, Vector3.up );
			if ( slopeAngle >= maxSlope )
				isFacingSlope = true;
			else
				isFacingSlope = false;

		} else
		{
			isFacingSlope = false;
			slopeAngle = 0;
		}
//		Debug.DrawLine ( ray.origin, ray.origin + ray.direction * rayDistance, Color.red );

		if ( isFacingSlope )
			animator.applyRootMotion = false;
		else
			animator.applyRootMotion = true;
		animator.SetFloat ( "Vertical", moveInput );
		bodyCollider.material = moveInput == 0 ? idleMaterial : movingMaterial;

		if ( !isPickingUp )
		{
			// check for objectives. only if not already picking one up
			// start by building a capsule of the player's size
			CapsuleCollider c = (CapsuleCollider) bodyCollider;
			radius = 0.5f;
			point1 = robotBody.position + robotBody.forward + Vector3.up * radius;
			point2 = point1 + Vector3.up * ( c.height - radius * 2 );
			// then check for a capsule ahead of the player.
			Collider[] objectives = Physics.OverlapCapsule ( point1, point2, radius, objectiveMask );
			if ( objectives != null && objectives.Length > 0 )
			{
				IsNearObjective = true;
				curObjective = objectives [ 0 ].transform.root.GetComponent<PickupSample> ();
				if ( objectives.Length > 1 )
					Debug.Log ( "Near " + objectives.Length + " objectives." );
			} else
			{
				IsNearObjective = false;
				curObjective = null;
			}
			Speed = new Vector3 ( animator.velocity.x, 0, animator.velocity.z ).magnitude;
		} else
		{
			Speed = 0;
		}
		ThrottleInput = 0;
		moveInput = 0;
	}

	public override void Move (float input)
	{
		ThrottleInput = input;
//		controller.SimpleMove ( robotBody.forward * input * moveSpeed );
		moveInput = input;
	}

	public override void Move (Vector3 direction)
	{
		direction *= moveSpeed;
		direction.y = Physics.gravity.y * Time.deltaTime;
		controller.Move ( direction );
	}

	public override void Rotate (float angle)
	{
		SteerAngle = angle * hRotateSpeed;
		robotBody.Rotate ( Vector3.up * SteerAngle * Time.deltaTime );
	}

	public override void RotateCamera (float horizontal, float vertical)
	{
//		cameraHAxis.Rotate ( Vector3.up * horizontal, Space.World );
//		cameraVAxis.Rotate ( Vector3.right * -vertical, Space.Self );
		VerticalAngle = vertical;
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
			if ( isPickingUp )
				actualCamera.SetParent ( head, false );
			else
				actualCamera.SetParent ( fpsPosition, false );
		}
	}

	public override Vector3 TransformDirection (Vector3 localDirection)
	{
		return robotBody.TransformDirection ( localDirection );
	}

	public override void PickupObjective (System.Action<PickupSample> onPickup)
	{
		if ( !IsNearObjective || curObjective == null )
			return;

		pickupCallback = onPickup;
		isPickingUp = true;
		animator.SetTrigger ( "Pickup" );
		animator.SetIKPosition ( AvatarIKGoal.LeftHand, curObjective.transform.position );
		animator.SetIKPositionWeight ( AvatarIKGoal.LeftHand, 1 );
		if ( curCamera == 0 )
		{
			actualCamera.SetParent ( head, false );
		}
	}

	public void OnPickup ()
	{
		if ( pickupCallback != null )
			pickupCallback ( curObjective );
		else
			Destroy ( curObjective );
		curObjective = null;
		IsNearObjective = false;
		animator.SetIKPositionWeight ( AvatarIKGoal.LeftHand, 0 );
		isPickingUp = false;
		if ( curCamera == 0 )
			actualCamera.SetParent ( fpsPosition, false );
	}

	public void OnFootDown (int footID)
	{
		OnFootDown ( footID, feet [ footID ] );
	}

	public void OnFootDown (int footID, Transform foot)
	{
		GameObject footprintInstance = Instantiate ( footprint, foot.position, Quaternion.identity );
		if ( footID == 1 )
		{
			Vector3 scale = footprintInstance.transform.localScale;
			scale.x *= -1;
			footprintInstance.transform.localScale = scale;
		}
		Vector3 forward = Vector3.ProjectOnPlane ( foot.forward, Vector3.up ).normalized;
		footprintInstance.transform.rotation = Quaternion.LookRotation ( forward, Vector3.up );
		Vector3 position = footprintInstance.transform.position;
		position.y += 0.01f;
		footprintInstance.transform.position = position;
		footprintInstance.transform.parent = footprintParent;

//		RaycastHit hit;
//		Physics.Raycast ( footprintInstance.transform.position, Vector3.down, out hit );
//		Vector3 position = hit.point;
//		position.y += 0.01f;
//		footprintInstance.transform.position = position;
//		footprintInstance.transform.rotation = Quaternion.LookRotation ( footprintInstance.transform.forward, hit.normal );
		footprints.Enqueue ( footprintInstance );
		if ( footprints.Count > maxFootprints )
		{
			GameObject oldestFootprint = footprints.Dequeue ();
			Destroy ( oldestFootprint );
		}
	}

	void OnAnimatorIK (int layerIndex)
	{
		Debug.Log ( "OAK called " + layerIndex );
	}

	#if UNITY_EDITOR
	void OnDrawGizmosSelected ()
	{
		if ( UnityEditor.SceneView.currentDrawingSceneView == null || UnityEditor.SceneView.currentDrawingSceneView.camera == null )
			return;
		
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere ( point1, radius );
		Gizmos.DrawWireSphere ( point2, radius );
		Vector3 camRight = UnityEditor.SceneView.currentDrawingSceneView.camera.transform.right;
		Gizmos.DrawLine ( point1 + camRight * radius, point2 + camRight * radius );
		Gizmos.DrawLine ( point1 - camRight * radius, point2 - camRight * radius );
	}
	#endif
}