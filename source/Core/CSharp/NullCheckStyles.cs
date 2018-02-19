// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

#pragma warning disable RCS1130

namespace Roslynator.CSharp
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum NullCheckStyles
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,

        // x == null
        /// <summary>
        /// 
        /// </summary>
        EqualsToNull = 1,

        // x != null
        /// <summary>
        /// 
        /// </summary>
        NotEqualsToNull = 2,

        /// <summary>
        /// 
        /// </summary>
        ComparisonToNull = EqualsToNull | NotEqualsToNull,

        // x is null
        /// <summary>
        /// 
        /// </summary>
        IsNull = 4,

        // !(x is null)
        /// <summary>
        /// 
        /// </summary>
        NotIsNull = 8,

        /// <summary>
        /// 
        /// </summary>
        IsPattern = IsNull | NotIsNull,

        // !x.HasValue
        /// <summary>
        /// 
        /// </summary>
        NotHasValue = 16,

        /// <summary>
        /// 
        /// </summary>
        CheckingNull = EqualsToNull | IsNull | NotHasValue,

        // x.HasValue
        /// <summary>
        /// 
        /// </summary>
        HasValue = 32,

        /// <summary>
        /// 
        /// </summary>
        CheckingNotNull = NotEqualsToNull | NotIsNull | HasValue,

        /// <summary>
        /// 
        /// </summary>
        HasValueProperty = HasValue | NotHasValue,

        /// <summary>
        /// 
        /// </summary>
        All = CheckingNull | CheckingNotNull
    }
}
