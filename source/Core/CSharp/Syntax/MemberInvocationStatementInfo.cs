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
    public readonly struct MemberInvocationStatementInfo : IEquatable<MemberInvocationStatementInfo>
    {
        private MemberInvocationStatementInfo(
            InvocationExpressionSyntax invocationExpression,
            ExpressionSyntax expression,
            SimpleNameSyntax name,
            ArgumentListSyntax argumentList)
        {
            InvocationExpression = invocationExpression;
            Expression = expression;
            Name = name;
            ArgumentList = argumentList;
        }

        private static MemberInvocationStatementInfo Default { get; } = new MemberInvocationStatementInfo();

        /// <summary>
        /// 
        /// </summary>
        public InvocationExpressionSyntax InvocationExpression { get; }

        /// <summary>
        /// 
        /// </summary>
        public ExpressionSyntax Expression { get; }

        /// <summary>
        /// 
        /// </summary>
        public SimpleNameSyntax Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public ArgumentListSyntax ArgumentList { get; }

        /// <summary>
        /// 
        /// </summary>
        public SeparatedSyntaxList<ArgumentSyntax> Arguments
        {
            get { return ArgumentList?.Arguments ?? default(SeparatedSyntaxList<ArgumentSyntax>); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ExpressionStatementSyntax Statement
        {
            get { return (ExpressionStatementSyntax)InvocationExpression?.Parent; }
        }

        /// <summary>
        /// 
        /// </summary>
        public MemberAccessExpressionSyntax MemberAccessExpression
        {
            get { return (MemberAccessExpressionSyntax)Expression?.Parent; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NameText
        {
            get { return Name?.Identifier.ValueText; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get { return InvocationExpression != null; }
        }

        internal static MemberInvocationStatementInfo Create(
            SyntaxNode node,
            bool allowMissing = false)
        {
            switch (node)
            {
                case ExpressionStatementSyntax expressionStatement:
                    return Create(expressionStatement, allowMissing);
                case InvocationExpressionSyntax invocationExpression:
                    return Create(invocationExpression, allowMissing);
            }

            return Default;
        }

        internal static MemberInvocationStatementInfo Create(
            ExpressionStatementSyntax expressionStatement,
            bool allowMissing = false)
        {
            if (!(expressionStatement?.Expression is InvocationExpressionSyntax invocationExpression))
                return Default;

            return CreateCore(invocationExpression, allowMissing);
        }

        internal static MemberInvocationStatementInfo Create(
            InvocationExpressionSyntax invocationExpression,
            bool allowMissing = false)
        {
            if (invocationExpression?.Parent?.IsKind(SyntaxKind.ExpressionStatement) != true)
                return Default;

            return CreateCore(invocationExpression, allowMissing);
        }

        private static MemberInvocationStatementInfo CreateCore(InvocationExpressionSyntax invocationExpression, bool allowMissing)
        {
            if (!(invocationExpression.Expression is MemberAccessExpressionSyntax memberAccessExpression))
                return Default;

            if (memberAccessExpression.Kind() != SyntaxKind.SimpleMemberAccessExpression)
                return Default;

            ExpressionSyntax expression = memberAccessExpression.Expression;

            if (!Check(expression, allowMissing))
                return Default;

            SimpleNameSyntax name = memberAccessExpression.Name;

            if (!Check(name, allowMissing))
                return Default;

            ArgumentListSyntax argumentList = invocationExpression.ArgumentList;

            if (argumentList == null)
                return Default;

            return new MemberInvocationStatementInfo(
                invocationExpression,
                expression,
                name,
                argumentList);
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
            return obj is MemberInvocationStatementInfo other && Equals(other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(MemberInvocationStatementInfo other)
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
        public static bool operator ==(MemberInvocationStatementInfo info1, MemberInvocationStatementInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(MemberInvocationStatementInfo info1, MemberInvocationStatementInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
