using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ArmRotationDemo : MonoBehaviour
{
	public Transform targetJoint;
	public Space rotationSpace;
	public float guiOffset;
	public string refFrame;

	Vector3 xPos;
	Vector3 yPos;
	Vector3 zPos;

	string info = "";
	float infoTime;
	float infoEndTime;

	void Awake ()
	{
//		Time.timeScale = 0.5f;
		StartCoroutine ( DoAnimation () );
	}

	void OnDisable ()
	{
		Time.timeScale = 1;
	}

	IEnumerator DoAnimation ()
	{
		float length = 3;
		info = "Version 1:";
		infoTime = Time.time;
		infoEndTime = Time.time + length;
		yield return new WaitForSeconds ( length );

		// capture initial rotation
		Quaternion startRotation = targetJoint.rotation;
		float start = infoTime = Time.time;
		infoEndTime = Time.time + length;
		info = "Rotate 30° on Z";
		float rotation = 30;
		// rotate 30 degrees about Y over 3 sec
		while ( Time.time <= start + length )
		{
			targetJoint.Rotate ( Vector3.up, rotation * Time.deltaTime, rotationSpace );
			yield return null;
		}

		yield return new WaitForSeconds ( 2 );

		start = infoTime = Time.time;
		infoEndTime = Time.time + length;
		info = "Now rotate 30° on Y";
		// rotate 30 degrees about Z over 3 sec
		while ( Time.time <= start + length )
		{
			targetJoint.Rotate ( Vector3.forward, rotation * Time.deltaTime, rotationSpace );
			yield return null;
		}

		yield return new WaitForSeconds ( length );

		// reset rotation
		targetJoint.rotation = startRotation;
		info = "Version 2:";
		infoTime = Time.time;
		infoEndTime = Time.time + length;
		yield return new WaitForSeconds ( length );

		start = infoTime = Time.time;
		infoEndTime = Time.time + length;
		info = "Rotate 30° on Y";
		// rotate 30 degrees about Z over 3 sec
		while ( Time.time <= start + length )
		{
			targetJoint.Rotate ( Vector3.forward, rotation * Time.deltaTime, rotationSpace );
			yield return null;
		}

		yield return new WaitForSeconds ( 2 );

		start = infoTime = Time.time;
		infoEndTime = Time.time + length;
		info = "Now rotate 30° on Z";
		// rotate 30 degrees about Y over 3 sec
		while ( Time.time <= start + length )
		{
			targetJoint.Rotate ( Vector3.up, rotation * Time.deltaTime, rotationSpace );
			yield return null;
		}
	}


	void OnDrawGizmos ()
	{
		Vector3 position = targetJoint.position + Vector3.up * 0.1f;
		Vector3 position2 = position;
		Vector3 up;
		Vector3 forward;
		Vector3 right;
		Vector3 up2;
		Vector3 center;
		Vector3 size;
		float length = 0.15f;
		if ( rotationSpace == Space.Self )
		{
			Gizmos.matrix = targetJoint.localToWorldMatrix;
			position2 = targetJoint.InverseTransformPoint ( position );
//			position = Vector3.up * 0.1f;
			up = targetJoint.up * 0.1f;
			forward = targetJoint.forward * length;
			right = targetJoint.right * length;
			up2 = targetJoint.up * length;

		} else
		{
//			position = targetJoint.position + Vector3.up * 0.1f;
			up = Vector3.up * 0.1f;
			forward = Vector3.forward * length;
			right = Vector3.right * length;
			up2 = Vector3.up * length;
		}

		Gizmos.color = Color.blue;
		size = Vector3.forward * length;
		center = position2 + size / 2;
		size.x = size.y = 0.005f;
		Gizmos.DrawCube ( center, size );
		yPos = position + forward;
//		yPos = targetJoint.position + up + forward;

		Gizmos.color = Color.red;
		size = Vector3.right * length;
		center = position2 + size / 2;
		size.z = size.y = 0.005f;
		Gizmos.DrawCube ( center, size );
		xPos = position + right;
//		xPos = targetJoint.position + up + right;

		Gizmos.color = Color.green;
		size = Vector3.up * length;
		center = position2 + size / 2;
		size.x = size.z = 0.005f;
		Gizmos.DrawCube ( center, size );
		zPos = position + up2;
//		zPos = targetJoint.position + up + up2;
	}

	void OnGUI ()
	{
		GUIStyle label = GUI.skin.label;
		int fontSize = label.fontSize;
		FontStyle fontStyle = label.fontStyle;

		label.fontSize = (int) ( 36f * Screen.height / 1080 );
		label.fontStyle = FontStyle.Bold;
		Vector2 screenPos = Camera.main.WorldToScreenPoint ( xPos );
		screenPos.y = Screen.height - screenPos.y;
		Vector2 size = new Vector2 ( 25, 25 );

		GUI.color = Color.red;
		GUI.Label ( new Rect ( screenPos - size, size * 2 ), "x" );

		GUI.color = Color.blue;
		screenPos = Camera.main.WorldToScreenPoint ( yPos );
		screenPos.y = Screen.height - screenPos.y;
		GUI.Label ( new Rect ( screenPos - size, size * 2 ), "y" );

		GUI.color = Color.green;
		screenPos = Camera.main.WorldToScreenPoint ( zPos );
		screenPos.y = Screen.height - screenPos.y;
		GUI.Label ( new Rect ( screenPos - size, size * 2 ), "z" );

		label.fontSize = (int) ( 36f * Screen.height / 1080 );
		label.fontStyle = fontStyle;
		TextAnchor alignment = label.alignment;
		label.alignment = TextAnchor.UpperCenter;
		GUI.color = Color.white;
		GUI.Label ( new Rect ( 25 + Screen.width * guiOffset, Screen.height - 50, Screen.width * 0.5f, Screen.height ), refFrame );
		label.alignment = alignment;

		Color c = Color.white;
		float delta = Time.time - infoTime;
		float diff = infoEndTime - infoTime;
		if ( delta < 0.4f )
			c.a = delta * 2.5f;
		else
		if ( delta > diff - 0.4f )
			c.a = ( diff - delta ) * 2.5f;
		else
			c.a = 1;
		GUI.color = c;
		Rect r = new Rect ( 25 + Screen.width * guiOffset, 25, Screen.width, Screen.height );

//		label.fontStyle = fontStyle;
		label.fontSize = (int) ( 36f * Screen.height / 1080 );
		GUI.Label ( r, info );
		label.fontSize = fontSize;
	}
}