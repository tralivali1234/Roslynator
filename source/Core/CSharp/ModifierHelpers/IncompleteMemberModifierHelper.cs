// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.ModifierHelpers
{
    internal class IncompleteMemberModifierHelper : AbstractModifierHelper<IncompleteMemberSyntax>
    {
        private IncompleteMemberModifierHelper()
        {
        }

        public static IncompleteMemberModifierHelper Instance { get; } = new IncompleteMemberModifierHelper();

        public override SyntaxNodeOrToken FindNodeOrTokenAfterModifiers(IncompleteMemberSyntax node)
        {
            return node.Type;
        }

        public override SyntaxTokenList GetModifiers(IncompleteMemberSyntax node)
        {
            return node.Modifiers;
        }

        public override IncompleteMemberSyntax WithModifiers(IncompleteMemberSyntax node, SyntaxTokenList modifiers)
        {
            return node.WithModifiers(modifiers);
        }
    }
}
