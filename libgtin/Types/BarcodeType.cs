//
//  BarcodeType.cs
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


using System.Collections.Generic;
using libgtin.Validation.Algorithms;

namespace libgtin.Types
{
	/// <summary>
	/// A valid set of definitons describing a valid GTIN barcode.
	/// </summary>
	public abstract class BarcodeType
	{
		/// <summary>
		/// The name of this barcode type.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// The length (in characters) of this barcode type.
		/// </summary>
		public abstract int Length { get; }

		/// <summary>
		/// Whether or not this barcode type supports embedded values.
		/// </summary>
		public abstract bool SupportsEmbeddedValue { get; }

		/// <summary>
		/// The order the embedded value and product ID are packed (if embedded values are supported. Ignored otherwise.)
		/// </summary>
		public abstract EmbeddedValuePackingOrder PackingOrder { get; }

		/// <summary>
		/// A set of AreaIDs used to identify barcodes with embedded prices.
		/// </summary>
		public abstract List<string> EmbeddedPriceIdentifiers { get; }

		/// <summary>
		/// A set of AreaIDs used to identify barcodes with embedded weights.
		/// </summary>
		public abstract List<string> EmbeddedWeightIdentifiers { get; }

		/// <summary>
		/// The maximum possible value an embedded value can hold.
		/// </summary>
		public abstract int MaxEmbeddedValue { get; }

		/// <summary>
		/// The length of the AreaID component.
		/// </summary>
		public virtual int AreaIDLength => 3;

		/// <summary>
		/// The index in the barcode where the AreaID component begins.
		/// </summary>
		public virtual int AreaIDIndex => 0;

		/// <summary>
		/// The length of the ProductID component.
		/// </summary>
		public abstract int ProductIDLength { get; }

		/// <summary>
		/// The index in the barcode where the ProductID component begins.
		/// </summary>
		public abstract int ProductIDIndex { get; }

		/// <summary>
		/// The length of the EmbeddedValue component.
		/// </summary>
		public abstract int EmbeddedValueLength { get; }

		/// <summary>
		/// The index in the barcode where the EmbeddedValue component begins.
		/// </summary>
		public abstract int EmbeddedValueIndex { get; }

		/// <summary>
		/// The length of the Checksum component.
		/// </summary>
		public virtual int ChecksumLength => 1;

		/// <summary>
		/// The index in the barcode where the checksum component begins.
		/// </summary>
		public virtual int ChecksumIndex => Length - 1;

		/// <summary>
		/// The algorithm used to calculate the checksum digit(s) for this barcode type.
		/// </summary>
		public abstract BarcodeChecksumAlgorithm Algorithm { get; }

		/// <summary>
		/// Gets the object's string representation.
		/// </summary>
		public override string ToString()
		{
			return Name;
		}
	}

	/// <summary>
	/// The order in which the product ID and the embedded value are stored in the barcode.
	/// </summary>
	public enum EmbeddedValuePackingOrder
	{
		/// <summary>
		/// The embedded value is stored after the product id.
		/// </summary>
		End,

		/// <summary>
		/// The embedded value is stored before the product id.
		/// </summary>
		Beginning
	}
}