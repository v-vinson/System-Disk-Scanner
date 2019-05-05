using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiskScanner.core
{
	[Serializable]
	class DirectoryDetails
	{
		public const double MAX_PROGRESS_VAL = 10000.0;

		public string Path { get; set; }

		public long Size { get; set; }

		public double ProgressVal { get; set; }

		public List<DirectoryDetails> Subdirectories { get; set; }

		public List<string> Subfiles { get; set; }

		private static readonly int processorCount = Environment.ProcessorCount;
		private static int threadsCount = 0;

		public static string SizeToString(long size)
		{
			double d_size = size;

			if (size > Math.Pow(1024, 4))
			{
				return (d_size / Math.Pow(1024, 4)).ToString("0.000") + " TB";
			}
			else if (size > Math.Pow(1024, 3))
			{
				return (d_size / Math.Pow(1024, 3)).ToString("0.000") + " GB";
			}
			else if (size > Math.Pow(1024, 2))
			{
				return (d_size / Math.Pow(1024, 2)).ToString("0.000") + " MB";
			}
			else if (size > 1024)
			{
				return (d_size / 1024).ToString("0.000") + " KB";
			}
			else
			{
				return d_size.ToString("0.00") + " B";
			}
		}

		public static DirectoryDetails AutoScanning(string path, ScanningMsg msg)
		{
			return AutoScanning(path, msg, MAX_PROGRESS_VAL);
		}

		private static DirectoryDetails AutoScanning(string path, ScanningMsg msg, double progressVal)
		{
			var dd = new DirectoryDetails(path);
			var taskPool = new List<Task>();

			msg.CurrentScanningPath = path;

			dd.ProgressVal = progressVal;

			try
			{
				var subdirectories = Directory.GetDirectories(path);
				var subfiles = Directory.GetFiles(path);

				double eachProgressVal = subdirectories.Length != 0 ? dd.ProgressVal / subdirectories.Length : 0;
				dd.ProgressVal = subdirectories.Length != 0 ? 0 : dd.ProgressVal;

				foreach (var dir in subdirectories)
				{
					if (threadsCount < processorCount * 5)
					{
						taskPool.Add(Task.Run(() =>
						{
							var ddChild = AutoScanning(dir, msg, eachProgressVal);

							dd.Subdirectories.Add(ddChild);
							dd.Size += ddChild.Size;
						}));

						threadsCount++;
					}
					else
					{
						var ddChild = AutoScanning(dir, msg, eachProgressVal);

						dd.Subdirectories.Add(ddChild);
						dd.Size += ddChild.Size;
					}
				}

				foreach (var file in subfiles)
				{
					dd.Subfiles.Add(file);
					dd.Size += new FileInfo(file).Length;
				}

				// 线程必须全部完成才能结束
				Task.WaitAll(taskPool.ToArray());

				// 汇报进度
				msg.progressVal += dd.ProgressVal / 10000;
			}
			catch (UnauthorizedAccessException)
			{
				msg.progressVal += dd.ProgressVal / 10000;
				msg.WarningMsg += ("Unauthoried: " + path + "\r\n");
			}
			catch (PathTooLongException)
			{
				msg.progressVal += dd.ProgressVal / 10000;
				msg.WarningMsg += ("PathTooLong: " + path + "\r\n");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return dd;
		}

		public DirectoryDetails(string path, bool autoScanning = false)
		{
			Path = path;
			ProgressVal = 0;

			if (autoScanning)
			{
				var scanningMsg = new ScanningMsg();
				var dd = AutoScanning(path, scanningMsg);

				Size = dd.Size;
				Subdirectories = dd.Subdirectories;
				Subfiles = dd.Subfiles;
			}
			else
			{
				Path = path;
				Size = 0;
				Subdirectories = new List<DirectoryDetails>();
				Subfiles = new List<string>();
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is DirectoryDetails dd)
			{
				return Path.Equals(dd.Path);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return Path.GetHashCode();
		}
	}
}
