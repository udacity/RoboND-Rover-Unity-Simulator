using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public Canvas canvas;
	public FPSRobotInput playerInput;
	public TrainingUI trainingUI;

	public GameObject socketObject;
	public GameObject serverObject;

	RobotRemoteControl remoteControl;

	void Awake ()
	{
		remoteControl = playerInput.GetComponent<RobotRemoteControl> ();
	}

	void Start ()
	{
		EnableCanvas ();
	}

	void LateUpdate ()
	{
		if ( Input.GetKeyDown ( KeyCode.F1 ) )
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene ( UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name );
		}
	}

	void EnableCanvas ()
	{
		canvas.enabled = true;
		playerInput.DisableFocus = true;
		playerInput.Unfocus ();
	}

	public void OnModeSelect (int mode)
	{
		// training
		if ( mode == 0 )
		{
			socketObject.SetActive ( false );
			serverObject.SetActive ( false );

			playerInput.DisableFocus = false;
			playerInput.Focus ();
			remoteControl.enabled = false;
			trainingUI.SetTrainingMode ( true );
		}

		// autonomous
		if ( mode == 1 )
		{
			socketObject.SetActive ( true );
			serverObject.SetActive ( true );

			playerInput.controllable = false;
			remoteControl.enabled = true;
//			playerInput.controller.SwitchCamera (); // set to 3rd person camera by default
			playerInput.DisableFocus = false;
			trainingUI.SetTrainingMode ( false );
			playerInput.controller.transform.eulerAngles = new Vector3 ( 0, Random.Range ( 0f, 360f ), 0 );
		}

		playerInput.controller.SwitchCamera ();
		canvas.enabled = false;
	}

	public void OnExitButton ()
	{
		Application.Quit ();
	}
}