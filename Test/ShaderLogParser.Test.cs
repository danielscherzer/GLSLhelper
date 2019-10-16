using GLSLhelper;
using System.Linq;
using Xunit;

namespace Test
{
	public class UnitTest1
	{
		[Fact]
		public void RemoveEmptyLines()
		{
			var input = @"";
			var parser = new ShaderLogParser(input);
			Assert.Empty(parser.Lines);
		}

		[Fact]
		public void RemoveEmptyLines2()
		{
			var input = @"
ERROR: 0:9: '' :  syntax error, unexpected IDENTIFIER, expecting COMMA or SEMICOLON
ERROR: 1 compilation errors.  No code generated.


";
			var parser = new ShaderLogParser(input);
			Assert.Equal(2, parser.Lines.Count());
			Assert.Equal(9, parser.Lines.First().LineNumber);
		}

		[Fact]
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
			Assert.Equal(6, parser.Lines.Count());
			Assert.Equal(9, parser.Lines.First().LineNumber);
			Assert.Equal(9, parser.Lines.ElementAt(2).LineNumber);
			Assert.Equal(9, parser.Lines.ElementAt(3).LineNumber);
			Assert.Equal(9, parser.Lines.ElementAt(4).LineNumber);
		}
	}
}
