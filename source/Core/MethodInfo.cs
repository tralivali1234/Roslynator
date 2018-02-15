// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Roslynator
{
    public struct MethodInfo : IEquatable<MethodInfo>
    {
        internal MethodInfo(IMethodSymbol symbol)
        {
            Debug.Assert(symbol != null);

            Symbol = symbol;
        }

        public IMethodSymbol Symbol { get; }

        public bool IsReturnType(SpecialType specialType)
        {
            return ReturnType.SpecialType == specialType;
        }

        public bool IsContainingType(SpecialType specialType)
        {
            return ContainingType?.SpecialType == specialType;
        }

        public bool ReturnsObject
        {
            get { return IsReturnType(SpecialType.System_Object); }
        }

        public bool ReturnsBoolean
        {
            get { return IsReturnType(SpecialType.System_Boolean); }
        }

        public bool ReturnsInt
        {
            get { return IsReturnType(SpecialType.System_Int32); }
        }

        public bool ReturnsString
        {
            get { return IsReturnType(SpecialType.System_String); }
        }

        public bool ReturnsIEnumerableOf(ITypeSymbol typeArgument)
        {
            return ReturnType.IsIEnumerableOf(typeArgument);
        }

        public bool ReturnsIEnumerableOf(Func<ITypeSymbol, bool> typeArgumentPredicate)
        {
            return ReturnType.IsIEnumerableOf(typeArgumentPredicate);
        }

        internal bool IsName(string name)
        {
            return string.Equals(Name, name, StringComparison.Ordinal);
        }

        internal bool IsName(string name1, string name2)
        {
            return IsName(name1)
                || IsName(name2);
        }

        internal bool IsName(string name1, string name2, string name3)
        {
            return IsName(name1)
                || IsName(name2)
                || IsName(name3);
        }

        private bool IsNullOrName(string name)
        {
            return name == null || IsName(name);
        }

        internal bool HasParameter(SpecialType parameterType)
        {
            ImmutableArray<IParameterSymbol> parameters = Parameters;

            return parameters.Length == 1
                && parameters[0].Type.SpecialType == parameterType;
        }

        internal bool HasParameters(SpecialType firstParameterType, SpecialType secondParameterType)
        {
            ImmutableArray<IParameterSymbol> parameters = Parameters;

            return parameters.Length == 2
                && parameters[0].Type.SpecialType == firstParameterType
                && parameters[1].Type.SpecialType == secondParameterType;
        }

        public bool IsPublic
        {
            get { return DeclaredAccessibility == Accessibility.Public; }
        }

        public bool IsInternal
        {
            get { return DeclaredAccessibility == Accessibility.Internal; }
        }

        public bool IsProtected
        {
            get { return DeclaredAccessibility == Accessibility.Protected; }
        }

        public bool IsPrivate
        {
            get { return DeclaredAccessibility == Accessibility.Private; }
        }

        internal bool IsPublicStaticNonGeneric(string name = null)
        {
            return IsNullOrName(name)
                && IsPublic
                && IsStatic
                && !IsGenericMethod;
        }

        internal bool IsPublicInstanceNonGeneric(string name = null)
        {
            return IsNullOrName(name)
                && IsPublic
                && !IsStatic
                && !IsGenericMethod;
        }

        internal bool IsLinqExtensionOfIEnumerableOfTWithoutParameters(
            string name,
            SemanticModel semanticModel,
            bool allowImmutableArrayExtension = false)
        {
            return IsLinqExtensionOfIEnumerableOfT(semanticModel, name, parameterCount: 1, allowImmutableArrayExtension: allowImmutableArrayExtension);
        }

        internal bool IsLinqElementAt(
            SemanticModel semanticModel,
            bool allowImmutableArrayExtension = false)
        {
            return IsLinqExtensionOfIEnumerableOfT(semanticModel, "ElementAt", parameterCount: 2, allowImmutableArrayExtension: allowImmutableArrayExtension)
                && Parameters[1].Type.IsInt();
        }

        internal bool IsLinqWhere(
            SemanticModel semanticModel,
            bool allowImmutableArrayExtension = false)
        {
            return IsLinqExtensionOfIEnumerableOfTWithPredicate("Where", parameterCount: 2, semanticModel: semanticModel, allowImmutableArrayExtension: allowImmutableArrayExtension);
        }

        internal bool IsLinqWhereWithIndex(SemanticModel semanticModel)
        {
            return IsLinqExtensionOfIEnumerableOfT(semanticModel, "Where", parameterCount: 2, allowImmutableArrayExtension: false)
                && SymbolUtility.IsPredicateFunc(Parameters[1].Type, Symbol.TypeArguments[0], semanticModel.Compilation.GetSpecialType(SpecialType.System_Int32), semanticModel);
        }

        internal bool IsLinqSelect(SemanticModel semanticModel, bool allowImmutableArrayExtension = false)
        {
            if (!Symbol.IsPublic())
                return false;

            if (!Symbol.ReturnType.IsConstructedFromIEnumerableOfT())
                return false;

            if (!IsName("Select"))
                return false;

            if (Symbol.Arity != 2)
                return false;

            INamedTypeSymbol containingType = Symbol.ContainingType;

            if (containingType == null)
                return false;

            if (containingType.Equals(semanticModel.GetTypeByMetadataName(MetadataNames.System_Linq_Enumerable)))
            {
                ImmutableArray<IParameterSymbol> parameters = Parameters;

                return parameters.Length == 2
                    && parameters[0].Type.IsConstructedFromIEnumerableOfT()
                    && SymbolUtility.IsFunc(parameters[1].Type, Symbol.TypeArguments[0], Symbol.TypeArguments[1], semanticModel);
            }
            else if (allowImmutableArrayExtension
                && containingType.Equals(semanticModel.GetTypeByMetadataName(MetadataNames.System_Linq_ImmutableArrayExtensions)))
            {
                ImmutableArray<IParameterSymbol> parameters = Parameters;

                return parameters.Length == 2
                    && parameters[0].Type.IsConstructedFromImmutableArrayOfT(semanticModel)
                    && SymbolUtility.IsFunc(parameters[1].Type, Symbol.TypeArguments[0], Symbol.TypeArguments[1], semanticModel);
            }

            return false;
        }

        internal bool IsLinqCast(SemanticModel semanticModel)
        {
            return Symbol.IsPublic()
                && Symbol.ReturnType.IsConstructedFromIEnumerableOfT()
                && IsName("Cast")
                && Symbol.Arity == 1
                && Symbol.Parameters.SingleOrDefault(shouldThrow: false)?.Type.IsIEnumerable() == true
                && Symbol
                    .ContainingType?
                    .Equals(semanticModel.GetTypeByMetadataName(MetadataNames.System_Linq_Enumerable)) == true;
        }

        internal bool IsLinqExtensionOfIEnumerableOfT(
            SemanticModel semanticModel,
            string name = null,
            int parameterCount = -1,
            bool allowImmutableArrayExtension = false)
        {
            if (!Symbol.IsPublic())
                return false;

            if (!IsNullOrName(name))
                return false;

            INamedTypeSymbol containingType = Symbol.ContainingType;

            if (containingType == null)
                return false;

            if (containingType.Equals(semanticModel.GetTypeByMetadataName(MetadataNames.System_Linq_Enumerable)))
            {
                ImmutableArray<IParameterSymbol> parameters = Parameters;

                return (parameterCount == -1 || parameters.Length == parameterCount)
                    && parameters[0].Type.IsConstructedFromIEnumerableOfT();
            }
            else if (allowImmutableArrayExtension
                && containingType.Equals(semanticModel.GetTypeByMetadataName(MetadataNames.System_Linq_ImmutableArrayExtensions)))
            {
                ImmutableArray<IParameterSymbol> parameters = Parameters;

                return (parameterCount == -1 || parameters.Length == parameterCount)
                    && parameters[0].Type.IsConstructedFromImmutableArrayOfT(semanticModel);
            }

            return false;
        }

        internal bool IsLinqExtensionOfIEnumerableOfTWithPredicate(
            SemanticModel semanticModel,
            string name = null,
            bool allowImmutableArrayExtension = false)
        {
            return IsLinqExtensionOfIEnumerableOfTWithPredicate(name, parameterCount: 2, semanticModel: semanticModel, allowImmutableArrayExtension: allowImmutableArrayExtension);
        }

        private bool IsLinqExtensionOfIEnumerableOfTWithPredicate(
            string name,
            int parameterCount,
            SemanticModel semanticModel,
            bool allowImmutableArrayExtension = false)
        {
            if (!Symbol.IsPublic())
                return false;

            if (!IsNullOrName(name))
                return false;

            INamedTypeSymbol containingType = Symbol.ContainingType;

            if (containingType == null)
                return false;

            if (containingType.Equals(semanticModel.GetTypeByMetadataName(MetadataNames.System_Linq_Enumerable)))
            {
                ImmutableArray<IParameterSymbol> parameters = Parameters;

                return parameters.Length == parameterCount
                    && parameters[0].Type.IsConstructedFromIEnumerableOfT()
                    && SymbolUtility.IsPredicateFunc(parameters[1].Type, Symbol.TypeArguments[0], semanticModel);
            }
            else if (allowImmutableArrayExtension
                && containingType.Equals(semanticModel.GetTypeByMetadataName(MetadataNames.System_Linq_ImmutableArrayExtensions)))
            {
                ImmutableArray<IParameterSymbol> parameters = Parameters;

                return parameters.Length == parameterCount
                    && parameters[0].Type.IsConstructedFromImmutableArrayOfT(semanticModel)
                    && SymbolUtility.IsPredicateFunc(parameters[1].Type, Symbol.TypeArguments[0], semanticModel);
            }

            return false;
        }

        #region IMethodSymbol
        public MethodKind MethodKind => Symbol.MethodKind;

        public bool IsGenericMethod => Symbol.IsGenericMethod;

        public bool IsExtensionMethod => Symbol.IsExtensionMethod;

        public bool IsAsync => Symbol.IsAsync;

        public bool ReturnsVoid => Symbol.ReturnsVoid;

        public ITypeSymbol ReturnType => Symbol.ReturnType;

        public ImmutableArray<ITypeSymbol> TypeArguments => Symbol.TypeArguments;

        public ImmutableArray<ITypeParameterSymbol> TypeParameters => Symbol.TypeParameters;

        public ImmutableArray<IParameterSymbol> Parameters => Symbol.Parameters;

        public IMethodSymbol ConstructedFrom => Symbol.ConstructedFrom;

        public IMethodSymbol OverriddenMethod => Symbol.OverriddenMethod;

        public IMethodSymbol ReducedFrom => Symbol.ReducedFrom;

        public SymbolKind Kind => Symbol.Kind;

        public string Name => Symbol.Name;

        public string MetadataName => Symbol.MetadataName;

        public ISymbol ContainingSymbol => Symbol.ContainingSymbol;

        public INamedTypeSymbol ContainingType => Symbol.ContainingType;

        public INamespaceSymbol ContainingNamespace => Symbol.ContainingNamespace;

        public bool IsStatic => Symbol.IsStatic;

        public bool IsVirtual => Symbol.IsVirtual;

        public bool IsOverride => Symbol.IsOverride;

        public bool IsAbstract => Symbol.IsAbstract;

        public bool IsSealed => Symbol.IsSealed;

        public Accessibility DeclaredAccessibility => Symbol.DeclaredAccessibility;
        #endregion IMethodSymbol

        public override bool Equals(object obj)
        {
            return obj is MethodInfo other && Equals(other);
        }

        public bool Equals(MethodInfo other)
        {
            return EqualityComparer<IMethodSymbol>.Default.Equals(Symbol, other.Symbol);
        }

        public override int GetHashCode()
        {
            return Symbol?.GetHashCode() ?? 0;
        }

        public static bool operator ==(MethodInfo info1, MethodInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(MethodInfo info1, MethodInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
