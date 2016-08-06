//
//  EAN13.cs
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
using libgtin.Validation.Algorithms;
using libgtin.Validation.Algorithms.Modulo;

namespace libgtin.Types.EAN
{
	public class EAN8 : BarcodeType
	{
		public override string Name => "EAN8";
		public override int Length => 8;
		public override bool SupportsEmbeddedValue => false;
		public override EmbeddedValuePackingOrder PackingOrder => EmbeddedValuePackingOrder.End;

		public override List<string> EmbeddedPriceIdentifiers => new List<string>
		{

		};
		public override List<string> EmbeddedWeightIdentifiers => new List<string>
		{

		};

		public override int MaxEmbeddedValue => 0;

		public override int ProductIDLength => 5;
		public override int ProductIDIndex => 3;
		public override int EmbeddedValueLength => 0;
		public override int EmbeddedValueIndex => 0;

		public override BarcodeChecksumAlgorithm Algorithm => Modulo10.Instance;
	}
}