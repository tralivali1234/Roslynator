// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator
{
    [Flags]
    public enum PreprocessorDirectiveRemoveOptions
    {
        None = 0,
        If = 1,
        Elif = 2,
        Else = 4,
        EndIf = 8,
        Region = 16,
        EndRegion = 32,
        RegionOrEndRegion = Region | EndRegion,
        Define = 64,
        Undef = 128,
        Error = 256,
        Warning = 512,
        Line = 1024,
        PragmaWarning = 2048,
        PragmaChecksum = 4096,
        Pragma = PragmaWarning | PragmaChecksum,
        Reference = 8192,
        Load = 16384,
        Bad = 32768,
        Shebang = 65536,
        All = If
            | Elif
            | Else
            | EndIf
            | Region
            | EndRegion
            | Define
            | Undef
            | Error
            | Warning
            | Line
            | PragmaWarning
            | PragmaChecksum
            | Reference
            | Load
            | Bad
            | Shebang,

        //All,
        //AllExceptRegion,
        //Region
    }
}
