// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;
using static Roslynator.CSharp.CSharpFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class DeclareEnumMemberWithZeroValueRefactoring
    {
        public static void AnalyzeNamedType(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (!namedTypeSymbol.IsEnumWithFlags(context.Compilation))
                return;

            if (ContainsMemberWithZeroValue(namedTypeSymbol))
                return;

            var enumDeclaration = namedTypeSymbol.GetSyntaxOrDefault(context.CancellationToken) as EnumDeclarationSyntax;

            Debug.Assert(enumDeclaration != null, namedTypeSymbol.ToString());

            if (enumDeclaration == null)
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.DeclareEnumMemberWithZeroValue, enumDeclaration.Identifier);
        }

        private static bool ContainsMemberWithZeroValue(INamedTypeSymbol namedTypeSymbol)
        {
            INamedTypeSymbol enumUnderlyingType = namedTypeSymbol.EnumUnderlyingType;

            switch (enumUnderlyingType.SpecialType)
            {
                case SpecialType.System_SByte:
                    return namedTypeSymbol.ExistsMember<IFieldSymbol>(f => f.HasConstantValue((sbyte)0));
                case SpecialType.System_Byte:
                    return namedTypeSymbol.ExistsMember<IFieldSymbol>(f => f.HasConstantValue((byte)0));
                case SpecialType.System_Int16:
                    return namedTypeSymbol.ExistsMember<IFieldSymbol>(f => f.HasConstantValue((short)0));
                case SpecialType.System_UInt16:
                    return namedTypeSymbol.ExistsMember<IFieldSymbol>(f => f.HasConstantValue((ushort)0));
                case SpecialType.System_Int32:
                    return namedTypeSymbol.ExistsMember<IFieldSymbol>(f => f.HasConstantValue((int)0));
                case SpecialType.System_UInt32:
                    return namedTypeSymbol.ExistsMember<IFieldSymbol>(f => f.HasConstantValue((uint)0));
                case SpecialType.System_Int64:
                    return namedTypeSymbol.ExistsMember<IFieldSymbol>(f => f.HasConstantValue((long)0));
                case SpecialType.System_UInt64:
                    return namedTypeSymbol.ExistsMember<IFieldSymbol>(f => f.HasConstantValue((ulong)0));
                default:
                    {
                        Debug.Fail(enumUnderlyingType.SpecialType.ToString());
                        return false;
                    }
            }
        }

        public static async Task<Document> RefactorAsync(
            Document document,
            EnumDeclarationSyntax enumDeclaration,
            CancellationToken cancellationToken)
        {
            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            INamedTypeSymbol symbol = semanticModel.GetDeclaredSymbol(enumDeclaration, cancellationToken);

            string name = NameGenerator.Default.EnsureUniqueEnumMemberName("None", symbol);

            EnumMemberDeclarationSyntax enumMember = EnumMemberDeclaration(
                Identifier(name).WithRenameAnnotation(),
                NumericLiteralExpression(0));

            enumMember = enumMember.WithTrailingTrivia(NewLine());

            EnumDeclarationSyntax newNode = enumDeclaration.WithMembers(enumDeclaration.Members.Insert(0, enumMember));

            return await document.ReplaceNodeAsync(enumDeclaration, newNode, cancellationToken).ConfigureAwait(false);
        }
    }
}
