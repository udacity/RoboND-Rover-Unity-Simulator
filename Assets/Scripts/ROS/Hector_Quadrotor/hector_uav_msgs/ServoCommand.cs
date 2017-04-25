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
	public class ServoCommand : IRosMessage
	{
		public Header_t header;
		public ushort[] value;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "d60ef35d4e3412dc6686b189be2523d0"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
uint16[] value"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__ServoCommand; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public ServoCommand()
		{
			value = new ushort[0];
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ServoCommand(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ServoCommand(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			int length = BitConverter.ToInt32 ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			value = new ushort[length];
			for ( int i = 0; i < length; i++ )
			{
				value [ i ] = BitConverter.ToUInt16 ( SERIALIZEDSTUFF, currentIndex );
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
			byte[] bytes = new byte[headerSize + 4 + value.Length];
			headerBytes.CopyTo ( bytes, 0 );
			pos += headerSize;
			BitConverter.GetBytes ( value.Length ).CopyTo ( bytes, pos );
			pos += 4;
			for (int i = 0; i < value.Length; i++)
			{
				BitConverter.GetBytes ( value [ i ] ).CopyTo ( bytes, pos );
				pos += 2;
			}

			return bytes;
		}

		public override void Randomize()
		{
			header.Randomize ();
			value = new ushort[3];
			for ( int i = 0; i < 3; i++ )
				value [ i ] = (ushort) UnityEngine.Random.Range ( 0, ushort.MaxValue );
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			ServoCommand other = (ServoCommand)____other;

			ret &= header == other.header;
			ret &= value.Equals ( other.value );
			return ret;
		}
	}
}