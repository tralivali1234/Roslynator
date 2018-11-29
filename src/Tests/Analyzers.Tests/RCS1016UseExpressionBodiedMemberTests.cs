﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1016UseExpressionBodiedMemberTests : AbstractCSharpCodeFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.UseExpressionBodiedMember;

        public override DiagnosticAnalyzer Analyzer { get; } = new UseExpressionBodiedMemberAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new UseExpressionBodiedMemberCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_Constructor()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    public C()
    [|{
        M();
    }|]

    void M() { }
}
", @"
class C
{
    public C() => M();

    void M() { }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_Destructor()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    ~C()
    [|{
        M();
    }|]

    void M() { }
}
", @"
class C
{
    ~C() => M();

    void M() { }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_Method()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M()
    [|{
        return null;
    }|]
}
", @"
class C
{
    string M() => null;
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_VoidMethod()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    [|{
        M();
    }|]
}
", @"
class C
{
    void M() => M();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_LocalFunction()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string LF()
        [|{
            return null;
        }|]
    }
}
", @"
class C
{
    void M()
    {
        string LF() => null;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_VoidLocalFunction()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        void LF()
        [|{
            M();
        }|]
    }
}
", @"
class C
{
    void M()
    {
        void LF() => M();
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_PropertyWithGetter()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string P
    [|{
        get { return null; }
    }|]
}
", @"
class C
{
    string P => null;
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_PropertyWithGetterAndSetter()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string _f;

    public string P
    {
        get [|{ return _f; }|]
        set [|{ _f = value; }|]
    }
}
", @"
class C
{
    string _f;

    public string P
    {
        get => _f;
        set => _f = value;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_IndexerWithGetter()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string this[int index]
    [|{
        get { return null; }
    }|]
}
", @"
class C
{
    string this[int index] => null;
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_IndexerWithGetterAndSetter()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string _f;

    string this[int index]
    {
        get [|{ return _f; }|]
        set [|{ _f = value; }|]
    }
}
", @"
class C
{
    string _f;

    string this[int index]
    {
        get => _f;
        set => _f = value;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_Operator()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    public static C operator !(C value)
    [|{
        return value;
    }|]
}
", @"
class C
{
    public static C operator !(C value) => value;
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_ConversionOperator()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    public static explicit operator C(string value)
    [|{
        return new C();
    }|]
}
", @"
class C
{
    public static explicit operator C(string value) => new C();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_Constructor_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    public C()
    [|{
        throw new System.NotImplementedException();
    }|]

    void M() { }
}
", @"
class C
{
    public C() => throw new System.NotImplementedException();

    void M() { }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_Destructor_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    ~C()
    [|{
        throw new System.NotImplementedException();
    }|]
}
", @"
class C
{
    ~C() => throw new System.NotImplementedException();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_Method_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M()
    [|{
        throw new System.NotImplementedException();
    }|]
}
", @"
class C
{
    string M() => throw new System.NotImplementedException();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_VoidMethod_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    [|{
        throw new System.NotImplementedException();
    }|]
}
", @"
class C
{
    void M() => throw new System.NotImplementedException();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_LocalFunction_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string LF()
        [|{
            throw new System.NotImplementedException();
        }|]
    }
}
", @"
class C
{
    void M()
    {
        string LF() => throw new System.NotImplementedException();
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_VoidLocalFunction_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        void LF()
        [|{
            throw new System.NotImplementedException();
        }|]
    }
}
", @"
class C
{
    void M()
    {
        void LF() => throw new System.NotImplementedException();
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_PropertyWithGetter_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string P
    [|{
        get { throw new System.NotImplementedException(); }
    }|]
}
", @"
class C
{
    string P => throw new System.NotImplementedException();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_PropertyWithGetterAndSetter_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string _f;

    public string P
    {
        get [|{ throw new System.NotImplementedException(); }|]
        set [|{ throw new System.NotImplementedException(); }|]
    }
}
", @"
class C
{
    string _f;

    public string P
    {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_IndexerWithGetter_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string this[int index]
    [|{
        get { throw new System.NotImplementedException(); }
    }|]
}
", @"
class C
{
    string this[int index] => throw new System.NotImplementedException();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_IndexerWithGetterAndSetter_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string _f;

    string this[int index]
    {
        get [|{ throw new System.NotImplementedException(); }|]
        set [|{ throw new System.NotImplementedException(); }|]
    }
}
", @"
class C
{
    string _f;

    string this[int index]
    {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_Operator_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    public static C operator !(C value)
    [|{
        throw new System.NotImplementedException();
    }|]
}
", @"
class C
{
    public static C operator !(C value) => throw new System.NotImplementedException();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task Test_ConversionOperator_Throw()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    public static explicit operator C(string value)
    [|{
        throw new System.NotImplementedException();
    }|]
}
", @"
class C
{
    public static explicit operator C(string value) => throw new System.NotImplementedException();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task TestNoDiagnostic_MethodWithMultipleStatements()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    string M()
    {
        M();
        return null;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task TestNoDiagnostic_MethodWithLocalFunction()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        void LF() { }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task TestNoDiagnostic_VoidMethodWithNoStatements()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task TestNoDiagnostic_VoidMethodWithMultipleStatements()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        M();
        M();
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task TestNoDiagnostic_MethodWithMultilineStatement()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    string M()
    {
        return @""a
            b"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task TestNoDiagnostic_PropertyWithMultipleStatement()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    string P
    {
        get
        {
            M();
            return null;
        }
    }

    string M() => null;
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task TestNoDiagnostic_PropertyInvalidStatement()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    string P
    {
        get { new object(); }
    }
}
", options: Options.AddAllowedCompilerDiagnosticId(CompilerDiagnosticIdentifiers.NotAllCodePathsReturnValue));
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task TestNoDiagnostic_AccessorWithAttribute()
        {
            await VerifyNoDiagnosticAsync(@"
using System.Diagnostics;

class C
{
    string P
    {
            [DebuggerStepThrough]
        get
        {
            return null;
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task TestNoDiagnostic_IndexerWithMultipleStatements()
        {
            await VerifyNoDiagnosticAsync(@"
using System.Diagnostics;

class C
{
    string this[int index]
    {
        get
        {
            M();
            return null;
        }
    }

    string M() => null;
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExpressionBodiedMember)]
        public async Task TestNoDiagnostic_ExpressionBody()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    public C() => VM();

    ~C() => VM();

    string M() => null;

    void VM() => VM();

    void M2()
    {
        string LF() => null;
    }

    void M3()
    {
        void LF() => VM();
    }

    string P => null;

    public string P2
    {
        get => _f;
        set => _f = value;
    }

    string this[int index] => null;

    string this[int index, int index2]
    {
        get => _f;
        set => _f = value;
    }

    public static C operator !(C value) => value;

    public static explicit operator C(string value) => new C();

    string _f;
}
");
        }
    }
}
