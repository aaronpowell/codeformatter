﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace Microsoft.DotNet.CodeFormatting.Tests
{
    public class PrivateFieldNamingRuleTests : GlobalSemanticRuleTestBase
    {
        internal override IGlobalSemanticFormattingRule Rule
        {
            get { return new Rules.PrivateFieldNamingRule(); }
        }

        [Fact]
        public void TestUnderScoreInPrivateFields()
        {
            var text = @"
using System;
class T
{
    private static int x;
    private static int s_y;
    // some trivia
    private static int m_z;
    // some trivia
    private int k = 1, m_s = 2, rsk_yz = 3, x_y_z;
    // some trivia
    [ThreadStatic] static int r;
    [ThreadStaticAttribute] static int b_r;
}";
            var expected = @"
using System;
class T
{
    private static int s_x;
    private static int s_y;
    // some trivia
    private static int s_z;
    // some trivia
    private int _k = 1, _s = 2, _rsk_yz = 3, _y_z;
    // some trivia
    [ThreadStatic]
    static int t_r;
    [ThreadStaticAttribute]
    static int t_r;
}";
            Verify(text, expected);
        }

        [Fact]
        public void CornerCaseNames()
        {
            var text = @"
class C
{
    private int x_;
    private int _;
    private int __;
    private int m_field1;
    private int field2_;
";

            var expected = @"
class C
{
    private int _x;
    private int _;
    private int __;
    private int _field1;
    private int _field2;
";

            Verify(text, expected, runFormatter: false);
        }

        [Fact]
        public void MultipleDeclarators()
        {
            var text = @"
class C1
{
    private int field1, field2, field3;
}

class C2
{
    private static int field1, field2, field3;
}

class C3
{
    internal int field1, field2, field3;
}
";

            var expected = @"
class C1
{
    private int _field1, _field2, _field3;
}

class C2
{
    private static int s_field1, s_field2, s_field3;
}

class C3
{
    internal int field1, field2, field3;
}
";

            Verify(text, expected, runFormatter: true);
        }

        /// <summary>
        /// If the name is pascal cased make it camel cased during the rewrite.  If it is not
        /// pascal cased then do not change the casing.
        /// </summary>
        [Fact]
        public void NameCasingField()
        {
            var text = @"
class C
{
    int Field;
    static int Other;
    int GCField;
    static int GCOther;
}
";

            var expected = @"
class C
{
    int _field;
    static int s_other;
    int _GCField;
    static int s_GCOther;
}
";

            Verify(text, expected, runFormatter: false);

        }
    }
}
