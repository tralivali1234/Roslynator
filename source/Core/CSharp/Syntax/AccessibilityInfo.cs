// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// Provides information about a declaration and its accessibility modifiers.
    /// </summary>
    public readonly struct AccessibilityInfo : IEquatable<AccessibilityInfo>
    {
        private AccessibilityInfo(SyntaxNode node, SyntaxTokenList modifiers, int tokenIndex, int secondTokenIndex = -1)
        {
            Node = node;
            Modifiers = modifiers;
            TokenIndex = tokenIndex;
            SecondTokenIndex = secondTokenIndex;
        }

        private static AccessibilityInfo Default { get; } = new AccessibilityInfo();

        /// <summary>
        /// The node that contains the modifiers.
        /// </summary>
        public SyntaxNode Node { get; }

        /// <summary>
        /// Modifiers that may contain accessibility modifers.
        /// </summary>
        public SyntaxTokenList Modifiers { get; }

        /// <summary>
        /// Zero-based index of the first accessibility modifer.
        /// </summary>
        public int TokenIndex { get; }

        /// <summary>
        /// Zero-based index of the second accessibility modifier.
        /// </summary>
        public int SecondTokenIndex { get; }

        /// <summary>
        /// First accessibility modifer.
        /// </summary>
        public SyntaxToken Token
        {
            get { return GetTokenOrDefault(TokenIndex); }
        }

        /// <summary>
        /// Second accessibility modifier.
        /// </summary>
        public SyntaxToken SecondToken
        {
            get { return GetTokenOrDefault(SecondTokenIndex); }
        }

        /// <summary>
        /// Explicit accessibility, i.e. accessibility defined by accessibility modifiers.
        /// </summary>
        public Accessibility ExplicitAccessibility
        {
            get
            {
                if (TokenIndex == -1)
                    return Accessibility.NotApplicable;

                switch (Token.Kind())
                {
                    case SyntaxKind.PublicKeyword:
                        {
                            return Accessibility.Public;
                        }
                    case SyntaxKind.PrivateKeyword:
                        {
                            return (SecondTokenIndex == -1)
                                ? Accessibility.Private
                                : Accessibility.ProtectedAndInternal;
                        }
                    case SyntaxKind.InternalKeyword:
                        {
                            return (SecondTokenIndex == -1)
                                ? Accessibility.Internal
                                : Accessibility.ProtectedOrInternal;
                        }
                    case SyntaxKind.ProtectedKeyword:
                        {
                            if (SecondTokenIndex == -1)
                                return Accessibility.Protected;

                            switch (SecondToken.Kind())
                            {
                                case SyntaxKind.PrivateKeyword:
                                    return Accessibility.ProtectedAndInternal;
                                case SyntaxKind.InternalKeyword:
                                    return Accessibility.ProtectedOrInternal;
                            }

                            break;
                        }
                }

                Debug.Fail(Modifiers.ToString());

                return Accessibility.NotApplicable;
            }
        }

        /// <summary>
        /// Determines whether this struct was initialized with an actual syntax.
        /// </summary>
        public bool Success
        {
            get { return Node != null; }
        }

        internal static AccessibilityInfo Create(SyntaxNode node)
        {
            if (node == null)
                return Default;

            switch (node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    return Create(node, ((ClassDeclarationSyntax)node).Modifiers);
                case SyntaxKind.ConstructorDeclaration:
                    return Create(node, ((ConstructorDeclarationSyntax)node).Modifiers);
                case SyntaxKind.ConversionOperatorDeclaration:
                    return Create(node, ((ConversionOperatorDeclarationSyntax)node).Modifiers);
                case SyntaxKind.DelegateDeclaration:
                    return Create(node, ((DelegateDeclarationSyntax)node).Modifiers);
                case SyntaxKind.DestructorDeclaration:
                    return Create(node, ((DestructorDeclarationSyntax)node).Modifiers);
                case SyntaxKind.EnumDeclaration:
                    return Create(node, ((EnumDeclarationSyntax)node).Modifiers);
                case SyntaxKind.EventDeclaration:
                    return Create(node, ((EventDeclarationSyntax)node).Modifiers);
                case SyntaxKind.EventFieldDeclaration:
                    return Create(node, ((EventFieldDeclarationSyntax)node).Modifiers);
                case SyntaxKind.FieldDeclaration:
                    return Create(node, ((FieldDeclarationSyntax)node).Modifiers);
                case SyntaxKind.IndexerDeclaration:
                    return Create(node, ((IndexerDeclarationSyntax)node).Modifiers);
                case SyntaxKind.InterfaceDeclaration:
                    return Create(node, ((InterfaceDeclarationSyntax)node).Modifiers);
                case SyntaxKind.MethodDeclaration:
                    return Create(node, ((MethodDeclarationSyntax)node).Modifiers);
                case SyntaxKind.OperatorDeclaration:
                    return Create(node, ((OperatorDeclarationSyntax)node).Modifiers);
                case SyntaxKind.PropertyDeclaration:
                    return Create(node, ((PropertyDeclarationSyntax)node).Modifiers);
                case SyntaxKind.StructDeclaration:
                    return Create(node, ((StructDeclarationSyntax)node).Modifiers);
                case SyntaxKind.IncompleteMember:
                    return Create(node, ((IncompleteMemberSyntax)node).Modifiers);
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                case SyntaxKind.UnknownAccessorDeclaration:
                    return Create(node, ((AccessorDeclarationSyntax)node).Modifiers);
            }

            return Default;
        }

        internal static AccessibilityInfo Create(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration == null)
                return Default;

            return Create(classDeclaration, classDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(ConstructorDeclarationSyntax constructorDeclaration)
        {
            if (constructorDeclaration == null)
                return Default;

            return Create(constructorDeclaration, constructorDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            if (conversionOperatorDeclaration == null)
                return Default;

            return Create(conversionOperatorDeclaration, conversionOperatorDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                return Default;

            return Create(delegateDeclaration, delegateDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(DestructorDeclarationSyntax destructorDeclaration)
        {
            if (destructorDeclaration == null)
                return Default;

            return Create(destructorDeclaration, destructorDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(EnumDeclarationSyntax enumDeclaration)
        {
            if (enumDeclaration == null)
                return Default;

            return Create(enumDeclaration, enumDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(EventDeclarationSyntax eventDeclaration)
        {
            if (eventDeclaration == null)
                return Default;

            return Create(eventDeclaration, eventDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            if (eventFieldDeclaration == null)
                return Default;

            return Create(eventFieldDeclaration, eventFieldDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration == null)
                return Default;

            return Create(fieldDeclaration, fieldDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration == null)
                return Default;

            return Create(indexerDeclaration, indexerDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (interfaceDeclaration == null)
                return Default;

            return Create(interfaceDeclaration, interfaceDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                return Default;

            return Create(methodDeclaration, methodDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(OperatorDeclarationSyntax operatorDeclaration)
        {
            if (operatorDeclaration == null)
                return Default;

            return Create(operatorDeclaration, operatorDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration == null)
                return Default;

            return Create(propertyDeclaration, propertyDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(StructDeclarationSyntax structDeclaration)
        {
            if (structDeclaration == null)
                return Default;

            return Create(structDeclaration, structDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(IncompleteMemberSyntax incompleteMember)
        {
            if (incompleteMember == null)
                return Default;

            return Create(incompleteMember, incompleteMember.Modifiers);
        }

        internal static AccessibilityInfo Create(AccessorDeclarationSyntax accessorDeclaration)
        {
            if (accessorDeclaration == null)
                return Default;

            if (!accessorDeclaration.Kind().Is(
                SyntaxKind.GetAccessorDeclaration,
                SyntaxKind.SetAccessorDeclaration,
                SyntaxKind.UnknownAccessorDeclaration))
            {
                return Default;
            }

            return Create(accessorDeclaration, accessorDeclaration.Modifiers);
        }

        internal static AccessibilityInfo Create(SyntaxNode node, SyntaxTokenList modifiers)
        {
            int count = modifiers.Count;

            for (int i = 0; i < count; i++)
            {
                switch (modifiers[i].Kind())
                {
                    case SyntaxKind.PublicKeyword:
                        {
                            return new AccessibilityInfo(node, modifiers, i);
                        }
                    case SyntaxKind.PrivateKeyword:
                    case SyntaxKind.InternalKeyword:
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                if (modifiers[j].IsKind(SyntaxKind.ProtectedKeyword))
                                    return new AccessibilityInfo(node, modifiers, i, j);
                            }

                            return new AccessibilityInfo(node, modifiers, i);
                        }
                    case SyntaxKind.ProtectedKeyword:
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                if (modifiers[j].IsKind(SyntaxKind.InternalKeyword, SyntaxKind.PrivateKeyword))
                                    return new AccessibilityInfo(node, modifiers, i, j);
                            }

                            return new AccessibilityInfo(node, modifiers, i);
                        }
                }
            }

            return new AccessibilityInfo(node, modifiers, -1);
        }

        internal ModifierListInfo ModifierListInfo()
        {
            return new ModifierListInfo(Node, Modifiers);
        }

        /// <summary>
        /// Creates a new <see cref="AccessibilityInfo"/> with modifiers updated.
        /// </summary>
        /// <param name="newModifiers"></param>
        /// <returns></returns>
        public AccessibilityInfo WithModifiers(SyntaxTokenList newModifiers)
        {
            ThrowInvalidOperationIfNotInitialized();

            ModifierListInfo info = ModifierListInfo().WithModifiers(newModifiers);

            return Create(info.Parent, info.Modifiers);
        }

        /// <summary>
        /// Creates a new <see cref="AccessibilityInfo"/> with accessibility modifiers updated.
        /// </summary>
        /// <param name="newAccessibility"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public AccessibilityInfo WithExplicitAccessibility(Accessibility newAccessibility, IModifierComparer comparer = null)
        {
            ThrowInvalidOperationIfNotInitialized();

            Accessibility accessibility = ExplicitAccessibility;

            if (accessibility == newAccessibility)
                return this;

            comparer = comparer ?? ModifierComparer.Instance;

            SyntaxNode declaration = Node;

            if (accessibility.IsSingleTokenAccessibility()
                && newAccessibility.IsSingleTokenAccessibility())
            {
                int insertIndex = comparer.GetInsertIndex(Modifiers, GetTokenKind());

                if (TokenIndex == insertIndex
                    || TokenIndex == insertIndex - 1)
                {
                    SyntaxToken newToken = SyntaxFactory.Token(GetTokenKind()).WithTriviaFrom(Token);

                    SyntaxTokenList newModifiers = Modifiers.Replace(Token, newToken);

                    return WithModifiers(newModifiers);
                }
            }

            if (accessibility != Accessibility.NotApplicable)
            {
                declaration = Modifier.RemoveAt(declaration, Math.Max(TokenIndex, SecondTokenIndex));

                if (SecondTokenIndex != -1)
                    declaration = Modifier.RemoveAt(declaration, Math.Min(TokenIndex, SecondTokenIndex));
            }

            if (newAccessibility != Accessibility.NotApplicable)
                declaration = Modifier.Insert(declaration, newAccessibility, comparer);

            return SyntaxInfo.AccessibilityInfo(declaration);

            SyntaxKind GetTokenKind()
            {
                switch (newAccessibility)
                {
                    case Accessibility.Private:
                        return SyntaxKind.PrivateKeyword;
                    case Accessibility.Protected:
                        return SyntaxKind.ProtectedKeyword;
                    case Accessibility.Internal:
                        return SyntaxKind.InternalKeyword;
                    case Accessibility.Public:
                        return SyntaxKind.PublicKeyword;
                    case Accessibility.NotApplicable:
                        return SyntaxKind.None;
                    default:
                        throw new ArgumentException("", nameof(newAccessibility));
                }
            }
        }

        private SyntaxToken GetTokenOrDefault(int index)
        {
            return (index == -1) ? default(SyntaxToken) : Modifiers[index];
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (Node == null)
                throw new InvalidOperationException($"{nameof(AccessibilityInfo)} is not initalized.");
        }

        /// <summary>
        /// Returns the string representation of the underlying syntax, not including its leading and trailing trivia.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Node?.ToString() ?? "";
        }

        /// <summary>
        /// Determines whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. </param>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            return obj is AccessibilityInfo other
                && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(AccessibilityInfo other)
        {
            return EqualityComparer<SyntaxNode>.Default.Equals(Node, other.Node);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<SyntaxNode>.Default.GetHashCode(Node);
        }

        public static bool operator ==(AccessibilityInfo info1, AccessibilityInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(AccessibilityInfo info1, AccessibilityInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
