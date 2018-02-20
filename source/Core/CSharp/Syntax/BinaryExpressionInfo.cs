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
    public readonly struct BinaryExpressionInfo : IEquatable<BinaryExpressionInfo>
    {
        private BinaryExpressionInfo(
            BinaryExpressionSyntax binaryExpression,
            ExpressionSyntax left,
            ExpressionSyntax right)
        {
            BinaryExpression = binaryExpression;
            Left = left;
            Right = right;
        }

        private static BinaryExpressionInfo Default { get; } = new BinaryExpressionInfo();

        /// <summary>
        /// 
        /// </summary>
        public BinaryExpressionSyntax BinaryExpression { get; }

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
        public SyntaxKind Kind
        {
            get { return BinaryExpression?.Kind() ?? SyntaxKind.None; }
        }

        /// <summary>
        /// Determines whether this instance contains an underlying syntax.
        /// </summary>
        public bool Success
        {
            get { return BinaryExpression != null; }
        }

        internal static BinaryExpressionInfo Create(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return CreateCore(
                Walk(node, walkDownParentheses) as BinaryExpressionSyntax,
                walkDownParentheses,
                allowMissing);
        }

        internal static BinaryExpressionInfo Create(
            BinaryExpressionSyntax binaryExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return CreateCore(binaryExpression, walkDownParentheses, allowMissing);
        }

        internal static BinaryExpressionInfo CreateCore(
            BinaryExpressionSyntax binaryExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            if (binaryExpression == null)
                return Default;

            ExpressionSyntax left = Walk(binaryExpression.Left, walkDownParentheses);

            if (!Check(left, allowMissing))
                return Default;

            ExpressionSyntax right = Walk(binaryExpression.Right, walkDownParentheses);

            if (!Check(right, allowMissing))
                return Default;

            return new BinaryExpressionInfo(binaryExpression, left, right);
        }

        /// <summary>
        /// Returns the string representation of the underlying syntax, not including its leading and trailing trivia.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return BinaryExpression?.ToString() ?? base.ToString();
        }

        /// <summary>
        /// Determines whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. </param>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            return obj is BinaryExpressionInfo other && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(BinaryExpressionInfo other)
        {
            return EqualityComparer<BinaryExpressionSyntax>.Default.Equals(BinaryExpression, other.BinaryExpression);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<BinaryExpressionSyntax>.Default.GetHashCode(BinaryExpression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator ==(BinaryExpressionInfo info1, BinaryExpressionInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(BinaryExpressionInfo info1, BinaryExpressionInfo info2)
        {
            return !(info1 == info2);
        }
    }
}