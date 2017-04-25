using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ros_CSharp;
using rbyte = Messages.std_msgs.Byte;

//using XmlRpc_Wrapper;

/*******************************************************************************
* RoverRemoteInput:
* 
* Handles input received from a separate app, and passes it to the RoverRemoteControl.
* This class attempts to connect to a Rover control node (the other app) at launch.
* If the initial connection fails, the ConnectionListener will be enabled, and listen for an incoming control app.
*******************************************************************************/

public class RoverRemoteInput : MonoBehaviour
{
	public RobotRemoteControl rover;
	[System.NonSerialized]
	public NodeHandle nh;

	Subscriber<rbyte> inputSubscriber;
	bool init;
	[System.NonSerialized]
	public bool wasSubscribed;

	public bool IsSubscribed { get { return inputSubscriber != null && inputSubscriber.NumPublishers > 0; } }
	public bool IsSubscriberNull { get { return inputSubscriber == null; } }


	void Start ()
	{
		ROSController.StartROS ( OnROSInit );
	}

	void Update ()
	{
		if ( wasSubscribed && !IsSubscribed )
		{
			rover.ThrottleInput = rover.BrakeInput = rover.SteeringAngle = 0;
			rover.robot.Stop ();
		}
	}

	void LateUpdate ()
	{
		// moved to LateUpdate so listener can update status text
		wasSubscribed = IsSubscribed;
	}

	void OnDestroy ()
	{
		ROSController.StopROS ();
	}

	void OnROSInit ()
	{
		if ( init )
			return;

		init = true;
		Debug.Log ( "Ros init (input)" );
		if ( nh == null )
		{
			nh = new NodeHandle ();
			ROSController.AddNode ( nh );
		}
		Subscribe ();
//		inputSubscriber = nh.subscribe<rbyte> ( "/RoverInput", 10, OnReceivedInput );
	}

	void OnReceivedInput (rbyte input)
	{
		if ( input == null )
		{
			#if UNITY_EDITOR
			Debug.LogError ("Received null input?");
			#endif
			return;
		}

		Debug.Log ( "Received input: " + input.data );

		byte data = input.data;

		bool throttle = ( data & 1 ) != 0;
		bool brake = ( data & 2 ) != 0;
		bool steerLeft = ( data & 4 ) != 0;
		bool steerRight = ( data & 8 ) != 0;

		rover.ThrottleInput = throttle ? 1 : 0;
		rover.BrakeInput = ( brake && !throttle ) ? 1 : 0;
		rover.SteeringAngle = steerLeft ? -1 :
			steerRight ? 1 :
			0;
	}

	public void Subscribe ()
	{
		if ( inputSubscriber == null  || inputSubscriber.NumPublishers == 0 )
		{
			inputSubscriber = nh.subscribe<rbyte> ( "/RoverInput", 10, OnReceivedInput );
		}
	}
}