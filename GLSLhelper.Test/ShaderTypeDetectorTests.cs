using Microsoft.VisualStudio.TestTools.UnitTesting;
using static GLSLhelper.ShaderTypeDetector;

namespace GLSLhelper.Test
{
	[TestClass()]
	public class ShaderTypeDetectorTests
	{
		[DataTestMethod()]
		[DataRow("void main() { }", ShaderType.Fragment)]
		[DataRow("void main() { gl_FragColor = vec4(1.0); }", ShaderType.Fragment)]
		[DataRow("out vec4 fragColor; void main() { fragColor = vec4(1.0f); }", ShaderType.Fragment)]
		[DataRow("layout(early_fragment_tests) in; out vec4 fragColor; void main() { fragColor = vec4(1.0f); }", ShaderType.Fragment)]
		[DataRow("void main() { gl_Position = vec4(1.0); }", ShaderType.Vertex)]
		[DataRow("void main() { EmitVertex(); }", ShaderType.Geometry)]
		[DataRow("void main() { gl_Position = vec4(gl_TessCoord.xy, 0.0, 1.0); }", ShaderType.TessellationEvaluation)]
		[DataRow("layout	(	vertices = 4) out; void main() { gl_TessLevelOuter[0] = -1; }", ShaderType.TessellationControl)]
		[DataRow("layout(vertices=4) out; void main() { }", ShaderType.TessellationControl)]
		[DataRow("layout (local_size_x = 16, local_size_y = 16) in; void main() { }", ShaderType.Compute)]
		public void AutoDetectShaderContentTypeTest(string sourceCode, ShaderType expectedResult)
		{
			var result = AutoDetectFromCode(sourceCode);
			Assert.AreEqual(expectedResult, result);
		}
	}
}