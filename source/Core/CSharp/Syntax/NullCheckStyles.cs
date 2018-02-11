// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

#pragma warning disable RCS1130

namespace Roslynator.CSharp.Syntax
{
    //XTODO: Roslynator.CSharp
    [Flags]
    public enum NullCheckStyles
    {
        None = 0,

        // x == null
        EqualsToNull = 1,

        // x != null
        NotEqualsToNull = 2,

        ComparisonToNull = EqualsToNull | NotEqualsToNull,

        // x is null
        IsNull = 4,

        // !(x is null)
        NotIsNull = 8,

        Pattern = IsNull | NotIsNull,

        // !x.HasValue
        NotHasValue = 16,

        CheckingNull = EqualsToNull | IsNull | NotHasValue,

        // x.HasValue
        HasValue = 32,

        CheckingNotNull = NotEqualsToNull | NotIsNull | HasValue,

        HasValueProperty = HasValue | NotHasValue,

        All = CheckingNull | CheckingNotNull
    }
}
