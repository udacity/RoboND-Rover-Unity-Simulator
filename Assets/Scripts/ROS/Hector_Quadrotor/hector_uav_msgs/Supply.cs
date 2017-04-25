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
	public class Supply : IRosMessage
	{
		public Header_t header;
		public float[] voltage;
		public float[] current;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "26f5225a2b836fba706a87e45759fdfc"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
float32[] voltage
float32[] current"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__Supply; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public Supply()
		{
			voltage = new float[0];
			current = new float[0];
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public Supply(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public Supply(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			// voltage
			int length = BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			voltage = new float[length];
			for ( int i = 0; i < length; i++ )
			{
				voltage[i] = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
				currentIndex += 4;
			}
			// current
			length = BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			current = new float[length];
			for ( int i = 0; i < length; i++ )
			{
				current[i] = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
				currentIndex += 4;
			}
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			int pos = 0;
			byte[] headerBytes = header.Serialize ();
			int headerSize = headerBytes.Length;
			BinarySerializer b = new BinarySerializer ();
			b.WriteInt ( headerSize );
			b.WriteBytes ( headerBytes );
			b.WriteInt ( voltage.Length );
			for ( int i = 0; i < voltage.Length; i++ )
				b.WriteFloat ( voltage [ i ] );
			b.WriteInt ( current.Length );
			for ( int i = 0; i < current.Length; i++ )
				b.WriteFloat ( current [ i ] );

			return b.GetBytes ();
		}

		public override void Randomize()
		{
			header.Randomize ();
			voltage = new float[3];
			current = new float[3];
			for ( int i = 0; i < 3; i++ )
			{
				voltage [ i ] = UnityEngine.Random.value;
				current [ i ] = UnityEngine.Random.value;
			}
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			Supply other = (Supply)____other;

			ret &= header == other.header;
			ret &= voltage.Equals ( other.voltage );
			ret &= current.Equals ( other.current );
			return ret;
		}
	}
}