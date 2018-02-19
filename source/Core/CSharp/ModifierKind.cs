// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.CSharp
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum ModifierKind
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        New = 1,
        /// <summary>
        /// 
        /// </summary>
        Public = 2,
        /// <summary>
        /// 
        /// </summary>
        Private = 4,
        /// <summary>
        /// 
        /// </summary>
        Protected = 8,
        /// <summary>
        /// 
        /// </summary>
        PrivateProtected = Private | Protected,
        /// <summary>
        /// 
        /// </summary>
        Internal = 16,
        /// <summary>
        /// 
        /// </summary>
        ProtectedInternal = Protected | Internal,
        /// <summary>
        /// 
        /// </summary>
        Accessibility = Public | Private | Protected | Internal,
        /// <summary>
        /// 
        /// </summary>
        Const = 32,
        /// <summary>
        /// 
        /// </summary>
        Static = 64,
        /// <summary>
        /// 
        /// </summary>
        Virtual = 128,
        /// <summary>
        /// 
        /// </summary>
        Sealed = 256,
        /// <summary>
        /// 
        /// </summary>
        Override = 512,
        /// <summary>
        /// 
        /// </summary>
        Abstract = 1024,
        /// <summary>
        /// 
        /// </summary>
        AbstractVirtualOverride = Abstract | Virtual | Override,
        /// <summary>
        /// 
        /// </summary>
        ReadOnly = 2048,
        /// <summary>
        /// 
        /// </summary>
        Extern = 4096,
        /// <summary>
        /// 
        /// </summary>
        Unsafe = 8192,
        /// <summary>
        /// 
        /// </summary>
        Volatile = 16384,
        /// <summary>
        /// 
        /// </summary>
        Async = 32768,
        /// <summary>
        /// 
        /// </summary>
        Partial = 65536,
        /// <summary>
        /// 
        /// </summary>
        Ref = 131072,
        /// <summary>
        /// 
        /// </summary>
        Out = 262144,
        /// <summary>
        /// 
        /// </summary>
        In = 524288,
        /// <summary>
        /// 
        /// </summary>
        Params = 1048576,
    }
}
