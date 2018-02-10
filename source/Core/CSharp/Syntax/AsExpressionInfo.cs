// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Roslynator.CSharp.Syntax.SyntaxInfoHelpers;

namespace Roslynator.CSharp.Syntax
{
    public struct AsExpressionInfo : IEquatable<AsExpressionInfo>
    {
        private static AsExpressionInfo Default { get; } = new AsExpressionInfo();

        private AsExpressionInfo(
            BinaryExpressionSyntax asExpression,
            ExpressionSyntax expression,
            TypeSyntax type)
        {
            AsExpression = asExpression;
            Expression = expression;
            Type = type;
        }

        public BinaryExpressionSyntax AsExpression { get; }

        public ExpressionSyntax Expression { get; }

        public TypeSyntax Type { get; }

        public bool Success
        {
            get { return Expression != null; }
        }

        internal static AsExpressionInfo Create(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return CreateCore(
                Walk(node, walkDownParentheses) as BinaryExpressionSyntax,
                walkDownParentheses,
                allowMissing);
        }

        internal static AsExpressionInfo Create(
            BinaryExpressionSyntax binaryExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return CreateCore(binaryExpression, walkDownParentheses, allowMissing);
        }

        internal static AsExpressionInfo CreateCore(
            BinaryExpressionSyntax binaryExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            if (binaryExpression?.Kind() != SyntaxKind.AsExpression)
                return Default;

            ExpressionSyntax expression = Walk(binaryExpression.Left, walkDownParentheses);

            if (!Check(expression, allowMissing))
                return Default;

            var type = binaryExpression.Right as TypeSyntax;

            if (!Check(type, allowMissing))
                return Default;

            return new AsExpressionInfo(binaryExpression, expression, type);
        }

        public override string ToString()
        {
            return AsExpression?.ToString() ?? base.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is AsExpressionInfo other && Equals(other);
        }

        public bool Equals(AsExpressionInfo other)
        {
            return EqualityComparer<BinaryExpressionSyntax>.Default.Equals(AsExpression, other.AsExpression);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<BinaryExpressionSyntax>.Default.GetHashCode(AsExpression);
        }

        public static bool operator ==(AsExpressionInfo info1, AsExpressionInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(AsExpressionInfo info1, AsExpressionInfo info2)
        {
            return !(info1 == info2);
        }
    }
}