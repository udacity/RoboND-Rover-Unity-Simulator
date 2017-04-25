using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Ros_CSharp;
using tf.net;
using tfmsg = Messages.tf2_msgs.TFMessage;

public class QuadTransformBroadcast : MonoBehaviour
{
	NodeHandle nh;
	Publisher<tfmsg> pub;
	Thread pubthread;

	void Awake ()
	{
//		emTransform emt = new emTransform ( transform );
//		emt.origin = new emVector3 ( Vector3.zero );
//		emt.UnityRotation = Quaternion.identity;
//		Messages.std_msgs.Time t = ROS.GetTime (System.DateTime.Now);
//		Messages.tf2_msgs.TFMessage tfm;
//		tfm.transforms[0].transform.rotation

		ROSController.StartROS ( OnROSInit );
	}

	void OnDestroy ()
	{
		ROSController.StopROS ();
	}

	void OnROSInit ()
	{
		nh = new NodeHandle ();
		pub = nh.advertise<tfmsg> ( "/quad", 10, false );
		pubthread = new Thread ( Publish );
		pubthread.Start ();
	}

	void Publish ()
	{
		int sleepTime = 1000 / 60;
		while ( ROS.ok && !ROS.shutting_down )
		{
			
			Thread.Sleep ( sleepTime );
		}
	}
}