using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOutput : MonoBehaviour
{
	public IRobotController controller;
	public Text infoText;
	public GameObject progressParent;
	public Image progressBar;

	System.Text.StringBuilder sb = new System.Text.StringBuilder ();

	void Start ()
	{
		if ( infoText == null )
		{
			enabled = false;
			return;
		}
//		infoText.gameObject.SetActive ( false );
//		infoText.gameObject.SetActive ( true );
		infoText.text = "WaitingForInput";
//		infoText.transform.parent.gameObject.SetActive ( false );
//		infoText.transform.parent.gameObject.SetActive ( true );
//		infoText.enabled = false;
//		infoText.enabled = true;
	}

	void Update ()
	{
		if ( infoText == null )
			return;
		
		sb.Length = 0;
		sb.Capacity = 16;
		float speed = controller.Speed;
		float steer = controller.SteerAngle;
		float vAngle = controller.VerticalAngle;
		float throttle = controller.ThrottleInput;
		float brake = controller.BrakeInput;
		Vector2 position = new Vector2 ( controller.Position.x, controller.Position.z );
//		Vector3 position = controller.Position;
		float pitch = controller.Pitch;
		// new: set yaw angle counter-clockwise and relative to positive-x
		float yaw = IRobotController.ConvertAngleToCCWXBased ( controller.Yaw );
//		float yaw = controller.Yaw;
		float roll = controller.Roll;

		sb.Append ( "Throttle: " + throttle.ToString ( "F1" ) + "\n" );
		sb.Append ( "Brake: " + brake.ToString ( "F1" ) + "\n" );
		sb.Append ( "Steer angle: " + steer.ToString ( "F4" ) + "\n" );
		sb.Append ( "Ground speed: " + speed.ToString ( "F1" ) + "m/s\n" );
		sb.Append ( "Position: " + position.ToString () + "\n" );
		sb.Append ( "Pitch angle: " + pitch.ToString ( "F2" ) + "\n" );
		sb.Append ( "Yaw angle: " + yaw.ToString ( "F2" ) + "\n" );
		sb.Append ( "Roll angle: " + roll.ToString ( "F2" ) + "\n" );
//		sb.Append ( "Camera zoom: " + controller.Zoom.ToString ( "F1" ) + "x\n" );
		sb.Append ( "Is near objective: " + ( controller.IsNearObjective ? "Yes" : "No" ) + "\n" );
		sb.Append ( "Is picking up:" + ( controller.IsPickingUpSample ? "Yes" : "No" ) );
		infoText.text = sb.ToString ();

		if ( controller.PickupProgress != -1 )
		{
			if ( !progressParent.activeSelf )
				progressParent.SetActive ( true );
			progressBar.fillAmount = controller.PickupProgress;
		} else
		{
			if ( progressParent.activeSelf )
				progressParent.SetActive ( false );
		}
	}
}