using System;
using System.Collections;

namespace NUnit.Framework.Constraints
{
    public class ConstraintExpression : ConstraintExpressionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConstraintExpression"/> class.
        /// </summary>
        public ConstraintExpression() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConstraintExpression"/> 
        /// class passing in a ConstraintBuilder, which may be pre-populated.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public ConstraintExpression(ConstraintBuilder builder)
            : base( builder ) { }

        #region Constraints Without Arguments
        /// <summary>
        /// Returns a constraint that tests for null
        /// </summary>
        public NullConstraint Null
        {
            get { return (NullConstraint)this.Append(new NullConstraint()); }
        }

        /// <summary>
        /// Returns a constraint that tests for True
        /// </summary>
        public TrueConstraint True
        {
            get { return (TrueConstraint)this.Append(new TrueConstraint()); }
        }

        /// <summary>
        /// Returns a constraint that tests for False
        /// </summary>
        public FalseConstraint False
        {
            get { return (FalseConstraint)this.Append(new FalseConstraint()); }
        }

        /// <summary>
        /// Returns a constraint that tests for NaN
        /// </summary>
        public NaNConstraint NaN
        {
            get { return (NaNConstraint)this.Append(new NaNConstraint()); }
        }

        /// <summary>
        /// Returns a constraint that tests for empty
        /// </summary>
        public EmptyConstraint Empty
        {
            get { return (EmptyConstraint)this.Append(new EmptyConstraint()); }
        }

        /// <summary>
        /// Returns a constraint that tests whether a collection 
        /// contains all unique items.
        /// </summary>
        public UniqueItemsConstraint Unique
        {
            get { return (UniqueItemsConstraint)this.Append(new UniqueItemsConstraint()); }
        }

        #endregion

        #region Constraints with an expected value

        #region Equality and Identity
        /// <summary>
        /// Returns a constraint that tests two items for equality
        /// </summary>
        public EqualConstraint EqualTo(object expected)
        {
            return (EqualConstraint)this.Append(new EqualConstraint(expected));
        }

        /// <summary>
        /// Returns a constraint that tests that two references are the same object
        /// </summary>
        public SameAsConstraint SameAs(object expected)
        {
            return (SameAsConstraint)this.Append(new SameAsConstraint(expected));
        }
        #endregion

        #region Comparison Constraints
        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public GreaterThanConstraint GreaterThan(IComparable expected)
        {
            return (GreaterThanConstraint)this.Append(new GreaterThanConstraint(expected));
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public GreaterThanOrEqualConstraint GreaterThanOrEqualTo(IComparable expected)
        {
            return (GreaterThanOrEqualConstraint)this.Append(new GreaterThanOrEqualConstraint(expected));
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public GreaterThanOrEqualConstraint AtLeast(IComparable expected)
        {
            return GreaterThanOrEqualTo(expected);
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public LessThanConstraint LessThan(IComparable expected)
        {
            return (LessThanConstraint)this.Append(new LessThanConstraint(expected));
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public LessThanOrEqualConstraint LessThanOrEqualTo(IComparable expected)
        {
            return (LessThanOrEqualConstraint)this.Append(new LessThanOrEqualConstraint(expected));
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public LessThanOrEqualConstraint AtMost(IComparable expected)
        {
            return LessThanOrEqualTo(expected);
        }
        #endregion

        #region Type Constraints
        /// <summary>
        /// Returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        public ExactTypeConstraint TypeOf(Type expectedType)
        {
            return (ExactTypeConstraint)this.Append(new ExactTypeConstraint(expectedType));
        }

#if NET_2_0
        /// <summary>
        /// Returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        public ExactTypeConstraint TypeOf<T>()
        {
            return TypeOf(typeof(T));
        }
#endif

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is of the type supplied as an argument or a derived type.
        /// </summary>
        [Obsolete("Use InstanceOf(expectedType)")]
        public InstanceOfTypeConstraint InstanceOfType(Type expectedType)
        {
            return InstanceOf(expectedType);
        }

#if NET_2_0
        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is of the type supplied as an argument or a derived type.
        /// </summary>
        [Obsolete("Use InstanceOf<T>()")]
        public InstanceOfTypeConstraint InstanceOfType<T>()
        {
            return InstanceOf<T>();
        }
#endif

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is of the type supplied as an argument or a derived type.
        /// </summary>
        public InstanceOfTypeConstraint InstanceOf(Type expectedType)
        {
            return (InstanceOfTypeConstraint)this.Append(new InstanceOfTypeConstraint(expectedType));
        }

#if NET_2_0
        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is of the type supplied as an argument or a derived type.
        /// </summary>
        public InstanceOfTypeConstraint InstanceOf<T>()
        {
            return InstanceOf(typeof(T));
        }
#endif

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public AssignableFromConstraint AssignableFrom(Type expectedType)
        {
            return (AssignableFromConstraint)this.Append(new AssignableFromConstraint(expectedType));
        }

#if NET_2_0
        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public AssignableFromConstraint AssignableFrom<T>()
        {
            return AssignableFrom(typeof(T));
        }
#endif
        #endregion

        #region Containing Constraint
        /// <summary>
        /// Returns a new CollectionContainsConstraint checking for the
        /// presence of a particular object in the collection.
        /// </summary>
        public CollectionContainsConstraint Contains(object expected)
        {
            return (CollectionContainsConstraint)this.Append(new CollectionContainsConstraint(expected));
        }

        /// <summary>
        /// Returns a new CollectionContainsConstraint checking for the
        /// presence of a particular object in the collection.
        /// </summary>
        public CollectionContainsConstraint Member(object expected)
        {
            return (CollectionContainsConstraint)this.Append(new CollectionContainsConstraint(expected));
        }
        #endregion

        #region String Constraints
        /// <summary>
        /// Resolves the chain of constraints using a
        /// SubstringConstraint as base.
        /// </summary>
        public SubstringConstraint StringContaining(string substring)
        {
            return (SubstringConstraint)this.Append(Is.StringContaining(substring));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// StartsWithConstraint as base.
        /// </summary>
        public StartsWithConstraint StringStarting(string substring)
        {
            return (StartsWithConstraint)this.Append(Is.StringStarting(substring));
        }

        /// <summary>
        /// Resolves the chain of constraints using a
        /// StringEndingConstraint as base.
        /// </summary>
        public EndsWithConstraint StringEnding(string substring)
        {
            return (EndsWithConstraint)this.Append(Is.StringEnding(substring));
        }

#if !NETCF
        /// <summary>
        /// Resolves the chain of constraints using a
        /// RegexConstraint as base.
        /// </summary>
        public RegexConstraint StringMatching(string pattern)
        {
            return (RegexConstraint)this.Append(Is.StringMatching(pattern));
        }
#endif
        #endregion

        #region Collection Constraints
        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is a collection containing the same elements as the 
        /// collection supplied as an argument.
        /// </summary>
        public CollectionEquivalentConstraint EquivalentTo(ICollection expected)
        {
            return (CollectionEquivalentConstraint)this.Append(Is.EquivalentTo(expected));
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is a subset of the collection supplied as an argument.
        /// </summary>
        public CollectionSubsetConstraint SubsetOf(ICollection expected)
        {
            return (CollectionSubsetConstraint)this.Append(Is.SubsetOf(expected));
        }
        #endregion

        #region Property Constraints
        /// <summary>
        /// Returns a new PropertyConstraintExpression, which will either
        /// test for the existence of the named property on the object
        /// being tested or apply any following constraint to that property.
        /// </summary>
        public ResolvableConstraintExpression Property(string name)
        {
            return this.Append(new PropOperator(name));
        }

        /// <summary>
        /// Returns a new ConstraintExpression, which will apply the following
        /// constraint to the Length property of the object being tested.
        /// </summary>
        public ResolvableConstraintExpression Length
        {
            get { return Property("Length"); }
        }

        /// <summary>
        /// Returns a new ConstraintExpression, which will apply the following
        /// constraint to the Count property of the object being tested.
        /// </summary>
        public ResolvableConstraintExpression Count
        {
            get { return Property("Count"); }
        }
        #endregion

        #endregion

        #region Prefix Operators
        /// <summary>
        /// Returns a ConstraintExpression that negates any
        /// following constraint.
        /// </summary>
        public ConstraintExpression Not
        {
            get { return this.Append(new NotOperator()); }
        }

        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if all of them succeed.
        /// </summary>
        public ConstraintExpression All
        {
            get { return this.Append(new AllOperator()); }
        }

        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if at least one of them succeeds.
        /// </summary>
        public ConstraintExpression Some
        {
            get { return this.Append(new SomeOperator()); }
        }

        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if all of them fail.
        /// </summary>
        public ConstraintExpression None
        {
            get { return this.Append(new NoneOperator()); }
        }
        #endregion
    }
}
