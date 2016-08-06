//
//  Barcode.cs
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
using System.IO;
using System.Linq;
using libgtin.Types;

namespace libgtin
{
	/// <summary>
	/// A GTIN barcode.
	/// </summary>
	public sealed class Barcode
	{
		/// <summary>
		/// The type of barcode this barcode is.
		/// </summary>
		public BarcodeType Type
		{
			get;
		}

		/// <summary>
		/// The geographical area ID of this barcode. Area IDs in the 20 range are store-specific, and
		/// usually indicate embedded values.
		/// </summary>
		public string AreaID
		{
			get
			{
				return internalBarcode.Substring(Type.AreaIDIndex, Type.AreaIDLength);
			}
		}

		/// <summary>
		/// The product identifies of this barcode. This value is of variable length, depending on whether or
		/// not the barcode has an embedded value. The length of this value will never be more than
		/// <see cref="BarcodeType.ProductIDLength"/> + <see cref="BarcodeType.EmbeddedValueLength"/>, and never less
		/// than <see cref="BarcodeType.ProductIDLength"/>.
		/// </summary>
		public string ProductID
		{
			get
			{
				if (HasEmbeddedPrice || HasEmbeddedWeight)
				{
					return internalBarcode.Substring(Type.ProductIDIndex, Type.ProductIDLength);
				}
				else
				{
					if (Type.PackingOrder == EmbeddedValuePackingOrder.End)
					{
						return internalBarcode.Substring(Type.ProductIDIndex, Type.ProductIDLength + Type.EmbeddedValueLength);
					}
					else
					{
						return internalBarcode.Substring(Type.EmbeddedValueIndex, Type.EmbeddedValueLength + Type.ProductIDLength);
					}
				}
			}
		}

		/// <summary>
		/// The checksum value of this barcode.
		/// </summary>
		public int Checksum
		{
			get
			{
				return int.Parse(internalBarcode.Substring(Type.ChecksumIndex, Type.ChecksumLength));
			}
		}

		/// <summary>
		/// Whether or not this barcode has an embedded price value.
		/// </summary>
		public bool HasEmbeddedPrice
		{
			get
			{
				return Type.SupportsEmbeddedValue && Type.EmbeddedPriceIdentifiers.Contains(this.AreaID);
			}
		}

		/// <summary>
		/// Whether or not this barcode has an embedded weight value.
		/// </summary>
		public bool HasEmbeddedWeight
		{
			get
			{
				return Type.SupportsEmbeddedValue && Type.EmbeddedWeightIdentifiers.Contains(this.AreaID);
			}
		}

		/// <summary>
		/// The internal barcode value.
		/// </summary>
		private readonly string internalBarcode;

		/// <summary>
		/// Creates a new instance of the <see cref="Barcode"/> class from a provided barcode.
		/// </summary>
		/// <returns>
		/// A <see cref="Barcode"/> object.
		/// </returns>
		/// <param name="barcode">A barcode. The barcode must be valid.</param>
		/// <exception cref="ArgumentException">
		/// Will be thrown if the <paramref name="barcode"/> value is not recognized as a valid barcode.
		/// Will also be thrown if the <paramref name="barcode"/> string contains any non-numerical characters.
		/// </exception>
		public Barcode(string barcode)
		{
			if (barcode.Any(c => !char.IsDigit(c)))
			{
				throw new ArgumentException("The barcode may only contain whole number digits.", nameof(barcode));
			}

			BarcodeType barcodeType = barcode.GetBarcodeType();
			if (barcodeType == null)
			{
				throw new ArgumentException("Failed to determine the type of the provided barcode.");
			}

			this.internalBarcode = barcode;
			this.Type = barcodeType;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="Barcode"/> class from a provided numerical barcode.
		/// </summary>
		/// <returns>
		/// A <see cref="Barcode"/> object.
		/// </returns>
		/// <param name="barcode">A barcode in numerical format. The barcode must be valid.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Will be thrown if the <paramref name="barcode"/> value is less than zero.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Will be thrown if the <paramref name="barcode"/> value is not recognized as a valid barcode.
		/// </exception>
		public Barcode(long barcode)
		{
			if (barcode < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(barcode), "The barcode must be a positive integer.");
			}

			BarcodeType barcodeType = barcode.ToString().GetBarcodeType();
			if (barcodeType == null)
			{
				throw new ArgumentException("Failed to determine the type of the provided barcode.");
			}

			this.internalBarcode = barcode.ToString();
			this.Type = barcodeType;
		}

		/// <summary>
		/// Gets the weight value embedded in the barcode.
		/// </summary>
		/// <returns>
		/// The weight value (no more than <see cref="BarcodeType.MaxEmbeddedValue"/>). If this barcode does not support
		/// embedded values, it returns <value>-1</value>.
		/// </returns>
		public int GetEmbeddedPrice()
		{
			if (!HasEmbeddedPrice)
			{
				return -1;
			}

			string internalEmbeddedPrice = internalBarcode.Substring(Type.EmbeddedValueIndex, Type.EmbeddedValueLength);

			int embeddedPrice;
			if (int.TryParse(internalEmbeddedPrice, out embeddedPrice))
			{
				return embeddedPrice;
			}
			else
			{
				throw new InvalidDataException("Failed to parse the embedded price to an integer. The barcode may be corrupt.");
			}
		}

		/// <summary>
		/// Gets the weight value embedded in the barcode.
		/// </summary>
		/// <returns>
		/// The weight value (no more than <see cref="BarcodeType.MaxEmbeddedValue"/>). If this barcode does not have an
		/// embedded value, it returns <value>-1</value>.
		/// </returns>
		public int GetEmbeddedWeight()
		{
			if (!HasEmbeddedWeight)
			{
				return -1;
			}

			string internalEmbeddedWeight = internalBarcode.Substring(Type.EmbeddedValueIndex, Type.EmbeddedValueLength);

			int embeddedWeight;
			if (int.TryParse(internalEmbeddedWeight, out embeddedWeight))
			{
				return embeddedWeight;
			}
			else
			{
				throw new InvalidDataException("Failed to parse the embedded weight to an integer. The barcode may be corrupt.");
			}
		}
	}
}


