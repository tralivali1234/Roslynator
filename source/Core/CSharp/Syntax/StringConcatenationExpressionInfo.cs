// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Roslynator.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.Syntax
{
    internal readonly struct StringConcatenationExpressionInfo : IEquatable<StringConcatenationExpressionInfo>, IReadOnlyList<ExpressionSyntax>
    {
        private StringConcatenationExpressionInfo(
            BinaryExpressionSyntax addExpression,
            ImmutableArray<ExpressionSyntax> expressions,
            TextSpan? span = null)
        {
            ContainsNonSpecificExpression = false;
            ContainsRegularLiteralExpression = false;
            ContainsVerbatimLiteralExpression = false;
            ContainsRegularInterpolatedStringExpression = false;
            ContainsVerbatimInterpolatedStringExpression = false;

            OriginalExpression = addExpression;
            Expressions = expressions;
            Span = span;

            foreach (ExpressionSyntax expression in expressions)
            {
                StringLiteralExpressionInfo stringLiteral = SyntaxInfo.StringLiteralExpressionInfo(expression);

                if (stringLiteral.Success)
                {
                    if (stringLiteral.IsVerbatim)
                    {
                        ContainsVerbatimLiteralExpression = true;
                    }
                    else
                    {
                        ContainsRegularLiteralExpression = true;
                    }
                }
                else if (expression.Kind() == SyntaxKind.InterpolatedStringExpression)
                {
                    if (((InterpolatedStringExpressionSyntax)expression).IsVerbatim())
                    {
                        ContainsVerbatimInterpolatedStringExpression = true;
                    }
                    else
                    {
                        ContainsRegularInterpolatedStringExpression = true;
                    }
                }
                else
                {
                    ContainsNonSpecificExpression = true;
                }
            }
        }

        private static StringConcatenationExpressionInfo Default { get; } = new StringConcatenationExpressionInfo();

        public ImmutableArray<ExpressionSyntax> Expressions { get; }

        //TODO: UnderlyingExpression
        public BinaryExpressionSyntax OriginalExpression { get; }

        public TextSpan? Span { get; }

        public int Count
        {
            get { return Expressions.Length; }
        }

        public ExpressionSyntax this[int index]
        {
            get { return Expressions[index]; }
        }

        IEnumerator<ExpressionSyntax> IEnumerable<ExpressionSyntax>.GetEnumerator()
        {
            return ((IEnumerable<ExpressionSyntax>)Expressions).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Expressions).GetEnumerator();
        }

        public ImmutableArray<ExpressionSyntax>.Enumerator GetEnumerator()
        {
            return Expressions.GetEnumerator();
        }

        public bool ContainsNonSpecificExpression { get; }

        public bool ContainsNonLiteralExpression
        {
            get { return ContainsInterpolatedStringExpression || ContainsNonSpecificExpression; }
        }

        public bool ContainsLiteralExpression
        {
            get { return ContainsRegularLiteralExpression || ContainsVerbatimLiteralExpression; }
        }

        public bool ContainsRegularLiteralExpression { get; }

        public bool ContainsVerbatimLiteralExpression { get; }

        public bool ContainsInterpolatedStringExpression
        {
            get { return ContainsRegularInterpolatedStringExpression || ContainsVerbatimInterpolatedStringExpression; }
        }

        public bool ContainsRegularInterpolatedStringExpression { get; }

        public bool ContainsVerbatimInterpolatedStringExpression { get; }

        //TODO: ContainsRegularExpression
        public bool ContainsRegular
        {
            get { return ContainsRegularLiteralExpression || ContainsRegularInterpolatedStringExpression; }
        }

        //TODO: ContainsVerbatimExpression
        public bool ContainsVerbatim
        {
            get { return ContainsVerbatimLiteralExpression || ContainsVerbatimInterpolatedStringExpression; }
        }

        public bool Success
        {
            get { return OriginalExpression != null; }
        }

        internal static StringConcatenationExpressionInfo Create(
            BinaryExpressionSyntax binaryExpression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (binaryExpression?.Kind() != SyntaxKind.AddExpression)
                return Default;

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            ImmutableArray<ExpressionSyntax> expressions = GetExpressions(binaryExpression, semanticModel, cancellationToken);

            if (expressions.IsDefault)
                return Default;

            return new StringConcatenationExpressionInfo(binaryExpression, expressions);
        }

        internal static StringConcatenationExpressionInfo Create(
            BinaryExpressionSelection binaryExpressionSelection,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            BinaryExpressionSyntax binaryExpression = binaryExpressionSelection.BinaryExpression;

            if (binaryExpression?.Kind() != SyntaxKind.AddExpression)
                return Default;

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            ImmutableArray<ExpressionSyntax> expressions = binaryExpressionSelection.Expressions;

            foreach (ExpressionSyntax expression in expressions)
            {
                if (!IsStringExpression(expression, semanticModel, cancellationToken))
                    return Default;
            }

            return new StringConcatenationExpressionInfo(binaryExpression, expressions, binaryExpressionSelection.Span);
        }

        private static bool IsStringExpression(ExpressionSyntax expression, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            if (expression == null)
                return false;

            if (expression.Kind().Is(
                SyntaxKind.StringLiteralExpression,
                SyntaxKind.InterpolatedStringExpression))
            {
                return true;
            }

            return semanticModel.GetTypeInfo(expression, cancellationToken)
                .ConvertedType?
                .IsString() == true;
        }

        private static ImmutableArray<ExpressionSyntax> GetExpressions(
            BinaryExpressionSyntax binaryExpression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken)
        {
            ImmutableArray<ExpressionSyntax>.Builder builder = null;

            while (true)
            {
                MethodInfo methodInfo = semanticModel.GetMethodInfo(binaryExpression, cancellationToken);

                if (methodInfo.Symbol != null
                    && methodInfo.MethodKind == MethodKind.BuiltinOperator
                    && methodInfo.Name == WellKnownMemberNames.AdditionOperatorName
                    && methodInfo.IsContainingType(SpecialType.System_String))
                {
                    (builder ?? (builder = ImmutableArray.CreateBuilder<ExpressionSyntax>())).Add(binaryExpression.Right);

                    ExpressionSyntax left = binaryExpression.Left;

                    if (left.IsKind(SyntaxKind.AddExpression))
                    {
                        binaryExpression = (BinaryExpressionSyntax)left;
                    }
                    else
                    {
                        builder.Add(left);
                        builder.Reverse();
                        return builder.ToImmutable();
                    }
                }
                else
                {
                    return default(ImmutableArray<ExpressionSyntax>);
                }
            }
        }

        public InterpolatedStringExpressionSyntax ToInterpolatedString()
        {
            ThrowInvalidOperationIfNotInitialized();

            StringBuilder sb = StringBuilderCache.GetInstance();

            sb.Append('$');

            if (!ContainsRegular)
                sb.Append('@');

            sb.Append('"');

            for (int i = 0; i < Expressions.Length; i++)
            {
                SyntaxKind kind = Expressions[i].Kind();

                StringLiteralExpressionInfo stringLiteral = SyntaxInfo.StringLiteralExpressionInfo(Expressions[i]);

                if (stringLiteral.Success)
                {
                    if (ContainsRegular
                        && stringLiteral.IsVerbatim)
                    {
                        string s = stringLiteral.ValueText;
                        s = StringUtility.DoubleBackslash(s);
                        s = StringUtility.EscapeQuote(s);
                        s = StringUtility.DoubleBraces(s);
                        s = s.Replace("\n", @"\n");
                        s = s.Replace("\r", @"\r");
                        sb.Append(s);
                    }
                    else
                    {
                        sb.Append(StringUtility.DoubleBraces(stringLiteral.InnerText));
                    }
                }
                else if (kind == SyntaxKind.InterpolatedStringExpression)
                {
                    var interpolatedString = (InterpolatedStringExpressionSyntax)Expressions[i];

                    bool isVerbatimInterpolatedString = interpolatedString.IsVerbatim();

                    foreach (InterpolatedStringContentSyntax content in interpolatedString.Contents)
                    {
                        Debug.Assert(content.IsKind(SyntaxKind.Interpolation, SyntaxKind.InterpolatedStringText), content.Kind().ToString());

                        switch (content.Kind())
                        {
                            case SyntaxKind.InterpolatedStringText:
                                {
                                    var text = (InterpolatedStringTextSyntax)content;

                                    if (ContainsRegular
                                        && isVerbatimInterpolatedString)
                                    {
                                        string s = text.TextToken.ValueText;
                                        s = StringUtility.DoubleBackslash(s);
                                        s = StringUtility.EscapeQuote(s);
                                        s = s.Replace("\n", @"\n");
                                        s = s.Replace("\r", @"\r");
                                        sb.Append(s);
                                    }
                                    else
                                    {
                                        sb.Append(content.ToString());
                                    }

                                    break;
                                }
                            case SyntaxKind.Interpolation:
                                {
                                    sb.Append(content.ToString());
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    sb.Append('{');
                    sb.Append(Expressions[i].ToString());
                    sb.Append('}');
                }
            }

            sb.Append("\"");

            return (InterpolatedStringExpressionSyntax)ParseExpression(StringBuilderCache.GetStringAndFree(sb));
        }

        public LiteralExpressionSyntax ToStringLiteral()
        {
            ThrowInvalidOperationIfNotInitialized();

            if (ContainsNonLiteralExpression)
                throw new InvalidOperationException();

            StringBuilder sb = StringBuilderCache.GetInstance();

            if (!ContainsRegular)
                sb.Append('@');

            sb.Append('"');

            foreach (ExpressionSyntax expression in Expressions)
            {
                StringLiteralExpressionInfo literal = SyntaxInfo.StringLiteralExpressionInfo(expression);

                if (literal.Success)
                {
                    if (ContainsRegular
                        && literal.IsVerbatim)
                    {
                        string s = literal.ValueText;
                        s = StringUtility.DoubleBackslash(s);
                        s = StringUtility.EscapeQuote(s);
                        s = s.Replace("\n", @"\n");
                        s = s.Replace("\r", @"\r");
                        sb.Append(s);
                    }
                    else
                    {
                        sb.Append(literal.InnerText);
                    }
                }
            }

            sb.Append('"');

            return (LiteralExpressionSyntax)ParseExpression(StringBuilderCache.GetStringAndFree(sb));
        }

        public LiteralExpressionSyntax ToMultilineStringLiteral()
        {
            ThrowInvalidOperationIfNotInitialized();

            if (ContainsNonLiteralExpression)
                throw new InvalidOperationException();

            StringBuilder sb = StringBuilderCache.GetInstance();

            sb.Append('@');
            sb.Append('"');

            for (int i = 0; i < Expressions.Length; i++)
            {
                if (Expressions[i].IsKind(SyntaxKind.StringLiteralExpression))
                {
                    var literal = (LiteralExpressionSyntax)Expressions[i];

                    string s = StringUtility.DoubleQuote(literal.Token.ValueText);

                    int charCount = 0;

                    if (s.Length > 0
                        && s[s.Length - 1] == '\n')
                    {
                        charCount = 1;

                        if (s.Length > 1
                            && s[s.Length - 2] == '\r')
                        {
                            charCount = 2;
                        }
                    }

                    sb.Append(s, 0, s.Length - charCount);

                    if (charCount > 0)
                    {
                        sb.AppendLine();
                    }
                    else if (i < Expressions.Length - 1)
                    {
                        TextSpan span = TextSpan.FromBounds(Expressions[i].Span.End, Expressions[i + 1].SpanStart);

                        if (OriginalExpression.SyntaxTree.IsMultiLineSpan(span))
                            sb.AppendLine();
                    }
                }
            }

            sb.Append('"');

            return (LiteralExpressionSyntax)ParseExpression(StringBuilderCache.GetStringAndFree(sb));
        }

        public override string ToString()
        {
            return (Span != null)
                ? OriginalExpression.ToString(Span.Value)
                : OriginalExpression.ToString();
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (OriginalExpression == null)
                throw new InvalidOperationException($"{nameof(StringConcatenationExpressionInfo)} is not initalized.");
        }

        public override bool Equals(object obj)
        {
            return obj is StringConcatenationExpressionInfo other && Equals(other);
        }

        public bool Equals(StringConcatenationExpressionInfo other)
        {
            return EqualityComparer<BinaryExpressionSyntax>.Default.Equals(OriginalExpression, other.OriginalExpression)
                && EqualityComparer<TextSpan?>.Default.Equals(Span, other.Span);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Span.GetHashCode(), Hash.Create(OriginalExpression));
        }

        public static bool operator ==(StringConcatenationExpressionInfo info1, StringConcatenationExpressionInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(StringConcatenationExpressionInfo info1, StringConcatenationExpressionInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
