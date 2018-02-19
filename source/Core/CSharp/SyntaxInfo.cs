// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp
{
    /// <summary>
    /// Serves as a factory for types in Roslynator.CSharp.Syntax namespace.
    /// </summary>
    public static class SyntaxInfo
    {
        /// <summary>
        /// Creates a new <see cref="Syntax.AccessibilityInfo"/> from the specified declaration.
        /// Check <see cref="Syntax.AccessibilityInfo.Success"/> to see if the operation succeded.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(SyntaxNode declaration)
        {
            return Syntax.AccessibilityInfo.Create(declaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(ClassDeclarationSyntax classDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(classDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructorDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(ConstructorDeclarationSyntax constructorDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(constructorDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conversionOperatorDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(conversionOperatorDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegateDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(DelegateDeclarationSyntax delegateDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(delegateDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destructorDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(DestructorDeclarationSyntax destructorDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(destructorDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(EnumDeclarationSyntax enumDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(enumDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(EventDeclarationSyntax eventDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(eventDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventFieldDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(eventFieldDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(FieldDeclarationSyntax fieldDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(fieldDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexerDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(IndexerDeclarationSyntax indexerDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(indexerDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(interfaceDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(MethodDeclarationSyntax methodDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(methodDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(OperatorDeclarationSyntax operatorDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(operatorDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(PropertyDeclarationSyntax propertyDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(propertyDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="structDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(StructDeclarationSyntax structDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(structDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="incompleteMember"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(IncompleteMemberSyntax incompleteMember)
        {
            return Syntax.AccessibilityInfo.Create(incompleteMember);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessorDeclaration"></param>
        /// <returns></returns>
        public static AccessibilityInfo AccessibilityInfo(AccessorDeclarationSyntax accessorDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(accessorDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static AsExpressionInfo AsExpressionInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.AsExpressionInfo.Create(
                node,
                walkDownParentheses,
                allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binaryExpression"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static AsExpressionInfo AsExpressionInfo(
            BinaryExpressionSyntax binaryExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.AsExpressionInfo.Create(
                binaryExpression,
                walkDownParentheses,
                allowMissing);
        }

        internal static BinaryExpressionChainInfo BinaryExpressionChainInfo(
            SyntaxNode node,
            bool walkDownParentheses = true)
        {
            return Syntax.BinaryExpressionChainInfo.Create(
                node,
                walkDownParentheses);
        }

        internal static BinaryExpressionChainInfo BinaryExpressionChainInfo(BinaryExpressionSyntax binaryExpression)
        {
            return Syntax.BinaryExpressionChainInfo.Create(binaryExpression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static BinaryExpressionInfo BinaryExpressionInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.BinaryExpressionInfo.Create(
                node,
                walkDownParentheses,
                allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binaryExpression"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static BinaryExpressionInfo BinaryExpressionInfo(
            BinaryExpressionSyntax binaryExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.BinaryExpressionInfo.Create(
                binaryExpression,
                walkDownParentheses,
                allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static ConditionalExpressionInfo ConditionalExpressionInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.ConditionalExpressionInfo.Create(
                node,
                walkDownParentheses,
                allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionalExpression"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static ConditionalExpressionInfo ConditionalExpressionInfo(
            ConditionalExpressionSyntax conditionalExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.ConditionalExpressionInfo.Create(
                conditionalExpression,
                walkDownParentheses,
                allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeParameterConstraint"></param>
        /// <returns></returns>
        public static GenericInfo GenericInfo(TypeParameterConstraintSyntax typeParameterConstraint)
        {
            return Syntax.GenericInfo.Create(typeParameterConstraint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constraintClause"></param>
        /// <returns></returns>
        public static GenericInfo GenericInfo(TypeParameterConstraintClauseSyntax constraintClause)
        {
            return Syntax.GenericInfo.Create(constraintClause);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static GenericInfo GenericInfo(SyntaxNode declaration)
        {
            return Syntax.GenericInfo.Create(declaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeParameterList"></param>
        /// <returns></returns>
        public static GenericInfo GenericInfo(TypeParameterListSyntax typeParameterList)
        {
            return Syntax.GenericInfo.Create(typeParameterList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classDeclaration"></param>
        /// <returns></returns>
        public static GenericInfo GenericInfo(ClassDeclarationSyntax classDeclaration)
        {
            return Syntax.GenericInfo.Create(classDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegateDeclaration"></param>
        /// <returns></returns>
        public static GenericInfo GenericInfo(DelegateDeclarationSyntax delegateDeclaration)
        {
            return Syntax.GenericInfo.Create(delegateDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceDeclaration"></param>
        /// <returns></returns>
        public static GenericInfo GenericInfo(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            return Syntax.GenericInfo.Create(interfaceDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localFunctionStatement"></param>
        /// <returns></returns>
        public static GenericInfo GenericInfo(LocalFunctionStatementSyntax localFunctionStatement)
        {
            return Syntax.GenericInfo.Create(localFunctionStatement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodDeclaration"></param>
        /// <returns></returns>
        public static GenericInfo GenericInfo(MethodDeclarationSyntax methodDeclaration)
        {
            return Syntax.GenericInfo.Create(methodDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="structDeclaration"></param>
        /// <returns></returns>
        public static GenericInfo GenericInfo(StructDeclarationSyntax structDeclaration)
        {
            return Syntax.GenericInfo.Create(structDeclaration);
        }

        internal static HexNumericLiteralExpressionInfo HexNumericLiteralExpressionInfo(SyntaxNode node, bool walkDownParentheses = true)
        {
            return Syntax.HexNumericLiteralExpressionInfo.Create(node, walkDownParentheses);
        }

        internal static HexNumericLiteralExpressionInfo HexNumericLiteralExpressionInfo(LiteralExpressionSyntax literalExpression)
        {
            return Syntax.HexNumericLiteralExpressionInfo.Create(literalExpression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifStatement"></param>
        /// <returns></returns>
        public static IfStatementInfo IfStatementInfo(IfStatementSyntax ifStatement)
        {
            return Syntax.IfStatementInfo.Create(ifStatement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localDeclarationStatement"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static LocalDeclarationStatementInfo LocalDeclarationStatementInfo(
            LocalDeclarationStatementSyntax localDeclarationStatement,
            bool allowMissing = false)
        {
            return Syntax.LocalDeclarationStatementInfo.Create(localDeclarationStatement, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static LocalDeclarationStatementInfo LocalDeclarationStatementInfo(
            ExpressionSyntax expression,
            bool allowMissing = false)
        {
            return Syntax.LocalDeclarationStatementInfo.Create(expression, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static MemberDeclarationsInfo MemberDeclarationsInfo(SyntaxNode declaration)
        {
            return Syntax.MemberDeclarationsInfo.Create(declaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static MemberDeclarationsInfo MemberDeclarationsInfo(NamespaceDeclarationSyntax declaration)
        {
            return Syntax.MemberDeclarationsInfo.Create(declaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static MemberDeclarationsInfo MemberDeclarationsInfo(ClassDeclarationSyntax declaration)
        {
            return Syntax.MemberDeclarationsInfo.Create(declaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static MemberDeclarationsInfo MemberDeclarationsInfo(StructDeclarationSyntax declaration)
        {
            return Syntax.MemberDeclarationsInfo.Create(declaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public static MemberDeclarationsInfo MemberDeclarationsInfo(InterfaceDeclarationSyntax declaration)
        {
            return Syntax.MemberDeclarationsInfo.Create(declaration);
        }

        internal static MemberDeclarationsInfo MemberDeclarationsInfo(MemberDeclarationsSelection selectedMembers)
        {
            return Syntax.MemberDeclarationsInfo.Create(selectedMembers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static MemberInvocationExpressionInfo MemberInvocationExpressionInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.MemberInvocationExpressionInfo.Create(
                node,
                walkDownParentheses,
                allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocationExpression"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static MemberInvocationExpressionInfo MemberInvocationExpressionInfo(
            InvocationExpressionSyntax invocationExpression,
            bool allowMissing = false)
        {
            return Syntax.MemberInvocationExpressionInfo.Create(
                invocationExpression,
                allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static MemberInvocationStatementInfo MemberInvocationStatementInfo(
            SyntaxNode node,
            bool allowMissing = false)
        {
            return Syntax.MemberInvocationStatementInfo.Create(
                node,
                allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressionStatement"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static MemberInvocationStatementInfo MemberInvocationStatementInfo(
            ExpressionStatementSyntax expressionStatement,
            bool allowMissing = false)
        {
            return Syntax.MemberInvocationStatementInfo.Create(
                expressionStatement,
                allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocationExpression"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static MemberInvocationStatementInfo MemberInvocationStatementInfo(
            InvocationExpressionSyntax invocationExpression,
            bool allowMissing = false)
        {
            return Syntax.MemberInvocationStatementInfo.Create(
                invocationExpression,
                allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(SyntaxNode node)
        {
            return Syntax.ModifiersInfo.Create(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(ClassDeclarationSyntax classDeclaration)
        {
            return Syntax.ModifiersInfo.Create(classDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructorDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(ConstructorDeclarationSyntax constructorDeclaration)
        {
            return Syntax.ModifiersInfo.Create(constructorDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conversionOperatorDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            return Syntax.ModifiersInfo.Create(conversionOperatorDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegateDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(DelegateDeclarationSyntax delegateDeclaration)
        {
            return Syntax.ModifiersInfo.Create(delegateDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destructorDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(DestructorDeclarationSyntax destructorDeclaration)
        {
            return Syntax.ModifiersInfo.Create(destructorDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(EnumDeclarationSyntax enumDeclaration)
        {
            return Syntax.ModifiersInfo.Create(enumDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(EventDeclarationSyntax eventDeclaration)
        {
            return Syntax.ModifiersInfo.Create(eventDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventFieldDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            return Syntax.ModifiersInfo.Create(eventFieldDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(FieldDeclarationSyntax fieldDeclaration)
        {
            return Syntax.ModifiersInfo.Create(fieldDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexerDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(IndexerDeclarationSyntax indexerDeclaration)
        {
            return Syntax.ModifiersInfo.Create(indexerDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            return Syntax.ModifiersInfo.Create(interfaceDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(MethodDeclarationSyntax methodDeclaration)
        {
            return Syntax.ModifiersInfo.Create(methodDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(OperatorDeclarationSyntax operatorDeclaration)
        {
            return Syntax.ModifiersInfo.Create(operatorDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(PropertyDeclarationSyntax propertyDeclaration)
        {
            return Syntax.ModifiersInfo.Create(propertyDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="structDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(StructDeclarationSyntax structDeclaration)
        {
            return Syntax.ModifiersInfo.Create(structDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="incompleteMember"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(IncompleteMemberSyntax incompleteMember)
        {
            return Syntax.ModifiersInfo.Create(incompleteMember);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessorDeclaration"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(AccessorDeclarationSyntax accessorDeclaration)
        {
            return Syntax.ModifiersInfo.Create(accessorDeclaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localDeclarationStatement"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(LocalDeclarationStatementSyntax localDeclarationStatement)
        {
            return Syntax.ModifiersInfo.Create(localDeclarationStatement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localFunctionStatement"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(LocalFunctionStatementSyntax localFunctionStatement)
        {
            return Syntax.ModifiersInfo.Create(localFunctionStatement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static ModifiersInfo ModifiersInfo(ParameterSyntax parameter)
        {
            return Syntax.ModifiersInfo.Create(parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="allowedStyles"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static NullCheckExpressionInfo NullCheckExpressionInfo(
            SyntaxNode node,
            NullCheckStyles allowedStyles = NullCheckStyles.ComparisonToNull | NullCheckStyles.IsPattern,
            bool walkDownParentheses = true,
            bool allowMissing = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Syntax.NullCheckExpressionInfo.Create(
                node,
                allowedStyles,
                walkDownParentheses,
                allowMissing,
                cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="semanticModel"></param>
        /// <param name="allowedStyles"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static NullCheckExpressionInfo NullCheckExpressionInfo(
            SyntaxNode node,
            SemanticModel semanticModel,
            NullCheckStyles allowedStyles = NullCheckStyles.All,
            bool walkDownParentheses = true,
            bool allowMissing = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Syntax.NullCheckExpressionInfo.Create(
                node,
                semanticModel,
                allowedStyles,
                walkDownParentheses,
                allowMissing,
                cancellationToken);
        }

        internal static ParametersInfo ParametersInfo(
            ConstructorDeclarationSyntax constructorDeclaration,
            bool allowMissing = false)
        {
            return Syntax.ParametersInfo.Create(constructorDeclaration, allowMissing);
        }

        internal static ParametersInfo ParametersInfo(
            MethodDeclarationSyntax methodDeclaration,
            bool allowMissing = false)
        {
            return Syntax.ParametersInfo.Create(methodDeclaration, allowMissing);
        }

        internal static ParametersInfo ParametersInfo(
            OperatorDeclarationSyntax operatorDeclaration,
            bool allowMissing = false)
        {
            return Syntax.ParametersInfo.Create(operatorDeclaration, allowMissing);
        }

        internal static ParametersInfo ParametersInfo(
            ConversionOperatorDeclarationSyntax conversionOperatorDeclaration,
            bool allowMissing = false)
        {
            return Syntax.ParametersInfo.Create(conversionOperatorDeclaration, allowMissing);
        }

        internal static ParametersInfo ParametersInfo(
            DelegateDeclarationSyntax delegateDeclaration,
            bool allowMissing = false)
        {
            return Syntax.ParametersInfo.Create(delegateDeclaration, allowMissing);
        }

        internal static ParametersInfo ParametersInfo(
            LocalFunctionStatementSyntax localFunction,
            bool allowMissing = false)
        {
            return Syntax.ParametersInfo.Create(localFunction, allowMissing);
        }

        internal static ParametersInfo ParametersInfo(
            IndexerDeclarationSyntax indexerDeclaration,
            bool allowMissing = false)
        {
            return Syntax.ParametersInfo.Create(indexerDeclaration, allowMissing);
        }

        internal static ParametersInfo ParametersInfo(
            SimpleLambdaExpressionSyntax simpleLambda,
            bool allowMissing = false)
        {
            return Syntax.ParametersInfo.Create(simpleLambda, allowMissing);
        }

        internal static ParametersInfo ParametersInfo(
            ParenthesizedLambdaExpressionSyntax parenthesizedLambda,
            bool allowMissing = false)
        {
            return Syntax.ParametersInfo.Create(parenthesizedLambda, allowMissing);
        }

        internal static ParametersInfo ParametersInfo(
            AnonymousMethodExpressionSyntax anonymousMethod,
            bool allowMissing = false)
        {
            return Syntax.ParametersInfo.Create(anonymousMethod, allowMissing);
        }

        internal static RegionInfo RegionInfo(SyntaxNode node)
        {
            return Syntax.RegionInfo.Create(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regionDirective"></param>
        /// <returns></returns>
        public static RegionInfo RegionInfo(RegionDirectiveTriviaSyntax regionDirective)
        {
            return Syntax.RegionInfo.Create(regionDirective);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endRegionDirective"></param>
        /// <returns></returns>
        public static RegionInfo RegionInfo(EndRegionDirectiveTriviaSyntax endRegionDirective)
        {
            return Syntax.RegionInfo.Create(endRegionDirective);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SimpleAssignmentExpressionInfo SimpleAssignmentExpressionInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleAssignmentExpressionInfo.Create(node, walkDownParentheses, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assignmentExpression"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SimpleAssignmentExpressionInfo SimpleAssignmentExpressionInfo(
            AssignmentExpressionSyntax assignmentExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleAssignmentExpressionInfo.Create(assignmentExpression, walkDownParentheses, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SimpleAssignmentStatementInfo SimpleAssignmentStatementInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleAssignmentStatementInfo.Create(node, walkDownParentheses, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assignmentExpression"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SimpleAssignmentStatementInfo SimpleAssignmentStatementInfo(
            AssignmentExpressionSyntax assignmentExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleAssignmentStatementInfo.Create(assignmentExpression, walkDownParentheses, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressionStatement"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SimpleAssignmentStatementInfo SimpleAssignmentStatementInfo(
            ExpressionStatementSyntax expressionStatement,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleAssignmentStatementInfo.Create(expressionStatement, walkDownParentheses, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifStatement"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SimpleIfElseInfo SimpleIfElseInfo(
            IfStatementSyntax ifStatement,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleIfElseInfo.Create(ifStatement, walkDownParentheses, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SimpleIfStatementInfo SimpleIfStatementInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleIfStatementInfo.Create(node, walkDownParentheses, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ifStatement"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SimpleIfStatementInfo SimpleIfStatementInfo(
            IfStatementSyntax ifStatement,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleIfStatementInfo.Create(ifStatement, walkDownParentheses, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localDeclarationStatement"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SingleLocalDeclarationStatementInfo SingleLocalDeclarationStatementInfo(
            LocalDeclarationStatementSyntax localDeclarationStatement,
            bool allowMissing = false)
        {
            return Syntax.SingleLocalDeclarationStatementInfo.Create(localDeclarationStatement, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variableDeclaration"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SingleLocalDeclarationStatementInfo SingleLocalDeclarationStatementInfo(
            VariableDeclarationSyntax variableDeclaration,
            bool allowMissing = false)
        {
            return Syntax.SingleLocalDeclarationStatementInfo.Create(variableDeclaration, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SingleLocalDeclarationStatementInfo SingleLocalDeclarationStatementInfo(ExpressionSyntax value)
        {
            return Syntax.SingleLocalDeclarationStatementInfo.Create(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="walkDownParentheses"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SingleParameterLambdaExpressionInfo SingleParameterLambdaExpressionInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SingleParameterLambdaExpressionInfo.Create(node, walkDownParentheses, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambdaExpression"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static SingleParameterLambdaExpressionInfo SingleParameterLambdaExpressionInfo(
            LambdaExpressionSyntax lambdaExpression,
            bool allowMissing = false)
        {
            return Syntax.SingleParameterLambdaExpressionInfo.Create(lambdaExpression, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public static StatementsInfo StatementsInfo(StatementSyntax statement)
        {
            return Syntax.StatementsInfo.Create(statement);
        }

        internal static StatementsInfo StatementsInfo(BlockSyntax block)
        {
            return Syntax.StatementsInfo.Create(block);
        }

        internal static StatementsInfo StatementsInfo(SwitchSectionSyntax switchSection)
        {
            return Syntax.StatementsInfo.Create(switchSection);
        }

        internal static StatementsInfo StatementsInfo(StatementsSelection selectedStatements)
        {
            return Syntax.StatementsInfo.Create(selectedStatements);
        }

        internal static StringConcatenationExpressionInfo StringConcatenationExpressionInfo(
            BinaryExpressionSyntax binaryExpression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Syntax.StringConcatenationExpressionInfo.Create(binaryExpression, semanticModel, cancellationToken);
        }

        internal static StringConcatenationExpressionInfo StringConcatenationExpressionInfo(
            BinaryExpressionSelection binaryExpressionSelection,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Syntax.StringConcatenationExpressionInfo.Create(binaryExpressionSelection, semanticModel, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="allowMissing"></param>
        /// <returns></returns>
        public static TypeParameterConstraintInfo TypeParameterConstraintInfo(TypeParameterConstraintSyntax constraint, bool allowMissing = false)
        {
            return Syntax.TypeParameterConstraintInfo.Create(constraint, allowMissing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeParameter"></param>
        /// <returns></returns>
        public static TypeParameterInfo TypeParameterInfo(TypeParameterSyntax typeParameter)
        {
            return Syntax.TypeParameterInfo.Create(typeParameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        public static XmlElementInfo XmlElementInfo(XmlNodeSyntax xmlNode)
        {
            return Syntax.XmlElementInfo.Create(xmlNode);
        }
    }
}