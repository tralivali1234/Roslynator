﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp
{
    internal static class CSharpUtility
    {
        private static readonly SymbolDisplayFormat _symbolDisplayFormat = new SymbolDisplayFormat(
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

        public static string GetCountOrLengthPropertyName(
            ExpressionSyntax expression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ITypeSymbol typeSymbol = semanticModel.GetTypeSymbol(expression, cancellationToken);

            if (typeSymbol == null)
                return null;

            return SymbolUtility.GetCountOrLengthPropertyName(typeSymbol, semanticModel, expression.SpanStart);
        }

        public static bool IsNamespaceInScope(
            SyntaxNode node,
            INamespaceSymbol namespaceSymbol,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (namespaceSymbol == null)
                throw new ArgumentNullException(nameof(namespaceSymbol));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            foreach (SyntaxNode ancestor in node.Ancestors())
            {
                switch (ancestor.Kind())
                {
                    case SyntaxKind.NamespaceDeclaration:
                        {
                            var namespaceDeclaration = (NamespaceDeclarationSyntax)ancestor;

                            if (IsNamespace(namespaceSymbol, namespaceDeclaration.Name, semanticModel, cancellationToken)
                                || IsNamespace(namespaceSymbol, namespaceDeclaration.Usings, semanticModel, cancellationToken))
                            {
                                return true;
                            }

                            break;
                        }
                    case SyntaxKind.CompilationUnit:
                        {
                            var compilationUnit = (CompilationUnitSyntax)ancestor;

                            if (IsNamespace(namespaceSymbol, compilationUnit.Usings, semanticModel, cancellationToken))
                                return true;

                            break;
                        }
                }
            }

            return false;
        }

        private static bool IsNamespace(
            INamespaceSymbol namespaceSymbol,
            SyntaxList<UsingDirectiveSyntax> usings,
            SemanticModel semanticModel,
            CancellationToken cancellationToken)
        {
            foreach (UsingDirectiveSyntax usingDirective in usings)
            {
                if (!usingDirective.StaticKeyword.IsKind(SyntaxKind.StaticKeyword)
                    && usingDirective.Alias == null
                    && IsNamespace(namespaceSymbol, usingDirective.Name, semanticModel, cancellationToken))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsNamespace(
            INamespaceSymbol namespaceSymbol,
            NameSyntax name,
            SemanticModel semanticModel,
            CancellationToken cancellationToken)
        {
            if (name != null)
            {
                ISymbol symbol = semanticModel.GetSymbol(name, cancellationToken);

                if (symbol?.Kind == SymbolKind.Namespace)
                {
                    string namespaceText = namespaceSymbol.ToString();

                    if (string.Equals(namespaceText, symbol.ToString(), StringComparison.Ordinal))
                    {
                        return true;
                    }
                    else if (name.IsParentKind(SyntaxKind.NamespaceDeclaration))
                    {
                        INamespaceSymbol containingNamespace = symbol.ContainingNamespace;

                        while (containingNamespace != null)
                        {
                            if (string.Equals(namespaceText, containingNamespace.ToString(), StringComparison.Ordinal))
                                return true;

                            containingNamespace = containingNamespace.ContainingNamespace;
                        }
                    }
                }
            }

            return false;
        }

        public static bool IsStaticClassInScope(
            SyntaxNode node,
            INamedTypeSymbol staticClassSymbol,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (staticClassSymbol == null)
                throw new ArgumentNullException(nameof(staticClassSymbol));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            foreach (SyntaxNode ancestor in node.Ancestors())
            {
                foreach (UsingDirectiveSyntax usingDirective in SyntaxInfo.UsingDirectiveListInfo(ancestor).Usings)
                {
                    if (usingDirective.StaticKeyword.IsKind(SyntaxKind.StaticKeyword))
                    {
                        NameSyntax name = usingDirective.Name;

                        if (name != null
                            && staticClassSymbol.Equals(semanticModel.GetSymbol(name, cancellationToken)))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool IsEmptyStringExpression(
            ExpressionSyntax expression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            SyntaxKind kind = expression.Kind();

            if (kind == SyntaxKind.StringLiteralExpression)
            {
                return ((LiteralExpressionSyntax)expression).Token.ValueText.Length == 0;
            }
            else if (kind == SyntaxKind.InterpolatedStringExpression)
            {
                return !((InterpolatedStringExpressionSyntax)expression).Contents.Any();
            }
            else if (kind == SyntaxKind.SimpleMemberAccessExpression)
            {
                var memberAccess = (MemberAccessExpressionSyntax)expression;

                if (memberAccess.Name?.Identifier.ValueText == "Empty")
                {
                    ISymbol symbol = semanticModel.GetSymbol(memberAccess, cancellationToken);

                    if (symbol?.Kind == SymbolKind.Field)
                    {
                        var fieldSymbol = (IFieldSymbol)symbol;

                        if (SymbolUtility.IsPublicStaticReadOnly(fieldSymbol, "Empty")
                            && fieldSymbol.ContainingType?.SpecialType == SpecialType.System_String
                            && fieldSymbol.Type.IsString())
                        {
                            return true;
                        }
                    }
                }
            }

            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

            if (optional.HasValue)
            {
                var value = optional.Value as string;

                return value?.Length == 0;
            }

            return false;
        }

        public static NameSyntax EnsureFullyQualifiedName(
            NameSyntax name,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ISymbol symbol = semanticModel.GetSymbol(name, cancellationToken);

            if (symbol != null)
            {
                if (semanticModel.GetAliasInfo(name, cancellationToken) != null
                    || !symbol.ContainingNamespace.IsGlobalNamespace)
                {
                    SymbolKind kind = symbol.Kind;

                    if (kind == SymbolKind.Namespace)
                    {
                        return SyntaxFactory.ParseName(symbol.ToString()).WithTriviaFrom(name);
                    }
                    else if (kind == SymbolKind.NamedType)
                    {
                        return (NameSyntax)((INamedTypeSymbol)symbol).ToTypeSyntax(_symbolDisplayFormat).WithTriviaFrom(name);
                    }
                }
            }

            return name;
        }

        public static bool IsNameOfExpression(
            SyntaxNode node,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return node.IsKind(SyntaxKind.InvocationExpression)
                && IsNameOfExpression((InvocationExpressionSyntax)node, semanticModel, cancellationToken);
        }

        public static bool IsNameOfExpression(
            InvocationExpressionSyntax invocationExpression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ExpressionSyntax expression = invocationExpression.Expression;

            if (expression?.Kind() == SyntaxKind.IdentifierName)
            {
                var identifierName = (IdentifierNameSyntax)expression;

                if (string.Equals(identifierName.Identifier.ValueText, "nameof", StringComparison.Ordinal)
                    && semanticModel.GetSymbol(invocationExpression, cancellationToken) == null)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsPropertyOfNullableOfT(
            IdentifierNameSyntax identifierName,
            string name,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (identifierName == null)
                return false;

            if (!string.Equals(identifierName.Identifier.ValueText, name, StringComparison.Ordinal))
                return false;

            ISymbol symbol = semanticModel.GetSymbol(identifierName, cancellationToken);

            return SymbolUtility.IsPropertyOfNullableOfT(symbol, name);
        }

        public static bool IsStringConcatenation(BinaryExpressionSyntax addExpression, SemanticModel semanticModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            return addExpression.Kind() == SyntaxKind.AddExpression
                && SymbolUtility.IsStringAdditionOperator(semanticModel.GetMethodSymbol(addExpression, cancellationToken));
        }

        public static bool IsStringLiteralConcatenation(BinaryExpressionSyntax binaryExpression)
        {
            if (binaryExpression?.Kind() != SyntaxKind.AddExpression)
                return false;

            while (true)
            {
                if (binaryExpression.Right?.WalkDownParentheses().Kind() != SyntaxKind.StringLiteralExpression)
                    return false;

                ExpressionSyntax left = binaryExpression.Left?.WalkDownParentheses();

                switch (left?.Kind())
                {
                    case SyntaxKind.StringLiteralExpression:
                        {
                            return true;
                        }
                    case SyntaxKind.AddExpression:
                        {
                            binaryExpression = (BinaryExpressionSyntax)left;
                            break;
                        }
                    default:
                        {
                            return false;
                        }
                }
            }
        }

        public static bool IsStringExpression(ExpressionSyntax expression, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            if (expression == null)
                return false;

            if (expression
                .WalkDownParentheses()
                .Kind()
                .Is(SyntaxKind.StringLiteralExpression, SyntaxKind.InterpolatedStringExpression))
            {
                return true;
            }

            return semanticModel.GetTypeInfo(expression, cancellationToken)
                .Type?
                .SpecialType == SpecialType.System_String;
        }

        public static bool IsBooleanExpression(
            ExpressionSyntax expression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (expression == null)
                return false;

            return CSharpFacts.IsBooleanExpression(expression.WalkDownParentheses().Kind())
                || semanticModel
                    .GetTypeInfo(expression, cancellationToken)
                    .ConvertedType?
                    .SpecialType == SpecialType.System_Boolean;
        }

        public static SyntaxToken GetIdentifier(SyntaxNode node)
        {
            switch (node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    return ((ClassDeclarationSyntax)node).Identifier;
                case SyntaxKind.StructDeclaration:
                    return ((StructDeclarationSyntax)node).Identifier;
                case SyntaxKind.InterfaceDeclaration:
                    return ((InterfaceDeclarationSyntax)node).Identifier;
                case SyntaxKind.EnumDeclaration:
                    return ((EnumDeclarationSyntax)node).Identifier;
                case SyntaxKind.DelegateDeclaration:
                    return ((DelegateDeclarationSyntax)node).Identifier;
                case SyntaxKind.MethodDeclaration:
                    return ((MethodDeclarationSyntax)node).Identifier;
                case SyntaxKind.ConstructorDeclaration:
                    return ((ConstructorDeclarationSyntax)node).Identifier;
                case SyntaxKind.PropertyDeclaration:
                    return ((PropertyDeclarationSyntax)node).Identifier;
                case SyntaxKind.IndexerDeclaration:
                    return ((IndexerDeclarationSyntax)node).ThisKeyword;
                case SyntaxKind.EventDeclaration:
                    return ((EventDeclarationSyntax)node).Identifier;
                case SyntaxKind.EventFieldDeclaration:
                    return ((EventFieldDeclarationSyntax)node).Declaration?.Variables.FirstOrDefault()?.Identifier ?? default(SyntaxToken);
                case SyntaxKind.FieldDeclaration:
                    return ((FieldDeclarationSyntax)node).Declaration?.Variables.FirstOrDefault()?.Identifier ?? default(SyntaxToken);
                case SyntaxKind.VariableDeclarator:
                    return ((VariableDeclaratorSyntax)node).Identifier;
            }

            return default(SyntaxToken);
        }

        public static bool IsPartOfExpressionThatMustBeConstant(LiteralExpressionSyntax literalExpression)
        {
            for (SyntaxNode parent = literalExpression.Parent; parent != null; parent = parent.Parent)
            {
                switch (parent.Kind())
                {
                    case SyntaxKind.AttributeArgument:
                    case SyntaxKind.Parameter:
                    case SyntaxKind.CaseSwitchLabel:
                        return true;
                    case SyntaxKind.FieldDeclaration:
                        return ((FieldDeclarationSyntax)parent).Modifiers.Contains(SyntaxKind.ConstKeyword);
                    case SyntaxKind.LocalDeclarationStatement:
                        return ((LocalDeclarationStatementSyntax)parent).Modifiers.Contains(SyntaxKind.ConstKeyword);
                    case SyntaxKind.Block:
                    case SyntaxKind.ExpressionStatement:
                    case SyntaxKind.EmptyStatement:
                    case SyntaxKind.LabeledStatement:
                    case SyntaxKind.GotoStatement:
                    case SyntaxKind.GotoCaseStatement:
                    case SyntaxKind.GotoDefaultStatement:
                    case SyntaxKind.BreakStatement:
                    case SyntaxKind.ContinueStatement:
                    case SyntaxKind.ReturnStatement:
                    case SyntaxKind.YieldReturnStatement:
                    case SyntaxKind.YieldBreakStatement:
                    case SyntaxKind.ThrowStatement:
                    case SyntaxKind.WhileStatement:
                    case SyntaxKind.DoStatement:
                    case SyntaxKind.ForStatement:
                    case SyntaxKind.ForEachStatement:
                    case SyntaxKind.ForEachVariableStatement:
                    case SyntaxKind.UsingStatement:
                    case SyntaxKind.FixedStatement:
                    case SyntaxKind.CheckedStatement:
                    case SyntaxKind.UncheckedStatement:
                    case SyntaxKind.UnsafeStatement:
                    case SyntaxKind.LockStatement:
                    case SyntaxKind.IfStatement:
                    case SyntaxKind.SwitchStatement:
                    case SyntaxKind.TryStatement:
                    case SyntaxKind.LocalFunctionStatement:
                    case SyntaxKind.GlobalStatement:
                    case SyntaxKind.NamespaceDeclaration:
                    case SyntaxKind.ClassDeclaration:
                    case SyntaxKind.StructDeclaration:
                    case SyntaxKind.InterfaceDeclaration:
                    case SyntaxKind.EnumDeclaration:
                    case SyntaxKind.DelegateDeclaration:
                    case SyntaxKind.EnumMemberDeclaration:
                    case SyntaxKind.EventFieldDeclaration:
                    case SyntaxKind.MethodDeclaration:
                    case SyntaxKind.OperatorDeclaration:
                    case SyntaxKind.ConversionOperatorDeclaration:
                    case SyntaxKind.ConstructorDeclaration:
                    case SyntaxKind.DestructorDeclaration:
                    case SyntaxKind.PropertyDeclaration:
                    case SyntaxKind.EventDeclaration:
                    case SyntaxKind.IndexerDeclaration:
                    case SyntaxKind.GetAccessorDeclaration:
                    case SyntaxKind.SetAccessorDeclaration:
                    case SyntaxKind.AddAccessorDeclaration:
                    case SyntaxKind.RemoveAccessorDeclaration:
                    case SyntaxKind.UnknownAccessorDeclaration:
                    case SyntaxKind.IncompleteMember:
                        return false;
                }
            }

            return false;
        }

        public static IEnumerable<SyntaxNode> EnumerateExpressionChain(ExpressionSyntax expression)
        {
            SyntaxNode e = expression;

            yield return e;

            while (true)
            {
                ExpressionSyntax last = GetLastChild(e);

                if (last != null)
                {
                    e = last;
                }
                else
                {
                    while (e != expression
                        && IsFirstChild(e))
                    {
                        e = e.Parent;
                    }

                    if (e == expression)
                        break;

                    e = GetPreviousSibling(e);
                }

                yield return e;
            }

            ExpressionSyntax GetLastChild(SyntaxNode node)
            {
                switch (node?.Kind())
                {
                    case SyntaxKind.ConditionalAccessExpression:
                        return ((ConditionalAccessExpressionSyntax)node).WhenNotNull;
                    case SyntaxKind.MemberBindingExpression:
                        return ((MemberBindingExpressionSyntax)node).Name;
                    case SyntaxKind.SimpleMemberAccessExpression:
                        return ((MemberAccessExpressionSyntax)node).Name;
                    case SyntaxKind.ElementAccessExpression:
                        return ((ElementAccessExpressionSyntax)node).Expression;
                    case SyntaxKind.InvocationExpression:
                        return ((InvocationExpressionSyntax)node).Expression;
                }

                return null;
            }

            SyntaxNode GetPreviousSibling(SyntaxNode node)
            {
                SyntaxNode parent = node.Parent;

                switch (parent.Kind())
                {
                    case SyntaxKind.ConditionalAccessExpression:
                        {
                            var conditionalAccess = (ConditionalAccessExpressionSyntax)parent;

                            if (conditionalAccess.WhenNotNull == node)
                                return conditionalAccess.Expression;

                            break;
                        }
                    case SyntaxKind.SimpleMemberAccessExpression:
                        {
                            var memberAccess = (MemberAccessExpressionSyntax)parent;

                            if (memberAccess.Name == node)
                                return memberAccess.Expression;

                            break;
                        }
                }

                return null;
            }

            bool IsFirstChild(SyntaxNode node)
            {
                SyntaxNode parent = node.Parent;

                switch (parent.Kind())
                {
                    case SyntaxKind.ConditionalAccessExpression:
                        return ((ConditionalAccessExpressionSyntax)parent).Expression == node;
                    case SyntaxKind.SimpleMemberAccessExpression:
                        return ((MemberAccessExpressionSyntax)parent).Expression == node;
                }

                return true;
            }
        }

        public static ArrowExpressionClauseSyntax GetExpressionBody(SyntaxNode node)
        {
            switch (node.Kind())
            {
                case SyntaxKind.MethodDeclaration:
                    return ((MethodDeclarationSyntax)node).ExpressionBody;
                case SyntaxKind.PropertyDeclaration:
                    return ((PropertyDeclarationSyntax)node).ExpressionBody;
                case SyntaxKind.IndexerDeclaration:
                    return ((IndexerDeclarationSyntax)node).ExpressionBody;
                case SyntaxKind.OperatorDeclaration:
                    return ((OperatorDeclarationSyntax)node).ExpressionBody;
                case SyntaxKind.ConversionOperatorDeclaration:
                    return ((ConversionOperatorDeclarationSyntax)node).ExpressionBody;
                case SyntaxKind.ConstructorDeclaration:
                    return ((ConstructorDeclarationSyntax)node).ExpressionBody;
                case SyntaxKind.DestructorDeclaration:
                    return ((DestructorDeclarationSyntax)node).ExpressionBody;
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                    return ((AccessorDeclarationSyntax)node).ExpressionBody;
                case SyntaxKind.LocalFunctionStatement:
                    return ((LocalFunctionStatementSyntax)node).ExpressionBody;
            }

            Debug.Assert(!CSharpFacts.CanHaveExpressionBody(node.Kind()), node.Kind().ToString());

            return null;
        }

        public static bool IsConditionallyAccessed(SyntaxNode node)
        {
            SyntaxNode prev = node;

            for (SyntaxNode parent = node.Parent; parent != null; parent = parent.Parent)
            {
                switch (parent.Kind())
                {
                    case SyntaxKind.SimpleMemberAccessExpression:
                    case SyntaxKind.ElementAccessExpression:
                    case SyntaxKind.InvocationExpression:
                        {
                            prev = parent;
                            continue;
                        }
                    case SyntaxKind.ConditionalAccessExpression:
                        {
                            return ((ConditionalAccessExpressionSyntax)parent).WhenNotNull == prev;
                        }
                    default:
                        {
                            return false;
                        }
                }
            }

            return false;
        }

        public static ExpressionSyntax GetTopmostExpressionInCallChain(ExpressionSyntax expression)
        {
            return (ExpressionSyntax)expression.WalkUp(f => f.IsKind(
                SyntaxKind.ConditionalAccessExpression,
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxKind.ElementAccessExpression,
                SyntaxKind.MemberBindingExpression,
                SyntaxKind.InvocationExpression));
        }
    }
}
