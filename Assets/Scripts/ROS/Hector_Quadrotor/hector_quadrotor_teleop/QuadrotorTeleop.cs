//=================================================================================================
// Copyright (c) 2012-2016, Institute of Flight Systems and Automatic Control,
// Technische Universität Darmstadt.
// All rights reserved.

// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of hector_quadrotor nor the names of its contributors
//       may be used to endorse or promote products derived from this software
//       without specific prior written permission.

// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//=================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ros_CSharp;
using Messages;
using Messages.geometry_msgs;
using Messages.tf2_msgs;
using Messages.sensor_msgs;
using hector_uav_msgs;

public class QuadrotorTeleop : MonoBehaviour
{
	public struct SAxis
	{
		public int axis;
		public double factor;
		public double offset;
	};

	public struct SButton
	{
		public int button;
	};

	public struct SAxes
	{
		public SAxis x;
		public SAxis y;
		public SAxis z;
		public SAxis thrust;
		public SAxis yaw;
	}

	public struct SButtons
	{
		public SButton slow;
		public SButton go;
		public SButton stop;
		public SButton interrupt;
	}


//	private:
//	typedef actionlib::SimpleActionClient<LandingAction> LandingClient;
//	typedef actionlib::SimpleActionClient<TakeoffAction> TakeoffClient;
//	typedef actionlib::SimpleActionClient<PoseAction> PoseClient;

	NodeHandle nh;
	Subscriber<Joy> joySubscriber;
	Publisher<TwistStamped> velocityPublisher;
	Publisher<AttitudeCommand> attitudePublisher;
	Publisher<YawRateCommand> yawRatePublisher;
	Publisher<ThrustCommand> thrustPublisher;
	ServiceClient<EnableMotors> motorEnableService;
//	boost::shared_ptr<LandingClient> landingClient;
//	boost::shared_ptr<TakeoffClient> takeoffClient;
//	boost::shared_ptr<PoseClient> poseClient;

	PoseStamped pose;
	double yaw;
	double slowFactor;
	string baseLinkFrame, baseStabilizedFrame, worldFrame;
	SButtons sButtons;
	SAxes sAxes;


	public void joyAttitudeCallback(Joy joy)
	{

		AttitudeCommand attitude = new AttitudeCommand ();
		ThrustCommand thrust = new ThrustCommand ();
		YawRateCommand yawrate = new YawRateCommand ();

//		attitude.header.stamp = thrust.header.stamp = yawrate.header.stamp = Time::now();
		attitude.header.frame_id = yawrate.header.frame_id = baseStabilizedFrame;
		thrust.header.frame_id = baseLinkFrame;

		attitude.roll = (float) ( -getAxis ( joy, sAxes.y ) * Math.PI / 180.0 );
		attitude.pitch = (float) ( getAxis ( joy, sAxes.x ) * Math.PI / 180.0 );
		if (getButton(joy, sButtons.slow))
		{
			attitude.roll *= (float) slowFactor;
			attitude.pitch *= (float) slowFactor;
		}
		attitudePublisher.publish(attitude);

		thrust.thrust = (float) getAxis(joy, sAxes.thrust);
		thrustPublisher.publish(thrust);

		yawrate.turnrate = (float) ( getAxis ( joy, sAxes.yaw ) * Math.PI / 180.0 );
		if (getButton(joy, sButtons.slow))
		{
			yawrate.turnrate *= (float) slowFactor;
		}

		yawRatePublisher.publish(yawrate);

		if (getButton(joy, sButtons.stop))
		{
			enableMotors(false);
		}
		else if (getButton(joy, sButtons.go))
		{
			enableMotors(true);
		}
	}

	void joyTwistCallback(Joy joy)
	{
		TwistStamped velocity = new TwistStamped ();
		velocity.header.frame_id = baseStabilizedFrame;
//		velocity.header.stamp = Time::now();

		velocity.twist.linear.x = getAxis(joy, sAxes.x);
		velocity.twist.linear.y = getAxis(joy, sAxes.y);
		velocity.twist.linear.z = getAxis(joy, sAxes.z);
		velocity.twist.angular.z = getAxis(joy, sAxes.yaw) * Math.PI/180.0;
		if (getButton(joy, sButtons.slow))
		{
			velocity.twist.linear.x *= slowFactor;
			velocity.twist.linear.y *= slowFactor;
			velocity.twist.linear.z *= slowFactor;
			velocity.twist.angular.z *= slowFactor;
		}
		velocityPublisher.publish(velocity);

		if (getButton(joy, sButtons.stop))
		{
			enableMotors(false);
		}
		else if (getButton(joy, sButtons.go))
		{
			enableMotors(true);
		}
	}

	void joyPoseCallback(Joy joy)
	{
//		Time now = Time::now();
		double dt = 0.0;
//		if (!pose.header.stamp.isZero()) {
//			dt = Math.Max(0.0, Math.Min(1.0, (now - pose.header.stamp).toSec()));
//		}

		if (getButton(joy, sButtons.go))
		{
//			pose.header.stamp = now;
			pose.header.frame_id = worldFrame;
			pose.pose.position.x += (Math.Cos(yaw) * getAxis(joy, sAxes.x) - Math.Sin(yaw) * getAxis(joy, sAxes.y)) * dt;
			pose.pose.position.y += (Math.Cos(yaw) * getAxis(joy, sAxes.y) + Math.Sin(yaw) * getAxis(joy, sAxes.x)) * dt;
			pose.pose.position.z += getAxis(joy, sAxes.z) * dt;
			yaw += getAxis(joy, sAxes.yaw) * Math.PI/180.0 * dt;
			tf.net.emQuaternion q = tf.net.emQuaternion.FromRPY ( new tf.net.emVector3 ( 0, 0, yaw ) );
//			tf2::Quaternion q;
//			q.setRPY(0.0, 0.0, yaw);

			pose.pose.orientation = q.ToMsg ();
//			pose.pose.orientation = tf2::toMsg(q);


			PoseGoal goal;
//			goal.target_pose = pose;
//			poseClient.sendGoal(goal);
		}
		if (getButton(joy, sButtons.interrupt))
		{
//			poseClient.cancelGoalsAtAndBeforeTime(Time::now());
		}
		if (getButton(joy, sButtons.stop))
		{
//			landingClient.sendGoalAndWait(LandingGoal(), Duration(10.0), Duration(10.0));
		}
	}

	public double getAxis(Joy joy, SAxis axis)
	{
		if (axis.axis == 0 || Math.Abs(axis.axis) > joy.axes.Length)
		{
			ROS.Error("Axis " + axis.axis + " out of range, joy has " + joy.axes.Length + " axes");
			return 0;
		}

		double output = Math.Abs(axis.axis) / axis.axis * joy.axes[Math.Abs(axis.axis) - 1] * axis.factor + axis.offset;

		// TODO keep or remove deadzone? may not be needed
		// if (Math.Abs(output) < axis.max_ * 0.2)
		// {
		//   output = 0.0;
		// }

		return output;
	}

	public bool getButton(Joy joy, SButton button)
	{
		if (button.button <= 0 || button.button > joy.buttons.Length)
		{
			ROS.Error("Button " + button.button + " out of range, joy has " + joy.buttons.Length + " buttons");
			return false;
		}

		return joy.buttons[button.button - 1] > 0;
	}

	public bool enableMotors(bool enable)
	{
//		if (!motorEnableService.waitForExistence(Duration(5.0)))
//		{
//			ROS.Warn("Motor enable service not found");
//			return false;
//		}

		EnableMotors srv = new EnableMotors ();
		srv.req.enable = enable;
		return motorEnableService.call(srv);
	}

	public void stop()
	{
//		if (velocityPublisher.getNumSubscribers() > 0)
//		{
//			velocityPublisher.publish(TwistStamped());
//		}
//		if (attitudePublisher.getNumSubscribers() > 0)
//		{
//			attitudePublisher.publish(AttitudeCommand());
//		}
//		if (thrustPublisher.getNumSubscribers() > 0)
//		{
//			thrustPublisher.publish(ThrustCommand());
//		}
//		if (yawRatePublisher.getNumSubscribers() > 0)
//		{
//			yawRatePublisher.publish(new YawRateCommand());
//		}
	}

	void Awake ()
	{
		ROSController.StartROS ( OnRosInit );
	}

	void OnDestroy ()
	{
		stop ();
	}

	void OnRosInit ()
	{
		NodeHandle privateNH = new NodeHandle("~");

//		privateNH.param<int>("x_axis", sAxes.x.axis, 5);
//		privateNH.param<int>("y_axis", sAxes.y.axis, 4);
//		privateNH.param<int>("z_axis", sAxes.z.axis, 2);
//		privateNH.param<int>("thrust_axis", sAxes.thrust.axis, -3);
//		privateNH.param<int>("yaw_axis", sAxes.yaw.axis, 1);
//
//		privateNH.param<double>("yaw_velocity_max", sAxes.yaw.factor, 90.0);
//
//		privateNH.param<int>("slow_button", sButtons.slow.button, 4);
//		privateNH.param<int>("go_button", sButtons.go.button, 1);
//		privateNH.param<int>("stop_button", sButtons.stop.button, 2);
//		privateNH.param<int>("interrupt_button", sButtons.interrupt.button, 3);
//		privateNH.param<double>("slow_factor", slowFactor, 0.2);

		// TODO dynamic reconfig
		string control_mode;
//		privateNH.param<std::string>("control_mode", control_mode, "twist");

		NodeHandle robot_nh = new NodeHandle ();

		// TODO factor out
//		robot_nh.param<std::string>("base_link_frame", baseLinkFrame, "base_link");
//		robot_nh.param<std::string>("world_frame", worldFrame, "world");
//		robot_nh.param<std::string>("base_stabilized_frame", baseStabilizedFrame, "base_stabilized");

//		if (control_mode == "attitude")
//		{
//			privateNH.param<double>("pitch_max", sAxes.x.factor, 30.0);
//			privateNH.param<double>("roll_max", sAxes.y.factor, 30.0);
//			privateNH.param<double>("thrust_max", sAxes.thrust.factor, 10.0);
//			privateNH.param<double>("thrust_offset", sAxes.thrust.offset, 10.0);
//
//			joySubscriber = nh.subscribe<sensor_msgs::Joy>("joy", 1,
//				boost::bind(&Teleop::joyAttitudeCallback, this, _1));
//			attitudePublisher = robot_nh.advertise<AttitudeCommand>(
//				"command/attitude", 10);
//			yawRatePublisher = robot_nh.advertise<YawRateCommand>(
//				"command/yawrate", 10);
//			thrustPublisher = robot_nh.advertise<ThrustCommand>("command/thrust",
//				10);
//		}
//		else if (control_mode == "velocity")
//		{
//			privateNH.param<double>("x_velocity_max", sAxes.x.factor, 2.0);
//			privateNH.param<double>("y_velocity_max", sAxes.y.factor, 2.0);
//			privateNH.param<double>("z_velocity_max", sAxes.z.factor, 2.0);
//
//			joySubscriber = nh.subscribe<sensor_msgs::Joy>("joy", 1,
//				boost::bind(&Teleop::joyTwistCallback, this, _1));
//			velocityPublisher = robot_nh.advertise<TwistStamped>("command/twist",
//				10);
//		}
//		else if (control_mode == "position")
//		{
//			privateNH.param<double>("x_velocity_max", sAxes.x.factor, 2.0);
//			privateNH.param<double>("y_velocity_max", sAxes.y.factor, 2.0);
//			privateNH.param<double>("z_velocity_max", sAxes.z.factor, 2.0);
//
//			joySubscriber = nh.subscribe<sensor_msgs::Joy>("joy", 1,
//				boost::bind(&Teleop::joyPoseCallback, this, _1));
//
//			pose.pose.position.x = 0;
//			pose.pose.position.y = 0;
//			pose.pose.position.z = 0;
//			pose.pose.orientation.x = 0;
//			pose.pose.orientation.y = 0;
//			pose.pose.orientation.z = 0;
//			pose.pose.orientation.w = 1;
//		}
//		else
//		{
//			ROS.Error("Unsupported control mode: " + control_mode);
//		}

		motorEnableService = robot_nh.serviceClient<EnableMotors>(
			"enable_motors");
//		takeoffClient = boost::shared_ptr<TakeoffClient>(new TakeoffClient(robot_nh, "action/takeoff"));
//		landingClient = boost::shared_ptr<LandingClient>(new LandingClient(robot_nh, "action/landing"));
//		poseClient = boost::shared_ptr<PoseClient>(new PoseClient(robot_nh, "action/pose"));
	}
}