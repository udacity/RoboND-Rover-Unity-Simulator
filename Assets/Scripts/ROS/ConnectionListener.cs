using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ros_CSharp;
using Messages;
using empty = Messages.std_msgs.Empty;

/*******************************************************************************
* ConnectionListener:
* 
* Advertises the input node so a connection can be made even after launch
*******************************************************************************/

/*public class RoverInputRequest : IRosMessage
{
	
}

public class RoverInputResponse : IRosMessage
{
	
}*/

public enum RoverStatus
{
	Disconnected,
	ListeningForInput,
	Connected
}

public class ConnectionListener : MonoBehaviour
{
	public RoverRemoteInput roverInput;
	public TrainingUI uiScript;

	ServiceServer srvListen;

	bool isServiceAdvertised;

	void Start ()
	{
		uiScript.roverStatus = RoverStatus.Disconnected;
		uiScript.roverStatusText.enabled = true;
		uiScript.roverStatusText.text = "Rover status: " + uiScript.roverStatus.ToString ();
	}

	void Update ()
	{
/*		if ( ROS.ok && !ROS.shutting_down )
		{
			if ( !isServiceAdvertised && roverInput.IsSubscribed )
//			if ( !isServiceAdvertised && !roverInput.IsSubscriberNull && !roverInput.IsSubscribed )
			{
				StartService ();
			}
			if ( isServiceAdvertised && roverInput.IsSubscribed )
			{
				StopService ();
			}
		}*/

		if ( roverInput.IsSubscribed && roverInput.wasSubscribed )
		{
			if ( isServiceAdvertised )
				uiScript.roverStatus = RoverStatus.ListeningForInput;
			else
				uiScript.roverStatus = RoverStatus.Connected;
		} else
		if ( roverInput.wasSubscribed && !roverInput.IsSubscribed )
			uiScript.roverStatus = RoverStatus.Disconnected;
		
		if ( uiScript.lastStatus != uiScript.roverStatus )
		{
			uiScript.lastStatus = uiScript.roverStatus;
			uiScript.roverStatusText.text = "Rover status: " + uiScript.roverStatus.ToString ();
		}
	}

/*	void StartService ()
	{
		Debug.Log ( "starting listening service" );
		srvListen = roverInput.nh.advertiseService<empty, empty> ( "/RoverInputListener", callback );
		isServiceAdvertised = true;
		uiScript.roverStatus = RoverStatus.ListeningForInput;
//		uiScript.roverStatusText.text = "Rover status: " + uiScript.roverStatus.ToString ();
	}

	void StopService ()
	{
		Debug.Log ( "stopping listening service" );
		srvListen.shutdown ();
		isServiceAdvertised = false;
		if ( ROS.ok && roverInput.IsSubscribed )
			uiScript.roverStatus = RoverStatus.Connected;
		else
			uiScript.roverStatus = RoverStatus.Disconnected;
//		uiScript.roverStatusText.text = "Rover status: " + uiScript.roverStatus.ToString ();
	}

	bool callback (empty request, ref empty response)
	{
		roverInput.Subscribe ();
		return true;
	}*/
}