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
	public class RuddersCommand : IRosMessage
	{
		public Header_t header;
		public float aileron;
		public float elevator;
		public float rudder;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "2e136cb8cfffc2233e404b320c27bca6"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
float32 aileron
float32 elevator
float32 rudder"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__RuddersCommand; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public RuddersCommand()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public RuddersCommand(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public RuddersCommand(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			aileron = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			elevator = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
			rudder = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += 4;
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
			BitConverter.GetBytes ( aileron ).CopyTo ( bytes, pos );
			pos += 4;
			BitConverter.GetBytes ( elevator ).CopyTo ( bytes, pos );
			pos += 4;
			BitConverter.GetBytes ( rudder ).CopyTo ( bytes, pos );
			pos += 4;

			return bytes;
		}

		public override void Randomize()
		{
			header.Randomize ();
			aileron = UnityEngine.Random.value;
			elevator = UnityEngine.Random.value;
			rudder = UnityEngine.Random.value;
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			RuddersCommand other = (RuddersCommand)____other;

			ret &= header == other.header;
			ret &= aileron == other.aileron;
			ret &= elevator == other.elevator;
			ret &= rudder == other.rudder;
			return ret;
		}
	}
}