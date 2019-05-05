using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiskScanner.core;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace DiskScanner
{
	public partial class MainWindow : Form
	{
		public const string backupPath = "backup.bin";

		private DirectoryDetails newDir = null;

		private DirectoryDetails backupDir = null;

		private ScanningMsg scanningMsg = null;

		private int interval = 50;

		public bool IsScanning { get; set; }

		public MainWindow()
		{
			InitializeComponent();
		}

		private void MainWindow_Load(object sender, EventArgs e)
		{
			CheckForIllegalCrossThreadCalls = false;

			IsScanning = false;

			btnCompare.Enabled = false;
			btnSave.Enabled = false;
			mainTimer.Interval = interval;

			lbBackupData.Text = "正在搜索备份数据...";
			Task.Run(() =>
			{
				backupDir = LoadData(backupPath);
				if (backupDir != null)
				{
					lbBackupData.Text = "备份数据已载入";
				}
				else
				{
					lbBackupData.Text = "无备份数据";
				}
			});
		}

		private void BtnScan_Click(object sender, EventArgs e)
		{
			txtScanningMsg.Text = "";
			pbScan.Value = 0;

			IsScanning = true;

			scanningMsg = new ScanningMsg();

			mainTimer.Start();
			Stopwatch watch = new Stopwatch();
			watch.Start();

			btnScan.Enabled = false;
			btnSave.Enabled = false;
			btnCompare.Enabled = false;

			Task.Run(() =>
			{
				newDir = DirectoryDetails.AutoScanning("C:\\", scanningMsg);
				IsScanning = false;

				Thread.Sleep(interval * 2);

				mainTimer.Stop();

				btnScan.Enabled = true;
				btnSave.Enabled = true;

				if (backupDir != null)
				{
					btnCompare.Enabled = true;
				}

				watch.Stop();

				pbScan.Value = pbScan.Maximum;
				lbMsg.Text = string.Format("扫描完成！用时 {0} 秒", watch.Elapsed.TotalSeconds);
			});
		}

		private void Compare(DirectoryDetails newDir, DirectoryDetails backupDir, long deltaVal)
		{
			foreach (var dir in newDir.Subdirectories)
			{
				int index = -1;
				if ((index = backupDir.Subdirectories.IndexOf(dir)) != -1)
				{
					if ((dir.Size - backupDir.Subdirectories[index].Size) > deltaVal)
					{
						txtCompareMsg.Text += string.Format("[已存在文件夹(+{0})] {1}\r\n", DirectoryDetails.SizeToString(dir.Size - backupDir.Subdirectories[index].Size), dir.Path);

						Compare(dir, backupDir.Subdirectories[index], deltaVal);
					}
				}
				else
				{
					if (dir.Size > deltaVal)
						txtCompareMsg.Text += string.Format("[新增文件夹(+{0})] {1}\r\n", DirectoryDetails.SizeToString(dir.Size), dir.Path);
				}
			}

			foreach (var file in newDir.Subfiles)
			{
				long size = new FileInfo(file).Length;

				int index = -1;
				if ((index = backupDir.Subfiles.IndexOf(file)) == -1)
				{
					if (size > deltaVal)
						txtCompareMsg.Text += string.Format("[新增文件({0})] {1}\r\n", DirectoryDetails.SizeToString(size), file);
				}
			}

		}

		private void BtnCompare_Click(object sender, EventArgs e)
		{
			txtCompareMsg.Text = "";

			if (newDir.Equals(backupDir))
			{
				try
				{
					Compare(newDir, backupDir, (long)(Convert.ToDouble(txtDeltaVal.Text) * Math.Pow(1024, 2)));

					txtCompareMsg.Text += "比较完成!";
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "提示");
				}
			}
			else
			{
				txtCompareMsg.Text = "无法比较：备份数据与扫描数据的根目录不相同";
			}
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			if (newDir != null)
			{
				try
				{
					SaveData(backupPath);
					lbMsg.Text = "数据备份成功";

					lbBackupData.Text = "正在搜索备份数据...";
					Task.Run(() =>
					{
						backupDir = LoadData(backupPath);
						if (backupDir != null)
						{
							lbBackupData.Text = "备份数据已载入";
						}
						else
						{
							lbBackupData.Text = "无备份数据";
						}
					});
				}
				catch (Exception ex)
				{
					lbMsg.Text = "数据备份失败，" + ex.Message;
				}
			}
			else
			{
				lbMsg.Text = "还未进行扫描，无法备份数据";
			}
		}

		private void MainTimer_Tick(object sender, EventArgs e)
		{
			txtScanningMsg.Text = scanningMsg.WarningMsg;
			lbMsg.Text = "正在扫描: " + scanningMsg.CurrentScanningPath;
			pbScan.Value = (int)(scanningMsg.progressVal * 1000000);
		}

		private DirectoryDetails LoadData(string path)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = null;

			DirectoryDetails dd = null;

			try
			{
				fs = new FileStream(path, FileMode.Open);
				dd = (DirectoryDetails)bf.Deserialize(fs);

				return dd;
			}
			catch
			{
				return dd;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
		}

		private void SaveData(string path)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = null;

			try
			{
				fs = new FileStream(path, FileMode.Create);
				bf.Serialize(fs, newDir);
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
		}

		private void BtnUpdateMsg_Click(object sender, EventArgs e)
		{
			MessageBox.Show("V1.1.0\r\n" +
							"在不影响其它工作（甚至游戏）的情况下，使用多线程扫描以提升效率\r\n" +
							"新增进度条功能\r\n" +
							"新增“更新日志”以查看各版本的更新内容", "更新日志");
		}
	}
}
