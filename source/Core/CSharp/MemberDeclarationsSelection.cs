// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp
{
    public class MemberDeclarationsSelection : SyntaxListSelection<MemberDeclarationSyntax>
    {
        private MemberDeclarationsSelection(MemberDeclarationSyntax declaration, SyntaxList<MemberDeclarationSyntax> members, TextSpan span, SelectionResult result)
             : this(declaration, members, span, result.FirstIndex, result.LastIndex)
        {
        }

        private MemberDeclarationsSelection(MemberDeclarationSyntax declaration, SyntaxList<MemberDeclarationSyntax> members, TextSpan span, int firstIndex, int lastIndex)
             : base(members, span, firstIndex, lastIndex)
        {
            Declaration = declaration;
        }

        public MemberDeclarationSyntax Declaration { get; }

        public static MemberDeclarationsSelection Create(NamespaceDeclarationSyntax namespaceDeclaration, TextSpan span)
        {
            if (namespaceDeclaration == null)
                throw new ArgumentNullException(nameof(namespaceDeclaration));

            return Create(namespaceDeclaration, namespaceDeclaration.Members, span);
        }

        public static MemberDeclarationsSelection Create(TypeDeclarationSyntax typeDeclaration, TextSpan span)
        {
            if (typeDeclaration == null)
                throw new ArgumentNullException(nameof(typeDeclaration));

            return Create(typeDeclaration, typeDeclaration.Members, span);
        }

        private static MemberDeclarationsSelection Create(MemberDeclarationSyntax memberDeclaration, SyntaxList<MemberDeclarationSyntax> members, TextSpan span)
        {
            SelectionResult result = SelectionResult.Create(members, span);

            return new MemberDeclarationsSelection(memberDeclaration, members, span, result);
        }

        public static bool TryCreate(NamespaceDeclarationSyntax namespaceDeclaration, TextSpan span, out MemberDeclarationsSelection selectedMembers)
        {
            selectedMembers = Create(namespaceDeclaration, span, 1, int.MaxValue);
            return selectedMembers != null;
        }

        public static bool TryCreate(NamespaceDeclarationSyntax namespaceDeclaration, TextSpan span, int minCount, out MemberDeclarationsSelection selectedMembers)
        {
            selectedMembers = Create(namespaceDeclaration, span, minCount, int.MaxValue);
            return selectedMembers != null;
        }

        public static bool TryCreate(NamespaceDeclarationSyntax namespaceDeclaration, TextSpan span, int minCount, int maxCount, out MemberDeclarationsSelection selectedMembers)
        {
            selectedMembers = Create(namespaceDeclaration, span, minCount, maxCount);
            return selectedMembers != null;
        }

        public static bool TryCreateExact(NamespaceDeclarationSyntax namespaceDeclaration, TextSpan span, int count, out MemberDeclarationsSelection selectedMembers)
        {
            selectedMembers = Create(namespaceDeclaration, span, count, count);
            return selectedMembers != null;
        }

        public static bool TryCreate(TypeDeclarationSyntax typeDeclaration, TextSpan span, out MemberDeclarationsSelection selectedMembers)
        {
            selectedMembers = Create(typeDeclaration, span, 1, int.MaxValue);
            return selectedMembers != null;
        }

        public static bool TryCreate(TypeDeclarationSyntax typeDeclaration, TextSpan span, int minCount, out MemberDeclarationsSelection selectedMembers)
        {
            selectedMembers = Create(typeDeclaration, span, minCount, int.MaxValue);
            return selectedMembers != null;
        }

        public static bool TryCreate(TypeDeclarationSyntax typeDeclaration, TextSpan span, int minCount, int maxCount, out MemberDeclarationsSelection selectedMembers)
        {
            selectedMembers = Create(typeDeclaration, span, minCount, maxCount);
            return selectedMembers != null;
        }

        public static bool TryCreateExact(TypeDeclarationSyntax typeDeclaration, TextSpan span, int count, out MemberDeclarationsSelection selectedMembers)
        {
            selectedMembers = Create(typeDeclaration, span, count, count);
            return selectedMembers != null;
        }

        private static MemberDeclarationsSelection Create(NamespaceDeclarationSyntax declaration, TextSpan span, int minCount, int maxCount)
        {
            if (declaration == null)
                return null;

            return Create(declaration, declaration.Members, span, minCount, maxCount);
        }

        private static MemberDeclarationsSelection Create(TypeDeclarationSyntax declaration, TextSpan span, int minCount, int maxCount)
        {
            if (declaration == null)
                return null;

            return Create(declaration, declaration.Members, span, minCount, maxCount);
        }

        private static MemberDeclarationsSelection Create(MemberDeclarationSyntax declaration, SyntaxList<MemberDeclarationSyntax> members, TextSpan span, int minCount, int maxCount)
        {
            SelectionResult result = SelectionResult.Create(members, span, minCount, maxCount);

            if (!result.Success)
                return null;

            return new MemberDeclarationsSelection(declaration, members, span, result);
        }
    }
}
