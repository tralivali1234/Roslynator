// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

#pragma warning disable RCS1138

namespace Roslynator.CSharp
{
    /// <summary>
    /// A factory for syntax nodes, tokens and trivia. This class is built on top of <see cref="SyntaxFactory"/>.
    /// </summary>
    public static class CSharpFactory
    {
        #region Trivia
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxTrivia EmptyWhitespace()
        {
            return SyntaxTrivia(SyntaxKind.WhitespaceTrivia, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxTrivia NewLine()
        {
            switch (Environment.NewLine)
            {
                case "\r":
                    return CarriageReturn;
                case "\n":
                    return LineFeed;
                default:
                    return CarriageReturnLineFeed;
            }
        }

        internal static SyntaxTriviaList IncreaseIndentation(SyntaxTrivia trivia)
        {
            return TriviaList(trivia, SingleIndentation(trivia));
        }

        internal static SyntaxTrivia SingleIndentation(SyntaxTrivia trivia)
        {
            if (trivia.IsWhitespaceTrivia())
            {
                string s = trivia.ToString();

                int length = s.Length;

                if (length > 0)
                {
                    if (s.All(f => f == '\t'))
                    {
                        return Tab;
                    }
                    else if (s.All(f => f == ' '))
                    {
                        if (length % 4 == 0)
                            return Whitespace("    ");

                        if (length % 3 == 0)
                            return Whitespace("   ");

                        if (length % 2 == 0)
                            return Whitespace("  ");
                    }
                }
            }

            return DefaultIndentation;
        }

        internal static SyntaxTrivia DefaultIndentation { get; } = Whitespace("    ");
        #endregion Trivia

        #region Token
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken TildeToken()
        {
            return Token(SyntaxKind.TildeToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ExclamationToken()
        {
            return Token(SyntaxKind.ExclamationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken DollarToken()
        {
            return Token(SyntaxKind.DollarToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken PercentToken()
        {
            return Token(SyntaxKind.PercentToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken CaretToken()
        {
            return Token(SyntaxKind.CaretToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AmpersandToken()
        {
            return Token(SyntaxKind.AmpersandToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AsteriskToken()
        {
            return Token(SyntaxKind.AsteriskToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken OpenParenToken()
        {
            return Token(SyntaxKind.OpenParenToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken CloseParenToken()
        {
            return Token(SyntaxKind.CloseParenToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken MinusToken()
        {
            return Token(SyntaxKind.MinusToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken PlusToken()
        {
            return Token(SyntaxKind.PlusToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken EqualsToken()
        {
            return Token(SyntaxKind.EqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken OpenBraceToken()
        {
            return Token(SyntaxKind.OpenBraceToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken CloseBraceToken()
        {
            return Token(SyntaxKind.CloseBraceToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken OpenBracketToken()
        {
            return Token(SyntaxKind.OpenBracketToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken CloseBracketToken()
        {
            return Token(SyntaxKind.CloseBracketToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken BarToken()
        {
            return Token(SyntaxKind.BarToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken BackslashToken()
        {
            return Token(SyntaxKind.BackslashToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ColonToken()
        {
            return Token(SyntaxKind.ColonToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken SemicolonToken()
        {
            return Token(SyntaxKind.SemicolonToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken DoubleQuoteToken()
        {
            return Token(SyntaxKind.DoubleQuoteToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken SingleQuoteToken()
        {
            return Token(SyntaxKind.SingleQuoteToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken LessThanToken()
        {
            return Token(SyntaxKind.LessThanToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken CommaToken()
        {
            return Token(SyntaxKind.CommaToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken GreaterThanToken()
        {
            return Token(SyntaxKind.GreaterThanToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken DotToken()
        {
            return Token(SyntaxKind.DotToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken QuestionToken()
        {
            return Token(SyntaxKind.QuestionToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken HashToken()
        {
            return Token(SyntaxKind.HashToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken SlashToken()
        {
            return Token(SyntaxKind.SlashToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken SlashGreaterThanToken()
        {
            return Token(SyntaxKind.SlashGreaterThanToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken LessThanSlashToken()
        {
            return Token(SyntaxKind.LessThanSlashToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken XmlCommentStartToken()
        {
            return Token(SyntaxKind.XmlCommentStartToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken XmlCommentEndToken()
        {
            return Token(SyntaxKind.XmlCommentEndToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken XmlCDataStartToken()
        {
            return Token(SyntaxKind.XmlCDataStartToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken XmlCDataEndToken()
        {
            return Token(SyntaxKind.XmlCDataEndToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken XmlProcessingInstructionStartToken()
        {
            return Token(SyntaxKind.XmlProcessingInstructionStartToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken XmlProcessingInstructionEndToken()
        {
            return Token(SyntaxKind.XmlProcessingInstructionEndToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken BarBarToken()
        {
            return Token(SyntaxKind.BarBarToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AmpersandAmpersandToken()
        {
            return Token(SyntaxKind.AmpersandAmpersandToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken MinusMinusToken()
        {
            return Token(SyntaxKind.MinusMinusToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken PlusPlusToken()
        {
            return Token(SyntaxKind.PlusPlusToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ColonColonToken()
        {
            return Token(SyntaxKind.ColonColonToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken QuestionQuestionToken()
        {
            return Token(SyntaxKind.QuestionQuestionToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken MinusGreaterThanToken()
        {
            return Token(SyntaxKind.MinusGreaterThanToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ExclamationEqualsToken()
        {
            return Token(SyntaxKind.ExclamationEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken EqualsEqualsToken()
        {
            return Token(SyntaxKind.EqualsEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken EqualsGreaterThanToken()
        {
            return Token(SyntaxKind.EqualsGreaterThanToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken LessThanEqualsToken()
        {
            return Token(SyntaxKind.LessThanEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken LessThanLessThanToken()
        {
            return Token(SyntaxKind.LessThanLessThanToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken LessThanLessThanEqualsToken()
        {
            return Token(SyntaxKind.LessThanLessThanEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken GreaterThanEqualsToken()
        {
            return Token(SyntaxKind.GreaterThanEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken GreaterThanGreaterThanToken()
        {
            return Token(SyntaxKind.GreaterThanGreaterThanToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken GreaterThanGreaterThanEqualsToken()
        {
            return Token(SyntaxKind.GreaterThanGreaterThanEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken SlashEqualsToken()
        {
            return Token(SyntaxKind.SlashEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AsteriskEqualsToken()
        {
            return Token(SyntaxKind.AsteriskEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken BarEqualsToken()
        {
            return Token(SyntaxKind.BarEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AmpersandEqualsToken()
        {
            return Token(SyntaxKind.AmpersandEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken PlusEqualsToken()
        {
            return Token(SyntaxKind.PlusEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken MinusEqualsToken()
        {
            return Token(SyntaxKind.MinusEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken CaretEqualsToken()
        {
            return Token(SyntaxKind.CaretEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken PercentEqualsToken()
        {
            return Token(SyntaxKind.PercentEqualsToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken BoolKeyword()
        {
            return Token(SyntaxKind.BoolKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ByteKeyword()
        {
            return Token(SyntaxKind.ByteKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken SByteKeyword()
        {
            return Token(SyntaxKind.SByteKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ShortKeyword()
        {
            return Token(SyntaxKind.ShortKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken UShortKeyword()
        {
            return Token(SyntaxKind.UShortKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken IntKeyword()
        {
            return Token(SyntaxKind.IntKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken UIntKeyword()
        {
            return Token(SyntaxKind.UIntKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken LongKeyword()
        {
            return Token(SyntaxKind.LongKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ULongKeyword()
        {
            return Token(SyntaxKind.ULongKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken DoubleKeyword()
        {
            return Token(SyntaxKind.DoubleKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken FloatKeyword()
        {
            return Token(SyntaxKind.FloatKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken DecimalKeyword()
        {
            return Token(SyntaxKind.DecimalKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken StringKeyword()
        {
            return Token(SyntaxKind.StringKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken CharKeyword()
        {
            return Token(SyntaxKind.CharKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken VoidKeyword()
        {
            return Token(SyntaxKind.VoidKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ObjectKeyword()
        {
            return Token(SyntaxKind.ObjectKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken TypeOfKeyword()
        {
            return Token(SyntaxKind.TypeOfKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken SizeOfKeyword()
        {
            return Token(SyntaxKind.SizeOfKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken NullKeyword()
        {
            return Token(SyntaxKind.NullKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken TrueKeyword()
        {
            return Token(SyntaxKind.TrueKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken FalseKeyword()
        {
            return Token(SyntaxKind.FalseKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken IfKeyword()
        {
            return Token(SyntaxKind.IfKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ElseKeyword()
        {
            return Token(SyntaxKind.ElseKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken WhileKeyword()
        {
            return Token(SyntaxKind.WhileKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ForKeyword()
        {
            return Token(SyntaxKind.ForKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ForEachKeyword()
        {
            return Token(SyntaxKind.ForEachKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken DoKeyword()
        {
            return Token(SyntaxKind.DoKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken SwitchKeyword()
        {
            return Token(SyntaxKind.SwitchKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken CaseKeyword()
        {
            return Token(SyntaxKind.CaseKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken DefaultKeyword()
        {
            return Token(SyntaxKind.DefaultKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken TryKeyword()
        {
            return Token(SyntaxKind.TryKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken CatchKeyword()
        {
            return Token(SyntaxKind.CatchKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken FinallyKeyword()
        {
            return Token(SyntaxKind.FinallyKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken LockKeyword()
        {
            return Token(SyntaxKind.LockKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken GotoKeyword()
        {
            return Token(SyntaxKind.GotoKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken BreakKeyword()
        {
            return Token(SyntaxKind.BreakKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ContinueKeyword()
        {
            return Token(SyntaxKind.ContinueKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ReturnKeyword()
        {
            return Token(SyntaxKind.ReturnKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ThrowKeyword()
        {
            return Token(SyntaxKind.ThrowKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken PublicKeyword()
        {
            return Token(SyntaxKind.PublicKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken PrivateKeyword()
        {
            return Token(SyntaxKind.PrivateKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken InternalKeyword()
        {
            return Token(SyntaxKind.InternalKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ProtectedKeyword()
        {
            return Token(SyntaxKind.ProtectedKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken StaticKeyword()
        {
            return Token(SyntaxKind.StaticKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ReadOnlyKeyword()
        {
            return Token(SyntaxKind.ReadOnlyKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken SealedKeyword()
        {
            return Token(SyntaxKind.SealedKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ConstKeyword()
        {
            return Token(SyntaxKind.ConstKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken FixedKeyword()
        {
            return Token(SyntaxKind.FixedKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken StackAllocKeyword()
        {
            return Token(SyntaxKind.StackAllocKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken VolatileKeyword()
        {
            return Token(SyntaxKind.VolatileKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken NewKeyword()
        {
            return Token(SyntaxKind.NewKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken OverrideKeyword()
        {
            return Token(SyntaxKind.OverrideKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AbstractKeyword()
        {
            return Token(SyntaxKind.AbstractKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken VirtualKeyword()
        {
            return Token(SyntaxKind.VirtualKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken EventKeyword()
        {
            return Token(SyntaxKind.EventKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ExternKeyword()
        {
            return Token(SyntaxKind.ExternKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken RefKeyword()
        {
            return Token(SyntaxKind.RefKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken OutKeyword()
        {
            return Token(SyntaxKind.OutKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken InKeyword()
        {
            return Token(SyntaxKind.InKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken IsKeyword()
        {
            return Token(SyntaxKind.IsKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AsKeyword()
        {
            return Token(SyntaxKind.AsKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ParamsKeyword()
        {
            return Token(SyntaxKind.ParamsKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ArgListKeyword()
        {
            return Token(SyntaxKind.ArgListKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken MakeRefKeyword()
        {
            return Token(SyntaxKind.MakeRefKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken RefTypeKeyword()
        {
            return Token(SyntaxKind.RefTypeKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken RefValueKeyword()
        {
            return Token(SyntaxKind.RefValueKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ThisKeyword()
        {
            return Token(SyntaxKind.ThisKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken BaseKeyword()
        {
            return Token(SyntaxKind.BaseKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken NamespaceKeyword()
        {
            return Token(SyntaxKind.NamespaceKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken UsingKeyword()
        {
            return Token(SyntaxKind.UsingKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ClassKeyword()
        {
            return Token(SyntaxKind.ClassKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken StructKeyword()
        {
            return Token(SyntaxKind.StructKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken InterfaceKeyword()
        {
            return Token(SyntaxKind.InterfaceKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken EnumKeyword()
        {
            return Token(SyntaxKind.EnumKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken DelegateKeyword()
        {
            return Token(SyntaxKind.DelegateKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken CheckedKeyword()
        {
            return Token(SyntaxKind.CheckedKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken UncheckedKeyword()
        {
            return Token(SyntaxKind.UncheckedKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken UnsafeKeyword()
        {
            return Token(SyntaxKind.UnsafeKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken OperatorKeyword()
        {
            return Token(SyntaxKind.OperatorKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ExplicitKeyword()
        {
            return Token(SyntaxKind.ExplicitKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ImplicitKeyword()
        {
            return Token(SyntaxKind.ImplicitKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken YieldKeyword()
        {
            return Token(SyntaxKind.YieldKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken PartialKeyword()
        {
            return Token(SyntaxKind.PartialKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AliasKeyword()
        {
            return Token(SyntaxKind.AliasKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken GlobalKeyword()
        {
            return Token(SyntaxKind.GlobalKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AssemblyKeyword()
        {
            return Token(SyntaxKind.AssemblyKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ModuleKeyword()
        {
            return Token(SyntaxKind.ModuleKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken TypeKeyword()
        {
            return Token(SyntaxKind.TypeKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken FieldKeyword()
        {
            return Token(SyntaxKind.FieldKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken MethodKeyword()
        {
            return Token(SyntaxKind.MethodKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ParamKeyword()
        {
            return Token(SyntaxKind.ParamKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken PropertyKeyword()
        {
            return Token(SyntaxKind.PropertyKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken TypeVarKeyword()
        {
            return Token(SyntaxKind.TypeVarKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken GetKeyword()
        {
            return Token(SyntaxKind.GetKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken SetKeyword()
        {
            return Token(SyntaxKind.SetKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AddKeyword()
        {
            return Token(SyntaxKind.AddKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken RemoveKeyword()
        {
            return Token(SyntaxKind.RemoveKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken WhereKeyword()
        {
            return Token(SyntaxKind.WhereKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken FromKeyword()
        {
            return Token(SyntaxKind.FromKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken GroupKeyword()
        {
            return Token(SyntaxKind.GroupKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken JoinKeyword()
        {
            return Token(SyntaxKind.JoinKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken IntoKeyword()
        {
            return Token(SyntaxKind.IntoKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken LetKeyword()
        {
            return Token(SyntaxKind.LetKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ByKeyword()
        {
            return Token(SyntaxKind.ByKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken SelectKeyword()
        {
            return Token(SyntaxKind.SelectKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken OrderByKeyword()
        {
            return Token(SyntaxKind.OrderByKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken OnKeyword()
        {
            return Token(SyntaxKind.OnKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken EqualsKeyword()
        {
            return Token(SyntaxKind.EqualsKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AscendingKeyword()
        {
            return Token(SyntaxKind.AscendingKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken DescendingKeyword()
        {
            return Token(SyntaxKind.DescendingKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken NameOfKeyword()
        {
            return Token(SyntaxKind.NameOfKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AsyncKeyword()
        {
            return Token(SyntaxKind.AsyncKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken AwaitKeyword()
        {
            return Token(SyntaxKind.AwaitKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken WhenKeyword()
        {
            return Token(SyntaxKind.WhenKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ElifKeyword()
        {
            return Token(SyntaxKind.ElifKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken EndIfKeyword()
        {
            return Token(SyntaxKind.EndIfKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken RegionKeyword()
        {
            return Token(SyntaxKind.RegionKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken EndRegionKeyword()
        {
            return Token(SyntaxKind.EndRegionKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken DefineKeyword()
        {
            return Token(SyntaxKind.DefineKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken UndefKeyword()
        {
            return Token(SyntaxKind.UndefKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken WarningKeyword()
        {
            return Token(SyntaxKind.WarningKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ErrorKeyword()
        {
            return Token(SyntaxKind.ErrorKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken LineKeyword()
        {
            return Token(SyntaxKind.LineKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken PragmaKeyword()
        {
            return Token(SyntaxKind.PragmaKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken HiddenKeyword()
        {
            return Token(SyntaxKind.HiddenKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ChecksumKeyword()
        {
            return Token(SyntaxKind.ChecksumKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken DisableKeyword()
        {
            return Token(SyntaxKind.DisableKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken RestoreKeyword()
        {
            return Token(SyntaxKind.RestoreKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken ReferenceKeyword()
        {
            return Token(SyntaxKind.ReferenceKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken LoadKeyword()
        {
            return Token(SyntaxKind.LoadKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken InterpolatedStringStartToken()
        {
            return Token(SyntaxKind.InterpolatedStringStartToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken InterpolatedStringEndToken()
        {
            return Token(SyntaxKind.InterpolatedStringEndToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken InterpolatedVerbatimStringStartToken()
        {
            return Token(SyntaxKind.InterpolatedVerbatimStringStartToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken UnderscoreToken()
        {
            return Token(SyntaxKind.UnderscoreToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken OmittedTypeArgumentToken()
        {
            return Token(SyntaxKind.OmittedTypeArgumentToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken OmittedArraySizeExpressionToken()
        {
            return Token(SyntaxKind.OmittedArraySizeExpressionToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken EndOfDirectiveToken()
        {
            return Token(SyntaxKind.EndOfDirectiveToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken EndOfDocumentationCommentToken()
        {
            return Token(SyntaxKind.EndOfDocumentationCommentToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SyntaxToken EndOfFileToken()
        {
            return Token(SyntaxKind.EndOfFileToken);
        }

        private static SyntaxToken Token(SyntaxKind syntaxKind)
        {
            return SyntaxFactory.Token(syntaxKind);
        }
        #endregion Token

        #region Type
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedBoolType()
        {
            return PredefinedType(SyntaxKind.BoolKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedByteType()
        {
            return PredefinedType(SyntaxKind.ByteKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedSByteType()
        {
            return PredefinedType(SyntaxKind.SByteKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedIntType()
        {
            return PredefinedType(SyntaxKind.IntKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedUIntType()
        {
            return PredefinedType(SyntaxKind.UIntKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedShortType()
        {
            return PredefinedType(SyntaxKind.ShortKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedUShortType()
        {
            return PredefinedType(SyntaxKind.UShortKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedLongType()
        {
            return PredefinedType(SyntaxKind.LongKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedULongType()
        {
            return PredefinedType(SyntaxKind.ULongKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedFloatType()
        {
            return PredefinedType(SyntaxKind.FloatKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedDoubleType()
        {
            return PredefinedType(SyntaxKind.DoubleKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedDecimalType()
        {
            return PredefinedType(SyntaxKind.DecimalKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedStringType()
        {
            return PredefinedType(SyntaxKind.StringKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedCharType()
        {
            return PredefinedType(SyntaxKind.CharKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax PredefinedObjectType()
        {
            return PredefinedType(SyntaxKind.ObjectKeyword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PredefinedTypeSyntax VoidType()
        {
            return PredefinedType(SyntaxKind.VoidKeyword);
        }

        private static PredefinedTypeSyntax PredefinedType(SyntaxKind syntaxKind)
        {
            return SyntaxFactory.PredefinedType(Token(syntaxKind));
        }

        #endregion Type

        #region List
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static ArgumentListSyntax ArgumentList(params ArgumentSyntax[] arguments)
        {
            return SyntaxFactory.ArgumentList(SeparatedList(arguments));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static ArgumentListSyntax ArgumentList(ArgumentSyntax argument)
        {
            return SyntaxFactory.ArgumentList(SingletonSeparatedList(argument));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static BracketedArgumentListSyntax BracketedArgumentList(params ArgumentSyntax[] arguments)
        {
            return SyntaxFactory.BracketedArgumentList(SeparatedList(arguments));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static BracketedArgumentListSyntax BracketedArgumentList(ArgumentSyntax argument)
        {
            return SyntaxFactory.BracketedArgumentList(SingletonSeparatedList(argument));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static AttributeListSyntax AttributeList(AttributeSyntax attribute)
        {
            return SyntaxFactory.AttributeList(SingletonSeparatedList(attribute));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static AttributeListSyntax AttributeList(params AttributeSyntax[] attributes)
        {
            return SyntaxFactory.AttributeList(SeparatedList(attributes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeArguments"></param>
        /// <returns></returns>
        public static AttributeArgumentListSyntax AttributeArgumentList(params AttributeArgumentSyntax[] attributeArguments)
        {
            return SyntaxFactory.AttributeArgumentList(SeparatedList(attributeArguments));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeArgument"></param>
        /// <returns></returns>
        public static AttributeArgumentListSyntax AttributeArgumentList(AttributeArgumentSyntax attributeArgument)
        {
            return SyntaxFactory.AttributeArgumentList(SingletonSeparatedList(attributeArgument));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessors"></param>
        /// <returns></returns>
        public static AccessorListSyntax AccessorList(params AccessorDeclarationSyntax[] accessors)
        {
            return SyntaxFactory.AccessorList(List(accessors));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public static AccessorListSyntax AccessorList(AccessorDeclarationSyntax accessor)
        {
            return SyntaxFactory.AccessorList(SingletonList(accessor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static ParameterListSyntax ParameterList(ParameterSyntax parameter)
        {
            return SyntaxFactory.ParameterList(SingletonSeparatedList(parameter));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ParameterListSyntax ParameterList(params ParameterSyntax[] parameters)
        {
            return SyntaxFactory.ParameterList(SeparatedList(parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static BracketedParameterListSyntax BracketedParameterList(ParameterSyntax parameter)
        {
            return SyntaxFactory.BracketedParameterList(SingletonSeparatedList(parameter));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static BracketedParameterListSyntax BracketedParameterList(params ParameterSyntax[] parameters)
        {
            return SyntaxFactory.BracketedParameterList(SeparatedList(parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static TypeArgumentListSyntax TypeArgumentList(TypeSyntax argument)
        {
            return SyntaxFactory.TypeArgumentList(SingletonSeparatedList(argument));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static TypeArgumentListSyntax TypeArgumentList(params TypeSyntax[] arguments)
        {
            return SyntaxFactory.TypeArgumentList(SeparatedList(arguments));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static TypeParameterListSyntax TypeParameterList(TypeParameterSyntax parameter)
        {
            return SyntaxFactory.TypeParameterList(SingletonSeparatedList(parameter));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static TypeParameterListSyntax TypeParameterList(params TypeParameterSyntax[] parameters)
        {
            return SyntaxFactory.TypeParameterList(SeparatedList(parameters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static BaseListSyntax BaseList(BaseTypeSyntax type)
        {
            return SyntaxFactory.BaseList(SingletonSeparatedList(type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static BaseListSyntax BaseList(params BaseTypeSyntax[] types)
        {
            return SyntaxFactory.BaseList(SeparatedList(types));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colonToken"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static BaseListSyntax BaseList(SyntaxToken colonToken, BaseTypeSyntax baseType)
        {
            return SyntaxFactory.BaseList(colonToken, SingletonSeparatedList(baseType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colonToken"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static BaseListSyntax BaseList(SyntaxToken colonToken, params BaseTypeSyntax[] types)
        {
            return SyntaxFactory.BaseList(colonToken, SeparatedList(types));
        }
        #endregion List

        #region MemberDeclaration

        internal static NamespaceDeclarationSyntax NamespaceDeclaration(string name, MemberDeclarationSyntax member)
        {
            return NamespaceDeclaration(ParseName(name), member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static NamespaceDeclarationSyntax NamespaceDeclaration(NameSyntax name, MemberDeclarationSyntax member)
        {
            return NamespaceDeclaration(name, SingletonList(member));
        }

        internal static NamespaceDeclarationSyntax NamespaceDeclaration(string name, SyntaxList<MemberDeclarationSyntax> members)
        {
            return NamespaceDeclaration(ParseName(name), members);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public static NamespaceDeclarationSyntax NamespaceDeclaration(NameSyntax name, SyntaxList<MemberDeclarationSyntax> members)
        {
            return SyntaxFactory.NamespaceDeclaration(
                name,
                default(SyntaxList<ExternAliasDirectiveSyntax>),
                default(SyntaxList<UsingDirectiveSyntax>),
                members);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="identifier"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public static ClassDeclarationSyntax ClassDeclaration(SyntaxTokenList modifiers, string identifier, SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>))
        {
            return ClassDeclaration(modifiers, Identifier(identifier), members);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="identifier"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public static ClassDeclarationSyntax ClassDeclaration(SyntaxTokenList modifiers, SyntaxToken identifier, SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>))
        {
            return SyntaxFactory.ClassDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                identifier,
                default(TypeParameterListSyntax),
                default(BaseListSyntax),
                default(SyntaxList<TypeParameterConstraintClauseSyntax>),
                members);
        }

        internal static ClassDeclarationSyntax ClassDeclaration(StructDeclarationSyntax structDeclaration)
        {
            if (structDeclaration == null)
                throw new ArgumentNullException(nameof(structDeclaration));

            SyntaxToken keyword = structDeclaration.Keyword;

            return SyntaxFactory.ClassDeclaration(
                structDeclaration.AttributeLists,
                structDeclaration.Modifiers,
                SyntaxFactory.Token(keyword.LeadingTrivia, SyntaxKind.ClassKeyword, keyword.TrailingTrivia),
                structDeclaration.Identifier,
                structDeclaration.TypeParameterList,
                structDeclaration.BaseList,
                structDeclaration.ConstraintClauses,
                structDeclaration.OpenBraceToken,
                structDeclaration.Members,
                structDeclaration.CloseBraceToken,
                structDeclaration.SemicolonToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="identifier"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public static StructDeclarationSyntax StructDeclaration(SyntaxTokenList modifiers, string identifier, SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>))
        {
            return StructDeclaration(modifiers, Identifier(identifier), members);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="identifier"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public static StructDeclarationSyntax StructDeclaration(SyntaxTokenList modifiers, SyntaxToken identifier, SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>))
        {
            return SyntaxFactory.StructDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                identifier,
                default(TypeParameterListSyntax),
                default(BaseListSyntax),
                default(SyntaxList<TypeParameterConstraintClauseSyntax>),
                members);
        }

        internal static StructDeclarationSyntax StructDeclaration(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration == null)
                throw new ArgumentNullException(nameof(classDeclaration));

            SyntaxToken keyword = classDeclaration.Keyword;

            return SyntaxFactory.StructDeclaration(
                classDeclaration.AttributeLists,
                classDeclaration.Modifiers,
                SyntaxFactory.Token(keyword.LeadingTrivia, SyntaxKind.StructKeyword, keyword.TrailingTrivia),
                classDeclaration.Identifier,
                classDeclaration.TypeParameterList,
                classDeclaration.BaseList,
                classDeclaration.ConstraintClauses,
                classDeclaration.OpenBraceToken,
                classDeclaration.Members,
                classDeclaration.CloseBraceToken,
                classDeclaration.SemicolonToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="identifier"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public static InterfaceDeclarationSyntax InterfaceDeclaration(SyntaxTokenList modifiers, string identifier, SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>))
        {
            return InterfaceDeclaration(modifiers, Identifier(identifier), members);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="identifier"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public static InterfaceDeclarationSyntax InterfaceDeclaration(SyntaxTokenList modifiers, SyntaxToken identifier, SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>))
        {
            return SyntaxFactory.InterfaceDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                identifier,
                default(TypeParameterListSyntax),
                default(BaseListSyntax),
                default(SyntaxList<TypeParameterConstraintClauseSyntax>),
                members);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="identifier"></param>
        /// <param name="parameterList"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static ConstructorDeclarationSyntax ConstructorDeclaration(SyntaxTokenList modifiers, SyntaxToken identifier, ParameterListSyntax parameterList, BlockSyntax body)
        {
            return SyntaxFactory.ConstructorDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                identifier,
                parameterList,
                default(ConstructorInitializerSyntax),
                body);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="identifier"></param>
        /// <param name="parameterList"></param>
        /// <param name="expressionBody"></param>
        /// <returns></returns>
        public static ConstructorDeclarationSyntax ConstructorDeclaration(SyntaxTokenList modifiers, SyntaxToken identifier, ParameterListSyntax parameterList, ArrowExpressionClauseSyntax expressionBody)
        {
            return SyntaxFactory.ConstructorDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                identifier,
                parameterList,
                default(ConstructorInitializerSyntax),
                expressionBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EnumMemberDeclarationSyntax EnumMemberDeclaration(string name, ExpressionSyntax value)
        {
            return EnumMemberDeclaration(Identifier(name), value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EnumMemberDeclarationSyntax EnumMemberDeclaration(SyntaxToken identifier, ExpressionSyntax value)
        {
            return EnumMemberDeclaration(identifier, EqualsValueClause(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EnumMemberDeclarationSyntax EnumMemberDeclaration(string name, EqualsValueClauseSyntax value)
        {
            return EnumMemberDeclaration(Identifier(name), value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EnumMemberDeclarationSyntax EnumMemberDeclaration(SyntaxToken identifier, EqualsValueClauseSyntax value)
        {
            return SyntaxFactory.EnumMemberDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                identifier,
                value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FieldDeclarationSyntax FieldDeclaration(SyntaxTokenList modifiers, TypeSyntax type, string identifier, ExpressionSyntax value = null)
        {
            return FieldDeclaration(
                modifiers,
                type,
                Identifier(identifier),
                value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="initializer"></param>
        /// <returns></returns>
        public static FieldDeclarationSyntax FieldDeclaration(SyntaxTokenList modifiers, TypeSyntax type, string identifier, EqualsValueClauseSyntax initializer)
        {
            return FieldDeclaration(
                modifiers,
                type,
                Identifier(identifier),
                initializer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FieldDeclarationSyntax FieldDeclaration(SyntaxTokenList modifiers, TypeSyntax type, SyntaxToken identifier, ExpressionSyntax value = null)
        {
            return FieldDeclaration(
                modifiers,
                type,
                identifier,
                (value != null) ? EqualsValueClause(value) : default(EqualsValueClauseSyntax));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="initializer"></param>
        /// <returns></returns>
        public static FieldDeclarationSyntax FieldDeclaration(SyntaxTokenList modifiers, TypeSyntax type, SyntaxToken identifier, EqualsValueClauseSyntax initializer)
        {
            return SyntaxFactory.FieldDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                VariableDeclaration(
                    type,
                    identifier,
                    initializer));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="returnType"></param>
        /// <param name="identifier"></param>
        /// <param name="parameterList"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static MethodDeclarationSyntax MethodDeclaration(
            SyntaxTokenList modifiers,
            TypeSyntax returnType,
            SyntaxToken identifier,
            ParameterListSyntax parameterList,
            BlockSyntax body)
        {
            return SyntaxFactory.MethodDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                returnType,
                default(ExplicitInterfaceSpecifierSyntax),
                identifier,
                default(TypeParameterListSyntax),
                parameterList,
                default(SyntaxList<TypeParameterConstraintClauseSyntax>),
                body,
                default(ArrowExpressionClauseSyntax));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="returnType"></param>
        /// <param name="identifier"></param>
        /// <param name="parameterList"></param>
        /// <param name="expressionBody"></param>
        /// <returns></returns>
        public static MethodDeclarationSyntax MethodDeclaration(
            SyntaxTokenList modifiers,
            TypeSyntax returnType,
            SyntaxToken identifier,
            ParameterListSyntax parameterList,
            ArrowExpressionClauseSyntax expressionBody)
        {
            return SyntaxFactory.MethodDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                returnType,
                default(ExplicitInterfaceSpecifierSyntax),
                identifier,
                default(TypeParameterListSyntax),
                parameterList,
                default(SyntaxList<TypeParameterConstraintClauseSyntax>),
                default(BlockSyntax),
                expressionBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="accessorList"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PropertyDeclarationSyntax PropertyDeclaration(
            SyntaxTokenList modifiers,
            TypeSyntax type,
            SyntaxToken identifier,
            AccessorListSyntax accessorList,
            ExpressionSyntax value = null)
        {
            return SyntaxFactory.PropertyDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                type,
                default(ExplicitInterfaceSpecifierSyntax),
                identifier,
                accessorList,
                default(ArrowExpressionClauseSyntax),
                (value != null) ? EqualsValueClause(value) : default(EqualsValueClauseSyntax),
                (value != null) ? SemicolonToken() : default(SyntaxToken));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="expressionBody"></param>
        /// <returns></returns>
        public static PropertyDeclarationSyntax PropertyDeclaration(
            SyntaxTokenList modifiers,
            TypeSyntax type,
            SyntaxToken identifier,
            ArrowExpressionClauseSyntax expressionBody)
        {
            return SyntaxFactory.PropertyDeclaration(
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                type,
                default(ExplicitInterfaceSpecifierSyntax),
                identifier,
                default(AccessorListSyntax),
                expressionBody,
                default(EqualsValueClauseSyntax));
        }
        #endregion MemberDeclaration

        #region AccessorDeclaration
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax GetAccessorDeclaration(BlockSyntax body)
        {
            return GetAccessorDeclaration(default(SyntaxTokenList), body);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax GetAccessorDeclaration(SyntaxTokenList modifiers, BlockSyntax body)
        {
            return AccessorDeclaration(
                SyntaxKind.GetAccessorDeclaration,
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                body);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressionBody"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax GetAccessorDeclaration(ArrowExpressionClauseSyntax expressionBody)
        {
            return GetAccessorDeclaration(default(SyntaxTokenList), expressionBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="expressionBody"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax GetAccessorDeclaration(SyntaxTokenList modifiers, ArrowExpressionClauseSyntax expressionBody)
        {
            return AccessorDeclaration(
                SyntaxKind.GetAccessorDeclaration,
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                expressionBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax SetAccessorDeclaration(BlockSyntax body)
        {
            return SetAccessorDeclaration(default(SyntaxTokenList), body);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax SetAccessorDeclaration(SyntaxTokenList modifiers, BlockSyntax body)
        {
            return AccessorDeclaration(
                SyntaxKind.SetAccessorDeclaration,
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                body);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressionBody"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax SetAccessorDeclaration(ArrowExpressionClauseSyntax expressionBody)
        {
            return SetAccessorDeclaration(default(SyntaxTokenList), expressionBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="expressionBody"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax SetAccessorDeclaration(SyntaxTokenList modifiers, ArrowExpressionClauseSyntax expressionBody)
        {
            return AccessorDeclaration(
                SyntaxKind.SetAccessorDeclaration,
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                expressionBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax AddAccessorDeclaration(BlockSyntax body)
        {
            return AddAccessorDeclaration(default(SyntaxTokenList), body);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax AddAccessorDeclaration(SyntaxTokenList modifiers, BlockSyntax body)
        {
            return AccessorDeclaration(
                SyntaxKind.AddAccessorDeclaration,
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                body);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressionBody"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax AddAccessorDeclaration(ArrowExpressionClauseSyntax expressionBody)
        {
            return AddAccessorDeclaration(default(SyntaxTokenList), expressionBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="expressionBody"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax AddAccessorDeclaration(SyntaxTokenList modifiers, ArrowExpressionClauseSyntax expressionBody)
        {
            return AccessorDeclaration(
                SyntaxKind.AddAccessorDeclaration,
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                expressionBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax RemoveAccessorDeclaration(BlockSyntax body)
        {
            return RemoveAccessorDeclaration(default(SyntaxTokenList), body);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax RemoveAccessorDeclaration(SyntaxTokenList modifiers, BlockSyntax body)
        {
            return AccessorDeclaration(
                SyntaxKind.RemoveAccessorDeclaration,
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                body);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressionBody"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax RemoveAccessorDeclaration(ArrowExpressionClauseSyntax expressionBody)
        {
            return RemoveAccessorDeclaration(default(SyntaxTokenList), expressionBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="expressionBody"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax RemoveAccessorDeclaration(SyntaxTokenList modifiers, ArrowExpressionClauseSyntax expressionBody)
        {
            return AccessorDeclaration(
                SyntaxKind.RemoveAccessorDeclaration,
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                expressionBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax AutoGetAccessorDeclaration(SyntaxTokenList modifiers = default(SyntaxTokenList))
        {
            return AutoAccessorDeclaration(SyntaxKind.GetAccessorDeclaration, modifiers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax AutoSetAccessorDeclaration(SyntaxTokenList modifiers = default(SyntaxTokenList))
        {
            return AutoAccessorDeclaration(SyntaxKind.SetAccessorDeclaration, modifiers);
        }

        private static AccessorDeclarationSyntax AutoAccessorDeclaration(SyntaxKind kind, SyntaxTokenList modifiers = default(SyntaxTokenList))
        {
            return AccessorDeclaration(
                kind,
                default(SyntaxList<AttributeListSyntax>),
                modifiers,
                Token(AccessorDeclarationKeywordKind(kind)),
                default(BlockSyntax),
                SemicolonToken());
        }

        private static SyntaxKind AccessorDeclarationKeywordKind(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.GetAccessorDeclaration:
                    return SyntaxKind.GetKeyword;
                case SyntaxKind.SetAccessorDeclaration:
                    return SyntaxKind.SetKeyword;
                case SyntaxKind.AddAccessorDeclaration:
                    return SyntaxKind.AddKeyword;
                case SyntaxKind.RemoveAccessorDeclaration:
                    return SyntaxKind.RemoveKeyword;
                case SyntaxKind.UnknownAccessorDeclaration:
                    return SyntaxKind.IdentifierToken;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }
        #endregion AccessorDeclaration

        #region Statement
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LocalDeclarationStatementSyntax LocalDeclarationStatement(TypeSyntax type, string identifier, ExpressionSyntax value = null)
        {
            return LocalDeclarationStatement(type, Identifier(identifier), value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LocalDeclarationStatementSyntax LocalDeclarationStatement(TypeSyntax type, SyntaxToken identifier, ExpressionSyntax value = null)
        {
            VariableDeclaratorSyntax variableDeclarator = (value != null)
                ? VariableDeclarator(identifier, EqualsValueClause(value))
                : SyntaxFactory.VariableDeclarator(identifier);

            return SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                    type,
                    SingletonSeparatedList(variableDeclarator)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="initializer"></param>
        /// <returns></returns>
        public static LocalDeclarationStatementSyntax LocalDeclarationStatement(TypeSyntax type, string identifier, EqualsValueClauseSyntax initializer)
        {
            return LocalDeclarationStatement(type, Identifier(identifier), initializer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="initializer"></param>
        /// <returns></returns>
        public static LocalDeclarationStatementSyntax LocalDeclarationStatement(TypeSyntax type, SyntaxToken identifier, EqualsValueClauseSyntax initializer)
        {
            return SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                    type,
                    SingletonSeparatedList(VariableDeclarator(identifier, initializer))));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static YieldStatementSyntax YieldReturnStatement(ExpressionSyntax expression)
        {
            return YieldStatement(SyntaxKind.YieldReturnStatement, expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static YieldStatementSyntax YieldBreakStatement()
        {
            return YieldStatement(SyntaxKind.YieldBreakStatement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <param name="catch"></param>
        /// <param name="finally"></param>
        /// <returns></returns>
        public static TryStatementSyntax TryStatement(BlockSyntax block, CatchClauseSyntax @catch, FinallyClauseSyntax @finally = null)
        {
            return SyntaxFactory.TryStatement(block, SingletonList(@catch), @finally);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ExpressionStatementSyntax SimpleAssignmentStatement(ExpressionSyntax left, ExpressionSyntax right)
        {
            return ExpressionStatement(SimpleAssignmentExpression(left, right));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ExpressionStatementSyntax SimpleAssignmentStatement(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return ExpressionStatement(SimpleAssignmentExpression(left, operatorToken, right));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public static BlockSyntax Block(StatementSyntax statement)
        {
            return SyntaxFactory.Block(SingletonList(statement));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openBrace"></param>
        /// <param name="statement"></param>
        /// <param name="closeBrace"></param>
        /// <returns></returns>
        public static BlockSyntax Block(SyntaxToken openBrace, StatementSyntax statement, SyntaxToken closeBrace)
        {
            return SyntaxFactory.Block(openBrace, SingletonList(statement), closeBrace);
        }
        #endregion Statement

        #region BinaryExpression
        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax AddExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.AddExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax AddExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.AddExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax SubtractExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.SubtractExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax SubtractExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.SubtractExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax MultiplyExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.MultiplyExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax MultiplyExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.MultiplyExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax DivideExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.DivideExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax DivideExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.DivideExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax ModuloExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.ModuloExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax ModuloExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.ModuloExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax LeftShiftExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.LeftShiftExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax LeftShiftExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.LeftShiftExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax RightShiftExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.RightShiftExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax RightShiftExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.RightShiftExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax LogicalOrExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.LogicalOrExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax LogicalOrExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.LogicalOrExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax LogicalAndExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.LogicalAndExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax LogicalAndExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.LogicalAndExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax BitwiseOrExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.BitwiseOrExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax BitwiseOrExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.BitwiseOrExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax BitwiseAndExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.BitwiseAndExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax BitwiseAndExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.BitwiseAndExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax ExclusiveOrExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.ExclusiveOrExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax ExclusiveOrExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.ExclusiveOrExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax EqualsExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.EqualsExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax EqualsExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.EqualsExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax NotEqualsExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.NotEqualsExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax NotEqualsExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.NotEqualsExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax LessThanExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.LessThanExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax LessThanExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.LessThanExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax LessThanOrEqualExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.LessThanOrEqualExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax LessThanOrEqualExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.LessThanOrEqualExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax GreaterThanExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.GreaterThanExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax GreaterThanExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.GreaterThanExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax GreaterThanOrEqualExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.GreaterThanOrEqualExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax GreaterThanOrEqualExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.GreaterThanOrEqualExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax IsExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.IsExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax IsExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.IsExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax AsExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.AsExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax AsExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.AsExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax CoalesceExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.CoalesceExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BinaryExpressionSyntax CoalesceExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return BinaryExpression(SyntaxKind.CoalesceExpression, left, operatorToken, right);
        }
        #endregion BinaryExpression

        #region PrefixUnaryExpression
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax UnaryPlusExpression(ExpressionSyntax operand)
        {
            return PrefixUnaryExpression(SyntaxKind.UnaryPlusExpression, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="operatorToken"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax UnaryPlusExpression(ExpressionSyntax operand, SyntaxToken operatorToken)
        {
            return PrefixUnaryExpression(SyntaxKind.UnaryPlusExpression, operatorToken, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax UnaryMinusExpression(ExpressionSyntax operand)
        {
            return PrefixUnaryExpression(SyntaxKind.UnaryMinusExpression, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="operatorToken"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax UnaryMinusExpression(ExpressionSyntax operand, SyntaxToken operatorToken)
        {
            return PrefixUnaryExpression(SyntaxKind.UnaryMinusExpression, operatorToken, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax BitwiseNotExpression(ExpressionSyntax operand)
        {
            return PrefixUnaryExpression(SyntaxKind.BitwiseNotExpression, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="operatorToken"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax BitwiseNotExpression(ExpressionSyntax operand, SyntaxToken operatorToken)
        {
            return PrefixUnaryExpression(SyntaxKind.BitwiseNotExpression, operatorToken, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax LogicalNotExpression(ExpressionSyntax operand)
        {
            return PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="operatorToken"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax LogicalNotExpression(ExpressionSyntax operand, SyntaxToken operatorToken)
        {
            return PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, operatorToken, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax PreIncrementExpression(ExpressionSyntax operand)
        {
            return PrefixUnaryExpression(SyntaxKind.PreIncrementExpression, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="operatorToken"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax PreIncrementExpression(ExpressionSyntax operand, SyntaxToken operatorToken)
        {
            return PrefixUnaryExpression(SyntaxKind.PreIncrementExpression, operatorToken, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax PreDecrementExpression(ExpressionSyntax operand)
        {
            return PrefixUnaryExpression(SyntaxKind.PreDecrementExpression, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="operatorToken"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax PreDecrementExpression(ExpressionSyntax operand, SyntaxToken operatorToken)
        {
            return PrefixUnaryExpression(SyntaxKind.PreDecrementExpression, operatorToken, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax AddressOfExpression(ExpressionSyntax operand)
        {
            return PrefixUnaryExpression(SyntaxKind.AddressOfExpression, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="operatorToken"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax AddressOfExpression(ExpressionSyntax operand, SyntaxToken operatorToken)
        {
            return PrefixUnaryExpression(SyntaxKind.AddressOfExpression, operatorToken, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax PointerIndirectionExpression(ExpressionSyntax operand)
        {
            return PrefixUnaryExpression(SyntaxKind.PointerIndirectionExpression, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="operatorToken"></param>
        /// <returns></returns>
        public static PrefixUnaryExpressionSyntax PointerIndirectionExpression(ExpressionSyntax operand, SyntaxToken operatorToken)
        {
            return PrefixUnaryExpression(SyntaxKind.PointerIndirectionExpression, operatorToken, operand);
        }
        #endregion PrefixUnaryExpression

        #region PostfixUnaryExpression
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static PostfixUnaryExpressionSyntax PostIncrementExpression(ExpressionSyntax operand)
        {
            return PostfixUnaryExpression(SyntaxKind.PostIncrementExpression, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="operatorToken"></param>
        /// <returns></returns>
        public static PostfixUnaryExpressionSyntax PostIncrementExpression(ExpressionSyntax operand, SyntaxToken operatorToken)
        {
            return PostfixUnaryExpression(SyntaxKind.PostIncrementExpression, operand, operatorToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static PostfixUnaryExpressionSyntax PostDecrementExpression(ExpressionSyntax operand)
        {
            return PostfixUnaryExpression(SyntaxKind.PostDecrementExpression, operand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="operatorToken"></param>
        /// <returns></returns>
        public static PostfixUnaryExpressionSyntax PostDecrementExpression(ExpressionSyntax operand, SyntaxToken operatorToken)
        {
            return PostfixUnaryExpression(SyntaxKind.PostDecrementExpression, operand, operatorToken);
        }
        #endregion PostfixUnaryExpression

        #region AssignmentExpression
        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax SimpleAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax SimpleAssignmentExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax AddAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.AddAssignmentExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax AddAssignmentExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.AddAssignmentExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax SubtractAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.SubtractAssignmentExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax SubtractAssignmentExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.SubtractAssignmentExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax MultiplyAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.MultiplyAssignmentExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax MultiplyAssignmentExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.MultiplyAssignmentExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax DivideAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.DivideAssignmentExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax DivideAssignmentExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.DivideAssignmentExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax ModuloAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.ModuloAssignmentExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax ModuloAssignmentExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.ModuloAssignmentExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax AndAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.AndAssignmentExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax AndAssignmentExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.AndAssignmentExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax ExclusiveOrAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.ExclusiveOrAssignmentExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax ExclusiveOrAssignmentExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.ExclusiveOrAssignmentExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax OrAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.OrAssignmentExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax OrAssignmentExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.OrAssignmentExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax LeftShiftAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.LeftShiftAssignmentExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax LeftShiftAssignmentExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.LeftShiftAssignmentExpression, left, operatorToken, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax RightShiftAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.RightShiftAssignmentExpression, left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="operatorToken"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static AssignmentExpressionSyntax RightShiftAssignmentExpression(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            return AssignmentExpression(SyntaxKind.RightShiftAssignmentExpression, left, operatorToken, right);
        }
        #endregion AssignmentExpression

        #region LiteralExpression
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax StringLiteralExpression(string value)
        {
            return SyntaxFactory.LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                Literal(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax CharacterLiteralExpression(char value)
        {
            return SyntaxFactory.LiteralExpression(
                SyntaxKind.CharacterLiteralExpression,
                Literal(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax NumericLiteralExpression(int value)
        {
            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                Literal(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax NumericLiteralExpression(uint value)
        {
            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                Literal(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax NumericLiteralExpression(sbyte value)
        {
            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                Literal(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax NumericLiteralExpression(decimal value)
        {
            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                Literal(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax NumericLiteralExpression(double value)
        {
            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                Literal(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax NumericLiteralExpression(float value)
        {
            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                Literal(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax NumericLiteralExpression(long value)
        {
            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                Literal(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax NumericLiteralExpression(ulong value)
        {
            return SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                Literal(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static LiteralExpressionSyntax TrueLiteralExpression()
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static LiteralExpressionSyntax FalseLiteralExpression()
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax BooleanLiteralExpression(bool value)
        {
            return (value) ? TrueLiteralExpression() : FalseLiteralExpression();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static LiteralExpressionSyntax NullLiteralExpression()
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LiteralExpressionSyntax LiteralExpression(object value)
        {
            if (value == null)
                return NullLiteralExpression();

            if (value is bool)
                return ((bool)value) ? TrueLiteralExpression() : FalseLiteralExpression();

            if (value is char)
                return CharacterLiteralExpression((char)value);

            if (value is sbyte)
                return NumericLiteralExpression((sbyte)value);

            if (value is byte)
                return NumericLiteralExpression((byte)value);

            if (value is short)
                return NumericLiteralExpression((short)value);

            if (value is ushort)
                return NumericLiteralExpression((ushort)value);

            if (value is int)
                return NumericLiteralExpression((int)value);

            if (value is uint)
                return NumericLiteralExpression((uint)value);

            if (value is long)
                return NumericLiteralExpression((long)value);

            if (value is ulong)
                return NumericLiteralExpression((ulong)value);

            if (value is decimal)
                return NumericLiteralExpression((decimal)value);

            if (value is float)
                return NumericLiteralExpression((float)value);

            if (value is double)
                return NumericLiteralExpression((double)value);

            return StringLiteralExpression(value.ToString());
        }
        #endregion LiteralExpression

        #region Expression
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="argumentList"></param>
        /// <returns></returns>
        public static ObjectCreationExpressionSyntax ObjectCreationExpression(TypeSyntax type, ArgumentListSyntax argumentList)
        {
            return SyntaxFactory.ObjectCreationExpression(type, argumentList, default(InitializerExpressionSyntax));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MemberAccessExpressionSyntax SimpleMemberAccessExpression(ExpressionSyntax expression, SimpleNameSyntax name)
        {
            return MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="operatorToken"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MemberAccessExpressionSyntax SimpleMemberAccessExpression(ExpressionSyntax expression, SyntaxToken operatorToken, SimpleNameSyntax name)
        {
            return MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, operatorToken, name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static InvocationExpressionSyntax SimpleMemberInvocationExpression(ExpressionSyntax expression, SimpleNameSyntax name)
        {
            return InvocationExpression(SimpleMemberAccessExpression(expression, name));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="name"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static InvocationExpressionSyntax SimpleMemberInvocationExpression(ExpressionSyntax expression, SimpleNameSyntax name, ArgumentSyntax argument)
        {
            return SimpleMemberInvocationExpression(expression, name, ArgumentList(argument));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="name"></param>
        /// <param name="argumentList"></param>
        /// <returns></returns>
        public static InvocationExpressionSyntax SimpleMemberInvocationExpression(ExpressionSyntax expression, SimpleNameSyntax name, ArgumentListSyntax argumentList)
        {
            return InvocationExpression(SimpleMemberAccessExpression(expression, name), argumentList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static InvocationExpressionSyntax NameOfExpression(string identifier)
        {
            return NameOfExpression(IdentifierName(identifier));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static InvocationExpressionSyntax NameOfExpression(ExpressionSyntax expression)
        {
            return InvocationExpression(
                IdentifierName("nameof"),
                ArgumentList(SyntaxFactory.Argument(expression)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static InitializerExpressionSyntax ArrayInitializerExpression(SeparatedSyntaxList<ExpressionSyntax> expressions = default(SeparatedSyntaxList<ExpressionSyntax>))
        {
            return InitializerExpression(SyntaxKind.ArrayInitializerExpression, expressions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openBraceToken"></param>
        /// <param name="expressions"></param>
        /// <param name="closeBraceToken"></param>
        /// <returns></returns>
        public static InitializerExpressionSyntax ArrayInitializerExpression(SyntaxToken openBraceToken, SeparatedSyntaxList<ExpressionSyntax> expressions, SyntaxToken closeBraceToken)
        {
            return InitializerExpression(SyntaxKind.ArrayInitializerExpression, openBraceToken, expressions, closeBraceToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static InitializerExpressionSyntax CollectionInitializerExpression(SeparatedSyntaxList<ExpressionSyntax> expressions = default(SeparatedSyntaxList<ExpressionSyntax>))
        {
            return InitializerExpression(SyntaxKind.CollectionInitializerExpression, expressions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openBraceToken"></param>
        /// <param name="expressions"></param>
        /// <param name="closeBraceToken"></param>
        /// <returns></returns>
        public static InitializerExpressionSyntax CollectionInitializerExpression(SyntaxToken openBraceToken, SeparatedSyntaxList<ExpressionSyntax> expressions, SyntaxToken closeBraceToken)
        {
            return InitializerExpression(SyntaxKind.CollectionInitializerExpression, openBraceToken, expressions, closeBraceToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static InitializerExpressionSyntax ComplexElementInitializerExpression(SeparatedSyntaxList<ExpressionSyntax> expressions = default(SeparatedSyntaxList<ExpressionSyntax>))
        {
            return InitializerExpression(SyntaxKind.ComplexElementInitializerExpression, expressions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openBraceToken"></param>
        /// <param name="expressions"></param>
        /// <param name="closeBraceToken"></param>
        /// <returns></returns>
        public static InitializerExpressionSyntax ComplexElementInitializerExpression(SyntaxToken openBraceToken, SeparatedSyntaxList<ExpressionSyntax> expressions, SyntaxToken closeBraceToken)
        {
            return InitializerExpression(SyntaxKind.ComplexElementInitializerExpression, openBraceToken, expressions, closeBraceToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static InitializerExpressionSyntax ObjectInitializerExpression(SeparatedSyntaxList<ExpressionSyntax> expressions = default(SeparatedSyntaxList<ExpressionSyntax>))
        {
            return InitializerExpression(SyntaxKind.ObjectInitializerExpression, expressions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openBraceToken"></param>
        /// <param name="expressions"></param>
        /// <param name="closeBraceToken"></param>
        /// <returns></returns>
        public static InitializerExpressionSyntax ObjectInitializerExpression(SyntaxToken openBraceToken, SeparatedSyntaxList<ExpressionSyntax> expressions, SyntaxToken closeBraceToken)
        {
            return InitializerExpression(SyntaxKind.ObjectInitializerExpression, openBraceToken, expressions, closeBraceToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static CheckedExpressionSyntax CheckedExpression(ExpressionSyntax expression)
        {
            return SyntaxFactory.CheckedExpression(SyntaxKind.CheckedExpression, expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openParenToken"></param>
        /// <param name="expression"></param>
        /// <param name="closeParenToken"></param>
        /// <returns></returns>
        public static CheckedExpressionSyntax CheckedExpression(SyntaxToken openParenToken, ExpressionSyntax expression, SyntaxToken closeParenToken)
        {
            return SyntaxFactory.CheckedExpression(SyntaxKind.CheckedExpression, CheckedKeyword(), openParenToken, expression, closeParenToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static CheckedExpressionSyntax UncheckedExpression(ExpressionSyntax expression)
        {
            return SyntaxFactory.CheckedExpression(SyntaxKind.UncheckedExpression, expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openParenToken"></param>
        /// <param name="expression"></param>
        /// <param name="closeParenToken"></param>
        /// <returns></returns>
        public static CheckedExpressionSyntax UncheckedExpression(SyntaxToken openParenToken, ExpressionSyntax expression, SyntaxToken closeParenToken)
        {
            return SyntaxFactory.CheckedExpression(SyntaxKind.UncheckedExpression, UncheckedKeyword(), openParenToken, expression, closeParenToken);
        }
        #endregion Expression

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IdentifierNameSyntax VarType()
        {
            return IdentifierName("var");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="typeArgument"></param>
        /// <returns></returns>
        public static GenericNameSyntax GenericName(string identifier, TypeSyntax typeArgument)
        {
            return GenericName(Identifier(identifier), typeArgument);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="typeArgument"></param>
        /// <returns></returns>
        public static GenericNameSyntax GenericName(SyntaxToken identifier, TypeSyntax typeArgument)
        {
            return SyntaxFactory.GenericName(identifier, TypeArgumentList(typeArgument));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="initializer"></param>
        /// <returns></returns>
        public static VariableDeclaratorSyntax VariableDeclarator(string identifier, EqualsValueClauseSyntax initializer)
        {
            return VariableDeclarator(Identifier(identifier), initializer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="initializer"></param>
        /// <returns></returns>
        public static VariableDeclaratorSyntax VariableDeclarator(SyntaxToken identifier, EqualsValueClauseSyntax initializer)
        {
            return SyntaxFactory.VariableDeclarator(identifier, default(BracketedArgumentListSyntax), initializer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static VariableDeclarationSyntax VariableDeclaration(TypeSyntax type, string identifier, ExpressionSyntax value = null)
        {
            return VariableDeclaration(type, Identifier(identifier), value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static VariableDeclarationSyntax VariableDeclaration(TypeSyntax type, SyntaxToken identifier, ExpressionSyntax value = null)
        {
            if (value != null)
            {
                return VariableDeclaration(type, identifier, EqualsValueClause(value));
            }
            else
            {
                return VariableDeclaration(type, SyntaxFactory.VariableDeclarator(identifier));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="initializer"></param>
        /// <returns></returns>
        public static VariableDeclarationSyntax VariableDeclaration(TypeSyntax type, SyntaxToken identifier, EqualsValueClauseSyntax initializer)
        {
            return VariableDeclaration(
                type,
                VariableDeclarator(identifier, initializer));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public static VariableDeclarationSyntax VariableDeclaration(TypeSyntax type, VariableDeclaratorSyntax variable)
        {
            return SyntaxFactory.VariableDeclaration(type, SingletonSeparatedList(variable));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static UsingDirectiveSyntax UsingStaticDirective(NameSyntax name)
        {
            return UsingDirective(
                StaticKeyword(),
                default(NameEqualsSyntax),
                name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usingKeyword"></param>
        /// <param name="staticKeyword"></param>
        /// <param name="name"></param>
        /// <param name="semicolonToken"></param>
        /// <returns></returns>
        public static UsingDirectiveSyntax UsingStaticDirective(SyntaxToken usingKeyword, SyntaxToken staticKeyword, NameSyntax name, SyntaxToken semicolonToken)
        {
            return UsingDirective(
                usingKeyword,
                staticKeyword,
                default(NameEqualsSyntax),
                name,
                semicolonToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static AttributeSyntax Attribute(NameSyntax name, AttributeArgumentSyntax argument)
        {
            return SyntaxFactory.Attribute(
                name,
                AttributeArgumentList(argument));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameEquals"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static AttributeArgumentSyntax AttributeArgument(NameEqualsSyntax nameEquals, ExpressionSyntax expression)
        {
            return SyntaxFactory.AttributeArgument(
                nameEquals: nameEquals,
                nameColon: default(NameColonSyntax),
                expression: expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameColon"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static AttributeArgumentSyntax AttributeArgument(NameColonSyntax nameColon, ExpressionSyntax expression)
        {
            return SyntaxFactory.AttributeArgument(
                nameEquals: default(NameEqualsSyntax),
                nameColon: nameColon,
                expression: expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameColon"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static ArgumentSyntax Argument(NameColonSyntax nameColon, ExpressionSyntax expression)
        {
            return SyntaxFactory.Argument(nameColon, default(SyntaxToken), expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static ParameterSyntax Parameter(TypeSyntax type, string identifier, ExpressionSyntax @default = null)
        {
            return Parameter(type, Identifier(identifier), @default);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static ParameterSyntax Parameter(TypeSyntax type, SyntaxToken identifier, ExpressionSyntax @default = null)
        {
            if (@default != null)
            {
                return Parameter(type, identifier, EqualsValueClause(@default));
            }
            else
            {
                return SyntaxFactory.Parameter(
                    default(SyntaxList<AttributeListSyntax>),
                    default(SyntaxTokenList),
                    type,
                    identifier,
                    default(EqualsValueClauseSyntax));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifier"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static ParameterSyntax Parameter(TypeSyntax type, SyntaxToken identifier, EqualsValueClauseSyntax @default)
        {
            return SyntaxFactory.Parameter(
                default(SyntaxList<AttributeListSyntax>),
                default(SyntaxTokenList),
                type,
                identifier,
                @default);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="typeParameterConstraint"></param>
        /// <returns></returns>
        public static TypeParameterConstraintClauseSyntax TypeParameterConstraintClause(
            string name,
            TypeParameterConstraintSyntax typeParameterConstraint)
        {
            return TypeParameterConstraintClause(IdentifierName(name), typeParameterConstraint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifierName"></param>
        /// <param name="typeParameterConstraint"></param>
        /// <returns></returns>
        public static TypeParameterConstraintClauseSyntax TypeParameterConstraintClause(
            IdentifierNameSyntax identifierName,
            TypeParameterConstraintSyntax typeParameterConstraint)
        {
            return SyntaxFactory.TypeParameterConstraintClause(identifierName, SingletonSeparatedList(typeParameterConstraint));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ClassOrStructConstraintSyntax ClassConstraint()
        {
            return ClassOrStructConstraint(SyntaxKind.ClassConstraint, ClassKeyword());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ClassOrStructConstraintSyntax StructConstraint()
        {
            return ClassOrStructConstraint(SyntaxKind.StructConstraint, StructKeyword());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentList"></param>
        /// <returns></returns>
        public static ConstructorInitializerSyntax BaseConstructorInitializer(ArgumentListSyntax argumentList = null)
        {
            return ConstructorInitializer(SyntaxKind.BaseConstructorInitializer, argumentList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="semicolonToken"></param>
        /// <param name="argumentList"></param>
        /// <returns></returns>
        public static ConstructorInitializerSyntax BaseConstructorInitializer(SyntaxToken semicolonToken, ArgumentListSyntax argumentList)
        {
            return ConstructorInitializer(SyntaxKind.BaseConstructorInitializer, semicolonToken, BaseKeyword(), argumentList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentList"></param>
        /// <returns></returns>
        public static ConstructorInitializerSyntax ThisConstructorInitializer(ArgumentListSyntax argumentList = null)
        {
            return ConstructorInitializer(SyntaxKind.ThisConstructorInitializer, argumentList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="semicolonToken"></param>
        /// <param name="argumentList"></param>
        /// <returns></returns>
        public static ConstructorInitializerSyntax ThisConstructorInitializer(SyntaxToken semicolonToken, ArgumentListSyntax argumentList)
        {
            return ConstructorInitializer(SyntaxKind.ThisConstructorInitializer, semicolonToken, ThisKeyword(), argumentList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="switchLabel"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        public static SwitchSectionSyntax SwitchSection(SwitchLabelSyntax switchLabel, StatementSyntax statement)
        {
            return SwitchSection(switchLabel, SingletonList(statement));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="switchLabel"></param>
        /// <param name="statements"></param>
        /// <returns></returns>
        public static SwitchSectionSyntax SwitchSection(SwitchLabelSyntax switchLabel, SyntaxList<StatementSyntax> statements)
        {
            return SyntaxFactory.SwitchSection(SingletonList(switchLabel), statements);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="switchLabels"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        public static SwitchSectionSyntax SwitchSection(SyntaxList<SwitchLabelSyntax> switchLabels, StatementSyntax statement)
        {
            return SyntaxFactory.SwitchSection(switchLabels, SingletonList(statement));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public static SwitchSectionSyntax DefaultSwitchSection(StatementSyntax statement)
        {
            return DefaultSwitchSection(SingletonList(statement));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        public static SwitchSectionSyntax DefaultSwitchSection(SyntaxList<StatementSyntax> statements)
        {
            return SwitchSection(DefaultSwitchLabel(), statements);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static CompilationUnitSyntax CompilationUnit(MemberDeclarationSyntax member)
        {
            return CompilationUnit(
                default(SyntaxList<UsingDirectiveSyntax>),
                member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usings"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static CompilationUnitSyntax CompilationUnit(SyntaxList<UsingDirectiveSyntax> usings, MemberDeclarationSyntax member)
        {
            return CompilationUnit(
                usings,
                SingletonList(member));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usings"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public static CompilationUnitSyntax CompilationUnit(SyntaxList<UsingDirectiveSyntax> usings, SyntaxList<MemberDeclarationSyntax> members)
        {
            return SyntaxFactory.CompilationUnit(
                default(SyntaxList<ExternAliasDirectiveSyntax>),
                usings,
                default(SyntaxList<AttributeListSyntax>),
                members);
        }

        internal static SyntaxList<UsingDirectiveSyntax> UsingDirectives(string name)
        {
            return SingletonList(UsingDirective(ParseName(name)));
        }

        internal static SyntaxList<UsingDirectiveSyntax> UsingDirectives(params string[] names)
        {
            return List(names.Select(f => UsingDirective(ParseName(f))));
        }
    }
}
