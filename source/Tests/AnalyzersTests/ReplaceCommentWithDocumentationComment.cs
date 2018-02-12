// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

#pragma warning disable RCS1007, RCS1016, RCS1100, RCS1106, RCS1138, RCS1163, RCS1164, RCS1176

// x
namespace Roslynator.CSharp.Analyzers.Tests
{
    // x
    public static class ReplaceCommentWithDocumentationComment
    {
        // x1
        // x2
        private class FooLeading
        {
            // x
            public string FieldName;

            // x
            public const string ConstantName = null;

            // x
            public FooLeading(object parameter)
            {
            }

            // x
            ~FooLeading()
            {
            }

            // x
            public event EventHandler EventName;

            // x
            public event EventHandler<EventArgs> EventName2
            {
                add { }
                remove { }
            }

            // x
            public string PropertyName { get; set; }

            // x
            public string this[int index]
            {
                get { return null; }
                set { }
            }

            // x
            public void MethodName<T>(object parameter)
            {
            }

            // x
            public static explicit operator FooLeading(string value)
            {
                return new FooLeading(null);
            }

            // x
            public static explicit operator string(FooLeading value)
            {
                return null;
            }

            // x
            public static FooLeading operator !(FooLeading value)
            {
                return new FooLeading(null);
            }

            // x
            public enum EnumName
            {
                // x
                None
            }

            // x
            public interface InterfaceName<T>
            {
            }

            // x
            public struct StructName<T>
            {
            }

            // x
            public class ClassName<T>
            {
            }

            // x
            public delegate void DelegateName<T>(object parameter);

            // n

            /// <summary>
            /// x
            /// </summary>
            public class Foo2
            {
            }
        }

        // x1
        // x2
        private class FooTrailing // x
        {
            public string FieldName; // x

            public const string ConstantName = null; // x

            public FooTrailing(object parameter) // x
            {
            }

            ~FooTrailing() // x
            {
            }

            public event EventHandler EventName; // x

            public event EventHandler<EventArgs> EventName2
            {
                add { }
                remove { }
            } // x

            public string PropertyName { get; set; } // x

            public string this[int index] // x
            {
                get { return null; }
                set { }
            }

            public void MethodName<T>(object parameter) // x
            {
            }

            public static explicit operator FooTrailing(string value) // x
            {
                return new FooTrailing(null);
            }

            public static explicit operator string(FooTrailing value) // x
            {
                return null;
            }

            public static FooTrailing operator !(FooTrailing value) // x
            {
                return new FooTrailing(null);
            }

            public enum EnumName // x
            {
                None // x
            }

            public interface InterfaceName<T> // x
            {
            }

            public struct StructName<T> // x
            {
            }

            public class ClassName<T> // x
            {
            }

            public delegate void DelegateName<T>(object parameter); // x
        }

        // n

        /// <summary>
        /// x
        /// </summary>
        private class Foo
        {
        }
    }
}
