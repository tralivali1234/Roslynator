// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.CSharp
{
    [Flags]
    public enum CommentKinds
    {

        None = 0,
        SingleLine = 1,
        MultiLine = 2,
        NonDocumentation = SingleLine | MultiLine,
        SingleLineDocumentation = 4,
        MultiLineDocumentation = 8,
        Documentation = SingleLineDocumentation | MultiLineDocumentation,
        All = NonDocumentation |  Documentation
    }
}
