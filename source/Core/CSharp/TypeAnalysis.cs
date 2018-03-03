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

        public bool IsImplicit()
        {
            return (_flags & TypeAnalysisFlags.Implicit) != 0;
        }

        public bool IsExplicit()
        {
            return (_flags & TypeAnalysisFlags.Explicit) != 0;
        }

        public bool SupportsImplicit()
        {
            return (_flags & TypeAnalysisFlags.SupportsImplicit) != 0;
        }

        public bool SupportsExplicit()
        {
            return (_flags & TypeAnalysisFlags.SupportsExplicit) != 0;
        }

        public bool IsValidSymbol()
        {
            return (_flags & TypeAnalysisFlags.ValidSymbol) != 0;
        }

        public bool IsTypeObvious()
        {
            return (_flags & TypeAnalysisFlags.TypeObvious) != 0;
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
