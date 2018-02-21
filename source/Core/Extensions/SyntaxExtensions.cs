// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator
{
    /// <summary>
    /// 
    /// </summary>
    public static class SyntaxExtensions
    {
        #region SeparatedSyntaxList<T>
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        public static SeparatedSyntaxList<TNode> ReplaceAt<TNode>(this SeparatedSyntaxList<TNode> list, int index, TNode newNode) where TNode : SyntaxNode
        {
            return list.Replace(list[index], newNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsFirst<TNode>(this SeparatedSyntaxList<TNode> list, TNode node) where TNode : SyntaxNode
        {
            return list.Any()
                && list.First() == node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsLast<TNode>(this SeparatedSyntaxList<TNode> list, TNode node) where TNode : SyntaxNode
        {
            return list.Any()
                && list.Last() == node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Any<TNode>(this SeparatedSyntaxList<TNode> list, Func<TNode, bool> predicate) where TNode : SyntaxNode
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (TNode node in list)
            {
                if (predicate(node))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool All<TNode>(this SeparatedSyntaxList<TNode> list, Func<TNode, bool> predicate) where TNode : SyntaxNode
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (TNode node in list)
            {
                if (!predicate(node))
                    return false;
            }

            return true;
        }

        internal static TNode SingleOrDefault<TNode>(this SeparatedSyntaxList<TNode> list, bool shouldthrow) where TNode : SyntaxNode
        {
            return (shouldthrow) ? list.SingleOrDefault() : (list.Count == 1) ? list[0] : default(TNode);
        }

        internal static TNode LastButOne<TNode>(this SeparatedSyntaxList<TNode> list) where TNode : SyntaxNode
        {
            return list[list.Count - 2];
        }

        internal static TNode LastButOneOrDefault<TNode>(this SeparatedSyntaxList<TNode> list) where TNode : SyntaxNode
        {
            return (list.Count > 1) ? list.LastButOne() : default(TNode);
        }
        #endregion SeparatedSyntaxList<T>

        #region SyntaxList<T>
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        public static SyntaxList<TNode> ReplaceAt<TNode>(this SyntaxList<TNode> list, int index, TNode newNode) where TNode : SyntaxNode
        {
            return list.Replace(list[index], newNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsFirst<TNode>(this SyntaxList<TNode> list, TNode node) where TNode : SyntaxNode
        {
            return list.Any()
                && list.First() == node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsLast<TNode>(this SyntaxList<TNode> list, TNode node) where TNode : SyntaxNode
        {
            return list.Any()
                && list.Last() == node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Any<TNode>(this SyntaxList<TNode> list, Func<TNode, bool> predicate) where TNode : SyntaxNode
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (TNode node in list)
            {
                if (predicate(node))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool All<TNode>(this SyntaxList<TNode> list, Func<TNode, bool> predicate) where TNode : SyntaxNode
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (TNode node in list)
            {
                if (!predicate(node))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool Contains<TNode>(this SyntaxList<TNode> list, TNode node) where TNode : SyntaxNode
        {
            return list.IndexOf(node) != -1;
        }

        internal static TNode SingleOrDefault<TNode>(this SyntaxList<TNode> list, bool shouldThrow) where TNode : SyntaxNode
        {
            return (shouldThrow) ? list.SingleOrDefault() : ((list.Count == 1) ? list[0] : default(TNode));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool SpanContainsDirectives<TNode>(this SyntaxList<TNode> list) where TNode : SyntaxNode
        {
            int count = list.Count;

            if (count == 0)
                return false;

            if (count == 1)
                return list.First().SpanContainsDirectives();

            for (int i = 1; i < count - 1; i++)
            {
                if (list[i].ContainsDirectives)
                    return true;
            }

            return list.First().SpanOrTrailingTriviaContainsDirectives()
                || list.Last().SpanOrLeadingTriviaContainsDirectives();
        }

        internal static TNode LastButOne<TNode>(this SyntaxList<TNode> list) where TNode : SyntaxNode
        {
            return list[list.Count - 2];
        }

        internal static TNode LastButOneOrDefault<TNode>(this SyntaxList<TNode> list) where TNode : SyntaxNode
        {
            return (list.Count > 1) ? list.LastButOne() : default(TNode);
        }
        #endregion SyntaxList<T>

        #region SyntaxNode
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static SyntaxTriviaList GetLeadingAndTrailingTrivia(this SyntaxNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();
            SyntaxTriviaList trailingTrivia = node.GetTrailingTrivia();

            if (leadingTrivia.Any())
            {
                if (trailingTrivia.Any())
                    return leadingTrivia.AddRange(trailingTrivia);

                return leadingTrivia;
            }

            if (trailingTrivia.Any())
                return trailingTrivia;

            return SyntaxTriviaList.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static TNode PrependToLeadingTrivia<TNode>(this TNode node, IEnumerable<SyntaxTrivia> trivia) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (trivia == null)
                throw new ArgumentNullException(nameof(trivia));

            return node.WithLeadingTrivia(node.GetLeadingTrivia().InsertRange(0, trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static TNode PrependToLeadingTrivia<TNode>(this TNode node, SyntaxTrivia trivia) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.WithLeadingTrivia(node.GetLeadingTrivia().Insert(0, trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static TNode PrependToTrailingTrivia<TNode>(this TNode node, IEnumerable<SyntaxTrivia> trivia) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (trivia == null)
                throw new ArgumentNullException(nameof(trivia));

            return node.WithTrailingTrivia(node.GetTrailingTrivia().InsertRange(0, trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static TNode PrependToTrailingTrivia<TNode>(this TNode node, SyntaxTrivia trivia) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.WithTrailingTrivia(node.GetTrailingTrivia().Insert(0, trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static TNode AppendToLeadingTrivia<TNode>(this TNode node, IEnumerable<SyntaxTrivia> trivia) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (trivia == null)
                throw new ArgumentNullException(nameof(trivia));

            return node.WithLeadingTrivia(node.GetLeadingTrivia().AddRange(trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static TNode AppendToLeadingTrivia<TNode>(this TNode node, SyntaxTrivia trivia) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.WithLeadingTrivia(node.GetLeadingTrivia().Add(trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static TNode AppendToTrailingTrivia<TNode>(this TNode node, IEnumerable<SyntaxTrivia> trivia) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (trivia == null)
                throw new ArgumentNullException(nameof(trivia));

            return node.WithTrailingTrivia(node.GetTrailingTrivia().AddRange(trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static TNode AppendToTrailingTrivia<TNode>(this TNode node, SyntaxTrivia trivia) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.WithTrailingTrivia(node.GetTrailingTrivia().Add(trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool SpanContainsDirectives(this SyntaxNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.ContainsDirectives
                && !node.GetLeadingTrivia().Any(f => f.IsDirective)
                && !node.GetTrailingTrivia().Any(f => f.IsDirective);
        }

        internal static bool SpanOrLeadingTriviaContainsDirectives(this SyntaxNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.ContainsDirectives
                && !node.GetTrailingTrivia().Any(f => f.IsDirective);
        }

        internal static bool SpanOrTrailingTriviaContainsDirectives(this SyntaxNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.ContainsDirectives
                && !node.GetLeadingTrivia().Any(f => f.IsDirective);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public static bool ContainsDirectives(this SyntaxNode node, TextSpan span)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            //XTODO: optimize
            return node.ContainsDirectives
                && node.DescendantTrivia(span).Any(f => f.IsDirective);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static TNode WithTriviaFrom<TNode>(this TNode node, SyntaxToken token) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node
                .WithLeadingTrivia(token.LeadingTrivia)
                .WithTrailingTrivia(token.TrailingTrivia);
        }

        internal static int GetSpanStartLine(this SyntaxNode node, CancellationToken cancellationToken = default(CancellationToken))
        {
            return node.SyntaxTree.GetLineSpan(node.Span, cancellationToken).StartLine();
        }

        internal static int GetFullSpanStartLine(this SyntaxNode node, CancellationToken cancellationToken = default(CancellationToken))
        {
            return node.SyntaxTree.GetLineSpan(node.FullSpan, cancellationToken).StartLine();
        }

        internal static int GetSpanEndLine(this SyntaxNode node, CancellationToken cancellationToken = default(CancellationToken))
        {
            return node.SyntaxTree.GetLineSpan(node.Span, cancellationToken).EndLine();
        }

        internal static int GetFullSpanEndLine(this SyntaxNode node, CancellationToken cancellationToken = default(CancellationToken))
        {
            return node.SyntaxTree.GetLineSpan(node.FullSpan, cancellationToken).EndLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="predicate"></param>
        /// <param name="ascendOutOfTrivia"></param>
        /// <returns></returns>
        public static TNode FirstAncestor<TNode>(
            this SyntaxNode node,
            Func<TNode, bool> predicate = null,
            bool ascendOutOfTrivia = true) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.Parent?.FirstAncestorOrSelf(predicate, ascendOutOfTrivia);
        }

        internal static string ToString(this SyntaxNode node, TextSpan span)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            TextSpan nodeSpan = node.Span;

            TextSpan? intersection = nodeSpan.Intersection(span);

            if (intersection == null)
                throw new ArgumentException("Span has no intersection with node span.", nameof(span));

            span = intersection.Value;

            return node.ToString().Substring(span.Start - nodeSpan.Start, span.Length);
        }

        internal static TextSpan LeadingTriviaSpan(this SyntaxNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return TextSpan.FromBounds(node.FullSpan.Start, node.Span.Start);
        }

        internal static TextSpan TrailingTriviaSpan(this SyntaxNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return TextSpan.FromBounds(node.Span.End, node.FullSpan.End);
        }

        internal static TNode WithAdditionalAnnotationsIf<TNode>(this TNode node, bool condition, params SyntaxAnnotation[] annotations) where TNode : SyntaxNode
        {
            return (condition) ? node.WithAdditionalAnnotations(annotations) : node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="descendIntoChildren"></param>
        /// <param name="descendIntoTrivia"></param>
        /// <returns></returns>
        public static TNode FirstDescendant<TNode>(
            this SyntaxNode node,
            Func<SyntaxNode, bool> descendIntoChildren = null,
            bool descendIntoTrivia = false) where TNode : SyntaxNode
        {
            foreach (SyntaxNode descendant in node.DescendantNodes(descendIntoChildren: descendIntoChildren, descendIntoTrivia: descendIntoTrivia))
            {
                if (descendant is TNode tnode)
                    return tnode;
            }

            return default(TNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="span"></param>
        /// <param name="descendIntoChildren"></param>
        /// <param name="descendIntoTrivia"></param>
        /// <returns></returns>
        public static TNode FirstDescendant<TNode>(
            this SyntaxNode node,
            TextSpan span,
            Func<SyntaxNode, bool> descendIntoChildren = null,
            bool descendIntoTrivia = false) where TNode : SyntaxNode
        {
            foreach (SyntaxNode descendant in node.DescendantNodes(span, descendIntoChildren: descendIntoChildren, descendIntoTrivia: descendIntoTrivia))
            {
                if (descendant is TNode tnode)
                    return tnode;
            }

            return default(TNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="descendIntoChildren"></param>
        /// <param name="descendIntoTrivia"></param>
        /// <returns></returns>
        public static TNode FirstDescendantOrSelf<TNode>(
            this SyntaxNode node,
            Func<SyntaxNode, bool> descendIntoChildren = null,
            bool descendIntoTrivia = false) where TNode : SyntaxNode
        {
            foreach (SyntaxNode descendant in node.DescendantNodesAndSelf(descendIntoChildren: descendIntoChildren, descendIntoTrivia: descendIntoTrivia))
            {
                if (descendant is TNode tnode)
                    return tnode;
            }

            return default(TNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <param name="span"></param>
        /// <param name="descendIntoChildren"></param>
        /// <param name="descendIntoTrivia"></param>
        /// <returns></returns>
        public static TNode FirstDescendantOrSelf<TNode>(
            this SyntaxNode node,
            TextSpan span,
            Func<SyntaxNode, bool> descendIntoChildren = null,
            bool descendIntoTrivia = false) where TNode : SyntaxNode
        {
            foreach (SyntaxNode descendant in node.DescendantNodesAndSelf(span, descendIntoChildren: descendIntoChildren, descendIntoTrivia: descendIntoTrivia))
            {
                if (descendant is TNode tnode)
                    return tnode;
            }

            return default(TNode);
        }
        #endregion SyntaxNode

        #region SyntaxNodeOrToken
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeOrToken"></param>
        /// <returns></returns>
        public static SyntaxNodeOrToken WithoutTrivia(this SyntaxNodeOrToken nodeOrToken)
        {
            if (nodeOrToken.IsNode)
            {
                return nodeOrToken.AsNode().WithoutTrivia();
            }
            else
            {
                return nodeOrToken.AsToken().WithoutTrivia();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeOrToken"></param>
        /// <returns></returns>
        public static SyntaxNodeOrToken WithoutLeadingTrivia(this SyntaxNodeOrToken nodeOrToken)
        {
            if (nodeOrToken.IsNode)
            {
                return nodeOrToken.AsNode().WithoutLeadingTrivia();
            }
            else
            {
                return nodeOrToken.AsToken().WithoutLeadingTrivia();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeOrToken"></param>
        /// <returns></returns>
        public static SyntaxNodeOrToken WithoutTrailingTrivia(this SyntaxNodeOrToken nodeOrToken)
        {
            if (nodeOrToken.IsNode)
            {
                return nodeOrToken.AsNode().WithoutTrailingTrivia();
            }
            else
            {
                return nodeOrToken.AsToken().WithoutTrailingTrivia();
            }
        }
        #endregion SyntaxNodeOrToken

        #region SyntaxToken
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static SyntaxToken PrependToLeadingTrivia(this SyntaxToken token, IEnumerable<SyntaxTrivia> trivia)
        {
            if (trivia == null)
                throw new ArgumentNullException(nameof(trivia));

            return token.WithLeadingTrivia(token.LeadingTrivia.InsertRange(0, trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static SyntaxToken PrependToLeadingTrivia(this SyntaxToken token, SyntaxTrivia trivia)
        {
            return token.WithLeadingTrivia(token.LeadingTrivia.Insert(0, trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static SyntaxToken PrependToTrailingTrivia(this SyntaxToken token, IEnumerable<SyntaxTrivia> trivia)
        {
            if (trivia == null)
                throw new ArgumentNullException(nameof(trivia));

            return token.WithTrailingTrivia(token.TrailingTrivia.InsertRange(0, trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static SyntaxToken PrependToTrailingTrivia(this SyntaxToken token, SyntaxTrivia trivia)
        {
            return token.WithTrailingTrivia(token.TrailingTrivia.Insert(0, trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static SyntaxToken AppendToTrailingTrivia(this SyntaxToken token, IEnumerable<SyntaxTrivia> trivia)
        {
            if (trivia == null)
                throw new ArgumentNullException(nameof(trivia));

            return token.WithTrailingTrivia(token.TrailingTrivia.AddRange(trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static SyntaxToken AppendToTrailingTrivia(this SyntaxToken token, SyntaxTrivia trivia)
        {
            return token.WithTrailingTrivia(token.TrailingTrivia.Add(trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static SyntaxToken AppendToLeadingTrivia(this SyntaxToken token, IEnumerable<SyntaxTrivia> trivia)
        {
            if (trivia == null)
                throw new ArgumentNullException(nameof(trivia));

            return token.WithLeadingTrivia(token.LeadingTrivia.AddRange(trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="trivia"></param>
        /// <returns></returns>
        public static SyntaxToken AppendToLeadingTrivia(this SyntaxToken token, SyntaxTrivia trivia)
        {
            return token.WithLeadingTrivia(token.LeadingTrivia.Add(trivia));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static SyntaxTriviaList LeadingAndTrailingTrivia(this SyntaxToken token)
        {
            SyntaxTriviaList leadingTrivia = token.LeadingTrivia;
            SyntaxTriviaList trailingTrivia = token.TrailingTrivia;

            if (leadingTrivia.Any())
            {
                if (trailingTrivia.Any())
                    return leadingTrivia.AddRange(trailingTrivia);

                return leadingTrivia;
            }

            if (trailingTrivia.Any())
                return trailingTrivia;

            return SyntaxTriviaList.Empty;
        }

        internal static int GetSpanStartLine(this SyntaxToken token, CancellationToken cancellationToken = default(CancellationToken))
        {
            return token.SyntaxTree.GetLineSpan(token.Span, cancellationToken).StartLine();
        }

        internal static int GetFullSpanStartLine(this SyntaxToken token, CancellationToken cancellationToken = default(CancellationToken))
        {
            return token.SyntaxTree.GetLineSpan(token.FullSpan, cancellationToken).StartLine();
        }

        internal static int GetSpanEndLine(this SyntaxToken token, CancellationToken cancellationToken = default(CancellationToken))
        {
            return token.SyntaxTree.GetLineSpan(token.Span, cancellationToken).EndLine();
        }

        internal static int GetFullSpanEndLine(this SyntaxToken token, CancellationToken cancellationToken = default(CancellationToken))
        {
            return token.SyntaxTree.GetLineSpan(token.FullSpan, cancellationToken).EndLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static SyntaxToken WithoutLeadingTrivia(this SyntaxToken token)
        {
            return token.WithLeadingTrivia(default(SyntaxTriviaList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static SyntaxToken WithoutTrailingTrivia(this SyntaxToken token)
        {
            return token.WithTrailingTrivia(default(SyntaxTriviaList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static SyntaxToken WithTriviaFrom(this SyntaxToken token, SyntaxNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return token
                .WithLeadingTrivia(node.GetLeadingTrivia())
                .WithTrailingTrivia(node.GetTrailingTrivia());
        }

        internal static TextSpan LeadingTriviaSpan(this SyntaxToken token)
        {
            return TextSpan.FromBounds(token.FullSpan.Start, token.Span.Start);
        }

        internal static TextSpan TrailingTriviaSpan(this SyntaxToken token)
        {
            return TextSpan.FromBounds(token.Span.End, token.FullSpan.End);
        }
        #endregion SyntaxToken

        #region SyntaxTokenList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenList"></param>
        /// <param name="index"></param>
        /// <param name="newToken"></param>
        /// <returns></returns>
        public static SyntaxTokenList ReplaceAt(this SyntaxTokenList tokenList, int index, SyntaxToken newToken)
        {
            return tokenList.Replace(tokenList[index], newToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Any(this SyntaxTokenList list, Func<SyntaxToken, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (SyntaxToken token in list)
            {
                if (predicate(token))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool All(this SyntaxTokenList list, Func<SyntaxToken, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (SyntaxToken token in list)
            {
                if (!predicate(token))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool Contains(this SyntaxTokenList tokens, SyntaxToken token)
        {
            return tokens.IndexOf(token) != -1;
        }
        #endregion SyntaxTokenList

        #region SyntaxTrivia
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trivia"></param>
        /// <param name="triviaList"></param>
        /// <returns></returns>
        public static bool TryGetContainingList(this SyntaxTrivia trivia, out SyntaxTriviaList triviaList)
        {
            SyntaxToken token = trivia.Token;

            SyntaxTriviaList leadingTrivia = token.LeadingTrivia;

            int index = leadingTrivia.IndexOf(trivia);

            if (index != -1)
            {
                triviaList = leadingTrivia;
                return true;
            }

            SyntaxTriviaList trailingTrivia = token.TrailingTrivia;

            index = trailingTrivia.IndexOf(trivia);

            if (index != -1)
            {
                triviaList = trailingTrivia;
                return true;
            }

            triviaList = default(SyntaxTriviaList);
            return false;
        }

        internal static int GetSpanStartLine(this SyntaxTrivia trivia, CancellationToken cancellationToken = default(CancellationToken))
        {
            return trivia.SyntaxTree.GetLineSpan(trivia.Span, cancellationToken).StartLine();
        }

        internal static int GetFullSpanStartLine(this SyntaxTrivia trivia, CancellationToken cancellationToken = default(CancellationToken))
        {
            return trivia.SyntaxTree.GetLineSpan(trivia.FullSpan, cancellationToken).StartLine();
        }

        internal static int GetSpanEndLine(this SyntaxTrivia trivia, CancellationToken cancellationToken = default(CancellationToken))
        {
            return trivia.SyntaxTree.GetLineSpan(trivia.Span, cancellationToken).EndLine();
        }

        internal static int GetFullSpanEndLine(this SyntaxTrivia trivia, CancellationToken cancellationToken = default(CancellationToken))
        {
            return trivia.SyntaxTree.GetLineSpan(trivia.FullSpan, cancellationToken).EndLine();
        }

        internal static TextSpan LeadingTriviaSpan(this SyntaxTrivia trivia)
        {
            return TextSpan.FromBounds(trivia.FullSpan.Start, trivia.Span.Start);
        }

        internal static TextSpan TrailingTriviaSpan(this SyntaxTrivia trivia)
        {
            return TextSpan.FromBounds(trivia.Span.End, trivia.FullSpan.End);
        }
        #endregion SyntaxTrivia

        #region SyntaxTriviaList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="triviaList"></param>
        /// <param name="index"></param>
        /// <param name="newTrivia"></param>
        /// <returns></returns>
        public static SyntaxTriviaList ReplaceAt(this SyntaxTriviaList triviaList, int index, SyntaxTrivia newTrivia)
        {
            return triviaList.Replace(triviaList[index], newTrivia);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Any(this SyntaxTriviaList list, Func<SyntaxTrivia, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (SyntaxTrivia trivia in list)
            {
                if (predicate(trivia))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool All(this SyntaxTriviaList list, Func<SyntaxTrivia, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (SyntaxTrivia trivia in list)
            {
                if (!predicate(trivia))
                    return false;
            }

            return true;
        }
        #endregion SyntaxTriviaList
    }
}
