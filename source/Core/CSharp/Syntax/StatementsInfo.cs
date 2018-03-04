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
    /// Provides information about a list of statements.
    /// </summary>
    public readonly struct StatementsInfo : IEquatable<StatementsInfo>, IReadOnlyList<StatementSyntax>
    {
        internal StatementsInfo(BlockSyntax block)
        {
            Debug.Assert(block != null);

            IsBlock = true;
            Statements = block.Statements;
        }

        internal StatementsInfo(SwitchSectionSyntax switchSection)
        {
            Debug.Assert(switchSection != null);

            IsBlock = false;
            Statements = switchSection.Statements;
        }

        private static StatementsInfo Default { get; } = new StatementsInfo();

        /// <summary>
        /// The node that contains the statements. It can be either a <see cref="BlockSyntax"/> or a <see cref="SwitchSectionSyntax"/>.
        /// </summary>
        public SyntaxNode Parent
        {
            get { return Statements.FirstOrDefault()?.Parent; }
        }

        /// <summary>
        /// The list of statements.
        /// </summary>
        public SyntaxList<StatementSyntax> Statements { get; }

        /// <summary>
        /// Determines whether the statements are contained in a <see cref="BlockSyntax"/>.
        /// </summary>
        public bool IsBlock { get; }

        /// <summary>
        /// Determines whether the statements are contained in a <see cref="SwitchSectionSyntax"/>.
        /// </summary>
        public bool IsSwitchSection
        {
            get { return Success && !IsBlock; }
        }

        /// <summary>
        /// Gets a block that contains the statements. Returns null if the statements are not contained in a block.
        /// </summary>
        public BlockSyntax Block
        {
            get { return (IsBlock) ? (BlockSyntax)Parent : null; }
        }

        /// <summary>
        /// Gets a switch section that contains the statements. Returns null if the statements are not contained in a switch section.
        /// </summary>
        public SwitchSectionSyntax SwitchSection
        {
            get { return (IsSwitchSection) ? (SwitchSectionSyntax)Parent : null; }
        }

        /// <summary>
        /// Determines whether this struct was initialized with an actual syntax.
        /// </summary>
        public bool Success
        {
            get { return Parent != null; }
        }

        /// <summary>
        /// The number of statement in the list.
        /// </summary>
        public int Count
        {
            get { return Statements.Count; }
        }

        /// <summary>
        /// Gets the statement at the specified index in the list.
        /// </summary>
        /// <returns>The statement at the specified index in the list.</returns>
        /// <param name="index">The zero-based index of the statement to get. </param>
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
        /// Gets the enumerator the list of statements.
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
        /// Creates a new <see cref="StatementsInfo"/> with the statements updated.
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        public StatementsInfo WithStatements(IEnumerable<StatementSyntax> statements)
        {
            return WithStatements(List(statements));
        }

        /// <summary>
        /// Creates a new <see cref="StatementsInfo"/> with the statements updated.
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
        /// Creates a new <see cref="StatementsInfo"/> with the specified node removed.
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
        /// Creates a new <see cref="StatementsInfo"/> with the specified old node replaced with a new node.
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
        /// Creates a new <see cref="StatementsInfo"/> with the specified statement added at the end.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public StatementsInfo Add(StatementSyntax statement)
        {
            return WithStatements(Statements.Add(statement));
        }

        /// <summary>
        /// Creates a new <see cref="StatementsInfo"/> with the specified statements added at the end.
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        public StatementsInfo AddRange(IEnumerable<StatementSyntax> statements)
        {
            return WithStatements(Statements.AddRange(statements));
        }

        /// <summary>
        /// True if the list has at least one statement.
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return Statements.Any();
        }

        /// <summary>
        /// The first statement in the list.
        /// </summary>
        /// <returns></returns>
        public StatementSyntax First()
        {
            return Statements.First();
        }

        /// <summary>
        /// The first statement in the list or null if the list is empty.
        /// </summary>
        /// <returns></returns>
        public StatementSyntax FirstOrDefault()
        {
            return Statements.FirstOrDefault();
        }

        /// <summary>
        /// Searches for a statement that matches the predicate and returns returns zero-based index of the first occurrence in the list.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int IndexOf(Func<StatementSyntax, bool> predicate)
        {
            return Statements.IndexOf(predicate);
        }

        /// <summary>
        /// The index of the statement in the list.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public int IndexOf(StatementSyntax statement)
        {
            return Statements.IndexOf(statement);
        }

        /// <summary>
        /// Creates a new <see cref="StatementsInfo"/> with the specified statement inserted at the index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        public StatementsInfo Insert(int index, StatementSyntax statement)
        {
            return WithStatements(Statements.Insert(index, statement));
        }

        /// <summary>
        /// Creates a new <see cref="StatementsInfo"/> with the specified statements inserted at the index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="statements"></param>
        /// <returns></returns>
        public StatementsInfo InsertRange(int index, IEnumerable<StatementSyntax> statements)
        {
            return WithStatements(Statements.InsertRange(index, statements));
        }

        /// <summary>
        /// The last statement in the list.
        /// </summary>
        /// <returns></returns>
        public StatementSyntax Last()
        {
            return Statements.Last();
        }

        /// <summary>
        /// The last statement in the list or null if the list is empty.
        /// </summary>
        /// <returns></returns>
        public StatementSyntax LastOrDefault()
        {
            return Statements.LastOrDefault();
        }

        /// <summary>
        /// Searches for a statement that matches the predicate and returns returns zero-based index of the last occurrence in the list.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int LastIndexOf(Func<StatementSyntax, bool> predicate)
        {
            return Statements.LastIndexOf(predicate);
        }

        /// <summary>
        /// Searches for a statement and returns zero-based index of the last occurrence in the list.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public int LastIndexOf(StatementSyntax statement)
        {
            return Statements.LastIndexOf(statement);
        }

        /// <summary>
        /// Creates a new <see cref="StatementsInfo"/> with the specified statement removed.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public StatementsInfo Remove(StatementSyntax statement)
        {
            return WithStatements(Statements.Remove(statement));
        }

        /// <summary>
        /// Creates a new <see cref="StatementsInfo"/> with the statement at the specified index removed.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StatementsInfo RemoveAt(int index)
        {
            return WithStatements(Statements.RemoveAt(index));
        }

        /// <summary>
        /// Creates a new <see cref="StatementsInfo"/> with the specified statement replaced with the new statement.
        /// </summary>
        /// <param name="statementInList"></param>
        /// <param name="newStatement"></param>
        /// <returns></returns>
        public StatementsInfo Replace(StatementSyntax statementInList, StatementSyntax newStatement)
        {
            return WithStatements(Statements.Replace(statementInList, newStatement));
        }

        /// <summary>
        /// Creates a new <see cref="StatementsInfo"/> with the statement at the specified index replaced with a new statement.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newStatement"></param>
        /// <returns></returns>
        public StatementsInfo ReplaceAt(int index, StatementSyntax newStatement)
        {
            return WithStatements(Statements.ReplaceAt(index, newStatement));
        }

        /// <summary>
        /// Creates a new <see cref="StatementsInfo"/> with the specified statement replaced with new statements.
        /// </summary>
        /// <param name="statementInList"></param>
        /// <param name="newStatements"></param>
        /// <returns></returns>
        public StatementsInfo ReplaceRange(StatementSyntax statementInList, IEnumerable<StatementSyntax> newStatements)
        {
            return WithStatements(Statements.ReplaceRange(statementInList, newStatements));
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (Parent == null)
                throw new InvalidOperationException($"{nameof(StatementsInfo)} is not initalized.");
        }

        /// <summary>
        /// Returns the string representation of the underlying syntax, not including its leading and trailing trivia.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Parent?.ToString() ?? "";
        }

        /// <summary>
        /// Determines whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. </param>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            return obj is StatementsInfo other && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(StatementsInfo other)
        {
            return EqualityComparer<SyntaxNode>.Default.Equals(Parent, other.Parent);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<SyntaxNode>.Default.GetHashCode(Parent);
        }

        public static bool operator ==(StatementsInfo info1, StatementsInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(StatementsInfo info1, StatementsInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
