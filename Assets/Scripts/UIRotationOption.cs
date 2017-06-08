using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRotationOption : MonoBehaviour
{
	public float Angle
	{
		get
		{
			float angle = 0;
			float.TryParse ( angleInput.text, out angle );
			return angle;
		}
	}
	public Vector3 Axis
	{
		get
		{
			switch ( axisDropdown.value )
			{
			case 0:
				return Vector3.right;
				break;
			case 1:
				return Vector3.forward;
				break;
			case 2:
				return Vector3.up;
				break;
			}
			return Vector3.up;
		}
	}

	public InputField angleInput;
	public Dropdown axisDropdown;
}