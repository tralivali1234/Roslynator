// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Roslynator
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileLinePositionSpanExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileLinePositionSpan"></param>
        /// <returns></returns>
        public static int StartLine(this FileLinePositionSpan fileLinePositionSpan)
        {
            return fileLinePositionSpan.StartLinePosition.Line;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileLinePositionSpan"></param>
        /// <returns></returns>
        public static int EndLine(this FileLinePositionSpan fileLinePositionSpan)
        {
            return fileLinePositionSpan.EndLinePosition.Line;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileLinePositionSpan"></param>
        /// <returns></returns>
        public static bool IsMultiLine(this FileLinePositionSpan fileLinePositionSpan)
        {
            return fileLinePositionSpan.StartLine() != fileLinePositionSpan.EndLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileLinePositionSpan"></param>
        /// <returns></returns>
        public static bool IsSingleLine(this FileLinePositionSpan fileLinePositionSpan)
        {
            return fileLinePositionSpan.StartLine() == fileLinePositionSpan.EndLine();
        }

        internal static int GetLineCount(this FileLinePositionSpan fileLinePositionSpan)
        {
            return fileLinePositionSpan.EndLine() - fileLinePositionSpan.StartLine() + 1;
        }
    }
}
