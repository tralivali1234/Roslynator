// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class ReplaceInterpolatedStringWithStringLiteralAnalysis
    {
        public static bool CanRefactor(InterpolatedStringExpressionSyntax interpolatedString)
        {
            SyntaxList<InterpolatedStringContentSyntax> contents = interpolatedString.Contents;

            return contents.Count == 0
                || (contents.Count == 1 && contents[0].Kind() == SyntaxKind.InterpolatedStringText);
        }
    }
}
