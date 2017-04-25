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
	public class RawMagnetic : IRosMessage
	{
		public Header_t header;
		public double[] channel = new double[3];


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "babd510868ac7b486e2097c79e1384c9"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
float64[3] channel"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__RawMagnetic; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public RawMagnetic()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public RawMagnetic(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public RawMagnetic(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			for ( int i = 0; i < 3; i++ )
			{
				channel [ i ] = BitConverter.ToDouble ( SERIALIZEDSTUFF, currentIndex );
				currentIndex += 8;
			}
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			int pos = 0;
			byte[] headerBytes = header.Serialize ();
			int headerSize = headerBytes.Length;
			byte[] bytes = new byte[headerSize + 24];
			headerBytes.CopyTo ( bytes, 0 );
			pos += headerSize;
			for ( int i = 0; i < 3; i++ )
			{
				BitConverter.GetBytes ( channel [ i ] ).CopyTo ( bytes, pos );
				pos += 8;
			}

			return bytes;
		}

		public override void Randomize()
		{
			header.Randomize ();
			System.Random r = new System.Random ();
			for ( int i = 0; i < 3; i++ )
				channel [ i ] = r.NextDouble ();
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			RawMagnetic other = (RawMagnetic)____other;

			ret &= header == other.header;
			ret &= channel.Equals ( other.channel );
			return ret;
		}
	}
}