using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Ros_CSharp;
using Messages;
using Header_t = Messages.std_msgs.Header;
using uint8 = System.Byte;

namespace hector_uav_msgs
{
	public class MotorCommand : IRosMessage
	{
		public Header_t header;
		float[] force;
		float[] torque;
		float[] frequency;
		float[] voltage;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "ccd4d4d4606731d1c73409e9bfa55808"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
float32[] force
float32[] torque
float32[] frequency
float32[] voltage"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__MotorCommand; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public MotorCommand()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public MotorCommand(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public MotorCommand(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			// force
			int length = BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			force = new float[length];
			for (int i = 0; i < length; i++)
			{
				force [ i ] = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
				currentIndex += 4;
			}
			// torque
			length = BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			torque = new float[length];
			for (int i = 0; i < length; i++)
			{
				torque [ i ] = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
				currentIndex += 4;
			}
			// frequency
			length = BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			frequency = new float[length];
			for (int i = 0; i < length; i++)
			{
				frequency [ i ] = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
				currentIndex += 4;
			}
			// voltage
			length = BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			voltage = new float[length];
			for (int i = 0; i < length; i++)
			{
				voltage [ i ] = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
				currentIndex += 4;
			}
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			int pos = 0;
			byte[] headerBytes = header.Serialize ();
			BinarySerializer b = new BinarySerializer ();
			b.WriteInt ( headerBytes.Length );
			b.WriteBytes ( headerBytes );
			// force
			b.WriteInt ( force.Length );
			for ( int i = 0; i < force.Length; i++ )
				b.WriteFloat ( force [ i ] );
			// torque
			b.WriteInt ( torque.Length );
			for ( int i = 0; i < torque.Length; i++ )
				b.WriteFloat ( torque [ i ] );
			// frequency
			b.WriteInt ( frequency.Length );
			for ( int i = 0; i < frequency.Length; i++ )
				b.WriteFloat ( frequency [ i ] );
			// voltage
			b.WriteInt ( voltage.Length );
			for ( int i = 0; i < voltage.Length; i++ )
				b.WriteFloat ( voltage [ i ] );

			return b.GetBytes ();
		}

		public override void Randomize()
		{
			header.Randomize ();
			force = new float[10];
			torque = new float[10];
			frequency = new float[10];
			voltage = new float[10];
			for (int i = 0; i < 10; i++)
			{
				force [ i ] = UnityEngine.Random.value;
				torque [ i ] = UnityEngine.Random.value;
				frequency [ i ] = UnityEngine.Random.value;
				voltage [ i ] = UnityEngine.Random.value;
			}
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			MotorCommand other = (MotorCommand)____other;

			ret &= header.Equals ( other.header );
			ret &= force.Equals ( other.force );
			ret &= torque.Equals ( other.torque );
			ret &= frequency.Equals ( other.frequency );
			ret &= voltage.Equals ( other.voltage );
			return ret;
		}
	}
}