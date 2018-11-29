﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1226AddParagraphToDocumentationCommentTests : AbstractCSharpCodeFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.AddParagraphToDocumentationComment;

        public override DiagnosticAnalyzer Analyzer { get; } = new AddParagraphToDocumentationCommentAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new AddParagraphToDocumentationCommentCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task Test_TwoParagraphs_Summary()
        {
            await VerifyDiagnosticAndFixAsync(@"
namespace N
{
    /// <summary>
    ///[| a
    /// 
    /// b|]
    /// </summary>
    class C
    {
    }
}
", @"
namespace N
{
    /// <summary>
    /// <para>a</para>
    /// <para>b</para>
    /// </summary>
    class C
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task Test_TwoParagraphs_Returns()
        {
            await VerifyDiagnosticAndFixAsync(@"
namespace N
{
    /// <summary></summary>
    /// <returns>
    ///[| a
    /// 
    /// b|]
    /// </returns>
    class C
    {
    }
}
", @"
namespace N
{
    /// <summary></summary>
    /// <returns>
    /// <para>a</para>
    /// <para>b</para>
    /// </returns>
    class C
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task Test_TwoParagraphs_Remarks()
        {
            await VerifyDiagnosticAndFixAsync(@"
namespace N
{
    /// <summary></summary>
    /// <remarks>
    ///[| a
    /// 
    /// b|]
    /// </remarks>
    class C
    {
    }
}
", @"
namespace N
{
    /// <summary></summary>
    /// <remarks>
    /// <para>a</para>
    /// <para>b</para>
    /// </remarks>
    class C
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task Test_TwoParagraphs_Multiline()
        {
            await VerifyDiagnosticAndFixAsync(@"
namespace N
{
    /// <summary>
    ///[| a
    /// b
    /// 
    /// c
    /// d|]
    /// </summary>
    class C
    {
    }
}
", @"
namespace N
{
    /// <summary>
    /// <para>
    /// a
    /// b
    /// </para>
    /// <para>
    /// c
    /// d
    /// </para>
    /// </summary>
    class C
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task Test_TwoParagraphs_Multiline2()
        {
            await VerifyDiagnosticAndFixAsync(@"
namespace N
{
    /// <summary>
    ///[| a
    /// b
    /// 
    /// c|]
    /// </summary>
    class C
    {
    }
}
", @"
namespace N
{
    /// <summary>
    /// <para>
    /// a
    /// b
    /// </para>
    /// <para>c</para>
    /// </summary>
    class C
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task Test_TwoParagraphs_Multiline3()
        {
            await VerifyDiagnosticAndFixAsync(@"
namespace N
{
    /// <summary>
    ///[| a
    /// 
    /// c
    /// d|]
    /// </summary>
    class C
    {
    }
}
", @"
namespace N
{
    /// <summary>
    /// <para>a</para>
    /// <para>
    /// c
    /// d
    /// </para>
    /// </summary>
    class C
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task Test_ThreeParagraphs()
        {
            await VerifyDiagnosticAndFixAsync(@"
namespace N
{
    /// <summary>
    ///[| a
    /// 
    /// b|]
    /// 
    /// c
    /// </summary>
    class C
    {
    }
}
", @"
namespace N
{
    /// <summary>
    /// <para>a</para>
    /// <para>b</para>
    /// <para>c</para>
    /// </summary>
    class C
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task Test_ThreeParagraphs_Multiline()
        {
            await VerifyDiagnosticAndFixAsync(@"
namespace N
{
    /// <summary>
    ///[| a
    /// b
    /// 
    /// c
    /// d|]
    /// 
    /// e
    /// f
    /// </summary>
    class C
    {
    }
}
", @"
namespace N
{
    /// <summary>
    /// <para>
    /// a
    /// b
    /// </para>
    /// <para>
    /// c
    /// d
    /// </para>
    /// <para>
    /// e
    /// f
    /// </para>
    /// </summary>
    class C
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task Test_TwoParagraphs_ElementsOnly()
        {
            await VerifyDiagnosticAndFixAsync(@"
namespace N
{
    /// <summary>
    /// [|<c></c>
    /// <c></c>
    /// 
    /// <code>
    /// </code>|]
    /// </summary>
    class C
    {
    }
}
", @"
namespace N
{
    /// <summary>
    /// <para>
    /// <c></c>
    /// <c></c>
    /// </para>
    /// <para>
    /// <code>
    /// </code>
    /// </para>
    /// </summary>
    class C
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task Test_ClassWithAttribute()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;

namespace N
{
    /// <summary>
    ///[| a
    /// 
    /// b|]
    /// </summary>
    [Obsolete]
    class C
    {
    }
}
", @"
using System;

namespace N
{
    /// <summary>
    /// <para>a</para>
    /// <para>b</para>
    /// </summary>
    [Obsolete]
    class C
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task TestNoDiagnostic_SimpleComment()
        {
            await VerifyNoDiagnosticAsync(@"
/// <summary>
/// a
/// </summary>
class C
{
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task TestNoDiagnostic_NoEmptyLine()
        {
            await VerifyNoDiagnosticAsync(@"
/// <summary>
/// a
/// b
/// </summary>
class C
{
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddParagraphToDocumentationComment)]
        public async Task TestNoDiagnostic_MissingEndTag()
        {
            await VerifyNoDiagnosticAsync(@"
/// <summary>
/// x
///
/// <a>x
///
/// </summary>
class C
{
}
");
        }
    }
}
