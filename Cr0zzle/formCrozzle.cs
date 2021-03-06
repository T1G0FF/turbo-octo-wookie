﻿using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Timers;
using System.ComponentModel;
using System.Linq;

namespace Assignment1
{
    public partial class formCrozzle : Form
    {
        CrozzleCreation HoriGenerator;
        CrozzleCreation VertGenerator;
        BackgroundWorker HoriWorker;
        BackgroundWorker VertWorker;
        BackgroundWorker FinalWorker;

        const int PADDING = 12;
        const int CELLSIZE = 25;
        const int FIVEMINUTES = 300; // Five Mins = 300 seconds
        const int FINALISATION = 30;

        private bool CrozzleFound = false;
        private bool WordlistFound = false;

        Crozzle crozzle;
        Wordlist wordlist;

        private static bool timeOver;
        private static int secondsElapsed;
        System.Timers.Timer oneSecondTimer;

        public formCrozzle()
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Text = "Crozzle";

            InitializeComponent();

            gridCrozzle_Initialize();
            listCrozzle_Initialize();
            lblScoreTitle_Initialize();
            lblScore_Initialize();
            btnSelectCrozzle_Initialize();
            btnSelectWordlist_Initialize();
            btnValidate_Initialize();
            btnCreate_Initialize();
            lblTimer_Initialize();
            progTimer_Initialize();

            if (Directory.Exists(LogFile.folderPath) == false)
            {
                Directory.CreateDirectory(LogFile.folderPath);
            }

            LogFile.WriteLine(new String('=', 60));
            LogFile.WriteLine("Crozzle Log - {0}", DateTime.Now.ToString());
            LogFile.WriteLine(new String('=', 60));

            oneSecondTimer = new System.Timers.Timer(1000);
            oneSecondTimer.Elapsed += new ElapsedEventHandler(FiveMinuteCheck);
        }

        #region Crozzle Creation
        private void FiveMinuteCheck(object sender, EventArgs e)
        {
            secondsElapsed += 1;
            lblTimer.Invoke((MethodInvoker)delegate
            {
                int timeLeft = FIVEMINUTES - secondsElapsed;
                string minutes = (timeLeft / 60).ToString();

                string seconds = ("00" + (timeLeft % 60).ToString());
                seconds = seconds.Substring(seconds.Length - 2);

                string time = minutes + ":" + seconds;
                btnCreate.Text = time;
                lblTimer.Text = time;
                progTimer.PerformStep();
                
            });
            
            if (secondsElapsed >= FIVEMINUTES)
            {
                oneSecondTimer.Stop();
                secondsElapsed = 0;

                btnCreate.Invoke((MethodInvoker)delegate
                {
                    btnCreate.Enabled = true;
                    btnCreate.Text = "Create";
                });
            }
            else if (secondsElapsed >= (FIVEMINUTES - FINALISATION))
            {
                timeOver = true;
                HoriWorker.CancelAsync();
                VertWorker.CancelAsync();

                if(FinalWorker.IsBusy == false)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        FinalWorker.RunWorkerAsync();
                    });
                }
            }
        }

        #endregion

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
            gridCrozzle.RowCount = crozzleHeight;
            gridCrozzle.ColumnCount = crozzleWidth;

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
            listCrozzle.Height = gridCrozzle.Height + gridCrozzle.Margin.Top - (lblScore.Height + lblScore.Margin.Vertical);
            listCrozzle.Width = 200;

            listCrozzle.Location = new Point(
                gridCrozzle.Width + gridCrozzle.Margin.Horizontal,
                lblScore.Height + lblScore.Margin.Vertical
                );
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

                        if (crozzle.ValidFile)
                        {
                            validCrozzleUpdateUI();
                        }
                        else
                        {
                            gridCrozzle.Rows.Clear();
                            MessageBox.Show("Invalid crozzle file! Check the log for details.");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogFile.WriteLine(ex.Message);
                        return;
                    }
                }
            }
        }

        private void validCrozzleUpdateUI()
        {
            gridCrozzle_SetSize(crozzle.Width, crozzle.Height);
            listCrozzle_SetSize();
            lblScoreTitle_SetSize();
            lblScore_SetSize();
            btnSelectCrozzle_SetSize();
            btnSelectWordlist_SetSize();
            btnValidate_SetSize();
            btnScore_SetSize();
            lblTimer_SetSize();
            progTimer_SetSize();

            gridCrozzle_LoadData();

            CrozzleFound = true;
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

            openFileDialog.Title = "Select the Word list file to load...";
            openFileDialog.Filter = "csv files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string tempName = openFileDialog.FileName;
                if (File.Exists(tempName))
                {
                    try
                    {
                        wordlist = new Wordlist(tempName);

                        if (wordlist.ValidFile)
                        {
                            listCrozzle_LoadData();
                        }
                        else
                        {
                            listCrozzle.Items.Clear();
                            MessageBox.Show("Invalid Word list file! Check the log for details.");
                        }

                        WordlistFound = true;
                    }
                    catch (Exception ex)
                    {
                        LogFile.WriteLine(ex.Message);
                        return;
                    }
                }
            }
        }
        #endregion

        #region Button - Validate
        private void btnValidate_Initialize()
        {
            btnValidate.Margin = new Padding(PADDING);
        }

        private void btnValidate_SetSize()
        {
            btnValidate.Location = new Point(
                gridCrozzle.Width + gridCrozzle.Margin.Horizontal + listCrozzle.Width - (btnValidate.Width + PADDING + btnCreate.Width),
                gridCrozzle.Height + gridCrozzle.Margin.Vertical
                );
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            if ((CrozzleFound && WordlistFound))
            {
                if (crozzle.ValidFile && wordlist.ValidFile)
                {
                    if (wordlist.Height == crozzle.Height && wordlist.Width == crozzle.Width)
                    {
                        try
                        {
                            Crozzle ValidatedCrozzle = CrozzleValidation.Validate(crozzle, wordlist);

                            if (ValidatedCrozzle != null)
                            {
                                crozzle = ValidatedCrozzle;
                                string score = CrozzleValidation.GetScore(crozzle, wordlist).ToString();
                                lblScore.Text = score;
#if DEBUG
                                LogFile.WriteLine("\t[INFO] Crozzle is Valid! Score: {0}", score);
#endif

                            }
                            else
                            {
                                LogFile.WriteLine("\t[!ERROR!] Crozzle is Invalid!");
                                lblScore.Text = "-1";
                            }
                        }
                        catch (Exception ex)
                        {
                            LogFile.WriteLine(ex.Message);
                            lblScore.Text = "-1";
                            return;
                        }
                    }
                    else
                    {
                        LogFile.WriteLine("\t[!ERROR!] Word list Dimensions do not match Crozzle Dimensions ([{0},{1}] != [{2},{3}])",
                            wordlist.Width,
                            crozzle.Width,
                            wordlist.Height,
                            crozzle.Height
                            );
                        lblScore.Text = "-1";
                    }
                }
                else
                {
                    MessageBox.Show("One or more of your files did not pass validation!");
                    lblScore.Text = "-1";
                }
            }
            else
            {
                MessageBox.Show("You must select both a crozzle and a word list to perform validation!");
                lblScore.Text = "-1";
            }
        }
        #endregion

        #region Button - Create
        private void btnCreate_Initialize()
        {
            btnCreate.Margin = new Padding(PADDING);
        }

        private void btnScore_SetSize()
        {
            btnCreate.Location = new Point(
                gridCrozzle.Width + gridCrozzle.Margin.Horizontal + listCrozzle.Width - (btnCreate.Width),
                gridCrozzle.Height + gridCrozzle.Margin.Vertical
                );
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (WordlistFound)
            {
                if (wordlist.ValidFile)
                {
                    btnCreate.Enabled = false;
                    this.UseWaitCursor = true;
                    progTimer.Visible = true;
                    lblTimer.Visible = true;
                                        
                    HoriGenerator = new CrozzleCreation(wordlist, CrozzleCreation.Direction.Horizontal);
                    VertGenerator = new CrozzleCreation(wordlist, CrozzleCreation.Direction.Vertical);

                    this.Refresh();

                    timeOver = false;
                    oneSecondTimer.Start();


                    HoriWorker = new BackgroundWorker();
                    HoriWorker.WorkerSupportsCancellation = true;
                    HoriWorker.WorkerReportsProgress = true;

                    HoriWorker.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                    {
                        HoriGenerator.GetBestCrozzle();
                    });

                    VertWorker = new BackgroundWorker();
                    VertWorker.WorkerSupportsCancellation = true;
                    VertWorker.WorkerReportsProgress = true;

                    VertWorker.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                    {
                        VertGenerator.GetBestCrozzle();
                    });

                    HoriWorker.RunWorkerAsync();
                    VertWorker.RunWorkerAsync();

                    FinalWorker = new BackgroundWorker();
                    FinalWorker.WorkerSupportsCancellation = true;
                    FinalWorker.WorkerReportsProgress = true;
                    FinalWorker.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
                    {
                        GetBest();
                    });
                }
                else
                {
                    MessageBox.Show("Your word list did not pass validation!");
                    lblScore.Text = "-1";
                }
            }
            else
            {
                MessageBox.Show("You must select a word list to perform creation!");
                lblScore.Text = "-1";
            }
        }

        private void GetBest()
        {
            Crozzle BestCrozzle = null;
            int BestScore = 0;

            List<Crozzle> HoriCrozzleList = HoriGenerator.Finalise();
            List<Crozzle> VertCrozzleList = VertGenerator.Finalise();

            List<Crozzle> MasterList = HoriCrozzleList.Concat(VertCrozzleList).ToList();

            this.Invoke((MethodInvoker)delegate
            {
                lblTimer.Text = "Finding best score... 0/" + MasterList.Count.ToString();
                progTimer.Maximum = MasterList.Count;
                progTimer.Step = 1;
                progTimer.Value = 0;
            });

            int counter = 0;
            foreach (Crozzle crozz in MasterList)
            {
                counter++;
                //////////////////////////
                crozzle = crozz;
                this.Invoke((MethodInvoker)delegate
                {
                    lblTimer.Text = "Finding best score... " + counter + "/" + MasterList.Count.ToString();
                    lblScore.Text = BestScore.ToString();
                    validCrozzleUpdateUI();
                    progTimer.PerformStep();
                    this.Refresh();
                });
                //////////////////////////
                Crozzle scoredCrozzle = CrozzleValidation.Validate(crozz, wordlist);
                if (scoredCrozzle != null)
                {
                    int CurrentScore = CrozzleValidation.GetScore(scoredCrozzle, wordlist);
                    if (CurrentScore > BestScore)
                    {
                        CrozzleArray temp = crozz.ToCrozzleArray();
                        BestCrozzle = new Crozzle(temp._crozzleGrid);
                        BestScore = CurrentScore;
                    }
                }
            }

            this.Invoke((MethodInvoker)delegate
            {
                crozzle = BestCrozzle;
                lblScore.Text = BestScore.ToString();

                progTimer.Visible = false;
                progTimer.Maximum = FIVEMINUTES;
                progTimer.Step = 1;
                progTimer.Value = 0;

                lblTimer.Visible = false;
                lblTimer.Text = "5:00";

                this.UseWaitCursor = false;

                validCrozzleUpdateUI();
            });
        }
        #endregion

        #region Labels - Score
        private void lblScoreTitle_Initialize()
        {
            lblScoreTitle.Margin = new Padding(PADDING);
        }

        private void lblScoreTitle_SetSize()
        {
            lblScoreTitle.Location = new Point(gridCrozzle.Width + gridCrozzle.Margin.Horizontal, PADDING);
        }

        private void lblScore_Initialize()
        {
            lblScore.Margin = new Padding(PADDING);
        }

        private void lblScore_SetSize()
        {
            lblScore.Location = new Point(gridCrozzle.Width + gridCrozzle.Margin.Horizontal + lblScoreTitle.Width + lblScoreTitle.Margin.Horizontal, PADDING);
        }
        #endregion

        #region Timer Components
        private void lblTimer_Initialize()
        {
            lblTimer.Margin = new Padding(PADDING);
        }

        private void lblTimer_SetSize()
        {
            lblTimer.Location = new Point(
                            ((gridCrozzle.Width + gridCrozzle.Margin.Horizontal) / 2) - (lblTimer.Width / 2),
                            ((gridCrozzle.Height + gridCrozzle.Margin.Vertical) / 2) - (lblTimer.Height / 2) + (progTimer.Height / 2));
        }
        
        private void progTimer_Initialize()
        {
            progTimer.Margin = new Padding(PADDING);
            progTimer.Maximum = FIVEMINUTES;
            progTimer.Step = 1;
            progTimer.Value = 0;
            progTimer_SetSize();
            lblTimer_SetSize();
        }

        private void progTimer_SetSize()
        {
            progTimer.Width = gridCrozzle.Width - 2;
            progTimer.Location = new Point(
                            ((gridCrozzle.Width + gridCrozzle.Margin.Horizontal) / 2) - (progTimer.Width / 2),
                            ((gridCrozzle.Height + gridCrozzle.Margin.Vertical) / 2) - progTimer.Height);
        }
        #endregion
    }
}
