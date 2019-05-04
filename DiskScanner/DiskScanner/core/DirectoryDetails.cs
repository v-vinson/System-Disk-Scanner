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
		public string Path { get; set; }

		public long Size { get; set; }

		public List<DirectoryDetails> Subdirectories { get; set; }

		public List<string> Subfiles { get; set; }

		public static DirectoryDetails AutoScanning(string path, ref string currentPath, ref string warningMsg)
		{
			var dd = new DirectoryDetails(path);

			currentPath = path;

			if (warningMsg == null)
			{
				warningMsg = "";
			}

			try
			{
				var subdirectories = Directory.GetDirectories(path);
				var subfiles = Directory.GetFiles(path);

				if (subdirectories.Length != 0)
				{
					foreach (var dir in subdirectories)
					{
						var ddChild = AutoScanning(dir, ref currentPath, ref warningMsg);

						dd.Subdirectories.Add(ddChild);
						dd.Size += ddChild.Size;
					}
				}

				foreach (var file in subfiles)
				{
					dd.Subfiles.Add(file);
					dd.Size += new FileInfo(file).Length;
				}
			}
			catch (UnauthorizedAccessException)
			{
				warningMsg += ("Unauthoried: " + path + "\r\n");
			}
			catch (PathTooLongException)
			{
				warningMsg += ("PathTooLong: " + path + "\r\n");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return dd;
		}

		public static string SizeToString(long size)
		{
			double d_size = size;

			if (size > Math.Pow(1024, 4))
			{
				return (d_size / Math.Pow(1024, 4)).ToString() + " TB";
			}
			else if (size > Math.Pow(1024, 3))
			{
				return (d_size / Math.Pow(1024, 3)).ToString() + " GB";
			}
			else if (size > Math.Pow(1024, 2))
			{
				return (d_size / Math.Pow(1024, 2)).ToString() + " MB";
			}
			else if (size > 1024)
			{
				return (d_size / 1024).ToString() + " KB";
			}
			else
			{
				return d_size.ToString() + " B";
			}
		}

		public DirectoryDetails(string path, bool autoScanning = false)
		{
			Path = path;

			if (autoScanning)
			{
				string currentPath = null;
				string warningMsg = null;
				var dd = AutoScanning(path, ref currentPath, ref warningMsg);

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
