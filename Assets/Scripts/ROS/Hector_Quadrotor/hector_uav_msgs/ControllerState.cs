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
	public class ControllerState : IRosMessage
	{
		public Header_t header;
		public uint8 source;
		public uint8 mode;
		public uint8 state;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "cf55b8af1d9e1de941887ee78e23079c"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
uint8 source
uint8 mode
uint8 state"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__ControllerState; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public ControllerState()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ControllerState(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ControllerState(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			source = SERIALIZEDSTUFF [ currentIndex++ ];
			mode = SERIALIZEDSTUFF [ currentIndex++ ];
			state = SERIALIZEDSTUFF [ currentIndex++ ];
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			int pos = 0;
			byte[] headerBytes = header.Serialize ();
			int headerSize = headerBytes.Length;
			int byteSize = sizeof (uint8);
			byte[] bytes = new byte[headerSize + 3];
			headerBytes.CopyTo ( bytes, 0 );
			pos += headerSize;
			bytes [ pos++ ] = source;
			bytes [ pos++ ] = mode;
			bytes [ pos++ ] = state;

			return bytes;
		}

		public override void Randomize()
		{
			header.Randomize ();
			source = (uint8) UnityEngine.Random.Range ( 0, 255 );
			mode = (uint8) UnityEngine.Random.Range ( 0, 255 );
			state = (uint8) UnityEngine.Random.Range ( 0, 255 );
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			ControllerState other = (ControllerState)____other;

			ret &= header == other.header;
			ret &= source == other.source;
			ret &= mode == other.mode;
			ret &= state == other.state;
			return ret;
		}
	}
}