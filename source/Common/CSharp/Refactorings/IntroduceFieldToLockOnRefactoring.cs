// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;
using static Roslynator.CSharp.CSharpTypeFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class IntroduceFieldToLockOnRefactoring
    {
        private const string LockObjectName = "_lockObject";

        public static async Task<Document> RefactorAsync(
            Document document,
            LockStatementSyntax lockStatement,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (lockStatement == null)
                throw new ArgumentNullException(nameof(lockStatement));

            MemberDeclarationSyntax containingMember = lockStatement.FirstAncestor<MemberDeclarationSyntax>();

            Debug.Assert(containingMember != null);

            if (containingMember == null)
                return document;

            var containingType = (TypeDeclarationSyntax)containingMember
                .Ancestors()
                .FirstOrDefault(f => f.IsKind(
                    SyntaxKind.ClassDeclaration,
                    SyntaxKind.InterfaceDeclaration,
                    SyntaxKind.StructDeclaration));

            Debug.Assert(containingType != null);

            if (containingType == null)
                return document;

            SyntaxList<MemberDeclarationSyntax> members = containingType.Members;

            int index = members.IndexOf(containingMember);

            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            string name = NameGenerator.Default.EnsureUniqueLocalName(
                LockObjectName,
                semanticModel,
                lockStatement.Expression.SpanStart,
                cancellationToken: cancellationToken);

            LockStatementSyntax newLockStatement = lockStatement
                .WithExpression(IdentifierName(Identifier(name).WithRenameAnnotation()));

            MemberDeclarationSyntax newContainingMember = containingMember
                .ReplaceNode(lockStatement, newLockStatement);

            bool isStatic = SyntaxInfo.ModifiersInfo(containingMember).IsStatic;

            FieldDeclarationSyntax field = CreateFieldDeclaration(name, isStatic).WithFormatterAnnotation();

            SyntaxList<MemberDeclarationSyntax> newMembers = members.ReplaceAt(index, newContainingMember);

            newMembers = newMembers.Insert(field);

            MemberDeclarationSyntax newNode = containingType.WithMembers(newMembers);

            return await document.ReplaceNodeAsync(containingType, newNode, cancellationToken).ConfigureAwait(false);
        }

        private static FieldDeclarationSyntax CreateFieldDeclaration(string name, bool isStatic)
        {
            return FieldDeclaration(
                (isStatic) ? Modifiers.PrivateStaticReadOnly() : Modifiers.PrivateReadOnly(),
                ObjectType(),
                Identifier(name),
                ObjectCreationExpression(ObjectType(), ArgumentList()));
        }
    }
}
