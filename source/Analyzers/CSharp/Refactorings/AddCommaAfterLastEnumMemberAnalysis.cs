// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AddCommaAfterLastEnumMemberAnalysis
    {
        public static void AnalyzeEnumDeclaration(SyntaxNodeAnalysisContext context)
        {
            var enumDeclaration = (EnumDeclarationSyntax)context.Node;

            SeparatedSyntaxList<EnumMemberDeclarationSyntax> members = enumDeclaration.Members;

            int count = members.Count;

            if (count == 0)
                return;

            if (count - members.SeparatorCount != 1)
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.AddCommaAfterLastEnumMember, Location.Create(enumDeclaration.SyntaxTree, new TextSpan(members.Last().Span.End, 0)));
        }
    }
}
