// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp
{
    //XTODO: del
    /// <summary>
    /// A wrapper for either an <see cref="BlockSyntax"/> or an <see cref="ArrowExpressionClauseSyntax"/>.
    /// </summary>
    internal readonly struct BlockOrArrowExpressionClause : IEquatable<BlockOrArrowExpressionClause>
    {
        private readonly BlockSyntax _block;
        private readonly ArrowExpressionClauseSyntax _arrowExpressionClause;

        internal BlockOrArrowExpressionClause(SyntaxNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            SyntaxKind kind = node.Kind();

            if (kind == SyntaxKind.Block)
            {
                _block = (BlockSyntax)node;
                _arrowExpressionClause = null;
            }
            else if (kind == SyntaxKind.ArrowExpressionClause)
            {
                _arrowExpressionClause = (ArrowExpressionClauseSyntax)node;
                _block = null;
            }
            else
            {
                throw new ArgumentException("Node must be either a block or an arrow expression clause.", nameof(node));
            }
        }

        internal BlockOrArrowExpressionClause(BlockSyntax block)
        {
            _block = block ?? throw new ArgumentNullException(nameof(block));
            _arrowExpressionClause = null;
        }

        internal BlockOrArrowExpressionClause(ArrowExpressionClauseSyntax arrowExpressionClause)
        {
            _arrowExpressionClause = arrowExpressionClause ?? throw new ArgumentNullException(nameof(arrowExpressionClause));
            _block = null;
        }

        internal SyntaxNode Node
        {
            get { return _block ?? (SyntaxNode)_arrowExpressionClause; }
        }

        /// <summary>
        /// Gets an underlying node kind.
        /// </summary>
        public SyntaxKind Kind
        {
            get
            {
                if (_block != null)
                    return SyntaxKind.Block;

                if (_arrowExpressionClause != null)
                    return SyntaxKind.ArrowExpressionClause;

                return SyntaxKind.None;
            }
        }

        /// <summary>
        /// Determines whether this <see cref="BlockOrArrowExpressionClause"/> is wrapping a block.
        /// </summary>
        public bool IsBlock
        {
            get { return Kind == SyntaxKind.Block; }
        }

        /// <summary>
        /// Determines whether this <see cref=" BlockOrArrowExpressionClause"/> is wrapping an arrow expression clause.
        /// </summary>
        public bool IsArrowExpressionClause
        {
            get { return Kind == SyntaxKind.ArrowExpressionClause; }
        }

        public ExpressionSyntax Expression
        {
            get
            {
                if (_block != null)
                {
                    StatementSyntax statement = _block.Statements.SingleOrDefault(shouldThrow: false);

                    if (statement != null)
                    {
                        SyntaxKind kind = statement.Kind();

                        if (kind == SyntaxKind.ExpressionStatement)
                        {
                            return ((ExpressionStatementSyntax)statement).Expression;
                        }
                        else if (kind == SyntaxKind.ReturnStatement)
                        {
                            return ((ReturnStatementSyntax)statement).Expression;
                        }
                    }
                }
                else if (_arrowExpressionClause != null)
                {
                    return _arrowExpressionClause.Expression;
                }

                return null;
            }
        }

        /// <summary>
        /// The node that contains the underlying node in its <see cref="SyntaxNode.ChildNodes"/> collection.
        /// </summary>
        public SyntaxNode Parent
        {
            get { return _block?.Parent ?? _arrowExpressionClause?.Parent; }
        }

        public TextSpan Span
        {
            get
            {
                if (_block != null)
                    return _block.Span;

                if (_arrowExpressionClause != null)
                    return _arrowExpressionClause.Span;

                return default(TextSpan);
            }
        }

        public TextSpan FullSpan
        {
            get
            {
                if (_block != null)
                    return _block.FullSpan;

                if (_arrowExpressionClause != null)
                    return _arrowExpressionClause.FullSpan;

                return default(TextSpan);
            }
        }

        /// <summary>
        /// Returns the underlying block if this <see cref="BlockOrArrowExpressionClause"/> is wrapping block.
        /// </summary>
        /// <returns></returns>
        public BlockSyntax AsBlock()
        {
            return _block;
        }

        /// <summary>
        /// Returns the underlying arrow expression clause if this <see cref="ArrowExpressionClauseSyntax"/> is wrapping arrow expression clause.
        /// </summary>
        /// <returns></returns>
        public ArrowExpressionClauseSyntax AsArrowExpressionClause()
        {
            return _arrowExpressionClause;
        }

        /// <summary>
        /// Returns the string representation of the underlying node, not including its leading and trailing trivia.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Node?.ToString() ?? "";
        }

        /// <summary>
        /// Determines whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. </param>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            return obj is BlockOrArrowExpressionClause other
                && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(BlockOrArrowExpressionClause other)
        {
            return Node == other.Node;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return Node?.GetHashCode() ?? 0;
        }

#pragma warning disable CS1591
        public static bool operator ==(BlockOrArrowExpressionClause left, BlockOrArrowExpressionClause right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BlockOrArrowExpressionClause left, BlockOrArrowExpressionClause right)
        {
            return !left.Equals(right);
        }

        public static implicit operator BlockOrArrowExpressionClause(BlockSyntax block)
        {
            return new BlockOrArrowExpressionClause(block);
        }

        public static implicit operator BlockSyntax(BlockOrArrowExpressionClause blockOrArrowExpressionClause)
        {
            return blockOrArrowExpressionClause.AsBlock();
        }

        public static implicit operator BlockOrArrowExpressionClause(ArrowExpressionClauseSyntax arrowExpressionClause)
        {
            return new BlockOrArrowExpressionClause(arrowExpressionClause);
        }

        public static implicit operator ArrowExpressionClauseSyntax(BlockOrArrowExpressionClause blockOrArrowExpressionClause)
        {
            return blockOrArrowExpressionClause.AsArrowExpressionClause();
        }
#pragma warning restore CS1591
    }
}
