// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// Provides information about a type parameter.
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
        /// The type parameter.
        /// </summary>
        public TypeParameterSyntax TypeParameter { get; }

        /// <summary>
        /// The type parameter name.
        /// </summary>
        public string Name
        {
            get { return TypeParameter?.Identifier.ValueText; }
        }

        /// <summary>
        /// Declaration node that contains this type parameter in its type parameter list.
        /// </summary>
        public SyntaxNode Declaration { get; }

        /// <summary>
        /// Type parameter list that contains this type parameter.
        /// </summary>
        public TypeParameterListSyntax TypeParameterList { get; }

        /// <summary>
        /// A list of type parameters that contains this type parameter.
        /// </summary>
        public SeparatedSyntaxList<TypeParameterSyntax> TypeParameters
        {
            get { return TypeParameterList?.Parameters ?? default(SeparatedSyntaxList<TypeParameterSyntax>); }
        }

        /// <summary>
        /// A list of constraint clauses.
        /// </summary>
        public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses { get; }

        /// <summary>
        /// The type parameter constraint clause. Returns null if a constraint clause if not declared.
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
        /// Determines whether this struct was initialized with an actual syntax.
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

        /// <summary>
        /// Returns the string representation of the underlying syntax, not including its leading and trailing trivia.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return TypeParameter?.ToString() ?? "";
        }

        /// <summary>
        /// Determines whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. </param>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            return obj is TypeParameterInfo other && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(TypeParameterInfo other)
        {
            return EqualityComparer<SyntaxNode>.Default.Equals(Declaration, other.Declaration);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<SyntaxNode>.Default.GetHashCode(Declaration);
        }

        public static bool operator ==(TypeParameterInfo info1, TypeParameterInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(TypeParameterInfo info1, TypeParameterInfo info2)
        {
            return !(info1 == info2);
        }
    }
}