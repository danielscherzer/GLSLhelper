using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GLSLhelper.Test
{
	[TestClass]
	public class ShaderLogParserTest
	{
		[TestMethod]
		public void RemoveEmptyLines()
		{
			var input = @"";
			var parser = new ShaderLogParser(input);
			Assert.IsTrue(parser.Lines.Count() == 0);
		}

		[TestMethod]
		public void RemoveEmptyLines2()
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
		public void LineNumbers()
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
			Assert.AreEqual(9, parser.Lines.ElementAt(2).LineNumber);
			Assert.AreEqual(9, parser.Lines.ElementAt(3).LineNumber);
			Assert.AreEqual(9, parser.Lines.ElementAt(4).LineNumber);
		}
	}
}
