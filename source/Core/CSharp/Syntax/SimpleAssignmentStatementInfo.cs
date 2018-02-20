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
    /// Provides information about a simple assignment expression in an expression statement.
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
        /// The simple assignment expression.
        /// </summary>
        public AssignmentExpressionSyntax AssignmentExpression { get; }

        /// <summary>
        /// The expression on the left of the assignment operator.
        /// </summary>
        public ExpressionSyntax Left { get; }

        /// <summary>
        /// The expression of the right of the assignment operator.
        /// </summary>
        public ExpressionSyntax Right { get; }

        //TODO: EqualsEqualsToken
        /// <summary>
        /// The operator of the simple assignment expression.
        /// </summary>
        public SyntaxToken OperatorToken
        {
            get { return AssignmentExpression?.OperatorToken ?? default(SyntaxToken); }
        }

        /// <summary>
        /// The expression statement the simple assignment expression is contained in.
        /// </summary>
        public ExpressionStatementSyntax Statement
        {
            get { return (ExpressionStatementSyntax)AssignmentExpression?.Parent; }
        }

        /// <summary>
        /// Determines whether this instance contains an underlying syntax.
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

        /// <summary>
        /// Returns the string representation of the underlying syntax, not including its leading and trailing trivia.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Statement?.ToString() ?? base.ToString();
        }

        /// <summary>
        /// Determines whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. </param>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            return obj is SimpleAssignmentStatementInfo other && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(SimpleAssignmentStatementInfo other)
        {
            return EqualityComparer<ExpressionStatementSyntax>.Default.Equals(Statement, other.Statement);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
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
