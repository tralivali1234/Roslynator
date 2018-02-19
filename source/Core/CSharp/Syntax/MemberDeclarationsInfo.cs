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
    /// 
    /// </summary>
    public readonly struct MemberDeclarationsInfo : IEquatable<MemberDeclarationsInfo>, IReadOnlyList<MemberDeclarationSyntax>
    {
        internal MemberDeclarationsInfo(MemberDeclarationSyntax declaration, SyntaxList<MemberDeclarationSyntax> members)
        {
            Members = members;
            Declaration = declaration;
        }

        private static MemberDeclarationsInfo Default { get; } = new MemberDeclarationsInfo();

        /// <summary>
        /// 
        /// </summary>
        public MemberDeclarationSyntax Declaration { get; }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxList<MemberDeclarationSyntax> Members { get; }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxKind Kind
        {
            get { return Declaration?.Kind() ?? SyntaxKind.None; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get { return Declaration != null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return Members.Count; }
        }

        /// <summary>Gets the element at the specified index in the read-only list.</summary>
        /// <returns>The element at the specified index in the read-only list.</returns>
        /// <param name="index">The zero-based index of the element to get. </param>
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
        /// 
        /// </summary>
        /// <returns></returns>
        public SyntaxList<MemberDeclarationSyntax>.Enumerator GetEnumerator()
        {
            return Members.GetEnumerator();
        }

        internal static MemberDeclarationsInfo Create(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            if (namespaceDeclaration == null)
                return Default;

            return new MemberDeclarationsInfo(namespaceDeclaration, namespaceDeclaration.Members);
        }

        internal static MemberDeclarationsInfo Create(TypeDeclarationSyntax typeDeclaration)
        {
            if (typeDeclaration == null)
                return Default;

            return new MemberDeclarationsInfo(typeDeclaration, typeDeclaration.Members);
        }

        internal static MemberDeclarationsInfo Create(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration == null)
                return Default;

            return new MemberDeclarationsInfo(classDeclaration, classDeclaration.Members);
        }

        internal static MemberDeclarationsInfo Create(StructDeclarationSyntax structDeclaration)
        {
            if (structDeclaration == null)
                return Default;

            return new MemberDeclarationsInfo(structDeclaration, structDeclaration.Members);
        }

        internal static MemberDeclarationsInfo Create(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (interfaceDeclaration == null)
                return Default;

            return new MemberDeclarationsInfo(interfaceDeclaration, interfaceDeclaration.Members);
        }

        internal static MemberDeclarationsInfo Create(SyntaxNode declaration)
        {
            switch (declaration?.Kind())
            {
                case SyntaxKind.NamespaceDeclaration:
                    {
                        var namespaceDeclaration = (NamespaceDeclarationSyntax)declaration;
                        return new MemberDeclarationsInfo(namespaceDeclaration, namespaceDeclaration.Members);
                    }
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                    {
                        var typeDeclaration = (TypeDeclarationSyntax)declaration;
                        return new MemberDeclarationsInfo(typeDeclaration, typeDeclaration.Members);
                    }
            }

            return Default;
        }

        internal static MemberDeclarationsInfo Create(MemberDeclarationsSelection selectedMembers)
        {
            return new MemberDeclarationsInfo(selectedMembers.Declaration, selectedMembers.UnderlyingList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo WithMembers(IEnumerable<MemberDeclarationSyntax> members)
        {
            return WithMembers(List(members));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo WithMembers(SyntaxList<MemberDeclarationSyntax> members)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Kind)
            {
                case SyntaxKind.NamespaceDeclaration:
                    {
                        var declaration = (NamespaceDeclarationSyntax)Declaration;
                        declaration = declaration.WithMembers(members);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }

                case SyntaxKind.ClassDeclaration:
                    {
                        var declaration = (ClassDeclarationSyntax)Declaration;
                        declaration = declaration.WithMembers(members);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }

                case SyntaxKind.StructDeclaration:
                    {
                        var declaration = (StructDeclarationSyntax)Declaration;
                        declaration = declaration.WithMembers(members);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }

                case SyntaxKind.InterfaceDeclaration:
                    {
                        var declaration = (InterfaceDeclarationSyntax)Declaration;
                        declaration = declaration.WithMembers(members);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo RemoveNode(SyntaxNode node, SyntaxRemoveOptions options)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Kind)
            {
                case SyntaxKind.NamespaceDeclaration:
                    {
                        var declaration = (NamespaceDeclarationSyntax)Declaration;
                        declaration = declaration.RemoveNode(node, options);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }

                case SyntaxKind.ClassDeclaration:
                    {
                        var declaration = (ClassDeclarationSyntax)Declaration;
                        declaration = declaration.RemoveNode(node, options);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }

                case SyntaxKind.StructDeclaration:
                    {
                        var declaration = (StructDeclarationSyntax)Declaration;
                        declaration = declaration.RemoveNode(node, options);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }

                case SyntaxKind.InterfaceDeclaration:
                    {
                        var declaration = (InterfaceDeclarationSyntax)Declaration;
                        declaration = declaration.RemoveNode(node, options);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo ReplaceNode(SyntaxNode oldNode, SyntaxNode newNode)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Kind)
            {
                case SyntaxKind.NamespaceDeclaration:
                    {
                        var declaration = (NamespaceDeclarationSyntax)Declaration;
                        declaration = declaration.ReplaceNode(oldNode, newNode);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }

                case SyntaxKind.ClassDeclaration:
                    {
                        var declaration = (ClassDeclarationSyntax)Declaration;
                        declaration = declaration.ReplaceNode(oldNode, newNode);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }

                case SyntaxKind.StructDeclaration:
                    {
                        var declaration = (StructDeclarationSyntax)Declaration;
                        declaration = declaration.ReplaceNode(oldNode, newNode);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }

                case SyntaxKind.InterfaceDeclaration:
                    {
                        var declaration = (InterfaceDeclarationSyntax)Declaration;
                        declaration = declaration.ReplaceNode(oldNode, newNode);
                        return new MemberDeclarationsInfo(declaration, declaration.Members);
                    }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo Add(MemberDeclarationSyntax member)
        {
            return WithMembers(Members.Add(member));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo AddRange(IEnumerable<MemberDeclarationSyntax> members)
        {
            return WithMembers(Members.AddRange(members));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return Members.Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MemberDeclarationSyntax First()
        {
            return Members.First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MemberDeclarationSyntax FirstOrDefault()
        {
            return Members.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int IndexOf(Func<MemberDeclarationSyntax, bool> predicate)
        {
            return Members.IndexOf(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public int IndexOf(MemberDeclarationSyntax member)
        {
            return Members.IndexOf(member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo Insert(int index, MemberDeclarationSyntax statement)
        {
            return WithMembers(Members.Insert(index, statement));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo InsertRange(int index, IEnumerable<MemberDeclarationSyntax> members)
        {
            return WithMembers(Members.InsertRange(index, members));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MemberDeclarationSyntax Last()
        {
            return Members.Last();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int LastIndexOf(Func<MemberDeclarationSyntax, bool> predicate)
        {
            return Members.LastIndexOf(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public int LastIndexOf(MemberDeclarationSyntax member)
        {
            return Members.LastIndexOf(member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MemberDeclarationSyntax LastOrDefault()
        {
            return Members.LastOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo Remove(MemberDeclarationSyntax member)
        {
            return WithMembers(Members.Remove(member));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo RemoveAt(int index)
        {
            return WithMembers(Members.RemoveAt(index));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeInList"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo Replace(MemberDeclarationSyntax nodeInList, MemberDeclarationSyntax newNode)
        {
            return WithMembers(Members.Replace(nodeInList, newNode));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo ReplaceAt(int index, MemberDeclarationSyntax newNode)
        {
            return WithMembers(Members.ReplaceAt(index, newNode));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeInList"></param>
        /// <param name="newNodes"></param>
        /// <returns></returns>
        public MemberDeclarationsInfo ReplaceRange(MemberDeclarationSyntax nodeInList, IEnumerable<MemberDeclarationSyntax> newNodes)
        {
            return WithMembers(Members.ReplaceRange(nodeInList, newNodes));
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (Declaration == null)
                throw new InvalidOperationException($"{nameof(MemberDeclarationsInfo)} is not initalized.");
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="string" /> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return Declaration?.ToString() ?? base.ToString();
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return obj is MemberDeclarationsInfo other && Equals(other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(MemberDeclarationsInfo other)
        {
            return EqualityComparer<MemberDeclarationSyntax>.Default.Equals(Declaration, other.Declaration);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<MemberDeclarationSyntax>.Default.GetHashCode(Declaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator ==(MemberDeclarationsInfo info1, MemberDeclarationsInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(MemberDeclarationsInfo info1, MemberDeclarationsInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
