// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// 
    /// </summary>
    public readonly struct TypeParameterInfo : IEquatable<TypeParameterInfo>
    {
        private TypeParameterInfo(
            TypeParameterSyntax typeParameter,
            SyntaxNode declaration,
            TypeParameterListSyntax typeParameterList,
            SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            TypeParameter = typeParameter;
            Declaration = declaration;
            TypeParameterList = typeParameterList;
            ConstraintClauses = constraintClauses;
        }

        private static TypeParameterInfo Default { get; } = new TypeParameterInfo();

        /// <summary>
        /// 
        /// </summary>
        public TypeParameterSyntax TypeParameter { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return TypeParameter?.Identifier.ValueText; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxNode Declaration { get; }

        /// <summary>
        /// 
        /// </summary>
        public TypeParameterListSyntax TypeParameterList { get; }

        /// <summary>
        /// 
        /// </summary>
        public SeparatedSyntaxList<TypeParameterSyntax> TypeParameters
        {
            get { return TypeParameterList?.Parameters ?? default(SeparatedSyntaxList<TypeParameterSyntax>); }
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses { get; }

        /// <summary>
        /// 
        /// </summary>
        public TypeParameterConstraintClauseSyntax ConstraintClause
        {
            get
            {
                string name = Name;

                foreach (TypeParameterConstraintClauseSyntax constraintClause in ConstraintClauses)
                {
                    if (string.Equals(name, constraintClause.Name.Identifier.ValueText, StringComparison.Ordinal))
                        return constraintClause;
                }

                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get { return TypeParameter != null; }
        }

        internal static TypeParameterInfo Create(TypeParameterSyntax typeParameter)
        {
            if (!(typeParameter.Parent is TypeParameterListSyntax typeParameterList))
                return Default;

            SyntaxNode parent = typeParameterList.Parent;

            switch (parent?.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    {
                        var classDeclaration = (ClassDeclarationSyntax)parent;
                        return new TypeParameterInfo(typeParameter, classDeclaration, typeParameterList, classDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.DelegateDeclaration:
                    {
                        var delegateDeclaration = (DelegateDeclarationSyntax)parent;
                        return new TypeParameterInfo(typeParameter, delegateDeclaration, typeParameterList, delegateDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.InterfaceDeclaration:
                    {
                        var interfaceDeclaration = (InterfaceDeclarationSyntax)parent;
                        return new TypeParameterInfo(typeParameter, interfaceDeclaration, typeParameterList, interfaceDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.LocalFunctionStatement:
                    {
                        var localFunctionStatement = (LocalFunctionStatementSyntax)parent;
                        return new TypeParameterInfo(typeParameter, localFunctionStatement, typeParameterList, localFunctionStatement.ConstraintClauses);
                    }
                case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax)parent;
                        return new TypeParameterInfo(typeParameter, methodDeclaration, typeParameterList, methodDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.StructDeclaration:
                    {
                        var structDeclaration = (StructDeclarationSyntax)parent;
                        return new TypeParameterInfo(typeParameter, structDeclaration, typeParameterList, structDeclaration.ConstraintClauses);
                    }
            }

            return Default;
        }

        internal static TypeParameterInfo Create(SyntaxNode declaration, string name)
        {
            switch (declaration?.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    {
                        var classDeclaration = (ClassDeclarationSyntax)declaration;

                        TypeParameterListSyntax typeParameterList = classDeclaration.TypeParameterList;

                        if (typeParameterList == null)
                            return Default;

                        TypeParameterSyntax typeParameter = typeParameterList.GetTypeParameterByName(name);

                        if (typeParameter == null)
                            return Default;

                        return new TypeParameterInfo(typeParameter, classDeclaration, typeParameterList, classDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.DelegateDeclaration:
                    {
                        var delegateDeclaration = (DelegateDeclarationSyntax)declaration;

                        TypeParameterListSyntax typeParameterList = delegateDeclaration.TypeParameterList;

                        if (typeParameterList == null)
                            return Default;

                        TypeParameterSyntax typeParameter = typeParameterList.GetTypeParameterByName(name);

                        if (typeParameter == null)
                            return Default;

                        return new TypeParameterInfo(typeParameter, delegateDeclaration, typeParameterList, delegateDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.InterfaceDeclaration:
                    {
                        var interfaceDeclaration = (InterfaceDeclarationSyntax)declaration;

                        TypeParameterListSyntax typeParameterList = interfaceDeclaration.TypeParameterList;

                        if (typeParameterList == null)
                            return Default;

                        TypeParameterSyntax typeParameter = typeParameterList.GetTypeParameterByName(name);

                        if (typeParameter == null)
                            return Default;

                        return new TypeParameterInfo(typeParameter, interfaceDeclaration, typeParameterList, interfaceDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.LocalFunctionStatement:
                    {
                        var localFunctionStatement = (LocalFunctionStatementSyntax)declaration;

                        TypeParameterListSyntax typeParameterList = localFunctionStatement.TypeParameterList;

                        if (typeParameterList == null)
                            return Default;

                        TypeParameterSyntax typeParameter = typeParameterList.GetTypeParameterByName(name);

                        if (typeParameter == null)
                            return Default;

                        return new TypeParameterInfo(typeParameter, localFunctionStatement, typeParameterList, localFunctionStatement.ConstraintClauses);
                    }
                case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax)declaration;

                        TypeParameterListSyntax typeParameterList = methodDeclaration.TypeParameterList;

                        if (typeParameterList == null)
                            return Default;

                        TypeParameterSyntax typeParameter = typeParameterList.GetTypeParameterByName(name);

                        if (typeParameter == null)
                            return Default;

                        return new TypeParameterInfo(typeParameter, methodDeclaration, typeParameterList, methodDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.StructDeclaration:
                    {
                        var structDeclaration = (StructDeclarationSyntax)declaration;

                        TypeParameterListSyntax typeParameterList = structDeclaration.TypeParameterList;

                        if (typeParameterList == null)
                            return Default;

                        TypeParameterSyntax typeParameter = typeParameterList.GetTypeParameterByName(name);

                        if (typeParameter == null)
                            return Default;

                        return new TypeParameterInfo(typeParameter, structDeclaration, typeParameterList, structDeclaration.ConstraintClauses);
                    }
            }

            return Default;
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="string" /> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return TypeParameter?.ToString() ?? base.ToString();
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            return obj is TypeParameterInfo other && Equals(other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(TypeParameterInfo other)
        {
            return EqualityComparer<SyntaxNode>.Default.Equals(Declaration, other.Declaration);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<SyntaxNode>.Default.GetHashCode(Declaration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator ==(TypeParameterInfo info1, TypeParameterInfo info2)
        {
            return info1.Equals(info2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <returns></returns>
        public static bool operator !=(TypeParameterInfo info1, TypeParameterInfo info2)
        {
            return !(info1 == info2);
        }
    }
}