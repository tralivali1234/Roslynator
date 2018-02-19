// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// 
    /// </summary>
    public readonly struct IfStatementInfo : IEquatable<IfStatementInfo>
    {
        private readonly ImmutableArray<IfStatementOrElseClause> _nodes;

        private IfStatementInfo(IfStatementSyntax ifStatement)
        {
            _nodes = GetCascade(ifStatement);
        }

        private static ImmutableArray<IfStatementOrElseClause> GetCascade(IfStatementSyntax ifStatement)
        {
            ElseClauseSyntax elseClause = ifStatement.Else;

            if (elseClause == null)
                return ImmutableArray.Create<IfStatementOrElseClause>(ifStatement);

            ImmutableArray<IfStatementOrElseClause>.Builder builder = ImmutableArray.CreateBuilder<IfStatementOrElseClause>();

            builder.Add(ifStatement);

            while (true)
            {
                StatementSyntax statement = elseClause.Statement;

                if (statement?.Kind() == SyntaxKind.IfStatement)
                {
                    ifStatement = (IfStatementSyntax)statement;

                    builder.Add(ifStatement);

                    elseClause = ifStatement.Else;

                    if (elseClause == null)
                        return builder.ToImmutableArray();
                }
                else
                {
                    builder.Add(elseClause);
                    return builder.ToImmutableArray();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ImmutableArray<IfStatementOrElseClause> Nodes
        {
            get { return (!_nodes.IsDefault) ? _nodes : ImmutableArray<IfStatementOrElseClause>.Empty; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get { return Nodes.Any(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public IfStatementSyntax TopmostIf
        {
            get { return Nodes.FirstOrDefault().AsIf(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EndsWithIf
        {
            get { return Nodes.LastOrDefault().IsIf; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EndsWithElse
        {
            get { return Nodes.LastOrDefault().IsElse; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSimpleIf
        {
            get { return Nodes.Length == 1; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSimpleIfElse
        {
            get
            {
                return Nodes.Length == 2
                    && Nodes[0].IsIf
                    && Nodes[1].IsElse;
            }
        }

        internal static IfStatementInfo Create(IfStatementSyntax ifStatement)
        {
            if (ifStatement == null)
                return default(IfStatementInfo);

            return new IfStatementInfo(ifStatement);
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="string" /> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return Nodes.FirstOrDefault().Node?.ToString() ?? base.ToString();
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return obj is IfStatementInfo other && Equals(other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IfStatementInfo other)
        {
            return EqualityComparer<IfStatementSyntax>.Default.Equals(TopmostIf, other.TopmostIf);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<IfStatementSyntax>.Default.GetHashCode(TopmostIf);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator ==(IfStatementInfo info1, IfStatementInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(IfStatementInfo info1, IfStatementInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
