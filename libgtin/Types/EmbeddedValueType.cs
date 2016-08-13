//
//  EmbeddedValueType.cs
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

namespace libgtin.Types
{
	public enum EmbeddedValueType
	{
		None = -1,
		PriceTwoDecimals    = 0,
		PriceOneDecimal     = 1,
		PriceNoDecimals     = 2,
		WeightThreeDecimals = 3,
		WeightTwoDecimals   = 4,
		WeightOneDecimal    = 5
	}
}