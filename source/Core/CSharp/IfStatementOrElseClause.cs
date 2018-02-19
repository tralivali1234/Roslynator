// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp
{
    /// <summary>
    /// 
    /// </summary>
    public readonly struct IfStatementOrElseClause : IEquatable<IfStatementOrElseClause>
    {
        internal IfStatementOrElseClause(SyntaxNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            SyntaxKind kind = node.Kind();

            if (kind != SyntaxKind.IfStatement
                && kind != SyntaxKind.ElseClause)
            {
                throw new ArgumentException("Node must be if statement or else clause.", nameof(node));
            }

            Kind = kind;
            Node = node;
        }

        internal IfStatementOrElseClause(IfStatementSyntax ifStatement)
        {
            Node = ifStatement ?? throw new ArgumentNullException(nameof(ifStatement));
            Kind = SyntaxKind.IfStatement;
        }

        internal IfStatementOrElseClause(ElseClauseSyntax elseClause)
        {
            Node = elseClause ?? throw new ArgumentNullException(nameof(elseClause));
            Kind = SyntaxKind.ElseClause;
        }

        internal SyntaxNode Node { get; }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxKind Kind { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsIf
        {
            get { return Kind == SyntaxKind.IfStatement; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsElse
        {
            get { return Kind == SyntaxKind.ElseClause; }
        }

        /// <summary>
        /// 
        /// </summary>
        public StatementSyntax Statement
        {
            get
            {
                var self = this;

                return (self.IsIf) ? self.AsIf().Statement : self.AsElse().Statement;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IfStatementSyntax AsIf()
        {
            return (IsIf) ? (IfStatementSyntax)Node : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ElseClauseSyntax AsElse()
        {
            return (IsElse) ? (ElseClauseSyntax)Node : null;
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="string" /> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return Node?.ToString() ?? base.ToString();
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IfStatementOrElseClause other)
        {
            return Node == other.Node;
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return obj is IfStatementOrElseClause other
                && Equals(other);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return Node?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(IfStatementOrElseClause left, IfStatementOrElseClause right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(IfStatementOrElseClause left, IfStatementOrElseClause right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifStatement"></param>
        public static implicit operator IfStatementOrElseClause(IfStatementSyntax ifStatement)
        {
            return new IfStatementOrElseClause(ifStatement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifOrElse"></param>
        public static implicit operator IfStatementSyntax(IfStatementOrElseClause ifOrElse)
        {
            return ifOrElse.AsIf();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elseClause"></param>
        public static implicit operator IfStatementOrElseClause(ElseClauseSyntax elseClause)
        {
            return new IfStatementOrElseClause(elseClause);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifOrElse"></param>
        public static implicit operator ElseClauseSyntax(IfStatementOrElseClause ifOrElse)
        {
            return ifOrElse.AsElse();
        }
    }
}
