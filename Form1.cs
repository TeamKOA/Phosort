using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phosort
{
    public partial class Form1 : Form
    {
        bool ratio = false;

        string openPath;
        string savePath;

        string[] files;
        int currentItem = 0;
        public Form1()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyEvent);
            this.ResizeEnd += new EventHandler(Form1_Resized);
        }

        #region KeyHandling
        private void Form1_KeyEvent(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    Next();
                    break;
                case Keys.Left:
                    Previous();
                    break;
                case Keys.Up:
                    Save();
                    break;
                case Keys.A:
                    ratio = !ratio;
                    break;
            }
        }

        void Next()
        {
            currentItem = currentItem++ > files.Length - 1 ? 0 : currentItem++;
            LoadPicture();
        }

        void Previous()
        {
            currentItem = currentItem-- < 0 ? files.Length - 1 : currentItem--;
            LoadPicture();
        }

        void Save()
        {
            File.Copy(files[currentItem], files[currentItem].Replace(openPath, savePath));
        }
        #endregion

        private void Form1_Resized(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size((int)((this.Size.Height - 75 )* (ratio ? (16F / 9F) : 1.5F)), this.Size.Height - 75);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DialogResult openRes = fdlg_open.ShowDialog();
            DialogResult saveRes = fdlg_save.ShowDialog();
            if (openRes != DialogResult.OK || saveRes != DialogResult.OK)
            {
                //Error handling lol
            }
            openPath = fdlg_open.SelectedPath;
            savePath = fdlg_save.SelectedPath;

            files = Directory.GetFiles(openPath);

            List<string> filteredFiles = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].ToLower().EndsWith(".jpg"))
                    filteredFiles.Add(files[i]);
            }
            files = filteredFiles.ToArray<string>();

            LoadPicture();
        }

        private void LoadPicture()
        {
            pictureBox1.ImageLocation = files[currentItem];
            pictureBox1.Load();
        }
    }
}
