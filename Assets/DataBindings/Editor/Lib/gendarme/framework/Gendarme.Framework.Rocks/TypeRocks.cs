//
// Gendarme.Framework.Rocks.TypeRocks
//
// Authors:
//	Sebastien Pouliot  <sebastien@ximian.com>
//      Daniel Abramov <ex@vingrad.ru>
//	Adrian Tsai <adrian_tsai@hotmail.com>
//	Andreas Noever <andreas.noever@gmail.com>
//
// Copyright (C) 2007-2008 Novell, Inc (http://www.novell.com)
// (C) 2007 Daniel Abramov
// Copyright (c) 2007 Adrian Tsai
// (C) 2008 Andreas Noever
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Mono.Cecil;

namespace Gendarme.Framework.Rocks {

	// add Type[Definition|Reference] extensions methods here
	// only if:
	// * you supply minimal documentation for them (xml)
	// * you supply unit tests for them
	// * they are required somewhere to simplify, even indirectly, the rules
	//   (i.e. don't bloat the framework in case of x, y or z in the future)

	/// <summary>
	/// TypeRocks contains extensions methods for Type[Definition|Reference]
	/// and the related collection classes.
	/// 
	/// Note: whenever possible try to use TypeReference since it's extend the
	/// reach/usability of the code.
	/// </summary>
	public static class TypeRocks {
		/// <summary>
		/// Check if the type is a delegate.
		/// </summary>
		/// <param name="self">The TypeReference on which the extension method can be called.</param>
		/// <returns>True if the type is a delegate, False otherwise.</returns>
		public static bool IsDelegate (this TypeReference self)
		{
			if (self == null)
				return false;

			TypeDefinition type = self.Resolve ();
			// e.g. this occurs for <Module> or GenericParameter
			if (null == type || type.BaseType == null)
				return false;

			if (type.BaseType.Namespace != "System")
				return false;

			string name = type.BaseType.Name;
			return ((name == "Delegate") || (name == "MulticastDelegate"));
		}

		/// <summary>
		/// Check if the type represent a floating-point type.
		/// </summary>
		/// <param name="self">The TypeReference on which the extension method can be called.</param>
		/// <returns>True if the type is System.Single (C# float) or System.Double (C3 double), False otherwise.</returns>
		public static bool IsFloatingPoint (this TypeReference self)
		{
			if (self == null)
				return false;

			if (self.Namespace != "System")
				return false;

			string name = self.Name;
			return ((name == "Single") || (name == "Double"));
		}

		/// <summary>
		/// Check if the type is static (2.0+)
		/// </summary>
		/// <param name="self">The TypeReference on which the extension method can be called.</param>
		/// <returns>True if the type is static, false otherwise.</returns>
		public static bool IsStatic (this TypeReference self)
		{
			if (self == null)
				return false;

			TypeDefinition type = self.Resolve ();
			if (type == null)
				return false;
			return (type.IsSealed && type.IsAbstract);
		}

		/// <summary>
		/// Check if the type is visible outside of the assembly.
		/// </summary>
		/// <param name="self">The TypeReference on which the extension method can be called.</param>
		/// <returns>True if the type can be used from outside of the assembly, false otherwise.</returns>
		public static bool IsVisible (this TypeReference self)
		{
			if (self == null)
				return false;

			TypeDefinition type = self.Resolve ();
			if (type == null)
				return true; // it's probably visible since we have a reference to it

			while (type.IsNested) {
				if (type.IsNestedPrivate || type.IsNestedAssembly)
					return false;
				// Nested classes are always inside the same assembly, so the cast is ok
				type = type.DeclaringType.Resolve ();
			}
			return type.IsPublic;
		}
	}
}
