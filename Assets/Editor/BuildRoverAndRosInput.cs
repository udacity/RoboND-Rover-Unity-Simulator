using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildRoverAndRosInput : MonoBehaviour
{
	[MenuItem ("Build/Build Rover and ROS Input", false, 10)]
	static void BuildRoverAndInput ()
	{
		string buildOutput = "Builds/ROS/";
		string fileName = "proto3-ros";
		if ( EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 )
			fileName += ".exe";
		else
			fileName += ".app";
		string[] levels = new string[1] { "Assets/Scenes/proto3.unity" };
		BuildPipeline.BuildPlayer ( levels, buildOutput + fileName, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None );

		fileName = "rosRemoteInput";
		if ( EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 )
			fileName += ".exe";
		else
			fileName += ".app";
		levels [ 0 ] = "Assets/Scenes/ros_rover_control.unity";
			BuildPipeline.BuildPlayer ( levels, buildOutput + fileName, EditorUserBuildSettings.activeBuildTarget, BuildOptions.ShowBuiltPlayer );
	}

	[MenuItem ("Build/Build Quad and ROS Input", false, 10)]
	static void BuildQuadAndInput ()
	{
		string buildOutput = "Builds/ROS/";
		string fileName = "proto4-ros";
		if ( EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 )
			fileName += ".exe";
		else
			fileName += ".app";
		string[] levels = new string[1] { "Assets/Scenes/proto4.unity" };
		BuildPipeline.BuildPlayer ( levels, buildOutput + fileName, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None );

		fileName = "ros-quad-input";
		if ( EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 )
			fileName += ".exe";
		else
			fileName += ".app";
		levels [ 0 ] = "Assets/Scenes/ros_quad_control.unity";
		BuildPipeline.BuildPlayer ( levels, buildOutput + fileName, EditorUserBuildSettings.activeBuildTarget, BuildOptions.ShowBuiltPlayer );
	}
}