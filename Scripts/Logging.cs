using UnityEngine;

namespace AeLa.Utilities.Eventful
{
	internal static class Logging
	{
		public static void Log(string message, LogType logType = LogType.Log)
		{
			Debug.LogFormat(logType, LogOption.None, null, "[Eventful] " + message);
		}

		public static void Warn(string message) => Log(message, LogType.Warning);

		public static void Error(string message) => Log(message, LogType.Error);
	}
}