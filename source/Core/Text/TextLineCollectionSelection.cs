// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.Text
{
    public class TextLineCollectionSelection : Selection<TextLine>
    {
        private TextLineCollectionSelection(TextLineCollection lines, TextSpan span, int firstIndex, int lastIndex)
            : base(span, firstIndex, lastIndex)
        {
            UnderlyingLines = lines;
        }

        public TextLineCollection UnderlyingLines { get; }

        protected override IReadOnlyList<TextLine> Items => UnderlyingLines;

        private static (int firstIndex, int lastIndex) GetIndexes(TextLineCollection lines, TextSpan span)
        {
            using (TextLineCollection.Enumerator en = lines.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    int i = 0;

                    while (span.Start >= en.Current.EndIncludingLineBreak
                        && en.MoveNext())
                    {
                        i++;
                    }

                    if (span.Start == en.Current.Start)
                    {
                        int j = i;

                        while (span.End > en.Current.EndIncludingLineBreak
                            && en.MoveNext())
                        {
                            j++;
                        }

                        if (span.End == en.Current.End
                            || span.End == en.Current.EndIncludingLineBreak)
                        {
                            return (i, j);
                        }
                    }
                }
            }

            return (-1, -1);
        }

        public static TextLineCollectionSelection Create(TextLineCollection lines, TextSpan span)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            (int firstIndex, int lastIndex) = GetIndexes(lines, span);

            return new TextLineCollectionSelection(lines, span, firstIndex, lastIndex);
        }

        public static bool TryCreate(TextLineCollection lines, TextSpan span, out TextLineCollectionSelection selection)
        {
            selection = null;

            if (lines.Count == 0)
                return false;

            if (span.IsEmpty)
                return false;

            (int firstIndex, int lastIndex) = GetIndexes(lines, span);

            if (firstIndex == -1)
                return false;

            selection = new TextLineCollectionSelection(lines, span, firstIndex, lastIndex);
            return true;
        }
    }
}
