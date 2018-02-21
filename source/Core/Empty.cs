// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Roslynator
{
    internal static class Empty
    {
        internal class Enumerator : IEnumerator
        {
            public static readonly IEnumerator Instance = new Enumerator();

            protected Enumerator()
            {
            }

            public object Current
            {
                get { throw new InvalidOperationException(); }
            }

            public bool MoveNext()
            {
                return false;
            }

            public void Reset()
            {
                throw new InvalidOperationException();
            }
        }

        internal class Enumerator<T> : Enumerator, IEnumerator<T>
        {
            new public static readonly IEnumerator<T> Instance = new Enumerator<T>();

            protected Enumerator()
            {
            }

            new public T Current
            {
                get { throw new InvalidOperationException(); }
            }

            public void Dispose()
            {
            }
        }
    }
}
