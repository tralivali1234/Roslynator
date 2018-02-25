// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Roslynator
{
    internal static class EnumerableExtensions
    {
        public static bool IsSorted<T>(this IEnumerable<T> enumerable, IComparer<T> comparer)
        {
            using (IEnumerator<T> en = enumerable.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    T member1 = en.Current;

                    while (en.MoveNext())
                    {
                        T member2 = en.Current;

                        if (comparer.Compare(member1, member2) > 0)
                            return false;

                        member1 = member2;
                    }
                }
            }

            return true;
        }
    }
}
