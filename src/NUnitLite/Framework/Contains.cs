// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.Collections;
using NUnit.Framework.Constraints;

namespace NUnit.Framework
{
    #region Contains Helper Class
    public class Contains
    {
        public static Constraint Substring(string substring)
        {
            return new SubstringConstraint(substring);
        }

        public static Constraint Item(object item)
        {
            return new CollectionContainsConstraint(item);
        }
    }
    #endregion
}
