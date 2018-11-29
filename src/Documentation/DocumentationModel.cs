﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Roslynator.Documentation
{
    public sealed class DocumentationModel
    {
        private readonly Func<ISymbol, bool> _isVisible;

        private readonly Dictionary<ISymbol, SymbolDocumentationData> _symbolData;

        private readonly Dictionary<IAssemblySymbol, XmlDocumentation> _xmlDocumentations;

        private ImmutableArray<string> _additionalXmlDocumentationPaths;

        private ImmutableArray<XmlDocumentation> _additionalXmlDocumentations;

        public DocumentationModel(
            Compilation compilation,
            IEnumerable<IAssemblySymbol> assemblies,
            Visibility visibility = Visibility.Public,
            IEnumerable<string> additionalXmlDocumentationPaths = null)
        {
            Compilation = compilation;
            Assemblies = ImmutableArray.CreateRange(assemblies);
            Visibility = visibility;

            _isVisible = GetIsVisible();
            _symbolData = new Dictionary<ISymbol, SymbolDocumentationData>();
            _xmlDocumentations = new Dictionary<IAssemblySymbol, XmlDocumentation>();

            if (additionalXmlDocumentationPaths != null)
                _additionalXmlDocumentationPaths = additionalXmlDocumentationPaths.ToImmutableArray();

            Func<ISymbol, bool> GetIsVisible()
            {
                switch (visibility)
                {
                    case Visibility.Public:
                        return f => f.IsPubliclyVisible();
                    case Visibility.Internal:
                        return f => f.IsPubliclyOrInternallyVisible();
                    case Visibility.Private:
                        return _ => true;
                    default:
                        throw new ArgumentException($"Unknown enum value '{visibility}'.", nameof(visibility));
                }
            }
        }

        public Compilation Compilation { get; }

        public ImmutableArray<IAssemblySymbol> Assemblies { get; }

        public string Language => Compilation.Language;

        public IEnumerable<MetadataReference> References => Compilation.References;

        internal IEnumerable<INamespaceSymbol> Namespaces
        {
            get
            {
                return Types
                    .Select(f => f.ContainingNamespace)
                    .Distinct(MetadataNameEqualityComparer<INamespaceSymbol>.Instance);
            }
        }

        public IEnumerable<INamedTypeSymbol> Types
        {
            get { return Assemblies.SelectMany(f => f.GetTypes(typeSymbol => IsVisible(typeSymbol))); }
        }

        public Visibility Visibility { get; }

        public bool IsVisible(ISymbol symbol) => _isVisible(symbol);

        public IEnumerable<IMethodSymbol> GetExtensionMethods()
        {
            foreach (INamedTypeSymbol typeSymbol in Types)
            {
                if (typeSymbol.MightContainExtensionMethods)
                {
                    foreach (ISymbol member in GetTypeModel(typeSymbol).Members)
                    {
                        if (member.Kind == SymbolKind.Method
                            && member.IsStatic
                            && IsVisible(member))
                        {
                            var methodSymbol = (IMethodSymbol)member;

                            if (methodSymbol.IsExtensionMethod)
                                yield return methodSymbol;
                        }
                    }
                }
            }
        }

        public IEnumerable<IMethodSymbol> GetExtensionMethods(INamedTypeSymbol typeSymbol)
        {
            foreach (INamedTypeSymbol symbol in Types)
            {
                if (symbol.MightContainExtensionMethods)
                {
                    foreach (ISymbol member in GetTypeModel(symbol).Members)
                    {
                        if (member.Kind == SymbolKind.Method
                            && member.IsStatic
                            && IsVisible(member))
                        {
                            var methodSymbol = (IMethodSymbol)member;

                            if (methodSymbol.IsExtensionMethod)
                            {
                                ITypeSymbol typeSymbol2 = GetExtendedType(methodSymbol);

                                if (typeSymbol == typeSymbol2)
                                    yield return methodSymbol;
                            }
                        }
                    }
                }
            }
        }

        public IEnumerable<INamedTypeSymbol> GetExtendedExternalTypes()
        {
            return Iterator().Distinct();

            IEnumerable<INamedTypeSymbol> Iterator()
            {
                foreach (IMethodSymbol methodSymbol in GetExtensionMethods())
                {
                    INamedTypeSymbol typeSymbol = GetExternalSymbol(methodSymbol);

                    if (typeSymbol != null)
                        yield return typeSymbol;
                }
            }

            INamedTypeSymbol GetExternalSymbol(IMethodSymbol methodSymbol)
            {
                INamedTypeSymbol type = GetExtendedType(methodSymbol);

                if (type == null)
                    return null;

                foreach (IAssemblySymbol assembly in Assemblies)
                {
                    if (type.ContainingAssembly == assembly)
                        return null;
                }

                return type;
            }
        }

        private static INamedTypeSymbol GetExtendedType(IMethodSymbol methodSymbol)
        {
            ITypeSymbol type = methodSymbol.Parameters[0].Type.OriginalDefinition;

            switch (type.Kind)
            {
                case SymbolKind.NamedType:
                    return (INamedTypeSymbol)type;
                case SymbolKind.TypeParameter:
                    return GetTypeParameterConstraintClass((ITypeParameterSymbol)type);
            }

            return null;

            INamedTypeSymbol GetTypeParameterConstraintClass(ITypeParameterSymbol typeParameter)
            {
                foreach (ITypeSymbol constraintType in typeParameter.ConstraintTypes)
                {
                    if (constraintType.TypeKind == TypeKind.Class)
                    {
                        return (INamedTypeSymbol)constraintType;
                    }
                    else if (constraintType.TypeKind == TypeKind.TypeParameter)
                    {
                        return GetTypeParameterConstraintClass((ITypeParameterSymbol)constraintType);
                    }
                }

                return null;
            }
        }

        public bool IsExternal(ISymbol symbol)
        {
            foreach (IAssemblySymbol assembly in Assemblies)
            {
                if (symbol.ContainingAssembly == assembly)
                    return false;
            }

            return true;
        }

        public TypeDocumentationModel GetTypeModel(INamedTypeSymbol typeSymbol)
        {
            if (_symbolData.TryGetValue(typeSymbol, out SymbolDocumentationData data)
                && data.Model != null)
            {
                return (TypeDocumentationModel)data.Model;
            }

            var typeModel = new TypeDocumentationModel(typeSymbol, this);

            _symbolData[typeSymbol] = data.WithModel(typeModel);

            return typeModel;
        }

        internal ISymbol GetFirstSymbolForDeclarationId(string id)
        {
            return DocumentationCommentId.GetFirstSymbolForDeclarationId(id, Compilation);
        }

        internal ISymbol GetFirstSymbolForReferenceId(string id)
        {
            return DocumentationCommentId.GetFirstSymbolForReferenceId(id, Compilation);
        }

        public SymbolXmlDocumentation GetXmlDocumentation(ISymbol symbol, string preferredCultureName = null)
        {
            if (_symbolData.TryGetValue(symbol, out SymbolDocumentationData data)
                && data.XmlDocumentation != null)
            {
                if (object.ReferenceEquals(data.XmlDocumentation, SymbolXmlDocumentation.Default))
                    return null;

                return data.XmlDocumentation;
            }

            IAssemblySymbol assembly = FindAssembly();

            if (assembly != null)
            {
                SymbolXmlDocumentation xmlDocumentation = GetXmlDocumentation(assembly, preferredCultureName)?.GetXmlDocumentation(symbol);

                if (xmlDocumentation != null)
                {
                    _symbolData[symbol] = data.WithXmlDocumentation(xmlDocumentation);
                    return xmlDocumentation;
                }
            }

            if (!_additionalXmlDocumentationPaths.IsDefault)
            {
                if (_additionalXmlDocumentations.IsDefault)
                {
                    _additionalXmlDocumentations = _additionalXmlDocumentationPaths
                        .Select(f => XmlDocumentation.Load(f))
                        .ToImmutableArray();
                }

                string commentId = symbol.GetDocumentationCommentId();

                foreach (XmlDocumentation xmlDocumentation in _additionalXmlDocumentations)
                {
                    SymbolXmlDocumentation documentation = xmlDocumentation.GetXmlDocumentation(symbol, commentId);

                    if (documentation != null)
                    {
                        _symbolData[symbol] = data.WithXmlDocumentation(documentation);
                        return documentation;
                    }
                }
            }

            _symbolData[symbol] = data.WithXmlDocumentation(SymbolXmlDocumentation.Default);
            return null;

            IAssemblySymbol FindAssembly()
            {
                foreach (IAssemblySymbol a in Assemblies)
                {
                    if (symbol.ContainingAssembly == a)
                        return a;
                }

                return null;
            }
        }

        private XmlDocumentation GetXmlDocumentation(IAssemblySymbol assembly, string preferredCultureName = null)
        {
            if (!_xmlDocumentations.TryGetValue(assembly, out XmlDocumentation xmlDocumentation))
            {
                if (Assemblies.Contains(assembly))
                {
                    var reference = Compilation.GetMetadataReference(assembly) as PortableExecutableReference;

                    string path = reference.FilePath;

                    if (preferredCultureName != null)
                    {
                        path = Path.GetDirectoryName(path);

                        path = Path.Combine(path, preferredCultureName);

                        if (Directory.Exists(path))
                        {
                            string fileName = Path.ChangeExtension(Path.GetFileNameWithoutExtension(reference.FilePath), "xml");

                            path = Path.Combine(path, fileName);

                            if (File.Exists(path))
                                xmlDocumentation = XmlDocumentation.Load(path);
                        }
                    }

                    if (xmlDocumentation == null)
                    {
                        path = Path.ChangeExtension(reference.FilePath, "xml");

                        if (File.Exists(path))
                            xmlDocumentation = XmlDocumentation.Load(path);
                    }
                }

                _xmlDocumentations[assembly] = xmlDocumentation;
            }

            return xmlDocumentation;
        }

        private readonly struct SymbolDocumentationData
        {
            public SymbolDocumentationData(
                object model,
                SymbolXmlDocumentation xmlDocumentation)
            {
                Model = model;
                XmlDocumentation = xmlDocumentation;
            }

            public object Model { get; }

            public SymbolXmlDocumentation XmlDocumentation { get; }

            public SymbolDocumentationData WithModel(object model)
            {
                return new SymbolDocumentationData(model, XmlDocumentation);
            }

            public SymbolDocumentationData WithXmlDocumentation(SymbolXmlDocumentation xmlDocumentation)
            {
                return new SymbolDocumentationData(Model, xmlDocumentation);
            }
        }
    }
}
