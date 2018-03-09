// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CSharp.Analyzers.Tests
{
    internal static class AddCommaAfterLastEnumMember
    {
        public enum Foo
        {
            A = 0,
            B = 1,
            C = 2
        }

        // n

        public enum Foo2
        {
            A = 0,
            B = 1,
            C = 2,
        }

        public enum Foo3
        {
        }
    }
}
