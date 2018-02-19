// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.CSharp
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum PreprocessorDirectiveKind
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        If = 1,
        /// <summary>
        /// 
        /// </summary>
        Elif = 2,
        /// <summary>
        /// 
        /// </summary>
        Else = 4,
        /// <summary>
        /// 
        /// </summary>
        EndIf = 8,
        /// <summary>
        /// 
        /// </summary>
        Region = 16,
        /// <summary>
        /// 
        /// </summary>
        EndRegion = 32,
        /// <summary>
        /// 
        /// </summary>
        Define = 64,
        /// <summary>
        /// 
        /// </summary>
        Undef = 128,
        /// <summary>
        /// 
        /// </summary>
        Error = 256,
        /// <summary>
        /// 
        /// </summary>
        Warning = 512,
        /// <summary>
        /// 
        /// </summary>
        Line = 1024,
        /// <summary>
        /// 
        /// </summary>
        PragmaWarning = 2048,
        /// <summary>
        /// 
        /// </summary>
        PragmaChecksum = 4096,
        /// <summary>
        /// 
        /// </summary>
        Pragma = PragmaWarning | PragmaChecksum,
        /// <summary>
        /// 
        /// </summary>
        Reference = 8192,
        /// <summary>
        /// 
        /// </summary>
        Load = 16384,
        /// <summary>
        /// 
        /// </summary>
        Bad = 32768,
        /// <summary>
        /// 
        /// </summary>
        Shebang = 65536,
        /// <summary>
        /// 
        /// </summary>
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
    }
}
