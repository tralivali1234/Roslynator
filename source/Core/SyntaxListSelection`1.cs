// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator
{
    public class SyntaxListSelection<TNode> : Selection<TNode> where TNode : SyntaxNode
    {
        protected SyntaxListSelection(SyntaxList<TNode> list, TextSpan span, int firstIndex, int lastIndex)
            : base(span, firstIndex, lastIndex)
        {
            UnderlyingList = list;
        }

        public SyntaxList<TNode> UnderlyingList { get; }

        protected override IReadOnlyList<TNode> Items => UnderlyingList;

        internal static (int firstIndex, int lastIndex) GetIndexes(SyntaxList<TNode> list, TextSpan span)
        {
            SyntaxList<TNode>.Enumerator en = list.GetEnumerator();

            if (en.MoveNext())
            {
                int i = 0;

                while (span.Start >= en.Current.FullSpan.End
                    && en.MoveNext())
                {
                    i++;
                }

                if (span.Start >= en.Current.FullSpan.Start
                    && span.Start <= en.Current.Span.Start)
                {
                    int j = i;

                    while (span.End > en.Current.FullSpan.End
                        && en.MoveNext())
                    {
                        j++;
                    }

                    if (span.End >= en.Current.Span.End
                        && span.End <= en.Current.FullSpan.End)
                    {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }

        public static SyntaxListSelection<TNode> Create(SyntaxList<TNode> list, TextSpan span)
        {
            (int firstIndex, int lastIndex) = GetIndexes(list, span);

            return new SyntaxListSelection<TNode>(list, span, firstIndex, lastIndex);
        }

        public static bool TryCreate(SyntaxList<TNode> list, TextSpan span, out SyntaxListSelection<TNode> selection)
        {
            selection = null;

            if (!list.Any())
                return false;

            if (span.IsEmpty)
                return false;

            (int firstIndex, int lastIndex) = GetIndexes(list, span);

            if (firstIndex == -1)
                return false;

            selection = new SyntaxListSelection<TNode>(list, span, firstIndex, lastIndex);
            return true;
        }
    }
}
