// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class DeclareEachTypeInSeparateFileRefactoring
    {
        public static void AnalyzeCompilationUnit(SyntaxNodeAnalysisContext context)
        {
            var compilationUnit = (CompilationUnitSyntax)context.Node;

            SyntaxList<MemberDeclarationSyntax> members = compilationUnit.Members;

            if (ContainsSingleNamespaceWithSingleNonNamespaceMember(members))
                return;

            using (IEnumerator<MemberDeclarationSyntax> en = ExtractTypeDeclarationToNewDocumentRefactoring.GetNonNestedTypeDeclarations(members).GetEnumerator())
            {
                if (en.MoveNext())
                {
                    MemberDeclarationSyntax firstMember = en.Current;

                    if (en.MoveNext())
                    {
                        ReportDiagnostic(context, firstMember);

                        do
                        {
                            ReportDiagnostic(context, en.Current);

                        } while (en.MoveNext());
                    }
                }
            }
        }

        private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, MemberDeclarationSyntax member)
        {
            SyntaxToken token = ExtractTypeDeclarationToNewDocumentRefactoring.GetIdentifier(member);

            if (token == default(SyntaxToken))
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.DeclareEachTypeInSeparateFile, token);
        }

        private static bool ContainsSingleNamespaceWithSingleNonNamespaceMember(SyntaxList<MemberDeclarationSyntax> members)
        {
            MemberDeclarationSyntax member = members.SingleOrDefault(shouldThrow: false);

            if (member?.Kind() != SyntaxKind.NamespaceDeclaration)
                return false;

            var namespaceDeclaration = (NamespaceDeclarationSyntax)member;

            member = namespaceDeclaration.Members.SingleOrDefault(shouldThrow: false);

            return member != null
                && member.Kind() != SyntaxKind.NamespaceDeclaration;
        }
    }
}
