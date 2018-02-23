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
    /// Provides information about generic syntax (class, struct, interface, delegate, method or local function).
    /// </summary>
    public readonly struct GenericInfo : IEquatable<GenericInfo>
    {
        private GenericInfo(ClassDeclarationSyntax classDeclaration)
            : this(classDeclaration, SyntaxKind.ClassDeclaration, classDeclaration.TypeParameterList, classDeclaration.ConstraintClauses)
        {
        }

        private GenericInfo(DelegateDeclarationSyntax delegateDeclaration)
            : this(delegateDeclaration, SyntaxKind.DelegateDeclaration, delegateDeclaration.TypeParameterList, delegateDeclaration.ConstraintClauses)
        {
        }

        private GenericInfo(InterfaceDeclarationSyntax interfaceDeclaration)
            : this(interfaceDeclaration, SyntaxKind.InterfaceDeclaration, interfaceDeclaration.TypeParameterList, interfaceDeclaration.ConstraintClauses)
        {
        }

        private GenericInfo(LocalFunctionStatementSyntax localFunctionStatement)
            : this(localFunctionStatement, SyntaxKind.LocalFunctionStatement, localFunctionStatement.TypeParameterList, localFunctionStatement.ConstraintClauses)
        {
        }

        private GenericInfo(MethodDeclarationSyntax methodDeclaration)
            : this(methodDeclaration, SyntaxKind.MethodDeclaration, methodDeclaration.TypeParameterList, methodDeclaration.ConstraintClauses)
        {
        }

        private GenericInfo(StructDeclarationSyntax structDeclaration)
            : this(structDeclaration, SyntaxKind.StructDeclaration, structDeclaration.TypeParameterList, structDeclaration.ConstraintClauses)
        {
        }

        private GenericInfo(
            SyntaxNode declaration,
            SyntaxKind kind,
            TypeParameterListSyntax typeParameterList,
            SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            Declaration = declaration;
            Kind = kind;
            TypeParameterList = typeParameterList;
            ConstraintClauses = constraintClauses;
        }

        private static GenericInfo Default { get; } = new GenericInfo();

        /// <summary>
        /// The declaration node (for example <see cref="ClassDeclarationSyntax"/> for a class).
        /// </summary>
        public SyntaxNode Declaration { get; }

        /// <summary>
        /// The kind of this generic syntax.
        /// </summary>
        public SyntaxKind Kind { get; }

        /// <summary>
        /// The type parameter list.
        /// </summary>
        public TypeParameterListSyntax TypeParameterList { get; }

        /// <summary>
        /// A list of type parameters.
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
        /// Searches for a type parameter with the specified name and returns the first occurrence within the type parameters.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeParameterSyntax FindTypeParameter(string name)
        {
            foreach (TypeParameterSyntax typeParameter in TypeParameters)
            {
                if (string.Equals(name, typeParameter.Identifier.ValueText, StringComparison.Ordinal))
                    return typeParameter;
            }

            return null;
        }

        /// <summary>
        /// Searches for a constraint clause with the specified type parameter name and returns the first occurrence within the constraint clauses.
        /// </summary>
        /// <param name="typeParameterName"></param>
        /// <returns></returns>
        public TypeParameterConstraintClauseSyntax FindConstraintClause(string typeParameterName)
        {
            foreach (TypeParameterConstraintClauseSyntax constraintClause in ConstraintClauses)
            {
                if (string.Equals(typeParameterName, constraintClause.Name.Identifier.ValueText, StringComparison.Ordinal))
                    return constraintClause;
            }

            return null;
        }

        /// <summary>
        /// Determines whether this struct was initialized with an actual syntax.
        /// </summary>
        public bool Success
        {
            get { return Declaration != null; }
        }

        internal static GenericInfo Create(TypeParameterConstraintSyntax typeParameterConstraint)
        {
            return Create(typeParameterConstraint?.Parent as TypeParameterConstraintClauseSyntax);
        }

        internal static GenericInfo Create(TypeParameterConstraintClauseSyntax constraintClause)
        {
            return Create(constraintClause?.Parent);
        }

        internal static GenericInfo Create(SyntaxNode declaration)
        {
            switch (declaration?.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    {
                        var classDeclaration = (ClassDeclarationSyntax)declaration;
                        return new GenericInfo(classDeclaration, SyntaxKind.ClassDeclaration, classDeclaration.TypeParameterList, classDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.DelegateDeclaration:
                    {
                        var delegateDeclaration = (DelegateDeclarationSyntax)declaration;
                        return new GenericInfo(delegateDeclaration, SyntaxKind.DelegateDeclaration, delegateDeclaration.TypeParameterList, delegateDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.InterfaceDeclaration:
                    {
                        var interfaceDeclaration = (InterfaceDeclarationSyntax)declaration;
                        return new GenericInfo(interfaceDeclaration, SyntaxKind.InterfaceDeclaration, interfaceDeclaration.TypeParameterList, interfaceDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.LocalFunctionStatement:
                    {
                        var localFunctionStatement = (LocalFunctionStatementSyntax)declaration;
                        return new GenericInfo(localFunctionStatement, SyntaxKind.LocalFunctionStatement, localFunctionStatement.TypeParameterList, localFunctionStatement.ConstraintClauses);
                    }
                case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax)declaration;
                        return new GenericInfo(methodDeclaration, SyntaxKind.MethodDeclaration, methodDeclaration.TypeParameterList, methodDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.StructDeclaration:
                    {
                        var structDeclaration = (StructDeclarationSyntax)declaration;
                        return new GenericInfo(structDeclaration, SyntaxKind.StructDeclaration, structDeclaration.TypeParameterList, structDeclaration.ConstraintClauses);
                    }
            }

            return Default;
        }

        internal static GenericInfo Create(TypeParameterListSyntax typeParameterList)
        {
            if (typeParameterList == null)
                return Default;

            SyntaxNode parent = typeParameterList.Parent;

            switch (parent?.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    {
                        var classDeclaration = (ClassDeclarationSyntax)parent;
                        return new GenericInfo(classDeclaration, SyntaxKind.ClassDeclaration, typeParameterList, classDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.DelegateDeclaration:
                    {
                        var delegateDeclaration = (DelegateDeclarationSyntax)parent;
                        return new GenericInfo(delegateDeclaration, SyntaxKind.DelegateDeclaration, typeParameterList, delegateDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.InterfaceDeclaration:
                    {
                        var interfaceDeclaration = (InterfaceDeclarationSyntax)parent;
                        return new GenericInfo(interfaceDeclaration, SyntaxKind.InterfaceDeclaration, typeParameterList, interfaceDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.LocalFunctionStatement:
                    {
                        var localFunctionStatement = (LocalFunctionStatementSyntax)parent;
                        return new GenericInfo(localFunctionStatement, SyntaxKind.LocalFunctionStatement, typeParameterList, localFunctionStatement.ConstraintClauses);
                    }
                case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax)parent;
                        return new GenericInfo(methodDeclaration, SyntaxKind.MethodDeclaration, typeParameterList, methodDeclaration.ConstraintClauses);
                    }
                case SyntaxKind.StructDeclaration:
                    {
                        var structDeclaration = (StructDeclarationSyntax)parent;
                        return new GenericInfo(structDeclaration, SyntaxKind.StructDeclaration, typeParameterList, structDeclaration.ConstraintClauses);
                    }
            }

            return Default;
        }

        internal static GenericInfo Create(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration == null)
                return Default;

            return new GenericInfo(classDeclaration);
        }

        internal static GenericInfo Create(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                return Default;

            return new GenericInfo(delegateDeclaration);
        }

        internal static GenericInfo Create(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (interfaceDeclaration == null)
                return Default;

            return new GenericInfo(interfaceDeclaration);
        }

        internal static GenericInfo Create(LocalFunctionStatementSyntax localFunctionStatement)
        {
            if (localFunctionStatement == null)
                return Default;

            return new GenericInfo(localFunctionStatement);
        }

        internal static GenericInfo Create(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                return Default;

            return new GenericInfo(methodDeclaration);
        }

        internal static GenericInfo Create(StructDeclarationSyntax structDeclaration)
        {
            if (structDeclaration == null)
                return Default;

            return new GenericInfo(structDeclaration);
        }

        /// <summary>
        /// Creates a new <see cref="GenericInfo"/> with the type parameter list updated.
        /// </summary>
        /// <param name="typeParameterList"></param>
        /// <returns></returns>
        public GenericInfo WithTypeParameterList(TypeParameterListSyntax typeParameterList)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Kind)
            {
                case SyntaxKind.ClassDeclaration:
                    return new GenericInfo(((ClassDeclarationSyntax)Declaration).WithTypeParameterList(typeParameterList));
                case SyntaxKind.DelegateDeclaration:
                    return new GenericInfo(((DelegateDeclarationSyntax)Declaration).WithTypeParameterList(typeParameterList));
                case SyntaxKind.InterfaceDeclaration:
                    return new GenericInfo(((InterfaceDeclarationSyntax)Declaration).WithTypeParameterList(typeParameterList));
                case SyntaxKind.LocalFunctionStatement:
                    return new GenericInfo(((LocalFunctionStatementSyntax)Declaration).WithTypeParameterList(typeParameterList));
                case SyntaxKind.MethodDeclaration:
                    return new GenericInfo(((MethodDeclarationSyntax)Declaration).WithTypeParameterList(typeParameterList));
                case SyntaxKind.StructDeclaration:
                    return new GenericInfo(((StructDeclarationSyntax)Declaration).WithTypeParameterList(typeParameterList));
            }

            Debug.Fail(Kind.ToString());
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="GenericInfo"/> with the specified type parameter removed.
        /// </summary>
        /// <param name="typeParameter"></param>
        /// <returns></returns>
        public GenericInfo RemoveTypeParameter(TypeParameterSyntax typeParameter)
        {
            ThrowInvalidOperationIfNotInitialized();

            var self = this;

            switch (self.Kind)
            {
                case SyntaxKind.ClassDeclaration:
                    return new GenericInfo(((ClassDeclarationSyntax)self.Declaration).WithTypeParameterList(RemoveTypeParameter()));
                case SyntaxKind.DelegateDeclaration:
                    return new GenericInfo(((DelegateDeclarationSyntax)self.Declaration).WithTypeParameterList(RemoveTypeParameter()));
                case SyntaxKind.InterfaceDeclaration:
                    return new GenericInfo(((InterfaceDeclarationSyntax)self.Declaration).WithTypeParameterList(RemoveTypeParameter()));
                case SyntaxKind.LocalFunctionStatement:
                    return new GenericInfo(((LocalFunctionStatementSyntax)self.Declaration).WithTypeParameterList(RemoveTypeParameter()));
                case SyntaxKind.MethodDeclaration:
                    return new GenericInfo(((MethodDeclarationSyntax)self.Declaration).WithTypeParameterList(RemoveTypeParameter()));
                case SyntaxKind.StructDeclaration:
                    return new GenericInfo(((StructDeclarationSyntax)self.Declaration).WithTypeParameterList(RemoveTypeParameter()));
            }

            Debug.Fail(Kind.ToString());
            return this;

            TypeParameterListSyntax RemoveTypeParameter()
            {
                SeparatedSyntaxList<TypeParameterSyntax> parameters = self.TypeParameters;

                return (parameters.Count == 1)
                    ? default(TypeParameterListSyntax)
                    : self.TypeParameterList.WithParameters(parameters.Remove(typeParameter));
            }
        }

        /// <summary>
        /// Creates a new <see cref="GenericInfo"/> with the constraint clauses updated.
        /// </summary>
        /// <param name="constraintClauses"></param>
        /// <returns></returns>
        public GenericInfo WithConstraintClauses(SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Kind)
            {
                case SyntaxKind.ClassDeclaration:
                    return new GenericInfo(((ClassDeclarationSyntax)Declaration).WithConstraintClauses(constraintClauses));
                case SyntaxKind.DelegateDeclaration:
                    return new GenericInfo(((DelegateDeclarationSyntax)Declaration).WithConstraintClauses(constraintClauses));
                case SyntaxKind.InterfaceDeclaration:
                    return new GenericInfo(((InterfaceDeclarationSyntax)Declaration).WithConstraintClauses(constraintClauses));
                case SyntaxKind.LocalFunctionStatement:
                    return new GenericInfo(((LocalFunctionStatementSyntax)Declaration).WithConstraintClauses(constraintClauses));
                case SyntaxKind.MethodDeclaration:
                    return new GenericInfo(((MethodDeclarationSyntax)Declaration).WithConstraintClauses(constraintClauses));
                case SyntaxKind.StructDeclaration:
                    return new GenericInfo(((StructDeclarationSyntax)Declaration).WithConstraintClauses(constraintClauses));
            }

            Debug.Fail(Kind.ToString());
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="GenericInfo"/> with the specified constraint clause removed.
        /// </summary>
        /// <param name="constraintClause"></param>
        /// <returns></returns>
        public GenericInfo RemoveConstraintClause(TypeParameterConstraintClauseSyntax constraintClause)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Kind)
            {
                case SyntaxKind.ClassDeclaration:
                    return new GenericInfo(((ClassDeclarationSyntax)Declaration).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
                case SyntaxKind.DelegateDeclaration:
                    return new GenericInfo(((DelegateDeclarationSyntax)Declaration).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
                case SyntaxKind.InterfaceDeclaration:
                    return new GenericInfo(((InterfaceDeclarationSyntax)Declaration).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
                case SyntaxKind.LocalFunctionStatement:
                    return new GenericInfo(((LocalFunctionStatementSyntax)Declaration).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
                case SyntaxKind.MethodDeclaration:
                    return new GenericInfo(((MethodDeclarationSyntax)Declaration).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
                case SyntaxKind.StructDeclaration:
                    return new GenericInfo(((StructDeclarationSyntax)Declaration).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
            }

            Debug.Fail(Kind.ToString());
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="GenericInfo"/> with all constraint clauses removed.
        /// </summary>
        /// <returns></returns>
        public GenericInfo RemoveAllConstraintClauses()
        {
            ThrowInvalidOperationIfNotInitialized();

            if (!ConstraintClauses.Any())
                return this;

            TypeParameterConstraintClauseSyntax first = ConstraintClauses.First();

            SyntaxToken token = first.WhereKeyword.GetPreviousToken();

            SyntaxTriviaList trivia = token.TrailingTrivia.EmptyIfWhitespace()
                .AddRange(first.GetLeadingTrivia().EmptyIfWhitespace())
                .AddRange(ConstraintClauses.Last().GetTrailingTrivia());

            return Create(Declaration.ReplaceToken(token, token.WithTrailingTrivia(trivia)))
                .WithConstraintClauses(default(SyntaxList<TypeParameterConstraintClauseSyntax>));
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (Declaration == null)
                throw new InvalidOperationException($"{nameof(GenericInfo)} is not initalized.");
        }

        /// <summary>
        /// Returns the string representation of the underlying syntax, not including its leading and trailing trivia.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Declaration?.ToString() ?? "";
        }

        /// <summary>
        /// Determines whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. </param>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            return obj is GenericInfo other && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(GenericInfo other)
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

        public static bool operator ==(GenericInfo info1, GenericInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(GenericInfo info1, GenericInfo info2)
        {
            return !(info1 == info2);
        }
    }
}