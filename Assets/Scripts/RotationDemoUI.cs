using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationDemoUI : MonoBehaviour
{
	public ArmRotationDemo arm;
	public Dropdown spaceDropdown;
	public UIRotationList rotations;

	void Awake ()
	{
		SetRotationSpace ( spaceDropdown.value );
	}

	public void PlaySequence ()
	{
		if ( rotations.rotations.Count == 0 )
			return;
		
		List<float> angles = new List<float> ();
		List<Vector3> axes = new List<Vector3> ();
		foreach ( UIRotationOption option in rotations.rotations )
		{
			angles.Add ( option.Angle );
			axes.Add ( option.Axis );
		}

		arm.PlayRotations ( angles.ToArray (), axes.ToArray () );
	}

	public void ResetArm ()
	{
		arm.ResetArm ();
	}

	public void SetRotationSpace (int space)
	{
		arm.SetRotationSpace ( space == 0 ? Space.World : Space.Self );
	}
}