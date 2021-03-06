﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GLSLhelper.Test
{
	[TestClass()]
	public class ExctractTests
	{
		[DataTestMethod()]
		[DataRow("float", "test1")]
		[DataRow("vec2", "_1test")]
		[DataRow("mat2", "_1test#")]
		public void UniformsCheckNamings(string type, string name)
		{
			var parser = Extract.Uniforms($"uniform {type} {name};");
			Assert.AreEqual(type, parser.First().type);
			Assert.AreEqual(name, parser.First().name);
		}

		[DataTestMethod()]
		[DataRow("		uniform	float	test1	;", "float", "test1")]
		[DataRow("		uniform				float test1    ;", "float", "test1")]
		public void UniformsCheckWhitespaces(string input, string type, string name)
		{
			var parser = Extract.Uniforms(input);
			Assert.AreEqual(type, parser.First().type);
			Assert.AreEqual(name, parser.First().name);
		}
	}
}