// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    public struct ModifiersInfo : IEquatable<ModifiersInfo>
    {
        private ModifiersInfo(SyntaxNode node, SyntaxTokenList modifiers)
        {
            Node = node;
            Modifiers = modifiers;
        }

        private static ModifiersInfo Default { get; } = new ModifiersInfo();

        public SyntaxNode Node { get; }

        public SyntaxTokenList Modifiers { get; }

        public Accessibility Accessibility
        {
            get { return CSharpAccessibility.GetAccessibility(Modifiers); }
        }

        //TODO: 
        public bool HasNew => Modifiers.Contains(SyntaxKind.NewKeyword);

        public bool HasPublic => Modifiers.Contains(SyntaxKind.PublicKeyword);

        public bool HasPrivate => Modifiers.Contains(SyntaxKind.PrivateKeyword);

        public bool HasProtected => Modifiers.Contains(SyntaxKind.ProtectedKeyword);

        public bool HasInternal => Modifiers.Contains(SyntaxKind.InternalKeyword);

        public bool HasConst => Modifiers.Contains(SyntaxKind.ConstKeyword);

        public bool HasStatic => Modifiers.Contains(SyntaxKind.StaticKeyword);

        public bool HasVirtual => Modifiers.Contains(SyntaxKind.VirtualKeyword);

        public bool HasSealed => Modifiers.Contains(SyntaxKind.SealedKeyword);

        public bool HasOverride => Modifiers.Contains(SyntaxKind.OverrideKeyword);

        public bool HasAbstract => Modifiers.Contains(SyntaxKind.AbstractKeyword);

        public bool HasReadOnly => Modifiers.Contains(SyntaxKind.ReadOnlyKeyword);

        public bool HasExtern => Modifiers.Contains(SyntaxKind.ExternKeyword);

        public bool HasUnsafe => Modifiers.Contains(SyntaxKind.UnsafeKeyword);

        public bool HasVolatile => Modifiers.Contains(SyntaxKind.VolatileKeyword);

        public bool HasAsync => Modifiers.Contains(SyntaxKind.AsyncKeyword);

        public bool HasPartial => Modifiers.Contains(SyntaxKind.PartialKeyword);

        public bool HasRef => Modifiers.Contains(SyntaxKind.RefKeyword);

        public bool HasOut => Modifiers.Contains(SyntaxKind.OutKeyword);

        public bool HasIn => Modifiers.Contains(SyntaxKind.InKeyword);

        public bool HasParams => Modifiers.Contains(SyntaxKind.ParamsKeyword);

        public bool Success
        {
            get { return Node != null; }
        }

        internal static ModifiersInfo Create(SyntaxNode node)
        {
            if (node == null)
                return Default;

            switch (node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    return new ModifiersInfo(node, ((ClassDeclarationSyntax)node).Modifiers);
                case SyntaxKind.ConstructorDeclaration:
                    return new ModifiersInfo(node, ((ConstructorDeclarationSyntax)node).Modifiers);
                case SyntaxKind.ConversionOperatorDeclaration:
                    return new ModifiersInfo(node, ((ConversionOperatorDeclarationSyntax)node).Modifiers);
                case SyntaxKind.DelegateDeclaration:
                    return new ModifiersInfo(node, ((DelegateDeclarationSyntax)node).Modifiers);
                case SyntaxKind.DestructorDeclaration:
                    return new ModifiersInfo(node, ((DestructorDeclarationSyntax)node).Modifiers);
                case SyntaxKind.EnumDeclaration:
                    return new ModifiersInfo(node, ((EnumDeclarationSyntax)node).Modifiers);
                case SyntaxKind.EventDeclaration:
                    return new ModifiersInfo(node, ((EventDeclarationSyntax)node).Modifiers);
                case SyntaxKind.EventFieldDeclaration:
                    return new ModifiersInfo(node, ((EventFieldDeclarationSyntax)node).Modifiers);
                case SyntaxKind.FieldDeclaration:
                    return new ModifiersInfo(node, ((FieldDeclarationSyntax)node).Modifiers);
                case SyntaxKind.IndexerDeclaration:
                    return new ModifiersInfo(node, ((IndexerDeclarationSyntax)node).Modifiers);
                case SyntaxKind.InterfaceDeclaration:
                    return new ModifiersInfo(node, ((InterfaceDeclarationSyntax)node).Modifiers);
                case SyntaxKind.MethodDeclaration:
                    return new ModifiersInfo(node, ((MethodDeclarationSyntax)node).Modifiers);
                case SyntaxKind.OperatorDeclaration:
                    return new ModifiersInfo(node, ((OperatorDeclarationSyntax)node).Modifiers);
                case SyntaxKind.PropertyDeclaration:
                    return new ModifiersInfo(node, ((PropertyDeclarationSyntax)node).Modifiers);
                case SyntaxKind.StructDeclaration:
                    return new ModifiersInfo(node, ((StructDeclarationSyntax)node).Modifiers);
                case SyntaxKind.IncompleteMember:
                    return new ModifiersInfo(node, ((IncompleteMemberSyntax)node).Modifiers);
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                case SyntaxKind.UnknownAccessorDeclaration:
                    return new ModifiersInfo(node, ((AccessorDeclarationSyntax)node).Modifiers);
                case SyntaxKind.LocalDeclarationStatement:
                    return new ModifiersInfo(node, ((LocalDeclarationStatementSyntax)node).Modifiers);
                case SyntaxKind.LocalFunctionStatement:
                    return new ModifiersInfo(node, ((LocalFunctionStatementSyntax)node).Modifiers);
                case SyntaxKind.Parameter:
                    return new ModifiersInfo(node, ((ParameterSyntax)node).Modifiers);
            }

            return Default;
        }

        internal static ModifiersInfo Create(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration == null)
                return Default;

            return new ModifiersInfo(classDeclaration, classDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(ConstructorDeclarationSyntax constructorDeclaration)
        {
            if (constructorDeclaration == null)
                return Default;

            return new ModifiersInfo(constructorDeclaration, constructorDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            if (conversionOperatorDeclaration == null)
                return Default;

            return new ModifiersInfo(conversionOperatorDeclaration, conversionOperatorDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                return Default;

            return new ModifiersInfo(delegateDeclaration, delegateDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(DestructorDeclarationSyntax destructorDeclaration)
        {
            if (destructorDeclaration == null)
                return Default;

            return new ModifiersInfo(destructorDeclaration, destructorDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(EnumDeclarationSyntax enumDeclaration)
        {
            if (enumDeclaration == null)
                return Default;

            return new ModifiersInfo(enumDeclaration, enumDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(EventDeclarationSyntax eventDeclaration)
        {
            if (eventDeclaration == null)
                return Default;

            return new ModifiersInfo(eventDeclaration, eventDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            if (eventFieldDeclaration == null)
                return Default;

            return new ModifiersInfo(eventFieldDeclaration, eventFieldDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration == null)
                return Default;

            return new ModifiersInfo(fieldDeclaration, fieldDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration == null)
                return Default;

            return new ModifiersInfo(indexerDeclaration, indexerDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (interfaceDeclaration == null)
                return Default;

            return new ModifiersInfo(interfaceDeclaration, interfaceDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                return Default;

            return new ModifiersInfo(methodDeclaration, methodDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(OperatorDeclarationSyntax operatorDeclaration)
        {
            if (operatorDeclaration == null)
                return Default;

            return new ModifiersInfo(operatorDeclaration, operatorDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration == null)
                return Default;

            return new ModifiersInfo(propertyDeclaration, propertyDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(StructDeclarationSyntax structDeclaration)
        {
            if (structDeclaration == null)
                return Default;

            return new ModifiersInfo(structDeclaration, structDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(IncompleteMemberSyntax incompleteMember)
        {
            if (incompleteMember == null)
                return Default;

            return new ModifiersInfo(incompleteMember, incompleteMember.Modifiers);
        }

        internal static ModifiersInfo Create(AccessorDeclarationSyntax accessorDeclaration)
        {
            if (accessorDeclaration == null)
                return Default;

            return new ModifiersInfo(accessorDeclaration, accessorDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(LocalDeclarationStatementSyntax localDeclarationStatement)
        {
            if (localDeclarationStatement == null)
                return Default;

            return new ModifiersInfo(localDeclarationStatement, localDeclarationStatement.Modifiers);
        }

        internal static ModifiersInfo Create(LocalFunctionStatementSyntax localFunctionStatement)
        {
            if (localFunctionStatement == null)
                return Default;

            return new ModifiersInfo(localFunctionStatement, localFunctionStatement.Modifiers);
        }

        internal static ModifiersInfo Create(ParameterSyntax parameter)
        {
            if (parameter == null)
                return Default;

            return new ModifiersInfo(parameter, parameter.Modifiers);
        }

        public ModifiersInfo WithModifiers(SyntaxTokenList modifiers)
        {
            ThrowInvalidOperationIfNotInitialized();

            return WithModifiers(Node, modifiers);
        }

        internal static ModifiersInfo WithModifiers(SyntaxNode node, SyntaxTokenList modifiers)
        {
            switch (node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    {
                        var classDeclaration = (ClassDeclarationSyntax)node;
                        ClassDeclarationSyntax newNode = classDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.ConstructorDeclaration:
                    {
                        var constructorDeclaration = (ConstructorDeclarationSyntax)node;
                        ConstructorDeclarationSyntax newNode = constructorDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.OperatorDeclaration:
                    {
                        var operatorDeclaration = (OperatorDeclarationSyntax)node;
                        OperatorDeclarationSyntax newNode = operatorDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.ConversionOperatorDeclaration:
                    {
                        var conversionOperatorDeclaration = (ConversionOperatorDeclarationSyntax)node;
                        ConversionOperatorDeclarationSyntax newNode = conversionOperatorDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.DelegateDeclaration:
                    {
                        var delegateDeclaration = (DelegateDeclarationSyntax)node;
                        DelegateDeclarationSyntax newNode = delegateDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.DestructorDeclaration:
                    {
                        var destructorDeclaration = (DestructorDeclarationSyntax)node;
                        DestructorDeclarationSyntax newNode = destructorDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.EnumDeclaration:
                    {
                        var enumDeclaration = (EnumDeclarationSyntax)node;
                        EnumDeclarationSyntax newNode = enumDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.EventDeclaration:
                    {
                        var eventDeclaration = (EventDeclarationSyntax)node;
                        EventDeclarationSyntax newNode = eventDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.EventFieldDeclaration:
                    {
                        var eventFieldDeclaration = (EventFieldDeclarationSyntax)node;
                        EventFieldDeclarationSyntax newNode = eventFieldDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.FieldDeclaration:
                    {
                        var fieldDeclaration = (FieldDeclarationSyntax)node;
                        FieldDeclarationSyntax newNode = fieldDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.IndexerDeclaration:
                    {
                        var indexerDeclaration = (IndexerDeclarationSyntax)node;
                        IndexerDeclarationSyntax newNode = indexerDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.InterfaceDeclaration:
                    {
                        var interfaceDeclaration = (InterfaceDeclarationSyntax)node;
                        InterfaceDeclarationSyntax newNode = interfaceDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax)node;
                        MethodDeclarationSyntax newNode = methodDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.PropertyDeclaration:
                    {
                        var propertyDeclaration = (PropertyDeclarationSyntax)node;
                        PropertyDeclarationSyntax newNode = propertyDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.StructDeclaration:
                    {
                        var structDeclaration = (StructDeclarationSyntax)node;
                        StructDeclarationSyntax newNode = structDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.IncompleteMember:
                    {
                        var incompleteMember = (IncompleteMemberSyntax)node;
                        IncompleteMemberSyntax newNode = incompleteMember.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                case SyntaxKind.UnknownAccessorDeclaration:
                    {
                        var accessorDeclaration = (AccessorDeclarationSyntax)node;
                        AccessorDeclarationSyntax newNode = accessorDeclaration.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.LocalDeclarationStatement:
                    {
                        var localDeclarationStatement = (LocalDeclarationStatementSyntax)node;
                        LocalDeclarationStatementSyntax newNode = localDeclarationStatement.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.LocalFunctionStatement:
                    {
                        var localFunctionStatement = (LocalFunctionStatementSyntax)node;
                        LocalFunctionStatementSyntax newNode = localFunctionStatement.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
                case SyntaxKind.Parameter:
                    {
                        var parameter = (ParameterSyntax)node;
                        ParameterSyntax newNode = parameter.WithModifiers(modifiers);
                        return new ModifiersInfo(newNode, newNode.Modifiers);
                    }
            }

            throw new InvalidOperationException();
        }

        public ModifierFlags GetFlags()
        {
            var flags = ModifierFlags.None;

            for (int i = 0; i < Modifiers.Count; i++)
            {
                switch (Modifiers[i].Kind())
                {
                    case SyntaxKind.PublicKeyword:
                        {
                            flags |= ModifierFlags.Public;
                            break;
                        }
                    case SyntaxKind.PrivateKeyword:
                        {
                            flags |= ModifierFlags.Private;
                            break;
                        }
                    case SyntaxKind.InternalKeyword:
                        {
                            flags |= ModifierFlags.Internal;
                            break;
                        }
                    case SyntaxKind.ProtectedKeyword:
                        {
                            flags |= ModifierFlags.Protected;
                            break;
                        }
                    case SyntaxKind.StaticKeyword:
                        {
                            flags |= ModifierFlags.Static;
                            break;
                        }
                    case SyntaxKind.ReadOnlyKeyword:
                        {
                            flags |= ModifierFlags.ReadOnly;
                            break;
                        }
                    case SyntaxKind.SealedKeyword:
                        {
                            flags |= ModifierFlags.Sealed;
                            break;
                        }
                    case SyntaxKind.ConstKeyword:
                        {
                            flags |= ModifierFlags.Const;
                            break;
                        }
                    case SyntaxKind.VolatileKeyword:
                        {
                            flags |= ModifierFlags.Volatile;
                            break;
                        }
                    case SyntaxKind.NewKeyword:
                        {
                            flags |= ModifierFlags.New;
                            break;
                        }
                    case SyntaxKind.OverrideKeyword:
                        {
                            flags |= ModifierFlags.Override;
                            break;
                        }
                    case SyntaxKind.AbstractKeyword:
                        {
                            flags |= ModifierFlags.Abstract;
                            break;
                        }
                    case SyntaxKind.VirtualKeyword:
                        {
                            flags |= ModifierFlags.Virtual;
                            break;
                        }
                    case SyntaxKind.RefKeyword:
                        {
                            flags |= ModifierFlags.Ref;
                            break;
                        }
                    case SyntaxKind.OutKeyword:
                        {
                            flags |= ModifierFlags.Out;
                            break;
                        }
                    case SyntaxKind.InKeyword:
                        {
                            flags |= ModifierFlags.In;
                            break;
                        }
                    case SyntaxKind.ParamsKeyword:
                        {
                            flags |= ModifierFlags.Params;
                            break;
                        }
                    case SyntaxKind.UnsafeKeyword:
                        {
                            flags |= ModifierFlags.Unsafe;
                            break;
                        }
                    case SyntaxKind.PartialKeyword:
                        {
                            flags |= ModifierFlags.Partial;
                            break;
                        }
                    case SyntaxKind.AsyncKeyword:
                        {
                            flags |= ModifierFlags.Async;
                            break;
                        }
                    default:
                        {
                            Debug.Fail(Modifiers[i].Kind().ToString());
                            break;
                        }
                }
            }

            return flags;
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (Node == null)
                throw new InvalidOperationException($"{nameof(ModifiersInfo)} is not initalized.");
        }

        public override string ToString()
        {
            return Node?.ToString() ?? base.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is ModifiersInfo other && Equals(other);
        }

        public bool Equals(ModifiersInfo other)
        {
            return EqualityComparer<SyntaxNode>.Default.Equals(Node, other.Node);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<SyntaxNode>.Default.GetHashCode(Node);
        }

        public static bool operator ==(ModifiersInfo info1, ModifiersInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(ModifiersInfo info1, ModifiersInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
