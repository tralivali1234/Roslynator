// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp
{
    /// <summary>
    /// Represents selected member declarations in a <see cref="SyntaxList{MemberDeclarationSyntax}"/>.
    /// </summary>
    public class MemberDeclarationListSelection : SyntaxListSelection<MemberDeclarationSyntax>
    {
        private MemberDeclarationListSelection(MemberDeclarationSyntax declaration, SyntaxList<MemberDeclarationSyntax> members, TextSpan span, SelectionResult result)
             : this(declaration, members, span, result.FirstIndex, result.LastIndex)
        {
        }

        private MemberDeclarationListSelection(MemberDeclarationSyntax declaration, SyntaxList<MemberDeclarationSyntax> members, TextSpan span, int firstIndex, int lastIndex)
             : base(members, span, firstIndex, lastIndex)
        {
            Declaration = declaration;
        }

        /// <summary>
        /// Gets a declaration that contains selected members.
        /// </summary>
        public MemberDeclarationSyntax Declaration { get; }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationListSelection"/> based on the specified namespace declaration and span.
        /// </summary>
        /// <param name="namespaceDeclaration"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public static MemberDeclarationListSelection Create(NamespaceDeclarationSyntax namespaceDeclaration, TextSpan span)
        {
            if (namespaceDeclaration == null)
                throw new ArgumentNullException(nameof(namespaceDeclaration));

            return Create(namespaceDeclaration, namespaceDeclaration.Members, span);
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationListSelection"/> based on the specified type declaration and span.
        /// </summary>
        /// <param name="typeDeclaration"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public static MemberDeclarationListSelection Create(TypeDeclarationSyntax typeDeclaration, TextSpan span)
        {
            if (typeDeclaration == null)
                throw new ArgumentNullException(nameof(typeDeclaration));

            return Create(typeDeclaration, typeDeclaration.Members, span);
        }

        private static MemberDeclarationListSelection Create(MemberDeclarationSyntax memberDeclaration, SyntaxList<MemberDeclarationSyntax> members, TextSpan span)
        {
            SelectionResult result = SelectionResult.Create(members, span);

            return new MemberDeclarationListSelection(memberDeclaration, members, span, result);
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationListSelection"/> based on the specified namespace declaration and span.
        /// </summary>
        /// <param name="namespaceDeclaration"></param>
        /// <param name="span"></param>
        /// <param name="selectedMembers"></param>
        /// <returns>True if the specified span contains at least one member; otherwise, false.</returns>
        public static bool TryCreate(NamespaceDeclarationSyntax namespaceDeclaration, TextSpan span, out MemberDeclarationListSelection selectedMembers)
        {
            selectedMembers = Create(namespaceDeclaration, span, 1, int.MaxValue);
            return selectedMembers != null;
        }

        internal static bool TryCreate(NamespaceDeclarationSyntax namespaceDeclaration, TextSpan span, int minCount, out MemberDeclarationListSelection selectedMembers)
        {
            selectedMembers = Create(namespaceDeclaration, span, minCount, int.MaxValue);
            return selectedMembers != null;
        }

        internal static bool TryCreate(NamespaceDeclarationSyntax namespaceDeclaration, TextSpan span, int minCount, int maxCount, out MemberDeclarationListSelection selectedMembers)
        {
            selectedMembers = Create(namespaceDeclaration, span, minCount, maxCount);
            return selectedMembers != null;
        }

        /// <summary>
        /// Creates a new <see cref="MemberDeclarationListSelection"/> based on the specified type declaration and span.
        /// </summary>
        /// <param name="typeDeclaration"></param>
        /// <param name="span"></param>
        /// <param name="selectedMembers"></param>
        /// <returns>True if the specified span contains at least one member; otherwise, false.</returns>
        public static bool TryCreate(TypeDeclarationSyntax typeDeclaration, TextSpan span, out MemberDeclarationListSelection selectedMembers)
        {
            selectedMembers = Create(typeDeclaration, span, 1, int.MaxValue);
            return selectedMembers != null;
        }

        internal static bool TryCreate(TypeDeclarationSyntax typeDeclaration, TextSpan span, int minCount, out MemberDeclarationListSelection selectedMembers)
        {
            selectedMembers = Create(typeDeclaration, span, minCount, int.MaxValue);
            return selectedMembers != null;
        }

        internal static bool TryCreate(TypeDeclarationSyntax typeDeclaration, TextSpan span, int minCount, int maxCount, out MemberDeclarationListSelection selectedMembers)
        {
            selectedMembers = Create(typeDeclaration, span, minCount, maxCount);
            return selectedMembers != null;
        }

        private static MemberDeclarationListSelection Create(NamespaceDeclarationSyntax declaration, TextSpan span, int minCount, int maxCount)
        {
            if (declaration == null)
                return null;

            return Create(declaration, declaration.Members, span, minCount, maxCount);
        }

        private static MemberDeclarationListSelection Create(TypeDeclarationSyntax declaration, TextSpan span, int minCount, int maxCount)
        {
            if (declaration == null)
                return null;

            return Create(declaration, declaration.Members, span, minCount, maxCount);
        }

        private static MemberDeclarationListSelection Create(MemberDeclarationSyntax declaration, SyntaxList<MemberDeclarationSyntax> members, TextSpan span, int minCount, int maxCount)
        {
            SelectionResult result = SelectionResult.Create(members, span, minCount, maxCount);

            if (!result.Success)
                return null;

            return new MemberDeclarationListSelection(declaration, members, span, result);
        }
    }
}
