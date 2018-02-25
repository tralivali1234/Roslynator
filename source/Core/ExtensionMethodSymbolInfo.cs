// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Roslynator
{
    public readonly struct ExtensionMethodSymbolInfo
    {
        internal ExtensionMethodSymbolInfo(IMethodSymbol symbol, IMethodSymbol reducedSymbol)
        {
            Symbol = symbol;
            ReducedSymbol = reducedSymbol;
        }

        public IMethodSymbol ReducedSymbol { get; }

        public IMethodSymbol Symbol { get; }

        public IMethodSymbol ReducedSymbolOrSymbol
        {
            get { return ReducedSymbol ?? Symbol; }
        }

        public bool IsFromReduced
        {
            get { return Symbol != null && !object.ReferenceEquals(ReducedSymbol, Symbol); }
        }

        public bool IsFromOrdinary
        {
            get { return Symbol != null && object.ReferenceEquals(ReducedSymbol, Symbol); }
        }

#pragma warning disable CS1591
        public override bool Equals(object obj)
        {
            return obj is ExtensionMethodSymbolInfo other && Equals(other);
        }

        public bool Equals(ExtensionMethodSymbolInfo other)
        {
            return EqualityComparer<IMethodSymbol>.Default.Equals(Symbol, other.Symbol);
        }

        public override int GetHashCode()
        {
            return Symbol?.GetHashCode() ?? 0;
        }

        public static bool operator ==(ExtensionMethodSymbolInfo info1, ExtensionMethodSymbolInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(ExtensionMethodSymbolInfo info1, ExtensionMethodSymbolInfo info2)
        {
            return !(info1 == info2);
        }
#pragma warning restore CS1591
    }
}
