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
    public readonly struct SimpleAssignmentStatementInfo : IEquatable<SimpleAssignmentStatementInfo>
    {
        private SimpleAssignmentStatementInfo(
            AssignmentExpressionSyntax assignmentExpression,
            ExpressionSyntax left,
            ExpressionSyntax right)
        {
            AssignmentExpression = assignmentExpression;
            Left = left;
            Right = right;
        }

        private static SimpleAssignmentStatementInfo Default { get; } = new SimpleAssignmentStatementInfo();

        /// <summary>
        /// 
        /// </summary>
        public AssignmentExpressionSyntax AssignmentExpression { get; }

        /// <summary>
        /// 
        /// </summary>
        public ExpressionSyntax Left { get; }

        /// <summary>
        /// 
        /// </summary>
        public ExpressionSyntax Right { get; }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxToken OperatorToken
        {
            get { return AssignmentExpression?.OperatorToken ?? default(SyntaxToken); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ExpressionStatementSyntax Statement
        {
            get { return (ExpressionStatementSyntax)AssignmentExpression?.Parent; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get { return AssignmentExpression != null; }
        }

        internal static SimpleAssignmentStatementInfo Create(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            switch (node)
            {
                case ExpressionStatementSyntax expressionStatement:
                    return Create(expressionStatement, walkDownParentheses, allowMissing);
                case AssignmentExpressionSyntax assignmentExpression:
                    return Create(assignmentExpression, walkDownParentheses, allowMissing);
            }

            return Default;
        }

        internal static SimpleAssignmentStatementInfo Create(
            ExpressionStatementSyntax expressionStatement,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            ExpressionSyntax expression = expressionStatement?.Expression;

            if (!Check(expression, allowMissing))
                return Default;

            if (expression?.Kind() != SyntaxKind.SimpleAssignmentExpression)
                return Default;

            var assignmentExpression = (AssignmentExpressionSyntax)expression;

            ExpressionSyntax left = WalkAndCheck(assignmentExpression.Left, allowMissing, walkDownParentheses);

            if (left == null)
                return Default;

            ExpressionSyntax right = WalkAndCheck(assignmentExpression.Right, allowMissing, walkDownParentheses);

            if (right == null)
                return Default;

            return new SimpleAssignmentStatementInfo(assignmentExpression, left, right);
        }

        internal static SimpleAssignmentStatementInfo Create(
            AssignmentExpressionSyntax assignmentExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            if (assignmentExpression?.Kind() != SyntaxKind.SimpleAssignmentExpression)
                return Default;

            if (!(assignmentExpression.Parent is ExpressionStatementSyntax expressionStatement))
                return Default;

            ExpressionSyntax left = WalkAndCheck(assignmentExpression.Left, allowMissing, walkDownParentheses);

            if (left == null)
                return Default;

            ExpressionSyntax right = WalkAndCheck(assignmentExpression.Right, allowMissing, walkDownParentheses);

            if (right == null)
                return Default;

            return new SimpleAssignmentStatementInfo(assignmentExpression, left, right);
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="string" /> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return Statement?.ToString() ?? base.ToString();
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return obj is SimpleAssignmentStatementInfo other && Equals(other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(SimpleAssignmentStatementInfo other)
        {
            return EqualityComparer<ExpressionStatementSyntax>.Default.Equals(Statement, other.Statement);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<ExpressionStatementSyntax>.Default.GetHashCode(Statement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator ==(SimpleAssignmentStatementInfo info1, SimpleAssignmentStatementInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(SimpleAssignmentStatementInfo info1, SimpleAssignmentStatementInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
