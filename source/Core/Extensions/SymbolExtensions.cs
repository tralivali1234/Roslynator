// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Roslynator
{
    public static class SymbolExtensions
    {
        #region ISymbol
        internal static ISymbol FindFirstImplementedInterfaceMember(this ISymbol symbol, bool allInterfaces = false)
        {
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));

            INamedTypeSymbol containingType = symbol.ContainingType;

            if (containingType != null)
            {
                ImmutableArray<INamedTypeSymbol> interfaces = containingType.GetInterfaces(allInterfaces);

                for (int i = 0; i < interfaces.Length; i++)
                {
                    ImmutableArray<ISymbol> members = interfaces[i].GetMembers();

                    for (int j = 0; j < members.Length; j++)
                    {
                        if (symbol.Equals(containingType.FindImplementationForInterfaceMember(members[j])))
                            return members[j];
                    }
                }
            }

            return default(ISymbol);
        }

        public static bool ImplementsInterfaceMember(this ISymbol symbol, bool allInterfaces = false)
        {
            return FindFirstImplementedInterfaceMember(symbol, allInterfaces) != null;
        }

        internal static TSymbol FindFirstImplementedInterfaceMember<TSymbol>(this ISymbol symbol, bool allInterfaces = false) where TSymbol : ISymbol
        {
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));

            INamedTypeSymbol containingType = symbol.ContainingType;

            if (containingType != null)
            {
                ImmutableArray<INamedTypeSymbol> interfaces = containingType.GetInterfaces(allInterfaces);

                for (int i = 0; i < interfaces.Length; i++)
                {
                    ImmutableArray<ISymbol> members = interfaces[i].GetMembers();

                    for (int j = 0; j < members.Length; j++)
                    {
                        if ((members[j] is TSymbol tmember)
                            && symbol.Equals(containingType.FindImplementationForInterfaceMember(tmember)))
                        {
                            return tmember;
                        }
                    }
                }
            }

            return default(TSymbol);
        }

        public static bool ImplementsInterfaceMember<TSymbol>(this ISymbol symbol, bool allInterfaces = false) where TSymbol : ISymbol
        {
            return !EqualityComparer<TSymbol>.Default.Equals(
                FindFirstImplementedInterfaceMember<TSymbol>(symbol, allInterfaces),
                default(TSymbol));
        }

        internal static bool IsAnyInterfaceMemberExplicitlyImplemented(this INamedTypeSymbol symbol, ISymbol interfaceSymbol)
        {
            foreach (ISymbol member in symbol.GetMembers())
            {
                switch (member.Kind)
                {
                    case SymbolKind.Event:
                        {
                            foreach (IEventSymbol eventSymbol in ((IEventSymbol)member).ExplicitInterfaceImplementations)
                            {
                                if (eventSymbol.ContainingType?.Equals(interfaceSymbol) == true)
                                    return true;
                            }

                            break;
                        }
                    case SymbolKind.Method:
                        {
                            foreach (IMethodSymbol methodSymbol in ((IMethodSymbol)member).ExplicitInterfaceImplementations)
                            {
                                if (methodSymbol.ContainingType?.Equals(interfaceSymbol) == true)
                                    return true;
                            }

                            break;
                        }
                    case SymbolKind.Property:
                        {
                            foreach (IPropertySymbol propertySymbol in ((IPropertySymbol)member).ExplicitInterfaceImplementations)
                            {
                                if (propertySymbol.ContainingType?.Equals(interfaceSymbol) == true)
                                    return true;
                            }

                            break;
                        }
                }
            }

            return false;
        }

        public static bool IsKind(this ISymbol symbol, SymbolKind kind)
        {
            return symbol?.Kind == kind;
        }

        public static bool IsKind(this ISymbol symbol, SymbolKind kind1, SymbolKind kind2)
        {
            if (symbol == null)
                return false;

            SymbolKind kind = symbol.Kind;

            return kind == kind1
                || kind == kind2;
        }

        public static bool IsKind(this ISymbol symbol, SymbolKind kind1, SymbolKind kind2, SymbolKind kind3)
        {
            if (symbol == null)
                return false;

            SymbolKind kind = symbol.Kind;

            return kind == kind1
                || kind == kind2
                || kind == kind3;
        }

        public static bool IsKind(this ISymbol symbol, SymbolKind kind1, SymbolKind kind2, SymbolKind kind3, SymbolKind kind4)
        {
            if (symbol == null)
                return false;

            SymbolKind kind = symbol.Kind;

            return kind == kind1
                || kind == kind2
                || kind == kind3
                || kind == kind4;
        }

        public static bool IsKind(this ISymbol symbol, SymbolKind kind1, SymbolKind kind2, SymbolKind kind3, SymbolKind kind4, SymbolKind kind5)
        {
            if (symbol == null)
                return false;

            SymbolKind kind = symbol.Kind;

            return kind == kind1
                || kind == kind2
                || kind == kind3
                || kind == kind4
                || kind == kind5;
        }

        public static bool IsPublic(this ISymbol symbol)
        {
            return symbol?.DeclaredAccessibility == Accessibility.Public;
        }

        public static bool IsInternal(this ISymbol symbol)
        {
            return symbol?.DeclaredAccessibility == Accessibility.Internal;
        }

        public static bool IsProtected(this ISymbol symbol)
        {
            return symbol?.DeclaredAccessibility == Accessibility.Protected;
        }

        public static bool IsPrivate(this ISymbol symbol)
        {
            return symbol?.DeclaredAccessibility == Accessibility.Private;
        }

        public static bool IsArrayType(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.ArrayType;
        }

        public static bool IsDynamicType(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.DynamicType;
        }

        public static bool IsErrorType(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.ErrorType;
        }

        public static bool IsEvent(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.Event;
        }

        public static bool IsField(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.Field;
        }

        public static bool IsLocal(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.Local;
        }

        public static bool IsMethod(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.Method;
        }

        public static bool IsNamedType(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.NamedType;
        }

        public static bool IsNamespace(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.Namespace;
        }

        public static bool IsParameter(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.Parameter;
        }

        public static bool IsProperty(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.Property;
        }

        public static bool IsTypeParameter(this ISymbol symbol)
        {
            return symbol?.Kind == SymbolKind.TypeParameter;
        }

        public static bool IsAsyncMethod(this ISymbol symbol)
        {
            return IsMethod(symbol)
                && ((IMethodSymbol)symbol).IsAsync;
        }

        internal static bool IsPropertyOfAnonymousType(this ISymbol symbol)
        {
            return symbol.IsProperty()
                && symbol.ContainingType.IsAnonymousType;
        }

        internal static SyntaxNode GetSyntax(this ISymbol symbol, CancellationToken cancellationToken = default(CancellationToken))
        {
            return symbol
                .DeclaringSyntaxReferences[0]
                .GetSyntax(cancellationToken);
        }

        internal static Task<SyntaxNode> GetSyntaxAsync(this ISymbol symbol, CancellationToken cancellationToken = default(CancellationToken))
        {
            return symbol
                .DeclaringSyntaxReferences[0]
                .GetSyntaxAsync(cancellationToken);
        }

        internal static SyntaxNode GetSyntaxOrDefault(this ISymbol symbol, CancellationToken cancellationToken = default(CancellationToken))
        {
            return symbol
                .DeclaringSyntaxReferences
                .FirstOrDefault()?
                .GetSyntax(cancellationToken);
        }

        internal static Task<SyntaxNode> GetSyntaxOrDefaultAsync(this ISymbol symbol, CancellationToken cancellationToken = default(CancellationToken))
        {
            return symbol
                .DeclaringSyntaxReferences
                .FirstOrDefault()?
                .GetSyntaxAsync(cancellationToken);
        }

        internal static bool TryGetSyntax<TNode>(this ISymbol symbol, out TNode node, CancellationToken cancellationToken = default(CancellationToken)) where TNode : SyntaxNode
        {
            ImmutableArray<SyntaxReference> syntaxReferences = symbol.DeclaringSyntaxReferences;

            if (syntaxReferences.Any())
            {
                node = syntaxReferences[0].GetSyntax(cancellationToken) as TNode;

                if (node != null)
                    return true;
            }

            node = null;
            return false;
        }

        public static bool HasAttribute(this ISymbol symbol, INamedTypeSymbol attributeSymbol)
        {
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));

            if (attributeSymbol != null)
            {
                ImmutableArray<AttributeData> attributes = symbol.GetAttributes();

                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i].AttributeClass.Equals(attributeSymbol))
                        return true;
                }
            }

            return false;
        }

        internal static AttributeData GetAttributeByMetadataName(this INamedTypeSymbol typeSymbol, string fullyQualifiedMetadataName, Compilation compilation)
        {
            ImmutableArray<AttributeData> attributes = typeSymbol.GetAttributes();

            if (attributes.Any())
            {
                INamedTypeSymbol attributeType = compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);

                if (attributeType != null)
                {
                    foreach (AttributeData attributeData in attributes)
                    {
                        if (attributeData.AttributeClass.Equals(attributeType))
                            return attributeData;
                    }
                }
            }

            return null;
        }

        internal static ImmutableArray<IParameterSymbol> ParametersOrDefault(this ISymbol symbol)
        {
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));

            switch (symbol.Kind)
            {
                case SymbolKind.Method:
                    return ((IMethodSymbol)symbol).Parameters;
                case SymbolKind.Property:
                    return ((IPropertySymbol)symbol).Parameters;
                default:
                    return default(ImmutableArray<IParameterSymbol>);
            }
        }

        internal static ISymbol BaseOverriddenSymbol(this ISymbol symbol)
        {
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));

            switch (symbol.Kind)
            {
                case SymbolKind.Method:
                    return ((IMethodSymbol)symbol).BaseOverriddenMethod();
                case SymbolKind.Property:
                    return ((IPropertySymbol)symbol).BaseOverriddenProperty();
                case SymbolKind.Event:
                    return ((IEventSymbol)symbol).BaseOverriddenEvent();
            }

            return null;
        }
        #endregion ISymbol

        #region IEventSymbol
        internal static IEventSymbol BaseOverriddenEvent(this IEventSymbol eventSymbol)
        {
            if (eventSymbol == null)
                throw new ArgumentNullException(nameof(eventSymbol));

            while (true)
            {
                IEventSymbol overriddenEvent = eventSymbol.OverriddenEvent;

                if (overriddenEvent == null)
                    return eventSymbol;

                eventSymbol = overriddenEvent;
            }
        }

        internal static IEnumerable<IEventSymbol> OverriddenEvents(this IEventSymbol eventSymbol)
        {
            if (eventSymbol == null)
                throw new ArgumentNullException(nameof(eventSymbol));

            IEventSymbol overriddenEvent = eventSymbol.OverriddenEvent;

            while (overriddenEvent != null)
            {
                yield return overriddenEvent;

                overriddenEvent = overriddenEvent.OverriddenEvent;
            }
        }
        #endregion IEventSymbol

        #region IFieldSymbol
        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, bool value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is bool value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, char value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is char value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, sbyte value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is sbyte value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, byte value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is byte value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, short value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is short value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, ushort value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is ushort value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, int value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is int value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, uint value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is uint value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, long value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is long value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, ulong value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is ulong value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, decimal value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is decimal value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, float value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is float value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, double value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is double value2
                    && value == value2;
            }

            return false;
        }

        public static bool HasConstantValue(this IFieldSymbol fieldSymbol, string value)
        {
            if (fieldSymbol == null)
                throw new ArgumentNullException(nameof(fieldSymbol));

            if (fieldSymbol.HasConstantValue)
            {
                object constantValue = fieldSymbol.ConstantValue;

                return constantValue is string value2
                    && value == value2;
            }

            return false;
        }
        #endregion IFieldSymbol

        #region IMethodSymbol
        internal static IMethodSymbol BaseOverriddenMethod(this IMethodSymbol methodSymbol)
        {
            if (methodSymbol == null)
                throw new ArgumentNullException(nameof(methodSymbol));

            while (true)
            {
                IMethodSymbol overriddenMethod = methodSymbol.OverriddenMethod;

                if (overriddenMethod == null)
                    return methodSymbol;

                methodSymbol = overriddenMethod;
            }
        }

        internal static IEnumerable<IMethodSymbol> OverriddenMethods(this IMethodSymbol methodSymbol)
        {
            if (methodSymbol == null)
                throw new ArgumentNullException(nameof(methodSymbol));

            IMethodSymbol overriddenMethod = methodSymbol.OverriddenMethod;

            while (overriddenMethod != null)
            {
                yield return overriddenMethod;

                overriddenMethod = overriddenMethod.OverriddenMethod;
            }
        }

        public static IMethodSymbol ReducedFromOrSelf(this IMethodSymbol methodSymbol)
        {
            if (methodSymbol == null)
                throw new ArgumentNullException(nameof(methodSymbol));

            return methodSymbol.ReducedFrom ?? methodSymbol;
        }

        public static bool IsReducedExtensionMethod(this IMethodSymbol methodSymbol)
        {
            if (methodSymbol == null)
                throw new ArgumentNullException(nameof(methodSymbol));

            return methodSymbol.MethodKind == MethodKind.ReducedExtension;
        }

        public static bool IsNonReducedExtensionMethod(this IMethodSymbol methodSymbol)
        {
            if (methodSymbol == null)
                throw new ArgumentNullException(nameof(methodSymbol));

            return methodSymbol.IsExtensionMethod
                && methodSymbol.MethodKind != MethodKind.ReducedExtension;
        }
        #endregion IMethodSymbol

        #region IParameterSymbol
        public static bool IsParamsOf(this IParameterSymbol parameterSymbol, SpecialType elementType)
        {
            if (parameterSymbol == null)
                throw new ArgumentNullException(nameof(parameterSymbol));

            return parameterSymbol.IsParams
                && (parameterSymbol.Type as IArrayTypeSymbol)?.ElementType.SpecialType == elementType;
        }

        public static bool IsParamsOf(
            this IParameterSymbol parameterSymbol,
            SpecialType elementType1,
            SpecialType elementType2)
        {
            if (parameterSymbol == null)
                throw new ArgumentNullException(nameof(parameterSymbol));

            return parameterSymbol.IsParams
                && (parameterSymbol.Type as IArrayTypeSymbol)?
                    .ElementType
                    .SpecialType
                    .Is(elementType1, elementType2) == true;
        }

        public static bool IsParamsOf(
            this IParameterSymbol parameterSymbol,
            SpecialType elementType1,
            SpecialType elementType2,
            SpecialType elementType3)
        {
            if (parameterSymbol == null)
                throw new ArgumentNullException(nameof(parameterSymbol));

            return parameterSymbol.IsParams
                && (parameterSymbol.Type as IArrayTypeSymbol)?
                    .ElementType
                    .SpecialType
                    .Is(elementType1, elementType2, elementType3) == true;
        }

        public static bool IsRefOrOut(this IParameterSymbol parameterSymbol)
        {
            if (parameterSymbol == null)
                throw new ArgumentNullException(nameof(parameterSymbol));

            RefKind refKind = parameterSymbol.RefKind;

            return refKind == RefKind.Ref
                || refKind == RefKind.Out;
        }
        #endregion IParameterSymbol

        #region IPropertySymbol
        internal static IPropertySymbol BaseOverriddenProperty(this IPropertySymbol propertySymbol)
        {
            if (propertySymbol == null)
                throw new ArgumentNullException(nameof(propertySymbol));

            while (true)
            {
                IPropertySymbol overriddenProperty = propertySymbol.OverriddenProperty;

                if (overriddenProperty == null)
                    return propertySymbol;

                propertySymbol = overriddenProperty;
            }
        }

        internal static IEnumerable<IPropertySymbol> OverriddenProperties(this IPropertySymbol propertySymbol)
        {
            if (propertySymbol == null)
                throw new ArgumentNullException(nameof(propertySymbol));

            IPropertySymbol overriddenProperty = propertySymbol.OverriddenProperty;

            while (overriddenProperty != null)
            {
                yield return overriddenProperty;

                overriddenProperty = overriddenProperty.OverriddenProperty;
            }
        }
        #endregion IPropertySymbol

        #region INamedTypeSymbol
        public static bool IsNullableOf(this INamedTypeSymbol namedTypeSymbol, SpecialType specialType)
        {
            if (namedTypeSymbol == null)
                throw new ArgumentNullException(nameof(namedTypeSymbol));

            return namedTypeSymbol.IsConstructedFrom(SpecialType.System_Nullable_T)
                && namedTypeSymbol.TypeArguments[0].SpecialType == specialType;
        }

        public static bool IsNullableOf(this INamedTypeSymbol namedTypeSymbol, ITypeSymbol typeArgument)
        {
            if (namedTypeSymbol == null)
                throw new ArgumentNullException(nameof(namedTypeSymbol));

            return namedTypeSymbol.IsConstructedFrom(SpecialType.System_Nullable_T)
                && namedTypeSymbol.TypeArguments[0] == typeArgument;
        }

        public static bool IsConstructedFrom(this INamedTypeSymbol namedTypeSymbol, SpecialType specialType)
        {
            if (namedTypeSymbol == null)
                throw new ArgumentNullException(nameof(namedTypeSymbol));

            return namedTypeSymbol.ConstructedFrom.SpecialType == specialType;
        }

        public static bool IsConstructedFrom(this INamedTypeSymbol namedTypeSymbol, ISymbol symbol)
        {
            if (namedTypeSymbol == null)
                throw new ArgumentNullException(nameof(namedTypeSymbol));

            return namedTypeSymbol.ConstructedFrom.Equals(symbol);
        }

        public static bool IsIEnumerableOf(this INamedTypeSymbol namedTypeSymbol, ITypeSymbol typeArgument)
        {
            if (namedTypeSymbol == null)
                throw new ArgumentNullException(nameof(namedTypeSymbol));

            if (typeArgument == null)
                throw new ArgumentNullException(nameof(typeArgument));

            return IsConstructedFrom(namedTypeSymbol, SpecialType.System_Collections_Generic_IEnumerable_T)
                && namedTypeSymbol.TypeArguments[0].Equals(typeArgument);
        }

        public static bool IsIEnumerableOf(this INamedTypeSymbol namedTypeSymbol, SpecialType specialType)
        {
            if (namedTypeSymbol == null)
                throw new ArgumentNullException(nameof(namedTypeSymbol));

            return IsConstructedFrom(namedTypeSymbol, SpecialType.System_Collections_Generic_IEnumerable_T)
                && namedTypeSymbol.TypeArguments[0].SpecialType == specialType;
        }

        public static bool IsConstructedFromIEnumerableOfT(this INamedTypeSymbol namedTypeSymbol)
        {
            return IsConstructedFrom(namedTypeSymbol, SpecialType.System_Collections_Generic_IEnumerable_T);
        }

        public static bool IsIEnumerableOrConstructedFromIEnumerableOfT(this INamedTypeSymbol namedTypeSymbol)
        {
            return IsIEnumerable(namedTypeSymbol)
                || IsConstructedFromIEnumerableOfT(namedTypeSymbol);
        }

        internal static bool IsConstructedFromTaskOfT(this INamedTypeSymbol namedTypeSymbol, SemanticModel semanticModel)
        {
            return namedTypeSymbol
                .ConstructedFrom
                .EqualsOrInheritsFrom(semanticModel.GetTypeByMetadataName(MetadataNames.System_Threading_Tasks_Task_T));
        }
        #endregion INamedTypeSymbol

        #region INamespaceSymbol
        internal static IEnumerable<INamespaceSymbol> ContainingNamespacesAndSelf(this INamespaceSymbol @namespace)
        {
            if (@namespace == null)
                throw new ArgumentNullException(nameof(@namespace));

            do
            {
                yield return @namespace;

                @namespace = @namespace.ContainingNamespace;

            } while (@namespace != null);
        }
        #endregion INamespaceSymbol

        #region ITypeSymbol
        public static bool IsNullableOf(this ITypeSymbol typeSymbol, SpecialType specialType)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            return typeSymbol.Kind == SymbolKind.NamedType
                && IsNullableOf((INamedTypeSymbol)typeSymbol, specialType);
        }

        public static bool IsNullableOf(this ITypeSymbol typeSymbol, ITypeSymbol typeArgument)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            return typeSymbol.Kind == SymbolKind.NamedType
                && IsNullableOf((INamedTypeSymbol)typeSymbol, typeArgument);
        }

        public static bool IsVoid(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.SpecialType == SpecialType.System_Void;
        }

        //XTODO: IsInt32
        public static bool IsInt(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.SpecialType == SpecialType.System_Int32;
        }

        public static bool IsBoolean(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.SpecialType == SpecialType.System_Boolean;
        }

        public static bool IsString(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.SpecialType == SpecialType.System_String;
        }

        public static bool IsObject(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.SpecialType == SpecialType.System_Object;
        }

        public static bool IsChar(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.SpecialType == SpecialType.System_Char;
        }

        public static IEnumerable<INamedTypeSymbol> BaseTypes(this ITypeSymbol type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            INamedTypeSymbol baseType = type.BaseType;

            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.BaseType;
            }
        }

        public static IEnumerable<ITypeSymbol> BaseTypesAndSelf(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            ITypeSymbol current = typeSymbol;

            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }

        public static bool Implements(this ITypeSymbol typeSymbol, SpecialType specialType, bool allInterfaces = false)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            ImmutableArray<INamedTypeSymbol> interfaces = typeSymbol.GetInterfaces(allInterfaces);

            for (int i = 0; i < interfaces.Length; i++)
            {
                if (interfaces[i].ConstructedFrom.SpecialType == specialType)
                    return true;
            }

            return false;
        }

        public static bool ImplementsAny(this ITypeSymbol typeSymbol, SpecialType specialType1, SpecialType specialType2, bool allInterfaces = false)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            ImmutableArray<INamedTypeSymbol> interfaces = typeSymbol.GetInterfaces(allInterfaces);

            for (int i = 0; i < interfaces.Length; i++)
            {
                if (interfaces[i].ConstructedFrom.SpecialType.Is(specialType1, specialType2))
                    return true;
            }

            return false;
        }

        public static bool ImplementsAny(this ITypeSymbol typeSymbol, SpecialType specialType1, SpecialType specialType2, SpecialType specialType3, bool allInterfaces = false)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            ImmutableArray<INamedTypeSymbol> interfaces = typeSymbol.GetInterfaces(allInterfaces);

            for (int i = 0; i < interfaces.Length; i++)
            {
                if (interfaces[i].ConstructedFrom.SpecialType.Is(specialType1, specialType2, specialType3))
                    return true;
            }

            return false;
        }

        public static bool Implements(this ITypeSymbol typeSymbol, ITypeSymbol interfaceSymbol, bool allInterfaces = false)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (interfaceSymbol != null)
            {
                ImmutableArray<INamedTypeSymbol> interfaces = typeSymbol.GetInterfaces(allInterfaces);

                for (int i = 0; i < interfaces.Length; i++)
                {
                    if (interfaces[i].Equals(interfaceSymbol))
                        return true;
                }
            }

            return false;
        }

        public static bool IsClass(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.TypeKind == TypeKind.Class;
        }

        public static bool IsInterface(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.TypeKind == TypeKind.Interface;
        }

        public static bool IsStruct(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.TypeKind == TypeKind.Struct;
        }

        public static bool IsEnum(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.TypeKind == TypeKind.Enum;
        }

        public static bool IsDelegate(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.TypeKind == TypeKind.Delegate;
        }

        internal static bool IsEnumWithFlags(this ITypeSymbol typeSymbol, SemanticModel semanticModel)
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            return IsEnumWithFlags(typeSymbol, semanticModel.Compilation);
        }

        internal static bool IsEnumWithFlags(this ITypeSymbol typeSymbol, Compilation compilation)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (compilation == null)
                throw new ArgumentNullException(nameof(compilation));

            return typeSymbol.IsEnum()
                && typeSymbol.HasAttribute(compilation.GetTypeByMetadataName(MetadataNames.System_FlagsAttribute));
        }

        public static bool SupportsExplicitDeclaration(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (typeSymbol.IsAnonymousType)
                return false;

            switch (typeSymbol.Kind)
            {
                case SymbolKind.TypeParameter:
                case SymbolKind.DynamicType:
                    {
                        return true;
                    }
                case SymbolKind.ArrayType:
                    {
                        return SupportsExplicitDeclaration(((IArrayTypeSymbol)typeSymbol).ElementType);
                    }
                case SymbolKind.NamedType:
                    {
                        var namedTypeSymbol = (INamedTypeSymbol)typeSymbol;

                        if (typeSymbol.IsTupleType)
                        {
                            foreach (IFieldSymbol tupleElement in namedTypeSymbol.TupleElements)
                            {
                                if (!SupportsExplicitDeclaration(tupleElement.Type))
                                    return false;
                            }

                            return true;
                        }

                        return !ContainsAnonymousType(namedTypeSymbol.TypeArguments);
                    }
            }

            return false;

            bool ContainsAnonymousType(ImmutableArray<ITypeSymbol> typeSymbols)
            {
                foreach (ITypeSymbol symbol in typeSymbols)
                {
                    if (symbol.IsAnonymousType)
                        return true;

                    if (symbol.Kind == SymbolKind.NamedType)
                    {
                        var namedTypeSymbol = (INamedTypeSymbol)symbol;

                        if (ContainsAnonymousType(namedTypeSymbol.TypeArguments))
                            return true;
                    }
                }

                return false;
            }
        }

        public static bool IsConstructedFrom(this ITypeSymbol typeSymbol, SpecialType specialType)
        {
            return (typeSymbol as INamedTypeSymbol)?.ConstructedFrom.SpecialType == specialType;
        }

        public static bool IsConstructedFrom(this ITypeSymbol typeSymbol, SpecialType specialType1, SpecialType specialType2)
        {
            return (typeSymbol as INamedTypeSymbol)?.ConstructedFrom.SpecialType.Is(specialType1, specialType2) == true;
        }

        public static bool IsConstructedFrom(this ITypeSymbol typeSymbol, SpecialType specialType1, SpecialType specialType2, SpecialType specialType3)
        {
            return (typeSymbol as INamedTypeSymbol)?.ConstructedFrom.SpecialType.Is(specialType1, specialType2, specialType3) == true;
        }

        public static bool IsConstructedFrom(this ITypeSymbol typeSymbol, ISymbol symbol)
        {
            return typeSymbol?.IsNamedType() == true
                && ((INamedTypeSymbol)typeSymbol).ConstructedFrom?.Equals(symbol) == true;
        }

        internal static bool IsEventHandlerOrConstructedFromEventHandlerOfT(
            this ITypeSymbol typeSymbol,
            SemanticModel semanticModel)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            return typeSymbol.Equals(semanticModel.GetTypeByMetadataName(MetadataNames.System_EventHandler))
                || typeSymbol.IsConstructedFrom(semanticModel.GetTypeByMetadataName(MetadataNames.System_EventHandler_T));
        }

        public static bool InheritsFrom(this ITypeSymbol type, ITypeSymbol baseClass, bool includeInterfaces = false)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            INamedTypeSymbol baseType = type.BaseType;

            while (baseType != null)
            {
                if (baseType.Equals(baseClass))
                    return true;

                baseType = baseType.BaseType;
            }

            if (includeInterfaces)
            {
                foreach (INamedTypeSymbol interfaceType in type.AllInterfaces)
                {
                    if (interfaceType.Equals(baseClass))
                        return true;
                }
            }

            return false;
        }

        public static bool EqualsOrInheritsFrom(this ITypeSymbol type, ITypeSymbol baseType, bool includeInterfaces = false)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.Equals(baseType)
                || InheritsFrom(type, baseType, includeInterfaces);
        }

        public static ISymbol FindMember(this ITypeSymbol typeSymbol, string name, Func<ISymbol, bool> predicate = null)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            ImmutableArray<ISymbol> members = typeSymbol.GetMembers(name);

            if (predicate != null)
            {
                return members.FirstOrDefault(predicate);
            }
            else
            {
                return members.FirstOrDefault();
            }
        }

        public static ISymbol FindMember(this ITypeSymbol typeSymbol, Func<ISymbol, bool> predicate)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return typeSymbol.GetMembers().FirstOrDefault(predicate);
        }

        public static TSymbol FindMember<TSymbol>(this ITypeSymbol typeSymbol, Func<TSymbol, bool> predicate = null) where TSymbol : ISymbol
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            return FindMemberImpl(typeSymbol.GetMembers(), predicate);
        }

        public static TSymbol FindMember<TSymbol>(this ITypeSymbol typeSymbol, string name, Func<TSymbol, bool> predicate = null) where TSymbol : ISymbol
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            return FindMemberImpl(typeSymbol.GetMembers(name), predicate);
        }

        private static TSymbol FindMemberImpl<TSymbol>(ImmutableArray<ISymbol> members, Func<TSymbol, bool> predicate) where TSymbol : ISymbol
        {
            if (predicate != null)
            {
                foreach (ISymbol symbol in members)
                {
                    if (symbol is TSymbol tsymbol
                        && predicate(tsymbol))
                    {
                        return tsymbol;
                    }
                }
            }
            else
            {
                foreach (ISymbol symbol in members)
                {
                    if (symbol is TSymbol tsymbol)
                        return tsymbol;
                }
            }

            return default(TSymbol);
        }

        //TODO: HasMember
        public static bool ExistsMember(this ITypeSymbol typeSymbol, string name, Func<ISymbol, bool> predicate = null)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            ImmutableArray<ISymbol> members = typeSymbol.GetMembers(name);

            if (predicate != null)
            {
                return members.Any(predicate);
            }
            else
            {
                return members.Any();
            }
        }

        public static bool ExistsMember(this ITypeSymbol typeSymbol, Func<ISymbol, bool> predicate)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return typeSymbol.GetMembers().Any(predicate);
        }

        public static bool ExistsMember<TSymbol>(this ITypeSymbol typeSymbol, Func<TSymbol, bool> predicate = null) where TSymbol : ISymbol
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            return ExistsMemberImpl(typeSymbol.GetMembers(), predicate);
        }

        public static bool ExistsMember<TSymbol>(this ITypeSymbol typeSymbol, string name, Func<TSymbol, bool> predicate = null) where TSymbol : ISymbol
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            return ExistsMemberImpl(typeSymbol.GetMembers(name), predicate);
        }

        private static bool ExistsMemberImpl<TSymbol>(ImmutableArray<ISymbol> members, Func<TSymbol, bool> predicate) where TSymbol : ISymbol
        {
            if (predicate != null)
            {
                foreach (ISymbol symbol in members)
                {
                    if (symbol is TSymbol tsymbol
                        && predicate(tsymbol))
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (ISymbol symbol in members)
                {
                    if (symbol is TSymbol tsymbol)
                        return true;
                }
            }

            return false;
        }

        internal static IFieldSymbol FindFieldWithConstantValue(this ITypeSymbol typeSymbol, object value)
        {
            foreach (ISymbol symbol in typeSymbol.GetMembers())
            {
                if (symbol.Kind == SymbolKind.Field)
                {
                    var fieldSymbol = (IFieldSymbol)symbol;

                    if (fieldSymbol.HasConstantValue
                        && object.Equals(fieldSymbol.ConstantValue, value))
                    {
                        return fieldSymbol;
                    }
                }
            }

            return null;
        }

        internal static bool IsTaskOrInheritsFromTask(this ITypeSymbol typeSymbol, SemanticModel semanticModel)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            INamedTypeSymbol taskSymbol = semanticModel.GetTypeByMetadataName(MetadataNames.System_Threading_Tasks_Task);

            return typeSymbol.EqualsOrInheritsFrom(taskSymbol);
        }

        internal static bool IsConstructedFromTaskOfT(this ITypeSymbol typeSymbol, SemanticModel semanticModel)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (typeSymbol.Kind == SymbolKind.NamedType)
            {
                INamedTypeSymbol taskOfT = semanticModel.GetTypeByMetadataName(MetadataNames.System_Threading_Tasks_Task_T);

                return ((INamedTypeSymbol)typeSymbol).ConstructedFrom.EqualsOrInheritsFrom(taskOfT);
            }

            return false;
        }

        internal static bool IsConstructedFromImmutableArrayOfT(this ITypeSymbol typeSymbol, SemanticModel semanticModel)
        {
            if (typeSymbol.Kind == SymbolKind.NamedType)
            {
                INamedTypeSymbol symbol = semanticModel.GetTypeByMetadataName(MetadataNames.System_Collections_Immutable_ImmutableArray_T);

                return symbol != null
                    && ((INamedTypeSymbol)typeSymbol).ConstructedFrom.Equals(symbol);
            }

            return false;
        }

        public static bool IsIEnumerable(this ITypeSymbol typeSymbol)
        {
            return typeSymbol?.SpecialType == SpecialType.System_Collections_IEnumerable;
        }

        public static bool IsIEnumerableOf(this ITypeSymbol typeSymbol, SpecialType specialType)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (typeSymbol.Kind == SymbolKind.NamedType)
            {
                var namedTypeSymbol = (INamedTypeSymbol)typeSymbol;

                return IsConstructedFrom(namedTypeSymbol, SpecialType.System_Collections_Generic_IEnumerable_T)
                    && namedTypeSymbol.TypeArguments[0].Equals(specialType);
            }

            return false;
        }

        public static bool IsIEnumerableOf(this ITypeSymbol typeSymbol, ITypeSymbol typeArgument)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (typeArgument == null)
                throw new ArgumentNullException(nameof(typeArgument));

            if (typeSymbol.Kind == SymbolKind.NamedType)
            {
                var namedTypeSymbol = (INamedTypeSymbol)typeSymbol;

                return IsConstructedFrom(namedTypeSymbol, SpecialType.System_Collections_Generic_IEnumerable_T)
                    && namedTypeSymbol.TypeArguments[0].Equals(typeArgument);
            }

            return false;
        }

        internal static bool IsIEnumerableOf(this ITypeSymbol typeSymbol, Func<ITypeSymbol, bool> predicate)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (typeSymbol.IsNamedType())
            {
                var namedTypeSymbol = (INamedTypeSymbol)typeSymbol;

                return namedTypeSymbol.ConstructedFrom.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T
                    && predicate(namedTypeSymbol.TypeArguments[0]);
            }

            return false;
        }

        public static bool IsConstructedFromIEnumerableOfT(this ITypeSymbol typeSymbol)
        {
            return IsConstructedFrom(typeSymbol, SpecialType.System_Collections_Generic_IEnumerable_T);
        }

        public static bool IsIEnumerableOrConstructedFromIEnumerableOfT(this ITypeSymbol typeSymbol)
        {
            return IsIEnumerable(typeSymbol)
                || IsConstructedFromIEnumerableOfT(typeSymbol);
        }

        public static bool IsReferenceOrNullableType(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            return typeSymbol.IsReferenceType
                || IsConstructedFrom(typeSymbol, SpecialType.System_Nullable_T);
        }

        private static ImmutableArray<INamedTypeSymbol> GetInterfaces(this ITypeSymbol typeSymbol, bool allInterfaces)
        {
            return (allInterfaces) ? typeSymbol.AllInterfaces : typeSymbol.Interfaces;
        }
        #endregion ITypeSymbol
    }
}
