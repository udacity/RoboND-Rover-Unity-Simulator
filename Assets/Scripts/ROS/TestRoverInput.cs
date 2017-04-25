using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Ros_CSharp;
using rbyte = Messages.std_msgs.Byte;

public class TestRoverInput : MonoBehaviour
{
	public RoverRemoteInput roverInput;

	NodeHandle nh;
	Thread pubthread;
	Publisher<rbyte> pub;

	byte dataToSend;

	void Awake ()
	{
		if ( !enabled )
			return;
		ROSController.StartROS ( OnRosInit );
	}

	void LateUpdate ()
	{
		int vert = (int) Input.GetAxisRaw ( "Vertical" );
		int horz = (int) Input.GetAxisRaw ( "Horizontal" );

		dataToSend = 0;
		if ( vert == 1 )
			dataToSend |= 1;
		else
		if ( vert == -1 )
			dataToSend |= 2;

		if ( horz == -1 )
			dataToSend |= 4;
		else
		if ( horz == 1 )
			dataToSend |= 8;
	}

	void OnDestroy ()
	{
		if ( enabled )
		{
			nh.Dispose ();
			ROSController.StopROS ();
		}
	}

	void OnRosInit ()
	{
		Debug.Log ( "Ros init (test). Input is: " + ( roverInput != null ) );
		if ( roverInput != null )
		{
			roverInput.nh = new NodeHandle ();
			ROSController.AddNode ( roverInput.nh );
			nh = roverInput.nh;

		} else
		{
			nh = new NodeHandle ();
			ROSController.AddNode ( nh );
		}
		pub = nh.advertise<rbyte> ( "/RoverInput", 0, false );
		pubthread = new Thread ( Publish );
		pubthread.Start ();
		Debug.Log ("Started publish thread");
	}

	void Publish ()
	{
		int sleep = 1000 / 60;
		Vector3 testPos = Vector3.zero;
		while ( ROS.ok && !ROS.shutting_down )
		{
			rbyte b = new rbyte ();
			b.data = dataToSend;
			pub.publish ( b );
			Thread.Sleep ( sleep );
//			Debug.Log ( "publishing " + dataToSend );
		}
	}
}