// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Roslynator.CSharp.Documentation
{
    //TODO: DocumentationCommentSettings
    public class DocumentationCommentGeneratorSettings
    {
        public DocumentationCommentGeneratorSettings(
            IEnumerable<string> comments = null,
            string indentation = null,
            bool singleLineSummary = false,
            bool returns = true
            )
        {
            Comments = (comments != null) ? ImmutableArray.CreateRange(comments) : ImmutableArray<string>.Empty;
            Indentation = indentation ?? "";
            SingleLineSummary = singleLineSummary;
            Returns = returns;
        }

        public static DocumentationCommentGeneratorSettings Default { get; } = new DocumentationCommentGeneratorSettings();

        //TODO: Lines
        public ImmutableArray<string> Comments { get; }

        public string Indentation { get; }

        public bool SingleLineSummary { get; }

        public bool Returns { get; }

        public DocumentationCommentGeneratorSettings WithComments(IEnumerable<string> comments)
        {
            return new DocumentationCommentGeneratorSettings(
                comments: comments,
                indentation: Indentation,
                singleLineSummary: SingleLineSummary,
                returns: Returns);
        }

        public DocumentationCommentGeneratorSettings WithIndentation(string indentation)
        {
            return new DocumentationCommentGeneratorSettings(
                comments: Comments,
                indentation: indentation,
                singleLineSummary: SingleLineSummary,
                returns: Returns);
        }

        public DocumentationCommentGeneratorSettings WithSingleLineSummary(bool singleLineSummary)
        {
            return new DocumentationCommentGeneratorSettings(
                comments: Comments,
                indentation: Indentation,
                singleLineSummary: singleLineSummary,
                returns: Returns);
        }

        public DocumentationCommentGeneratorSettings WithReturns(bool returns)
        {
            return new DocumentationCommentGeneratorSettings(
                comments: Comments,
                indentation: Indentation,
                singleLineSummary: SingleLineSummary,
                returns: returns);
        }
    }
}
