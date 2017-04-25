using UnityEngine;
using System.Collections;

public class AnotherIK : MonoBehaviour
{
	public Vector3 Target = new Vector3(-8, 0, 0);
	public Vector3 BendTarget = new Vector3(-4, 0, 3);
	public float Transition = 1f;

	public GameObject[] Members = new GameObject[3];

	public GameObject TargetGameObject, BendTargetGameObject;

	public bool TargetByGO = false;
	public bool Enabled = true;
	public bool isDebug = true;

	Vector3 BendPos;

	//Private
	Quaternion[] StartRotations = new Quaternion[3];
	Vector3 TargetStart = new Vector3();
	Vector3 BendTargetStart = new Vector3();

	void Start()
	{
		for(int i = 0; i < Members.Length; i++)StartRotations[i] = Members[i].transform.rotation;//Стартовые ротации сочленений
		TargetStart = Target - Members[0].transform.position;
		BendTargetStart = BendTarget - Members[0].transform.position;
	}

	void LateUpdate () 
	{
		if(TargetByGO)
		{
			Target = TargetGameObject.transform.position;
			BendTarget = BendTargetGameObject.transform.position;
		}

		if(Enabled)Calc();
	}

	void Calc()
	{
		float M0Length = Vector3.Distance(Members[0].transform.position, Members[1].transform.position);
		float M1Length = Vector3.Distance(Members[1].transform.position, Members[2].transform.position);
		float M2Length = M0Length + M1Length;

		float Hypotenuse = M0Length;
		float TargetDist = Vector3.Distance(Members[0].transform.position, Target);
		TargetDist = Mathf.Min(TargetDist, M2Length - 0.0001f);

		float Adjacent = (Mathf.Pow(Hypotenuse, 2) - Mathf.Pow(M1Length, 2) + Mathf.Pow(TargetDist, 2)) / (2 * TargetDist);
		float IKAngle = Mathf.Acos(Adjacent/Hypotenuse) * Mathf.Rad2Deg;

		//Fill IK Info

		Vector3 TargetPos = Target;
		Vector3 BendTargetPos = BendTarget;

		Transform M0Parent = Members[0].transform.parent;
		Transform M1Parent = Members[1].transform.parent;
		Transform M2Parent = Members[2].transform.parent;

		Vector3 M0Scale = Members[0].transform.localScale;
		Vector3 M1Scale = Members[1].transform.localScale;
		Vector3 M2Scale = Members[2].transform.localScale;

		Vector3 M0LocalPos = Members[0].transform.localPosition;
		Vector3 M1LocalPos = Members[1].transform.localPosition;
		Vector3 M2LocalPos = Members[2].transform.localPosition;

		Quaternion M0Rotation = Members[0].transform.rotation;
		Quaternion M1Rotation = Members[1].transform.rotation;
		Quaternion M2Rotation = Members[2].transform.rotation;

		//Reset objects
		Target = TargetStart + Members[0].transform.position;
		BendTarget = BendTargetStart + Members[0].transform.position;
		Members[0].transform.rotation = StartRotations[0];
		Members[1].transform.rotation = StartRotations[1];
		Members[2].transform.rotation = StartRotations[2];

		//Bla-bla-bla
		transform.position = Members[0].transform.position;
//		transform.LookAt(TargetPos, BendTargetPos - transform.position);

		GameObject M0AxisCorrection = new GameObject("M0AxisCorrection");
		GameObject M1AxisCorrection = new GameObject("M1AxisCorrection");
		GameObject M2AxisCorrection = new GameObject("M2AxisCorrection");

		M0AxisCorrection.transform.position = Members[0].transform.position;
		M0AxisCorrection.transform.LookAt(Members[1].transform.position, transform.root.up);
		M0AxisCorrection.transform.parent = transform;
		Members[0].transform.parent = M0AxisCorrection.transform;

		M1AxisCorrection.transform.position = Members[1].transform.position;
		M1AxisCorrection.transform.LookAt(Members[2].transform.position, transform.root.up);
		M1AxisCorrection.transform.parent = M0AxisCorrection.transform;
		Members[1].transform.parent = M1AxisCorrection.transform;

		M2AxisCorrection.transform.position = Members[2].transform.position;
		M2AxisCorrection.transform.parent = M1AxisCorrection.transform;
		Members[2].transform.parent = M2AxisCorrection.transform;

		//Reset targets
		Target = TargetPos;
		BendTarget = BendTargetPos;

		//Apply rotation of correctors
		M0AxisCorrection.transform.LookAt(Target, BendTarget - M0AxisCorrection.transform.position);
		//M0AxisCorrection.transform.localRotation.eulerAngles.x -= IKAngle;
		M0AxisCorrection.transform.localRotation = Quaternion.Euler(
			M0AxisCorrection.transform.localRotation.eulerAngles.x - IKAngle,
			M0AxisCorrection.transform.localRotation.eulerAngles.y,
			M0AxisCorrection.transform.localRotation.eulerAngles.z);
		M1AxisCorrection.transform.LookAt(Target, BendTarget - M0AxisCorrection.transform.position);
		M2AxisCorrection.transform.rotation = Quaternion.identity;

		//Restore
		Members[0].transform.parent = M0Parent;
		Members[1].transform.parent = M1Parent;
		Members[2].transform.parent = M2Parent;

		Members[0].transform.localScale = M0Scale;
		Members[1].transform.localScale = M1Scale;
		Members[2].transform.localScale = M2Scale;

		Members[0].transform.localPosition = M0LocalPos;
		Members[1].transform.localPosition = M1LocalPos;
		Members[2].transform.localPosition = M2LocalPos;

		//Clean up correctors
		Destroy(M0AxisCorrection);
		Destroy(M1AxisCorrection);
		Destroy(M2AxisCorrection);


		//Transition
		Transition = Mathf.Clamp01(Transition);
		Members[0].transform.rotation = Quaternion.Slerp(M0Rotation, Members[0].transform.rotation, Transition);
		Members[1].transform.rotation = Quaternion.Slerp(M1Rotation, Members[1].transform.rotation, Transition);
		Members[2].transform.rotation = Quaternion.Slerp(M2Rotation, Members[2].transform.rotation, Transition);


		//Debug
		if(isDebug)
		{
			Debug.DrawLine(transform.position, Members[0].transform.position, Color.yellow);
			Debug.DrawLine(transform.position, Target, Color.green);
			Debug.DrawLine(Members[1].transform.position, BendTarget, Color.blue);
		}
	}
}