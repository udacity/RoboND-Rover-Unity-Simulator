//=================================================================================================
// Copyright (c) 2012-2016, Institute of Flight Systems and Automatic Control,
// Technische Universität Darmstadt.
// All rights reserved.

// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of hector_quadrotor nor the names of its contributors
//       may be used to endorse or promote products derived from this software
//       without specific prior written permission.

// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//=================================================================================================
using System;
using System.Collections;
using UnityEngine;
using hector_uav_msgs;

namespace hector_uav_msgs
{
	public static class Functions
	{
		public static string getFunctionString (byte function)
		{
			RC.RCState st = (RC.RCState) function;
			return st.ToString ();
		}

		public static bool hasAxis (RC rc, byte function)
		{
			return rc.axis_function.Contains ( function );
		}

		public static bool getAxis (RC rc, byte function, out float value)
		{
			value = 0;
			if ( !rc.valid )
				return false;
			int index = rc.axis_function.IndexOf ( function );
			if ( index == -1 )
				return false;
			value = rc.axis [ index ];
			return true;
		}

		public static void setAxis (RC rc, byte function, float value)
		{
			rc.axis_function.Add ( function );
			rc.axis.Add ( value );
		}

		public static bool hasSwitch (RC rc, byte function)
		{
			return rc.swit_function.Contains ( function );
		}

		public static bool getSwitch (RC rc, byte function, out byte value)
		{
			value = (byte) 0;
			if ( !rc.valid )
				return false;
			int index = rc.swit_function.IndexOf ( function );
			if ( index == -1 )
				return false;
			value = rc.swit [ index ];
			return true;
		}

		public static void setSwitch (RC rc, byte function, byte value)
		{
			rc.swit_function.Add ( function );
			rc.swit.Add ( value );
		}
	}
} // namespace hector_uav_msgs