using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssignHiddenMaterials : MonoBehaviour
{
	[MenuItem ("Hidden/Assign Optimized Leaves")]
	static void AssignOptimizedLeaves ()
	{
		Object o = Selection.activeObject;
		if ( o == null )
			return;

		Material m = (Material) o;
		if ( m == null )
			return;

		m.shader = Shader.Find ( "Hidden/Nature/Tree Creator Leaves Optimized" );

		EditorUtility.SetDirty ( m );
		AssetDatabase.SaveAssets ();
	}
}