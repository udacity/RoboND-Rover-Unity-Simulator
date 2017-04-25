#pragma strict

var forearm : Transform;
var hand : Transform;
var target : Transform;

var transition : float = 1.0;
var elbowAngle : float;

private var armIK : Transform;
private var armRotation : Transform;

private var upperArmLength : float;
private var forearmLength : float;
private var armLength : float;

function Start () {
	var armIKGameObject = new GameObject("Arm IK");
	armIK = armIKGameObject.transform;
	armIK.parent = transform;
	var armRotationGameObject = new GameObject("Arm Rotation");
	armRotation = armRotationGameObject.transform;
	armRotation.parent = armIK;
	upperArmLength = Vector3.Distance(transform.position, forearm.position);
	forearmLength = Vector3.Distance(forearm.position, hand.position);
	armLength = upperArmLength + forearmLength;
}

function Update () {

}

function LateUpdate(){
	//Store rotation before IK.
	var storeUpperArmRotation : Quaternion = transform.rotation;
	var storeForearmRotation : Quaternion = forearm.rotation;
	
	//Upper Arm looks target.
	armIK.position = transform.position;
	armIK.LookAt(forearm);
	armRotation.position = transform.position;
	armRotation.rotation = transform.rotation;
	armIK.LookAt(target);
	transform.rotation = armRotation.rotation;

	//Upper Arm IK angle.
	var targetDistance : float = Vector3.Distance(transform.position, target.position);	
	targetDistance = Mathf.Min(targetDistance, armLength - 0.00001);		
	var adjacent : float = ((upperArmLength*upperArmLength) - (forearmLength*forearmLength) + (targetDistance*targetDistance))/(2*targetDistance);
	var angle : float = Mathf.Acos(adjacent/upperArmLength) * Mathf.Rad2Deg;
	transform.RotateAround(transform.position, transform.forward, -angle);
	
	//Forearm looks target.
	armIK.position = forearm.position;
	armIK.LookAt(hand);
	armRotation.position = forearm.position;
	armRotation.rotation = forearm.rotation;
	armIK.LookAt(target);
	forearm.rotation = armRotation.rotation;
	
	//Elbow angle.
	transform.RotateAround(transform.position, target.position - transform.position, elbowAngle);
	
	//Transition IK rotations with animation rotation.
	transition = Mathf.Clamp01(transition);
	transform.rotation = Quaternion.Slerp(storeUpperArmRotation, transform.rotation, transition);
	forearm.rotation = Quaternion.Slerp(storeForearmRotation, forearm.rotation, transition);
}