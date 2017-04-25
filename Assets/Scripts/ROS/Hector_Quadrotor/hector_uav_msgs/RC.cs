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
	public class RC : IRosMessage
	{
		public Header_t header;
		public uint8 status;
		public bool valid;
		public List<float> axis;
		public List<uint8> axis_function;
		public List<uint8> swit;
		public List<uint8> swit_function;

		public enum RCState : byte
		{
			Unknown = 0,
			Roll = 1,
			Pitch,
			Yaw,
			Steer,
			Height,
			Thrust,
			Brake
		};


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "2691c2fe8c5ab2323146bdd8dd2e449e"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
uint8 status
bool valid
float32[] axis
uint8[] axis_function
uint8[] swit
uint8[] swit_function"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__RC; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public RC()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public RC(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public RC(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			status = SERIALIZEDSTUFF [ currentIndex++ ];
			valid = SERIALIZEDSTUFF [ currentIndex++ ] != 0;
			// axis
			int length = System.BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			axis = new List<float> ( length );
			for (int i = 0; i < length; i++)
			{
				axis.Add ( System.BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex ) );
				currentIndex += 4;
			}
			// axis function
			length = System.BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			axis_function = new List<uint8> ( length );
			for ( int i = 0; i < length; i++ )
				axis_function.Add ( SERIALIZEDSTUFF [ currentIndex++ ] );
			// swit
			length = System.BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			axis_function = new List<uint8> ( length );
			for ( int i = 0; i < length; i++ )
				axis_function.Add ( SERIALIZEDSTUFF [ currentIndex++ ] );
			// swit function
			length = System.BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			axis_function = new List<uint8> ( length );
			for ( int i = 0; i < length; i++ )
				axis_function.Add ( SERIALIZEDSTUFF [ currentIndex++ ] );
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
			b.WriteByte ( status );
			b.WriteBool ( valid );
			b.WriteInt ( axis.Count );
			for ( int i = 0; i < axis.Count; i++ )
				b.WriteFloat ( axis [ i ] );
			b.WriteInt ( axis_function.Count );
			b.WriteBytes ( axis_function.ToArray () );
			b.WriteInt ( swit.Count );
			b.WriteBytes ( swit.ToArray () );
			b.WriteInt ( swit_function.Count );
			b.WriteBytes ( swit_function.ToArray () );

			return b.GetBytes ();
		}

		public override void Randomize()
		{
			header.Randomize ();
			int count = UnityEngine.Random.Range ( 0, 10 );
			// axis
			axis = new List<float> ( count );
			for ( int i = 0; i < count; i++ )
				axis [ i ] = UnityEngine.Random.value;
			// axis function
			axis_function = new List<byte> ( count );
			for ( int i = 0; i < count; i++ )
				axis_function [ i ] = (byte) UnityEngine.Random.Range ( 0, 255 );
			// swit
			swit = new List<byte> ( count );
			for ( int i = 0; i < count; i++ )
				swit [ i ] = (byte) UnityEngine.Random.Range ( 0, 255 );
			// swit function
			swit_function = new List<byte> ( count );
			for ( int i = 0; i < count; i++ )
				swit_function [ i ] = (byte) UnityEngine.Random.Range ( 0, 255 );
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			RC other = (RC)____other;

			ret &= header == other.header;
			ret &= status == other.status;
			ret &= valid == other.valid;
			ret &= axis.Equals ( other.axis );
			ret &= axis_function.Equals ( other.axis_function );
			ret &= swit.Equals ( other.swit );
			ret &= swit_function.Equals ( other.swit_function );
			return ret;
		}
	}
}