using System;
using Serilog;

namespace JungleDash_Godot
{
	internal static class Jungle_Dash_Logger
	{
		public static Serilog.ILogger logger { get; private set; }
		public static bool Initialized { get; private set; }

		public static void init(string logfilename)
		{
			logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Console()
				.WriteTo.File(logfilename, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
				.CreateLogger();

			logger.Debug("Logger Initialisiert");
			Initialized = true;
		}
	}
}
