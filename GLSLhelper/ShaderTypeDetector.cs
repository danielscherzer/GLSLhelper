using System.Text.RegularExpressions;

namespace GLSLhelper
{
	public static class ShaderTypeDetector
	{
		public static readonly Regex TessellationControlVertices = new Regex(@"layout\s*\(\s*vertices");

		public static ShaderType AutoDetectShaderContentType(string shaderCode)
		{
			if (shaderCode.Contains("EmitVertex")) return ShaderType.Geometry;
			if (shaderCode.Contains("local_size_")) return ShaderType.Compute;
			if (TessellationControlVertices.IsMatch(shaderCode)) return ShaderType.TessellationControl;
			if (shaderCode.Contains("gl_TessCoord")) return ShaderType.TessellationEvaluation;
			if (shaderCode.Contains("gl_Position")) return ShaderType.Vertex;
			return ShaderType.Fragment;
		}
	}
}
