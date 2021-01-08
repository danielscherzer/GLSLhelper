using System.Collections.Generic;

namespace GLSLhelper
{
	public static partial class GlslSpecification
	{
		private static readonly Dictionary<string, TokenType> s_reservedWords = Initialize();

		public const string Operators = "~.;,+-*/()[]{}<>=&$!%?:|^\\";

		public static IEnumerable<KeyValuePair<string, TokenType>> ReservedWords => s_reservedWords;

		public static TokenType GetReservedWordType(string word)
		{
			if (s_reservedWords.TryGetValue(word, out var type)) return type;
			return TokenType.Identifier;
		}

		public static bool IsIdentifierChar(char c) => char.IsLetterOrDigit(c) || '_' == c;

		public static bool IsIdentifierStartChar(char c) => char.IsLetter(c) || '_' == c;

		private static void AddRange(this Dictionary<string, TokenType> result, IEnumerable<string> words, TokenType type)
		{
			foreach (var word in words)
			{
				result[word] = type;
			}
		}
	}
}