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
	public class MotorStatus : IRosMessage
	{
		public Header_t header;
		public bool on;
		public bool running;
		public float[] voltage;
		public float[] frequency;
		public float[] current;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "d771017cd812838d32da48fbe32b0928"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
uint8 on
uint8 running
float32[] voltage
float32[] frequency
float32[] current"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__MotorStatus; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public MotorStatus()
		{
			on = false;
			running = false;
			voltage = new float[0];
			frequency = new float[0];
			current = new float[0];
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public MotorStatus(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public MotorStatus(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			on = SERIALIZEDSTUFF [ currentIndex++ ] != 0;
			running = SERIALIZEDSTUFF [ currentIndex++ ] != 0;
			// voltage
			int count = BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			voltage = new float[count];
			for (int i = 0; i < count; i++)
			{
				voltage [ i ] = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
				currentIndex += 4;
			}
			// frequency
			count = BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			frequency = new float[count];
			for (int i = 0; i < count; i++)
			{
				frequency [ i ] = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
				currentIndex += 4;
			}
			// current
			count = BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			current = new float[count];
			for (int i = 0; i < count; i++)
			{
				current [ i ] = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
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
			b.WriteBool ( on );
			b.WriteBool ( running );
			// voltage
			b.WriteInt ( voltage.Length );
			for ( int i = 0; i < voltage.Length; i++ )
				b.WriteFloat ( voltage [ i ] );
			// frequency
			b.WriteInt ( frequency.Length );
			for ( int i = 0; i < frequency.Length; i++ )
				b.WriteFloat ( frequency [ i ] );
			// current
			b.WriteInt ( current.Length );
			for ( int i = 0; i < current.Length; i++ )
				b.WriteFloat ( current [ i ] );

			return b.GetBytes ();
		}

		public override void Randomize()
		{
			header = new Header_t ();
			header.Randomize ();
			on = UnityEngine.Random.value > 0.5f;
			running = on;
			voltage = new float[10];
			frequency = new float[10];
			current = new float[10];
			for (int i = 0; i < 10; i++)
			{
				voltage [ i ] = UnityEngine.Random.value;
				frequency [ i ] = UnityEngine.Random.value;
				current [ i ] = UnityEngine.Random.value;
			}
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			MotorStatus other = (MotorStatus)____other;

			ret &= header == other.header;
			ret &= on == other.on;
			ret &= running = other.running;
			ret &= voltage.Equals ( other.voltage );
			ret &= frequency.Equals ( other.frequency );
			ret &= current.Equals ( other.current );
			return ret;
		}
	}
}