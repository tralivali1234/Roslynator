// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Roslynator.CSharp.Syntax.SyntaxInfoHelpers;

namespace Roslynator.CSharp.Syntax
{
    public readonly struct StringLiteralExpressionInfo : IEquatable<StringLiteralExpressionInfo>
    {
        private StringLiteralExpressionInfo(LiteralExpressionSyntax expression)
        {
            Expression = expression;
        }

        private static StringLiteralExpressionInfo Default { get; } = new StringLiteralExpressionInfo();

        public LiteralExpressionSyntax Expression { get; }

        public SyntaxToken Token
        {
            get { return Expression?.Token ?? default(SyntaxToken); }
        }

        public string Text
        {
            get { return Token.Text; }
        }

        public string InnerText
        {
            get
            {
                string text = Text;

                int length = text.Length;

                if (length == 0)
                    return "";

                if (text[0] == '@')
                    return text.Substring(2, length - 3);

                return text.Substring(1, length - 2);
            }
        }

        public string ValueText
        {
            get { return Token.ValueText; }
        }

        public bool IsRegular
        {
            get { return Text.StartsWith("\"", StringComparison.Ordinal); }
        }

        public bool IsVerbatim
        {
            get { return Text.StartsWith("@", StringComparison.Ordinal); }
        }

        public bool ContainsLinefeed
        {
            get
            {
                if (IsRegular)
                    return ValueText.Contains("\n");

                if (IsVerbatim)
                    return Text.Contains("\n");

                return false;
            }
        }

        public bool ContainsEscapeSequence
        {
            get
            {
                if (IsRegular)
                    return Text.Contains("\\");

                if (IsVerbatim)
                    return InnerText.Contains("\"\"");

                return false;
            }
        }

        public bool Success
        {
            get { return Expression != null; }
        }

        internal static StringLiteralExpressionInfo Create(
            SyntaxNode node,
            bool walkDownParentheses = true)
        {
            return Create(Walk(node, walkDownParentheses) as LiteralExpressionSyntax);
        }

        internal static StringLiteralExpressionInfo Create(LiteralExpressionSyntax literalExpression)
        {
            if (literalExpression?.Kind() != SyntaxKind.StringLiteralExpression)
                return Default;

            return new StringLiteralExpressionInfo(literalExpression);
        }

        public override string ToString()
        {
            return Expression?.ToString() ?? "";
        }

        public override bool Equals(object obj)
        {
            return obj is StringLiteralExpressionInfo other && Equals(other);
        }

        public bool Equals(StringLiteralExpressionInfo other)
        {
            return EqualityComparer<LiteralExpressionSyntax>.Default.Equals(Expression, other.Expression);
        }

        public override int GetHashCode()
        {
            return Expression?.GetHashCode() ?? 0;
        }

        public static bool operator ==(StringLiteralExpressionInfo info1, StringLiteralExpressionInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(StringLiteralExpressionInfo info1, StringLiteralExpressionInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
