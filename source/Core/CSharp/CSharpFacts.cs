// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp
{
    public static class CSharpFacts
    {
        internal static string GetTitle(SyntaxNode node)
        {
            switch (node.Kind())
            {
                case SyntaxKind.IfStatement:
                    return "if statement";
                case SyntaxKind.ElseClause:
                    return "else clause";
                case SyntaxKind.DoStatement:
                    return "do statement";
                case SyntaxKind.ForEachStatement:
                case SyntaxKind.ForEachVariableStatement:
                    return "foreach statement";
                case SyntaxKind.ForStatement:
                    return "for statement";
                case SyntaxKind.UsingStatement:
                    return "using statement";
                case SyntaxKind.WhileStatement:
                    return "while statement";
                case SyntaxKind.LockStatement:
                    return "lock statement";
                case SyntaxKind.FixedStatement:
                    return "fixed statement";
                case SyntaxKind.SwitchStatement:
                    return "switch statement";
                case SyntaxKind.BreakStatement:
                    return "break statement";
                case SyntaxKind.ContinueStatement:
                    return "continue statement";
                case SyntaxKind.ReturnStatement:
                    return "return statement";
                case SyntaxKind.YieldReturnStatement:
                    return "yield return statement";
                case SyntaxKind.YieldBreakStatement:
                    return "yield break statement";
                case SyntaxKind.MethodDeclaration:
                    return "method";
                case SyntaxKind.OperatorDeclaration:
                    return "operator method";
                case SyntaxKind.ConversionOperatorDeclaration:
                    return "conversion method";
                case SyntaxKind.ConstructorDeclaration:
                    return "constructor";
                case SyntaxKind.DestructorDeclaration:
                    return "destructor";
                case SyntaxKind.PropertyDeclaration:
                    return "property";
                case SyntaxKind.IndexerDeclaration:
                    return "indexer";
                case SyntaxKind.EventDeclaration:
                case SyntaxKind.EventFieldDeclaration:
                    return "event";
                case SyntaxKind.FieldDeclaration:
                    return (((FieldDeclarationSyntax)node).Modifiers.Contains(SyntaxKind.ConstKeyword)) ? "const" : "field";
                case SyntaxKind.DelegateDeclaration:
                    return "delegate";
                case SyntaxKind.NamespaceDeclaration:
                    return "namespace";
                case SyntaxKind.ClassDeclaration:
                    return "class";
                case SyntaxKind.StructDeclaration:
                    return "struct";
                case SyntaxKind.InterfaceDeclaration:
                    return "interface";
                case SyntaxKind.EnumDeclaration:
                    return "enum";
                case SyntaxKind.IncompleteMember:
                    return "member";
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                case SyntaxKind.UnknownAccessorDeclaration:
                    return "accessor";
                case SyntaxKind.LocalDeclarationStatement:
                    return "local declaration";
                case SyntaxKind.LocalFunctionStatement:
                    return "local function";
                case SyntaxKind.Parameter:
                    return "parameter";
                default:
                    {
                        Debug.Fail(node.Kind().ToString());

                        if (node is StatementSyntax)
                            return "statement";

                        if (node is MemberDeclarationSyntax)
                            return "member";

                        throw new ArgumentException("", nameof(node));
                    }
            }
        }

        public static bool IsCommentTrivia(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.SingleLineCommentTrivia,
                SyntaxKind.MultiLineCommentTrivia,
                SyntaxKind.SingleLineDocumentationCommentTrivia,
                SyntaxKind.MultiLineDocumentationCommentTrivia);
        }

        public static bool CanHaveStatements(SyntaxKind kind)
        {
            return kind.Is(SyntaxKind.Block, SyntaxKind.SwitchSection);
        }

        public static bool CanHaveMembers(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.CompilationUnit,
                SyntaxKind.NamespaceDeclaration,
                SyntaxKind.ClassDeclaration,
                SyntaxKind.StructDeclaration,
                SyntaxKind.InterfaceDeclaration);
        }

        public static bool IsNestedMethod(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.SimpleLambdaExpression,
                SyntaxKind.ParenthesizedLambdaExpression,
                SyntaxKind.AnonymousMethodExpression,
                SyntaxKind.LocalFunctionStatement);
        }

        public static bool IsLoopStatement(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.ForStatement,
                SyntaxKind.ForEachStatement,
                SyntaxKind.WhileStatement,
                SyntaxKind.DoStatement);
        }

        public static bool IsBooleanLiteralExpression(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.TrueLiteralExpression,
                SyntaxKind.FalseLiteralExpression);
        }

        public static bool IsLambdaExpression(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.SimpleLambdaExpression,
                SyntaxKind.ParenthesizedLambdaExpression);
        }

        public static bool IsAnonymousFunctionExpression(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.AnonymousMethodExpression,
                SyntaxKind.SimpleLambdaExpression,
                SyntaxKind.ParenthesizedLambdaExpression);
        }

        public static bool IsJumpStatement(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.BreakStatement,
                SyntaxKind.ContinueStatement,
                SyntaxKind.GotoCaseStatement,
                SyntaxKind.GotoDefaultStatement,
                SyntaxKind.ReturnStatement,
                SyntaxKind.ThrowStatement);
        }

        internal static bool IsJumpStatementOrYieldBreakStatement(SyntaxKind kind)
        {
            return IsJumpStatement(kind)
                || kind == SyntaxKind.YieldBreakStatement;
        }

        public static bool IsIncrementOrDecrementExpression(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.PreIncrementExpression,
                SyntaxKind.PreDecrementExpression,
                SyntaxKind.PostIncrementExpression,
                SyntaxKind.PostDecrementExpression);
        }

        public static bool SupportsCompoundAssignment(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.AddExpression:
                case SyntaxKind.SubtractExpression:
                case SyntaxKind.MultiplyExpression:
                case SyntaxKind.DivideExpression:
                case SyntaxKind.ModuloExpression:
                case SyntaxKind.BitwiseAndExpression:
                case SyntaxKind.ExclusiveOrExpression:
                case SyntaxKind.BitwiseOrExpression:
                case SyntaxKind.LeftShiftExpression:
                case SyntaxKind.RightShiftExpression:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CanHaveModifiers(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.ConstructorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                case SyntaxKind.DelegateDeclaration:
                case SyntaxKind.DestructorDeclaration:
                case SyntaxKind.EnumDeclaration:
                case SyntaxKind.EventDeclaration:
                case SyntaxKind.EventFieldDeclaration:
                case SyntaxKind.FieldDeclaration:
                case SyntaxKind.IndexerDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.MethodDeclaration:
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.PropertyDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.IncompleteMember:
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                case SyntaxKind.UnknownAccessorDeclaration:
                case SyntaxKind.LocalDeclarationStatement:
                case SyntaxKind.LocalFunctionStatement:
                case SyntaxKind.Parameter:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CanHaveExpressionBody(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.MethodDeclaration:
                case SyntaxKind.PropertyDeclaration:
                case SyntaxKind.IndexerDeclaration:
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                case SyntaxKind.ConstructorDeclaration:
                case SyntaxKind.DestructorDeclaration:
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CanHaveEmbeddedStatement(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.IfStatement:
                case SyntaxKind.ElseClause:
                case SyntaxKind.ForEachStatement:
                case SyntaxKind.ForEachVariableStatement:
                case SyntaxKind.ForStatement:
                case SyntaxKind.UsingStatement:
                case SyntaxKind.WhileStatement:
                case SyntaxKind.DoStatement:
                case SyntaxKind.LockStatement:
                case SyntaxKind.FixedStatement:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CanHaveAccessibility(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.ConstructorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                case SyntaxKind.DelegateDeclaration:
                case SyntaxKind.DestructorDeclaration:
                case SyntaxKind.EnumDeclaration:
                case SyntaxKind.EventDeclaration:
                case SyntaxKind.EventFieldDeclaration:
                case SyntaxKind.FieldDeclaration:
                case SyntaxKind.IndexerDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.MethodDeclaration:
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.PropertyDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.IncompleteMember:
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.UnknownAccessorDeclaration:
                    return true;
                default:
                    return false;
            }
        }

        internal static bool CanContainContinueStatement(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.WhileStatement,
                SyntaxKind.DoStatement,
                SyntaxKind.ForStatement,
                SyntaxKind.ForEachStatement);
        }

        internal static bool IsSingleTokenExpression(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.IdentifierName:
                case SyntaxKind.PredefinedType:
                case SyntaxKind.ThisExpression:
                case SyntaxKind.BaseExpression:
                case SyntaxKind.NumericLiteralExpression:
                case SyntaxKind.StringLiteralExpression:
                case SyntaxKind.CharacterLiteralExpression:
                case SyntaxKind.TrueLiteralExpression:
                case SyntaxKind.FalseLiteralExpression:
                case SyntaxKind.NullLiteralExpression:
                    return true;
                default:
                    return false;
            }
        }

        // http://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/built-in-types-table
        public static bool IsPredefinedType(SpecialType specialType)
        {
            switch (specialType)
            {
                case SpecialType.System_Object:
                case SpecialType.System_Boolean:
                case SpecialType.System_Char:
                case SpecialType.System_SByte:
                case SpecialType.System_Byte:
                case SpecialType.System_Int16:
                case SpecialType.System_UInt16:
                case SpecialType.System_Int32:
                case SpecialType.System_UInt32:
                case SpecialType.System_Int64:
                case SpecialType.System_UInt64:
                case SpecialType.System_Decimal:
                case SpecialType.System_Single:
                case SpecialType.System_Double:
                case SpecialType.System_String:
                case SpecialType.System_Void:
                    return true;
            }

            return false;
        }

        // https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/types-and-variables
        public static bool IsSimpleType(SpecialType specialType)
        {
            switch (specialType)
            {
                case SpecialType.System_Boolean:
                case SpecialType.System_Char:
                case SpecialType.System_SByte:
                case SpecialType.System_Byte:
                case SpecialType.System_Int16:
                case SpecialType.System_UInt16:
                case SpecialType.System_Int32:
                case SpecialType.System_UInt32:
                case SpecialType.System_Int64:
                case SpecialType.System_UInt64:
                case SpecialType.System_Decimal:
                case SpecialType.System_Single:
                case SpecialType.System_Double:
                    return true;
            }

            return false;
        }

        public static bool SupportsPrefixOrPostfixUnaryOperator(SpecialType specialType)
        {
            switch (specialType)
            {
                case SpecialType.System_SByte:
                case SpecialType.System_Byte:
                case SpecialType.System_Int16:
                case SpecialType.System_UInt16:
                case SpecialType.System_Int32:
                case SpecialType.System_UInt32:
                case SpecialType.System_Int64:
                case SpecialType.System_UInt64:
                case SpecialType.System_Char:
                case SpecialType.System_Single:
                case SpecialType.System_Double:
                case SpecialType.System_Decimal:
                    return true;
            }

            return false;
        }

        internal static string GetTitle(SyntaxKind kind)
        {
            if (kind == SyntaxKind.ReadOnlyKeyword)
                return "read-only";

            return SyntaxFacts.GetText(kind);
        }
    }
}
