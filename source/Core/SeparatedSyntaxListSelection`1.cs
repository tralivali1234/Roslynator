// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public class SeparatedSyntaxListSelection<TNode> : Selection<TNode> where TNode : SyntaxNode
    {
        private SeparatedSyntaxListSelection(SeparatedSyntaxList<TNode> list, TextSpan span, SelectionResult result)
            : this(list, span, result.FirstIndex, result.LastIndex)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="span"></param>
        /// <param name="firstIndex"></param>
        /// <param name="lastIndex"></param>
        protected SeparatedSyntaxListSelection(SeparatedSyntaxList<TNode> list, TextSpan span, int firstIndex, int lastIndex)
            : base(span, firstIndex, lastIndex)
        {
            UnderlyingList = list;
        }

        /// <summary>
        /// 
        /// </summary>
        public SeparatedSyntaxList<TNode> UnderlyingList { get; }

        /// <summary>
        /// 
        /// </summary>
        protected override IReadOnlyList<TNode> Items => UnderlyingList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public static SeparatedSyntaxListSelection<TNode> Create(SeparatedSyntaxList<TNode> list, TextSpan span)
        {
            SelectionResult result = SelectionResult.Create(list, span);

            return new SeparatedSyntaxListSelection<TNode>(list, span, result.FirstIndex, result.LastIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="span"></param>
        /// <param name="selection"></param>
        /// <returns></returns>
        public static bool TryCreate(SeparatedSyntaxList<TNode> list, TextSpan span, out SeparatedSyntaxListSelection<TNode> selection)
        {
            selection = Create(list, span, 1, int.MaxValue);
            return selection != null;
        }

        internal static bool TryCreate(SeparatedSyntaxList<TNode> list, TextSpan span, int minCount, out SeparatedSyntaxListSelection<TNode> selection)
        {
            selection = Create(list, span, minCount, int.MaxValue);
            return selection != null;
        }

        internal static bool TryCreate(SeparatedSyntaxList<TNode> list, TextSpan span, int minCount, int maxCount, out SeparatedSyntaxListSelection<TNode> selection)
        {
            selection = Create(list, span, minCount, maxCount);
            return selection != null;
        }

        private static SeparatedSyntaxListSelection<TNode> Create(SeparatedSyntaxList<TNode> list, TextSpan span, int minCount, int maxCount)
        {
            SelectionResult result = SelectionResult.Create(list, span, minCount, maxCount);

            if (!result.Success)
                return null;

            return new SeparatedSyntaxListSelection<TNode>(list, span, result);
        }
    }
}
