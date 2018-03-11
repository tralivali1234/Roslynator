// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings.If
{
    internal abstract class IfRefactoring
    {
        protected IfRefactoring(IfStatementSyntax ifStatement)
        {
            IfStatement = ifStatement;
        }

        public abstract IfRefactoringKind Kind { get; }

        public abstract string Title { get; }

        public abstract Task<Document> RefactorAsync(Document document, CancellationToken cancellationToken = default(CancellationToken));

        public IfStatementSyntax IfStatement { get; }
    }
}
