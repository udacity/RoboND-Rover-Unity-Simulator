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
	public class HeadingCommand : IRosMessage
	{
		public Header_t header;
		public float heading;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "bbd082d3b4bc79a5314bb6f95aaedc70"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
float32 heading"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__HeadingCommand; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public HeadingCommand()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public HeadingCommand(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public HeadingCommand(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			heading = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += sizeof (float);
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			int pos = 0;
			byte[] headerBytes = header.Serialize ();
			int headerSize = headerBytes.Length;
			int byteSize = sizeof (uint8);
			byte[] bytes = new byte[headerSize + sizeof (float)];
			headerBytes.CopyTo ( bytes, 0 );
			pos += headerSize;
			BitConverter.GetBytes ( heading ).CopyTo ( bytes, pos );
			pos += sizeof (float);

			return bytes;
		}

		public override void Randomize()
		{
			header.Randomize ();
			heading = UnityEngine.Random.value;
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			HeadingCommand other = (HeadingCommand)____other;

			ret &= header == other.header;
			ret &= heading == other.heading;
			return ret;
		}
	}
}