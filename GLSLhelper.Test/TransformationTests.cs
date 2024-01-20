using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GLSLhelper.Test
{
	[TestClass]
	public class TransformationTests
	{
		[DataTestMethod]
		[DataRow("// a test//", "")]
		[DataRow("// a test\nsomething\n// comment ", "\nsomething\n")]
		public void RemoveLineComments(string input, string expectedOutput)
		{
			var actual = Transformation.RemoveLineComments(input);
			Assert.AreEqual(expectedOutput, actual);
		}

		[DataTestMethod]
		[DataRow("/*test2;test3;*/", "")]
		[DataRow("/*test2;\ntest3;*/", "\n")]
		public void RemoveBlockComments(string input, string expectedOutput)
		{
			var actual = Transformation.ReplaceBlockCommentsByEmptyLines(input);
			Assert.AreEqual(expectedOutput, actual);
		}

		[DataTestMethod]
		[DataRow("// an test\n/*test2;\ntest3;\n*/\n", "\n\n\n\n")]
		public void RemoveComments(string input, string expectedOutput)
		{
			var actual = Transformation.RemoveComments(input);
			Assert.AreEqual(expectedOutput, actual);
		}

		[TestMethod]
		public void RemoveCommentsProgram()
		{
			var input = Transformation.UnixLineEndings(@"#version 140
// an alysis of https://www.shadertoy.com/view/XsXXDn
// http://www.pouet.net/prod.php?which=57245
// If you intend to reuse this shader, please add credits to 'Danilo Guanabara'
/*uniform vec2 iResolution;
uniform float iGlobalTime;
*/
/*void mainImage( out vec4 fragColor, in vec2 fragCoord )*/
{
	vec2 uv = fragCoord.xy / iResolution; // range [0,1]^2; origin lower left corner
	float aspect = iResolution.x / iResolution.y;
	vec2 p = uv - 0.5; // range [-0.5 0.5]^2; origin in center
	p.x *= aspect; // view port aspect correction
	vec2 uvTime = uv 
		+ p / dist * (sin(time) + 1.0) * abs( sin( dist * 9.0 - time * 2.0))
}
");
			var expectedOutput = Transformation.UnixLineEndings(@"#version 140







{
	vec2 uv = fragCoord.xy / iResolution; 
	float aspect = iResolution.x / iResolution.y;
	vec2 p = uv - 0.5; 
	p.x *= aspect; 
	vec2 uvTime = uv 
		+ p / dist * (sin(time) + 1.0) * abs( sin( dist * 9.0 - time * 2.0))
}
");
			var actual = Transformation.RemoveComments(input);
			Assert.AreEqual(expectedOutput, actual);
		}

		[TestMethod]
		public void ExpandNoIncludes()
		{
			var input = Transformation.UnixLineEndings(@"");
			int count = 0;
			var actual = Transformation.ExpandIncludes(input, include => { ++count; return string.Empty; });
			Assert.AreEqual(0, count);
		}

		[TestMethod]
		public void ExpandIncludes()
		{
			var input = @"#version 330
#include ""../ libs / camera.glsl""
#include ""../libs/hg_sdf.glsl""
#include ""../libs/operators.glsl""
uniform vec2 iResolution; ";
			int count = 0;
			var actual = Transformation.ExpandIncludes(input, include => { ++count; return "INCLUDE"; });
			Assert.AreEqual(3, count);
		}

		[TestMethod]
		public void ExpandIncludes2()
		{
			var input = Transformation.UnixLineEndings(@"#version 330
#include ""../ libs / camera.glsl""
#include ""../libs/hg_sdf.glsl""
#include ""../libs/operators.glsl""
uniform vec2 iResolution; ");
			var expected = Transformation.UnixLineEndings(@"#version 330
INCLUDE
#line 2

INCLUDE
#line 3

INCLUDE
#line 4

uniform vec2 iResolution; ");
			var actual = Transformation.ExpandIncludes(input, include => "INCLUDE");
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ExpandNestedIncludes()
		{
			var input = @"#version 330
#include ""../ libs / camera.glsl""
#include ""../libs/hg_sdf.glsl""
#include ""../libs/operators.glsl""
uniform vec2 iResolution; ";

			int count = 0;
			var actual = Transformation.ExpandIncludes(input, include => { ++count; return "#include \"../libs/nested.glsl\""; });
			Assert.AreEqual(4, count);
		}

		[TestMethod]
		public void ExpandIncludesInComment()
		{
			var input = @"#version 330
//#include ""../ libs / camera.glsl""
#include ""../libs/hg_sdf.glsl""
/*#include ""../libs/operators.glsl""*/
uniform vec2 iResolution; ";
			int count = 0;
			var actual = Transformation.ExpandIncludes(input, include => { ++count; return string.Empty; });
			Assert.AreEqual(1, count);
		}
	}
}
