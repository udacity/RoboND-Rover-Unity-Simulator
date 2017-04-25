using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideURIInput : MonoBehaviour
{
	public InputField uriInput;
	public Button initButton;

	void Awake ()
	{
		uriInput.gameObject.SetActive ( false );
		initButton.gameObject.SetActive ( false );
		if ( string.IsNullOrEmpty ( Environment.GetEnvironmentVariable ( "ROS_MASTER_URI", EnvironmentVariableTarget.User ) ) &&
			string.IsNullOrEmpty ( Environment.GetEnvironmentVariable ( "ROS_MASTER_URI", EnvironmentVariableTarget.Machine ) ) )
		{
			uriInput.gameObject.SetActive ( true );
			initButton.gameObject.SetActive ( true );
		}
	}

	public void DoInitROS ()
	{
		if ( !string.IsNullOrEmpty ( uriInput.text ) )
		{
			uriInput.gameObject.SetActive ( false );
			initButton.gameObject.SetActive ( false );

			Ros_CSharp.ROS.ROS_MASTER_URI = uriInput.text;
			ROSController.delayedStart = false;
			ROSController.StartROS ();
		}
	}
}