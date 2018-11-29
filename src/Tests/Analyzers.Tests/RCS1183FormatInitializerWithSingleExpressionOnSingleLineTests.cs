﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1183FormatInitializerWithSingleExpressionOnSingleLineTests : AbstractCSharpCodeFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.FormatInitializerWithSingleExpressionOnSingleLine;

        public override DiagnosticAnalyzer Analyzer { get; } = new FormatInitializerWithSingleExpressionOnSingleLineAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new InitializerCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_ObjectInitializer()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    object P { get; set; }

    void M()
    {
        var x = new C()
        [|{
            P = null
        }|];
    }
}
", @"
class C
{
    object P { get; set; }

    void M()
    {
        var x = new C() { P = null };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_ObjectInitializer_TrailingComma()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    object P { get; set; }

    void M()
    {
        var x = new C()
        [|{
            P = null,
        }|];
    }
}
", @"
class C
{
    object P { get; set; }

    void M()
    {
        var x = new C() { P = null };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_ObjectInitializer_TrailingComma2()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    public C P { get; set; }

    void M()
    {
        var items = new List<C>()
        {
            new C
            {
                P =
                new C()
            },
            new C
            [|{
                P = new C(),
            }|],
        };
    }
}
", @"
using System.Collections.Generic;

class C
{
    public C P { get; set; }

    void M()
    {
        var items = new List<C>()
        {
            new C
            {
                P =
                new C()
            },
            new C { P = new C() },
        };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_ObjectInitializer_AssignmentExpression()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    C P1 { get; set; }

    string P2 { get; set; }

    void M()
    {
        var x = new C()
        {
            P2 = null,
            P1 =
            [|{
                P2 = null
            }|]
        };
    }
}
", @"
class C
{
    C P1 { get; set; }

    string P2 { get; set; }

    void M()
    {
        var x = new C()
        {
            P2 = null,
            P1 = { P2 = null }
        };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_ObjectInitializer_TrailingComma_AssignmentExpression()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    C P1 { get; set; }

    string P2 { get; set; }

    void M()
    {
        var x = new C()
        {
            P2 = null,
            P1 =
            [|{
                P2 = null,
            }|]
        };
    }
}
", @"
class C
{
    C P1 { get; set; }

    string P2 { get; set; }

    void M()
    {
        var x = new C()
        {
            P2 = null,
            P1 = { P2 = null }
        };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_DictionaryInitializer()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    void M()
    {
        var dic = new Dictionary<int, string>()
        [|{
            { 0, null }
        }|];
    }
}
", @"
using System.Collections.Generic;

class C
{
    void M()
    {
        var dic = new Dictionary<int, string>() { { 0, null } };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_DictionaryInitializer_CSharp6()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    void M()
    {
        var dic = new Dictionary<int, string>()
        [|{
            [0] = null
        }|];
    }
}
", @"
using System.Collections.Generic;

class C
{
    void M()
    {
        var dic = new Dictionary<int, string>() { [0] = null };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_CollectionInitializer()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    void M()
    {
        var items = new List<string>()
        [|{
            null
        }|];
    }
}
", @"
using System.Collections.Generic;

class C
{
    void M()
    {
        var items = new List<string>() { null };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_ArrayInitializer()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        var items = new C[]
        [|{
            null
        }|];
    }
}
", @"
class C
{
    void M()
    {
        var items = new C[] { null };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_ArrayInitializer_Field()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string[] _f =
    [|{
        null
    }|];
}
", @"
class C
{
    string[] _f = { null };
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_ArrayInitializer_TrailingComma_Field()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string[] _f =
    [|{
        null,
    }|];
}
", @"
class C
{
    string[] _f = { null };
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_ArrayInitializer_Local()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string[] x =
        [|{
            null
        }|];
    }
}
", @"
class C
{
    void M()
    {
        string[] x = { null };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task Test_ArrayInitializer_TrailingComma_Local()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string[] x =
        [|{
            null,
        }|];
    }
}
", @"
class C
{
    void M()
    {
        string[] x = { null };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task TestNoDiagnostic_MultipleExpressions()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    object P1 { get; set; }
    object P2 { get; set; }

    void M()
    {
        var x = new C()
        {
            P1 = null,
            P2 = null
        };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task TestNoDiagnostic_MultilineExpression()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    object P1 { get; set; }
    object P2 { get; set; }

    void M()
    {
        var x = new C()
        {
            P1
                = null
        };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.FormatInitializerWithSingleExpressionOnSingleLine)]
        public async Task TestNoDiagnostic_InitializerContainsComment()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    object P1 { get; set; }
    object P2 { get; set; }

    void M()
    {
        var x = new C()
        { // ...
            P1 = null
        };
    }
}
");
        }
    }
}
