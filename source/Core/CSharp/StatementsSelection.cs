// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp
{
    public class StatementsSelection : SyntaxListSelection<StatementSyntax>
    {
        private StatementsSelection(SyntaxList<StatementSyntax> statements, TextSpan span, int startIndex, int endIndex)
             : base(statements, span, startIndex, endIndex)
        {
        }

        public SyntaxList<StatementSyntax> Statements
        {
            get { return (SyntaxList<StatementSyntax>)Items; }
        }

        public static StatementsSelection Create(BlockSyntax block, TextSpan span)
        {
            if (block == null)
                throw new ArgumentNullException(nameof(block));

            return Create(block.Statements, span);
        }

        public static StatementsSelection Create(SwitchSectionSyntax switchSection, TextSpan span)
        {
            if (switchSection == null)
                throw new ArgumentNullException(nameof(switchSection));

            return Create(switchSection.Statements, span);
        }

        public static StatementsSelection Create(StatementsInfo statementsInfo, TextSpan span)
        {
            return Create(statementsInfo.Statements, span);
        }

        private static StatementsSelection Create(SyntaxList<StatementSyntax> statements, TextSpan span)
        {
            (int startIndex, int endIndex) = GetIndexes(statements, span);

            return new StatementsSelection(statements, span, startIndex, endIndex);
        }

        public static bool TryCreate(BlockSyntax block, TextSpan span, out StatementsSelection selectedStatements)
        {
            if (block == null)
            {
                selectedStatements = null;
                return false;
            }

            return TryCreate(block.Statements, span, out selectedStatements);
        }

        public static bool TryCreate(SwitchSectionSyntax switchSection, TextSpan span, out StatementsSelection selectedStatements)
        {
            if (switchSection == null)
            {
                selectedStatements = null;
                return false;
            }

            return TryCreate(switchSection.Statements, span, out selectedStatements);
        }

        public static bool TryCreate(SyntaxList<StatementSyntax> statements, TextSpan span, out StatementsSelection selectedStatements)
        {
            selectedStatements = null;

            if (span.IsEmpty)
                return false;

            if (!statements.Any())
                return false;

            (int startIndex, int endIndex) = GetIndexes(statements, span);

            if (startIndex == -1)
                return false;

            selectedStatements = new StatementsSelection(statements, span, startIndex, endIndex);
            return true;
        }
    }
}
