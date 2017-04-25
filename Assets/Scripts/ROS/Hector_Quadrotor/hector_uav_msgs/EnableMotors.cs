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
	public class EnableMotors : IRosService
	{
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override SrvTypes srvtype() { return SrvTypes.hector_uav_msgs__EnableMotors; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string ServiceDefinition() { return @"bool data
---
bool success
string message"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "09fb03525b03e7ea1fd3992bafd87e16"; }

		[System.Diagnostics.DebuggerStepThrough]
		public EnableMotors()
		{
			InitSubtypes(new Request(), new Response());
		}

		public Response Invoke(Func<Request, Response> fn, Request req)
		{
			RosServiceDelegate rsd = (m)=>{
				Request r = m as Request;
				if (r == null)
					throw new Exception("Invalid Service Request Type");
				return fn(r);
			};
			return (Response)GeneralInvoke(rsd, (IRosMessage)req);
		}

		public Request req { get { return (Request)RequestMessage; } set { RequestMessage = (IRosMessage)value; } }
		public Response resp { get { return (Response)ResponseMessage; } set { ResponseMessage = (IRosMessage)value; } }

		public class Request : IRosMessage
		{
			public bool enable;

			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override string MD5Sum() { return "8c1211af706069c994c06e00eb59ac9e"; }
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override bool HasHeader() { return false; }
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override bool IsMetaType() { return false; }
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override string MessageDefinition() { return @"bool enable"; }
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__EnableMotors__Request; }
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override bool IsServiceComponent() { return true; }

			[System.Diagnostics.DebuggerStepThrough]
			public Request()
			{

			}

			[System.Diagnostics.DebuggerStepThrough]
			public Request(byte[] SERIALIZEDSTUFF)
			{
				Deserialize(SERIALIZEDSTUFF);
			}

			[System.Diagnostics.DebuggerStepThrough]
			public Request(byte[] SERIALIZEDSTUFF, ref int currentIndex)
			{
				Deserialize(SERIALIZEDSTUFF, ref currentIndex);
			}



			public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
			{
				enable = SERIALIZEDSTUFF [ currentIndex++ ] != 0;
			}

			[System.Diagnostics.DebuggerStepThrough]
			public override byte[] Serialize(bool partofsomethingelse)
			{
				return new byte[1] { enable ? (byte)1 : (byte)0 };
			}

			public override void Randomize()
			{
				enable = UnityEngine.Random.value > 0.5f;
			}

			public override bool Equals(IRosMessage ____other)
			{
				if (____other == null) return false;
				bool ret = true;
				hector_uav_msgs.EnableMotors.Request other = (hector_uav_msgs.EnableMotors.Request)____other;

				ret &= enable == other.enable;
				return ret;
			}
		}

		public class Response : IRosMessage
		{
			public bool success;


			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override string MD5Sum() { return "358e233cde0c8a8bcfea4ce193f8fc15"; }
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override bool HasHeader() { return false; }
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override bool IsMetaType() { return false; }
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override string MessageDefinition() { return @"bool success"; }
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__EnableMotors__Response; }
			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public override bool IsServiceComponent() { return true; }

			[System.Diagnostics.DebuggerStepThrough]
			public Response()
			{

			}

			[System.Diagnostics.DebuggerStepThrough]
			public Response(byte[] SERIALIZEDSTUFF)
			{
				Deserialize(SERIALIZEDSTUFF);
			}
			[System.Diagnostics.DebuggerStepThrough]
			public Response(byte[] SERIALIZEDSTUFF, ref int currentIndex)
			{
				Deserialize(SERIALIZEDSTUFF, ref currentIndex);
			}



			//[System.Diagnostics.DebuggerStepThrough]
			public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
			{
				success = SERIALIZEDSTUFF [ currentIndex++ ] != 0;
			}

			[System.Diagnostics.DebuggerStepThrough]
			public override byte[] Serialize(bool partofsomethingelse)
			{
				return new byte[1] { success ? (byte)1 : (byte)0 };
			}

			public override void Randomize()
			{
				success = UnityEngine.Random.value > 0.5f;
			}

			public override bool Equals(IRosMessage ____other)
			{
				if (____other == null) return false;
				bool ret = true;
				hector_uav_msgs.EnableMotors.Response other = (hector_uav_msgs.EnableMotors.Response)____other;

				ret &= success == other.success;
				return ret;
			}
		}
	}
}