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
	public class LandingGoal : IRosMessage
	{
		public string target_frame;
		public string source_frame;
		public Time source_time;
		public Duration timeout;
		public Time target_time;
		public string fixed_frame;
		public bool advanced;


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "9a975e3289dd0b8ab52f7f62f182cd2a"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"string target_frame
string source_frame
time source_time
duration timeout
time target_time
string fixed_frame
bool advanced"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__LandingGoal; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public LandingGoal()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public LandingGoal(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public LandingGoal(byte[] SERIALIZEDSTUFF, ref int currentIndex)
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

			//target_frame
			target_frame = "";
			piecesize = BitConverter.ToInt32(SERIALIZEDSTUFF, currentIndex);
			currentIndex += 4;
			target_frame = Encoding.ASCII.GetString(SERIALIZEDSTUFF, currentIndex, piecesize);
			currentIndex += piecesize;
			//source_frame
			source_frame = "";
			piecesize = BitConverter.ToInt32(SERIALIZEDSTUFF, currentIndex);
			currentIndex += 4;
			source_frame = Encoding.ASCII.GetString(SERIALIZEDSTUFF, currentIndex, piecesize);
			currentIndex += piecesize;
			//source_time
			source_time = new Time(new TimeData(
				BitConverter.ToUInt32(SERIALIZEDSTUFF, currentIndex),
				BitConverter.ToUInt32(SERIALIZEDSTUFF, currentIndex+Marshal.SizeOf(typeof(System.Int32)))));
			currentIndex += 2*Marshal.SizeOf(typeof(System.Int32));
			//timeout
			timeout = new Duration(new TimeData(
				BitConverter.ToUInt32(SERIALIZEDSTUFF, currentIndex),
				BitConverter.ToUInt32(SERIALIZEDSTUFF, currentIndex+Marshal.SizeOf(typeof(System.Int32)))));
			currentIndex += 2*Marshal.SizeOf(typeof(System.Int32));
			//target_time
			target_time = new Time(new TimeData(
				BitConverter.ToUInt32(SERIALIZEDSTUFF, currentIndex),
				BitConverter.ToUInt32(SERIALIZEDSTUFF, currentIndex+Marshal.SizeOf(typeof(System.Int32)))));
			currentIndex += 2*Marshal.SizeOf(typeof(System.Int32));
			//fixed_frame
			fixed_frame = "";
			piecesize = BitConverter.ToInt32(SERIALIZEDSTUFF, currentIndex);
			currentIndex += 4;
			fixed_frame = Encoding.ASCII.GetString(SERIALIZEDSTUFF, currentIndex, piecesize);
			currentIndex += piecesize;
			//advanced
			advanced = SERIALIZEDSTUFF[currentIndex++]==1;
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

			//target_frame
			if (target_frame == null)
				target_frame = "";
			scratch1 = Encoding.ASCII.GetBytes((string)target_frame);
			thischunk = new byte[scratch1.Length + 4];
			scratch2 = BitConverter.GetBytes(scratch1.Length);
			Array.Copy(scratch1, 0, thischunk, 4, scratch1.Length);
			Array.Copy(scratch2, thischunk, 4);
			pieces.Add(thischunk);
			//source_frame
			if (source_frame == null)
				source_frame = "";
			scratch1 = Encoding.ASCII.GetBytes((string)source_frame);
			thischunk = new byte[scratch1.Length + 4];
			scratch2 = BitConverter.GetBytes(scratch1.Length);
			Array.Copy(scratch1, 0, thischunk, 4, scratch1.Length);
			Array.Copy(scratch2, thischunk, 4);
			pieces.Add(thischunk);
			//source_time
			pieces.Add(BitConverter.GetBytes(source_time.data.sec));
			pieces.Add(BitConverter.GetBytes(source_time.data.nsec));
			//timeout
			pieces.Add(BitConverter.GetBytes(timeout.data.sec));
			pieces.Add(BitConverter.GetBytes(timeout.data.nsec));
			//target_time
			pieces.Add(BitConverter.GetBytes(target_time.data.sec));
			pieces.Add(BitConverter.GetBytes(target_time.data.nsec));
			//fixed_frame
			if (fixed_frame == null)
				fixed_frame = "";
			scratch1 = Encoding.ASCII.GetBytes((string)fixed_frame);
			thischunk = new byte[scratch1.Length + 4];
			scratch2 = BitConverter.GetBytes(scratch1.Length);
			Array.Copy(scratch1, 0, thischunk, 4, scratch1.Length);
			Array.Copy(scratch2, thischunk, 4);
			pieces.Add(thischunk);
			//advanced
			thischunk = new byte[1];
			thischunk[0] = (byte) ((bool)advanced ? 1 : 0 );
			pieces.Add(thischunk);
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

			//target_frame
			strlength = rand.Next(100) + 1;
			strbuf = new byte[strlength];
			rand.NextBytes(strbuf);  //fill the whole buffer with random bytes
			for (int __x__ = 0; __x__ < strlength; __x__++)
				if (strbuf[__x__] == 0) //replace null chars with non-null random ones
					strbuf[__x__] = (byte)(rand.Next(254) + 1);
			strbuf[strlength - 1] = 0; //null terminate
			target_frame = Encoding.ASCII.GetString(strbuf);
			//source_frame
			strlength = rand.Next(100) + 1;
			strbuf = new byte[strlength];
			rand.NextBytes(strbuf);  //fill the whole buffer with random bytes
			for (int __x__ = 0; __x__ < strlength; __x__++)
				if (strbuf[__x__] == 0) //replace null chars with non-null random ones
					strbuf[__x__] = (byte)(rand.Next(254) + 1);
			strbuf[strlength - 1] = 0; //null terminate
			source_frame = Encoding.ASCII.GetString(strbuf);
			//source_time
			source_time = new Time(new TimeData(
				Convert.ToUInt32(rand.Next()),
				Convert.ToUInt32(rand.Next())));
			//timeout
			timeout = new Duration(new TimeData(
				Convert.ToUInt32(rand.Next()),
				Convert.ToUInt32(rand.Next())));
			//target_time
			target_time = new Time(new TimeData(
				Convert.ToUInt32(rand.Next()),
				Convert.ToUInt32(rand.Next())));
			//fixed_frame
			strlength = rand.Next(100) + 1;
			strbuf = new byte[strlength];
			rand.NextBytes(strbuf);  //fill the whole buffer with random bytes
			for (int __x__ = 0; __x__ < strlength; __x__++)
				if (strbuf[__x__] == 0) //replace null chars with non-null random ones
					strbuf[__x__] = (byte)(rand.Next(254) + 1);
			strbuf[strlength - 1] = 0; //null terminate
			fixed_frame = Encoding.ASCII.GetString(strbuf);
			//advanced
			advanced = rand.Next(2) == 1;
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			hector_uav_msgs.LandingGoal other = (hector_uav_msgs.LandingGoal)____other;

			ret &= target_frame == other.target_frame;
			ret &= source_frame == other.source_frame;
			ret &= source_time.data.Equals(other.source_time.data);
			ret &= timeout.data.Equals(other.timeout.data);
			ret &= target_time.data.Equals(other.target_time.data);
			ret &= fixed_frame == other.fixed_frame;
			ret &= advanced == other.advanced;
			// for each SingleType st:
			//    ret &= {st.Name} == other.{st.Name};
			return ret;
		}
	}
}
