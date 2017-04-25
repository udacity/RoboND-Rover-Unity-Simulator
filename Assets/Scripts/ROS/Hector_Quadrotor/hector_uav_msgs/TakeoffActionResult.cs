using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using uint8 = System.Byte;
using Messages.geometry_msgs;
using Messages.sensor_msgs;
using Messages.actionlib_msgs;
using Messages;
using Messages.std_msgs;
using String=System.String;
using hector_uav_msgs;

namespace hector_uav_msgs
{
	#if !TRACE
	[System.Diagnostics.DebuggerStepThrough]
	#endif
	public class TakeoffActionResult : IRosMessage
	{

		public Header header;
		public Messages.actionlib_msgs.GoalStatus status;
		public hector_uav_msgs.PoseResult result;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "1eb06eeff08fa7ea874431638cb52332"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"std_msgs/Header header
actionlib_msgs/GoalStatus status
PoseResult result"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__TakeoffActionResult; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public TakeoffActionResult()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public TakeoffActionResult(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public TakeoffActionResult(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			int arraylength=-1;
			bool hasmetacomponents = false;
			object __thing;
			int piecesize=0;
			byte[] thischunk, scratch1, scratch2;
			IntPtr h;

			//header
			header = new Header(SERIALIZEDSTUFF, ref currentIndex);
			//status
			status = new Messages.actionlib_msgs.GoalStatus(SERIALIZEDSTUFF, ref currentIndex);
			//result
			result = new hector_uav_msgs.PoseResult(SERIALIZEDSTUFF, ref currentIndex);
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			int currentIndex=0, length=0;
			bool hasmetacomponents = false;
			byte[] thischunk, scratch1, scratch2;
			List<byte[]> pieces = new List<byte[]>();
			GCHandle h;

			//header
			if (header == null)
				header = new Header();
			pieces.Add(header.Serialize(true));
			//status
			if (status == null)
				status = new Messages.actionlib_msgs.GoalStatus();
			pieces.Add(status.Serialize(true));
			//result
			if (result == null)
				result = new hector_uav_msgs.PoseResult();
			pieces.Add(result.Serialize(true));
			//combine every array in pieces into one array and return it
			int __a_b__f = pieces.Sum((__a_b__c)=>__a_b__c.Length);
			int __a_b__e=0;
			byte[] __a_b__d = new byte[__a_b__f];
			foreach(var __p__ in pieces)
			{
				Array.Copy(__p__,0,__a_b__d,__a_b__e,__p__.Length);
				__a_b__e += __p__.Length;
			}
			return __a_b__d;
		}

		public override void Randomize()
		{
			int arraylength=-1;
			Random rand = new Random();
			int strlength;
			byte[] strbuf, myByte;

			//header
			header = new Header();
			header.Randomize();
			//status
			status = new Messages.actionlib_msgs.GoalStatus();
			status.Randomize();
			//result
			result = new hector_uav_msgs.PoseResult();
			result.Randomize();
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			hector_uav_msgs.TakeoffActionResult other = (hector_uav_msgs.TakeoffActionResult)____other;

			ret &= header.Equals(other.header);
			ret &= status.Equals(other.status);
			ret &= result.Equals(other.result);
			// for each SingleType st:
			//    ret &= {st.Name} == other.{st.Name};
			return ret;
		}
	}
}
