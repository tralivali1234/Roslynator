// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Comparers;
using Roslynator.CSharp.Syntax;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp
{
    internal static class CSharpAccessibility
    {
        internal static Accessibility GetDefaultExplicitAccessibility(MemberDeclarationSyntax member)
        {
            switch (member.Kind())
            {
                case SyntaxKind.ConstructorDeclaration:
                    {
                        if (((ConstructorDeclarationSyntax)member).Modifiers.Contains(SyntaxKind.StaticKeyword))
                        {
                            return Accessibility.NotApplicable;
                        }
                        else
                        {
                            return Accessibility.Private;
                        }
                    }
                case SyntaxKind.DestructorDeclaration:
                    {
                        return Accessibility.NotApplicable;
                    }
                case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax)member;

                        if (methodDeclaration.Modifiers.Contains(SyntaxKind.PartialKeyword)
                            || methodDeclaration.ExplicitInterfaceSpecifier != null
                            || methodDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                        {
                            return Accessibility.NotApplicable;
                        }
                        else
                        {
                            return Accessibility.Private;
                        }
                    }
                case SyntaxKind.PropertyDeclaration:
                    {
                        var propertyDeclaration = (PropertyDeclarationSyntax)member;

                        if (propertyDeclaration.ExplicitInterfaceSpecifier != null
                            || propertyDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                        {
                            return Accessibility.NotApplicable;
                        }
                        else
                        {
                            return Accessibility.Private;
                        }
                    }
                case SyntaxKind.IndexerDeclaration:
                    {
                        var indexerDeclaration = (IndexerDeclarationSyntax)member;

                        if (indexerDeclaration.ExplicitInterfaceSpecifier != null
                            || indexerDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                        {
                            return Accessibility.NotApplicable;
                        }
                        else
                        {
                            return Accessibility.Private;
                        }
                    }
                case SyntaxKind.EventDeclaration:
                    {
                        var eventDeclaration = (EventDeclarationSyntax)member;

                        if (eventDeclaration.ExplicitInterfaceSpecifier != null)
                        {
                            return Accessibility.NotApplicable;
                        }
                        else
                        {
                            return Accessibility.Private;
                        }
                    }
                case SyntaxKind.EventFieldDeclaration:
                    {
                        if (member.IsParentKind(SyntaxKind.InterfaceDeclaration))
                        {
                            return Accessibility.NotApplicable;
                        }
                        else
                        {
                            return Accessibility.Private;
                        }
                    }
                case SyntaxKind.FieldDeclaration:
                    {
                        return Accessibility.Private;
                    }
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                    {
                        return Accessibility.Public;
                    }
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.EnumDeclaration:
                case SyntaxKind.DelegateDeclaration:
                    {
                        if (member.IsParentKind(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration))
                        {
                            return Accessibility.Private;
                        }
                        else
                        {
                            return Accessibility.Internal;
                        }
                    }
            }

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetExplicitAccessibility(MemberDeclarationSyntax member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            return GetAccessibility(member.GetModifiers());
        }

        public static Accessibility GetAccessibility(MemberDeclarationSyntax member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            switch (member.Kind())
            {
                case SyntaxKind.ConstructorDeclaration:
                    {
                        var constructorDeclaration = (ConstructorDeclarationSyntax)member;

                        if (constructorDeclaration.Modifiers.Contains(SyntaxKind.StaticKeyword))
                        {
                            return Accessibility.Private;
                        }
                        else
                        {
                            return GetAccessibilityOrDefault(constructorDeclaration, constructorDeclaration.Modifiers);
                        }
                    }
                case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax)member;

                        SyntaxTokenList modifiers = methodDeclaration.Modifiers;

                        if (modifiers.Contains(SyntaxKind.PartialKeyword))
                        {
                            return Accessibility.Private;
                        }
                        else if (methodDeclaration.ExplicitInterfaceSpecifier != null
                            || methodDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                        {
                            return Accessibility.Public;
                        }
                        else
                        {
                            return GetAccessibilityOrDefault(methodDeclaration, modifiers);
                        }
                    }
                case SyntaxKind.PropertyDeclaration:
                    {
                        var propertyDeclaration = (PropertyDeclarationSyntax)member;

                        if (propertyDeclaration.ExplicitInterfaceSpecifier != null
                            || propertyDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                        {
                            return Accessibility.Public;
                        }
                        else
                        {
                            return GetAccessibilityOrDefault(propertyDeclaration, propertyDeclaration.Modifiers);
                        }
                    }
                case SyntaxKind.IndexerDeclaration:
                    {
                        var indexerDeclaration = (IndexerDeclarationSyntax)member;

                        if (indexerDeclaration.ExplicitInterfaceSpecifier != null
                            || indexerDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                        {
                            return Accessibility.Public;
                        }
                        else
                        {
                            return GetAccessibilityOrDefault(indexerDeclaration, indexerDeclaration.Modifiers);
                        }
                    }
                case SyntaxKind.EventDeclaration:
                    {
                        var eventDeclaration = (EventDeclarationSyntax)member;

                        if (eventDeclaration.ExplicitInterfaceSpecifier != null)
                        {
                            return Accessibility.Public;
                        }
                        else
                        {
                            return GetAccessibilityOrDefault(eventDeclaration, eventDeclaration.Modifiers);
                        }
                    }
                case SyntaxKind.EventFieldDeclaration:
                    {
                        if (member.IsParentKind(SyntaxKind.InterfaceDeclaration))
                        {
                            return Accessibility.Public;
                        }
                        else
                        {
                            var eventFieldDeclaration = (EventFieldDeclarationSyntax)member;

                            return GetAccessibilityOrDefault(eventFieldDeclaration, eventFieldDeclaration.Modifiers);
                        }
                    }
                case SyntaxKind.FieldDeclaration:
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.EnumDeclaration:
                case SyntaxKind.DelegateDeclaration:
                    {
                        return GetAccessibilityOrDefault(member, member.GetModifiers());
                    }
                case SyntaxKind.DestructorDeclaration:
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                case SyntaxKind.EnumMemberDeclaration:
                case SyntaxKind.NamespaceDeclaration:
                    {
                        return Accessibility.Public;
                    }
            }

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetAccessibilityOrDefault(MemberDeclarationSyntax member)
        {
            return GetAccessibilityOrDefault(member, member.GetModifiers());
        }

        private static Accessibility GetAccessibilityOrDefault(MemberDeclarationSyntax member, SyntaxTokenList modifiers)
        {
            Accessibility accessibility = GetAccessibility(modifiers);

            if (accessibility == Accessibility.NotApplicable)
            {
                accessibility = GetDefaultExplicitAccessibility(member);
            }

            return accessibility;
        }

        public static Accessibility GetAccessibility(SyntaxTokenList modifiers)
        {
            int count = modifiers.Count;

            for (int i = 0; i < count; i++)
            {
                switch (modifiers[i].Kind())
                {
                    case SyntaxKind.PublicKeyword:
                        {
                            return Accessibility.Public;
                        }
                    case SyntaxKind.PrivateKeyword:
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                if (modifiers[j].Kind() == SyntaxKind.ProtectedKeyword)
                                    return Accessibility.ProtectedAndInternal;
                            }

                            return Accessibility.Private;
                        }
                    case SyntaxKind.InternalKeyword:
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                if (modifiers[j].Kind() == SyntaxKind.ProtectedKeyword)
                                    return Accessibility.ProtectedOrInternal;
                            }

                            return Accessibility.Internal;
                        }
                    case SyntaxKind.ProtectedKeyword:
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                switch (modifiers[j].Kind())
                                {
                                    case SyntaxKind.InternalKeyword:
                                        return Accessibility.ProtectedOrInternal;
                                    case SyntaxKind.PrivateKeyword:
                                        return Accessibility.ProtectedAndInternal;
                                }
                            }

                            return Accessibility.Protected;
                        }
                }
            }

            return Accessibility.NotApplicable;
        }

        public static TNode ChangeAccessibility<TNode>(
            TNode node,
            Accessibility newAccessibility,
            IModifierComparer comparer = null) where TNode : SyntaxNode
        {
            AccessibilityInfo info = SyntaxInfo.AccessibilityInfo(node);

            AccessibilityInfo newInfo = ChangeAccessibility(info, newAccessibility, comparer);

            return (TNode)newInfo.Node;
        }

        public static AccessibilityInfo ChangeAccessibility(
            AccessibilityInfo info,
            Accessibility newAccessibility,
            IModifierComparer comparer = null)
        {
            if (!info.Success)
                return info;

            Accessibility accessibility = info.Accessibility;

            if (accessibility == newAccessibility)
                return info;

            comparer = comparer ?? ModifierComparer.Instance;

            SyntaxNode node = info.Node;

            if (accessibility.IsSingleTokenAccessibility()
                && newAccessibility.IsSingleTokenAccessibility())
            {
                int insertIndex = comparer.GetInsertIndex(info.Modifiers, GetKind(newAccessibility));

                if (info.TokenIndex == insertIndex
                    || info.TokenIndex == insertIndex - 1)
                {
                    SyntaxToken newToken = CreateToken(newAccessibility).WithTriviaFrom(info.Token);

                    SyntaxTokenList newModifiers = info.Modifiers.Replace(info.Token, newToken);

                    return info.WithModifiers(newModifiers);
                }
            }

            if (accessibility != Accessibility.NotApplicable)
            {
                node = Modifier.RemoveAt(node, Math.Max(info.TokenIndex, info.SecondTokenIndex));

                if (info.SecondTokenIndex != -1)
                    node = Modifier.RemoveAt(node, Math.Min(info.TokenIndex, info.SecondTokenIndex));
            }

            if (newAccessibility != Accessibility.NotApplicable)
            {
                node = InsertModifier(node, newAccessibility, comparer);
            }

            return SyntaxInfo.AccessibilityInfo(node);
        }

        private static SyntaxNode InsertModifier(SyntaxNode node, Accessibility accessibility, IModifierComparer comparer)
        {
            switch (accessibility)
            {
                case Accessibility.Private:
                    {
                        return node.InsertModifier(SyntaxKind.PrivateKeyword, comparer);
                    }
                case Accessibility.Protected:
                    {
                        return node.InsertModifier(SyntaxKind.ProtectedKeyword, comparer);
                    }
                case Accessibility.ProtectedAndInternal:
                    {
                        node = node.InsertModifier(SyntaxKind.PrivateKeyword, comparer);

                        return node.InsertModifier(SyntaxKind.ProtectedKeyword, comparer);
                    }
                case Accessibility.Internal:
                    {
                        return node.InsertModifier(SyntaxKind.InternalKeyword, comparer);
                    }
                case Accessibility.Public:
                    {
                        return node.InsertModifier(SyntaxKind.PublicKeyword, comparer);
                    }
                case Accessibility.ProtectedOrInternal:
                    {
                        node = node.InsertModifier(SyntaxKind.ProtectedKeyword, comparer);

                        return node.InsertModifier(SyntaxKind.InternalKeyword, comparer);
                    }
            }

            return node;
        }

        private static SyntaxToken CreateToken(Accessibility accessibility)
        {
            switch (accessibility)
            {
                case Accessibility.Private:
                    return PrivateKeyword();
                case Accessibility.Protected:
                    return ProtectedKeyword();
                case Accessibility.Internal:
                    return InternalKeyword();
                case Accessibility.Public:
                    return PublicKeyword();
                case Accessibility.NotApplicable:
                    return default(SyntaxToken);
                default:
                    throw new ArgumentException("", nameof(accessibility));
            }
        }

        private static SyntaxKind GetKind(Accessibility accessibility)
        {
            switch (accessibility)
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
                    throw new ArgumentException("", nameof(accessibility));
            }
        }
    }
}
