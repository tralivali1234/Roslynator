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
    public readonly struct LocalDeclarationStatementInfo : IEquatable<LocalDeclarationStatementInfo>
    {
        private LocalDeclarationStatementInfo(
            LocalDeclarationStatementSyntax statement,
            VariableDeclarationSyntax declaration,
            TypeSyntax type)
        {
            Statement = statement;
            Declaration = declaration;
            Type = type;
        }

        private static LocalDeclarationStatementInfo Default { get; } = new LocalDeclarationStatementInfo();

        /// <summary>
        /// 
        /// </summary>
        public LocalDeclarationStatementSyntax Statement { get; }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxTokenList Modifiers
        {
            get { return Statement?.Modifiers ?? default(SyntaxTokenList); }
        }

        /// <summary>
        /// 
        /// </summary>
        public TypeSyntax Type { get; }

        /// <summary>
        /// 
        /// </summary>
        public VariableDeclarationSyntax Declaration { get; }

        /// <summary>
        /// 
        /// </summary>
        public SeparatedSyntaxList<VariableDeclaratorSyntax> Variables
        {
            get { return Declaration?.Variables ?? default(SeparatedSyntaxList<VariableDeclaratorSyntax>); }
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxToken SemicolonToken
        {
            get { return Statement?.SemicolonToken ?? default(SyntaxToken); }
        }

        /// <summary>
        /// Determines whether this instance contains an underlying syntax.
        /// </summary>
        public bool Success
        {
            get { return Statement != null; }
        }

        internal static LocalDeclarationStatementInfo Create(
            LocalDeclarationStatementSyntax localDeclarationStatement,
            bool allowMissing = false)
        {
            VariableDeclarationSyntax variableDeclaration = localDeclarationStatement?.Declaration;

            if (!Check(variableDeclaration, allowMissing))
                return Default;

            TypeSyntax type = variableDeclaration.Type;

            if (!Check(type, allowMissing))
                return Default;

            if (!variableDeclaration.Variables.Any())
                return Default;

            return new LocalDeclarationStatementInfo(localDeclarationStatement, variableDeclaration, type);
        }

        internal static LocalDeclarationStatementInfo Create(
            ExpressionSyntax value,
            bool allowMissing = false)
        {
            SyntaxNode node = value?.WalkUpParentheses().Parent;

            if (node?.Kind() != SyntaxKind.EqualsValueClause)
                return Default;

            node = node.Parent;

            if (node?.Kind() != SyntaxKind.VariableDeclarator)
                return Default;

            if (!(node?.Parent is VariableDeclarationSyntax declaration))
                return Default;

            TypeSyntax type = declaration.Type;

            if (!Check(type, allowMissing))
                return Default;

            if (!(declaration.Parent is LocalDeclarationStatementSyntax localDeclarationStatement))
                return Default;

            return new LocalDeclarationStatementInfo(localDeclarationStatement, declaration, type);
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
            return obj is LocalDeclarationStatementInfo other && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(LocalDeclarationStatementInfo other)
        {
            return EqualityComparer<LocalDeclarationStatementSyntax>.Default.Equals(Statement, other.Statement);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<LocalDeclarationStatementSyntax>.Default.GetHashCode(Statement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator ==(LocalDeclarationStatementInfo info1, LocalDeclarationStatementInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(LocalDeclarationStatementInfo info1, LocalDeclarationStatementInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
