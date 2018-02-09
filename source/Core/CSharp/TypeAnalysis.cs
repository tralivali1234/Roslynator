// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp
{
    internal static class TypeAnalysis
    {
        public static TypeAnalysisFlags AnalyzeType(
            VariableDeclarationSyntax variableDeclaration,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (variableDeclaration == null)
                throw new ArgumentNullException(nameof(variableDeclaration));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            TypeSyntax type = variableDeclaration.Type;

            if (type != null)
            {
                SeparatedSyntaxList<VariableDeclaratorSyntax> variables = variableDeclaration.Variables;

                if (variables.Count > 0
                    && !variableDeclaration.IsParentKind(SyntaxKind.FieldDeclaration, SyntaxKind.EventFieldDeclaration))
                {
                    ExpressionSyntax expression = variables[0].Initializer?.Value;

                    if (expression != null)
                    {
                        ITypeSymbol typeSymbol = semanticModel.GetTypeSymbol(type, cancellationToken);

                        if (typeSymbol?.IsErrorType() == false)
                        {
                            TypeAnalysisFlags flags;

                            if (typeSymbol.IsDynamicType())
                            {
                                flags = TypeAnalysisFlags.Dynamic;
                            }
                            else
                            {
                                flags = TypeAnalysisFlags.ValidSymbol;

                                if (type.IsVar)
                                {
                                    flags |= TypeAnalysisFlags.Implicit;

                                    if (typeSymbol.SupportsExplicitDeclaration())
                                        flags |= TypeAnalysisFlags.SupportsExplicit;
                                }
                                else
                                {
                                    flags |= TypeAnalysisFlags.Explicit;

                                    if (variables.Count == 1
                                        && !IsLocalConstDeclaration(variableDeclaration)
                                        && !expression.IsKind(SyntaxKind.NullLiteralExpression)
                                        && typeSymbol.Equals(semanticModel.GetTypeSymbol(expression, cancellationToken)))
                                    {
                                        flags |= TypeAnalysisFlags.SupportsImplicit;
                                    }
                                }

                                if (IsTypeObvious(expression, semanticModel, cancellationToken))
                                    flags |= TypeAnalysisFlags.TypeObvious;
                            }

                            return flags;
                        }
                    }
                }
            }

            return TypeAnalysisFlags.None;
        }

        public static TypeAnalysisFlags AnalyzeType(
            DeclarationExpressionSyntax declarationExpression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (declarationExpression == null)
                throw new ArgumentNullException(nameof(declarationExpression));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            TypeSyntax type = declarationExpression.Type;

            if (type != null)
            {
                VariableDesignationSyntax designation = declarationExpression.Designation;

                if (designation?.Kind() == SyntaxKind.SingleVariableDesignation)
                {
                    var symbol = semanticModel.GetDeclaredSymbol((SingleVariableDesignationSyntax)designation, cancellationToken) as ILocalSymbol;

                    if (symbol?.IsErrorType() == false)
                    {
                        ITypeSymbol typeSymbol = symbol.Type;

                        if (typeSymbol?.IsErrorType() == false)
                        {
                            TypeAnalysisFlags flags;

                            if (typeSymbol.IsDynamicType())
                            {
                                flags = TypeAnalysisFlags.Dynamic;
                            }
                            else
                            {
                                flags = TypeAnalysisFlags.ValidSymbol;

                                if (type.IsVar)
                                {
                                    flags |= TypeAnalysisFlags.Implicit;

                                    if (symbol.Type.SupportsExplicitDeclaration())
                                        flags |= TypeAnalysisFlags.SupportsExplicit;
                                }
                                else
                                {
                                    flags |= TypeAnalysisFlags.Explicit;
                                    flags |= TypeAnalysisFlags.SupportsImplicit;
                                }
                            }

                            return flags;
                        }
                    }
                }
            }

            return TypeAnalysisFlags.None;
        }

        private static bool IsLocalConstDeclaration(VariableDeclarationSyntax variableDeclaration)
        {
            SyntaxNode parent = variableDeclaration.Parent;

            return parent?.Kind() == SyntaxKind.LocalDeclarationStatement
                && ((LocalDeclarationStatementSyntax)parent).IsConst;
        }

        private static bool IsTypeObvious(
            ExpressionSyntax expression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken)
        {
            switch (expression.Kind())
            {
                case SyntaxKind.ObjectCreationExpression:
                case SyntaxKind.ArrayCreationExpression:
                case SyntaxKind.CastExpression:
                case SyntaxKind.AsExpression:
                case SyntaxKind.ThisExpression:
                case SyntaxKind.DefaultExpression:
                    {
                        return true;
                    }
                case SyntaxKind.SimpleMemberAccessExpression:
                    {
                        ISymbol symbol = semanticModel.GetSymbol(expression, cancellationToken);

                        return symbol?.Kind == SymbolKind.Field
                            && symbol.ContainingType?.TypeKind == TypeKind.Enum;
                    }
            }

            return false;
        }

        public static TypeAnalysisFlags AnalyzeType(ForEachStatementSyntax forEachStatement, SemanticModel semanticModel)
        {
            if (forEachStatement == null)
                throw new ArgumentNullException(nameof(forEachStatement));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            TypeSyntax type = forEachStatement.Type;

            if (type != null)
            {
                ForEachStatementInfo info = semanticModel.GetForEachStatementInfo(forEachStatement);

                ITypeSymbol typeSymbol = info.ElementType;

                if (typeSymbol?.IsErrorType() == false)
                {
                    TypeAnalysisFlags flags;

                    if (typeSymbol.IsDynamicType())
                    {
                        flags = TypeAnalysisFlags.Dynamic;
                    }
                    else
                    {
                        flags = TypeAnalysisFlags.ValidSymbol;

                        if (type.IsVar)
                        {
                            flags |= TypeAnalysisFlags.Implicit;

                            if (typeSymbol.SupportsExplicitDeclaration())
                                flags |= TypeAnalysisFlags.SupportsExplicit;
                        }
                        else
                        {
                            flags |= TypeAnalysisFlags.Explicit;

                            if (info.ElementConversion.IsIdentity)
                                flags |= TypeAnalysisFlags.SupportsImplicit;
                        }
                    }

                    return flags;
                }
            }

            return TypeAnalysisFlags.None;
        }
    }
}
