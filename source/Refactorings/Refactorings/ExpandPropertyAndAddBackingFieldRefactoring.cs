// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class ExpandPropertyAndAddBackingFieldRefactoring
    {
        public static async Task<Document> RefactorAsync(
            Document document,
            PropertyDeclarationSyntax propertyDeclaration,
            bool prefixIdentifierWithUnderscore = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            string fieldName = StringUtility.ToCamelCase(
                propertyDeclaration.Identifier.ValueText,
                prefixWithUnderscore: prefixIdentifierWithUnderscore);

            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            fieldName = NameGenerator.Default.EnsureUniqueMemberName(
                fieldName,
                semanticModel,
                propertyDeclaration.SpanStart,
                cancellationToken: cancellationToken);

            FieldDeclarationSyntax fieldDeclaration = CreateBackingField(propertyDeclaration, fieldName)
                .WithFormatterAnnotation();

            PropertyDeclarationSyntax newPropertyDeclaration = ExpandPropertyAndAddBackingField(propertyDeclaration, fieldName);

            newPropertyDeclaration = ExpandPropertyRefactoring.ReplaceAbstractWithVirtual(newPropertyDeclaration);

            newPropertyDeclaration = newPropertyDeclaration
                .WithTriviaFrom(propertyDeclaration)
                .WithFormatterAnnotation();

            MemberDeclarationsInfo info = SyntaxInfo.MemberDeclarationsInfo(propertyDeclaration.Parent);

            int propertyIndex = info.IndexOf(propertyDeclaration);

            if (IsReadOnlyAutoProperty(propertyDeclaration))
            {
                IPropertySymbol propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclaration, cancellationToken);

                ImmutableArray<SyntaxNode> nodes = await SyntaxFinder.FindReferencesAsync(propertySymbol, document, cancellationToken: cancellationToken).ConfigureAwait(false);

                IdentifierNameSyntax newNode = IdentifierName(fieldName);

                info = SyntaxInfo.MemberDeclarationsInfo(info.Declaration.ReplaceNodes(nodes, (f, _) => newNode.WithTriviaFrom(f)));
            }

            SyntaxList<MemberDeclarationSyntax> newMembers = info.Members
                .ReplaceAt(propertyIndex, newPropertyDeclaration)
                .InsertMember(fieldDeclaration);

            return await document.ReplaceMembersAsync(info, newMembers, cancellationToken).ConfigureAwait(false);
        }

        private static bool IsReadOnlyAutoProperty(PropertyDeclarationSyntax propertyDeclaration)
        {
            AccessorListSyntax accessorList = propertyDeclaration.AccessorList;

            return accessorList?.Getter()?.IsAutoGetter() == true
                && accessorList.Setter() == null;
        }

        private static PropertyDeclarationSyntax ExpandPropertyAndAddBackingField(PropertyDeclarationSyntax propertyDeclaration, string name)
        {
            AccessorDeclarationSyntax getter = propertyDeclaration.Getter();

            if (getter != null)
            {
                AccessorDeclarationSyntax newGetter = getter
                    .WithBody(Block(ReturnStatement(IdentifierName(name))))
                    .WithSemicolonToken(default(SyntaxToken));

                propertyDeclaration = propertyDeclaration
                    .ReplaceNode(getter, newGetter)
                    .WithInitializer(null)
                    .WithSemicolonToken(default(SyntaxToken));
            }

            AccessorDeclarationSyntax setter = propertyDeclaration.Setter();

            if (setter != null)
            {
                AccessorDeclarationSyntax newSetter = setter
                    .WithBody(Block(
                        SimpleAssignmentStatement(
                                IdentifierName(name),
                                IdentifierName("value"))))
                    .WithSemicolonToken(default(SyntaxToken));

                propertyDeclaration = propertyDeclaration.ReplaceNode(setter, newSetter);
            }

            AccessorListSyntax accessorList = propertyDeclaration.AccessorList
                .RemoveWhitespace()
                .WithCloseBraceToken(propertyDeclaration.AccessorList.CloseBraceToken.WithLeadingTrivia(NewLine()));

            return propertyDeclaration
                .WithAccessorList(accessorList);
        }

        private static FieldDeclarationSyntax CreateBackingField(PropertyDeclarationSyntax propertyDeclaration, string name)
        {
            SyntaxTokenList modifiers = Modifiers.Private();

            if (propertyDeclaration.Modifiers.Contains(SyntaxKind.StaticKeyword))
                modifiers = modifiers.Add(StaticKeyword());

            return FieldDeclaration(
                modifiers,
                propertyDeclaration.Type,
                name,
                propertyDeclaration.Initializer);
        }
    }
}
