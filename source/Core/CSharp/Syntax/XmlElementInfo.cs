// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    public struct XmlElementInfo : IEquatable<XmlElementInfo>
    {
        private XmlElementInfo(XmlNodeSyntax element, string localName, XmlElementKind elementKind)
        {
            Element = element;
            LocalName = localName;
            ElementKind = elementKind;
        }

        private static XmlElementInfo Default { get; } = new XmlElementInfo();

        public XmlNodeSyntax Element { get; }

        public string LocalName { get; }

        public XmlElementKind ElementKind { get; }

        public SyntaxKind Kind
        {
            get { return Element?.Kind() ?? SyntaxKind.None; }
        }

        public bool IsXmlElement
        {
            get { return Kind == SyntaxKind.XmlElement; }
        }

        //TODO: IsEmptyElement + smazat IsXmlElement
        public bool IsXmlEmptyElement
        {
            get { return Kind == SyntaxKind.XmlEmptyElement; }
        }

        public bool Success
        {
            get { return Element != null; }
        }

        internal static XmlElementInfo Create(XmlNodeSyntax node)
        {
            switch (node)
            {
                case XmlElementSyntax element:
                    {
                        string localName = element.StartTag?.Name?.LocalName.ValueText;

                        if (localName == null)
                            return Default;

                        return new XmlElementInfo(element, localName, GetXmlElementKind(localName));
                    }
                case XmlEmptyElementSyntax element:
                    {
                        string localName = element.Name?.LocalName.ValueText;

                        if (localName == null)
                            return Default;

                        return new XmlElementInfo(element, localName, GetXmlElementKind(localName));
                    }
            }

            return Default;
        }

        private static XmlElementKind GetXmlElementKind(string localName)
        {
            switch (localName)
            {
                case "include":
                case "INCLUDE":
                    return XmlElementKind.Include;
                case "exclude":
                case "EXCLUDE":
                    return XmlElementKind.Exclude;
                case "inheritdoc":
                case "INHERITDOC":
                    return XmlElementKind.InheritDoc;
                case "summary":
                case "SUMMARY":
                    return XmlElementKind.Summary;
                case "exception":
                case "EXCEPTION":
                    return XmlElementKind.Exception;
                default:
                    return XmlElementKind.None;
            }
        }

        internal bool IsLocalName(string localName, StringComparison comparison = StringComparison.Ordinal)
        {
            return string.Equals(LocalName, localName, comparison);
        }

        internal bool IsLocalName(string localName1, string localName2, StringComparison comparison = StringComparison.Ordinal)
        {
            return IsLocalName(localName1, comparison)
                || IsLocalName(localName2, comparison);
        }

        public override bool Equals(object obj)
        {
            return obj is XmlElementInfo other && Equals(other);
        }

        public bool Equals(XmlElementInfo other)
        {
            return EqualityComparer<XmlNodeSyntax>.Default.Equals(Element, other.Element);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<XmlNodeSyntax>.Default.GetHashCode(Element);
        }

        public static bool operator ==(XmlElementInfo info1, XmlElementInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(XmlElementInfo info1, XmlElementInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
