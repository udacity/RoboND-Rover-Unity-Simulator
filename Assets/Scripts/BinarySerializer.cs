#pragma warning disable 162
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BinarySerializer
{
	public int ByteCount { get { return byteList.Count; } }

	List<byte> byteList;
	byte[] byteArray;
	int currentPosition;
	int count;
	bool isWriting;

	public BinarySerializer ()
	{
		byteList = new List<byte> ( 200 );
		isWriting = true;
	}

	public BinarySerializer (byte[] fromBytes)
	{
		byteArray = fromBytes;
		isWriting = false;
	}

	~BinarySerializer ()
	{
		if ( byteList != null )
			byteList.Clear ();
		byteList = null;
		byteArray = null;
	}

	public byte[] GetBytes ()
	{
		return byteList.ToArray ();
	}

	public void WriteByte (byte b)
	{
		if ( !isWriting )
		{
		#if UNITY_EDITOR
			throw new UnauthorizedAccessException ( "BinarySerializer is set to Read." );
		#endif
			return;
		}
		byteList.Add ( b );
	}

	public void WriteByte (short s)
	{
		if ( !isWriting )
		{
			#if UNITY_EDITOR
			throw new UnauthorizedAccessException ( "BinarySerializer is set to Read." );
			#endif
			return;
		}
		byteList.Add ( (byte)s );
	}

	public void WriteByte (int n)
	{
		if ( !isWriting )
		{
			#if UNITY_EDITOR
			throw new UnauthorizedAccessException ( "BinarySerializer is set to Read." );
			#endif
			return;
		}
		byteList.Add ( (byte)n );
	}

	public void WriteBytes (byte[] bytesToWrite, int count = -1)
	{
		if ( !isWriting )
		{
			#if UNITY_EDITOR
			throw new UnauthorizedAccessException ( "BinarySerializer is set to Read." );
			#endif
			return;
		}
		if ( count == -1 )
			count = bytesToWrite.Length;
		if ( count == bytesToWrite.Length )
			byteList.AddRange ( bytesToWrite );
		else
		if ( count > 0 && count < bytesToWrite.Length )
		{
			byte[] newBytes = new byte[count];
			newBytes.CopyFrom ( bytesToWrite );
//			bytesToWrite.CopyTo ( newBytes, count );
			byteList.AddRange ( newBytes );
		} else
		{
			throw new ArgumentOutOfRangeException ( "Count parameter out of range for write. Bytes is " + bytesToWrite.Length + " and count is " + count );
		}
	}
	
	public void WriteBool (bool b)
	{
		WriteBytes ( BitConverter.GetBytes ( b ) );
	}

	public void WriteShort (short s)
	{
		WriteBytes ( BitConverter.GetBytes ( s ) );
	}

	public void WriteShort (int n)
	{
		WriteBytes ( BitConverter.GetBytes ( (short) n ) );
	}

	public void WriteInt (int n)
	{
		WriteBytes ( BitConverter.GetBytes ( n ) );
	}

	public void WriteFloat (float f)
	{
		WriteBytes ( BitConverter.GetBytes ( f ) );
	}

	public void WriteDouble (double d)
	{
		WriteBytes ( BitConverter.GetBytes ( d ) );
	}

	public void WriteVector2 (Vector2 v)
	{
		WriteBytes ( BitConverter.GetBytes ( v.x ) );
		WriteBytes ( BitConverter.GetBytes ( v.y ) );
	}

	public void WriteVector3 (Vector3 v)
	{
		WriteBytes ( BitConverter.GetBytes ( v.x ) );
		WriteBytes ( BitConverter.GetBytes ( v.y ) );
		WriteBytes ( BitConverter.GetBytes ( v.z ) );
	}

	public void WriteString (string s)
	{
		byte[] bytes = s.GetBytes ();
		#if UNITY_EDITOR
		if ( bytes.Length > short.MaxValue )
			Debug.LogException (new System.ArgumentOutOfRangeException ( "WriteString: string length exceeds short.MaxValue " + bytes.Length.ToString () ) );
		#endif
		WriteShort ( (short) bytes.Length );
		if ( bytes.Length > 0 )
			WriteBytes ( bytes );
	}

	public byte ReadByte ()
	{
		if ( isWriting )
		{
			#if UNITY_EDITOR
			throw new UnauthorizedAccessException ( "BinarySerializer is set to Write." );
			#endif
			return 0;
		}
		int pos = currentPosition;
		currentPosition++;
		return byteArray [ pos ];
	}

	public byte[] ReadBytes (int count)
	{
		if ( isWriting )
		{
			#if UNITY_EDITOR
			throw new UnauthorizedAccessException ( "BinarySerializer is set to Write." );
			#endif
			return new byte[ count ];
		}
		int pos = currentPosition;
		currentPosition += count;
		byte[] bytes = new byte[ count ];
		for ( int i = 0; i < count; i++ )
			bytes [ i ] = byteArray [ pos + i ];
		return bytes;
	}

	public bool ReadBool ()
	{
		return BitConverter.ToBoolean ( ReadBytes ( 1 ), 0 );
	}

	public short ReadShort ()
	{
		return BitConverter.ToInt16 ( ReadBytes ( 2 ), 0 );
	}

	public int ReadInt ()
	{
		return BitConverter.ToInt32 ( ReadBytes ( 4 ), 0 );
	}

	public float ReadFloat ()
	{
		return BitConverter.ToSingle ( ReadBytes ( 4 ), 0 );
	}

	public double ReadDouble ()
	{
		return BitConverter.ToDouble ( ReadBytes ( 8 ), 0 );
	}

	public Vector2 ReadVector2 ()
	{
		return new Vector2 (
			BitConverter.ToSingle ( ReadBytes ( 4 ), 0 ),
			BitConverter.ToSingle ( ReadBytes ( 4 ), 0 )
		);
	}

	public Vector3 ReadVector3 ()
	{
		return new Vector3 (
			BitConverter.ToSingle ( ReadBytes ( 4 ), 0 ),
			BitConverter.ToSingle ( ReadBytes ( 4 ), 0 ),
			BitConverter.ToSingle ( ReadBytes ( 4 ), 0 )
		);
	}

	public string ReadString ()
	{
		short length = ReadShort ();
		if ( length > 0 )
			return ReadBytes ( length ).GetString ();
		else
		if ( length == 0 )
			return "";
		else
		{
			#if UNITY_EDITOR
			Debug.LogException ( new ArgumentOutOfRangeException ( "ReadString: negative string length, probably wrote too long a string" ) );
			#endif
			return "";
		}
	}
}