using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SocketIO;
using System;
using System.Security.AccessControl;
using UnityEngine.UI;
using System.Text;

public class CommandServer : MonoBehaviour
{
	public RobotRemoteControl robotRemoteControl;
	public IRobotController robotController;
	public Camera frontFacingCamera;
	private SocketIOComponent _socket;
	public RawImage inset1;
	public RawImage inset2;
	public RawImage inset3;

//	Texture2D inset1Tex;
//	Texture2D inset2Tex;
//	Texture2D inset3Tex;

	void Start()
	{
		_socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
		_socket.On("open", OnOpen);
		_socket.On("data", OnData);
		_socket.On("manual", onManual);
		_socket.On ( "fixed_turn", OnFixedTurn );
		_socket.On ( "pickup", OnPickup );
//		_socket.On ( "get_samples", GetSamplePositions );
		robotController = robotRemoteControl.robot;
		frontFacingCamera = robotController.recordingCam;
//		inset1Tex = new Texture2D ( 1, 1 );
//		inset2Tex = new Texture2D ( 1, 1 );
//		inset3Tex = new Texture2D ( 1, 1 );
	}

	void OnOpen(SocketIOEvent obj)
	{
//		Debug.Log("Connection Open");
		EmitTelemetry(obj);
	}

	// 
	void onManual(SocketIOEvent obj)
	{
		EmitTelemetry (obj);
	}

	void OnData(SocketIOEvent obj)
	{
//		Debug.Log ( "Steer" );
		JSONObject jsonObject = obj.data;
		robotRemoteControl.SteeringAngle = -float.Parse(jsonObject.GetField("steering_angle").str); // he wanted the angles CCW
		robotRemoteControl.ThrottleInput = float.Parse(jsonObject.GetField("throttle").str);
		if ( jsonObject.HasField ( "brake" ) )
			robotRemoteControl.BrakeInput = float.Parse ( jsonObject.GetField ( "brake" ).str );
		else
			robotRemoteControl.BrakeInput = 0;

		// testing memory leak
//		return;

		// try to load image1
		bool loaded = false;
		Vector2 size = Vector2.zero;
		Texture2D tex = null;
		string imageInfo = "";
		byte[] imageBytes = null;
		if ( jsonObject.HasField ( "inset_image1" ) )
			imageInfo = jsonObject.GetField ( "inset_image1" ).str;
		if ( !string.IsNullOrEmpty ( imageInfo ) )
			imageBytes = Convert.FromBase64String ( imageInfo );
		if ( imageBytes != null && imageBytes.Length != 0 )
		{
			tex = new Texture2D ( 1, 1 );
			loaded = tex.LoadImage ( imageBytes, true );
		}
		if ( loaded && inset1 != null && tex.width > 1 && tex.height > 1 )
		{
			if ( inset1.texture != null )
				Destroy ( inset1.texture );
			inset1.texture = tex;
//			inset1Tex = tex;
//			inset1.texture = inset1Tex;
			size = inset1.rectTransform.sizeDelta;
			size.x = 1f * tex.width / tex.height * size.y;
//			size.x = 1f * inset1Tex.width / inset1Tex.height * size.y;
			inset1.rectTransform.sizeDelta = size;
			inset1.CrossFadeAlpha ( 1, 0.3f, false );
		} else
		if ( inset1 != null )
		{
//			tex = null;
		}
		// try to load image2
		loaded = false;
//		if ( tex != null )
//			Destroy ( tex );
		tex = null;
		imageInfo = "";
		imageBytes = null;
		if ( jsonObject.HasField ( "inset_image2" ) )
			imageInfo = jsonObject.GetField ( "inset_image2" ).str;
		if ( !string.IsNullOrEmpty ( imageInfo ) )
			imageBytes = Convert.FromBase64String ( imageInfo );
		if ( imageBytes != null && imageBytes.Length != 0 )
		{
			tex = new Texture2D ( 1, 1 );
			loaded = tex.LoadImage ( imageBytes, true );
		}
		if ( loaded && inset2 != null && tex.width > 1 && tex.height > 1 )
		{
			if ( inset2.texture != null )
				Destroy ( inset2.texture );
			inset2.texture = tex;
//			inset2Tex = tex;
//			inset2.texture = inset2Tex;
			size = inset2.rectTransform.sizeDelta;
			size.x = 1f * tex.width / tex.height * size.y;
//			size.x = 1f * inset2Tex.width / inset2Tex.height * size.y;
			inset2.rectTransform.sizeDelta = size;
			inset2.CrossFadeAlpha ( 1, 0.3f, false );
		} else
		if ( inset2 != null )
		{
//			if ( inset1.texture != null )
//				Destroy ( inset1.texture );
		}
		// try to load image3
		loaded = false;
//		if ( tex != null )
//			Destroy ( tex );
		tex = null;
		imageInfo = "";
		imageBytes = null;
		if ( jsonObject.HasField ( "inset_image3" ) )
			imageInfo = jsonObject.GetField ( "inset_image3" ).str;
		if ( !string.IsNullOrEmpty ( imageInfo ) )
			imageBytes = Convert.FromBase64String ( imageInfo );
		if ( imageBytes != null && imageBytes.Length != 0 )
		{
			tex = new Texture2D ( 1, 1 );
			loaded = tex.LoadImage ( imageBytes, true );
		}
		if ( loaded && inset3 != null && tex.width > 1 && tex.height > 1 )
		{
			if ( inset3.texture != null )
				Destroy ( inset3.texture );
			inset3.texture = tex;
//			inset3Tex = tex;
//			inset3.texture = inset3Tex;
			size = inset3.rectTransform.sizeDelta;
			size.x = 1f * tex.width / tex.height * size.y;
//			size.x = 1f * inset3Tex.width / inset3Tex.height * size.y;
			inset3.rectTransform.sizeDelta = size;
			inset3.CrossFadeAlpha ( 1, 0.3f, false );
		} else
		if ( inset3 != null )
		{
		}
//		if ( tex != null )
//			Destroy ( tex );
		tex = null;
		EmitTelemetry(obj);
	}

	void OnFixedTurn(SocketIOEvent obj)
	{
		JSONObject json = obj.data;
		float angle = float.Parse ( json.GetField ( "angle" ).str );
		float time = 0;
		if ( json.HasField ( "time" ) )
			time = float.Parse ( json.GetField ( "time" ).str );
		robotRemoteControl.FixedTurn ( angle, time );
		EmitTelemetry ( obj );
	}

	void OnPickup (SocketIOEvent obj)
	{
		robotRemoteControl.PickupSample ();
		EmitTelemetry ( obj );
	}

	void EmitTelemetry(SocketIOEvent obj)
	{
//		Debug.Log ( "Emitting" );
		UnityMainThreadDispatcher.Instance().Enqueue(() =>
		{
			print("Attempting to Send...");

			// Collect Data from the Car
			Dictionary<string, string> data = new Dictionary<string, string>();

			data["steering_angle"] = robotController.SteerAngle.ToString("N4");
//			data["vert_angle"] = robotController.VerticalAngle.ToString ("N4");
			data["throttle"] = robotController.ThrottleInput.ToString("N4");
			data["brake"] = robotController.BrakeInput.ToString ("N4");
			data["speed"] = robotController.Speed.ToString("N4");
			Vector3 pos = robotController.Position;
			data["position"] = pos.x.ToString ("N4") + robotController.csvSeparatorChar + pos.z.ToString ("N4");
			data["pitch"] = robotController.Pitch.ToString ("N4");
			// new: convert the angle to CCW, x-based
			data["yaw"] = IRobotController.ConvertAngleToCCWXBased ( robotController.Yaw ).ToString ("N4");
			data["roll"] = robotController.Roll.ToString ("N4");
//			data["fixed_turn"] = robotController.IsTurningInPlace ? "1" : "0";
			data["near_sample"] = robotController.IsNearObjective ? "1" : "0";
			data["picking_up"] = robotController.IsPickingUpSample ? "1" : "0";
//			Debug.Log ("picking_up is " + robotController.IsPickingUpSample);
			data["sample_count"] = ObjectiveSpawner.samples.Length.ToString ();

			StringBuilder sample_x = new StringBuilder ();
			StringBuilder sample_y = new StringBuilder ();
			for (int i = 0; i <ObjectiveSpawner.samples.Length; i++)
			{
				GameObject go = ObjectiveSpawner.samples[i];
				sample_x.Append ( go.transform.position.x.ToString ("N2") + robotController.csvSeparatorChar );
				sample_y.Append ( go.transform.position.z.ToString ("N2") + robotController.csvSeparatorChar );
			}
			if (ObjectiveSpawner.samples.Length != 0)
			{
				sample_x.Remove (sample_x.Length - 1, 1);
				sample_y.Remove (sample_y.Length - 1, 1);
			}
			data["samples_x"] = sample_x.ToString ();
			data["samples_y"] = sample_y.ToString ();
			data["image"] = Convert.ToBase64String(CameraHelper.CaptureFrame(frontFacingCamera));

//			Debug.Log ("sangle " + data["steering_angle"] + " vert " + data["vert_angle"] + " throt " + data["throttle"] + " speed " + data["speed"] + " image " + data["image"]);
			_socket.Emit("telemetry", new JSONObject(data));
		});
	}
}
