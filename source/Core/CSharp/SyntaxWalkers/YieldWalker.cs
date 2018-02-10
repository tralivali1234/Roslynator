// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.SyntaxWalkers
{
    internal class YieldWalker : SkipNestedMethodWalker
    {
        private bool _success;

        private YieldWalker()
        {
        }

        public static bool ContainsYield(SyntaxNode node)
        {
            var walker = new YieldWalker();

            walker.Visit(node);

            return walker._success;
        }

        public override void Visit(SyntaxNode node)
        {
            if (!_success)
                base.Visit(node);
        }

        public override void VisitYieldStatement(YieldStatementSyntax node)
        {
            _success = true;
        }
    }
}
