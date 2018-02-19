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
    public readonly struct SingleParameterLambdaExpressionInfo : IEquatable<SingleParameterLambdaExpressionInfo>
    {
        private SingleParameterLambdaExpressionInfo(
            LambdaExpressionSyntax lambdaExpression,
            ParameterSyntax parameter,
            CSharpSyntaxNode body)
        {
            LambdaExpression = lambdaExpression;
            Parameter = parameter;
            Body = body;
        }

        private static SingleParameterLambdaExpressionInfo Default { get; } = new SingleParameterLambdaExpressionInfo();

        /// <summary>
        /// 
        /// </summary>
        public LambdaExpressionSyntax LambdaExpression { get; }

        /// <summary>
        /// 
        /// </summary>
        public ParameterSyntax Parameter { get; }

        /// <summary>
        /// 
        /// </summary>
        public CSharpSyntaxNode Body { get; }

        /// <summary>
        /// 
        /// </summary>
        public ParameterListSyntax ParameterList
        {
            get { return (IsParenthesizedLambda) ? (ParameterListSyntax)Parameter.Parent : null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSimpleLambda
        {
            get { return LambdaExpression?.Kind() == SyntaxKind.SimpleLambdaExpression; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsParenthesizedLambda
        {
            get { return LambdaExpression?.Kind() == SyntaxKind.ParenthesizedLambdaExpression; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get { return LambdaExpression != null; }
        }

        internal static SingleParameterLambdaExpressionInfo Create(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return CreateCore(Walk(node, walkDownParentheses) as LambdaExpressionSyntax, allowMissing);
        }

        internal static SingleParameterLambdaExpressionInfo Create(
            LambdaExpressionSyntax lambdaExpression,
            bool allowMissing = false)
        {
            return CreateCore(lambdaExpression, allowMissing);
        }

        internal static SingleParameterLambdaExpressionInfo CreateCore(
            LambdaExpressionSyntax lambdaExpression,
            bool allowMissing = false)
        {
            switch (lambdaExpression?.Kind())
            {
                case SyntaxKind.SimpleLambdaExpression:
                    {
                        var simpleLambda = (SimpleLambdaExpressionSyntax)lambdaExpression;

                        ParameterSyntax parameter = simpleLambda.Parameter;

                        if (!Check(parameter, allowMissing))
                            break;

                        CSharpSyntaxNode body = simpleLambda.Body;

                        if (!Check(body, allowMissing))
                            break;

                        return new SingleParameterLambdaExpressionInfo(simpleLambda, parameter, body);
                    }
                case SyntaxKind.ParenthesizedLambdaExpression:
                    {
                        var parenthesizedLambda = (ParenthesizedLambdaExpressionSyntax)lambdaExpression;

                        ParameterSyntax parameter = parenthesizedLambda
                            .ParameterList?
                            .Parameters
                            .SingleOrDefault(shouldthrow: false);

                        if (!Check(parameter, allowMissing))
                            break;

                        CSharpSyntaxNode body = parenthesizedLambda.Body;

                        if (!Check(body, allowMissing))
                            break;

                        return new SingleParameterLambdaExpressionInfo(parenthesizedLambda, parameter, body);
                    }
            }

            return Default;
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="string" /> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return LambdaExpression?.ToString() ?? base.ToString();
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return obj is SingleParameterLambdaExpressionInfo other && Equals(other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(SingleParameterLambdaExpressionInfo other)
        {
            return EqualityComparer<LambdaExpressionSyntax>.Default.Equals(LambdaExpression, other.LambdaExpression);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<LambdaExpressionSyntax>.Default.GetHashCode(LambdaExpression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator ==(SingleParameterLambdaExpressionInfo info1, SingleParameterLambdaExpressionInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(SingleParameterLambdaExpressionInfo info1, SingleParameterLambdaExpressionInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
