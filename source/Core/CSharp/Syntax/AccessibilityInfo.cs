// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    //XTODO: AccessModifiersInfo, ExplicitAccessibilityInfo
    public struct AccessibilityInfo : IEquatable<AccessibilityInfo>
    {
        private AccessibilityInfo(SyntaxNode node, SyntaxTokenList modifiers, int tokenIndex, int secondTokenIndex = -1)
        {
            Node = node;
            Modifiers = modifiers;
            TokenIndex = tokenIndex;
            SecondTokenIndex = secondTokenIndex;
        }

        private static AccessibilityInfo Default { get; } = new AccessibilityInfo();

        public SyntaxNode Node { get; }

        public SyntaxTokenList Modifiers { get; }

        public int TokenIndex { get; }

        public int SecondTokenIndex { get; }

        public SyntaxToken Token
        {
            get { return GetTokenOrDefault(TokenIndex); }
        }

        public SyntaxToken SecondToken
        {
            get { return GetTokenOrDefault(SecondTokenIndex); }
        }

        //TODO: ExplicitAccessibility
        public Accessibility Accessibility
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
                case SyntaxKind.LocalDeclarationStatement:
                    return Create(node, ((LocalDeclarationStatementSyntax)node).Modifiers);
                case SyntaxKind.LocalFunctionStatement:
                    return Create(node, ((LocalFunctionStatementSyntax)node).Modifiers);
                case SyntaxKind.Parameter:
                    return Create(node, ((ParameterSyntax)node).Modifiers);
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

        internal ModifiersInfo ModifiersInfo()
        {
            return new ModifiersInfo(Node, Modifiers);
        }

        public AccessibilityInfo WithModifiers(SyntaxTokenList newModifiers)
        {
            ThrowInvalidOperationIfNotInitialized();

            ModifiersInfo info = ModifiersInfo().WithModifiers(newModifiers);

            return Create(info.Node, info.Modifiers);
        }

        public AccessibilityInfo WithAccessibility(Accessibility newAccessibility, IModifierComparer comparer = null)
        {
            ThrowInvalidOperationIfNotInitialized();

            return CSharpAccessibility.ChangeAccessibility(this, newAccessibility, comparer);
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

        public override string ToString()
        {
            return Node?.ToString() ?? base.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is AccessibilityInfo other
                && Equals(other);
        }

        public bool Equals(AccessibilityInfo other)
        {
            return EqualityComparer<SyntaxNode>.Default.Equals(Node, other.Node);
        }

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
