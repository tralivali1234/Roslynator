// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Roslynator
{
    /// <summary>
    /// Represents a method symbol.
    /// </summary>
    public readonly struct MethodInfo : IEquatable<MethodInfo>
    {
        internal MethodInfo(IMethodSymbol symbol)
        {
            Debug.Assert(symbol != null);

            Symbol = symbol;
        }

        /// <summary>
        /// A method symbol.
        /// </summary>
        public IMethodSymbol Symbol { get; }

        /// <summary>
        /// Determines whether this method return type is the specified type.
        /// </summary>
        /// <param name="specialType"></param>
        /// <returns></returns>
        public bool IsReturnType(SpecialType specialType)
        {
            return ReturnType.SpecialType == specialType;
        }

        /// <summary>
        /// Determines whether this method is contained in the specified type.
        /// </summary>
        /// <param name="specialType"></param>
        /// <returns></returns>
        public bool IsContainingType(SpecialType specialType)
        {
            return ContainingType?.SpecialType == specialType;
        }

        /// <summary>
        /// Determines whether this method return type is <see cref="object"/>.
        /// </summary>
        public bool ReturnsObject
        {
            get { return IsReturnType(SpecialType.System_Object); }
        }

        /// <summary>
        /// Determines whether this method return type is <see cref="bool"/>.
        /// </summary>
        public bool ReturnsBoolean
        {
            get { return IsReturnType(SpecialType.System_Boolean); }
        }

        /// <summary>
        /// Determines whether this method return type is <see cref="int"/>.
        /// </summary>
        public bool ReturnsInt
        {
            get { return IsReturnType(SpecialType.System_Int32); }
        }

        /// <summary>
        /// Determines whether this method return type is <see cref="string"/>.
        /// </summary>
        public bool ReturnsString
        {
            get { return IsReturnType(SpecialType.System_String); }
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

        /// <summary>
        /// Determines whether this method is public.
        /// </summary>
        public bool IsPublic
        {
            get { return DeclaredAccessibility == Accessibility.Public; }
        }

        /// <summary>
        /// Determines whether this method is internal.
        /// </summary>
        public bool IsInternal
        {
            get { return DeclaredAccessibility == Accessibility.Internal; }
        }

        /// <summary>
        /// Determined whether this method is protected.
        /// </summary>
        public bool IsProtected
        {
            get { return DeclaredAccessibility == Accessibility.Protected; }
        }

        /// <summary>
        /// Determines whether this method is private.
        /// </summary>
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
        /// <summary>
        /// Gets <see cref="IMethodSymbol.MethodKind"/>.
        /// </summary>
        public MethodKind MethodKind => Symbol.MethodKind;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.IsGenericMethod"/>.
        /// </summary>
        public bool IsGenericMethod => Symbol.IsGenericMethod;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.IsExtensionMethod"/>.
        /// </summary>
        public bool IsExtensionMethod => Symbol.IsExtensionMethod;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.IsAsync"/>.
        /// </summary>
        public bool IsAsync => Symbol.IsAsync;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.ReturnsVoid"/>.
        /// </summary>
        public bool ReturnsVoid => Symbol.ReturnsVoid;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.ReturnType"/>.
        /// </summary>
        public ITypeSymbol ReturnType => Symbol.ReturnType;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.TypeArguments"/>.
        /// </summary>
        public ImmutableArray<ITypeSymbol> TypeArguments => Symbol.TypeArguments;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.TypeParameters"/>.
        /// </summary>
        public ImmutableArray<ITypeParameterSymbol> TypeParameters => Symbol.TypeParameters;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.Parameters"/>.
        /// </summary>
        public ImmutableArray<IParameterSymbol> Parameters => Symbol.Parameters;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.ConstructedFrom"/>.
        /// </summary>
        public IMethodSymbol ConstructedFrom => Symbol.ConstructedFrom;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.OverriddenMethod"/>.
        /// </summary>
        public IMethodSymbol OverriddenMethod => Symbol.OverriddenMethod;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.ReducedFrom"/>.
        /// </summary>
        public IMethodSymbol ReducedFrom => Symbol.ReducedFrom;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.Kind"/>.
        /// </summary>
        public SymbolKind Kind => Symbol.Kind;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.Name"/>.
        /// </summary>
        public string Name => Symbol.Name;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.MetadataName"/>.
        /// </summary>
        public string MetadataName => Symbol.MetadataName;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.ContainingSymbol"/>.
        /// </summary>
        public ISymbol ContainingSymbol => Symbol.ContainingSymbol;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.ContainingType"/>.
        /// </summary>
        public INamedTypeSymbol ContainingType => Symbol.ContainingType;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.ContainingNamespace"/>.
        /// </summary>
        public INamespaceSymbol ContainingNamespace => Symbol.ContainingNamespace;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.IsStatic"/>.
        /// </summary>
        public bool IsStatic => Symbol.IsStatic;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.IsVirtual"/>.
        /// </summary>
        public bool IsVirtual => Symbol.IsVirtual;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.IsOverride"/>.
        /// </summary>
        public bool IsOverride => Symbol.IsOverride;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.IsAbstract"/>.
        /// </summary>
        public bool IsAbstract => Symbol.IsAbstract;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.IsSealed"/>.
        /// </summary>
        public bool IsSealed => Symbol.IsSealed;

        /// <summary>
        /// Gets <see cref="IMethodSymbol.DeclaredAccessibility"/>.
        /// </summary>
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
