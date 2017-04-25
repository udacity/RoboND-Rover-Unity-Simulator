using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmAnimator : MonoBehaviour
{
	public Transform target;
	Animator animator;

	float weight;

	void Awake ()
	{
		animator = GetComponent<Animator> ();
		animator.SetLayerWeight ( 0, 1 );
		animator.SetTarget ( AvatarTarget.LeftHand, 0.5f );
	}

	void Update ()
	{
		if ( Input.GetKeyDown ( KeyCode.P ) )
		{
			animator.SetTrigger ( "Pickup" );
		}

		float wheel = Input.GetAxis ( "Mouse ScrollWheel" );
		weight = Mathf.Clamp01 ( weight + wheel );
	}

	void OnGUI ()
	{
		GUI.Label ( new Rect ( 5, 5, 50, 20 ), weight.ToString ( "F2" ) );
	}

	void OnAnimatorIK (int layerIndex)
	{
		Debug.Log ( "OAK" );
		if ( layerIndex == 0 )
		{
			Debug.Log ( "0!" );
			animator.SetIKPosition ( AvatarIKGoal.RightHand, target.position );
			animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
//			animator.SetIKRotation ( AvatarIKGoal.RightHand, target.rotation );
//			animator.SetIKRotationWeight ( AvatarIKGoal.RightHand, 1 );
//			animator.SetIKHintPosition ( AvatarIKHint.RightElbow, target.position );
//			animator.SetIKHintPositionWeight ( AvatarIKHint.RightElbow, 1 );
		}

	}
}