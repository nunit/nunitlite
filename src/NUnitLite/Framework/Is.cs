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
    #region Is Helper Class
    public class Is
    {
        #region Prefix Operators
        /// <summary>
        /// Is.Not returns a ConstraintBuilder that negates
        /// the constraint that follows it.
        /// </summary>
        public static ConstraintExpression Not
        {
            get { return new ConstraintExpression().Not; }
        }

        /// <summary>
        /// Is.All returns a ConstraintBuilder, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if all of them succeed. This property is
        /// a synonym for Has.AllItems.
        /// </summary>
        public static ConstraintExpression All
        {
            get { return new ConstraintExpression().All; }
        }
        #endregion

        #region Constraints Without Arguments

        #region Null

        /// <summary>
        /// Returns a constraint that tests for null
        /// </summary>
        public static NullConstraint Null
        {
            get { return new NullConstraint(); }
        }

        #endregion

        #region True

        /// <summary>
        /// Returns a constraint that tests for True
        /// </summary>
        public static TrueConstraint True
        {
            get { return new TrueConstraint(); }
        }

        #endregion

        #region False

        /// <summary>
        /// Returns a constraint that tests for False
        /// </summary>
        public static FalseConstraint False
        {
            get { return new FalseConstraint(); }
        }

        #endregion

        #region NaN

        /// <summary>
        /// Returns a constraint that tests for NaN
        /// </summary>
        public static NaNConstraint NaN
        {
            get { return new NaNConstraint(); }
        }

        #endregion

        #region Empty

        /// <summary>
        /// Returns a constraint that tests for empty
        /// </summary>
        public static EmptyConstraint Empty
        {
            get { return new EmptyConstraint(); }
        }

        #endregion

        #region Unique

        /// <summary>
        /// Returns a constraint that tests whether a collection 
        /// contains all unique items.
        /// </summary>
        public static UniqueItemsConstraint Unique
        {
            get { return new UniqueItemsConstraint(); }
        }

        #endregion

#if !NETCF
        #region BinarySerializable
        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in binary format.
        /// </summary>
        public static BinarySerializableConstraint BinarySerializable
        {
            get { return new BinarySerializableConstraint(); }
        }
        #endregion
#endif

#if !NETCF_1_0
        #region XmlSerializable

        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in xml format.
        /// </summary>
        public static XmlSerializableConstraint XmlSerializable
        {
            get { return new XmlSerializableConstraint(); }
        }

        #endregion
#endif

        #endregion

        #region Constraints with an expected value

        #region Equality and Identity
        /// <summary>
        /// Is.EqualTo returns a constraint that tests whether the
        /// actual value equals the supplied argument
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static EqualConstraint EqualTo(object expected)
        {
            return new EqualConstraint(expected);
        }
        /// <summary>
        /// Is.SameAs returns a constraint that tests whether the
        /// actual value is the same object as the supplied argument.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static Constraint SameAs(object expected)
        {
            return new SameAsConstraint(expected);
        }
        #endregion

        #region Comparison Constraints
        /// <summary>
        /// Is.GreaterThan returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public static Constraint GreaterThan(IComparable expected)
        {
            return new GreaterThanConstraint(expected);
        }
        /// <summary>
        /// Is.GreaterThanOrEqualTo returns a constraint that tests whether the
        /// actual value is greater than or equal to the suppled argument
        /// </summary>
        public static Constraint GreaterThanOrEqualTo(IComparable expected)
        {
            return new GreaterThanOrEqualConstraint(expected);
        }

        /// <summary>
        /// Is.AtLeast is a synonym for Is.GreaterThanOrEqualTo
        /// </summary>
        public static Constraint AtLeast(IComparable expected)
        {
            return GreaterThanOrEqualTo(expected);
        }

        /// <summary>
        /// Is.LessThan returns a constraint that tests whether the
        /// actual value is less than the suppled argument
        /// </summary>
        public static Constraint LessThan(IComparable expected)
        {
            return new LessThanConstraint(expected);
        }

        /// <summary>
        /// Is.LessThanOrEqualTo returns a constraint that tests whether the
        /// actual value is less than or equal to the suppled argument
        /// </summary>
        public static Constraint LessThanOrEqualTo(IComparable expected)
        {
            return new LessThanOrEqualConstraint(expected);
        }

        /// <summary>
        /// Is.AtMost is a synonym for Is.LessThanOrEqualTo
        /// </summary>
        public static Constraint AtMost(IComparable expected)
        {
            return LessThanOrEqualTo(expected);
        }
        #endregion

        #region Type Constraints
        /// <summary>
        /// Returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        public static Constraint TypeOf(Type expectedType)
        {
            return new ExactTypeConstraint(expectedType);
        }

        /// <summary>
        /// Is.InstanceOfType returns a constraint that tests whether 
        /// the actual value is of the type supplied as an argument
        /// or a derived type.
        /// </summary>
        public static Constraint InstanceOfType(Type expectedType)
        {
            return new InstanceOfTypeConstraint(expectedType);
        }

        /// <summary>
        /// Is.AssignableFrom returns a constraint that tests whether
        /// the actual value is assignable from the type supplied as
        /// an argument.
        /// </summary>
        /// <param name="expectedType"></param>
        /// <returns></returns>
        public static Constraint AssignableFrom(Type expectedType)
        {
            return new AssignableFromConstraint(expectedType);
        }
        #endregion

        #region String Constraints
        /// <summary>
        /// Is.StringContaining returns a constraint that succeeds if the actual
        /// value contains the substring supplied as an argument.
        /// </summary>
        public static SubstringConstraint StringContaining(string substring)
        {
            return new SubstringConstraint(substring);
        }

        /// <summary>
        /// Is.StringStarting returns a constraint that succeeds if the actual
        /// value starts with the substring supplied as an argument.
        /// </summary>
        public static StartsWithConstraint StringStarting(string substring)
        {
            return new StartsWithConstraint(substring);
        }

        /// <summary>
        /// Is.StringEnding returns a constraint that succeeds if the actual
        /// value ends with the substring supplied as an argument.
        /// </summary>
        public static EndsWithConstraint StringEnding(string substring)
        {
            return new EndsWithConstraint(substring);
        }

#if !NETCF
        /// <summary>
        /// Is.StringMatching returns a constraint that succeeds if the actual
        /// value matches the regular expression supplied as an argument.
        /// </summary>
        public static RegexConstraint StringMatching(string pattern)
        {
            return new RegexConstraint(pattern);
        }
#endif
        #endregion

        #region Collection Constraints
        /// <summary>
        /// Is.EquivalentTo returns a constraint that tests whether
        /// the actual value is a collection containing the same
        /// elements as the collection supplied as an arument
        /// </summary>
        public static Constraint EquivalentTo(ICollection expected)
        {
            return new CollectionEquivalentConstraint(expected);
        }

        /// <summary>
        /// Is.SubsetOf returns a constraint that tests whether
        /// the actual value is a subset of the collection 
        /// supplied as an arument
        /// </summary>
        public static Constraint SubsetOf(ICollection expected)
        {
            return new CollectionSubsetConstraint(expected);
        }
        #endregion

        #endregion

        /// <summary>
        /// Returns a constraint that tests whether the path provided 
        /// is the same as an expected path after canonicalization.
        /// </summary>
        public static SamePathConstraint SamePath(string expected)
        {
            return new SamePathConstraint(expected);
        }

        /// <summary>
        /// Returns a constraint that tests whether the path provided 
        /// is the same path or under an expected path after canonicalization.
        /// </summary>
        public static SamePathOrUnderConstraint SamePathOrUnder(string expected)
        {
            return new SamePathOrUnderConstraint(expected);
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual value falls 
        /// within a specified range.
        /// </summary>
        public static RangeConstraint InRange(IComparable from, IComparable to)
        {
            return new RangeConstraint(from, to);
        }
    }
    #endregion
}
