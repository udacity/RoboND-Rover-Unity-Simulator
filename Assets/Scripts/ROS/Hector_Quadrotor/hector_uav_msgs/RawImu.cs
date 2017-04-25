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
	public class RawImu : IRosMessage
	{
		public Header_t header;
		short[] angular_velocity = new short[3];
		short[] linear_acceleration = new short[3];




		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "398f651a68070a719c7938171d0fcc45"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
int16[3] angular_velocity
int16[3] linear_acceleration"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__RawImu; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public RawImu()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public RawImu(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public RawImu(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			for (int i = 0; i < 3; i++)
			{
				angular_velocity [ i ] = BitConverter.ToInt16 ( SERIALIZEDSTUFF, currentIndex );
				currentIndex += 2;
			}
			for (int i = 0; i < 3; i++)
			{
				linear_acceleration [ i ] = BitConverter.ToInt16 ( SERIALIZEDSTUFF, currentIndex );
				currentIndex += 2;
			}
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			int pos = 0;
			byte[] headerBytes = header.Serialize ();
			int headerSize = headerBytes.Length;
			byte[] bytes = new byte[headerSize + 12];
			headerBytes.CopyTo ( bytes, 0 );
			pos += headerSize;
			for (int i = 0; i < 3; i++)
			{
				BitConverter.GetBytes ( angular_velocity [ i ] ).CopyTo ( bytes, pos );
				pos += 2;
			}
			for (int i = 0; i < 3; i++)
			{
				BitConverter.GetBytes ( linear_acceleration [ i ] ).CopyTo ( bytes, pos );
				pos += 2;
			}

			return bytes;
		}

		public override void Randomize()
		{
			header.Randomize ();
			System.Random r = new System.Random ();
			for ( int i = 0; i < 3; i++ )
			{
				angular_velocity [ i ] = (short) r.Next ( short.MaxValue );
				linear_acceleration [ i ] = (short) r.Next ( short.MaxValue );
			}
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			RawImu other = (RawImu)____other;

			ret &= header == other.header;
			ret &= angular_velocity.Equals ( other.angular_velocity );
			ret &= linear_acceleration.Equals ( other.linear_acceleration );
			return ret;
		}
	}
}