namespace JsonTreeView
{
	partial class MainForm
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
            JsonTreeView = new System.Windows.Forms.TreeView();
            applicationControl1 = new ApplicationControl();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // JsonTreeView
            // 
            JsonTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            JsonTreeView.Location = new System.Drawing.Point(0, 0);
            JsonTreeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            JsonTreeView.Name = "JsonTreeView";
            JsonTreeView.Size = new System.Drawing.Size(253, 424);
            JsonTreeView.TabIndex = 0;
            // 
            // applicationControl1
            // 
            applicationControl1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            applicationControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            applicationControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            applicationControl1.ExeName = "Notepad.exe";
            applicationControl1.Location = new System.Drawing.Point(0, 0);
            applicationControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            applicationControl1.Name = "applicationControl1";
            applicationControl1.Size = new System.Drawing.Size(503, 424);
            applicationControl1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(JsonTreeView);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(applicationControl1);
            splitContainer1.Size = new System.Drawing.Size(760, 424);
            splitContainer1.SplitterDistance = 253;
            splitContainer1.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(760, 424);
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "JSON TreeView";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private ApplicationControl applicationControl1;
        private System.Windows.Forms.TreeView JsonTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

