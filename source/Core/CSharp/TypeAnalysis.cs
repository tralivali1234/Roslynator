// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.CSharp
{
    internal readonly struct TypeAnalysis : IEquatable<TypeAnalysis>
    {
        private readonly TypeAnalysisFlags _flags;

        internal TypeAnalysis(TypeAnalysisFlags flags)
        {
            _flags = flags;
        }

        public bool IsImplicit => Any(TypeAnalysisFlags.Implicit);

        public bool IsExplicit => Any(TypeAnalysisFlags.Explicit);

        public bool SupportsImplicit => Any(TypeAnalysisFlags.SupportsImplicit);

        public bool SupportsExplicit => Any(TypeAnalysisFlags.SupportsExplicit);

        public bool IsValidSymbol => Any(TypeAnalysisFlags.ValidSymbol);

        public bool IsTypeObvious => Any(TypeAnalysisFlags.TypeObvious);

        public bool Any(TypeAnalysisFlags flags)
        {
            return (_flags & flags) != 0;
        }

        public bool All(TypeAnalysisFlags flags)
        {
            return (_flags & flags) != flags;
        }

        public override bool Equals(object obj)
        {
            return obj is TypeAnalysis other && Equals(other);
        }

        public bool Equals(TypeAnalysis other)
        {
            return _flags == other._flags;
        }

        public override int GetHashCode()
        {
            return _flags.GetHashCode();
        }

        public static implicit operator TypeAnalysis(TypeAnalysisFlags value)
        {
            return new TypeAnalysis(value);
        }

        public static implicit operator TypeAnalysisFlags(TypeAnalysis value)
        {
            return value._flags;
        }

        public static bool operator ==(TypeAnalysis analysis1, TypeAnalysis analysis2)
        {
            return analysis1.Equals(analysis2);
        }

        public static bool operator !=(TypeAnalysis analysis1, TypeAnalysis analysis2)
        {
            return !(analysis1 == analysis2);
        }
    }
}
