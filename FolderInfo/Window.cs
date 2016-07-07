using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FolderInfo
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
        }

        private void textBoxRootPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.dataGrid.DataSource = GetFolderInfos(this.textBoxRootPath.Text);
            }
        }

        private object GetFolderInfos(string rootPath)
        {
            var rootDir = new DirectoryInfo(rootPath);
            return rootDir.EnumerateDirectories().Select(d => new { d.Name, Size = GetFullSize(d) }).ToList();
        }

        private FolderSize GetFullSize(DirectoryInfo dir)
        {
            return dir.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(f => f.Length);
        }
    }

    public struct FolderSize
    {
        private long _size;
        private FolderSize(long size)
        {
            _size = size;
        }
        public override string ToString()
        {
            return _size < 1024 ? string.Format("{0:N0} bytes", _size)
                : _size < 1024 * 1024 ? string.Format("{0:N} KB", _size / 1024d)
                : _size < 1024 * 1024 * 1024 ? string.Format("{0:N} MB", _size / (1024 * 1024d))
                : string.Format("{0:N} GB", _size / (1024 * 1024 * 1024d));
        }
        public static implicit operator FolderSize(long size) { return new FolderSize(size); }
    }
}
