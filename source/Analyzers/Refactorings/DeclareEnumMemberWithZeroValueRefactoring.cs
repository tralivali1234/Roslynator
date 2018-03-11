// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class DeclareEnumMemberWithZeroValueRefactoring
    {
        public static void AnalyzeNamedType(SymbolAnalysisContext context, INamedTypeSymbol flagsAttribute)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.IsImplicitlyDeclared)
                return;

            if (namedTypeSymbol.TypeKind != TypeKind.Enum)
                return;

            if (!namedTypeSymbol.HasAttribute(flagsAttribute))
                return;

            if (ContainsMemberWithZeroValue(namedTypeSymbol))
                return;

            var enumDeclaration = (EnumDeclarationSyntax)namedTypeSymbol.GetSyntax(context.CancellationToken);

            context.ReportDiagnostic(DiagnosticDescriptors.DeclareEnumMemberWithZeroValue, enumDeclaration.Identifier);
        }

        private static bool ContainsMemberWithZeroValue(INamedTypeSymbol namedTypeSymbol)
        {
            SpecialType specialType = namedTypeSymbol.EnumUnderlyingType.SpecialType;

            if (specialType == SpecialType.System_Int32)
                return namedTypeSymbol.ContainsMember<IFieldSymbol>(f => f.HasConstantValue((int)0));

            switch (specialType)
            {
                case SpecialType.System_SByte:
                    return namedTypeSymbol.ContainsMember<IFieldSymbol>(f => f.HasConstantValue((sbyte)0));
                case SpecialType.System_Byte:
                    return namedTypeSymbol.ContainsMember<IFieldSymbol>(f => f.HasConstantValue((byte)0));
                case SpecialType.System_Int16:
                    return namedTypeSymbol.ContainsMember<IFieldSymbol>(f => f.HasConstantValue((short)0));
                case SpecialType.System_UInt16:
                    return namedTypeSymbol.ContainsMember<IFieldSymbol>(f => f.HasConstantValue((ushort)0));
                case SpecialType.System_UInt32:
                    return namedTypeSymbol.ContainsMember<IFieldSymbol>(f => f.HasConstantValue((uint)0));
                case SpecialType.System_Int64:
                    return namedTypeSymbol.ContainsMember<IFieldSymbol>(f => f.HasConstantValue((long)0));
                case SpecialType.System_UInt64:
                    return namedTypeSymbol.ContainsMember<IFieldSymbol>(f => f.HasConstantValue((ulong)0));
                default:
                    {
                        Debug.Fail(specialType.ToString());
                        return false;
                    }
            }
        }
    }
}
