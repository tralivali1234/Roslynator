// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Roslynator.CSharp.Syntax.SyntaxInfoHelpers;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// 
    /// </summary>
    public readonly struct ConditionalExpressionInfo : IEquatable<ConditionalExpressionInfo>
    {
        private ConditionalExpressionInfo(
            ExpressionSyntax condition,
            ExpressionSyntax whenTrue,
            ExpressionSyntax whenFalse)
        {
            Condition = condition;
            WhenTrue = whenTrue;
            WhenFalse = whenFalse;
        }

        private static ConditionalExpressionInfo Default { get; } = new ConditionalExpressionInfo();

        /// <summary>
        /// 
        /// </summary>
        public ConditionalExpressionSyntax ConditionalExpression
        {
            get { return Condition?.FirstAncestor<ConditionalExpressionSyntax>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ExpressionSyntax Condition { get; }

        /// <summary>
        /// 
        /// </summary>
        public ExpressionSyntax WhenTrue { get; }

        /// <summary>
        /// 
        /// </summary>
        public ExpressionSyntax WhenFalse { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get { return Condition != null; }
        }

        internal static ConditionalExpressionInfo Create(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return CreateCore(
                Walk(node, walkDownParentheses) as ConditionalExpressionSyntax,
                walkDownParentheses,
                allowMissing);
        }

        internal static ConditionalExpressionInfo Create(
            ConditionalExpressionSyntax conditionalExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return CreateCore(conditionalExpression, walkDownParentheses, allowMissing);
        }

        internal static ConditionalExpressionInfo CreateCore(
            ConditionalExpressionSyntax conditionalExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            if (conditionalExpression == null)
                return Default;

            ExpressionSyntax condition = WalkAndCheck(conditionalExpression.Condition, allowMissing, walkDownParentheses);

            if (condition == null)
                return Default;

            ExpressionSyntax whenTrue = WalkAndCheck(conditionalExpression.WhenTrue, allowMissing, walkDownParentheses);

            if (whenTrue == null)
                return Default;

            ExpressionSyntax whenFalse = WalkAndCheck(conditionalExpression.WhenFalse, allowMissing, walkDownParentheses);

            if (whenFalse == null)
                return Default;

            return new ConditionalExpressionInfo(condition, whenTrue, whenFalse);
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="string" /> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return ConditionalExpression?.ToString() ?? base.ToString();
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return obj is ConditionalExpressionInfo other && Equals(other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ConditionalExpressionInfo other)
        {
            return EqualityComparer<ConditionalExpressionSyntax>.Default.Equals(ConditionalExpression, other.ConditionalExpression);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<ConditionalExpressionSyntax>.Default.GetHashCode(ConditionalExpression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator ==(ConditionalExpressionInfo info1, ConditionalExpressionInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(ConditionalExpressionInfo info1, ConditionalExpressionInfo info2)
        {
            return !(info1 == info2);
        }
    }
}