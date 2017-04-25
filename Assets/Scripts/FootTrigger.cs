using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootTrigger : MonoBehaviour
{
	WalkerController walker;
	public int footID;
	public Transform footprint;

	void Awake ()
	{
		walker = transform.root.GetComponent<WalkerController> ();
	}

	void OnTriggerEnter (Collider other)
	{
		if ( other.gameObject.layer == LayerMask.NameToLayer ( "Ground" ) )
			walker.OnFootDown ( footID, footprint );
	}
}