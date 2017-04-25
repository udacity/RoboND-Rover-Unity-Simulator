using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingUI : MonoBehaviour
{
	public RoverStatus roverStatus { get; set; }
	public bool showROSStatus { get; set; }

	public bool isTrainingMode;

	public FPSRobotInput robotInput;
	public IRobotController robotController;

	public RectTransform trainingArea;
	public RectTransform rightArea;
	public Text saveStatus;
	public Text recordStatus;
	public RawImage roboCamImage;
	public RawImage inset1Tex;
	public RawImage inset2Tex;
	public RawImage inset3Tex;
	public Text roverStatusText;
	[System.NonSerialized]
	public RoverStatus lastStatus;

	float roboCamAlpha = 1;
	bool recording;
	bool saveRecording;

	void Awake ()
	{
		robotController = robotInput.controller;
		SetTrainingMode ( isTrainingMode );
		saveStatus.text = "";
		roverStatusText.text = "ROS status: " + roverStatus.ToString ();
		roverStatusText.enabled = false;
	}

	void LateUpdate ()
	{
		if ( robotController.getSaveStatus () )
		{
			saveStatus.text = "Capturing Data: " + (int) ( 100 * robotController.getSavePercent () ) + "%";
		}
		else if(saveRecording) 
		{
			saveStatus.text = "";
			recording = false;
//			RecordingPause.SetActive(false);
			recordStatus.text = "Not Recording";
			recordStatus.color = Color.red;
			saveRecording = false;
		}

		if ( Input.GetButtonDown ( "Record" ) )
			ToggleRecording ();

		if ( Input.GetKeyDown ( KeyCode.C ) )
		{
			roboCamAlpha = 1 - roboCamAlpha;
			roboCamImage.CrossFadeAlpha ( roboCamAlpha, 0.35f, false );
		}
	}

	void ToggleRecording ()
	{
		if ( recording )
		{
			saveRecording = true;
			robotController.IsRecording = false;
//			robotInput.DisableFocus = true;
//			robotInput.Unfocus ();

		} else
		{
			if ( robotController.CheckSaveLocation ( OnBeginRecord, OnCancelRecord ) )
			{
				OnBeginRecord ();
//				recording = true;
//				robotController.IsRecording = true;
//				recordStatus.text = "RECORDING";
//				robotInput.DisableFocus = false;
//				robotInput.Focus ();
			} else
			{
				robotInput.DisableFocus = true;
				robotInput.Unfocus ();
			}
		}
	}

	void OnBeginRecord ()
	{
		recording = true;
		robotController.IsRecording = true;
		recordStatus.text = "RECORDING";
		recordStatus.color = Color.green;
		robotInput.DisableFocus = false;
		robotInput.Focus ();
	}

	void OnCancelRecord ()
	{
//		recording = false;
//		robotController.IsRecording = false;
//		recordStatus.text = "Not Recording";
//		recordStatus.color = Color.red;
		robotInput.DisableFocus = false;
		robotInput.Focus ();
	}

	public void SetTrainingMode (bool training)
	{
		isTrainingMode = training;
		trainingArea.gameObject.SetActive ( isTrainingMode );
		rightArea.gameObject.SetActive ( isTrainingMode );
		enabled = isTrainingMode;
		robotInput.isTrainingMode = training;
		inset1Tex.enabled = !training;
		inset1Tex.CrossFadeAlpha ( 0, 0, true );
		inset2Tex.enabled = !training;
		inset2Tex.CrossFadeAlpha ( 0, 0, true );
		inset3Tex.enabled = !training;
		inset3Tex.CrossFadeAlpha ( 0, 0, true );
	}
}