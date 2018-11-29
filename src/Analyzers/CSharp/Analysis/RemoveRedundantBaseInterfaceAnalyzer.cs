﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RemoveRedundantBaseInterfaceAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.RemoveRedundantBaseInterface); }
        }

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            base.Initialize(context);

            context.RegisterSyntaxNodeAction(AnalyzeBaseList, SyntaxKind.BaseList);
        }

        public static void AnalyzeBaseList(SyntaxNodeAnalysisContext context)
        {
            var baseList = (BaseListSyntax)context.Node;

            if (!baseList.IsParentKind(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration, SyntaxKind.InterfaceDeclaration))
                return;

            if (baseList.ContainsDiagnostics)
                return;

            if (baseList.SpanContainsDirectives())
                return;

            SeparatedSyntaxList<BaseTypeSyntax> baseTypes = baseList.Types;

            if (baseTypes.Count <= 1)
                return;

            bool isFirst = true;
            INamedTypeSymbol typeSymbol = null;
            var baseClassInfo = default(SymbolInterfaceInfo);
            List<SymbolInterfaceInfo> baseInterfaceInfos = null;

            foreach (BaseTypeSyntax baseType in baseTypes)
            {
                TypeSyntax type = baseType.Type;

                if (type?.IsMissing == false
                    && (context.SemanticModel.GetSymbol(type, context.CancellationToken) is INamedTypeSymbol baseSymbol))
                {
                    TypeKind typeKind = baseSymbol.TypeKind;

                    ImmutableArray<INamedTypeSymbol> allInterfaces = baseSymbol.AllInterfaces;

                    if (typeKind == TypeKind.Class)
                    {
                        if (!isFirst)
                            break;

                        if (allInterfaces.Any())
                            baseClassInfo = new SymbolInterfaceInfo(baseType, baseSymbol, allInterfaces);
                    }
                    else if (typeKind == TypeKind.Interface)
                    {
                        var baseInterfaceInfo = new SymbolInterfaceInfo(baseType, baseSymbol, allInterfaces);

                        if (baseInterfaceInfos == null)
                        {
                            if (allInterfaces.Any())
                                baseInterfaceInfos = new List<SymbolInterfaceInfo>() { baseInterfaceInfo };
                        }
                        else
                        {
                            foreach (SymbolInterfaceInfo baseInterfaceInfo2 in baseInterfaceInfos)
                            {
                                Analyze(context, baseInterfaceInfo, baseInterfaceInfo2);
                                Analyze(context, baseInterfaceInfo2, baseInterfaceInfo);
                            }
                        }

                        if (baseClassInfo.IsValid)
                        {
                            if (typeSymbol == null)
                                typeSymbol = context.SemanticModel.GetDeclaredSymbol((TypeDeclarationSyntax)baseList.Parent, context.CancellationToken);

                            Analyze(context, baseInterfaceInfo, baseClassInfo, typeSymbol);
                        }
                    }
                }

                if (isFirst)
                    isFirst = false;
            }
        }

        private static void Analyze(
            SyntaxNodeAnalysisContext context,
            in SymbolInterfaceInfo interfaceInfo,
            in SymbolInterfaceInfo interfaceInfo2,
            INamedTypeSymbol typeSymbol = null)
        {
            if (IsFixable(interfaceInfo, interfaceInfo2, typeSymbol))
            {
                BaseTypeSyntax baseType = interfaceInfo.BaseType;

                context.ReportDiagnostic(
                    DiagnosticDescriptors.RemoveRedundantBaseInterface,
                    baseType,
                    SymbolDisplay.ToMinimalDisplayString(interfaceInfo.Symbol, context.SemanticModel, baseType.SpanStart, SymbolDisplayFormats.Default),
                    SymbolDisplay.ToMinimalDisplayString(interfaceInfo2.Symbol, context.SemanticModel, baseType.SpanStart, SymbolDisplayFormats.Default));
            }
        }

        private static bool IsFixable(
            in SymbolInterfaceInfo interfaceInfo,
            in SymbolInterfaceInfo interfaceInfo2,
            INamedTypeSymbol typeSymbol = null)
        {
            ImmutableArray<ISymbol> members = default;

            foreach (INamedTypeSymbol interfaceSymbol in interfaceInfo2.Interfaces)
            {
                if (interfaceInfo.Symbol.Equals(interfaceSymbol))
                {
                    if (typeSymbol != null)
                    {
                        if (members.IsDefault)
                            members = typeSymbol.GetMembers();

                        if (IsExplicitlyImplemented(interfaceInfo, members))
                            continue;
                    }

                    return true;
                }
            }

            return false;
        }

        private static bool IsExplicitlyImplemented(in SymbolInterfaceInfo interfaceInfo, ImmutableArray<ISymbol> members)
        {
            if (IsExplicitlyImplemented(interfaceInfo.Symbol, members))
                return true;

            foreach (INamedTypeSymbol interfaceSymbol in interfaceInfo.Interfaces)
            {
                if (IsExplicitlyImplemented(interfaceSymbol, members))
                    return true;
            }

            return false;
        }

        internal static bool IsExplicitlyImplemented(ISymbol interfaceSymbol, ImmutableArray<ISymbol> members)
        {
            foreach (ISymbol member in members)
            {
                switch (member.Kind)
                {
                    case SymbolKind.Event:
                        {
                            foreach (IEventSymbol eventSymbol in ((IEventSymbol)member).ExplicitInterfaceImplementations)
                            {
                                if (eventSymbol.ContainingType?.Equals(interfaceSymbol) == true)
                                    return true;
                            }

                            break;
                        }
                    case SymbolKind.Method:
                        {
                            foreach (IMethodSymbol methodSymbol in ((IMethodSymbol)member).ExplicitInterfaceImplementations)
                            {
                                if (methodSymbol.ContainingType?.Equals(interfaceSymbol) == true)
                                    return true;
                            }

                            break;
                        }
                    case SymbolKind.Property:
                        {
                            foreach (IPropertySymbol propertySymbol in ((IPropertySymbol)member).ExplicitInterfaceImplementations)
                            {
                                if (propertySymbol.ContainingType?.Equals(interfaceSymbol) == true)
                                    return true;
                            }

                            break;
                        }
                }
            }

            return false;
        }

        private readonly struct SymbolInterfaceInfo
        {
            public SymbolInterfaceInfo(BaseTypeSyntax baseType, INamedTypeSymbol symbol, ImmutableArray<INamedTypeSymbol> interfaces)
            {
                BaseType = baseType;
                Symbol = symbol;
                Interfaces = interfaces;
            }

            public bool IsValid
            {
                get { return BaseType != null; }
            }

            public BaseTypeSyntax BaseType { get; }

            public INamedTypeSymbol Symbol { get; }

            public ImmutableArray<INamedTypeSymbol> Interfaces { get; }
        }
    }
}
