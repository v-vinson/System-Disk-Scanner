using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskScanner.core
{
	class ScanningMsg
	{
		public string CurrentScanningPath { get; set; }

		public string WarningMsg { get; set; }

		public double progressVal { get; set; }

		public ScanningMsg()
		{
			CurrentScanningPath = "";
			WarningMsg = "";
			progressVal = 0;
		}
	}
}
