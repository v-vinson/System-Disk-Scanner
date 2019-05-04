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

namespace DiskScanner
{
	public partial class MainWindow : Form
	{
		public const string backupPath = "backup.bin";

		private DirectoryDetails dir = null;

		private DirectoryDetails backup_dir = null;

		private string currentPath = null;

		private string warningMsg = null;

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
				backup_dir = LoadData(backupPath);
				if (backup_dir != null)
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
			IsScanning = true;

			mainTimer.Start();

			btnScan.Enabled = false;
			btnSave.Enabled = false;
			btnCompare.Enabled = false;

			Task.Run(() =>
			{
				dir = DirectoryDetails.AutoScanning("C:\\", ref currentPath, ref warningMsg);
				IsScanning = false;

				Thread.Sleep(interval * 2);

				mainTimer.Stop();

				btnScan.Enabled = true;
				btnSave.Enabled = true;

				if (backup_dir != null)
				{
					btnCompare.Enabled = true;
				}

				lbMsg.Text = "扫描完成！";
			});
		}

		private void Compare(DirectoryDetails new_dir, DirectoryDetails backup_dir, long deltaVal)
		{
			foreach (var dir in new_dir.Subdirectories)
			{
				int index = -1;
				if ((index = backup_dir.Subdirectories.IndexOf(dir)) != -1)
				{
					if ((dir.Size - backup_dir.Subdirectories[index].Size) > deltaVal)
					{
						txtCompareMsg.Text += string.Format("[已存在文件夹(+{0})] {1}\r\n", DirectoryDetails.SizeToString(dir.Size - backup_dir.Subdirectories[index].Size), dir.Path);

						Compare(dir, backup_dir.Subdirectories[index], deltaVal);
					}
				}
				else
				{
					if (dir.Size > deltaVal)
						txtCompareMsg.Text += string.Format("[新增文件夹(+{0})] {1}\r\n", DirectoryDetails.SizeToString(dir.Size), dir.Path);
				}
			}

			foreach (var file in new_dir.Subfiles)
			{
				long size = new FileInfo(file).Length;

				int index = -1;
				if ((index = backup_dir.Subfiles.IndexOf(file)) == -1)
				{
					if (size > deltaVal)
						txtCompareMsg.Text += string.Format("[新增文件({0})] {1}\r\n", DirectoryDetails.SizeToString(size), file);
				}
			}

		}

		private void BtnCompare_Click(object sender, EventArgs e)
		{
			txtCompareMsg.Text = "";

			if (dir.Equals(backup_dir))
			{
				try
				{
					Compare(dir, backup_dir, (long)(Convert.ToDouble(txtDeltaVal.Text) * Math.Pow(1024, 2)));

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
			if (dir != null)
			{
				try
				{
					SaveData(backupPath);
					lbMsg.Text = "数据备份成功";

					lbBackupData.Text = "正在搜索备份数据...";
					Task.Run(() =>
					{
						backup_dir = LoadData(backupPath);
						if (backup_dir != null)
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
			txtScanningMsg.Text = warningMsg;
			lbMsg.Text = "正在扫描: " + currentPath;
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
				bf.Serialize(fs, dir);
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
	}
}
