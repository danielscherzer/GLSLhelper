using System.Text.RegularExpressions;

namespace GLSLhelper
{
	public static class RegexPatterns
	{
		public static string BlockComment => $@"{Regex.Escape("/*")}(.|[\r\n])*?{Regex.Escape("*/")}"; //match everything till end block comment
		public static string Include => @"#include\s+""([^""]+)"""; //match everything inside " except " so we get shortest ".+" match
		public static string Uniform => @"uniform\s+([^\s]+)\s+([^\s]+)\s*;"; //matches uniform<spaces>type<spaces>name<spaces>; 
	}
}
