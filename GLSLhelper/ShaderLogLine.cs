using System.Text;

namespace GLSLhelper
{
	/// <summary>
	/// Contains a parsed shader log line
	/// </summary>
	public class ShaderLogLine
	{
		/// <summary>
		/// The string type for the well known type warning
		/// </summary>
		public const string WellKnownTypeWarning = "WARNING";
		
		/// <summary>
		/// The string type for the well known type error
		/// </summary>
		public const string WellKnownTypeError = "ERROR";
		
		/// <summary>
		/// The string type for the well known type information
		/// </summary>
		public const string WellKnownTypeInfo = "INFO";
		
		/// <summary>
		/// The type
		/// </summary>
		public string Type = string.Empty;
		
		/// <summary>
		/// The file number
		/// </summary>
		public int FileNumber = -1;
		
		/// <summary>
		/// The line number
		/// </summary>
		public int LineNumber = -1;
		
		/// <summary>
		/// The message
		/// </summary>
		public string Message = string.Empty;
		
		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="string" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			if (string.Empty != Type)
			{
				sb.Append(Type + ": ");
			}
			if (-1 != LineNumber)
			{
				sb.Append("Line ");
				sb.Append(LineNumber.ToString());
				sb.Append(" : ");
			}
			sb.Append(Message);
			return sb.ToString();
		}
	}
}
