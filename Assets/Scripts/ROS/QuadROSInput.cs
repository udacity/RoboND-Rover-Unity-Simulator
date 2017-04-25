using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Ros_CSharp;
using rquat = Messages.geometry_msgs.Quaternion;

// note: this publisher uses a quaternion as an easy way of sending 4 float (double) inputs, but they aren't related to one another and in no way represent an actual quaternion

public class QuadROSInput : MonoBehaviour
{
	NodeHandle nh;
	Publisher<rquat> pub;
	Thread pubthread;

	public Slider[] rotorThrusts;
	public Slider masterThrustSlider;
	public Slider pitchSlider;
	public Slider rollSlider;
	public bool live;

	float forwardPitchMultiplier = 1;
	float backwardPitchMultiplier = 1;
	float rightRollMultiplier = 1;
	float leftRollMultiplier = 1;

	float[] forces = new float[4];

	void Awake ()
	{
		if ( live )
			ROSController.StartROS ( OnROSInit );
		ResetSliders ();
	}

	void Update ()
	{
		if ( Input.GetKeyDown ( KeyCode.Escape ) )
		{
			ROSController.StopROS ();
			Application.Quit ();
		}
	}

	void OnDestroy ()
	{
		if ( live )
			ROSController.StopROS ();
	}

	void OnROSInit ()
	{
		nh = new NodeHandle ();
		pub = nh.advertise<rquat> ( "/quad", 10, false );
		pubthread = new Thread ( Publish );
		pubthread.Start ();
	}

	void Publish ()
	{
		int sleepTime = 1000 / 60;
		while ( ROS.ok && !ROS.shutting_down )
		{
			rquat q = new rquat ();
			q.x = forces [ 0 ];
			q.y = forces [ 1 ];
			q.z = forces [ 2 ];
			q.w = forces [ 3 ];
			pub.publish ( q );
			Thread.Sleep ( sleepTime );
		}
	}

	void UpdateMultipliers ()
	{
		float pitch = 2 * pitchSlider.value - 1;
		float roll = 2 * rollSlider.value - 1;
//		Debug.Log ( "pitch is: " + pitch + " roll is: " + roll );
		forwardPitchMultiplier = pitch > 0 ? Mathf.Lerp ( 1f, 0.8f, pitch ) : 1;
		backwardPitchMultiplier = pitch < 0 ? Mathf.Lerp ( 1f, 0.8f, -pitch ) : 1;
		rightRollMultiplier = roll > 0 ? Mathf.Lerp ( 1f, 0.8f, roll ) : 1;
		leftRollMultiplier = roll < 0 ? Mathf.Lerp ( 1f, 0.8f, -roll ) : 1;
	}

	void UpdateIndividualRotors ()
	{
		float master = masterThrustSlider.value;
		forces[0] = rotorThrusts [ 0 ].value = master * forwardPitchMultiplier * leftRollMultiplier;
		forces[1] = rotorThrusts [ 1 ].value = master * forwardPitchMultiplier * rightRollMultiplier;
		forces[2] = rotorThrusts [ 2 ].value = master * backwardPitchMultiplier * leftRollMultiplier;
		forces[3] = rotorThrusts [ 3 ].value = master * backwardPitchMultiplier * rightRollMultiplier;
	}

	public void ResetSliders ()
	{
		pitchSlider.value = 0.5f;
		rollSlider.value = 0.5f;
		masterThrustSlider.value = 0.5f;
//		for ( int i = 0; i < 4; i++ )
//			forces [ i ] = rotorThrusts [ i ].value = 0.5f;

		UpdateMultipliers ();
		UpdateIndividualRotors ();
	}

	public void OnMasterThrustChanged (float value)
	{
		if ( Mathf.Abs ( value - 0.5f ) < 0.02f )
			value = masterThrustSlider.value = 0.5f;

		UpdateMultipliers ();
		UpdateIndividualRotors ();
	}

	public void OnPitchChanged (float value)
	{
		if ( Mathf.Abs ( value - 0.5f ) < 0.02f )
			value = pitchSlider.value = 0.5f;

		UpdateMultipliers ();
		UpdateIndividualRotors ();
	}

	public void OnRollChanged (float value)
	{
		if ( Mathf.Abs ( value - 0.5f ) < 0.02f )
			value = rollSlider.value = 0.5f;

		UpdateMultipliers ();
		UpdateIndividualRotors ();
	}
}