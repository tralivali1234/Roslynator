// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Refactorings
{
    internal static class UseGenericEventHandlerRefactoring
    {
        public static void AnalyzeEvent(SymbolAnalysisContext context, INamedTypeSymbol eventHandlerSymbol)
        {
            var eventSymbol = (IEventSymbol)context.Symbol;

            if (eventSymbol.IsOverride)
                return;

            if (!eventSymbol.ExplicitInterfaceImplementations.IsDefaultOrEmpty)
                return;

            if (eventSymbol.ImplementsInterfaceMember<IEventSymbol>(allInterfaces: true))
                return;

            var namedType = eventSymbol.Type as INamedTypeSymbol;

            if (namedType?.Arity != 0)
                return;

            if (namedType.Equals(eventHandlerSymbol))
                return;

            IMethodSymbol delegateInvokeMethod = namedType.DelegateInvokeMethod;

            if (delegateInvokeMethod == null)
                return;

            ImmutableArray<IParameterSymbol> parameters = delegateInvokeMethod.Parameters;

            if (parameters.Length != 2)
                return;

            if (!parameters[0].Type.IsObject())
                return;

            SyntaxNode node = eventSymbol.GetSyntaxOrDefault(context.CancellationToken);

            Debug.Assert(node != null, eventSymbol.ToString());

            if (node == null)
                return;

            TypeSyntax type = GetTypeSyntax(node);

            context.ReportDiagnostic(DiagnosticDescriptors.UseGenericEventHandler, type);
        }

        private static TypeSyntax GetTypeSyntax(SyntaxNode node)
        {
            switch (node.Kind())
            {
                case SyntaxKind.EventDeclaration:
                    {
                        return ((EventDeclarationSyntax)node).Type;
                    }
                case SyntaxKind.VariableDeclarator:
                    {
                        var declarator = (VariableDeclaratorSyntax)node;

                        SyntaxNode parent = declarator.Parent;

                        if (parent?.Kind() == SyntaxKind.VariableDeclaration)
                        {
                            var declaration = (VariableDeclarationSyntax)parent;

                            return declaration.Type;
                        }

                        break;
                    }
            }

            throw new InvalidOperationException();
        }
    }
}
