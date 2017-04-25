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
	public class RawRC : IRosMessage
	{
		public Header_t header;
		uint8 status;
		ushort[] channel;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "f1584488325f747abea0b77036f70e2c"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
uint8 status
uint16[] channel"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__RawRC; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public RawRC()
		{
			channel = new ushort[0];
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public RawRC(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public RawRC(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			status = SERIALIZEDSTUFF [ currentIndex++ ];
			int length = BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			channel = new ushort[length];
			for (int i = 0; i < length; i++)
			{
				channel [ i ] = BitConverter.ToUInt16 ( SERIALIZEDSTUFF, currentIndex );
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
			int byteSize = sizeof (uint8);
			byte[] bytes = new byte[headerSize + 5 + channel.Length];
			headerBytes.CopyTo ( bytes, 0 );
			pos += headerSize;
			bytes [ pos++ ] = status;
			int length = channel.Length;
			BitConverter.GetBytes ( length ).CopyTo ( bytes, pos );
			pos += 4;
			for ( int i = 0; i < length; i++ )
			{
				BitConverter.GetBytes ( channel [ i ] ).CopyTo ( bytes, pos );
				pos += 2;
			}

			return bytes;
		}

		public override void Randomize()
		{
			header.Randomize ();
			status = (byte) UnityEngine.Random.Range ( 0, 255 );
			channel = new ushort[3];
			for ( int i = 0; i < 3; i++ )
				channel [ i ] = (ushort) UnityEngine.Random.Range ( 0, ushort.MaxValue );
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			RawRC other = (RawRC)____other;

			ret &= header == other.header;
			ret &= status.Equals ( other.status );
			ret &= channel.Equals ( other.channel );
			return ret;
		}
	}
}