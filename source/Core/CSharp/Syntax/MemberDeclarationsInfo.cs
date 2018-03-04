// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// Provides information about a list of member declarations.
    /// </summary>
    public readonly struct MemberDeclarationsInfo : IEquatable<MemberDeclarationsInfo>, IReadOnlyList<MemberDeclarationSyntax>
    {
        internal MemberDeclarationsInfo(SyntaxList<MemberDeclarationSyntax> members)
        {
            Members = members;
        }

        private static MemberDeclarationsInfo Default { get; } = new MemberDeclarationsInfo();

        /// <summary>
        /// The declaration that contains the members.
        /// </summary>
        public SyntaxNode Parent
        {
            get { return Members.FirstOrDefault()?.Parent; }
        }

        /// <summary>
        /// A list of members.
        /// </summary>
        public SyntaxList<MemberDeclarationSyntax> Members { get; }

        /// <summary>
        /// Determines whether this struct was initialized with an actual syntax.
        /// </summary>
        public bool Success
        {
            get { return Parent != null; }
        }

        /// <summary>
        /// A number of members in the list.
        /// </summary>
        public int Count
        {
            get { return Members.Count; }
        }

        /// <summary>
        /// Gets the member at the specified index in the list.
        /// </summary>
        /// <returns>The member at the specified index in the list.</returns>
        /// <param name="index">The zero-based index of the member to get. </param>
        public MemberDeclarationSyntax this[int index]
        {
            get { return Members[index]; }
        }

        IEnumerator<MemberDeclarationSyntax> IEnumerable<MemberDeclarationSyntax>.GetEnumerator()
        {
            return ((IReadOnlyList<MemberDeclarationSyntax>)Members).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyList<MemberDeclarationSyntax>)Members).GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for the list of members.
        /// </summary>
        /// <returns></returns>
        public SyntaxList<MemberDeclarationSyntax>.Enumerator GetEnumerator()
        {
            return Members.GetEnumerator();
        }

        internal static MemberDeclarationsInfo Create(CompilationUnitSyntax compilationUnit)
        {
            if (compilationUnit == null)
                return Default;

            return new MemberDeclarationsInfo(compilationUnit.Members);
        }

        internal static MemberDeclarationsInfo Create(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            if (namespaceDeclaration == null)
                return Default;

            return new MemberDeclarationsInfo(namespaceDeclaration.Members);
        }

        internal static MemberDeclarationsInfo Create(TypeDeclarationSyntax typeDeclaration)
        {
            if (typeDeclaration == null)
                return Default;

            return new MemberDeclarationsInfo(typeDeclaration.Members);
        }

        internal static MemberDeclarationsInfo Create(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration == null)
                return Default;

            return new MemberDeclarationsInfo(classDeclaration.Members);
        }

        internal static MemberDeclarationsInfo Create(StructDeclarationSyntax structDeclaration)
        {
            if (structDeclaration == null)
                return Default;

            return new MemberDeclarationsInfo(structDeclaration.Members);
        }

        internal static MemberDeclarationsInfo Create(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (interfaceDeclaration == null)
                return Default;

            return new MemberDeclarationsInfo(interfaceDeclaration.Members);
        }

        internal static MemberDeclarationsInfo Create(SyntaxNode node)
        {
            switch (node?.Kind())
            {
                case SyntaxKind.CompilationUnit:
                    {
                        var compilationUnit = (CompilationUnitSyntax)node;
                        return new MemberDeclarationsInfo(compilationUnit.Members);
                    }
                case SyntaxKind.NamespaceDeclaration:
                    {
                        var namespaceDeclaration = (NamespaceDeclarationSyntax)node;
                        return new MemberDeclarationsInfo(namespaceDeclaration.Members);
                    }
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                    {
                        var typeDeclaration = (TypeDeclarationSyntax)node;
                        return new MemberDeclarationsInfo(typeDeclaration.Members);
                    }
            }

            return Default;
        }

        internal static MemberDeclarationsInfo Create(MemberDeclarationsSelection selectedMembers)
        {
            return new MemberDeclarationsInfo(selectedMembers.UnderlyingList);
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the members updated.
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo WithMembers(IEnumerable<MemberDeclarationSyntax> members)
        {
            return WithMembers(List(members));
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the members updated.
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo WithMembers(SyntaxList<MemberDeclarationSyntax> members)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Parent.Kind())
            {
                case SyntaxKind.CompilationUnit:
                    {
                        var compilationUnit = (CompilationUnitSyntax)Parent;
                        compilationUnit = compilationUnit.WithMembers(members);
                        return new MemberDeclarationsInfo(compilationUnit.Members);
                    }
                case SyntaxKind.NamespaceDeclaration:
                    {
                        var declaration = (NamespaceDeclarationSyntax)Parent;
                        declaration = declaration.WithMembers(members);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }

                case SyntaxKind.ClassDeclaration:
                    {
                        var declaration = (ClassDeclarationSyntax)Parent;
                        declaration = declaration.WithMembers(members);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }

                case SyntaxKind.StructDeclaration:
                    {
                        var declaration = (StructDeclarationSyntax)Parent;
                        declaration = declaration.WithMembers(members);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }

                case SyntaxKind.InterfaceDeclaration:
                    {
                        var declaration = (InterfaceDeclarationSyntax)Parent;
                        declaration = declaration.WithMembers(members);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the specified node removed.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo RemoveNode(SyntaxNode node, SyntaxRemoveOptions options)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Parent.Kind())
            {
                case SyntaxKind.CompilationUnit:
                    {
                        var compilationUnit = (CompilationUnitSyntax)Parent;
                        compilationUnit = compilationUnit.RemoveNode(node, options);
                        return new MemberDeclarationsInfo(compilationUnit.Members);
                    }
                case SyntaxKind.NamespaceDeclaration:
                    {
                        var declaration = (NamespaceDeclarationSyntax)Parent;
                        declaration = declaration.RemoveNode(node, options);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }

                case SyntaxKind.ClassDeclaration:
                    {
                        var declaration = (ClassDeclarationSyntax)Parent;
                        declaration = declaration.RemoveNode(node, options);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }

                case SyntaxKind.StructDeclaration:
                    {
                        var declaration = (StructDeclarationSyntax)Parent;
                        declaration = declaration.RemoveNode(node, options);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }

                case SyntaxKind.InterfaceDeclaration:
                    {
                        var declaration = (InterfaceDeclarationSyntax)Parent;
                        declaration = declaration.RemoveNode(node, options);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the specified old node replaced with a new node.
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo ReplaceNode(SyntaxNode oldNode, SyntaxNode newNode)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Parent.Kind())
            {
                case SyntaxKind.CompilationUnit:
                    {
                        var compilationUnit = (CompilationUnitSyntax)Parent;
                        compilationUnit = compilationUnit.ReplaceNode(oldNode, newNode);
                        return new MemberDeclarationsInfo(compilationUnit.Members);
                    }
                case SyntaxKind.NamespaceDeclaration:
                    {
                        var declaration = (NamespaceDeclarationSyntax)Parent;
                        declaration = declaration.ReplaceNode(oldNode, newNode);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }

                case SyntaxKind.ClassDeclaration:
                    {
                        var declaration = (ClassDeclarationSyntax)Parent;
                        declaration = declaration.ReplaceNode(oldNode, newNode);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }

                case SyntaxKind.StructDeclaration:
                    {
                        var declaration = (StructDeclarationSyntax)Parent;
                        declaration = declaration.ReplaceNode(oldNode, newNode);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }

                case SyntaxKind.InterfaceDeclaration:
                    {
                        var declaration = (InterfaceDeclarationSyntax)Parent;
                        declaration = declaration.ReplaceNode(oldNode, newNode);
                        return new MemberDeclarationsInfo(declaration.Members);
                    }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the specified member added at the end.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo Add(MemberDeclarationSyntax member)
        {
            return WithMembers(Members.Add(member));
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the specified members added at the end.
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo AddRange(IEnumerable<MemberDeclarationSyntax> members)
        {
            return WithMembers(Members.AddRange(members));
        }

        /// <summary>
        /// True if the list has at least one member.
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return Members.Any();
        }

        /// <summary>
        /// The first member in the list.
        /// </summary>
        /// <returns></returns>
        public MemberDeclarationSyntax First()
        {
            return Members.First();
        }

        /// <summary>
        /// The first member in the list or null if the list is empty.
        /// </summary>
        /// <returns></returns>
        public MemberDeclarationSyntax FirstOrDefault()
        {
            return Members.FirstOrDefault();
        }

        /// <summary>
        /// Searches for a member that matches the predicate and returns returns zero-based index of the first occurrence in the list.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int IndexOf(Func<MemberDeclarationSyntax, bool> predicate)
        {
            return Members.IndexOf(predicate);
        }

        /// <summary>
        /// The index of the member in the list.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public int IndexOf(MemberDeclarationSyntax member)
        {
            return Members.IndexOf(member);
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the specified member inserted at the index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo Insert(int index, MemberDeclarationSyntax member)
        {
            return WithMembers(Members.Insert(index, member));
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the specified members inserted at the index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo InsertRange(int index, IEnumerable<MemberDeclarationSyntax> members)
        {
            return WithMembers(Members.InsertRange(index, members));
        }

        /// <summary>
        /// The last member in the list.
        /// </summary>
        /// <returns></returns>
        public MemberDeclarationSyntax Last()
        {
            return Members.Last();
        }

        /// <summary>
        /// The last member in the list or null if the list is empty.
        /// </summary>
        /// <returns></returns>
        public MemberDeclarationSyntax LastOrDefault()
        {
            return Members.LastOrDefault();
        }

        /// <summary>
        /// Searches for a member that matches the predicate and returns returns zero-based index of the last occurrence in the list.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int LastIndexOf(Func<MemberDeclarationSyntax, bool> predicate)
        {
            return Members.LastIndexOf(predicate);
        }

        /// <summary>
        /// Searches for a member and returns zero-based index of the last occurrence in the list.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public int LastIndexOf(MemberDeclarationSyntax member)
        {
            return Members.LastIndexOf(member);
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the specified member removed.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo Remove(MemberDeclarationSyntax member)
        {
            return WithMembers(Members.Remove(member));
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the member at the specified index removed.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo RemoveAt(int index)
        {
            return WithMembers(Members.RemoveAt(index));
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the specified member replaced with the new member.
        /// </summary>
        /// <param name="memberInList"></param>
        /// <param name="newMember"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo Replace(MemberDeclarationSyntax memberInList, MemberDeclarationSyntax newMember)
        {
            return WithMembers(Members.Replace(memberInList, newMember));
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the member at the specified index replaced with a new member.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newMember"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo ReplaceAt(int index, MemberDeclarationSyntax newMember)
        {
            return WithMembers(Members.ReplaceAt(index, newMember));
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationsInfo"/> with the specified member replaced with new members.
        /// </summary>
        /// <param name="memberInList"></param>
        /// <param name="newMembers"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo ReplaceRange(MemberDeclarationSyntax memberInList, IEnumerable<MemberDeclarationSyntax> newMembers)
        {
            return WithMembers(Members.ReplaceRange(memberInList, newMembers));
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (Parent == null)
                throw new InvalidOperationException($"{nameof(MemberDeclarationsInfo)} is not initalized.");
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
            return obj is MemberDeclarationsInfo other && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(MemberDeclarationsInfo other)
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

        public static bool operator ==(MemberDeclarationsInfo info1, MemberDeclarationsInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(MemberDeclarationsInfo info1, MemberDeclarationsInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
