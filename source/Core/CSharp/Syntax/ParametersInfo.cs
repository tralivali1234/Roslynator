// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Roslynator.CSharp.Syntax.SyntaxInfoHelpers;

namespace Roslynator.CSharp.Syntax
{
    internal readonly struct ParametersInfo : IEquatable<ParametersInfo>
    {
        public ParametersInfo(TypeParameterListSyntax typeParameterList, ParameterSyntax parameter, CSharpSyntaxNode body)
        {
            TypeParameterList = typeParameterList;
            Body = body;
            Parameter = parameter;
            ParameterList = default(BaseParameterListSyntax);
        }

        public ParametersInfo(TypeParameterListSyntax typeParameterList, BaseParameterListSyntax parameterList, CSharpSyntaxNode body)
        {
            TypeParameterList = typeParameterList;
            Body = body;
            Parameter = default(ParameterSyntax);
            ParameterList = parameterList;
        }

        private static ParametersInfo Default { get; } = new ParametersInfo();

        public SyntaxNode Node
        {
            get { return ParameterList?.Parent ?? Parameter?.Parent; }
        }

        public TypeParameterListSyntax TypeParameterList { get; }

        public SeparatedSyntaxList<TypeParameterSyntax> TypeParameters
        {
            get { return TypeParameterList?.Parameters ?? default(SeparatedSyntaxList<TypeParameterSyntax>); }
        }

        public ParameterSyntax Parameter { get; }

        public BaseParameterListSyntax ParameterList { get; }

        public SeparatedSyntaxList<ParameterSyntax> Parameters
        {
            get { return ParameterList?.Parameters ?? default(SeparatedSyntaxList<ParameterSyntax>); }
        }

        public CSharpSyntaxNode Body { get; }

        public bool Success
        {
            get { return Node != null; }
        }

        internal static ParametersInfo Create(ConstructorDeclarationSyntax constructorDeclaration, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = constructorDeclaration.ParameterList;

            if (!CheckParameterList(parameterList, allowMissing))
                return Default;

            CSharpSyntaxNode body = constructorDeclaration.BodyOrExpressionBody();

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(default(TypeParameterListSyntax), parameterList, body);
        }

        internal static ParametersInfo Create(MethodDeclarationSyntax methodDeclaration, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = methodDeclaration.ParameterList;

            if (!Check(parameterList, allowMissing))
                return Default;

            SeparatedSyntaxList<ParameterSyntax> parameters = parameterList.Parameters;

            if (!CheckParameters(parameters, allowMissing))
                return Default;

            TypeParameterListSyntax typeParameterList = methodDeclaration.TypeParameterList;
            SeparatedSyntaxList<TypeParameterSyntax> typeParameters = typeParameterList?.Parameters ?? default(SeparatedSyntaxList<TypeParameterSyntax>);

            if (!CheckTypeParameters(typeParameters, allowMissing))
                return Default;

            if (!parameters.Any()
                && !typeParameters.Any())
            {
                return Default;
            }

            CSharpSyntaxNode body = methodDeclaration.BodyOrExpressionBody();

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(typeParameterList, parameterList, body);
        }

        internal static ParametersInfo Create(OperatorDeclarationSyntax operatorDeclaration, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = operatorDeclaration.ParameterList;

            if (!CheckParameterList(parameterList, allowMissing))
                return Default;

            CSharpSyntaxNode body = operatorDeclaration.BodyOrExpressionBody();

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(default(TypeParameterListSyntax), parameterList, body);
        }

        internal static ParametersInfo Create(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = conversionOperatorDeclaration.ParameterList;

            if (!CheckParameterList(parameterList, allowMissing))
                return Default;

            CSharpSyntaxNode body = conversionOperatorDeclaration.BodyOrExpressionBody();

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(default(TypeParameterListSyntax), parameterList, body);
        }

        internal static ParametersInfo Create(DelegateDeclarationSyntax delegateDeclaration, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = delegateDeclaration.ParameterList;

            if (!Check(parameterList, allowMissing))
                return Default;

            SeparatedSyntaxList<ParameterSyntax> parameters = parameterList.Parameters;

            if (!CheckParameters(parameters, allowMissing))
                return Default;

            TypeParameterListSyntax typeParameterList = delegateDeclaration.TypeParameterList;
            SeparatedSyntaxList<TypeParameterSyntax> typeParameters = typeParameterList?.Parameters ?? default(SeparatedSyntaxList<TypeParameterSyntax>);

            if (!CheckTypeParameters(typeParameters, allowMissing))
                return Default;

            if (!parameters.Any()
                && !typeParameters.Any())
            {
                return Default;
            }

            return new ParametersInfo(typeParameterList, parameterList, default(CSharpSyntaxNode));
        }

        internal static ParametersInfo Create(LocalFunctionStatementSyntax localFunction, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = localFunction.ParameterList;

            if (!Check(parameterList, allowMissing))
                return Default;

            SeparatedSyntaxList<ParameterSyntax> parameters = parameterList.Parameters;

            if (!CheckParameters(parameters, allowMissing))
                return Default;

            TypeParameterListSyntax typeParameterList = localFunction.TypeParameterList;
            SeparatedSyntaxList<TypeParameterSyntax> typeParameters = typeParameterList?.Parameters ?? default(SeparatedSyntaxList<TypeParameterSyntax>);

            if (!CheckTypeParameters(typeParameters, allowMissing))
                return Default;

            if (!parameters.Any()
                && !typeParameters.Any())
            {
                return Default;
            }

            CSharpSyntaxNode body = localFunction.BodyOrExpressionBody();

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(typeParameterList, parameterList, body);
        }

        internal static ParametersInfo Create(IndexerDeclarationSyntax indexerDeclaration, bool allowMissing = false)
        {
            BaseParameterListSyntax parameterList = indexerDeclaration.ParameterList;

            if (!CheckParameterList(parameterList, allowMissing))
                return Default;

            CSharpSyntaxNode body = indexerDeclaration.AccessorList ?? (CSharpSyntaxNode)indexerDeclaration.ExpressionBody;

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(default(TypeParameterListSyntax), parameterList, body);
        }

        internal static ParametersInfo Create(SimpleLambdaExpressionSyntax simpleLambda, bool allowMissing = false)
        {
            ParameterSyntax parameter = simpleLambda.Parameter;

            if (!Check(parameter, allowMissing))
                return Default;

            CSharpSyntaxNode body = simpleLambda.Body;

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(default(TypeParameterListSyntax), parameter, body);
        }

        internal static ParametersInfo Create(ParenthesizedLambdaExpressionSyntax parenthesizedLambda, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = parenthesizedLambda.ParameterList;

            if (!CheckParameterList(parameterList, allowMissing))
                return Default;

            CSharpSyntaxNode body = parenthesizedLambda.Body;

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(default(TypeParameterListSyntax), parameterList, body);
        }

        internal static ParametersInfo Create(AnonymousMethodExpressionSyntax anonymousMethod, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = anonymousMethod.ParameterList;

            if (!CheckParameterList(parameterList, allowMissing))
                return Default;

            CSharpSyntaxNode body = anonymousMethod.Body;

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(default(TypeParameterListSyntax), parameterList, body);
        }

        private static bool CheckParameterList(BaseParameterListSyntax parameterList, bool allowMissing)
        {
            if (!Check(parameterList, allowMissing))
                return false;

            SeparatedSyntaxList<ParameterSyntax> parameters = parameterList.Parameters;

            if (!parameters.Any())
                return false;

            foreach (ParameterSyntax parameter in parameters)
            {
                if (!Check(parameter, allowMissing))
                    return false;
            }

            return true;
        }

        private static bool CheckTypeParameters(SeparatedSyntaxList<TypeParameterSyntax> typeParameters, bool allowMissing)
        {
            foreach (TypeParameterSyntax typeParameter in typeParameters)
            {
                if (!Check(typeParameter, allowMissing))
                    return false;
            }

            return true;
        }

        private static bool CheckParameters(SeparatedSyntaxList<ParameterSyntax> parameters, bool allowMissing)
        {
            foreach (ParameterSyntax parameter in parameters)
            {
                if (!Check(parameter, allowMissing))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return Node?.ToString() ?? "";
        }

        public override bool Equals(object obj)
        {
            return obj is ParametersInfo other && Equals(other);
        }

        public bool Equals(ParametersInfo other)
        {
            return EqualityComparer<SyntaxNode>.Default.Equals(Node, other.Node);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<SyntaxNode>.Default.GetHashCode(Node);
        }

        public static bool operator ==(ParametersInfo info1, ParametersInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(ParametersInfo info1, ParametersInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
