using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddLODGroup : MonoBehaviour
{
	[MenuItem ("Utility/Add LODGroup", false, 20)]
	static void AddLG ()
	{
		GameObject[] gobs = Selection.gameObjects;
		foreach ( GameObject gob in gobs )
		{
			LODGroup g = gob.AddComponent<LODGroup> ();
			LOD[] lods = new LOD[1];
			lods [ 0 ].screenRelativeTransitionHeight = 0.08f;
			lods [ 0 ].renderers = new Renderer[1] { gob.GetComponent<Renderer> () };
			g.SetLODs ( lods );
			g.RecalculateBounds ();
		}
	}

	[MenuItem ("Utility/Add Multi-renderer LG", false, 21)]
	static void AddMRLG ()
	{
		GameObject[] gobs = Selection.gameObjects;
		foreach ( GameObject gob in gobs )
		{
			LODGroup g = gob.AddComponent<LODGroup> ();
			LOD[] lods = new LOD[1];
			lods [ 0 ].screenRelativeTransitionHeight = 0.11f;
			lods [ 0 ].renderers = gob.GetComponentsInChildren<Renderer> ();
			g.SetLODs ( lods );
			g.RecalculateBounds ();
		}
	}

	[MenuItem ("Utility/Add Bridge LG", false, 22)]
	static void AddBridgeLG ()
	{
		GameObject[] gobs = Selection.gameObjects;
		foreach ( GameObject gob in gobs )
		{
			Renderer r = gob.GetComponent<Renderer> ();
			LODGroup g = gob.AddComponent<LODGroup> ();
			List<Renderer> list = new List<Renderer> ();
			gob.GetComponentsInChildren<Renderer> ( true, list );
			LOD[] lods = new LOD[1];
			lods [ 0 ].screenRelativeTransitionHeight = 0.4f;
			list.Remove ( r );
			lods [ 0 ].renderers = list.ToArray ();
			g.SetLODs ( lods );
			g.RecalculateBounds ();
		}
	}
}