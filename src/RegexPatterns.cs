using System.Text.RegularExpressions;

namespace GLSLhelper
{
	public static class RegexPatterns
	{
		/// <summary>
		/// Matches everything till end block comment into a group
		/// </summary>
		/// <value>
		/// Regular expression.
		/// </value>
		//public static string BlockComment => $@"{Regex.Escape("/*")}(.|[\r\n])*?{Regex.Escape("*/")}";
		public static readonly Regex BlockComment = new Regex(@"/\*(.|[\r\n])*?\*/");

		/// <summary>
		/// Matches everything inside #include "(.)" except " so we get shortest ".+" match into a group
		/// </summary>
		/// <value>
		/// Regular expression.
		/// </value>
		//public static string Include => @"#include\s+""([^""]+)"""; //
		public static readonly Regex Include = new Regex(@"#include\s+""([^""]+)""");

		/// <summary>
		/// Matches uniform<spaces>type<spaces>name<spaces>; into two groups 
		/// </summary>
		/// <value>
		/// Regular expression.
		/// </value>
		//public static string Uniform => @"uniform\s+([^\s]+)\s+([^\s]+)\s*;";
		public static readonly Regex Uniform = new Regex(@"uniform\s+([^\s]+)\s+([^\s]+)\s*;");
	}
}
