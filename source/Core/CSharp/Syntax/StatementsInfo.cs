// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// 
    /// </summary>
    public readonly struct StatementsInfo : IEquatable<StatementsInfo>, IReadOnlyList<StatementSyntax>
    {
        internal StatementsInfo(BlockSyntax block)
        {
            Debug.Assert(block != null);

            Node = block;
            IsBlock = true;
            Statements = block.Statements;
        }

        internal StatementsInfo(SwitchSectionSyntax switchSection)
        {
            Debug.Assert(switchSection != null);

            Node = switchSection;
            IsBlock = false;
            Statements = switchSection.Statements;
        }

        private static StatementsInfo Default { get; } = new StatementsInfo();

        /// <summary>
        /// 
        /// </summary>
        public CSharpSyntaxNode Node { get; }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxList<StatementSyntax> Statements { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBlock { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSwitchSection
        {
            get { return Success && !IsBlock; }
        }

        /// <summary>
        /// 
        /// </summary>
        public BlockSyntax Block
        {
            get { return (IsBlock) ? (BlockSyntax)Node : null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SwitchSectionSyntax SwitchSection
        {
            get { return (IsSwitchSection) ? (SwitchSectionSyntax)Node : null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get { return Node != null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return Statements.Count; }
        }

        /// <summary>Gets the element at the specified index in the read-only list.</summary>
        /// <returns>The element at the specified index in the read-only list.</returns>
        /// <param name="index">The zero-based index of the element to get. </param>
        public StatementSyntax this[int index]
        {
            get { return Statements[index]; }
        }

        IEnumerator<StatementSyntax> IEnumerable<StatementSyntax>.GetEnumerator()
        {
            return ((IReadOnlyList<StatementSyntax>)Statements).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyList<StatementSyntax>)Statements).GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SyntaxList<StatementSyntax>.Enumerator GetEnumerator()
        {
            return Statements.GetEnumerator();
        }

        internal static StatementsInfo Create(BlockSyntax block)
        {
            if (block == null)
                return Default;

            return new StatementsInfo(block);
        }

        internal static StatementsInfo Create(SwitchSectionSyntax switchSection)
        {
            if (switchSection == null)
                return Default;

            return new StatementsInfo(switchSection);
        }

        internal static StatementsInfo Create(StatementSyntax statement)
        {
            if (statement == null)
                return Default;

            SyntaxNode parent = statement.Parent;

            switch (parent?.Kind())
            {
                case SyntaxKind.Block:
                    return new StatementsInfo((BlockSyntax)parent);
                case SyntaxKind.SwitchSection:
                    return new StatementsInfo((SwitchSectionSyntax)parent);
                default:
                    return Default;
            }
        }

        internal static StatementsInfo Create(StatementsSelection selectedStatements)
        {
            return Create(selectedStatements?.UnderlyingList.FirstOrDefault());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        public StatementsInfo WithStatements(IEnumerable<StatementSyntax> statements)
        {
            return WithStatements(List(statements));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        public StatementsInfo WithStatements(SyntaxList<StatementSyntax> statements)
        {
            ThrowInvalidOperationIfNotInitialized();

            if (IsBlock)
                return new StatementsInfo(Block.WithStatements(statements));

            if (IsSwitchSection)
                return new StatementsInfo(SwitchSection.WithStatements(statements));

            throw new InvalidOperationException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public StatementsInfo RemoveNode(SyntaxNode node, SyntaxRemoveOptions options)
        {
            ThrowInvalidOperationIfNotInitialized();

            if (IsBlock)
                return new StatementsInfo(Block.RemoveNode(node, options));

            if (IsSwitchSection)
                return new StatementsInfo(SwitchSection.RemoveNode(node, options));

            throw new InvalidOperationException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        public StatementsInfo ReplaceNode(SyntaxNode oldNode, SyntaxNode newNode)
        {
            ThrowInvalidOperationIfNotInitialized();

            if (IsBlock)
                return new StatementsInfo(Block.ReplaceNode(oldNode, newNode));

            if (IsSwitchSection)
                return new StatementsInfo(SwitchSection.ReplaceNode(oldNode, newNode));

            throw new InvalidOperationException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public StatementsInfo Add(StatementSyntax statement)
        {
            return WithStatements(Statements.Add(statement));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        public StatementsInfo AddRange(IEnumerable<StatementSyntax> statements)
        {
            return WithStatements(Statements.AddRange(statements));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return Statements.Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StatementSyntax First()
        {
            return Statements.First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StatementSyntax FirstOrDefault()
        {
            return Statements.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int IndexOf(Func<StatementSyntax, bool> predicate)
        {
            return Statements.IndexOf(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public int IndexOf(StatementSyntax statement)
        {
            return Statements.IndexOf(statement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        public StatementsInfo Insert(int index, StatementSyntax statement)
        {
            return WithStatements(Statements.Insert(index, statement));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="statements"></param>
        /// <returns></returns>
        public StatementsInfo InsertRange(int index, IEnumerable<StatementSyntax> statements)
        {
            return WithStatements(Statements.InsertRange(index, statements));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StatementSyntax Last()
        {
            return Statements.Last();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int LastIndexOf(Func<StatementSyntax, bool> predicate)
        {
            return Statements.LastIndexOf(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public int LastIndexOf(StatementSyntax statement)
        {
            return Statements.LastIndexOf(statement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StatementSyntax LastOrDefault()
        {
            return Statements.LastOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public StatementsInfo Remove(StatementSyntax statement)
        {
            return WithStatements(Statements.Remove(statement));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StatementsInfo RemoveAt(int index)
        {
            return WithStatements(Statements.RemoveAt(index));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeInList"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        public StatementsInfo Replace(StatementSyntax nodeInList, StatementSyntax newNode)
        {
            return WithStatements(Statements.Replace(nodeInList, newNode));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        public StatementsInfo ReplaceAt(int index, StatementSyntax newNode)
        {
            return WithStatements(Statements.ReplaceAt(index, newNode));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeInList"></param>
        /// <param name="newNodes"></param>
        /// <returns></returns>
        public StatementsInfo ReplaceRange(StatementSyntax nodeInList, IEnumerable<StatementSyntax> newNodes)
        {
            return WithStatements(Statements.ReplaceRange(nodeInList, newNodes));
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (Node == null)
                throw new InvalidOperationException($"{nameof(StatementsInfo)} is not initalized.");
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="string" /> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return Node?.ToString() ?? base.ToString();
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return obj is StatementsInfo other && Equals(other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(StatementsInfo other)
        {
            return EqualityComparer<CSharpSyntaxNode>.Default.Equals(Node, other.Node);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<CSharpSyntaxNode>.Default.GetHashCode(Node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator ==(StatementsInfo info1, StatementsInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(StatementsInfo info1, StatementsInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
