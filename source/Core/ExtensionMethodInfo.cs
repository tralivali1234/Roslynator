// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Roslynator
{
    internal readonly struct ExtensionMethodInfo
    {
        private ExtensionMethodInfo(IMethodSymbol methodSymbol, IMethodSymbol reducedSymbol)
        {
            MethodInfo = new MethodInfo(methodSymbol);
            ReducedSymbol = reducedSymbol;
        }

        public MethodInfo MethodInfo { get; }

        public IMethodSymbol ReducedSymbol { get; }

        public IMethodSymbol Symbol
        {
            get { return MethodInfo.Symbol; }
        }

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

        public static ExtensionMethodInfo Create(IMethodSymbol methodSymbol, ExtensionMethodKind kind = ExtensionMethodKind.None)
        {
            if (methodSymbol?.IsExtensionMethod == true)
            {
                IMethodSymbol reducedFrom = methodSymbol.ReducedFrom;

                if (reducedFrom != null)
                {
                    if (kind != ExtensionMethodKind.NonReduced)
                    {
                        return new ExtensionMethodInfo(reducedFrom, methodSymbol);
                    }
                }
                else if (kind != ExtensionMethodKind.Reduced)
                {
                    return new ExtensionMethodInfo(methodSymbol, null);
                }
            }

            return default(ExtensionMethodInfo);
        }
    }
}
