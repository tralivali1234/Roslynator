// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator
{
    /// <summary>
    /// 
    /// </summary>
    public enum ExtensionMethodKind
    {
        /// <summary>
        /// Unspecified extension method.
        /// </summary>
        None = 0,
        /// <summary>
        /// Non-reduced extension method (with "this" parameter not removed).
        /// </summary>
        NonReduced = 1,
        /// <summary>
        /// Reduced extension method (with "this" parameter removed).
        /// </summary>
        Reduced = 2,
    }
}
