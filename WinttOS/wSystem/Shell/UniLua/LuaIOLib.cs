
// TODO

using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;

namespace UniLua
{
	internal class FileStreamManager
	{
		private Dictionary<int, FileStream> fileStreams = new();
		private int nextId = 1;

		public int AddFileStream(FileStream stream)
		{
			int id = nextId++;
			fileStreams[id] = stream;
			return id;
		}

		public FileStream GetFileStream(int id)
		{
			fileStreams.TryGetValue(id, out var stream);
			return stream;
		}

		public bool RemoveFileStream(int id)
		{
			return fileStreams.Remove(id);
		}
	}
	internal class  LuaIOLib
	{
		public const string LIB_NAME = "io";

		public static FileStreamManager streamManager;

        public static int OpenLib( ILuaState lua )
		{
			NameFuncPair[] define = new NameFuncPair[]
			{
				new NameFuncPair( "close", 		IO_Close ),
				new NameFuncPair( "flush", 		IO_Flush ),
				new NameFuncPair( "input", 		IO_Input ),
				new NameFuncPair( "lines", 		IO_Lines ),
				new NameFuncPair( "open", 		IO_Open ),
				new NameFuncPair( "output", 	IO_Output ),
				new NameFuncPair( "popen", 		IO_Popen ),
				new NameFuncPair( "read", 		IO_Read ),
				new NameFuncPair( "tmpfile", 	IO_Tmpfile ),
				new NameFuncPair( "type", 		IO_Type ),
				new NameFuncPair( "write", 		IO_Write ),
				new NameFuncPair( "seek",		IO_Seek),
			};

			lua.L_NewLib( define );

			streamManager = new();

            return 1;
		}

		private static int GetFileIdFromTable(ILuaState lua, int idx)
		{
			lua.GetField(idx, "fileId");
			int fileId = lua.ToInteger(-1);
			lua.Pop(1);
			return fileId;
		}

		private static void PushFileTable(ILuaState lua, int fileId)
		{
			lua.NewTable();
			lua.PushInteger(fileId);
			lua.SetField(-2, "fileId");
            lua.PushCSharpFunction(IO_Close);
            lua.SetField(-2, "close");
            lua.PushCSharpFunction(IO_Flush);
            lua.SetField(-2, "flush");
            lua.PushCSharpFunction(IO_Read);
            lua.SetField(-2, "read");
            lua.PushCSharpFunction(IO_Write);
            lua.SetField(-2, "write");
            lua.PushCSharpFunction(IO_Seek);
            lua.SetField(-2, "seek");
        }

		private static int IO_Close( ILuaState lua )
		{
			int fileId = GetFileIdFromTable(lua, 1);
			var stream = streamManager.GetFileStream(fileId);
			if(stream != null)
			{
				stream.Close();
				// TODO: Store files until lua program finished
				streamManager.RemoveFileStream(fileId);
				lua.PushBoolean(true);
			}
			else
			{
				lua.PushNil();
				lua.PushString("not a file");
			}
			return 1;
		}

		private static int IO_Flush( ILuaState lua )
		{
			int fileId = GetFileIdFromTable(lua, 1);
            var stream = streamManager.GetFileStream(fileId);
            if (stream != null)
			{
				stream.Flush();
				lua.PushBoolean(true);
			}
			else
			{
				lua.PushNil();
				lua.PushString("not a file");
			}
			return 1;
		}

		private static int IO_Input( ILuaState lua )
		{
			// TODO
			return 0;
		}

		private static int IO_Lines( ILuaState lua )
		{
			var path = GetFileIdFromTable(lua, 1);
			try
			{
				// TODO: Remake when implementing fs mapping
				var lines = File.ReadAllLines(GlobalData.CurrentVolume + path);
				lua.NewTable();
				int idx = 1;
				foreach(var line in lines)
				{
					lua.PushString(line);
					lua.RawSetI(-2, idx);
					idx++;
				}
				return 1;
			}
			catch (Exception e)
			{
				lua.PushNil();
				lua.PushString(e.Message);
				return 1;
			}
		}

		private static int IO_Open( ILuaState lua )
		{
			var path = lua.ToString(1);
			var mode = lua.L_OptString(2, "r");
			FileStream stream;
			try
			{
				switch (mode)
				{
					case "r":
						stream = File.OpenRead(GlobalData.CurrentVolume + path);
						break;
					case "w":
						stream = File.OpenWrite(GlobalData.CurrentVolume + path);
						break;
					case "a":
						stream = new FileStream(GlobalData.CurrentVolume + path, FileMode.Append);
						break;
					default:
						lua.PushNil();
						lua.PushString("invalid mode");
						return 1;
				}
				int fileId = streamManager.AddFileStream(stream);
				PushFileTable(lua, fileId);
				return 1;
			}
			catch (Exception e)
			{
                lua.PushNil();
                lua.PushString(e.Message);
                return 1;
            }
		}

		private static int IO_Output( ILuaState lua )
		{
			// TODO	
			return 0;
		}

		private static int IO_Popen( ILuaState lua )
		{
			return 0;
		}

		private static int IO_Read( ILuaState lua )
		{
            int fileId = GetFileIdFromTable(lua, 1);
            var stream = streamManager.GetFileStream(fileId);
            if (stream == null)
			{
				lua.PushNil();
				lua.PushString("not a file");
				return 1;
			}

			var reader = new StreamReader(stream);
			var format = lua.L_OptString(2, "*l");

			switch (format)
			{
				case "*n":
					int c = reader.Read();
					if (c == -1)
						lua.PushNil();
					else
						lua.PushNumber(c);
					break;
				case "*l":
					lua.PushString(reader.ReadLine());
					break;
				case "*a":
					lua.PushString(reader.ReadToEnd());
					break;
				default:
					lua.PushNil();
					lua.PushString("invalid format");
					return 1;
			}

			return 1;
		}

		private static int IO_Tmpfile( ILuaState lua )
		{
			var tempFile = Path.GetTempFileName();
			if(tempFile == null)
			{
				lua.PushNil();
				lua.PushString("unable to get temp file");
				return 1;
			}
			var fileStream = File.Open(tempFile, FileMode.OpenOrCreate);
			int fileId = streamManager.AddFileStream(fileStream);
			PushFileTable(lua, fileId);
			return 1;
		}

		private static int IO_Type( ILuaState lua )
		{
            int fileId = lua.ToInteger(1);
            var stream = streamManager.GetFileStream(fileId);
			if (stream == null)
				lua.PushNil();
			else if (!stream.CanSeek && !stream.CanRead && !stream.CanWrite)
				lua.PushString("closed file");
			else
				lua.PushString("file");
			return 1;
		}

		private static int IO_Write( ILuaState lua )
		{
			int fileId = GetFileIdFromTable(lua, 1);
            var stream = streamManager.GetFileStream(fileId);
            if (stream == null)
            {
				lua.PushNil();
				lua.PushString("not a file");
				return 1;
            }

			var writer = new StreamWriter(stream);
			for(int i = 2; i <= lua.GetTop(); i++)
			{
				writer.Write(lua.ToString(i));
			}
			writer.Flush();
			lua.PushBoolean(true);
            return 1;
		}

		private static int IO_Seek( ILuaState lua )
		{
			int fileId = GetFileIdFromTable(lua, 1);
			string whence = lua.L_OptString(2, "cur");
			long offset = lua.L_OptInt(3, 0);

			var stream = streamManager.GetFileStream(fileId);
			SeekOrigin origin;
			try
			{
				origin = whence switch
                {
                    "set" => SeekOrigin.Begin,
                    "cur" => SeekOrigin.Current,
                    "end" => SeekOrigin.End,
                    _ => throw new Exception()
                };
            }
			catch
			{
				lua.PushNil();
				lua.PushString("invalid seek mode");
				return 1;
			}

			long newPos = stream.Seek(offset, origin);
			lua.PushUInt64((ulong)newPos);
			return 1;
		}
	}

}

