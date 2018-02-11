// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    public static class SyntaxInfo
    {
        public static AccessibilityInfo AccessibilityInfo(SyntaxNode declaration)
        {
            return Syntax.AccessibilityInfo.Create(declaration);
        }

        public static AccessibilityInfo AccessibilityInfo(ClassDeclarationSyntax classDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(classDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(ConstructorDeclarationSyntax constructorDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(constructorDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(conversionOperatorDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(DelegateDeclarationSyntax delegateDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(delegateDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(DestructorDeclarationSyntax destructorDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(destructorDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(EnumDeclarationSyntax enumDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(enumDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(EventDeclarationSyntax eventDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(eventDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(eventFieldDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(FieldDeclarationSyntax fieldDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(fieldDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(IndexerDeclarationSyntax indexerDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(indexerDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(interfaceDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(MethodDeclarationSyntax methodDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(methodDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(OperatorDeclarationSyntax operatorDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(operatorDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(PropertyDeclarationSyntax propertyDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(propertyDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(StructDeclarationSyntax structDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(structDeclaration);
        }

        public static AccessibilityInfo AccessibilityInfo(IncompleteMemberSyntax incompleteMember)
        {
            return Syntax.AccessibilityInfo.Create(incompleteMember);
        }

        public static AccessibilityInfo AccessibilityInfo(AccessorDeclarationSyntax accessorDeclaration)
        {
            return Syntax.AccessibilityInfo.Create(accessorDeclaration);
        }

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

        public static GenericInfo GenericInfo(TypeParameterConstraintSyntax typeParameterConstraint)
        {
            return Syntax.GenericInfo.Create(typeParameterConstraint);
        }

        public static GenericInfo GenericInfo(TypeParameterConstraintClauseSyntax constraintClause)
        {
            return Syntax.GenericInfo.Create(constraintClause);
        }

        public static GenericInfo GenericInfo(SyntaxNode declaration)
        {
            return Syntax.GenericInfo.Create(declaration);
        }

        public static GenericInfo GenericInfo(TypeParameterListSyntax typeParameterList)
        {
            return Syntax.GenericInfo.Create(typeParameterList);
        }

        public static GenericInfo GenericInfo(ClassDeclarationSyntax classDeclaration)
        {
            return Syntax.GenericInfo.Create(classDeclaration);
        }

        public static GenericInfo GenericInfo(DelegateDeclarationSyntax delegateDeclaration)
        {
            return Syntax.GenericInfo.Create(delegateDeclaration);
        }

        public static GenericInfo GenericInfo(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            return Syntax.GenericInfo.Create(interfaceDeclaration);
        }

        public static GenericInfo GenericInfo(LocalFunctionStatementSyntax localFunctionStatement)
        {
            return Syntax.GenericInfo.Create(localFunctionStatement);
        }

        public static GenericInfo GenericInfo(MethodDeclarationSyntax methodDeclaration)
        {
            return Syntax.GenericInfo.Create(methodDeclaration);
        }

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

        public static IfStatementInfo IfStatementInfo(IfStatementSyntax ifStatement)
        {
            return Syntax.IfStatementInfo.Create(ifStatement);
        }

        public static LocalDeclarationStatementInfo LocalDeclarationStatementInfo(
            LocalDeclarationStatementSyntax localDeclarationStatement,
            bool allowMissing = false)
        {
            return Syntax.LocalDeclarationStatementInfo.Create(localDeclarationStatement, allowMissing);
        }

        public static LocalDeclarationStatementInfo LocalDeclarationStatementInfo(
            ExpressionSyntax expression,
            bool allowMissing = false)
        {
            return Syntax.LocalDeclarationStatementInfo.Create(expression, allowMissing);
        }

        public static MemberDeclarationsInfo MemberDeclarationsInfo(SyntaxNode declaration)
        {
            return Syntax.MemberDeclarationsInfo.Create(declaration);
        }

        public static MemberDeclarationsInfo MemberDeclarationsInfo(NamespaceDeclarationSyntax declaration)
        {
            return Syntax.MemberDeclarationsInfo.Create(declaration);
        }

        public static MemberDeclarationsInfo MemberDeclarationsInfo(ClassDeclarationSyntax declaration)
        {
            return Syntax.MemberDeclarationsInfo.Create(declaration);
        }

        public static MemberDeclarationsInfo MemberDeclarationsInfo(StructDeclarationSyntax declaration)
        {
            return Syntax.MemberDeclarationsInfo.Create(declaration);
        }

        public static MemberDeclarationsInfo MemberDeclarationsInfo(InterfaceDeclarationSyntax declaration)
        {
            return Syntax.MemberDeclarationsInfo.Create(declaration);
        }

        internal static MemberDeclarationsInfo MemberDeclarationsInfo(MemberDeclarationsSelection selectedMembers)
        {
            return Syntax.MemberDeclarationsInfo.Create(selectedMembers);
        }

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

        public static MemberInvocationExpressionInfo MemberInvocationExpressionInfo(
            InvocationExpressionSyntax invocationExpression,
            bool allowMissing = false)
        {
            return Syntax.MemberInvocationExpressionInfo.Create(
                invocationExpression,
                allowMissing);
        }

        public static MemberInvocationStatementInfo MemberInvocationStatementInfo(
            SyntaxNode node,
            bool allowMissing = false)
        {
            return Syntax.MemberInvocationStatementInfo.Create(
                node,
                allowMissing);
        }

        public static MemberInvocationStatementInfo MemberInvocationStatementInfo(
            ExpressionStatementSyntax expressionStatement,
            bool allowMissing = false)
        {
            return Syntax.MemberInvocationStatementInfo.Create(
                expressionStatement,
                allowMissing);
        }

        public static MemberInvocationStatementInfo MemberInvocationStatementInfo(
            InvocationExpressionSyntax invocationExpression,
            bool allowMissing = false)
        {
            return Syntax.MemberInvocationStatementInfo.Create(
                invocationExpression,
                allowMissing);
        }

        public static ModifiersInfo ModifiersInfo(SyntaxNode node)
        {
            return Syntax.ModifiersInfo.Create(node);
        }

        public static ModifiersInfo ModifiersInfo(ClassDeclarationSyntax classDeclaration)
        {
            return Syntax.ModifiersInfo.Create(classDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(ConstructorDeclarationSyntax constructorDeclaration)
        {
            return Syntax.ModifiersInfo.Create(constructorDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            return Syntax.ModifiersInfo.Create(conversionOperatorDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(DelegateDeclarationSyntax delegateDeclaration)
        {
            return Syntax.ModifiersInfo.Create(delegateDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(DestructorDeclarationSyntax destructorDeclaration)
        {
            return Syntax.ModifiersInfo.Create(destructorDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(EnumDeclarationSyntax enumDeclaration)
        {
            return Syntax.ModifiersInfo.Create(enumDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(EventDeclarationSyntax eventDeclaration)
        {
            return Syntax.ModifiersInfo.Create(eventDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            return Syntax.ModifiersInfo.Create(eventFieldDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(FieldDeclarationSyntax fieldDeclaration)
        {
            return Syntax.ModifiersInfo.Create(fieldDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(IndexerDeclarationSyntax indexerDeclaration)
        {
            return Syntax.ModifiersInfo.Create(indexerDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            return Syntax.ModifiersInfo.Create(interfaceDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(MethodDeclarationSyntax methodDeclaration)
        {
            return Syntax.ModifiersInfo.Create(methodDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(OperatorDeclarationSyntax operatorDeclaration)
        {
            return Syntax.ModifiersInfo.Create(operatorDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(PropertyDeclarationSyntax propertyDeclaration)
        {
            return Syntax.ModifiersInfo.Create(propertyDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(StructDeclarationSyntax structDeclaration)
        {
            return Syntax.ModifiersInfo.Create(structDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(IncompleteMemberSyntax incompleteMember)
        {
            return Syntax.ModifiersInfo.Create(incompleteMember);
        }

        public static ModifiersInfo ModifiersInfo(AccessorDeclarationSyntax accessorDeclaration)
        {
            return Syntax.ModifiersInfo.Create(accessorDeclaration);
        }

        public static ModifiersInfo ModifiersInfo(LocalDeclarationStatementSyntax localDeclarationStatement)
        {
            return Syntax.ModifiersInfo.Create(localDeclarationStatement);
        }

        public static ModifiersInfo ModifiersInfo(LocalFunctionStatementSyntax localFunctionStatement)
        {
            return Syntax.ModifiersInfo.Create(localFunctionStatement);
        }

        public static ModifiersInfo ModifiersInfo(ParameterSyntax parameter)
        {
            return Syntax.ModifiersInfo.Create(parameter);
        }

        public static NullCheckExpressionInfo NullCheckExpressionInfo(
            SyntaxNode node,
            NullCheckKind allowedKinds = NullCheckKind.All,
            bool walkDownParentheses = true,
            bool allowMissing = false,
            SemanticModel semanticModel = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Syntax.NullCheckExpressionInfo.Create(
                node,
                allowedKinds,
                walkDownParentheses,
                allowMissing,
                semanticModel,
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

        internal static RegionInfo RegionInfo(RegionDirectiveTriviaSyntax regionDirective)
        {
            return Syntax.RegionInfo.Create(regionDirective);
        }

        internal static RegionInfo RegionInfo(EndRegionDirectiveTriviaSyntax endRegionDirective)
        {
            return Syntax.RegionInfo.Create(endRegionDirective);
        }

        public static SimpleAssignmentExpressionInfo SimpleAssignmentExpressionInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleAssignmentExpressionInfo.Create(node, walkDownParentheses, allowMissing);
        }

        public static SimpleAssignmentExpressionInfo SimpleAssignmentExpressionInfo(
            AssignmentExpressionSyntax assignmentExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleAssignmentExpressionInfo.Create(assignmentExpression, walkDownParentheses, allowMissing);
        }

        public static SimpleAssignmentStatementInfo SimpleAssignmentStatementInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleAssignmentStatementInfo.Create(node, walkDownParentheses, allowMissing);
        }

        public static SimpleAssignmentStatementInfo SimpleAssignmentStatementInfo(
            AssignmentExpressionSyntax assignmentExpression,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleAssignmentStatementInfo.Create(assignmentExpression, walkDownParentheses, allowMissing);
        }

        public static SimpleAssignmentStatementInfo SimpleAssignmentStatementInfo(
            ExpressionStatementSyntax expressionStatement,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleAssignmentStatementInfo.Create(expressionStatement, walkDownParentheses, allowMissing);
        }

        public static SimpleIfElseInfo SimpleIfElseInfo(
            IfStatementSyntax ifStatement,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleIfElseInfo.Create(ifStatement, walkDownParentheses, allowMissing);
        }

        public static SimpleIfStatementInfo SimpleIfStatementInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleIfStatementInfo.Create(node, walkDownParentheses, allowMissing);
        }

        public static SimpleIfStatementInfo SimpleIfStatementInfo(
            IfStatementSyntax ifStatement,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SimpleIfStatementInfo.Create(ifStatement, walkDownParentheses, allowMissing);
        }

        public static SingleLocalDeclarationStatementInfo SingleLocalDeclarationStatementInfo(
            LocalDeclarationStatementSyntax localDeclarationStatement,
            bool allowMissing = false)
        {
            return Syntax.SingleLocalDeclarationStatementInfo.Create(localDeclarationStatement, allowMissing);
        }

        public static SingleLocalDeclarationStatementInfo SingleLocalDeclarationStatementInfo(
            VariableDeclarationSyntax variableDeclaration,
            bool allowMissing = false)
        {
            return Syntax.SingleLocalDeclarationStatementInfo.Create(variableDeclaration, allowMissing);
        }

        public static SingleLocalDeclarationStatementInfo SingleLocalDeclarationStatementInfo(ExpressionSyntax value)
        {
            return Syntax.SingleLocalDeclarationStatementInfo.Create(value);
        }

        public static SingleParameterLambdaExpressionInfo SingleParameterLambdaExpressionInfo(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Syntax.SingleParameterLambdaExpressionInfo.Create(node, walkDownParentheses, allowMissing);
        }

        public static SingleParameterLambdaExpressionInfo SingleParameterLambdaExpressionInfo(
            LambdaExpressionSyntax lambdaExpression,
            bool allowMissing = false)
        {
            return Syntax.SingleParameterLambdaExpressionInfo.Create(lambdaExpression, allowMissing);
        }

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

        public static TypeParameterConstraintInfo TypeParameterConstraintInfo(TypeParameterConstraintSyntax constraint, bool allowMissing = false)
        {
            return Syntax.TypeParameterConstraintInfo.Create(constraint, allowMissing);
        }

        public static TypeParameterInfo TypeParameterInfo(TypeParameterSyntax typeParameter)
        {
            return Syntax.TypeParameterInfo.Create(typeParameter);
        }

        public static XmlElementInfo XmlElementInfo(XmlNodeSyntax xmlNode)
        {
            return Syntax.XmlElementInfo.Create(xmlNode);
        }
    }
}