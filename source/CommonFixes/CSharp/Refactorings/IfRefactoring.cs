// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Refactorings.If;
using Roslynator.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal abstract class IfRefactoring
    {
        public abstract Task<Document> RefactorAsync(Document document, CancellationToken cancellationToken = default(CancellationToken));

        public static Task<Document> RefactorAsync(Document document, IfAnalysis ifAnalysis, CancellationToken cancellationToken = default(CancellationToken))
        {
            switch (ifAnalysis.Kind)
            {
                case IfRefactoringKind.IfElseToAssignmentWithCoalesceExpression:
                    {
                        return IfElseToAssignmentWithCoalesceExpressionAsync(document, (IfElseToAssignmentWithCoalesceExpressionAnalysis)ifAnalysis, cancellationToken);
                    }
                case IfRefactoringKind.IfElseToAssignmentWithConditionalExpression:
                    {
                        return IfElseToAssignmentWithConditionalExpressionAsync(document, (IfElseToAssignmentWithConditionalExpressionAnalysis)ifAnalysis, cancellationToken);
                    }
                case IfRefactoringKind.AssignmentAndIfElseToAssignmentWithConditionalExpression:
                    {
                        var analysis = (AssignmentAndIfElseToAssignmentWithConditionalExpressionAnalysis)ifAnalysis;

                        ConditionalExpressionSyntax conditionalExpression = CreateConditionalExpression(analysis.IfStatement.Condition, analysis.WhenTrue, analysis.WhenFalse);

                        ExpressionStatementSyntax newStatement = analysis.Statement.ReplaceNode(analysis.Left, conditionalExpression);

                        return ToAssignmentWithConditionalExpressionAsync(document, analysis, newStatement, cancellationToken);
                    }
                case IfRefactoringKind.LocalDeclarationAndIfElseAssignmentWithConditionalExpression:
                    {
                        var analysis = (LocalDeclarationAndIfElseToAssignmentWithConditionalExpressionAnalysis)ifAnalysis;

                        ConditionalExpressionSyntax conditionalExpression = CreateConditionalExpression(analysis.IfStatement.Condition, analysis.WhenTrue, analysis.WhenFalse);

                        VariableDeclaratorSyntax declarator = analysis.Statement.Declaration.Variables[0];

                        EqualsValueClauseSyntax initializer = declarator.Initializer;

                        EqualsValueClauseSyntax newInitializer = (initializer != null)
                            ? initializer.WithValue(conditionalExpression)
                            : EqualsValueClause(conditionalExpression);

                        LocalDeclarationStatementSyntax newStatement = analysis.Statement.ReplaceNode(declarator, declarator.WithInitializer(newInitializer));

                        return ToAssignmentWithConditionalExpressionAsync(document, analysis, newStatement, cancellationToken);
                    }
                case IfRefactoringKind.IfElseToAssignmentWithExpression:
                    {
                        return IfElseToAssignmentWithExpressionAsync(document, (IfElseToAssignmentWithExpressionAnalysis)ifAnalysis, cancellationToken);
                    }
                case IfRefactoringKind.IfElseToAssignmentWithCondition:
                    {
                        return IfElseToAssignmentWithConditionAsync(document, (IfElseToAssignmentWithConditionAnalysis)ifAnalysis, cancellationToken);
                    }
                case IfRefactoringKind.IfElseToReturnWithCoalesceExpression:
                case IfRefactoringKind.IfElseToYieldReturnWithCoalesceExpression:
                case IfRefactoringKind.IfReturnToReturnWithCoalesceExpression:
                    {
                        return IfToReturnWithCoalesceExpressionAsync(document, (IfToReturnWithCoalesceExpressionAnalysis)ifAnalysis, cancellationToken);
                    }
                case IfRefactoringKind.IfElseToReturnWithConditionalExpression:
                    {
                        return IfElseToReturnWithConditionalExpressionAsync(document, (IfElseToReturnWithConditionalExpressionAnalysis)ifAnalysis, cancellationToken);
                    }
                case IfRefactoringKind.IfElseToReturnWithBooleanExpression:
                    {
                        return IfElseToReturnWithBooleanExpressionAsync(document, (IfElseToReturnWithBooleanExpressionAnalysis)ifAnalysis, cancellationToken);
                    }
                case IfRefactoringKind.IfElseToReturnWithExpression:
                case IfRefactoringKind.IfElseToYieldReturnWithExpression:
                case IfRefactoringKind.IfReturnToReturnWithExpression:
                    {
                        return IfToReturnWithExpressionAsync(document, (IfToReturnWithExpressionAnalysis)ifAnalysis, cancellationToken);
                    }
                case IfRefactoringKind.IfElseToYieldReturnWithConditionalExpression:
                    {
                        return IfElseToYieldReturnWithConditionalExpressionAsync(document, (IfElseToYieldReturnWithConditionalExpressionAnalysis)ifAnalysis, cancellationToken);
                    }
                case IfRefactoringKind.IfElseToYieldReturnWithBooleanExpression:
                    {
                        return IfElseToYieldReturnWithBooleanExpressionAsync(document, (IfElseToYieldReturnWithBooleanExpressionAnalysis)ifAnalysis, cancellationToken);
                    }
                case IfRefactoringKind.IfReturnToReturnWithConditionalExpression:
                    {
                        return IfReturnToReturnWithConditionalExpressionAsync(document, (IfReturnToReturnWithConditionalExpressionAnalysis)ifAnalysis, cancellationToken);
                    }
                case IfRefactoringKind.IfReturnToReturnWithBooleanExpression:
                    {
                        return IfReturnToReturnWithBooleanExpressionAsync(document, (IfReturnToReturnWithBooleanExpressionAnalysis)ifAnalysis, cancellationToken);
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
        }

        public static async Task<Document> IfElseToAssignmentWithCoalesceExpressionAsync(
            Document document,
            IfElseToAssignmentWithCoalesceExpressionAnalysis analysis,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            BinaryExpressionSyntax coalesceExpression = CodeFixesUtility.CreateCoalesceExpression(
                semanticModel.GetTypeSymbol(analysis.Left, cancellationToken),
                analysis.Right1.WithoutTrivia(),
                analysis.Right2.WithoutTrivia(),
                analysis.IfStatement.SpanStart,
                semanticModel);

            ExpressionStatementSyntax newNode = SimpleAssignmentStatement(analysis.Left.WithoutTrivia(), coalesceExpression)
                .WithTriviaFrom(analysis.IfStatement)
                .WithFormatterAnnotation();

            return await document.ReplaceNodeAsync(analysis.IfStatement, newNode, cancellationToken).ConfigureAwait(false);
        }

        public static Task<Document> IfElseToAssignmentWithConditionalExpressionAsync(
            Document document,
            IfElseToAssignmentWithConditionalExpressionAnalysis analysis,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ConditionalExpressionSyntax conditionalExpression = CreateConditionalExpression(analysis.IfStatement.Condition, analysis.WhenTrue, analysis.WhenFalse);

            ExpressionStatementSyntax newNode = SimpleAssignmentStatement(analysis.Left, conditionalExpression)
                .WithTriviaFrom(analysis.IfStatement)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(analysis.IfStatement, newNode, cancellationToken);
        }

        public static Task<Document> ToAssignmentWithConditionalExpressionAsync(
            Document document,
            ToAssignmentWithConditionalExpressionAnalysis analysis,
            StatementSyntax newStatement,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            StatementListInfo statementsInfo = SyntaxInfo.StatementListInfo(analysis.IfStatement);

            SyntaxList<StatementSyntax> statements = statementsInfo.Statements;

            int index = statements.IndexOf(analysis.IfStatement);

            SyntaxList<StatementSyntax> newStatements = statements
            .RemoveAt(index)
            .ReplaceAt(index - 1, newStatement);

            return document.ReplaceStatementsAsync(statementsInfo, newStatements, cancellationToken);
        }

        public static Task<Document> IfElseToAssignmentWithExpressionAsync(
            Document document,
            IfElseToAssignmentWithExpressionAnalysis analysis,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ExpressionStatementSyntax newNode = analysis.ExpressionStatement
                .WithTriviaFrom(analysis.IfStatement)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(analysis.IfStatement, newNode, cancellationToken);
        }

        public static async Task<Document> IfElseToAssignmentWithConditionAsync(
            Document document,
            IfElseToAssignmentWithConditionAnalysis analysis,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ExpressionSyntax right = analysis.Right.WithoutTrivia();

            if (analysis.Negate)
            {
                SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

                right = Negation.LogicallyNegate(right, semanticModel, cancellationToken);
            }

            ExpressionStatementSyntax newNode = SimpleAssignmentStatement(analysis.Left.WithoutTrivia(), right)
                .WithTriviaFrom(analysis.IfStatement)
                .WithFormatterAnnotation();

            return await document.ReplaceNodeAsync(analysis.IfStatement, newNode, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<Document> IfToReturnWithCoalesceExpressionAsync(Document document, IfToReturnWithCoalesceExpressionAnalysis analysis, CancellationToken cancellationToken = default(CancellationToken))
        {
            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            IfStatementSyntax ifStatement = analysis.IfStatement;
            int position = ifStatement.SpanStart;

            ITypeSymbol targetType = GetTargetType();

            BinaryExpressionSyntax coalesceExpression = CodeFixesUtility.CreateCoalesceExpression(
                targetType,
                analysis.Left.WithoutTrivia(),
                analysis.Right.WithoutTrivia(),
                position,
                semanticModel);

            StatementSyntax statement;
            if (analysis.IsYield)
            {
                statement = YieldReturnStatement(coalesceExpression);
            }
            else
            {
                statement = ReturnStatement(coalesceExpression);
            }

            if (ifStatement.IsSimpleIf())
            {
                StatementListInfo statementsInfo = SyntaxInfo.StatementListInfo(ifStatement);

                SyntaxList<StatementSyntax> statements = statementsInfo.Statements;

                int index = statements.IndexOf(ifStatement);

                StatementSyntax newNode = statement
                    .WithLeadingTrivia(ifStatement.GetLeadingTrivia())
                    .WithTrailingTrivia(statements[index + 1].GetTrailingTrivia())
                    .WithFormatterAnnotation();

                SyntaxList<StatementSyntax> newStatements = statements
                    .RemoveAt(index)
                    .ReplaceAt(index, newNode);

                return await document.ReplaceStatementsAsync(statementsInfo, newStatements, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                StatementSyntax newNode = statement
                    .WithTriviaFrom(ifStatement)
                    .WithFormatterAnnotation();

                return await document.ReplaceNodeAsync(ifStatement, newNode, cancellationToken).ConfigureAwait(false);
            }

            ITypeSymbol GetTargetType()
            {
                IMethodSymbol methodSymbol = semanticModel.GetEnclosingSymbol<IMethodSymbol>(position, cancellationToken);

                Debug.Assert(methodSymbol != null, "");

                if (methodSymbol?.IsErrorType() == false)
                {
                    ITypeSymbol returnType = methodSymbol.ReturnType;

                    if (!returnType.IsErrorType())
                    {
                        if (methodSymbol.IsAsync)
                        {
                            if (returnType is INamedTypeSymbol namedTypeSymbol
                                && namedTypeSymbol.ConstructedFrom.EqualsOrInheritsFrom(semanticModel.GetTypeByMetadataName(MetadataNames.System_Threading_Tasks_Task_T)))
                            {
                                return namedTypeSymbol.TypeArguments[0];
                            }
                        }
                        else if (!analysis.IsYield)
                        {
                            return returnType;
                        }
                        else if (returnType.IsIEnumerable())
                        {
                            return semanticModel.Compilation.ObjectType;
                        }
                        else if (returnType.IsConstructedFromIEnumerableOfT())
                        {
                            return ((INamedTypeSymbol)returnType).TypeArguments[0];
                        }
                    }
                }

                return null;
            }
        }

        public static Task<Document> IfElseToReturnWithConditionalExpressionAsync(
            Document document,
            IfElseToReturnWithConditionalExpressionAnalysis analysis,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ConditionalExpressionSyntax conditionalExpression = CreateConditionalExpression(analysis.IfStatement.Condition, analysis.Expression1, analysis.Expression2);

            StatementSyntax newNode = ReturnStatement(conditionalExpression)
                .WithTriviaFrom(analysis.IfStatement)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(analysis.IfStatement, newNode, cancellationToken);
        }

        public static async Task<Document> IfElseToReturnWithBooleanExpressionAsync(
            Document document,
            IfToReturnWithBooleanExpressionAnalysis analysis,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IfStatementSyntax ifStatement = analysis.IfStatement;

            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            ExpressionSyntax expression = GetBooleanExpression(ifStatement.Condition, analysis.Expression1, analysis.Expression2, semanticModel, cancellationToken);

            StatementSyntax newNode = ReturnStatement(expression)
                .WithTriviaFrom(ifStatement)
                .WithFormatterAnnotation();

            return await document.ReplaceNodeAsync(ifStatement, newNode, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<Document> IfToReturnWithExpressionAsync(
            Document document,
            IfToReturnWithExpressionAnalysis analysis,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ExpressionSyntax expression = analysis.Expression;

            if (analysis.Negate)
            {
                SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

                expression = Negation.LogicallyNegate(expression, semanticModel, cancellationToken);
            }

            StatementSyntax statement;
            if (analysis.IsYield)
            {
                statement = YieldReturnStatement(expression);
            }
            else
            {
                statement = ReturnStatement(expression);
            }

            IfStatementSyntax ifStatement = analysis.IfStatement;

            if (ifStatement.IsSimpleIf())
            {
                StatementListInfo statementsInfo = SyntaxInfo.StatementListInfo(ifStatement);

                SyntaxList<StatementSyntax> statements = statementsInfo.Statements;

                int index = statements.IndexOf(ifStatement);

                StatementSyntax newNode = statement
                    .WithLeadingTrivia(ifStatement.GetLeadingTrivia())
                    .WithTrailingTrivia(statements[index + 1].GetTrailingTrivia())
                    .WithFormatterAnnotation();

                SyntaxList<StatementSyntax> newStatements = statements
                    .RemoveAt(index)
                    .ReplaceAt(index, newNode);

                return await document.ReplaceStatementsAsync(statementsInfo, newStatements, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                StatementSyntax newNode = statement
                    .WithTriviaFrom(ifStatement)
                    .WithFormatterAnnotation();

                return await document.ReplaceNodeAsync(ifStatement, newNode, cancellationToken).ConfigureAwait(false);
            }
        }

        public static Task<Document> IfElseToYieldReturnWithConditionalExpressionAsync(
            Document document,
            IfElseToYieldReturnWithConditionalExpressionAnalysis analysis,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IfStatementSyntax ifStatement = analysis.IfStatement;
            ConditionalExpressionSyntax conditionalExpression = CreateConditionalExpression(ifStatement.Condition, analysis.Expression1, analysis.Expression2);

            StatementSyntax newNode = YieldReturnStatement(conditionalExpression)
                .WithTriviaFrom(ifStatement)
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(ifStatement, newNode, cancellationToken);
        }

        public static async Task<Document> IfElseToYieldReturnWithBooleanExpressionAsync(
            Document document,
            IfToReturnWithBooleanExpressionAnalysis analysis,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IfStatementSyntax ifStatement = analysis.IfStatement;

            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            ExpressionSyntax expression = GetBooleanExpression(ifStatement.Condition, analysis.Expression1, analysis.Expression2, semanticModel, cancellationToken);

            StatementSyntax newNode = YieldReturnStatement(expression)
                .WithTriviaFrom(ifStatement)
                .WithFormatterAnnotation();

            return await document.ReplaceNodeAsync(ifStatement, newNode, cancellationToken).ConfigureAwait(false);
        }

        public static Task<Document> IfReturnToReturnWithConditionalExpressionAsync(
            Document document,
            IfReturnToReturnWithConditionalExpressionAnalysis analysis,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IfStatementSyntax ifStatement = analysis.IfStatement;

            StatementListInfo statementsInfo = SyntaxInfo.StatementListInfo(ifStatement);

            SyntaxList<StatementSyntax> statements = statementsInfo.Statements;

            int index = statements.IndexOf(ifStatement);

            ConditionalExpressionSyntax conditionalExpression = CreateConditionalExpression(ifStatement.Condition, analysis.Expression1, analysis.Expression2);

            StatementSyntax newStatement = ReturnStatement(conditionalExpression)
                .WithLeadingTrivia(ifStatement.GetLeadingTrivia())
                .WithTrailingTrivia(statements[index + 1].GetTrailingTrivia())
                .WithFormatterAnnotation();

            SyntaxList<StatementSyntax> newStatements = statements
                .RemoveAt(index)
                .ReplaceAt(index, newStatement);

            return document.ReplaceStatementsAsync(statementsInfo, newStatements, cancellationToken);
        }

        public static async Task<Document> IfReturnToReturnWithBooleanExpressionAsync(
            Document document,
            IfReturnToReturnWithBooleanExpressionAnalysis analysis,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            StatementListInfo statementsInfo = SyntaxInfo.StatementListInfo(analysis.IfStatement);

            SyntaxList<StatementSyntax> statements = statementsInfo.Statements;

            int index = statements.IndexOf(analysis.IfStatement);

            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            ExpressionSyntax expression = GetBooleanExpression(
                analysis.IfStatement.Condition,
                analysis.Expression1,
                analysis.Expression2,
                semanticModel,
                cancellationToken);

            StatementSyntax newStatement = ReturnStatement(expression)
                .WithLeadingTrivia(analysis.IfStatement.GetLeadingTrivia())
                .WithTrailingTrivia(statements[index + 1].GetTrailingTrivia())
                .WithFormatterAnnotation();

            SyntaxList<StatementSyntax> newStatements = statements
                .RemoveAt(index)
                .ReplaceAt(index, newStatement);

            return await document.ReplaceStatementsAsync(statementsInfo, newStatements, cancellationToken).ConfigureAwait(false);
        }

        private static ConditionalExpressionSyntax CreateConditionalExpression(ExpressionSyntax condition, ExpressionSyntax whenTrue, ExpressionSyntax whenFalse)
        {
            if (condition.Kind() != SyntaxKind.ParenthesizedExpression)
            {
                condition = ParenthesizedExpression(condition.WithoutTrivia())
                    .WithTriviaFrom(condition);
            }

            return ConditionalExpression(condition, whenTrue, whenFalse);
        }

        private static ExpressionSyntax GetBooleanExpression(
            ExpressionSyntax condition,
            ExpressionSyntax expression1,
            ExpressionSyntax expression2,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            switch (expression1.Kind())
            {
                case SyntaxKind.TrueLiteralExpression:
                    {
                        switch (expression2.Kind())
                        {
                            case SyntaxKind.TrueLiteralExpression:
                                return expression2;
                            case SyntaxKind.FalseLiteralExpression:
                                return condition;
                            default:
                                return LogicalOrExpression(condition, expression2);
                        }
                    }
                case SyntaxKind.FalseLiteralExpression:
                    {
                        switch (expression2.Kind())
                        {
                            case SyntaxKind.TrueLiteralExpression:
                                return Negation.LogicallyNegate(condition, semanticModel, cancellationToken);
                            case SyntaxKind.FalseLiteralExpression:
                                return expression2;
                            default:
                                return LogicalAndExpression(Negation.LogicallyNegate(condition, semanticModel, cancellationToken), expression2);
                        }
                    }
                default:
                    {
                        switch (expression2.Kind())
                        {
                            case SyntaxKind.TrueLiteralExpression:
                                return LogicalOrExpression(Negation.LogicallyNegate(condition, semanticModel, cancellationToken), expression1);
                            case SyntaxKind.FalseLiteralExpression:
                                return LogicalAndExpression(condition, expression1);
                            default:
                                throw new InvalidOperationException();
                        }
                    }
            }
        }

        private static BinaryExpressionSyntax LogicalAndExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return CSharpFactory.LogicalAndExpression(
                left.Parenthesize(),
                right.Parenthesize());
        }

        private static BinaryExpressionSyntax LogicalOrExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return CSharpFactory.LogicalOrExpression(
                left.Parenthesize(),
                right.Parenthesize());
        }
    }
}
