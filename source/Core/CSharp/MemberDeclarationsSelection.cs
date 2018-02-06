// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp
{
    public class MemberDeclarationsSelection : SyntaxListSelection<MemberDeclarationSyntax>
    {
        private MemberDeclarationsSelection(MemberDeclarationSyntax containingDeclaration, SyntaxList<MemberDeclarationSyntax> members, TextSpan span, int startIndex, int endIndex)
             : base(members, span, startIndex, endIndex)
        {
            ContainingDeclaration = containingDeclaration;
        }

        public MemberDeclarationSyntax ContainingDeclaration { get; }

        public static MemberDeclarationsSelection Create(NamespaceDeclarationSyntax namespaceDeclaration, TextSpan span)
        {
            if (namespaceDeclaration == null)
                throw new ArgumentNullException(nameof(namespaceDeclaration));

            return Create(namespaceDeclaration, namespaceDeclaration.Members, span);
        }

        public static MemberDeclarationsSelection Create(ClassDeclarationSyntax classDeclaration, TextSpan span)
        {
            if (classDeclaration == null)
                throw new ArgumentNullException(nameof(classDeclaration));

            return Create(classDeclaration, classDeclaration.Members, span);
        }

        public static MemberDeclarationsSelection Create(StructDeclarationSyntax structDeclaration, TextSpan span)
        {
            if (structDeclaration == null)
                throw new ArgumentNullException(nameof(structDeclaration));

            return Create(structDeclaration, structDeclaration.Members, span);
        }

        public static MemberDeclarationsSelection Create(InterfaceDeclarationSyntax interfaceDeclaration, TextSpan span)
        {
            if (interfaceDeclaration == null)
                throw new ArgumentNullException(nameof(interfaceDeclaration));

            return Create(interfaceDeclaration, interfaceDeclaration.Members, span);
        }

        private static MemberDeclarationsSelection Create(MemberDeclarationSyntax memberDeclaration, SyntaxList<MemberDeclarationSyntax> members, TextSpan span)
        {
            (int startIndex, int endIndex) = GetIndexes(members, span);

            return new MemberDeclarationsSelection(memberDeclaration, members, span, startIndex, endIndex);
        }

        public static bool TryCreate(NamespaceDeclarationSyntax namespaceDeclaration, TextSpan span, out MemberDeclarationsSelection selectedMembers)
        {
            if (namespaceDeclaration != null)
            {
                return TryCreate(namespaceDeclaration, namespaceDeclaration.Members, span, out selectedMembers);
            }
            else
            {
                selectedMembers = null;
                return false;
            }
        }

        public static bool TryCreate(ClassDeclarationSyntax classDeclaration, TextSpan span, out MemberDeclarationsSelection selectedMembers)
        {
            if (classDeclaration != null)
            {
                return TryCreate(classDeclaration, classDeclaration.Members, span, out selectedMembers);
            }
            else
            {
                selectedMembers = null;
                return false;
            }
        }

        public static bool TryCreate(StructDeclarationSyntax structDeclaration, TextSpan span, out MemberDeclarationsSelection selectedMembers)
        {
            if (structDeclaration != null)
            {
                return TryCreate(structDeclaration, structDeclaration.Members, span, out selectedMembers);
            }
            else
            {
                selectedMembers = null;
                return false;
            }
        }

        public static bool TryCreate(InterfaceDeclarationSyntax interfaceDeclaration, TextSpan span, out MemberDeclarationsSelection selectedMembers)
        {
            if (interfaceDeclaration != null)
            {
                return TryCreate(interfaceDeclaration, interfaceDeclaration.Members, span, out selectedMembers);
            }
            else
            {
                selectedMembers = null;
                return false;
            }
        }

        private static bool TryCreate(MemberDeclarationSyntax containingDeclaration, SyntaxList<MemberDeclarationSyntax> members, TextSpan span, out MemberDeclarationsSelection selectedMembers)
        {
            selectedMembers = null;

            if (!members.Any())
                return false;

            if (span.IsEmpty)
                return false;

            (int startIndex, int endIndex) = GetIndexes(members, span);

            if (startIndex == -1)
                return false;

            selectedMembers = new MemberDeclarationsSelection(containingDeclaration, members, span, startIndex, endIndex);
            return true;
        }
    }
}
