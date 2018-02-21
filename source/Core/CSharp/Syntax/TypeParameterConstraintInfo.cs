// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Roslynator.CSharp.Syntax.SyntaxInfoHelpers;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// Provides information about a type parameter constraint.
    /// </summary>
    public readonly struct TypeParameterConstraintInfo : IEquatable<TypeParameterConstraintInfo>
    {
        private TypeParameterConstraintInfo(
            TypeParameterConstraintSyntax constraint,
            TypeParameterConstraintClauseSyntax constraintClause,
            SyntaxNode declaration,
            TypeParameterListSyntax typeParameterList,
            SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            Constraint = constraint;
            ConstraintClause = constraintClause;
            Declaration = declaration;
            TypeParameterList = typeParameterList;
            ConstraintClauses = constraintClauses;
        }

        private static TypeParameterConstraintInfo Default { get; } = new TypeParameterConstraintInfo();

        /// <summary>
        /// The type parameter constraint.
        /// </summary>
        public TypeParameterConstraintSyntax Constraint { get; }

        /// <summary>
        /// The constraint clause that contains this constraint in <see cref="TypeParameterConstraintClauseSyntax.Constraints"/> collection.
        /// </summary>
        public TypeParameterConstraintClauseSyntax ConstraintClause { get; }

        /// <summary>
        /// A list of constraints that contains this constraint.
        /// </summary>
        public SeparatedSyntaxList<TypeParameterConstraintSyntax> Constraints
        {
            get { return ConstraintClause?.Constraints ?? default(SeparatedSyntaxList<TypeParameterConstraintSyntax>); }
        }

        /// <summary>
        /// The identifier name of this constraint.
        /// </summary>
        public IdentifierNameSyntax Name
        {
            get { return ConstraintClause?.Name; }
        }

        /// <summary>
        /// The name of this constraint.
        /// </summary>
        public string NameText
        {
            get { return Name?.Identifier.ValueText; }
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
        /// The type parameter that this constraint is declared for. Returns null if a type parameter is not declared.
        /// </summary>
        public TypeParameterSyntax TypeParameter
        {
            get
            {
                foreach (TypeParameterSyntax typeParameter in TypeParameters)
                {
                    if (string.Equals(NameText, typeParameter.Identifier.ValueText, StringComparison.Ordinal))
                        return typeParameter;
                }

                return null;
            }
        }

        /// <summary>
        /// Determines whether this struct was initialized with an actual syntax.
        /// </summary>
        public bool Success
        {
            get { return Constraint != null; }
        }

        internal bool IsDuplicateConstraint
        {
            get
            {
                if (Constraint == null)
                    return false;

                SeparatedSyntaxList<TypeParameterConstraintSyntax> constraints = Constraints;

                int index = constraints.IndexOf(Constraint);

                SyntaxKind kind = Constraint.Kind();

                switch (kind)
                {
                    case SyntaxKind.ClassConstraint:
                    case SyntaxKind.StructConstraint:
                        {
                            for (int i = 0; i < index; i++)
                            {
                                if (constraints[i].Kind() == kind)
                                    return true;
                            }

                            break;
                        }
                }

                return false;
            }
        }

        internal static TypeParameterConstraintInfo Create(
            TypeParameterConstraintSyntax constraint,
            bool allowMissing = false)
        {
            if (!(constraint?.Parent is TypeParameterConstraintClauseSyntax constraintClause))
                return Default;

            IdentifierNameSyntax name = constraintClause.Name;

            if (!Check(name, allowMissing))
                return Default;

            SyntaxNode parent = constraintClause.Parent;

            switch (parent?.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    {
                        var classDeclaration = (ClassDeclarationSyntax)parent;

                        TypeParameterListSyntax typeParameterList = classDeclaration.TypeParameterList;

                        if (!Check(typeParameterList, allowMissing))
                            return Default;

                        return new TypeParameterConstraintInfo(constraint, constraintClause, classDeclaration, typeParameterList, classDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.DelegateDeclaration:
                    {
                        var delegateDeclaration = (DelegateDeclarationSyntax)parent;

                        TypeParameterListSyntax typeParameterList = delegateDeclaration.TypeParameterList;

                        if (!Check(typeParameterList, allowMissing))
                            return Default;

                        return new TypeParameterConstraintInfo(constraint, constraintClause, delegateDeclaration, typeParameterList, delegateDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.InterfaceDeclaration:
                    {
                        var interfaceDeclaration = (InterfaceDeclarationSyntax)parent;

                        TypeParameterListSyntax typeParameterList = interfaceDeclaration.TypeParameterList;

                        if (!Check(typeParameterList, allowMissing))
                            return Default;

                        return new TypeParameterConstraintInfo(constraint, constraintClause, interfaceDeclaration, interfaceDeclaration.TypeParameterList, interfaceDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.LocalFunctionStatement:
                    {
                        var localFunctionStatement = (LocalFunctionStatementSyntax)parent;

                        TypeParameterListSyntax typeParameterList = localFunctionStatement.TypeParameterList;

                        if (!Check(typeParameterList, allowMissing))
                            return Default;

                        return new TypeParameterConstraintInfo(constraint, constraintClause, localFunctionStatement, typeParameterList, localFunctionStatement.ConstraintClauses);
                    }
                case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax)parent;

                        TypeParameterListSyntax typeParameterList = methodDeclaration.TypeParameterList;

                        if (!Check(typeParameterList, allowMissing))
                            return Default;

                        return new TypeParameterConstraintInfo(constraint, constraintClause, methodDeclaration, typeParameterList, methodDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.StructDeclaration:
                    {
                        var structDeclaration = (StructDeclarationSyntax)parent;

                        TypeParameterListSyntax typeParameterList = structDeclaration.TypeParameterList;

                        if (!Check(typeParameterList, allowMissing))
                            return Default;

                        return new TypeParameterConstraintInfo(constraint, constraintClause, structDeclaration, typeParameterList, structDeclaration.ConstraintClauses);
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
            return Constraint?.ToString() ?? "";
        }

        /// <summary>
        /// Determines whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. </param>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            return obj is TypeParameterConstraintInfo other && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(TypeParameterConstraintInfo other)
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

        public static bool operator ==(TypeParameterConstraintInfo info1, TypeParameterConstraintInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(TypeParameterConstraintInfo info1, TypeParameterConstraintInfo info2)
        {
            return !(info1 == info2);
        }
    }
}