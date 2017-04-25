using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal class SampleSample
{
	public GameObject[] activeSamples;
	public Vector3[] positions;
	public Quaternion[] rotations;
}

public class ObjectiveSpawner : MonoBehaviour
{
	public static GameObject[] samples;
	static Queue<SampleSample> storedSamples;
	static Dictionary<GameObject, Vector3> initialPositions;

	public GameObject[] prefabs;
	GameObject[] objectives;
	public int spawnCount;
	public Color[] colors;

	string colorProp = "_CristalColor";

	void Start ()
	{
		int count = transform.childCount;
		if ( spawnCount == 0 )
			spawnCount = Random.Range ( 1, count );


		objectives = new GameObject [ count ];
		List<int> indices = new List<int> ();
		for ( int i = 0; i < count; i++ )
		{
			objectives [ i ] = transform.GetChild ( i ).gameObject;
			indices.Add ( i );
			GameObject go = Instantiate ( prefabs [ Random.Range ( 0, prefabs.Length ) ] );
			go.transform.position = objectives [ i ].transform.position; // - Vector3.up * go.GetComponent<Collider> ().bounds.extents.y;
			go.transform.rotation = Quaternion.Euler ( new Vector3 ( 0, Random.Range ( 0f, 360f ), 0 ) );
			go.transform.localScale = Vector3.one * Random.Range ( 0.36f, 0.5f );
			go.transform.SetParent ( transform );
			Destroy ( objectives [ i ] );
			objectives [ i ] = go;
			go.SetActive ( false );
//			objectives [ i ].SetActive ( false );
		}

		List<GameObject> activeSamples = new List<GameObject> ();
		for ( int i = 0; i < spawnCount; i++ )
		{
			int index = Random.Range ( 0, indices.Count );
			GameObject ob = objectives [ indices [ index ] ];
			ob.SetActive ( true );
			activeSamples.Add ( ob );
//			ob.GetComponent<Renderer> ().material.SetColor ( colorProp, colors [ Random.Range ( 0, colors.Length ) ] );
			indices.RemoveAt ( index );
		}
		samples = activeSamples.ToArray ();
		initialPositions = new Dictionary<GameObject, Vector3> ();
		for ( int i = 0; i < samples.Length; i++ )
			initialPositions.Add ( samples [ i ], samples [ i ].transform.position );
//		storedSamples = new Queue<SampleSample> ();
	}

	public static void RemoveSample (GameObject go)
	{
		List<GameObject> l = new List<GameObject> ( samples );
		l.Remove ( go );
		samples = l.ToArray ();
	}

	public static void SetInitialPositions ()
	{
		foreach ( KeyValuePair<GameObject, Vector3> pair in initialPositions )
		{
			pair.Key.transform.parent = null;
			pair.Key.transform.position = pair.Value;
		}
	}

/*	public static void Sample ()
	{
		SampleSample s = new SampleSample ();
		int count = samples.Length;
		s.activeSamples = new GameObject[count];
		s.positions = new Vector3[count];
		s.rotations = new Quaternion[count];
		for ( int i = 0; i < count; i++ )
		{
			s.activeSamples [ i ] = samples [ i ];
			s.positions [ i ] = samples [ i ].transform.position;
			s.rotations [ i ] = samples [ i ].transform.rotation;
		}
		storedSamples.Enqueue ( s );
	}

	public static void LoadSample ()
	{
		SampleSample s = storedSamples.Dequeue ();
		int count = s.activeSamples.Length;
		for (int i = 0; i < count; i++)
		{
			s.activeSamples [ i ].transform.position = s.positions [ i ];
			s.activeSamples [ i ].transform.rotation = s.rotations [ i ];
		}
	}*/
}