// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.Documentation
{
    //TODO: ren
    internal static class DocumentationCommentProvider
    {
        private static readonly Regex _commentedEmptyLineRegex = new Regex(@"^///\s*(\r?\n|$)", RegexOptions.Multiline);

        private static SyntaxTrivia Empty => default(SyntaxTrivia);

        public static SyntaxTrivia GetDocumentationCommentTrivia(ISymbol symbol, SemanticModel semanticModel, int position, CancellationToken cancellationToken = default(CancellationToken))
        {
            string xml = symbol.GetDocumentationCommentXml(cancellationToken: cancellationToken);

            if (string.IsNullOrEmpty(xml))
                return Empty;

            string innerXml = GetInnerXml(xml);

            Debug.Assert(innerXml != null, xml);

            if (innerXml == null)
                return Empty;

            string innerXmlWithSlashes = AddSlashes(innerXml.TrimEnd());

            SyntaxTriviaList leadingTrivia = ParseLeadingTrivia(innerXmlWithSlashes);

            if (leadingTrivia.Count != 1)
                return Empty;

            SyntaxTrivia trivia = leadingTrivia.First();

            if (trivia.Kind() != SyntaxKind.SingleLineDocumentationCommentTrivia)
                return Empty;

            if (!trivia.HasStructure)
                return Empty;

            SyntaxNode structure = trivia.GetStructure();

            if (structure.Kind() != SyntaxKind.SingleLineDocumentationCommentTrivia)
                return Empty;

            var commentTrivia = (DocumentationCommentTriviaSyntax)structure;

            var rewriter = new DocumentationCommentTriviaRewriter(position, semanticModel);

            // Remove T: from cref attribute and replace `1 with {T}
            commentTrivia = (DocumentationCommentTriviaSyntax)rewriter.VisitDocumentationCommentTrivia(commentTrivia);

            // Remove <filterpriority> element
            commentTrivia = RemoveFilterPriorityElement(commentTrivia);

            string text = commentTrivia.ToFullString();

            // Remove /// from empty lines
            text = _commentedEmptyLineRegex.Replace(text, "");

            leadingTrivia = ParseLeadingTrivia(text);

            if (leadingTrivia.Count == 1)
                return leadingTrivia.First();

            return Empty;
        }

        private static DocumentationCommentTriviaSyntax RemoveFilterPriorityElement(DocumentationCommentTriviaSyntax commentTrivia)
        {
            SyntaxList<XmlNodeSyntax> content = commentTrivia.Content;

            for (int i = content.Count - 1; i >= 0; i--)
            {
                XmlNodeSyntax xmlNode = content[i];

                if (xmlNode.IsKind(SyntaxKind.XmlElement))
                {
                    var xmlElement = (XmlElementSyntax)xmlNode;

                    if (xmlElement.IsLocalName("filterpriority", StringComparison.OrdinalIgnoreCase))
                        content = content.RemoveAt(i);
                }
            }

            return commentTrivia.WithContent(content);
        }

        private static string GetInnerXml(string comment)
        {
            using (var sr = new StringReader(comment))
            {
                var settings = new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Fragment };

                using (XmlReader reader = XmlReader.Create(sr, settings))
                {
                    if (reader.Read()
                        && reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "member":
                            case "doc":
                                {
                                    try
                                    {
                                        return reader.ReadInnerXml();
                                    }
                                    catch (XmlException ex)
                                    {
                                        Debug.Fail(ex.ToString());
                                        return null;
                                    }
                                }
                            default:
                                {
                                    Debug.Fail(reader.Name);
                                    return null;
                                }
                        }
                    }
                }
            }

            return null;
        }

        private static string AddSlashes(string innerXml)
        {
            StringBuilder sb = StringBuilderCache.GetInstance();

            string indent = null;

            using (var sr = new StringReader(innerXml))
            {
                string s = null;

                while ((s = sr.ReadLine()) != null)
                {
                    if (s.Length > 0)
                    {
                        indent = indent ?? Regex.Match(s, "^ *").Value;

                        sb.Append("/// ");
                        s = Regex.Replace(s, $"^{indent}", "");

                        sb.AppendLine(s);
                    }
                }
            }

            return StringBuilderCache.GetStringAndFree(sb);
        }
    }
}
