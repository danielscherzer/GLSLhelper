using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GLSLhelper.Test
{
	[TestClass]
	public class ShaderLogParserTest
	{
		[TestMethod]
		public void EmptyLog()
		{
			var input = @"";
			var parser = new ShaderLogParser(input);
			Assert.IsTrue(parser.Lines.Count() == 0);
		}

		[TestMethod]
		public void Parse2Errors()
		{
			var input = @"
ERROR: 0:9: '' :  syntax error, unexpected IDENTIFIER, expecting COMMA or SEMICOLON
ERROR: 1 compilation errors.  No code generated.


";
			var parser = new ShaderLogParser(input);
			Assert.AreEqual(2, parser.Lines.Count());
			Assert.AreEqual(9, parser.Lines.First().LineNumber);
		}

		
		[TestMethod]
		public void ParseNvError()
		{
			var input = "D:\\Daten\\C#\\Zenseless\\examples\\ACG\\02 - ShaderDebugDialogExample\\Content\\shader.vert(10): error C0000: syntax error, unexpected '[', expecting \"::\" at token \"[\"";
			var parser = new ShaderLogParser(input);
			Assert.AreEqual(1, parser.Lines.Count());
			Assert.AreEqual(10, parser.Lines.First().LineNumber);
		}

		[TestMethod]
		public void ParseNvErrors()
		{
			var input = @"D:\Daten\C#\Zenseless\examples\ACG\02 - ShaderDebugDialogExample\Content\shader.vert(3): error C0000: syntax error, unexpected identifier, expecting '{' at token ""c3""\n
D:\Daten\C#\Zenseless\examples\ACG\02 - ShaderDebugDialogExample\Content\shader.vert(10): error C1503: undefined variable ""pos""\n
D:\Daten\C#\Zenseless\examples\ACG\02 - ShaderDebugDialogExample\Content\shader.vert(11): error C1503: undefined variable ""pos""";
			var parser = new ShaderLogParser(input);
			Assert.AreEqual(3, parser.Lines.Count());
			Assert.AreEqual(3, parser.Lines.First().LineNumber);
			Assert.AreEqual(10, parser.Lines.ElementAt(1).LineNumber);
			Assert.AreEqual(11, parser.Lines.ElementAt(2).LineNumber);
		}

		[TestMethod]
		public void Parse6Errors()
		{
			var input = @"
ERROR: 0:9: '' :  syntax error, unexpected IDENTIFIER, expecting COMMA or SEMICOLON
ERROR: 1 compilation errors.  No code generated.



ERROR: 0:9: 'dist' : undeclared identifier 
ERROR: 0:9: 'dist' : redefinition 
ERROR: 0:9: '' : compilation terminated 
ERROR: 3 compilation errors.  No code generated.


";
			var parser = new ShaderLogParser(input);
			Assert.AreEqual(6, parser.Lines.Count());
			Assert.AreEqual(9, parser.Lines.First().LineNumber);
			Assert.AreEqual(9, parser.Lines.ElementAt(1).LineNumber);
			Assert.AreEqual(9, parser.Lines.ElementAt(2).LineNumber);
			Assert.AreEqual(9, parser.Lines.ElementAt(3).LineNumber);
			Assert.AreEqual(ShaderLogLine.WellKnownTypeError, parser.Lines.ElementAt(3).Type);
		}

		[TestMethod]
		public void GlslcParsing()
		{
			var input = @".\shader.vert:3: error: 'non-opaque uniforms outside a block' : not allowed when using GLSL for Vulkan
1 error generated.";
			var parser = new ShaderLogParser(input);
			Assert.AreEqual(2, parser.Lines.Count());
			Assert.AreEqual(3, parser.Lines.First().LineNumber);
			Assert.AreEqual(ShaderLogLine.WellKnownTypeError, parser.Lines.First().Type);
		}

		[TestMethod]
		public void GlslcParsing2()
		{
			var input = @"
C:\work\IrrlichtBAW\branch\examples_tests\42.EnvmapLookup\envCubeMapShaders\envmap.frag:441: error: 'texelFetch' : no matching overloaded function found
C:\work\IrrlichtBAW\branch\examples_tests\42.EnvmapLookup\envCubeMapShaders\envmap.frag:442: error: '=' :  cannot convert from ' const float' to ' temp highp uint'
2 errors generated.";
			var parser = new ShaderLogParser(input);
			Assert.AreEqual(3, parser.Lines.Count());
			Assert.AreEqual(441, parser.Lines.First().LineNumber);
			Assert.AreEqual(442, parser.Lines.ElementAt(1).LineNumber);
			Assert.AreEqual(ShaderLogLine.WellKnownTypeError, parser.Lines.ElementAt(1).Type);
		}

		[TestMethod]
		public void GlslcParsing3()
		{
			var input = @"C:\work\IrrlichtBAW\branch\examples_tests\42.EnvmapLookup\envCubeMapShaders\envmap.frag:356: error: 'NdotV' : no such field in structure\n
C:\work\IrrlichtBAW\branch\examples_tests\42.EnvmapLookup\envCubeMapShaders\envmap.frag:356: error: '*' :  wrong operand types: no operation '*' exists that takes a left-hand operand of type ' temp structure{ global structure{ global structure{ global highp 3-component vector of float dir,  global highp 2X3 matrix of float dPosdScreen} V,  global highp 3-component vector of float N,  global highp float NdotV,  global highp float NdotV_squared} isotropic,  global highp 3-component vector of float T,  global highp 3-component vector of float B,  global highp float TdotV,  global highp float BdotV}' and a right operand of type ' temp structure{ global structure{ global structure{ global highp 3-component vector of float dir,  global highp 2X3 matrix of float dPosdScreen} V,  global highp 3-component vector of float N,  global highp float NdotV,  global highp float NdotV_squared} isotropic,  global highp 3-component vector of float T,  global highp 3-component vector of float B,  global highp float TdotV,  global highp float BdotV}' (or there is no acceptable conversion)\n
C:\work\IrrlichtBAW\branch\examples_tests\42.EnvmapLookup\envCubeMapShaders\envmap.frag:356: error: 'assign' :  cannot convert from ' temp structure{ global structure{ global structure{ global highp 3-component vector of float dir,  global highp 2X3 matrix of float dPosdScreen} V,  global highp 3-component vector of float N,  global highp float NdotV,  global highp float NdotV_squared} isotropic,  global highp 3-component vector of float T,  global highp 3-component vector of float B,  global highp float TdotV,  global highp float BdotV}' to ' global highp float'";
			var parser = new ShaderLogParser(input);
			Assert.AreEqual(3, parser.Lines.Count());
			Assert.AreEqual(356, parser.Lines.First().LineNumber);
			Assert.AreEqual(356, parser.Lines.ElementAt(1).LineNumber);
			Assert.AreEqual(356, parser.Lines.ElementAt(2).LineNumber);
			Assert.AreEqual(ShaderLogLine.WellKnownTypeError, parser.Lines.ElementAt(1).Type);
		}
	}
}
