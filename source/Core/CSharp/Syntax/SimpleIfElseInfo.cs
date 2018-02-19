// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Roslynator.CSharp.Syntax.SyntaxInfoHelpers;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// 
    /// </summary>
    public readonly struct SimpleIfElseInfo : IEquatable<SimpleIfElseInfo>
    {
        private SimpleIfElseInfo(
            IfStatementSyntax ifStatement,
            ExpressionSyntax condition,
            StatementSyntax whenTrue,
            StatementSyntax whenFalse)
        {
            IfStatement = ifStatement;
            Condition = condition;
            WhenTrue = whenTrue;
            WhenFalse = whenFalse;
        }

        private static SimpleIfElseInfo Default { get; } = new SimpleIfElseInfo();

        /// <summary>
        /// 
        /// </summary>
        public IfStatementSyntax IfStatement { get; }

        /// <summary>
        /// 
        /// </summary>
        public ExpressionSyntax Condition { get; }

        /// <summary>
        /// 
        /// </summary>
        public StatementSyntax WhenTrue { get; }

        /// <summary>
        /// 
        /// </summary>
        public StatementSyntax WhenFalse { get; }

        /// <summary>
        /// 
        /// </summary>
        public ElseClauseSyntax Else
        {
            get { return IfStatement?.Else; }
        }

        internal static SimpleIfElseInfo Create(
            IfStatementSyntax ifStatement,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            if (ifStatement?.IsParentKind(SyntaxKind.ElseClause) != false)
                return Default;

            StatementSyntax whenFalse = ifStatement.Else?.Statement;

            if (!Check(whenFalse, allowMissing))
                return Default;

            if (whenFalse.IsKind(SyntaxKind.IfStatement))
                return Default;

            StatementSyntax whenTrue = ifStatement.Statement;

            if (!Check(whenTrue, allowMissing))
                return Default;

            ExpressionSyntax condition = WalkAndCheck(ifStatement.Condition, allowMissing, walkDownParentheses);

            if (condition == null)
                return Default;

            return new SimpleIfElseInfo(ifStatement, condition, whenTrue, whenFalse);
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="string" /> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return IfStatement?.ToString() ?? base.ToString();
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return obj is SimpleIfElseInfo other && Equals(other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(SimpleIfElseInfo other)
        {
            return EqualityComparer<IfStatementSyntax>.Default.Equals(IfStatement, other.IfStatement);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<IfStatementSyntax>.Default.GetHashCode(IfStatement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator ==(SimpleIfElseInfo info1, SimpleIfElseInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(SimpleIfElseInfo info1, SimpleIfElseInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
