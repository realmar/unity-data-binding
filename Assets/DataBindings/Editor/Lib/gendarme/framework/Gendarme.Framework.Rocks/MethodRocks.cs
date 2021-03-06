//
// Gendarme.Framework.Rocks.MethodRocks
//
// Authors:
//	Sebastien Pouliot  <sebastien@ximian.com>
//	Adrian Tsai <adrian_tsai@hotmail.com>
//	Daniel Abramov <ex@vingrad.ru>
//	Andreas Noever <andreas.noever@gmail.com>
//	Cedric Vivier  <cedricv@neonux.com>
//
// Copyright (C) 2007-2008 Novell, Inc (http://www.novell.com)
// Copyright (c) 2007 Adrian Tsai
// Copyright (C) 2008 Daniel Abramov
// (C) 2008 Andreas Noever
// Copyright (C) 2008 Cedric Vivier
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

using System.Collections.Generic;

using Mono.Cecil;

namespace Gendarme.Framework.Rocks {

	// add Method[Reference|Definition][Collection] extensions methods here
	// only if:
	// * you supply minimal documentation for them (xml)
	// * you supply unit tests for them
	// * they are required somewhere to simplify, even indirectly, the rules
	//   (i.e. don't bloat the framework in case of x, y or z in the future)

	/// <summary>
	/// MethodRocks contains extensions methods for Method[Definition|Reference]
	/// and the related collection classes.
	/// 
	/// Note: whenever possible try to use MethodReference since it's extend the
	/// reach/usability of the code.
	/// </summary>
	public static class MethodRocks {
		/// <summary>
		/// Check if the MethodReference is defined as the entry point of it's assembly.
		/// </summary>
		/// <param name="self">The MethodReference on which the extension method can be called.</param>
		/// <returns>True if the method is defined as the entry point of it's assembly, False otherwise</returns>
		public static bool IsEntryPoint (this MethodReference self)
		{
			return ((self != null) && (self == self.Module.Assembly.EntryPoint));
		}

		/// <summary>
		/// Check if the signature of a method is consitent for it's use as a Main method.
		/// Note: it doesn't check that the method is the EntryPoint of it's assembly.
		/// <code>
		/// static [void|int] Main ()
		/// static [void|int] Main (string[] args)
		/// </code>
		/// </summary>gre
		/// <param name="self">The MethodReference on which the extension method can be called.</param>
		/// <returns>True if the method is a valid Main, False otherwise.</returns>
		public static bool IsMain (this MethodReference self)
		{
			if (self == null)
				return false;

			MethodDefinition method = self.Resolve ();
			// Main must be static
			if (!method.IsStatic)
				return false;

			if (method.Name != "Main")
				return false;

			// Main must return void or int
			switch (method.ReturnType.Name) {
			case "Void":
			case "Int32":
				// ok, continue checks
				break;
			default:
				return false;
			}

			// Main (void)
			if (!method.HasParameters)
				return true;

			IList<ParameterDefinition> pdc = method.Parameters;
			if (pdc.Count != 1)
				return false;

			// Main (string[] args)
			return (pdc [0].ParameterType.Name == "String[]");
		}

		/// <summary>
		/// Check if the method corresponds to the get or set operation on a property.
		/// </summary>
		/// <param name="self">The MethodReference on which the extension method can be called.</param>
		/// <returns>True if the method is a getter or a setter, False otherwise</returns>
		public static bool IsProperty (this MethodReference self)
		{
			if (self == null)
				return false;

			MethodDefinition method = self.Resolve ();
			if (method == null)
				return false;
			return ((method.SemanticsAttributes & (MethodSemanticsAttributes.Getter | MethodSemanticsAttributes.Setter)) != 0);
		}

		/// <summary>
		/// Check if the method is visible outside of the assembly.
		/// </summary>
		/// <param name="self">The MethodReference on which the extension method can be called.</param>
		/// <returns>True if the method can be used from outside of the assembly, false otherwise.</returns>
		public static bool IsVisible (this MethodReference self)
		{
			if (self == null)
				return false;

			MethodDefinition method = self.Resolve ();
			if ((method == null) || method.IsPrivate || method.IsAssembly)
				return false;
			return self.DeclaringType.Resolve ().IsVisible ();
		}
	}
}
