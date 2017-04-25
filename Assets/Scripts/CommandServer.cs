using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SocketIO;
using System;
using System.Security.AccessControl;
using UnityEngine.UI;

public class CommandServer : MonoBehaviour
{
	public RobotRemoteControl robotRemoteControl;
	public IRobotController robotController;
	public Camera frontFacingCamera;
	private SocketIOComponent _socket;
	public RawImage inset1;
	public RawImage inset2;
	public RawImage inset3;

	Texture2D inset1Tex;
	Texture2D inset2Tex;
	Texture2D inset3Tex;

	void Start()
	{
		_socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
		_socket.On("open", OnOpen);
		_socket.On("steer", OnSteer);
		_socket.On("manual", onManual);
		_socket.On ( "fixed_turn", OnFixedTurn );
		_socket.On ( "pickup", OnPickup );
		_socket.On ( "get_samples", GetSamplePositions );
		robotController = robotRemoteControl.robot;
		frontFacingCamera = robotController.recordingCam;
		inset1Tex = new Texture2D ( 1, 1 );
		inset2Tex = new Texture2D ( 1, 1 );
		inset3Tex = new Texture2D ( 1, 1 );
	}

	void Update ()
	{
		if ( Input.GetKeyDown ( KeyCode.P ) )
		{
			GetSamplePositions ( null );
		}
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

	void OnSteer(SocketIOEvent obj)
	{
//		Debug.Log ( "Steer" );
		JSONObject jsonObject = obj.data;
		robotRemoteControl.SteeringAngle = -float.Parse(jsonObject.GetField("steering_angle").str); // he wanted the angles CCW
		robotRemoteControl.ThrottleInput = float.Parse(jsonObject.GetField("throttle").str);
		if ( jsonObject.HasField ( "brake" ) )
			robotRemoteControl.BrakeInput = float.Parse ( jsonObject.GetField ( "brake" ).str );
		else
			robotRemoteControl.BrakeInput = 0;

		// try to load image1
		bool loaded = false;
		Vector2 size = Vector2.zero;
		Texture2D tex = new Texture2D ( 1, 1 );
		string imageInfo = "";
		byte[] imageBytes = null;
		if ( jsonObject.HasField ( "inset_image" ) )
			imageInfo = jsonObject.GetField ( "inset_image" ).str;
		if ( !string.IsNullOrEmpty ( imageInfo ) )
			imageBytes = Convert.FromBase64String ( imageInfo );
		if ( imageBytes != null && imageBytes.Length != 0 )
			loaded = tex.LoadImage ( imageBytes, true );
		if ( loaded && inset1 != null && tex.width > 1 && tex.height > 1 )
		{
			inset1Tex = tex;
			inset1.texture = inset1Tex;
			size = inset1.rectTransform.sizeDelta;
			size.x = 1f * inset1Tex.width / inset1Tex.height * size.y;
			inset1.rectTransform.sizeDelta = size;
			inset1.CrossFadeAlpha ( 1, 0.3f, false );
		} else
		if ( inset1 != null )
		{
//			if ( tex.width == 1 || tex.height == 1 )
//				inset1.CrossFadeAlpha ( 0, 0.0f, true );
		}
		// try to load image2
		loaded = false;
		tex = new Texture2D ( 1, 1 );
		imageInfo = "";
		imageBytes = null;
		if ( jsonObject.HasField ( "inset_image2" ) )
			imageInfo = jsonObject.GetField ( "inset_image2" ).str;
		if ( !string.IsNullOrEmpty ( imageInfo ) )
			imageBytes = Convert.FromBase64String ( imageInfo );
		if ( imageBytes != null && imageBytes.Length != 0 )
			loaded = tex.LoadImage ( imageBytes, true );
		if ( loaded && inset2Tex != null && tex.width > 1 && tex.height > 1 )
		{
			inset2Tex = tex;
			inset2.texture = inset2Tex;
			size = inset2.rectTransform.sizeDelta;
			size.x = 1f * inset2Tex.width / inset2Tex.height * size.y;
			inset2.rectTransform.sizeDelta = size;
			inset2.CrossFadeAlpha ( 1, 0.3f, false );
		} else
		if ( inset2 != null )
		{
//			if ( tex.width == 1 || tex.height == 1 )
//				inset2.CrossFadeAlpha ( 0, 0.0f, true );
		}
		// try to load image3
		loaded = false;
		tex = new Texture2D ( 1, 1 );
		imageInfo = "";
		imageBytes = null;
		if ( jsonObject.HasField ( "inset_image3" ) )
			imageInfo = jsonObject.GetField ( "inset_image3" ).str;
		if ( !string.IsNullOrEmpty ( imageInfo ) )
			imageBytes = Convert.FromBase64String ( imageInfo );
		if ( imageBytes != null && imageBytes.Length != 0 )
			loaded = tex.LoadImage ( imageBytes, true );
		if ( loaded && inset3Tex != null && tex.width > 1 && tex.height > 1 )
		{
			inset3Tex = tex;
			inset3.texture = inset3Tex;
			size = inset3.rectTransform.sizeDelta;
			size.x = 1f * inset3Tex.width / inset3Tex.height * size.y;
			inset3.rectTransform.sizeDelta = size;
			inset3.CrossFadeAlpha ( 1, 0.3f, false );
		} else
		if ( inset3 != null )
		{
//			if ( tex.width == 1 || tex.height == 1 )
//				inset3.CrossFadeAlpha ( 0, 0.0f, true );
		}
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
			data["position"] = pos.x.ToString ("N4") + "," + pos.z.ToString ("N4");
			data["pitch"] = robotController.Pitch.ToString ("N4");
			// new: convert the angle to CCW, x-based
			data["yaw"] = IRobotController.ConvertAngleToCCWXBased ( robotController.Yaw ).ToString ("N4");
			data["roll"] = robotController.Roll.ToString ("N4");
			data["fixed_turn"] = robotController.IsTurningInPlace ? "1" : "0";
			data["near_sample"] = robotController.IsNearObjective ? "1" : "0";
			data["picking_up"] = robotController.IsPickingUpSample ? "1" : "0";
			data["image"] = Convert.ToBase64String(CameraHelper.CaptureFrame(frontFacingCamera));

//			Debug.Log ("sangle " + data["steering_angle"] + " vert " + data["vert_angle"] + " throt " + data["throttle"] + " speed " + data["speed"] + " image " + data["image"]);
			_socket.Emit("telemetry", new JSONObject(data));
		});
	}

	void GetSamplePositions (SocketIOEvent obj)
	{
		UnityMainThreadDispatcher.Instance ().Enqueue ( () =>
		{
			Debug.Log ("Sending sample positions");

			Dictionary<string, string> data = new Dictionary<string, string> ();

			int count = ObjectiveSpawner.samples.Length;
			data[ "sample_count" ] = count.ToString ();
			string x = "";
			string y = "";

			for (int i = 0; i < count; i++ )
			{
				GameObject go = ObjectiveSpawner.samples[i];
				x += go.transform.position.x.ToString ("N2") + ",";
				y += go.transform.position.z.ToString ("N2") + ",";
//				data[ "sample" + i + "_pos_x" ] = go.transform.position.x.ToString ("N4");
//				data[ "sample" + i + "_pos_y" ] = go.transform.position.z.ToString ("N4");
			}
			x = x.TrimEnd (',');
			y = y.TrimEnd (',');
			data["samples_x"] = x;
			data["samples_y"] = y;
//			Debug.Log (x + "\n" + y);

//			foreach (KeyValuePair<string, string> pair in data)
//				Debug.Log ("key: " + pair.Key + " value: " + pair.Value + "\n");
			_socket.Emit ("sample_positions", new JSONObject(data));
		} );
	}
}