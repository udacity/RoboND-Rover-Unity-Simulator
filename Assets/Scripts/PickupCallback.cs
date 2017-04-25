using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCallback : MonoBehaviour
{
	public IRobotController robot;

	public void OnPickup ()
	{
		robot.SendMessage ( "OnPickup" );
	}
}