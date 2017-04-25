using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ros_CSharp;
//using rfl = Messages.std_msgs.Float32;
using rquat = Messages.geometry_msgs.Quaternion;

// note: this listener uses a quaternion as an easy way of receiving 4 float (double) inputs, but they aren't related to one another and in no way represent an actual quaternion

public class QuadROSListener : MonoBehaviour
{
	public ComplexQuadController quad;
	public bool active;

	NodeHandle nh;
	Subscriber<rquat> subscriber;

	void Awake ()
	{
		if ( active )
			ROSController.StartROS ( OnROSInit );
	}

	void OnDestroy ()
	{
		if ( active )
			ROSController.StopROS ();
	}

	void OnROSInit ()
	{
		nh = new NodeHandle ();
		nh.subscribe<rquat> ( "/quad", 10, OnInputReceived );
	}

	void OnInputReceived (rquat input)
	{
		quad.Steer ( (float) input.x, (float) input.y, (float) input.z, (float) input.w );
	}
}