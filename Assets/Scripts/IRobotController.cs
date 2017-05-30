using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RobotSample
{
	public Vector2 Position2 { get { return new Vector2 ( position.x, position.z ); } }
	public Quaternion rotation;
	public Vector3 position;
	public float throttle;
	public float brake;
	public float speed;
	public float pitch;
	public float yaw;
	public float roll;
	public float steerAngle;
	public float verticalAngle;
	public Vector3 curSamplePosition;
	public Quaternion curSampleRotation;
	public GameObject curSample;
	public bool triggerPickup;
	public bool stopPickup;
	public string timestamp;
}

public abstract class IRobotController : MonoBehaviour
{
	public abstract float Zoom { get; }
	public abstract void Move (float input);
	public virtual void Move (float throttle, float brake) {}
	public abstract void Move (Vector3 direction);
	public abstract void Rotate (float anglePercent);
	public virtual void RotateRaw (float angle) {}
	public abstract void RotateCamera (float horizontal, float vertical);
	public abstract void ZoomCamera (float amount);
	public abstract void ResetZoom ();
	public abstract void SwitchCamera ();
	public abstract Vector3 TransformDirection (Vector3 localDirection);
	public virtual void PickupObjective (System.Action<PickupSample> onPickup) {}
	public virtual void CarryObjective (GameObject objective) { Destroy ( objective ); }
	public virtual void FixedTurn (float angle, float time) {}
	public virtual void Stop () {}

	public string csvSeparatorChar = ",";
	public Transform robotBody;
	public Transform cameraHAxis;
	public Transform cameraVAxis;
	public Transform fpsPosition;
	public Transform tpsPosition;
	public Transform tpsParent;
	public Transform actualCamera;
	public Camera camera;
	public Camera recordingCam;

	public float ThrottleInput { get; protected set; }
	public float BrakeInput { get; protected set; }
	public float Speed { get; protected set; }
	public float SteerAngle { get; protected set; }
	public float VerticalAngle { get; protected set; }
	public bool IsNearObjective { get; protected set; }
	public Vector3 Position { get { return transform.position; } }
	public float Yaw { get { return transform.localEulerAngles.y; } }
	public float Pitch { get { return transform.localEulerAngles.x; } }
	public float Roll { get { return transform.localEulerAngles.z; } }
	public bool IsTurningInPlace { get; protected set; }
	public float PickupProgress { get; set; }
	public bool IsPickingUpSample { get; protected set; }
	public bool allowStrafe;
	public bool allowSprint;
	public bool allowJump;
	public float hRotateSpeed = 90;
	public float vRotateSpeed = 90;
	public float moveSpeed = 5;
	public float sprintMultiplier = 2;
	public float cameraZoomSpeed = 20;
	public float maxSlope = 50;
	public LayerMask objectiveMask;

	protected PickupSample curObjective;
//	protected GameObject curObjective;

	public const string CSVFileName = "robot_log.csv";
	public const string DirFrames = "IMG";
	private string m_saveLocation = "";
	private Queue<RobotSample> samples;
	private int TotalSamples;
	private bool isSaving;
	public bool IsSaving { get { return isSaving; } }
	private Vector3 saved_position;
	private Quaternion saved_rotation;
	float saved_vAngle;
	System.Action beginRecordCallback;
	System.Action cancelRecordCallback;
	public float recordRate = 25;
	protected float recordTime;
	float nextRecordTime;
	private bool m_isRecording = false;
	public bool IsRecording {
		get
		{
			return m_isRecording;
		}

		set
		{
			m_isRecording = value;
			if(value == true)
			{ 
				Debug.Log("Starting to record");
				samples = new Queue<RobotSample>();
//				StartCoroutine(Sample());             
			} 
			else
			{
				Debug.Log("Stopping record");
//				StopCoroutine(Sample());
				Debug.Log("Writing to disk");
				//save the cars coordinate parameters so we can reset it to this properly after capturing data
				saved_position = transform.position;
				saved_rotation = transform.rotation;
				saved_vAngle = cameraVAxis.localEulerAngles.x;
				//see how many samples we captured use this to show save percentage in UISystem script
				TotalSamples = samples.Count;
				isSaving = true;
				TireTracks.ClearTracks ();
				StartCoroutine(WriteSamplesToDisk());

			};
		}
	}

	public bool CheckSaveLocation(System.Action saveCallback, SimpleFileBrowser.OnCancel cancelCallback)
	{
		if (m_saveLocation != "") 
		{
			return true;
		}
		else
		{
			cancelRecordCallback = new System.Action ( cancelCallback );
			beginRecordCallback = saveCallback;
			SimpleFileBrowser.ShowSaveDialog (OpenFolder, CancelRecord, true, null, "Select Output Folder", "Select");
		}
		return false;
	}

	//Changed the WriteSamplesToDisk to a IEnumerator method that plays back recording along with percent status from UISystem script 
	//instead of showing frozen screen until all data is recorded
	public IEnumerator WriteSamplesToDisk()
	{
		string newLine = "\n";
		if ( Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer )
			newLine = "\r\n";

		if ( string.IsNullOrEmpty ( csvSeparatorChar ) )
		{
			Debug.LogWarning ( "Warning: empty/null separator. using `,`" );
			csvSeparatorChar = ",";
		}
		yield return null;
		if ( samples.Count > 0 )
		{
			string row = "Path" + csvSeparatorChar + "SteerAngle" + csvSeparatorChar + "Throttle" + csvSeparatorChar + "Brake" + csvSeparatorChar + "Speed" + csvSeparatorChar + "X_Position" +
				csvSeparatorChar + "Y_Position" + csvSeparatorChar + "Pitch" + csvSeparatorChar + "Yaw" + csvSeparatorChar + "Roll" + newLine;
			File.AppendAllText (Path.Combine (m_saveLocation, CSVFileName), row);

			ObjectiveSpawner.SetInitialPositions ();
			int count = samples.Count;
			RobotSample sample = null;
			int badPickupCount = 0;
			bool isPickingUp = false;
			for ( int i = 0; i < count; i++ )
			{
				// hack: for some reason there are lots of extra samples during the pickup, so artificially advance and drop them
				if ( !IsPickingUpSample && isPickingUp )
				{
					do
					{
						sample = samples.Dequeue ();
						i++;
						badPickupCount++;
					} while ( !sample.stopPickup && samples.Count > 0 && i < count );
					isPickingUp = false;
					IsPickingUpSample = false;
				}
				sample = samples.Dequeue ();
				transform.position = sample.position;
				transform.rotation = sample.rotation;
				if ( sample.triggerPickup )
				{
					isPickingUp = true;
//					Debug.Log ( "recorded pickup" );
					curObjective = sample.curSample.GetComponent<PickupSample> ();
					curObjective.transform.parent = null;
					curObjective.Collider.enabled = true;
					PickupObjective ( null );
				}
				if ( sample.stopPickup )
				{
					Debug.Log ( "STOP!!" );
					isPickingUp = false;
					IsPickingUpSample = false;
				}
				string camPath = WriteImage ( recordingCam, "robocam", sample.timestamp );

				row = camPath + csvSeparatorChar + sample.steerAngle + csvSeparatorChar + sample.throttle + csvSeparatorChar + sample.brake + csvSeparatorChar + sample.speed + csvSeparatorChar +
					sample.position.x + csvSeparatorChar + sample.position.z + csvSeparatorChar + sample.pitch + csvSeparatorChar + sample.yaw + csvSeparatorChar + sample.roll + newLine;
				File.AppendAllText (Path.Combine (m_saveLocation, CSVFileName), row);

				yield return null;
			}
			Debug.Log ( "bad pickup count: " + badPickupCount );
			isSaving = false;
			//need to reset the car back to its position before ending recording, otherwise sometimes the car ended up in strange areas
			transform.position = saved_position;
			transform.rotation = saved_rotation;
			Vector3 euler = cameraVAxis.localEulerAngles;
			euler.x = saved_vAngle;
			cameraVAxis.localEulerAngles = euler;
			Move ( 0 );
			Rotate ( 0 );
			RotateCamera ( 0, 0 );
		}
	}

	public float getSavePercent()
	{
		return (float)(TotalSamples-samples.Count)/TotalSamples;
	}

	public bool getSaveStatus()
	{
		return isSaving;
	}

	public RobotSample GetSample (bool force = false)
	{
		RobotSample sample = null;
		if ( Time.time > nextRecordTime || force )
		{
			if (m_saveLocation != "")
			{
//				Debug.Log ( "Recording!" );
				sample = new RobotSample();
				
				sample.timestamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
				sample.steerAngle = SteerAngle;
//				sample.steerAngle = m_SteerAngle / m_MaximumSteerAngle;
				sample.verticalAngle = VerticalAngle;
				sample.throttle = ThrottleInput;
				sample.brake = BrakeInput;
				sample.speed = Speed;
				sample.pitch = Pitch;
				// new: convert yaw angle to CCW, x-based
				sample.yaw = ConvertAngleToCCWXBased ( Yaw );
				sample.roll = Roll;
				sample.position = transform.position;
				sample.rotation = transform.rotation;
//				Debug.Log ( "Throt " + sample.throttle + " brake " + sample.brake + " pos " + sample.position + " yaw " + sample.yaw );
				
				samples.Enqueue(sample);
//				ObjectiveSpawner.Sample ();
			}
			nextRecordTime = Time.time + recordTime;
		}
		return sample;
	}

	public void TriggerPickup ()
	{
		RobotSample s = GetSample ( true );
		s.triggerPickup = true;
		s.curSample = curObjective.gameObject;
//		var iter = samples.GetEnumerator ();
//		RobotSample s = null;
//		while ( iter.MoveNext () )
//		{
//			s = iter.Current;
//		}
//		s.triggerPickup = true;
//		s.curSample = curObjective.gameObject;
	}

	public void StopPickup ()
	{
		RobotSample s = GetSample ( true );
		s.stopPickup = true;
//		var iter = samples.GetEnumerator ();
//		RobotSample s = null;
//		while ( iter.MoveNext () )
//		{
//			s = iter.Current;
//		}
//		s.stopPickup = true;
	}

/*	public IEnumerator Sample()
	{
		// Start the Coroutine to Capture Data Every Second.
		// Persist that Information to a CSV and Perist the Camera Frame
		yield return new WaitForSeconds(0.0666666666666667f);

		if (m_saveLocation != "")
		{
			RobotSample sample = new RobotSample();

			sample.timestamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
			sample.steerAngle = SteerAngle;
//			sample.steerAngle = m_SteerAngle / m_MaximumSteerAngle;
			sample.verticalAngle = VerticalAngle;
			sample.throttle = ThrottleInput;
			sample.brake = BrakeInput;
			sample.speed = Speed;
			sample.pitch = Pitch;
			sample.yaw = Yaw;
			sample.roll = Roll;
			sample.position = transform.position;
			sample.rotation = transform.rotation;
			Debug.Log ( "Throt " + sample.throttle + " brake " + sample.brake + " pos " + sample.position + " yaw " + sample.yaw );

			samples.Enqueue(sample);

			sample = null;
			//may or may not be needed
		}

		// Only reschedule if the button hasn't toggled
		if (IsRecording)
		{
			StartCoroutine(Sample());
		}

	}*/

	private void OpenFolder(string location)
	{
		m_saveLocation = location;
		try
		{
			Debug.Log ("Deleting any previous IMG folder and CSV file..");
			if ( File.Exists ( Path.Combine ( m_saveLocation, CSVFileName ) ) )
				File.Delete ( Path.Combine ( m_saveLocation, CSVFileName ) );
			if ( Directory.Exists ( Path.Combine ( m_saveLocation, DirFrames ) ) )
				Directory.Delete ( Path.Combine ( m_saveLocation, DirFrames ), true );
			
		}
		catch (System.Exception e)
		{
			Debug.LogException ( e );
			CancelRecord ();
			return;
		}
		Directory.CreateDirectory (Path.Combine(m_saveLocation, DirFrames));
		if ( beginRecordCallback != null )
			beginRecordCallback ();
	}

	void CancelRecord ()
	{
		cancelRecordCallback ();
	}

	private string WriteImage (Camera camera, string prepend, string timestamp)
	{
		//needed to force camera update 
		camera.Render();
		RenderTexture targetTexture = camera.targetTexture;
		RenderTexture.active = targetTexture;
		Texture2D texture2D = new Texture2D (targetTexture.width, targetTexture.height, TextureFormat.RGB24, false);
		texture2D.ReadPixels (new Rect (0, 0, targetTexture.width, targetTexture.height), 0, 0);
		texture2D.Apply ();
		byte[] image = texture2D.EncodeToJPG ();
		UnityEngine.Object.DestroyImmediate (texture2D);
		string directory = Path.Combine(m_saveLocation, DirFrames);
		string path = Path.Combine(directory, prepend + "_" + timestamp + ".jpg");
		File.WriteAllBytes (path, image);
		image = null;
		return path;
	}

	public static float ConvertAngleToCCWXBased (float angle)
	{
		angle = -angle + 90f;

		while ( angle > 360f )
			angle -= 360f;
		while ( angle < 0f )
			angle += 360f;

		return angle;
	}

	public static float ConvertAngleToCWZBased (float angle)
	{
		angle = -angle - 90f;

		while ( angle > 360f )
			angle -= 360f;
		while ( angle < 0f )
			angle += 360f;

		return angle;
	}
}