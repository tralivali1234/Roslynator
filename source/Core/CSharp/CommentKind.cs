// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.CSharp
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum CommentKind
    {

        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        SingleLine = 1,
        /// <summary>
        /// 
        /// </summary>
        MultiLine = 2,
        /// <summary>
        /// 
        /// </summary>
        NonDocumentation = SingleLine | MultiLine,
        /// <summary>
        /// 
        /// </summary>
        SingleLineDocumentation = 4,
        /// <summary>
        /// 
        /// </summary>
        MultiLineDocumentation = 8,
        /// <summary>
        /// 
        /// </summary>
        Documentation = SingleLineDocumentation | MultiLineDocumentation,
        /// <summary>
        /// 
        /// </summary>
        All = NonDocumentation |  Documentation
    }
}
