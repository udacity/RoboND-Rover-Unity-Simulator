using System;
using System.Collections.Generic;
using uint8 = System.Byte;
using Messages;
using hector_uav_msgs;
using System.Linq;

namespace hector_uav_msgs
{
	#if !TRACE
	[System.Diagnostics.DebuggerStepThrough]
	#endif
	public class TakeoffAction : IRosMessage
	{
		public hector_uav_msgs.TakeoffActionGoal action_goal;
		public hector_uav_msgs.TakeoffActionResult action_result;
		public hector_uav_msgs.TakeoffActionFeedback action_feedback;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "a3b6083cae7da419ac7a4cb028a39fb6"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"hector_uav_msgs/TakeoffActionGoal action_goal
hector_uav_msgs/TakeoffActionResult action_result
hector_uav_msgs/TakeoffActionFeedback action_feedback"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__TakeoffAction; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public TakeoffAction()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public TakeoffAction(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public TakeoffAction(byte[] SERIALIZEDSTUFF, ref int currentIndex)
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

			//action_goal
			action_goal = new hector_uav_msgs.TakeoffActionGoal(SERIALIZEDSTUFF, ref currentIndex);
			//action_result
			action_result = new hector_uav_msgs.TakeoffActionResult(SERIALIZEDSTUFF, ref currentIndex);
			//action_feedback
			action_feedback = new hector_uav_msgs.TakeoffActionFeedback(SERIALIZEDSTUFF, ref currentIndex);
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			int currentIndex=0, length=0;
			bool hasmetacomponents = false;
			byte[] thischunk, scratch1, scratch2;
			List<byte[]> pieces = new List<byte[]>();

			//action_goal
			if (action_goal == null)
				action_goal = new hector_uav_msgs.TakeoffActionGoal();
			pieces.Add(action_goal.Serialize(true));
			//action_result
			if (action_result == null)
				action_result = new hector_uav_msgs.TakeoffActionResult();
			pieces.Add(action_result.Serialize(true));
			//action_feedback
			if (action_feedback == null)
				action_feedback = new hector_uav_msgs.TakeoffActionFeedback();
			pieces.Add(action_feedback.Serialize(true));
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

			//action_goal
			action_goal = new hector_uav_msgs.TakeoffActionGoal();
			action_goal.Randomize();
			//action_result
			action_result = new hector_uav_msgs.TakeoffActionResult();
			action_result.Randomize();
			//action_feedback
			action_feedback = new hector_uav_msgs.TakeoffActionFeedback();
			action_feedback.Randomize();
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			hector_uav_msgs.TakeoffAction other = (hector_uav_msgs.TakeoffAction)____other;

			ret &= action_goal.Equals(other.action_goal);
			ret &= action_result.Equals(other.action_result);
			ret &= action_feedback.Equals(other.action_feedback);
			// for each SingleType st:
			//    ret &= {st.Name} == other.{st.Name};
			return ret;
		}
	}
}
