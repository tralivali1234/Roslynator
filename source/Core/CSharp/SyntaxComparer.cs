// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Roslynator.CSharp
{
    internal static class SyntaxComparer
    {
        public static bool AreEquivalent(
            SyntaxNode node1,
            SyntaxNode node2,
            bool disregardTrivia = true,
            bool topLevel = false)
        {
            if (disregardTrivia)
            {
                return SyntaxFactory.AreEquivalent(node1, node2, topLevel: topLevel);
            }
            else
            {
                if (node1 == null)
                {
                    return node2 == null;
                }

                return node1.IsEquivalentTo(node2, topLevel: topLevel);
            }
        }

        public static bool AreEquivalent(
            SyntaxNode node1,
            SyntaxNode node2,
            SyntaxNode node3,
            bool disregardTrivia = true,
            bool topLevel = false)
        {
            return AreEquivalent(node1, node2, disregardTrivia: disregardTrivia, topLevel: topLevel)
                && AreEquivalent(node1, node3, disregardTrivia: disregardTrivia, topLevel: topLevel);
        }
    }
}
