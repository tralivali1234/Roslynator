// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CSharp
{
    internal static class ModifierFlagsExtensions
    {
        public static bool Any(this ModifierFlags flags, ModifierFlags value)
        {
            return (flags & value) != 0;
        }

        public static bool All(this ModifierFlags flags, ModifierFlags value)
        {
            return (flags & value) != value;
        }

        public static bool HasNew(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.New);
        }

        public static bool HasPublic(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Public);
        }

        public static bool HasPrivate(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Private);
        }

        public static bool HasProtected(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Protected);
        }

        public static bool HasInternal(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Internal);
        }

        public static bool HasConst(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Const);
        }

        public static bool HasStatic(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Static);
        }

        public static bool HasVirtual(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Virtual);
        }

        public static bool HasSealed(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Sealed);
        }

        public static bool HasOverride(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Override);
        }

        public static bool HasAbstract(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Abstract);
        }

        public static bool HasReadOnly(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.ReadOnly);
        }

        public static bool HasExtern(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Extern);
        }

        public static bool HasUnsafe(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Unsafe);
        }

        public static bool HasVolatile(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Volatile);
        }

        //TODO: HasAsync
        public static bool HasAsync(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Async);
        }

        public static bool HasPartial(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Partial);
        }

        public static bool HasRef(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Ref);
        }

        public static bool HasOut(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Out);
        }

        public static bool HasIn(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.In);
        }

        public static bool HasParams(this ModifierFlags flags)
        {
            return Any(flags, ModifierFlags.Params);
        }
    }
}
