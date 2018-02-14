// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Roslynator.CSharp.Syntax.SyntaxInfoHelpers;

namespace Roslynator.CSharp.Syntax
{
    public struct SimpleIfStatementInfo : IEquatable<SimpleIfStatementInfo>
    {
        private SimpleIfStatementInfo(
            IfStatementSyntax ifStatement,
            ExpressionSyntax condition,
            StatementSyntax statement)
        {
            IfStatement = ifStatement;
            Condition = condition;
            Statement = statement;
        }

        private static SimpleIfStatementInfo Default { get; } = new SimpleIfStatementInfo();

        public IfStatementSyntax IfStatement { get; }

        public ExpressionSyntax Condition { get; }

        public StatementSyntax Statement { get; }

        public bool Success
        {
            get { return IfStatement != null; }
        }

        internal static SimpleIfStatementInfo Create(
            SyntaxNode node,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            return Create(node as IfStatementSyntax, walkDownParentheses, allowMissing);
        }

        internal static SimpleIfStatementInfo Create(
            IfStatementSyntax ifStatement,
            bool walkDownParentheses = true,
            bool allowMissing = false)
        {
            if (ifStatement?.IsSimpleIf() != true)
                return Default;

            ExpressionSyntax condition = WalkAndCheck(ifStatement.Condition, allowMissing, walkDownParentheses);

            if (condition == null)
                return Default;

            StatementSyntax statement = ifStatement.Statement;

            if (!Check(statement, allowMissing))
                return Default;

            return new SimpleIfStatementInfo(ifStatement, condition, statement);
        }

        public override string ToString()
        {
            return IfStatement?.ToString() ?? base.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is SimpleIfStatementInfo other && Equals(other);
        }

        public bool Equals(SimpleIfStatementInfo other)
        {
            return EqualityComparer<IfStatementSyntax>.Default.Equals(IfStatement, other.IfStatement);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<IfStatementSyntax>.Default.GetHashCode(IfStatement);
        }

        public static bool operator ==(SimpleIfStatementInfo info1, SimpleIfStatementInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(SimpleIfStatementInfo info1, SimpleIfStatementInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
