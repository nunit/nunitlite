// ***********************************************************************
// Copyright (c) 2007 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Specialized;

namespace NUnit.Framework
{
	/// <summary>
	/// PropertyAttribute is used to attach information to a test as a name/value pair..
	/// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class PropertyAttribute : Attribute
    {
        private string propertyName;
        private object propertyValue;

        public PropertyAttribute(string propertyName, object propertyValue)
        {
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
        }

        protected PropertyAttribute(object propertyValue)
        {
            this.propertyName = this.GetType().Name;
            if (propertyName.EndsWith("Attribute"))
                propertyName = propertyName.Substring(0, propertyName.Length - 9);
            this.propertyValue = propertyValue;
        }

        public string Name
        {
            get { return propertyName; }
        }

        public virtual object Value
        {
            get { return propertyValue; }
        }
    }
}
