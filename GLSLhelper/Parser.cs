using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GLSLhelper
{
	public static class Parser
	{
		public static IEnumerable<(string type, string name)> ParseUniforms(string uncommentedShaderCode)
		{
			foreach (Match match in RegexPatterns.Uniform.Matches(uncommentedShaderCode))
			{
				var type = match.Groups[1].ToString();
				var name = match.Groups[2].ToString();
				yield return (type, name);
			}
		}
	}
}
