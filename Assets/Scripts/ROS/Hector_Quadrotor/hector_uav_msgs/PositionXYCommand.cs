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
	public class PositionXYCommand : IRosMessage
	{
		public Header_t header;
		public float x;
		public float y;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "7b4d52af2aa98221d9bb260976d6a201"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
float32 x
float32 y"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__PositionXYCommand; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public PositionXYCommand()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public PositionXYCommand(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public PositionXYCommand(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			x = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += sizeof (float);
			y = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += sizeof (float);
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			int pos = 0;
			byte[] headerBytes = header.Serialize ();
			int headerSize = headerBytes.Length;
			byte[] bytes = new byte[headerSize + 2 * sizeof (float)];
			headerBytes.CopyTo ( bytes, 0 );
			pos += headerSize;
			BitConverter.GetBytes ( x ).CopyTo ( bytes, pos );
			pos += sizeof (float);
			BitConverter.GetBytes ( y ).CopyTo ( bytes, pos );
			pos += sizeof (float);

			return bytes;
		}

		public override void Randomize()
		{
			header.Randomize ();
			x = UnityEngine.Random.value;
			y = UnityEngine.Random.value;
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			PositionXYCommand other = (PositionXYCommand)____other;

			ret &= header == other.header;
			ret &= x == other.x;
			ret &= y == other.y;
			return ret;
		}
	}
}