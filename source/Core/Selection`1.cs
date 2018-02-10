// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator
{
    public abstract class Selection<T> : IReadOnlyList<T>
    {
        protected Selection(IReadOnlyList<T> items, TextSpan span, int firstIndex, int lastIndex)
        {
            UnderlyingList = items;
            Span = span;
            FirstIndex = firstIndex;
            LastIndex = lastIndex;
        }

        public TextSpan Span { get; }

        public IReadOnlyList<T> UnderlyingList { get; }

        public int FirstIndex { get; }

        public int LastIndex { get; }

        public int Count
        {
            get
            {
                if (Any())
                {
                    return LastIndex - FirstIndex + 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < FirstIndex
                    || index > LastIndex)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, "");
                }

                return UnderlyingList[index];
            }
        }

        public bool Any()
        {
            return FirstIndex != -1;
        }

        public T First()
        {
            return UnderlyingList[FirstIndex];
        }

        public T FirstOrDefault()
        {
            if (Any())
            {
                return UnderlyingList[FirstIndex];
            }
            else
            {
                return default(T);
            }
        }

        public T Last()
        {
            return UnderlyingList[LastIndex];
        }

        public T LastOrDefault()
        {
            if (Any())
            {
                return UnderlyingList[LastIndex];
            }
            else
            {
                return default(T);
            }
        }

        private IEnumerable<T> Enumerate()
        {
            if (Any())
            {
                for (int i = FirstIndex; i <= LastIndex; i++)
                    yield return UnderlyingList[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Enumerate().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
