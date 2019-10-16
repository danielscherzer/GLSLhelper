using System;
using System.Collections.Generic;

namespace GLSLhelper
{
	/// <summary>
	/// Class that parses GLSL shader logs into lines with line numbers
	/// </summary>
	public class ShaderLogParser
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ShaderLogParser"/> class by parsing the given log string.
		/// </summary>
		/// <param name="log">The log.</param>
		public ShaderLogParser(string log)
		{
			//parse error log
			log = log.Replace("\r", string.Empty);
			char[] splitChars = new char[] { '\n' };
			var errorLines = new List<ShaderLogLine>();
			var otherLines = new List<ShaderLogLine>();
			foreach (var line in log.Split(splitChars, StringSplitOptions.RemoveEmptyEntries))
			{
				ShaderLogLine logLine;
				try
				{
					logLine = ParseLogLineNVIDIA(line);
				}
				catch
				{
					logLine = ParseLogLine(line);
				}
				if (logLine.Type.StartsWith(ShaderLogLine.WellKnownTypeError))
				{
					errorLines.Add(logLine);
				}
				else
				{
					otherLines.Add(logLine);
				}
			}
			//first error messages, then all others
			lines = errorLines;
			lines.AddRange(otherLines);
		}

		/// <summary>
		/// Gets the lines.
		/// </summary>
		/// <value>
		/// The lines.
		/// </value>
		public IEnumerable<ShaderLogLine> Lines { get { return lines; } }

		/// <summary>
		/// Parses the log line.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		private ShaderLogLine ParseLogLine(string line)
		{
			ShaderLogLine logLine = new ShaderLogLine();
			char[] splitChars = new char[] { ':' };
			var elements = line.Split(splitChars, 4);
			switch(elements.Length)
			{
				case 4:
					logLine.Type = ParseType(elements[0]);
					logLine.FileNumber = Parse(elements[1]);
					logLine.LineNumber = Parse(elements[2]);
					logLine.Message = elements[3];
					break;
				case 3:
					logLine.Type = ParseType(elements[0]);
					logLine.Message = elements[1] + ":" + elements[2];
					break;
				case 2:
					logLine.Type = ParseType(elements[0]);
					logLine.Message = elements[1];
					break;
				case 1:
					logLine.Message = elements[0];
					break;
				default:
					throw new ArgumentException(line);
			}
			logLine.Message = logLine.Message.Trim();
			return logLine;
		}

		/// <summary>
		/// Parses the log line NVIDIA.
		/// </summary>
		/// <param name="line">The line.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		private ShaderLogLine ParseLogLineNVIDIA(string line)
		{
			ShaderLogLine logLine = new ShaderLogLine();
			char[] splitChars = new char[] { ':' };
			var elements = line.Split(splitChars, 3);
			switch (elements.Length)
			{
				case 3:
					logLine.FileNumber = ParseNVFileNumber(elements[0]);
					logLine.LineNumber = ParseNVLineNumber(elements[0]);
					logLine.Type = ParseNVType(elements[1]);
					logLine.Message = elements[1] + ":" + elements[2];
					break;
				default:
					throw new ArgumentException(line);
			}
			logLine.Message = logLine.Message.Trim();
			return logLine;
		}

		/// <summary>
		/// Parses the type of NIVIDA.
		/// </summary>
		/// <param name="v">The v.</param>
		/// <returns></returns>
		private string ParseNVType(string v)
		{
			char[] splitChars = new char[] { ' ', '\t' };
			var elements = v.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
			return ParseType(elements[0]);
		}

		/// <summary>
		/// Parses NVIDIA line number.
		/// </summary>
		/// <param name="v">The v.</param>
		/// <returns></returns>
		private int ParseNVLineNumber(string v)
		{
			char[] splitChars = new char[] { '(',')', ' ', '\t' };
			var elements = v.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
			return Parse(elements[1]);
		}

		/// <summary>
		/// Parses the NVIDIA file number.
		/// </summary>
		/// <param name="v">The v.</param>
		/// <returns></returns>
		private int ParseNVFileNumber(string v)
		{
			char[] splitChars = new char[] { '(', ')', ' ', '\t' };
			var elements = v.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
			return Parse(elements[0]);
		}

		/// <summary>
		/// Parses the type.
		/// </summary>
		/// <param name="typeString">The type string.</param>
		/// <returns></returns>
		private string ParseType(string typeString)
		{
			return typeString.ToUpperInvariant().Trim();
		}

		/// <summary>
		/// Parses the specified number.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <returns></returns>
		private int Parse(string number)
		{
			if (int.TryParse(number, out int output))
			{
				return output;
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// The lines
		/// </summary>
		private List<ShaderLogLine> lines = new List<ShaderLogLine>();
	}
}
