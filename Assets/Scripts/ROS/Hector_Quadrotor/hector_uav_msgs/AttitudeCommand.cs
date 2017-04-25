using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Ros_CSharp;
using Messages;
using Header_t = Messages.std_msgs.Header;

namespace hector_uav_msgs
{
	public class AttitudeCommand : IRosMessage
	{
		public Header_t header;
		public float roll;
		public float pitch;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "cceacd88dad80f3e3fd1466d24264ec6"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
float32 roll
float32 pitch"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__Attitudecommand; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public AttitudeCommand()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public AttitudeCommand(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public AttitudeCommand(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			roll = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += sizeof (float);
			pitch = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += sizeof (float);
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			int pos = 0;
			byte[] headerBytes = header.Serialize ();
			int headerSize = headerBytes.Length;
			int floatSize = sizeof (float);
			byte[] bytes = new byte[headerSize + 2 * floatSize];
			headerBytes.CopyTo ( bytes, 0 );
			pos += headerSize;
			BitConverter.GetBytes ( roll ).CopyTo ( bytes, pos );
			pos += floatSize;
			BitConverter.GetBytes ( pitch ).CopyTo ( bytes, pos );
			pos += floatSize;

			return bytes;
		}

		public override void Randomize()
		{
			header.Randomize ();
			roll = UnityEngine.Random.Range ( float.MinValue, float.MaxValue );
			pitch = UnityEngine.Random.Range ( float.MinValue, float.MaxValue );
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			AttitudeCommand other = (AttitudeCommand)____other;

			ret &= header == other.header;
			ret &= roll == other.roll;
			ret &= pitch == other.pitch;
			return ret;
		}
	}
}