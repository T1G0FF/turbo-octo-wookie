using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace Assignment1
{
    public partial class formCrozzle : Form
    {
        const int PADDING = 12;
        const int CELLSIZE = 25;

        private bool CrozzleFound = false;
        private bool WordlistFound = false;

        Crozzle crozzle;
        Wordlist wordlist;

        public formCrozzle()
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Text = "Crozzle";
            
            InitializeComponent();

            gridCrozzle_Initialize();
            listCrozzle_Initialize();
            btnSelectCrozzle_Initialize();
            btnSelectWordlist_Initialize();
        }

        #region Datagrid - Crozzle
        #region Events
        private void gridCrozzle_SelectionChanged(object sender, EventArgs e)
        {
            gridCrozzle.ClearSelection();
        }
        #endregion

        private void gridCrozzle_Initialize()
        {
            gridCrozzle.AllowUserToAddRows = false;
            gridCrozzle.AllowUserToDeleteRows = false;
            gridCrozzle.AllowUserToOrderColumns = false;
            gridCrozzle.AllowUserToResizeColumns = false;
            gridCrozzle.AllowUserToResizeRows = false;

            gridCrozzle.RowHeadersVisible = false;
            gridCrozzle.ColumnHeadersVisible = false;

            gridCrozzle.ScrollBars = ScrollBars.None;
            gridCrozzle.Margin = new Padding(PADDING);
            gridCrozzle.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void gridCrozzle_SetSize(int crozzleWidth, int crozzleHeight)
        {
            gridCrozzle.RowCount = crozzleWidth;
            gridCrozzle.ColumnCount = crozzleHeight;

            foreach (DataGridViewRow r in gridCrozzle.Rows)
            {
                r.Height = CELLSIZE;
            }

            foreach (DataGridViewColumn c in gridCrozzle.Columns)
            {
                c.Width = CELLSIZE;
            }

            gridCrozzle.Height = crozzleHeight * CELLSIZE + 3;
            gridCrozzle.Width = crozzleWidth * CELLSIZE + 3;

            gridCrozzle.Location = new Point(PADDING, PADDING);
        }

        private void gridCrozzle_LoadData()
        {
            for (int y = 0; y < crozzle.Height; y++)
            {
                for (int x = 0; x < crozzle.Width; x++)
                {
                    char inputValue = crozzle[x, y];
                    gridCrozzle[x, y].Value = inputValue;
                    if (inputValue == ' ')
                    {
                        gridCrozzle[x, y].Style.BackColor = Color.Black;
                    }
                    else
                    {
                        gridCrozzle[x, y].Style.BackColor = DefaultBackColor;
                    }
                }
            }
        }
        #endregion

        #region ListBox - Wordlist
        private void listCrozzle_Initialize()
        {
            listCrozzle.Margin = new Padding(PADDING);
            listCrozzle.Font = new Font(FontFamily.GenericMonospace, 9.0F);
        }

        private void listCrozzle_SetSize()
        {
            listCrozzle.Height = gridCrozzle.Height + gridCrozzle.Margin.Vertical + btnSelectCrozzle.Height;
            listCrozzle.Width = 200;

            listCrozzle.Location = new Point(gridCrozzle.Width + gridCrozzle.Margin.Horizontal, PADDING);
        }

        private void listCrozzle_LoadData()
        {
            listCrozzle.Items.Clear();
            foreach (string word in wordlist)
            {
                listCrozzle.Items.Add(word);
            }
        }
        #endregion

        #region Button - Crozzle
        private void btnSelectCrozzle_Initialize()
        {
            btnSelectCrozzle.Margin = new Padding(PADDING);
        }

        private void btnSelectCrozzle_SetSize()
        {
            btnSelectCrozzle.Location = new Point(
                (gridCrozzle.Width + gridCrozzle.Margin.Horizontal) / 2 - (btnSelectCrozzle.Width + PADDING / 2),
                gridCrozzle.Height + gridCrozzle.Margin.Vertical
                );
        }

        private void btnSelectCrozzle_Click(object sender, EventArgs e)
        {
            CrozzleFound = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = ".";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            openFileDialog.Title = "Select the Crozzle file to Load...";
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string tempName = openFileDialog.FileName;
                if (File.Exists(tempName))
                {
                    try
                    {
                        crozzle = new Crozzle(tempName);
                        gridCrozzle_SetSize(crozzle.Width, crozzle.Height);
                        listCrozzle_SetSize();
                        btnSelectCrozzle_SetSize();
                        btnSelectWordlist_SetSize();

                        gridCrozzle_LoadData();

                        CrozzleFound = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message); // TODO Write to log
                        return;
                    }
                }
            }
        }
        #endregion

        #region Button - Wordlist
        private void btnSelectWordlist_Initialize()
        {
            btnSelectWordlist.Margin = new Padding(PADDING);
        }

        private void btnSelectWordlist_SetSize()
        {
            btnSelectWordlist.Location = new Point(
                (gridCrozzle.Width + gridCrozzle.Margin.Horizontal) / 2 + (PADDING / 2),
                gridCrozzle.Height + gridCrozzle.Margin.Vertical
                );
        }

        private void btnSelectWordlist_Click(object sender, EventArgs e)
        {
            WordlistFound = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = ".";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            openFileDialog.Title = "Select the Wordlist file to load...";
            openFileDialog.Filter = "csv files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string tempName = openFileDialog.FileName;
                if (File.Exists(tempName))
                {
                    try
                    {
                        wordlist = new Wordlist(tempName);

                        listCrozzle_LoadData();

                        WordlistFound = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message); // TODO Write to log
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
