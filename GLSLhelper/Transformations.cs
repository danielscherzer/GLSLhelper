using System;
using System.Text.RegularExpressions;

namespace GLSLhelper
{
	public static class Transformations
	{
		/// <summary>
		/// Searches for #include statements in the shader code and replaces them by the code in the include resource.
		/// </summary>
		/// <param name="shaderCode">The shader code.</param>
		/// <param name="GetIncludeCode">Functor that will be called with the include path as parameter and returns the include shader code.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">GetIncludeCode</exception>
		public static string ExpandIncludes(string shaderCode, Func<string, string> GetIncludeCode)
		{
			if (GetIncludeCode == null) throw new ArgumentNullException(nameof(GetIncludeCode));

			var lines = shaderCode.Split(new[] { '\n' }, StringSplitOptions.None); //if UNIX style line endings still working so do not use Envirnoment.NewLine
			int lineNr = 1;
			foreach (var line in lines)
			{
				// Search for include pattern (e.g. #include raycast.glsl) (nested not supported)
				foreach (Match match in Regex.Matches(line, RegexPatterns.Include, RegexOptions.Singleline))
				{
					var sFullMatch = match.Value;
					var includeName = match.Groups[1].ToString(); // get the include
					var includeCode = GetIncludeCode(includeName);
					var lineNumberCorrection = $"{Environment.NewLine}#line {lineNr}{Environment.NewLine}";
					shaderCode = shaderCode.Replace(sFullMatch, includeCode + lineNumberCorrection); // replace #include with actual include code
				}
				++lineNr;
			}
			return shaderCode;
		}

		public static string RemoveBlockComments(string shaderCode)
		{
			return Regex.Replace(shaderCode, RegexPatterns.BlockComment, string.Empty);
		}

		public static string RemoveLineComments(string shaderCode)
		{
			var pattern = "//.*"; //match everything till end of line
			return Regex.Replace(shaderCode, pattern, string.Empty);
		}

		public static string RemoveComments(string shaderCode)
		{
			return RemoveLineComments(RemoveBlockComments(shaderCode));
		}
	}
}
