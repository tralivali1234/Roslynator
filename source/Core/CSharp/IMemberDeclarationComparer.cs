// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp
{
    /// <summary>
    /// Comparer for <see cref="MemberDeclarationSyntax"/>.
    /// </summary>
    public interface IMemberDeclarationComparer : IComparer<MemberDeclarationSyntax>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        int GetInsertIndex(SyntaxList<MemberDeclarationSyntax> members, MemberDeclarationSyntax member);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <param name="memberKind"></param>
        /// <returns></returns>
        int GetInsertIndex(SyntaxList<MemberDeclarationSyntax> members, SyntaxKind memberKind);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <param name="isConst"></param>
        /// <returns></returns>
        int GetFieldInsertIndex(SyntaxList<MemberDeclarationSyntax> members, bool isConst);
    }
}
