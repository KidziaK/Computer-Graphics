namespace CG_04_ClippingAndFilling
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
            this.checkBoxLineDrawing = new System.Windows.Forms.CheckBox();
            this.checkBoxClipping = new System.Windows.Forms.CheckBox();
            this.checkBoxPolygon = new System.Windows.Forms.CheckBox();
            this.pictureBoxDrawingArea = new System.Windows.Forms.PictureBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonFill = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDrawingArea)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxDrawingArea, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1109, 547);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.buttonReset, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxLineDrawing, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxClipping, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxPolygon, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.buttonFill, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(779, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(327, 541);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // checkBoxLineDrawing
            // 
            this.checkBoxLineDrawing.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxLineDrawing.AutoSize = true;
            this.checkBoxLineDrawing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxLineDrawing.Location = new System.Drawing.Point(3, 3);
            this.checkBoxLineDrawing.Name = "checkBoxLineDrawing";
            this.checkBoxLineDrawing.Size = new System.Drawing.Size(321, 102);
            this.checkBoxLineDrawing.TabIndex = 4;
            this.checkBoxLineDrawing.Text = "Line Drawing";
            this.checkBoxLineDrawing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxLineDrawing.UseVisualStyleBackColor = true;
            this.checkBoxLineDrawing.CheckedChanged += new System.EventHandler(this.checkBoxLineDrawing_CheckedChanged);
            // 
            // checkBoxClipping
            // 
            this.checkBoxClipping.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxClipping.AutoSize = true;
            this.checkBoxClipping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxClipping.Location = new System.Drawing.Point(3, 111);
            this.checkBoxClipping.Name = "checkBoxClipping";
            this.checkBoxClipping.Size = new System.Drawing.Size(321, 102);
            this.checkBoxClipping.TabIndex = 5;
            this.checkBoxClipping.Text = "Clipping";
            this.checkBoxClipping.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxClipping.UseVisualStyleBackColor = true;
            this.checkBoxClipping.CheckedChanged += new System.EventHandler(this.checkBoxClipping_CheckedChanged);
            // 
            // checkBoxPolygon
            // 
            this.checkBoxPolygon.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxPolygon.AutoSize = true;
            this.checkBoxPolygon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxPolygon.Location = new System.Drawing.Point(3, 219);
            this.checkBoxPolygon.Name = "checkBoxPolygon";
            this.checkBoxPolygon.Size = new System.Drawing.Size(321, 102);
            this.checkBoxPolygon.TabIndex = 6;
            this.checkBoxPolygon.Text = "Polygon";
            this.checkBoxPolygon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxPolygon.UseVisualStyleBackColor = true;
            this.checkBoxPolygon.CheckedChanged += new System.EventHandler(this.checkBoxPolygon_CheckedChanged);
            // 
            // pictureBoxDrawingArea
            // 
            this.pictureBoxDrawingArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxDrawingArea.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxDrawingArea.Name = "pictureBoxDrawingArea";
            this.pictureBoxDrawingArea.Size = new System.Drawing.Size(770, 541);
            this.pictureBoxDrawingArea.TabIndex = 1;
            this.pictureBoxDrawingArea.TabStop = false;
            this.pictureBoxDrawingArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDrawingArea_MouseUp);
            // 
            // buttonReset
            // 
            this.buttonReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReset.Location = new System.Drawing.Point(3, 435);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(321, 103);
            this.buttonReset.TabIndex = 7;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonFill
            // 
            this.buttonFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFill.Location = new System.Drawing.Point(3, 327);
            this.buttonFill.Name = "buttonFill";
            this.buttonFill.Size = new System.Drawing.Size(321, 102);
            this.buttonFill.TabIndex = 8;
            this.buttonFill.Text = "Fill";
            this.buttonFill.UseVisualStyleBackColor = true;
            this.buttonFill.Click += new System.EventHandler(this.buttonFill_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 547);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDrawingArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBoxDrawingArea;
        private System.Windows.Forms.CheckBox checkBoxLineDrawing;
        private System.Windows.Forms.CheckBox checkBoxClipping;
        private System.Windows.Forms.CheckBox checkBoxPolygon;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonFill;
    }
}

