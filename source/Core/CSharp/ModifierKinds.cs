// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.CSharp
{
    [Flags]
    public enum ModifierKinds
    {
        None = 0,
        New = 1,
        Public = 2,
        Private = 4,
        Protected = 8,
        PrivateProtected = Private | Protected,
        Internal = 16,
        ProtectedInternal = Protected | Internal,
        Accessibility = Public | Private | Protected | Internal,
        Const = 32,
        Static = 64,
        Virtual = 128,
        Sealed = 256,
        Override = 512,
        Abstract = 1024,
        AbstractVirtualOverride = Abstract | Virtual | Override,
        ReadOnly = 2048,
        Extern = 4096,
        Unsafe = 8192,
        Volatile = 16384,
        Async = 32768,
        Partial = 65536,
        Ref = 131072,
        Out = 262144,
        In = 524288,
        Params = 1048576,
    }
}
