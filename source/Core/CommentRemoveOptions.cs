// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator
{
    [Flags]
    public enum CommentRemoveOptions
    {
        None = 0,
        SingleLineComment = 1,
        MultiLineComment = 2,
        NonDocumentationComment = SingleLineComment | MultiLineComment,
        SingleLineDocumentationComment = 4,
        MultiLineDocumentationComment = 8,
        DocumentationComment = SingleLineDocumentationComment | MultiLineDocumentationComment,
        All = NonDocumentationComment |  DocumentationComment
    }
}
