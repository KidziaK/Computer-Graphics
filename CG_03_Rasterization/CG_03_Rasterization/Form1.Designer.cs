﻿namespace CG_03_Rasterization
{
    partial class Form1
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxDrawArea = new System.Windows.Forms.PictureBox();
            this.checkBoxCircle = new System.Windows.Forms.CheckBox();
            this.checkBoxLine = new System.Windows.Forms.CheckBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.checkBoxMarker = new System.Windows.Forms.CheckBox();
            this.checkBoxStar = new System.Windows.Forms.CheckBox();
            this.checkBoxPen = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDrawArea)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxDrawArea, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1455, 766);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.checkBoxCircle, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxLine, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonRefresh, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxMarker, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxStar, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxPen, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1449, 54);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // pictureBoxDrawArea
            // 
            this.pictureBoxDrawArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxDrawArea.InitialImage = null;
            this.pictureBoxDrawArea.Location = new System.Drawing.Point(3, 63);
            this.pictureBoxDrawArea.Name = "pictureBoxDrawArea";
            this.pictureBoxDrawArea.Size = new System.Drawing.Size(1449, 700);
            this.pictureBoxDrawArea.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDrawArea.TabIndex = 0;
            this.pictureBoxDrawArea.TabStop = false;
            this.pictureBoxDrawArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDrawArea_MouseUp);
            // 
            // checkBoxCircle
            // 
            this.checkBoxCircle.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCircle.BackgroundImage = global::CG_03_Rasterization.Properties.Resources.oval;
            this.checkBoxCircle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxCircle.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxCircle.Location = new System.Drawing.Point(63, 3);
            this.checkBoxCircle.Name = "checkBoxCircle";
            this.checkBoxCircle.Size = new System.Drawing.Size(54, 48);
            this.checkBoxCircle.TabIndex = 2;
            this.checkBoxCircle.UseVisualStyleBackColor = true;
            this.checkBoxCircle.CheckedChanged += new System.EventHandler(this.checkBoxCircle_CheckedChanged);
            // 
            // checkBoxLine
            // 
            this.checkBoxLine.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxLine.BackgroundImage = global::CG_03_Rasterization.Properties.Resources.Editing_Line_icon;
            this.checkBoxLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxLine.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxLine.Location = new System.Drawing.Point(3, 3);
            this.checkBoxLine.Name = "checkBoxLine";
            this.checkBoxLine.Size = new System.Drawing.Size(54, 48);
            this.checkBoxLine.TabIndex = 0;
            this.checkBoxLine.UseVisualStyleBackColor = true;
            this.checkBoxLine.CheckedChanged += new System.EventHandler(this.checkBoxLine_CheckedChanged);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.BackgroundImage = global::CG_03_Rasterization.Properties.Resources.reload;
            this.buttonRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRefresh.Location = new System.Drawing.Point(303, 3);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(48, 48);
            this.buttonRefresh.TabIndex = 3;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // checkBoxMarker
            // 
            this.checkBoxMarker.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMarker.BackgroundImage = global::CG_03_Rasterization.Properties.Resources.marker;
            this.checkBoxMarker.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxMarker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxMarker.Location = new System.Drawing.Point(123, 3);
            this.checkBoxMarker.Name = "checkBoxMarker";
            this.checkBoxMarker.Size = new System.Drawing.Size(54, 48);
            this.checkBoxMarker.TabIndex = 4;
            this.checkBoxMarker.UseVisualStyleBackColor = true;
            // 
            // checkBoxStar
            // 
            this.checkBoxStar.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxStar.BackgroundImage = global::CG_03_Rasterization.Properties.Resources.star;
            this.checkBoxStar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxStar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxStar.Location = new System.Drawing.Point(243, 3);
            this.checkBoxStar.Name = "checkBoxStar";
            this.checkBoxStar.Size = new System.Drawing.Size(54, 48);
            this.checkBoxStar.TabIndex = 5;
            this.checkBoxStar.UseVisualStyleBackColor = true;
            this.checkBoxStar.CheckedChanged += new System.EventHandler(this.checkBoxStar_CheckedChanged);
            // 
            // checkBoxPen
            // 
            this.checkBoxPen.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxPen.BackgroundImage = global::CG_03_Rasterization.Properties.Resources.pen;
            this.checkBoxPen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxPen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxPen.Location = new System.Drawing.Point(183, 3);
            this.checkBoxPen.Name = "checkBoxPen";
            this.checkBoxPen.Size = new System.Drawing.Size(54, 48);
            this.checkBoxPen.TabIndex = 6;
            this.checkBoxPen.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1455, 766);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDrawArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxDrawArea;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox checkBoxCircle;
        private System.Windows.Forms.CheckBox checkBoxLine;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.CheckBox checkBoxMarker;
        private System.Windows.Forms.CheckBox checkBoxStar;
        private System.Windows.Forms.CheckBox checkBoxPen;
    }
}

