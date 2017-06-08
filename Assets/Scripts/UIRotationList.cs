using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRotationList : MonoBehaviour
{
	public UIRotationOption prefab;
	public List<UIRotationOption> rotations = new List<UIRotationOption> ();

	public void AddRotation (Transform t)
	{
		UIRotationOption inst = Instantiate ( prefab, transform );
		inst.transform.SetSiblingIndex ( t.GetSiblingIndex () + 1 );
		rotations.Add ( inst );
		inst.gameObject.SetActive ( true );
	}

	public void AddRotationBefore (Transform t)
	{
		UIRotationOption inst = Instantiate ( prefab, transform );
		inst.transform.SetSiblingIndex ( t.GetSiblingIndex () - 1 );
		rotations.Add ( inst );
		inst.gameObject.SetActive ( true );
	}

	public void RemoveRotation (UIRotationOption option)
	{
		rotations.Remove ( option );
		Destroy ( option.gameObject );
	}
}