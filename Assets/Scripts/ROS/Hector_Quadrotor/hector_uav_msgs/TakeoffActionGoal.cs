using System;
using System.Collections.Generic;
using uint8 = System.Byte;
using Messages;
using hector_uav_msgs;

namespace hector_uav_msgs
{
	#if !TRACE
	[System.Diagnostics.DebuggerStepThrough]
	#endif
	public class TakeoffActionGoal : IRosMessage
	{
		public Messages.std_msgs.Header header;
		public Messages.actionlib_msgs.GoalID goal_id;
		public hector_uav_msgs.TakeoffGoal goal;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "4b30be6cd12b9e72826df56b481f40e0"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"Header header
actionlib_msgs/GoalID goal_id
hector_uav_msgs/TakeoffGoal goal"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__TakeoffActionGoal; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public TakeoffActionGoal()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public TakeoffActionGoal(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public TakeoffActionGoal(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Messages.std_msgs.Header ( SERIALIZEDSTUFF, ref currentIndex );
			goal_id = new Messages.actionlib_msgs.GoalID ( SERIALIZEDSTUFF, ref currentIndex );
			goal = new TakeoffGoal ( SERIALIZEDSTUFF, ref currentIndex );
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			int pos = 0;
			byte[] headerBytes = header.Serialize ();
			byte[] goalIDBytes = goal_id.Serialize ();
			byte[] goalBytes = goal.Serialize ();

			byte[] bytes = new byte[ headerBytes.Length + goalBytes.Length + goalIDBytes.Length ];
			headerBytes.CopyTo ( bytes, 0 );
			pos = headerBytes.Length;
			goalIDBytes.CopyTo ( bytes, pos );
			pos += goalIDBytes.Length;
			goalBytes.CopyTo ( bytes, pos );

			return bytes;
		}

		public override void Randomize()
		{
			header = new Messages.std_msgs.Header ();
			header.Randomize ();
			goal_id = new Messages.actionlib_msgs.GoalID ();
			goal_id.Randomize ();
			goal = new TakeoffGoal ();
			goal.Randomize ();
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			hector_uav_msgs.TakeoffActionGoal other = (hector_uav_msgs.TakeoffActionGoal)____other;

			ret &= goal.Equals(other.goal);
			ret &= goal_id.Equals(other.goal_id);
			return ret;
		}
	}
}
