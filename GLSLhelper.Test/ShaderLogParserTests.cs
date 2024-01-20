using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace GLSLhelper.Test
{
	[TestClass]
	public class ShaderLogParserTests
	{
		[TestMethod]
		public void EmptyLog()
		{
			var input = @"";
			var parser = new ShaderLogParser(input);
			Assert.IsTrue(parser.Lines.Count() == 0);
		}

		private const string shaderFileName1 = @"D:\C#\02 - Content\shader.vert";
		[DataTestMethod()]
		[DataRow("\nERROR: 0:9: '' :  syntax error, unexpected IDENTIFIER, expecting COMMA or SEMICOLON", 9, MessageType.Error, "0")]
		[DataRow("\nWARNING: 0:0987: '' :  syntax error, unexpected IDENTIFIER, expecting COMMA or SEMICOLON", 987, MessageType.Warning, "0")]

		[DataRow("0(22) : error C1036: assignment to const variable color1", 22, MessageType.Error, "0")]
		[DataRow(shaderFileName1 + "(3): warning C0000: syntax error, unexpected identifier, expecting", 3, MessageType.Warning, shaderFileName1)]
		[DataRow(shaderFileName1 + "(3): error C0000: syntax error, unexpected identifier, expecting", 3, MessageType.Error, shaderFileName1)]
		[DataRow(shaderFileName1 + @"(1023): error C1503: undefined variable ""pos""", 1023, MessageType.Error, shaderFileName1)]

		[DataRow(shaderFileName1 + ":3: error: 'non-opaque uniforms outside a block' : not allowed when using GLSL for Vulkan", 3, MessageType.Error, shaderFileName1)]
		[DataRow(shaderFileName1 + ":356: Error: 'NdotV' : no such field in structure", 356, MessageType.Error, shaderFileName1)]
		[DataRow("\n" + shaderFileName1 + ":356: error: '*' :  wrong operand types: no operation '*' exists that takes a left-hand operand of type ' temp structure{ global structure{ global structure{ global highp 3-component vector of float dir,  global highp 2X3 matrix of float dPosdScreen} V,  global highp 3-component vector of float N,  global highp float NdotV,  global highp float NdotV_squared} isotropic,  global highp 3-component vector of float T,  global highp 3-component vector of float B,  global highp float TdotV,  global highp float BdotV}' and a right operand of type ' temp structure{ global structure{ global structure{ global highp 3-component vector of float dir,  global highp 2X3 matrix of float dPosdScreen} V,  global highp 3-component vector of float N,  global highp float NdotV,  global highp float NdotV_squared} isotropic,  global highp 3-component vector of float T,  global highp 3-component vector of float B,  global highp float TdotV,  global highp float BdotV}' (or there is no acceptable conversion)\n", 356, MessageType.Error, shaderFileName1)]
		[DataRow(shaderFileName1 + ":356: error: error: 'assign' :  cannot convert from ' temp structure{ global structure{ global structure{ global highp 3-component vector of float dir,  global highp 2X3 matrix of float dPosdScreen} V,  global highp 3-component vector of float N,  global highp float NdotV,  global highp float NdotV_squared} isotropic,  global highp 3-component vector of float T,  global highp 3-component vector of float B,  global highp float TdotV,  global highp float BdotV}' to ' global highp float'", 356, MessageType.Error, shaderFileName1)]

		[DataRow(@"WARNING:" + shaderFileName1 + ":5: 'assign' :  cannot convert from ' const 3-component vector of float'", 5, MessageType.Warning, shaderFileName1)]
		[DataRow(@"ERROR:" + shaderFileName1 + ":5: 'assign' :  cannot convert from ' const 3-component vector of float'", 5, MessageType.Error, shaderFileName1)]
		[DataRow(@"ERROR:" + shaderFileName1 + ":0345: 'assign' :  cannot convert from ' const 3-component vector of float'", 345, MessageType.Error, shaderFileName1)]
		public void ParseFirstOutputLine(string input, int lineNumber, MessageType type, string fileId)
		{
			var parser = new ShaderLogParser(input);
			Assert.AreEqual(1, parser.Lines.Count());
			var line = parser.Lines.First();
			Assert.AreEqual(lineNumber, line.LineNumber);
			Assert.AreEqual(type, line.Type);
			Assert.AreEqual(fileId, line.FileId);
		}

		private static IEnumerable<object[]> GetParseTypesData()
		{
			yield return new object[] { "\nERROR: 0:9: '' :  syntax error\nERROR: 1 compilation errors.  No code generated.", new MessageType[] { MessageType.Error, MessageType.Message } };
			yield return new object[] { "./shader.vert:3: error: 'non-opaque' : not allowed\n1 error generated.", new MessageType[] { MessageType.Error, MessageType.Message } };
		}

		[DataTestMethod()]
		[DynamicData(nameof(GetParseTypesData), DynamicDataSourceType.Method)]
		public void ParseTypes(string input, MessageType[] types)
		{
			var parser = new ShaderLogParser(input);
			Assert.AreEqual(types.Length, parser.Lines.Count());
			for (int i = 0; i < types.Length; ++i)
			{
				Assert.AreEqual(types[i], parser.Lines.ElementAt(i).Type);
			}
		}

		[TestMethod]
		public void Parse6Errors()
		{
			var input = "\nERROR: 0:9: '' :  syntax error, unexpected IDENTIFIER, expecting COMMA or SEMICOLON\nERROR: 1 compilation errors.  No code generated.\n\n\nERROR: 0:9: 'dist' : undeclared identifier\nERROR: 0:9: 'dist' : redefinition\nERROR: 0:9: '' : compilation terminated\nERROR: 3 compilation errors.  No code generated.\n\n";
			var parser = new ShaderLogParser(input);
			Assert.AreEqual(6, parser.Lines.Count());
			Assert.AreEqual(9, parser.Lines.First().LineNumber);
			Assert.AreEqual(9, parser.Lines.ElementAt(1).LineNumber);
			Assert.AreEqual(9, parser.Lines.ElementAt(2).LineNumber);
			Assert.AreEqual(9, parser.Lines.ElementAt(3).LineNumber);
			Assert.AreEqual(MessageType.Error, parser.Lines.ElementAt(3).Type);
			Assert.AreEqual(MessageType.Message, parser.Lines.Last().Type);
		}
	}
}
