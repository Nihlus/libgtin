//
//  BarcodeChecksumAlgorithm.cs
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

namespace libgtin.Validation.Algorithms
{
	/// <summary>
	/// An algorithm capable of verifying and generating a checksum value for a provided barcode.
	/// </summary>
	public abstract class BarcodeChecksumAlgorithm
	{
		/// <summary>
		/// Verifies the specified barcode using this algorithm.
		/// </summary>
		/// <returns>
		/// <value>true</value> if the barcode had a valid checksum; <value>false</value> otherwise.
		/// </returns>
		/// <param name="barcode">The barcode to verify.</param>
		/// <param name="checksum">The actual checksum of the verified barcode.</param>
		public abstract bool Verify(Barcode barcode, out int checksum);

		/// <summary>
		/// Verifies the specified barcode using this algorithm.
		/// </summary>
		/// <returns>
		/// <value>true</value> if the barcode had a valid checksum; <value>false</value> otherwise.
		/// </returns>
		/// <param name="barcode">The barcode to verify.</param>
		/// <param name="checksum">The actual checksum of the verified barcode.</param>
		public abstract bool Verify(string barcode, out int checksum);

		/// <summary>
		/// Verifies the specified barcode using this algorithm.
		/// </summary>
		/// <returns>
		/// <value>true</value> if the barcode had a valid checksum; <value>false</value> otherwise.
		/// </returns>
		/// <param name="barcode">The barcode to verify.</param>
		public abstract bool Verify(Barcode barcode);

		/// <summary>
		/// Verifies the specified barcode using this algorithm.
		/// </summary>
		/// <returns>
		/// <value>true</value> if the barcode had a valid checksum; <value>false</value> otherwise.
		/// </returns>
		/// <param name="barcode">The barcode to verify.</param>
		public abstract bool Verify(string barcode);
	}
}