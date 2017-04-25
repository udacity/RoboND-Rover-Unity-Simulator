using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ros_CSharp;
using XmlRpc_Wrapper;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ROSController : MonoBehaviour
{
	public static ROSController instance;
	static Queue<Action> callbacks = new Queue<Action> ();
	static Queue<NodeHandle> nodes = new Queue<NodeHandle> ();
	public static bool delayedStart;

	bool starting;
	bool stopping;
	bool delete;

	void Awake ()
	{
		if ( instance != null && instance != this )
		{
			Debug.LogError ( "Too many ROSControllers! Only one must exist." );
			Destroy ( gameObject );
			return;
		}

//		Debug.Log ( "ros master is " + ROS.ROS_MASTER_URI );
		if ( string.IsNullOrEmpty ( Environment.GetEnvironmentVariable ( "ROS_MASTER_URI", EnvironmentVariableTarget.User ) ) &&
			string.IsNullOrEmpty ( Environment.GetEnvironmentVariable ( "ROS_MASTER_URI", EnvironmentVariableTarget.Machine ) ) )
			delayedStart = true;
		instance = this;
		StartROS ();
	}

	void OnDestroy ()
	{
		StopROS ();
	}

	void OnApplicationQuit ()
	{
		StopROS ();
	}

/*	void OnGUI ()
	{
		float width = 150;
		float y = 5;
		float height = 20;
		GUI.Box ( new Rect ( 5, y, width + 5, 100 ), "" );
		GUI.Label ( new Rect ( 10, y, width, height ), "ROS started: " + ROS.isStarted () );
		y += height;
		GUI.Label ( new Rect ( 10, y, width, height ), "ROS OK: " + ROS.ok );
		y += height;
		GUI.Label ( new Rect ( 10, y, width, height ), "ROS stopping: " + ROS.shutting_down );
		y += height * 2;

		if ( ROS.isStarted () )
		{
			if ( GUI.Button ( new Rect ( 5, y, width + 5, height ), "Stop ROS" ) )
			{
				ROSController.StopROS ();
			}
		} else
		{
			if ( GUI.Button ( new Rect ( 5, y, width + 5, height ), "Start ROS" ) )
			{
				ROSController.StartROS ();
			}
		}
	}*/

	public static void StartROS (Action callback = null)
	{
		#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
			return;
		#endif

		if ( instance == null )
		{
			if ( callback != null )
				callbacks.Enqueue ( callback );
			GameObject go = new GameObject ( "ROSController" );
			go.AddComponent<ROSController> ();
			return;
		}

		if ( ROS.isStarted () && ROS.ok )
		{
			if ( callback != null )
				callback ();
			return;
		}

		if ( callback != null )
			callbacks.Enqueue ( callback );
		
		if ( instance.starting )
			return;

		// this gets set when the environment variable ROS_MASTER_URI isn't set
		if ( delayedStart )
			return;

		string timeString = DateTime.UtcNow.ToString ( "MM_dd_yy_HH_MM_ss" );
//		Debug.Log ( timeString );
		instance.starting = true;
		instance.stopping = false;
		Debug.Log ( "ROS is starting" );
		ROS.Init ( new string[0], "ROS_Unity_" + timeString );
		instance.StartCoroutine ( instance.WaitForInit () );
		XmlRpcUtil.SetLogLevel(XmlRpcUtil.XMLRPC_LOG_LEVEL.ERROR);
	}

	public static void StopROS ()
	{
		if ( ROS.isStarted () && !ROS.shutting_down && !instance.stopping )
		{
			instance.starting = false;
			instance.stopping = true;
			while ( nodes.Count > 0 )
			{
				NodeHandle node = nodes.Dequeue ();
				node.shutdown ();
				node.Dispose ();
			}
			Debug.Log ( "stopping ROS" );
			ROS.shutdown ();
			ROS.waitForShutdown ();
		}
	}

	public static void AddNode (NodeHandle nh)
	{
		nodes.Enqueue ( nh );
	}

	IEnumerator WaitForInit ()
	{
		// apparently ROS.shutting_down never gets turned off...
//		if ( ROS.shutting_down )
//		{
//			Debug.LogError ( "ROS is already shutting down" );
//		}
		while ( !ROS.isStarted () && !ROS.ok && !stopping )
			yield return null;
		
		if ( ROS.ok && !stopping )
		{
			starting = false;
			Debug.Log ( "ROS Init successful" );
			while ( callbacks != null && callbacks.Count > 0 )
				callbacks.Dequeue () ();
		}
	}
}