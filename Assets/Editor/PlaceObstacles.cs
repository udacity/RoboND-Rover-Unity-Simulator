using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlaceObstacles : EditorWindow
{
	Object prefab;
	TerrainData td;

	[MenuItem ("Utility/Place obstacles", false, 1)]
	static void Init ()
	{
		PlaceObstacles obst = PlaceObstacles.GetWindow<PlaceObstacles> ( "Obstacle placer" );
		obst.Show ();
	}

	void OnGUI ()
	{
		td = (TerrainData) EditorGUILayout.ObjectField ( td, typeof (TerrainData), false );


		GUILayout.FlexibleSpace ();

		if ( td != null )
		if ( GUILayout.Button ( "Populate", EditorStyles.toolbarButton ) )
		{
			Populate ();
		}
	}

	void Populate ()
	{
		GameObject parent = new GameObject ( "Obstacles" );
		Bounds bounds = td.bounds;
		Vector3 size = bounds.size;

		TreePrototype[] trees = td.treePrototypes;
	}
}