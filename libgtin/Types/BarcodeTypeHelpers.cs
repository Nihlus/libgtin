//
//  BarcodeTypeHelpers.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace libgtin.Types
{
	/// <summary>
	/// A set of helper functions for finding and determining types of barcodes.
	/// </summary>
	public static class BarcodeTypeHelpers
	{
		/// <summary>
		/// The cached barcode type definitions found in the current running environment.
		/// </summary>
		private static readonly List<Type> CachedBarcodeTypes = new List<Type>();

		/// <summary>
		/// Creates the static instance of the <see cref="BarcodeTypeHelpers"/>, and scans the current running environment
		/// for subclasses of <see cref="BarcodeType"/>.
		/// </summary>
		static BarcodeTypeHelpers()
		{
			CacheAccessibleBarcodeTypes();
		}

		/// <summary>
		/// Scans the current running environment for any subclasses of <see cref="BarcodeType"/>, and caches them for
		/// use with identifying different barcodes.
		/// </summary>
		private static void CacheAccessibleBarcodeTypes()
		{
			CachedBarcodeTypes.Clear();

			Assembly executingAssembly = Assembly.GetExecutingAssembly();

			// Cache BarcodeType definitions in the executing assembly's loaded modules
			foreach (Module assemblyModule in executingAssembly.GetLoadedModules())
			{
				foreach (Type type in assemblyModule.GetTypes())
				{
					if (type.IsSubclassOf(typeof(BarcodeType)))
					{
						if (!CachedBarcodeTypes.Contains(type))
						{
							CachedBarcodeTypes.Add(type);
						}

						Console.WriteLine(type.Name);
					}
				}
			}
		}

		/// <summary>
		/// Determines the type of the provided <paramref name="barcode"/>.
		/// </summary>
		/// <param name="barcode">A barcode string.</param>
		/// <returns>
		/// A valid <see cref="BarcodeType"/> object, containing the definitions of the type.
		/// This method returns <value>null</value> if no matching type could be found.
		/// </returns>
		public static BarcodeType GetBarcodeType(this string barcode)
		{
			foreach (Type barcodeAssemblyType in CachedBarcodeTypes)
			{
				BarcodeType barcodeType = (BarcodeType)Activator.CreateInstance(barcodeAssemblyType);

				if (barcode.Length != barcodeType.Length)
				{
					continue;
				}

				if (!barcodeType.Algorithm.Verify(barcode))
				{
					continue;
				}

				if (barcodeType.SupportsEmbeddedValue)
				{
					string embeddedValueID = barcode.Substring(barcodeType.AreaIDIndex, barcodeType.AreaIDLength - 1);
					if (barcodeType.EmbeddedPriceIdentifiers.Contains(embeddedValueID) || barcodeType.EmbeddedWeightIdentifiers.Contains(embeddedValueID))
					{
						int embeddedValue = int.Parse(barcode.Substring(barcodeType.EmbeddedValueIndex, barcodeType.EmbeddedValueLength));
						if (embeddedValue > barcodeType.MaxEmbeddedValue || embeddedValue < 0)
						{
							continue;
						}
					}
				}

				return barcodeType;
			}

			return null;
		}
	}
}