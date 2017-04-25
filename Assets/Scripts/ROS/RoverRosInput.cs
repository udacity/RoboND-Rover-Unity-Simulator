using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Ros_CSharp;
using rbyte = Messages.std_msgs.Byte;
using empty = Messages.std_msgs.Empty;

public class RoverRosInput : MonoBehaviour
{
	public Toggle right;
	public Toggle down;
	public Toggle left;
	public Toggle up;

	NodeHandle nh;
	Thread pubthread;
	Publisher<rbyte> pub;
	ServiceClient<empty, empty> srvClient;

	byte dataToSend;

	void Awake ()
	{
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

		up.isOn = vert == 1;
		down.isOn = vert == -1;
		right.isOn = horz == 1;
		left.isOn = horz == -1;

		if ( Input.GetKeyDown (KeyCode.Escape) )
		{
			ROSController.StopROS ();
			Application.Quit ();
		}
	}

	void OnDestroy ()
	{
		if ( !ROS.shutting_down )
		{
			ROSController.StopROS ();
		}
	}

	void OnRosInit ()
	{
		nh = new NodeHandle ();
		ROSController.AddNode ( nh );
		pub = nh.advertise<rbyte> ( "/RoverInput", 0, false );
		pubthread = new Thread ( Publish );
		pubthread.Start ();
		Debug.Log ("Started publish thread");
//		srvClient = nh.serviceClient<empty, empty> ( "/RoverInputListener" );

//		if ( srvClient.IsValid )
//		{
//			empty dummy = new empty ();
//			srvClient.call (null, ref dummy);
//		}
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