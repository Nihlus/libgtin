//
//  Modulo10.cs
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

namespace libgtin.Validation.Algorithms.Modulo
{
	/// <summary>
	/// Calculates the Modulo 10 checksum for barcodes.
	/// </summary>
	public class Modulo10 : BarcodeChecksumAlgorithm
	{
		/// <summary>
		/// Singleton instance of the Modulo10 algorithm.
		/// </summary>
		public static readonly Modulo10 Instance = new Modulo10();
		private Modulo10()
		{

		}

		/// <summary>
		/// Verifies the specified barcode using this algorithm.
		/// </summary>
		/// <returns>
		/// <value>true</value> if the barcode had a valid checksum; <value>false</value> otherwise.
		/// </returns>
		/// <param name="barcode">The barcode to verify.</param>
		/// <param name="checksum">The actual checksum of the verified barcode.</param>
		public override bool Verify(Barcode barcode, out int checksum)
		{
			string barcodeWithoutChecksum = barcode.AreaID + barcode.ProductID;
			if (barcode.HasEmbeddedPrice)
			{
				barcodeWithoutChecksum += barcode.GetEmbeddedPrice().ToString();
			}

			if (barcode.HasEmbeddedWeight)
			{
				barcodeWithoutChecksum += barcode.GetEmbeddedWeight().ToString();
			}

			checksum = CalculateChecksum(barcodeWithoutChecksum);
			return checksum == barcode.Checksum;
		}

		/// <summary>
		/// Verifies the specified barcode using this algorithm.
		/// </summary>
		/// <returns>
		/// <value>true</value> if the barcode had a valid checksum; <value>false</value> otherwise.
		/// </returns>
		/// <param name="barcode">The barcode to verify.</param>
		/// <param name="checksum">The actual checksum of the verified barcode.</param>
		public override bool Verify(string barcode, out int checksum)
		{
			string barcodeWithoutChecksum = barcode.Substring(0, barcode.Length - 1);
			int barcodeChecksum = int.Parse(barcode.Substring(barcode.Length - 1));

			checksum = CalculateChecksum(barcodeWithoutChecksum);
			return checksum == barcodeChecksum;
		}

		/// <summary>
		/// Verifies the specified barcode using this algorithm.
		/// </summary>
		/// <returns>
		/// <value>true</value> if the barcode had a valid checksum; <value>false</value> otherwise.
		/// </returns>
		/// <param name="barcode">The barcode to verify.</param>
		public override bool Verify(Barcode barcode)
		{
			string barcodeWithoutChecksum = barcode.AreaID + barcode.ProductID;
			if (barcode.HasEmbeddedPrice)
			{
				barcodeWithoutChecksum += barcode.GetEmbeddedPrice().ToString();
			}

			if (barcode.HasEmbeddedWeight)
			{
				barcodeWithoutChecksum += barcode.GetEmbeddedWeight().ToString();
			}

			int checksum = CalculateChecksum(barcodeWithoutChecksum);
			return checksum == barcode.Checksum;
		}

		/// <summary>
		/// Verifies the specified barcode using this algorithm.
		/// </summary>
		/// <returns>
		/// <value>true</value> if the barcode had a valid checksum; <value>false</value> otherwise.
		/// </returns>
		/// <param name="barcode">The barcode to verify.</param>
		public override bool Verify(string barcode)
		{
			string barcodeWithoutChecksum = barcode.Substring(0, barcode.Length - 1);
			int barcodeChecksum = int.Parse(barcode.Substring(barcode.Length - 1));

			int checksum = CalculateChecksum(barcodeWithoutChecksum);
			return checksum == barcodeChecksum;
		}

		/// <summary>
		/// Calculates the checksum for a provided barcode.
		/// </summary>
		/// <param name="barcodeWithoutChecksum">The barcode to calculate the checksum for, without an appended sum.</param>
		private static int CalculateChecksum(string barcodeWithoutChecksum)
		{
			int sum = 0;
			for (int i = 0; i < barcodeWithoutChecksum.Length; ++i)
			{
				int weight;

				if ((barcodeWithoutChecksum.Length - i) % 2 == 0)
				{
					// Even digit, thus a weight of 1
					weight = 1;
				}
				else
				{
					// Odd digit, thus a weight of 3
					weight = 3;
				}

				int digit = int.Parse(barcodeWithoutChecksum[i].ToString());
				sum += (digit * weight);
			}
			return 10 - (sum % 10);
		}
	}
}