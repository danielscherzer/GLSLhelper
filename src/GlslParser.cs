using Sprache;
using System.Collections.Generic;
using System.Linq;

namespace GLSLhelper
{
	public partial class GlslParser
	{
		private static readonly Parser<string> NumberWithTrailingDigit = from number in Parse.Number
																		 from trailingDot in Parse.Char('.')
																		 select number + trailingDot;
		private static readonly Parser<string> ParserNumber = Parse.DecimalInvariant.Or(NumberWithTrailingDigit);
		private static readonly Parser<string> ParserComment = new CommentParser().AnyComment;
		private static readonly Parser<string> ParserString = from start in Parse.Char('"')
															  from text in Parse.CharExcept("\"\r\n").Many().Text()
															  from end in Parse.Char('"').Optional()
															  select start + text + end;
		private static readonly Parser<string> ParserPreprocessor = from _ in Parse.Char('#')
																	from rest in Parse.LetterOrDigit.Many().Text()
																	select rest;
		private static readonly Parser<string> ParserIdentifier = Parse.Identifier(Parse.Char(GlslSpecification.IsIdentifierStartChar, "Identifier start"),
																			Parse.Char(GlslSpecification.IsIdentifierChar, "Identifier character"));

		//static readonly Parser<string> ParserFunction = from i in ParserIdentifier
		//												from w in Parse.WhiteSpace.Optional()
		//												from op in Parse.Char('(')
		//												select i;
		private static readonly Parser<char> ParserOperator = Parse.Chars(GlslSpecification.Operators);


		private readonly Parser<IEnumerable<Token>> tokenParser;

		public GlslParser()
		{
			var comment = ParserComment.Select(value => new Token(TokenType.Comment, value));
			var quotedString = ParserString.Select(value => new Token(TokenType.QuotedString, value));
			var preprocessor = ParserPreprocessor.Select(value => new Token(TokenType.Preprocessor, value));
			var number = ParserNumber.Select(value => new Token(TokenType.Number, value));
			var identifier = ParserIdentifier.Select(value => new Token(GlslSpecification.GetReservedWordType(value), value));
			var op = ParserOperator.Select(value => new Token(TokenType.Operator, value.ToString()));
			var token = comment.Or(preprocessor).Or(quotedString).Or(number).Or(identifier).Or(op);
			tokenParser = token.Positioned().Token().XMany();
		}

		public IEnumerable<IToken> Tokenize(string text)
		{
			if (0 == text.Trim().Length) yield break;
			var tokens = tokenParser.TryParse(text);
			if (tokens.WasSuccessful)
			{
				foreach (var token in tokens.Value)
				{
					yield return token;
				}
			}
		}
	}
}
