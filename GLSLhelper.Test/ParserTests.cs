using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GLSLhelper.Tests
{
	[TestClass()]
	public class ParserTests
	{
		[DataTestMethod()]
		[DataRow("float", "test1")]
		[DataRow("vec2", "_1test")]
		[DataRow("mat2", "_1test#")]
		public void ParseUniformsCheckNamings(string type, string name)
		{
			var parser = Parser.ParseUniforms($"uniform {type} {name};");
			Assert.AreEqual(type, parser.First().type);
			Assert.AreEqual(name, parser.First().name);
		}

		[DataTestMethod()]
		[DataRow("		uniform	float	test1	;", "float", "test1")]
		[DataRow("		uniform				float test1    ;", "float", "test1")]
		public void ParseUniformsCheckWhitespaces(string input, string type, string name)
		{
			var parser = Parser.ParseUniforms(input);
			Assert.AreEqual(type, parser.First().type);
			Assert.AreEqual(name, parser.First().name);
		}
	}
}