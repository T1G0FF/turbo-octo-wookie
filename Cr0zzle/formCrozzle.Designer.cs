namespace Assignment1
{
    partial class formCrozzle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridCrozzle = new System.Windows.Forms.DataGridView();
            this.listCrozzle = new System.Windows.Forms.ListBox();
            this.btnSelectCrozzle = new System.Windows.Forms.Button();
            this.btnSelectWordlist = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.lblScoreTitle = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridCrozzle)).BeginInit();
            this.SuspendLayout();
            // 
            // gridCrozzle
            // 
            this.gridCrozzle.AllowUserToAddRows = false;
            this.gridCrozzle.AllowUserToDeleteRows = false;
            this.gridCrozzle.AllowUserToResizeColumns = false;
            this.gridCrozzle.AllowUserToResizeRows = false;
            this.gridCrozzle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCrozzle.ColumnHeadersVisible = false;
            this.gridCrozzle.Location = new System.Drawing.Point(12, 12);
            this.gridCrozzle.Name = "gridCrozzle";
            this.gridCrozzle.ReadOnly = true;
            this.gridCrozzle.Size = new System.Drawing.Size(300, 300);
            this.gridCrozzle.TabIndex = 0;
            this.gridCrozzle.SelectionChanged += new System.EventHandler(this.gridCrozzle_SelectionChanged);
            // 
            // listCrozzle
            // 
            this.listCrozzle.FormattingEnabled = true;
            this.listCrozzle.Location = new System.Drawing.Point(324, 38);
            this.listCrozzle.Name = "listCrozzle";
            this.listCrozzle.Size = new System.Drawing.Size(150, 277);
            this.listCrozzle.TabIndex = 1;
            // 
            // btnSelectCrozzle
            // 
            this.btnSelectCrozzle.Location = new System.Drawing.Point(62, 318);
            this.btnSelectCrozzle.Name = "btnSelectCrozzle";
            this.btnSelectCrozzle.Size = new System.Drawing.Size(100, 23);
            this.btnSelectCrozzle.TabIndex = 2;
            this.btnSelectCrozzle.Text = "Load Crozzle";
            this.btnSelectCrozzle.UseVisualStyleBackColor = true;
            this.btnSelectCrozzle.Click += new System.EventHandler(this.btnSelectCrozzle_Click);
            // 
            // btnSelectWordlist
            // 
            this.btnSelectWordlist.Location = new System.Drawing.Point(162, 318);
            this.btnSelectWordlist.Name = "btnSelectWordlist";
            this.btnSelectWordlist.Size = new System.Drawing.Size(100, 23);
            this.btnSelectWordlist.TabIndex = 3;
            this.btnSelectWordlist.Text = "Load Word list";
            this.btnSelectWordlist.UseVisualStyleBackColor = true;
            this.btnSelectWordlist.Click += new System.EventHandler(this.btnSelectWordlist_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(349, 317);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(100, 23);
            this.btnValidate.TabIndex = 4;
            this.btnValidate.Text = "Validate";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // lblScoreTitle
            // 
            this.lblScoreTitle.AutoSize = true;
            this.lblScoreTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreTitle.Location = new System.Drawing.Point(324, 12);
            this.lblScoreTitle.Name = "lblScoreTitle";
            this.lblScoreTitle.Size = new System.Drawing.Size(44, 13);
            this.lblScoreTitle.TabIndex = 5;
            this.lblScoreTitle.Text = "Score:";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(374, 12);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(13, 13);
            this.lblScore.TabIndex = 6;
            this.lblScore.Text = "--";
            // 
            // formCrozzle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(484, 352);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.lblScoreTitle);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.btnSelectWordlist);
            this.Controls.Add(this.btnSelectCrozzle);
            this.Controls.Add(this.listCrozzle);
            this.Controls.Add(this.gridCrozzle);
            this.Name = "formCrozzle";
            this.Text = "Crozzle";
            ((System.ComponentModel.ISupportInitialize)(this.gridCrozzle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridCrozzle;
        private System.Windows.Forms.ListBox listCrozzle;
        private System.Windows.Forms.Button btnSelectCrozzle;
        private System.Windows.Forms.Button btnSelectWordlist;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Label lblScoreTitle;
        private System.Windows.Forms.Label lblScore;
    }
}

