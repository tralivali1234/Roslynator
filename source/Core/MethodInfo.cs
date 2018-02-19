// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Roslynator
{
    /// <summary>
    /// 
    /// </summary>
    public readonly struct MethodInfo : IEquatable<MethodInfo>
    {
        internal MethodInfo(IMethodSymbol symbol)
        {
            Debug.Assert(symbol != null);

            Symbol = symbol;
        }

        /// <summary>
        /// 
        /// </summary>
        public IMethodSymbol Symbol { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specialType"></param>
        /// <returns></returns>
        public bool IsReturnType(SpecialType specialType)
        {
            return ReturnType.SpecialType == specialType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specialType"></param>
        /// <returns></returns>
        public bool IsContainingType(SpecialType specialType)
        {
            return ContainingType?.SpecialType == specialType;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnsObject
        {
            get { return IsReturnType(SpecialType.System_Object); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnsBoolean
        {
            get { return IsReturnType(SpecialType.System_Boolean); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnsInt
        {
            get { return IsReturnType(SpecialType.System_Int32); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnsString
        {
            get { return IsReturnType(SpecialType.System_String); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeArgument"></param>
        /// <returns></returns>
        public bool ReturnsIEnumerableOf(ITypeSymbol typeArgument)
        {
            return ReturnType.IsIEnumerableOf(typeArgument);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeArgumentPredicate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        public bool IsPublic
        {
            get { return DeclaredAccessibility == Accessibility.Public; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsInternal
        {
            get { return DeclaredAccessibility == Accessibility.Internal; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsProtected
        {
            get { return DeclaredAccessibility == Accessibility.Protected; }
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public MethodKind MethodKind => Symbol.MethodKind;

        /// <summary>
        /// 
        /// </summary>
        public bool IsGenericMethod => Symbol.IsGenericMethod;

        /// <summary>
        /// 
        /// </summary>
        public bool IsExtensionMethod => Symbol.IsExtensionMethod;

        /// <summary>
        /// 
        /// </summary>
        public bool IsAsync => Symbol.IsAsync;

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnsVoid => Symbol.ReturnsVoid;

        /// <summary>
        /// 
        /// </summary>
        public ITypeSymbol ReturnType => Symbol.ReturnType;

        /// <summary>
        /// 
        /// </summary>
        public ImmutableArray<ITypeSymbol> TypeArguments => Symbol.TypeArguments;

        /// <summary>
        /// 
        /// </summary>
        public ImmutableArray<ITypeParameterSymbol> TypeParameters => Symbol.TypeParameters;

        /// <summary>
        /// 
        /// </summary>
        public ImmutableArray<IParameterSymbol> Parameters => Symbol.Parameters;

        /// <summary>
        /// 
        /// </summary>
        public IMethodSymbol ConstructedFrom => Symbol.ConstructedFrom;

        /// <summary>
        /// 
        /// </summary>
        public IMethodSymbol OverriddenMethod => Symbol.OverriddenMethod;

        /// <summary>
        /// 
        /// </summary>
        public IMethodSymbol ReducedFrom => Symbol.ReducedFrom;

        /// <summary>
        /// 
        /// </summary>
        public SymbolKind Kind => Symbol.Kind;

        /// <summary>
        /// 
        /// </summary>
        public string Name => Symbol.Name;

        /// <summary>
        /// 
        /// </summary>
        public string MetadataName => Symbol.MetadataName;

        /// <summary>
        /// 
        /// </summary>
        public ISymbol ContainingSymbol => Symbol.ContainingSymbol;

        /// <summary>
        /// 
        /// </summary>
        public INamedTypeSymbol ContainingType => Symbol.ContainingType;

        /// <summary>
        /// 
        /// </summary>
        public INamespaceSymbol ContainingNamespace => Symbol.ContainingNamespace;

        /// <summary>
        /// 
        /// </summary>
        public bool IsStatic => Symbol.IsStatic;

        /// <summary>
        /// 
        /// </summary>
        public bool IsVirtual => Symbol.IsVirtual;

        /// <summary>
        /// 
        /// </summary>
        public bool IsOverride => Symbol.IsOverride;

        /// <summary>
        /// 
        /// </summary>
        public bool IsAbstract => Symbol.IsAbstract;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSealed => Symbol.IsSealed;

        /// <summary>
        /// 
        /// </summary>
        public Accessibility DeclaredAccessibility => Symbol.DeclaredAccessibility;
        #endregion IMethodSymbol

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return obj is MethodInfo other && Equals(other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(MethodInfo other)
        {
            return EqualityComparer<IMethodSymbol>.Default.Equals(Symbol, other.Symbol);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return Symbol?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator ==(MethodInfo info1, MethodInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(MethodInfo info1, MethodInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
