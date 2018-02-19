// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator
{
    /// <summary>
    /// 
    /// </summary>
    public static class SyntaxTreeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <param name="span"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static int GetStartLine(
            this SyntaxTree syntaxTree,
            TextSpan span,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (syntaxTree == null)
                throw new ArgumentNullException(nameof(syntaxTree));

            return syntaxTree.GetLineSpan(span, cancellationToken).StartLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <param name="span"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static int GetEndLine(
            this SyntaxTree syntaxTree,
            TextSpan span,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (syntaxTree == null)
                throw new ArgumentNullException(nameof(syntaxTree));

            return syntaxTree.GetLineSpan(span, cancellationToken).EndLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <param name="span"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static bool IsMultiLineSpan(
            this SyntaxTree syntaxTree,
            TextSpan span,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (syntaxTree == null)
                throw new ArgumentNullException(nameof(syntaxTree));

            return syntaxTree.GetLineSpan(span, cancellationToken).IsMultiLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <param name="span"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static bool IsSingleLineSpan(
            this SyntaxTree syntaxTree,
            TextSpan span,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (syntaxTree == null)
                throw new ArgumentNullException(nameof(syntaxTree));

            return syntaxTree.GetLineSpan(span, cancellationToken).IsSingleLine();
        }

        internal static int GetLineCount(
            this SyntaxTree syntaxTree,
            TextSpan span,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (syntaxTree == null)
                throw new ArgumentNullException(nameof(syntaxTree));

            return syntaxTree.GetLineSpan(span, cancellationToken).GetLineCount();
        }
    }
}
