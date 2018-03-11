// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class ReorderTypeParameterConstraintsRefactoring
    {
        public static void AnalyzeTypeParameterList(SyntaxNodeAnalysisContext context)
        {
            var typeParameterList = (TypeParameterListSyntax)context.Node;

            GenericInfo genericInfo = SyntaxInfo.GenericInfo(typeParameterList);

            if (!genericInfo.Success)
                return;

            if (!genericInfo.TypeParameters.Any())
                return;

            if (!genericInfo.ConstraintClauses.Any())
                return;

            if (genericInfo.ConstraintClauses.SpanContainsDirectives())
                return;

            if (!IsFixable(genericInfo.TypeParameters, genericInfo.ConstraintClauses))
                return;

            context.ReportDiagnostic(
                DiagnosticDescriptors.ReorderTypeParameterConstraints,
                genericInfo.ConstraintClauses.First());
        }

        private static bool IsFixable(
            SeparatedSyntaxList<TypeParameterSyntax> typeParameters,
            SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            int lastIndex = -1;

            for (int i = 0; i < typeParameters.Count; i++)
            {
                string name = typeParameters[i].Identifier.ValueText;

                int index = IndexOf(constraintClauses, name);

                if (index != -1)
                {
                    if (index < lastIndex)
                        return true;

                    lastIndex = index;
                }
            }

            return false;
        }

        private static int IndexOf(SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses, string name)
        {
            for (int i = 0; i < constraintClauses.Count; i++)
            {
                if (constraintClauses[i].Name.Identifier.ValueText == name)
                    return i;
            }

            return -1;
        }
    }
}
