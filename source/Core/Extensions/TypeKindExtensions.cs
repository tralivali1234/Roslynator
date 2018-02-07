// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Roslynator
{
    internal static class TypeKindExtensions
    {
        public static bool Is(this TypeKind typeKind, TypeKind typeKind1, TypeKind typeKind2)
        {
            return typeKind == typeKind1
                || typeKind == typeKind2;
        }

        public static bool Is(this TypeKind typeKind, TypeKind typeKind1, TypeKind typeKind2, TypeKind typeKind3)
        {
            return typeKind == typeKind1
                || typeKind == typeKind2
                || typeKind == typeKind3;
        }

        public static bool Is(this TypeKind typeKind, TypeKind typeKind1, TypeKind typeKind2, TypeKind typeKind3, TypeKind typeKind4)
        {
            return typeKind == typeKind1
                || typeKind == typeKind2
                || typeKind == typeKind3
                || typeKind == typeKind4;
        }

        public static bool Is(this TypeKind typeKind, TypeKind typeKind1, TypeKind typeKind2, TypeKind typeKind3, TypeKind typeKind4, TypeKind typeKind5)
        {
            return typeKind == typeKind1
                || typeKind == typeKind2
                || typeKind == typeKind3
                || typeKind == typeKind4
                || typeKind == typeKind5;
        }
    }
}
