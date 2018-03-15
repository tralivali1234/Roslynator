// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp
{
    public static class CSharpFacts
    {
        internal static ImmutableArray<SyntaxKind> AssignmentExpressionKinds { get; } = ImmutableArray.CreateRange(new SyntaxKind[]
        {
            SyntaxKind.SimpleAssignmentExpression,
            SyntaxKind.AddAssignmentExpression,
            SyntaxKind.SubtractAssignmentExpression,
            SyntaxKind.MultiplyAssignmentExpression,
            SyntaxKind.DivideAssignmentExpression,
            SyntaxKind.ModuloAssignmentExpression,
            SyntaxKind.AndAssignmentExpression,
            SyntaxKind.ExclusiveOrAssignmentExpression,
            SyntaxKind.OrAssignmentExpression,
            SyntaxKind.LeftShiftAssignmentExpression,
            SyntaxKind.RightShiftAssignmentExpression
        });

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

        /// <summary>
        /// Returns true if a syntax of the specified kind is comment trivia.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsCommentTrivia(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.SingleLineCommentTrivia,
                SyntaxKind.MultiLineCommentTrivia,
                SyntaxKind.SingleLineDocumentationCommentTrivia,
                SyntaxKind.MultiLineDocumentationCommentTrivia);
        }

        /// <summary>
        /// Returns true if a syntax of the specified kind can have statements. It can be either <see cref="BlockSyntax"/> or <see cref="SwitchSectionSyntax"/>.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool CanHaveStatements(SyntaxKind kind)
        {
            return kind.Is(SyntaxKind.Block, SyntaxKind.SwitchSection);
        }

        /// <summary>
        /// Returns true if a syntax of the specified kind can have members.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool CanHaveMembers(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.CompilationUnit,
                SyntaxKind.NamespaceDeclaration,
                SyntaxKind.ClassDeclaration,
                SyntaxKind.StructDeclaration,
                SyntaxKind.InterfaceDeclaration);
        }

        /// <summary>
        /// Returns true if a syntax of the specified kind if local function or anonymous function.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsFunction(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.SimpleLambdaExpression,
                SyntaxKind.ParenthesizedLambdaExpression,
                SyntaxKind.AnonymousMethodExpression,
                SyntaxKind.LocalFunctionStatement);
        }

        /// <summary>
        /// Returns true if a syntax of the specified kind is a for, foreach, while or do statement.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsLoopStatement(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.ForStatement,
                SyntaxKind.ForEachStatement,
                SyntaxKind.WhileStatement,
                SyntaxKind.DoStatement);
        }

        /// <summary>
        /// Returns true if a syntax of the specified kind is true or false literal expression.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsBooleanLiteralExpression(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.TrueLiteralExpression,
                SyntaxKind.FalseLiteralExpression);
        }

        /// <summary>
        /// Returns true if a syntax of the specified kind is a lambda expression.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsLambdaExpression(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.SimpleLambdaExpression,
                SyntaxKind.ParenthesizedLambdaExpression);
        }

        /// <summary>
        /// Returns true if a syntax of the specified kind is an anonymous method or lambda expression.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsAnonymousFunctionExpression(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.AnonymousMethodExpression,
                SyntaxKind.SimpleLambdaExpression,
                SyntaxKind.ParenthesizedLambdaExpression);
        }

        /// <summary>
        /// Returns true if a syntax of the specified kind is a jump statement.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if a syntax of the specified kind is pre/post increment/decrement expression.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsIncrementOrDecrementExpression(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.PreIncrementExpression,
                SyntaxKind.PreDecrementExpression,
                SyntaxKind.PostIncrementExpression,
                SyntaxKind.PostDecrementExpression);
        }

        public static bool IsCompoundAssignment(SyntaxKind assignmentExpressionKind)
        {
            switch (assignmentExpressionKind)
            {
                case SyntaxKind.AddAssignmentExpression:
                case SyntaxKind.SubtractAssignmentExpression:
                case SyntaxKind.MultiplyAssignmentExpression:
                case SyntaxKind.DivideAssignmentExpression:
                case SyntaxKind.ModuloAssignmentExpression:
                case SyntaxKind.AndAssignmentExpression:
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                case SyntaxKind.OrAssignmentExpression:
                case SyntaxKind.LeftShiftAssignmentExpression:
                case SyntaxKind.RightShiftAssignmentExpression:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns true if a syntax of the specified kind can be simplified to a compound assignment.
        /// </summary>
        /// <param name="binaryExpressionKind"></param>
        /// <returns></returns>
        public static bool SupportsCompoundAssignment(SyntaxKind binaryExpressionKind)
        {
            switch (binaryExpressionKind)
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

        /// <summary>
        /// Returns true if a syntax of the specified kind can have modifiers.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if a syntax of the specified kind can have expression body.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
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
                case SyntaxKind.LocalFunctionStatement:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns true if a syntax of the specified kind can have an embedded statement.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if a syntax of the specified kind can have accessibility modifiers.
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns true if a syntax of the specified kind is a predefined type.
        /// </summary>
        /// <param name="specialType"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns true if a syntax of the specified kind is a simple type.
        /// </summary>
        /// <param name="specialType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if an expression of the specified type can be used in a prefix or postfix unary operator.
        /// </summary>
        /// <param name="specialType"></param>
        /// <returns></returns>
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

        internal static bool ExistsImplicitNumericConversion(ITypeSymbol from, ITypeSymbol to)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));

            if (to == null)
                throw new ArgumentNullException(nameof(to));

            return ExistsImplicitNumericConversion(from.SpecialType, to.SpecialType);
        }

        // http://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/implicit-numeric-conversions-table
        internal static bool ExistsImplicitNumericConversion(SpecialType from, SpecialType to)
        {
            switch (from)
            {
                case SpecialType.System_Char:
                    {
                        switch (to)
                        {
                            case SpecialType.System_UInt16:
                            case SpecialType.System_Int32:
                            case SpecialType.System_UInt32:
                            case SpecialType.System_Int64:
                            case SpecialType.System_UInt64:
                            case SpecialType.System_Single:
                            case SpecialType.System_Double:
                            case SpecialType.System_Decimal:
                                return true;
                        }

                        break;
                    }
                case SpecialType.System_SByte:
                    {
                        switch (to)
                        {
                            case SpecialType.System_Int16:
                            case SpecialType.System_Int32:
                            case SpecialType.System_Int64:
                            case SpecialType.System_Single:
                            case SpecialType.System_Double:
                            case SpecialType.System_Decimal:
                                return true;
                        }

                        break;
                    }
                case SpecialType.System_Byte:
                    {
                        switch (to)
                        {
                            case SpecialType.System_Int16:
                            case SpecialType.System_UInt16:
                            case SpecialType.System_Int32:
                            case SpecialType.System_UInt32:
                            case SpecialType.System_Int64:
                            case SpecialType.System_UInt64:
                            case SpecialType.System_Single:
                            case SpecialType.System_Double:
                            case SpecialType.System_Decimal:
                                return true;
                        }

                        break;
                    }
                case SpecialType.System_Int16:
                    {
                        switch (to)
                        {
                            case SpecialType.System_Int32:
                            case SpecialType.System_Int64:
                            case SpecialType.System_Single:
                            case SpecialType.System_Double:
                            case SpecialType.System_Decimal:
                                return true;
                        }

                        break;
                    }
                case SpecialType.System_UInt16:
                    {
                        switch (to)
                        {
                            case SpecialType.System_Int32:
                            case SpecialType.System_UInt32:
                            case SpecialType.System_Int64:
                            case SpecialType.System_UInt64:
                            case SpecialType.System_Single:
                            case SpecialType.System_Double:
                            case SpecialType.System_Decimal:
                                return true;
                        }

                        break;
                    }
                case SpecialType.System_Int32:
                    {
                        switch (to)
                        {
                            case SpecialType.System_Int64:
                            case SpecialType.System_Single:
                            case SpecialType.System_Double:
                            case SpecialType.System_Decimal:
                                return true;
                        }

                        break;
                    }
                case SpecialType.System_UInt32:
                    {
                        switch (to)
                        {
                            case SpecialType.System_Int64:
                            case SpecialType.System_UInt64:
                            case SpecialType.System_Single:
                            case SpecialType.System_Double:
                            case SpecialType.System_Decimal:
                                return true;
                        }

                        break;
                    }
                case SpecialType.System_Int64:
                case SpecialType.System_UInt64:
                    {
                        switch (to)
                        {
                            case SpecialType.System_Single:
                            case SpecialType.System_Double:
                            case SpecialType.System_Decimal:
                                return true;
                        }

                        break;
                    }
                case SpecialType.System_Single:
                    {
                        switch (to)
                        {
                            case SpecialType.System_Double:
                                return true;
                        }

                        break;
                    }
            }

            return false;
        }

        public static bool IsSwitchLabel(SyntaxKind kind)
        {
            return kind.Is(
                SyntaxKind.CasePatternSwitchLabel,
                SyntaxKind.CaseSwitchLabel,
                SyntaxKind.DefaultSwitchLabel);
        }

        public static bool IsBooleanExpression(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.LogicalOrExpression:
                case SyntaxKind.LogicalAndExpression:
                case SyntaxKind.EqualsExpression:
                case SyntaxKind.NotEqualsExpression:
                case SyntaxKind.LessThanExpression:
                case SyntaxKind.LessThanOrEqualExpression:
                case SyntaxKind.GreaterThanExpression:
                case SyntaxKind.GreaterThanOrEqualExpression:
                case SyntaxKind.IsExpression:
                case SyntaxKind.LogicalNotExpression:
                case SyntaxKind.TrueLiteralExpression:
                case SyntaxKind.FalseLiteralExpression:
                    return true;
                default:
                    return false;
            }
        }

        internal static SyntaxKind GetCompoundAssignmentKind(SyntaxKind binaryExpressionKind)
        {
            switch (binaryExpressionKind)
            {
                case SyntaxKind.AddExpression:
                    return SyntaxKind.AddAssignmentExpression;
                case SyntaxKind.SubtractExpression:
                    return SyntaxKind.SubtractAssignmentExpression;
                case SyntaxKind.MultiplyExpression:
                    return SyntaxKind.MultiplyAssignmentExpression;
                case SyntaxKind.DivideExpression:
                    return SyntaxKind.DivideAssignmentExpression;
                case SyntaxKind.ModuloExpression:
                    return SyntaxKind.ModuloAssignmentExpression;
                case SyntaxKind.LeftShiftExpression:
                    return SyntaxKind.LeftShiftAssignmentExpression;
                case SyntaxKind.RightShiftExpression:
                    return SyntaxKind.RightShiftAssignmentExpression;
                case SyntaxKind.BitwiseOrExpression:
                    return SyntaxKind.OrAssignmentExpression;
                case SyntaxKind.BitwiseAndExpression:
                    return SyntaxKind.AndAssignmentExpression;
                case SyntaxKind.ExclusiveOrExpression:
                    return SyntaxKind.ExclusiveOrAssignmentExpression;
                default:
                    return SyntaxKind.None;
            }
        }

        internal static SyntaxKind GetCompoundAssignmentOperatorKind(SyntaxKind compoundAssignmentKind)
        {
            switch (compoundAssignmentKind)
            {
                case SyntaxKind.AddAssignmentExpression:
                    return SyntaxKind.PlusEqualsToken;
                case SyntaxKind.SubtractAssignmentExpression:
                    return SyntaxKind.MinusEqualsToken;
                case SyntaxKind.MultiplyAssignmentExpression:
                    return SyntaxKind.AsteriskEqualsToken;
                case SyntaxKind.DivideAssignmentExpression:
                    return SyntaxKind.SlashEqualsToken;
                case SyntaxKind.ModuloAssignmentExpression:
                    return SyntaxKind.PercentEqualsToken;
                case SyntaxKind.LeftShiftAssignmentExpression:
                    return SyntaxKind.LessThanLessThanEqualsToken;
                case SyntaxKind.RightShiftAssignmentExpression:
                    return SyntaxKind.GreaterThanGreaterThanEqualsToken;
                case SyntaxKind.OrAssignmentExpression:
                    return SyntaxKind.BarEqualsToken;
                case SyntaxKind.AndAssignmentExpression:
                    return SyntaxKind.AmpersandEqualsToken;
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                    return SyntaxKind.CaretEqualsToken;
                default:
                    return SyntaxKind.None;
            }
        }

        internal static bool CanBeInitializerExpressionInForStatement(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.InvocationExpression:
                case SyntaxKind.PreIncrementExpression:
                case SyntaxKind.PreDecrementExpression:
                case SyntaxKind.PostIncrementExpression:
                case SyntaxKind.PostDecrementExpression:
                case SyntaxKind.ObjectCreationExpression:
                case SyntaxKind.AwaitExpression:
                    return true;
            }

            return SyntaxFacts.IsAssignmentExpression(kind);
        }
    }
}
