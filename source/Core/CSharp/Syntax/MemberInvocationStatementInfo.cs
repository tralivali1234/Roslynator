// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// Provides information about invocation expression in an expression statement.
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
        /// The invocation expression.
        /// </summary>
        public InvocationExpressionSyntax InvocationExpression { get; }

        /// <summary>
        /// The expression that contains the member being invoked.
        /// </summary>
        public ExpressionSyntax Expression { get; }

        /// <summary>
        /// The name of the member being invoked.
        /// </summary>
        public SimpleNameSyntax Name { get; }

        /// <summary>
        /// The argument list.
        /// </summary>
        public ArgumentListSyntax ArgumentList { get; }

        /// <summary>
        /// A list of arguments.
        /// </summary>
        public SeparatedSyntaxList<ArgumentSyntax> Arguments
        {
            get { return ArgumentList?.Arguments ?? default(SeparatedSyntaxList<ArgumentSyntax>); }
        }

        /// <summary>
        /// The expression statement that contains the invocation expression.
        /// </summary>
        public ExpressionStatementSyntax Statement
        {
            get { return (ExpressionStatementSyntax)InvocationExpression?.Parent; }
        }

        /// <summary>
        /// The member access expression.
        /// </summary>
        public MemberAccessExpressionSyntax MemberAccessExpression
        {
            get { return (MemberAccessExpressionSyntax)Expression?.Parent; }
        }

        /// <summary>
        /// The name of the member being invoked.
        /// </summary>
        public string NameText
        {
            get { return Name?.Identifier.ValueText; }
        }

        /// <summary>
        /// Determines whether this struct was initialized with an actual syntax.
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

            return CreateImpl(invocationExpression, allowMissing);
        }

        internal static MemberInvocationStatementInfo Create(
            InvocationExpressionSyntax invocationExpression,
            bool allowMissing = false)
        {
            if (invocationExpression?.Parent?.IsKind(SyntaxKind.ExpressionStatement) != true)
                return Default;

            return CreateImpl(invocationExpression, allowMissing);
        }

        private static MemberInvocationStatementInfo CreateImpl(InvocationExpressionSyntax invocationExpression, bool allowMissing)
        {
            MemberInvocationExpressionInfo info = MemberInvocationExpressionInfo.Create(invocationExpression, allowMissing);

            if (!info.Success)
                return Default;

            return new MemberInvocationStatementInfo(
                invocationExpression,
                info.Expression,
                info.Name,
                info.ArgumentList);
        }

        /// <summary>
        /// Returns the string representation of the underlying syntax, not including its leading and trailing trivia.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Statement?.ToString() ?? "";
        }

        /// <summary>
        /// Determines whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. </param>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            return obj is MemberInvocationStatementInfo other && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(MemberInvocationStatementInfo other)
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

        public static bool operator ==(MemberInvocationStatementInfo info1, MemberInvocationStatementInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(MemberInvocationStatementInfo info1, MemberInvocationStatementInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
