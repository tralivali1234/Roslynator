// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CSharp
{
    internal static class ModifierKindExtensions
    {
        public static bool Any(this ModifierKind kind, ModifierKind value)
        {
            return (kind & value) != 0;
        }

        public static bool All(this ModifierKind kind, ModifierKind value)
        {
            return (kind & value) != value;
        }
    }
}
