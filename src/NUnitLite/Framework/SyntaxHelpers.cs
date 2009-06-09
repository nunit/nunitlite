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

    #region Has Helper Class
    /// <summary>
    /// Summary description for Has
    /// </summary>
    public class Has
    {
        /// <summary>
        /// Nested class that allows us to restrict the number
        /// of key words that may appear after Has.No.
        /// </summary>
        public class HasNoPrefixBuilder
        {
            /// <summary>
            /// Return a ConstraintBuilder conditioned to apply
            /// the following constraint to a property.
            /// </summary>
            /// <param name="name">The property name</param>
            /// <returns>A ConstraintBuilder</returns>
            public ConstraintExpression Property(string name)
            {
                return new ConstraintExpression().Not.Property(name);
            }

            /// <summary>
            /// Return a Constraint that succeeds if the expected object is
            /// not contained in a collection.
            /// </summary>
            /// <param name="expected">The expected object</param>
            /// <returns>A Constraint</returns>
            public Constraint Member(object expected)
            {
                return new NotConstraint(new CollectionContainsConstraint(expected));
            }
        }

        #region Prefix Operators
        /// <summary>
        /// Has.No returns a ConstraintBuilder that negates
        /// the constraint that follows it.
        /// </summary>
        public static HasNoPrefixBuilder No
        {
            get { return new HasNoPrefixBuilder(); }
        }

        /// <summary>
        /// Has.AllItems returns a ConstraintBuilder, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if all of them succeed.
        /// </summary>
        public static ConstraintExpression All
        {
            get { return new ConstraintExpression().All; }
        }

        /// <summary>
        /// Has.Some returns a ConstraintBuilder, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if any of them succeed. It is a synonym
        /// for Has.Item.
        /// </summary>
        public static ConstraintExpression Some
        {
            get { return new ConstraintExpression().Some; }
        }

        /// <summary>
        /// Has.None returns a ConstraintBuilder, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding only if none of them succeed.
        /// </summary>
        public static ConstraintExpression None
        {
            get { return new ConstraintExpression().None; }
        }

        /// <summary>
        /// Returns a new ConstraintBuilder, which will apply the
        /// following constraint to a named property of the object
        /// being tested.
        /// </summary>
        /// <param name="name">The name of the property</param>
        public static ConstraintExpression Property(string name)
        {
            return new ConstraintExpression().Property(name);
        }
        #endregion

        #region Property Constraints
        /// <summary>
        /// Returns a new PropertyConstraint checking for the
        /// existence of a particular property value.
        /// </summary>
        /// <param name="name">The name of the property to look for</param>
        /// <param name="expected">The expected value of the property</param>
        public static Constraint Property(string name, object expected)
        {
            return new PropertyConstraint(name, new EqualConstraint(expected));
        }

        /// <summary>
        /// Returns a new PropertyConstraint for the Length property
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Constraint Length(int length)
        {
            return Property("Length", length);
        }

        /// <summary>
        /// Returns a new PropertyConstraint or the Count property
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Constraint Count(int count)
        {
            return Property("Count", count);
        }
        #endregion

        #region Member Constraint
        /// <summary>
        /// Returns a new CollectionContainsConstraint checking for the
        /// presence of a particular object in the collection.
        /// </summary>
        /// <param name="expected">The expected object</param>
        public static Constraint Member(object expected)
        {
            return new CollectionContainsConstraint(expected);
        }
        #endregion
    }
    #endregion
}
