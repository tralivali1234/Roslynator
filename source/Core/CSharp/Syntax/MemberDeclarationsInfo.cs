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
    public struct MemberDeclarationsInfo : IEquatable<MemberDeclarationsInfo>, IReadOnlyList<MemberDeclarationSyntax>
    {
        internal MemberDeclarationsInfo(MemberDeclarationSyntax declaration, SyntaxList<MemberDeclarationSyntax> members)
        {
            Members = members;
            Declaration = declaration;
        }

        private static MemberDeclarationsInfo Default { get; } = new MemberDeclarationsInfo();

        public MemberDeclarationSyntax Declaration { get; }

        public SyntaxList<MemberDeclarationSyntax> Members { get; }

        public SyntaxKind Kind
        {
            get { return Declaration?.Kind() ?? SyntaxKind.None; }
        }

        public bool Success
        {
            get { return Declaration != null; }
        }

        public int Count
        {
            get { return Members.Count; }
        }

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

        public MemberDeclarationsInfo WithMembers(IEnumerable<MemberDeclarationSyntax> members)
        {
            return WithMembers(List(members));
        }

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

        public MemberDeclarationsInfo Add(MemberDeclarationSyntax member)
        {
            return WithMembers(Members.Add(member));
        }

        public MemberDeclarationsInfo AddRange(IEnumerable<MemberDeclarationSyntax> members)
        {
            return WithMembers(Members.AddRange(members));
        }

        public bool Any()
        {
            return Members.Any();
        }

        public MemberDeclarationSyntax First()
        {
            return Members.First();
        }

        public MemberDeclarationSyntax FirstOrDefault()
        {
            return Members.FirstOrDefault();
        }

        public int IndexOf(Func<MemberDeclarationSyntax, bool> predicate)
        {
            return Members.IndexOf(predicate);
        }

        public int IndexOf(MemberDeclarationSyntax member)
        {
            return Members.IndexOf(member);
        }

        public MemberDeclarationsInfo Insert(int index, MemberDeclarationSyntax statement)
        {
            return WithMembers(Members.Insert(index, statement));
        }

        public MemberDeclarationsInfo InsertRange(int index, IEnumerable<MemberDeclarationSyntax> members)
        {
            return WithMembers(Members.InsertRange(index, members));
        }

        public MemberDeclarationSyntax Last()
        {
            return Members.Last();
        }

        public int LastIndexOf(Func<MemberDeclarationSyntax, bool> predicate)
        {
            return Members.LastIndexOf(predicate);
        }

        public int LastIndexOf(MemberDeclarationSyntax member)
        {
            return Members.LastIndexOf(member);
        }

        public MemberDeclarationSyntax LastOrDefault()
        {
            return Members.LastOrDefault();
        }

        public MemberDeclarationsInfo Remove(MemberDeclarationSyntax member)
        {
            return WithMembers(Members.Remove(member));
        }

        public MemberDeclarationsInfo RemoveAt(int index)
        {
            return WithMembers(Members.RemoveAt(index));
        }

        public MemberDeclarationsInfo Replace(MemberDeclarationSyntax nodeInList, MemberDeclarationSyntax newNode)
        {
            return WithMembers(Members.Replace(nodeInList, newNode));
        }

        public MemberDeclarationsInfo ReplaceAt(int index, MemberDeclarationSyntax newNode)
        {
            return WithMembers(Members.ReplaceAt(index, newNode));
        }

        public MemberDeclarationsInfo ReplaceRange(MemberDeclarationSyntax nodeInList, IEnumerable<MemberDeclarationSyntax> newNodes)
        {
            return WithMembers(Members.ReplaceRange(nodeInList, newNodes));
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (Declaration == null)
                throw new InvalidOperationException($"{nameof(MemberDeclarationsInfo)} is not initalized.");
        }

        public override string ToString()
        {
            return Declaration?.ToString() ?? base.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is MemberDeclarationsInfo other && Equals(other);
        }

        public bool Equals(MemberDeclarationsInfo other)
        {
            return EqualityComparer<MemberDeclarationSyntax>.Default.Equals(Declaration, other.Declaration);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<MemberDeclarationSyntax>.Default.GetHashCode(Declaration);
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
