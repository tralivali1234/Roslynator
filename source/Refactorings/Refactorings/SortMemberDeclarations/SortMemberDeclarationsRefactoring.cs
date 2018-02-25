// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Syntax;
using Roslynator.CSharp.SyntaxRewriters.SortMembers;

namespace Roslynator.CSharp.Refactorings.SortMemberDeclarations
{
    internal static class SortMemberDeclarationsRefactoring
    {
        public static void ComputeRefactoring(RefactoringContext context, NamespaceDeclarationSyntax namespaceDeclaration)
        {
            if (MemberDeclarationsSelection.TryCreate(namespaceDeclaration, context.Span, out MemberDeclarationsSelection selectedMembers))
                ComputeRefactoring(context, selectedMembers);
        }

        public static void ComputeRefactoring(RefactoringContext context, ClassDeclarationSyntax classDeclaration)
        {
            if (MemberDeclarationsSelection.TryCreate(classDeclaration, context.Span, out MemberDeclarationsSelection selectedMembers))
                ComputeRefactoring(context, selectedMembers);
        }

        public static void ComputeRefactoring(RefactoringContext context, StructDeclarationSyntax structDeclaration)
        {
            if (MemberDeclarationsSelection.TryCreate(structDeclaration, context.Span, out MemberDeclarationsSelection selectedMembers))
                ComputeRefactoring(context, selectedMembers);
        }

        public static void ComputeRefactoring(RefactoringContext context, InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (MemberDeclarationsSelection.TryCreate(interfaceDeclaration, context.Span, out MemberDeclarationsSelection selectedMembers))
                ComputeRefactoring(context, selectedMembers);
        }

        private static void ComputeRefactoring(RefactoringContext context, MemberDeclarationsSelection selectedMembers)
        {
            if (selectedMembers.Count > 1)
            {
                ImmutableArray<MemberDeclarationSyntax> members = selectedMembers.ToImmutableArray();

                SyntaxKind kind = GetSingleKindOrDefault(members);

                if (kind != SyntaxKind.None)
                {
                    if (MemberDeclarationComparer.CanBeSortedByName(kind))
                    {
                        ComputeRefactoring(
                            context,
                            MemberDeclarationSortMode.ByKindThenByName,
                            "Sort members by name",
                            selectedMembers,
                            members);
                    }
                }
                else
                {
                    ComputeRefactoring(
                        context,
                        MemberDeclarationSortMode.ByKind,
                        "Sort members by kind",
                        selectedMembers,
                        members);

                    ComputeRefactoring(
                        context,
                        MemberDeclarationSortMode.ByKindThenByName,
                        "Sort members by kind then by name",
                        selectedMembers,
                        members);
                }
            }
        }

        private static void ComputeRefactoring(
            RefactoringContext context,
            MemberDeclarationSortMode sortMode,
            string title,
            MemberDeclarationsSelection selectedMembers,
            ImmutableArray<MemberDeclarationSyntax> members)
        {
            if (members.IsSorted(MemberDeclarationComparer.GetInstance(sortMode)))
                return;

            context.RegisterRefactoring(
                title,
                cancellationToken => RefactorAsync(context.Document, selectedMembers, sortMode, cancellationToken));
        }

        private static Task<Document> RefactorAsync(
            Document document,
            MemberDeclarationsSelection selectedMembers,
            MemberDeclarationSortMode sortMode,
            CancellationToken cancellationToken)
        {
            MemberDeclarationComparer comparer = MemberDeclarationComparer.GetInstance(sortMode);

            MemberDeclarationsInfo info = SyntaxInfo.MemberDeclarationsInfo(selectedMembers);

            SyntaxList<MemberDeclarationSyntax> members = info.Members;

            SyntaxList<MemberDeclarationSyntax> newMembers = members
                .Take(selectedMembers.FirstIndex)
                .Concat(selectedMembers.OrderBy(f => f, comparer))
                .Concat(members.Skip(selectedMembers.LastIndex + 1))
                .ToSyntaxList();

            return document.ReplaceMembersAsync(info, newMembers, cancellationToken);
        }

        public static async Task<Document> RefactorAsync(
            Document document,
            IComparer<MemberDeclarationSyntax> memberComparer,
            IComparer<EnumMemberDeclarationSyntax> enumMemberComparer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            if (root.IsKind(SyntaxKind.CompilationUnit))
            {
                var rewriter = new SortMemberDeclarationsRewriter(memberComparer, enumMemberComparer);

                SyntaxNode newRoot = rewriter.VisitCompilationUnit((CompilationUnitSyntax)root);

                return document.WithSyntaxRoot(newRoot);
            }

            return document;
        }

        private static SyntaxKind GetSingleKindOrDefault(ImmutableArray<MemberDeclarationSyntax> members)
        {
            SyntaxKind kind = members[0].Kind();

            for (int i = 1; i < members.Length; i++)
            {
                if (members[i].Kind() != kind)
                    return SyntaxKind.None;
            }

            return kind;
        }
    }
}