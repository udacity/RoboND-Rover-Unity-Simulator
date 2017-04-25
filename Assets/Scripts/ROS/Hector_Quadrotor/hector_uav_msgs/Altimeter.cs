using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Ros_CSharp;
using Messages;
using Header_t = Messages.std_msgs.Header;


namespace hector_uav_msgs
{
	#if !TRACE
	[System.Diagnostics.DebuggerStepThrough]
	#endif
	public class Altimeter : IRosMessage
	{
		public const float STANDARD_PRESSURE = 1013.25f;

		public Header_t header;
		public float altitude;
		public float pressure;
		public float qnh;

		public static float AltitudeFromPressure (float pressure, float qnh = Altimeter.STANDARD_PRESSURE)
		{
			return (float) ( 288.15 / 0.0065 * ( 1.0 - System.Math.Pow ( pressure / qnh, 1.0 / 5.255 ) ) );
		}

		public static float PressureFromAltitude (float altitude, float qnh = Altimeter.STANDARD_PRESSURE)
		{
			return (float) ( qnh * System.Math.Pow ( 1.0 - ( 0.0065 * altitude ) / 288.15, 5.255 ) );
		}

		public static Altimeter AltitudeFromPressure (Altimeter altimeter)
		{
			if ( altimeter.qnh == 0f )
				altimeter.qnh = STANDARD_PRESSURE;
			altimeter.altitude = AltitudeFromPressure ( altimeter.pressure, altimeter.qnh );
			return altimeter;
		}

		static Altimeter PressureFromAltitude (Altimeter altimeter)
		{
			if ( altimeter.qnh == 0f ) 
				altimeter.qnh = STANDARD_PRESSURE;
			altimeter.pressure = PressureFromAltitude ( altimeter.altitude, altimeter.qnh );
			return altimeter;
		}



		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "c785451e2f67a76b902818138e9b53c6"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"header header 
float32 altitude
float32 pressure
float32 qnh"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__Altimeter; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public Altimeter()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public Altimeter(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public Altimeter(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			header = new Header_t (SERIALIZEDSTUFF, ref currentIndex);
			altitude = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += sizeof (float);
			pressure = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
			currentIndex += sizeof (float);
			qnh = BitConverter.ToSingle ( SERIALIZEDSTUFF, currentIndex );
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
			byte[] bytes = new byte[headerSize + 3 * floatSize];
			headerBytes.CopyTo ( bytes, 0 );
			pos += headerSize;
			BitConverter.GetBytes ( altitude ).CopyTo ( bytes, pos );
			pos += floatSize;
			BitConverter.GetBytes ( pressure ).CopyTo ( bytes, pos );
			pos += floatSize;
			BitConverter.GetBytes ( qnh ).CopyTo ( bytes, pos );
			pos += floatSize;

			return bytes;
		}

		public override void Randomize()
		{
			header.Randomize ();
			altitude = UnityEngine.Random.Range ( float.MinValue, float.MaxValue );
			pressure = UnityEngine.Random.Range ( float.MinValue, float.MaxValue );
			qnh = UnityEngine.Random.Range ( float.MinValue, float.MaxValue );
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			bool ret = true;
			Altimeter other = (Altimeter)____other;

			ret &= header == other.header;
			ret &= altitude == other.altitude;
			ret &= pressure == other.pressure;
			ret &= qnh == other.qnh;
			return ret;
		}
	} // class Altimeter
} // namespace hector_uav_msgs

/*namespace Ros_CSharp
{
	namespace message_traits
	{
		public class IsFixedSize : hector_uav_msgs.Altimeter
	template <class ContainerAllocator>
	struct IsFixedSize< ::hector_uav_msgs::Altimeter_<ContainerAllocator> >
	  : FalseType
	  { };

	template <class ContainerAllocator>
	struct IsFixedSize< ::hector_uav_msgs::Altimeter_<ContainerAllocator> const>
	  : FalseType
	  { };

	template <class ContainerAllocator>
	struct IsMessage< ::hector_uav_msgs::Altimeter_<ContainerAllocator> >
	  : TrueType
	  { };

	template <class ContainerAllocator>
	struct IsMessage< ::hector_uav_msgs::Altimeter_<ContainerAllocator> const>
	  : TrueType
	  { };

	template <class ContainerAllocator>
	struct HasHeader< ::hector_uav_msgs::Altimeter_<ContainerAllocator> >
	  : TrueType
	  { };

	template <class ContainerAllocator>
	struct HasHeader< ::hector_uav_msgs::Altimeter_<ContainerAllocator> const>
	  : TrueType
	  { };


	template<class ContainerAllocator>
	struct MD5Sum< ::hector_uav_msgs::Altimeter_<ContainerAllocator> >
	{
	  static const char* value()
	  {
		return "c785451e2f67a76b902818138e9b53c6";
	  }

	  static const char* value(const ::hector_uav_msgs::Altimeter_<ContainerAllocator>&) { return value(); }
	  static const uint64_t static_value1 = 0xc785451e2f67a76bULL;
	  static const uint64_t static_value2 = 0x902818138e9b53c6ULL;
	};

	template<class ContainerAllocator>
	struct DataType< ::hector_uav_msgs::Altimeter_<ContainerAllocator> >
	{
	  static const char* value()
	  {
		return "hector_uav_msgs/Altimeter";
	  }

	  static const char* value(const ::hector_uav_msgs::Altimeter_<ContainerAllocator>&) { return value(); }
	};

	template<class ContainerAllocator>
	struct Definition< ::hector_uav_msgs::Altimeter_<ContainerAllocator> >
	{
	  static const char* value()
	  {
		return "Header header\n\
	float32 altitude\n\
	float32 pressure\n\
	float32 qnh\n\
	\n\
	================================================================================\n\
	MSG: std_msgs/Header\n\
	# Standard metadata for higher-level stamped data types.\n\
	# This is generally used to communicate timestamped data \n\
	# in a particular coordinate frame.\n\
	# \n\
	# sequence ID: consecutively increasing ID \n\
	uint32 seq\n\
	#Two-integer timestamp that is expressed as:\n\
	# * stamp.sec: seconds (stamp_secs) since epoch (in Python the variable is called 'secs')\n\
	# * stamp.nsec: nanoseconds since stamp_secs (in Python the variable is called 'nsecs')\n\
	# time-handling sugar is provided by the client library\n\
	time stamp\n\
	#Frame this data is associated with\n\
	# 0: no frame\n\
	# 1: global frame\n\
	string frame_id\n\
	";
	  }

	  static const char* value(const ::hector_uav_msgs::Altimeter_<ContainerAllocator>&) { return value(); }
	};

	} // namespace message_traits
}*/ // namespace Ros_CSharp

/*namespace Ros_CSharp
{
	namespace serialization
	{

	  template<class ContainerAllocator> struct Serializer< ::hector_uav_msgs::Altimeter_<ContainerAllocator> >
	  {
		template<typename Stream, typename T> inline static void allInOne(Stream& stream, T m)
		{
		  stream.next(m.header);
		  stream.next(m.altitude);
		  stream.next(m.pressure);
		  stream.next(m.qnh);
		}

		ROS_DECLARE_ALLINONE_SERIALIZER
	  }; // struct Altimeter_

	} // namespace serialization
}*/ // namespace Ros_CSharp

/*namespace Ros_CSharp
{
	namespace message_operations
	{

	template<class ContainerAllocator>
	struct Printer< ::hector_uav_msgs::Altimeter_<ContainerAllocator> >
	{
	  template<typename Stream> static void stream(Stream& s, const std::string& indent, const ::hector_uav_msgs::Altimeter_<ContainerAllocator>& v)
	  {
		s << indent << "header: ";
		s << std::endl;
		Printer< ::std_msgs::Header_<ContainerAllocator> >::stream(s, indent + "  ", v.header);
		s << indent << "altitude: ";
		Printer<float>::stream(s, indent + "  ", v.altitude);
		s << indent << "pressure: ";
		Printer<float>::stream(s, indent + "  ", v.pressure);
		s << indent << "qnh: ";
		Printer<float>::stream(s, indent + "  ", v.qnh);
	  }
	};

	} // namespace message_operations
}*/ // namespace Ros_CSharp