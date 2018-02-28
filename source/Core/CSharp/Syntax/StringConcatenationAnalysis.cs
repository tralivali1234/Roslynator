// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    internal struct StringConcatenationAnalysis : IEquatable<StringConcatenationAnalysis>
    {
        private readonly Flags _flags;

        private StringConcatenationAnalysis(Flags flags)
        {
            _flags = flags;
        }

        public static StringConcatenationAnalysis Create(StringConcatenationExpressionInfo stringConcatenation)
        {
            var flags = Flags.None;

            foreach (ExpressionSyntax expression in stringConcatenation.Expressions())
            {
                StringLiteralExpressionInfo stringLiteral = SyntaxInfo.StringLiteralExpressionInfo(expression);

                if (stringLiteral.Success)
                {
                    if (stringLiteral.IsVerbatim)
                    {
                        flags |= Flags.ContainsVerbatimStringLiteral;
                    }
                    else
                    {
                        flags |= Flags.ContainsRegularStringLiteral;
                    }
                }
                else if (expression.Kind() == SyntaxKind.InterpolatedStringExpression)
                {
                    if (((InterpolatedStringExpressionSyntax)expression).IsVerbatim())
                    {
                        flags |= Flags.ContainsVerbatimInterpolatedString;
                    }
                    else
                    {
                        flags |= Flags.ContainsRegularInterpolatedString;
                    }
                }
                else
                {
                    flags |= Flags.ContainsUnspecifiedExpression;
                }
            }

            return new StringConcatenationAnalysis(flags);
        }

        public bool ContainsUnspecifiedExpression => (_flags & Flags.ContainsUnspecifiedExpression) != 0;

        public bool ContainsNonStringLiteral => (_flags & Flags.ContainsNonStringLiteral) != 0;

        public bool ContainsStringLiteral => (_flags & Flags.ContainsStringLiteral) != 0;

        public bool ContainsRegularStringLiteral => (_flags & Flags.ContainsRegularStringLiteral) != 0;

        public bool ContainsVerbatimStringLiteral => (_flags & Flags.ContainsVerbatimStringLiteral) != 0;

        public bool ContainsInterpolatedString => (_flags & Flags.ContainsInterpolatedString) != 0;

        public bool ContainsRegularInterpolatedString => (_flags & Flags.ContainsRegularInterpolatedString) != 0;

        public bool ContainsVerbatimInterpolatedString => (_flags & Flags.ContainsVerbatimInterpolatedString) != 0;

        public bool ContainsRegularExpression => (_flags & Flags.ContainsRegularExpression) != 0;

        public bool ContainsVerbatimExpression => (_flags & Flags.ContainsVerbatimExpression) != 0;

        public override bool Equals(object obj)
        {
            return obj is StringConcatenationAnalysis other && Equals(other);
        }

        public bool Equals(StringConcatenationAnalysis other)
        {
            return _flags == other._flags;
        }

        public override int GetHashCode()
        {
            return _flags.GetHashCode();
        }

        public static bool operator ==(StringConcatenationAnalysis analysis1, StringConcatenationAnalysis analysis2)
        {
            return analysis1.Equals(analysis2);
        }

        public static bool operator !=(StringConcatenationAnalysis analysis1, StringConcatenationAnalysis analysis2)
        {
            return !(analysis1 == analysis2);
        }

        [Flags]
        private enum Flags
        {
            None = 0,
            ContainsUnspecifiedExpression = 1,
            ContainsRegularStringLiteral = 2,
            ContainsVerbatimStringLiteral = 4,
            ContainsStringLiteral = ContainsRegularStringLiteral | ContainsVerbatimStringLiteral,
            ContainsRegularInterpolatedString = 8,
            ContainsRegularExpression = ContainsRegularStringLiteral | ContainsRegularInterpolatedString,
            ContainsVerbatimInterpolatedString = 16,
            ContainsVerbatimExpression = ContainsVerbatimStringLiteral | ContainsVerbatimInterpolatedString,
            ContainsInterpolatedString = ContainsRegularInterpolatedString | ContainsVerbatimInterpolatedString,
            ContainsNonStringLiteral = ContainsInterpolatedString | ContainsUnspecifiedExpression
        }
    }
}
