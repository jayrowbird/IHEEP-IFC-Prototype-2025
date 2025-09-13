namespace WinFormsConsoleReadonly
{
    partial class IheepIFCPrototypeForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IheepIFCPrototypeForm));
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ShowBoundingBoxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            FindConnectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ClearSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ShowHide = new System.Windows.Forms.ToolStripMenuItem();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            JsonTreeView = new System.Windows.Forms.TreeView();
            LogTextBox = new System.Windows.Forms.TextBox();
            glControl1 = new OpenTK.GLControl.GLControl();
            ProcessInfoTextBox = new System.Windows.Forms.TextBox();
            timer2 = new System.Windows.Forms.Timer(components);
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPageOBJconversion = new System.Windows.Forms.TabPage();
            textBoxOBJProcessingText = new System.Windows.Forms.TextBox();
            tabPageXMLconversion = new System.Windows.Forms.TabPage();
            textBoxXMLProcessingText = new System.Windows.Forms.TextBox();
            tabPageViewer = new System.Windows.Forms.TabPage();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageOBJconversion.SuspendLayout();
            tabPageXMLconversion.SuspendLayout();
            tabPageViewer.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem, ShowHide });
            menuStrip1.Location = new System.Drawing.Point(3, 3);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.ShowItemToolTips = true;
            menuStrip1.Size = new System.Drawing.Size(889, 25);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, toolStripSeparator, toolStripSeparator1, printToolStripMenuItem, printPreviewToolStripMenuItem, toolStripSeparator2, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 21);
            fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("newToolStripMenuItem.Image");
            newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
            newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            newToolStripMenuItem.Text = "&New";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("openToolStripMenuItem.Image");
            openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            openToolStripMenuItem.Text = "&Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // printToolStripMenuItem
            // 
            printToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("printToolStripMenuItem.Image");
            printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            printToolStripMenuItem.Name = "printToolStripMenuItem";
            printToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P;
            printToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            printToolStripMenuItem.Text = "&Print";
            // 
            // printPreviewToolStripMenuItem
            // 
            printPreviewToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("printPreviewToolStripMenuItem.Image");
            printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            printPreviewToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            printPreviewToolStripMenuItem.Text = "Print Pre&view";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { undoToolStripMenuItem, redoToolStripMenuItem, toolStripSeparator3, cutToolStripMenuItem, copyToolStripMenuItem, pasteToolStripMenuItem, toolStripSeparator4, selectAllToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z;
            undoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y;
            redoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            redoToolStripMenuItem.Text = "&Redo";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("cutToolStripMenuItem.Image");
            cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X;
            cutToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            cutToolStripMenuItem.Text = "Cu&t";
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("copyToolStripMenuItem.Image");
            copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            copyToolStripMenuItem.Text = "&Copy";
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("pasteToolStripMenuItem.Image");
            pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            pasteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            pasteToolStripMenuItem.Text = "&Paste";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(141, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            selectAllToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            selectAllToolStripMenuItem.Text = "Select &All";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ShowBoundingBoxesToolStripMenuItem, FindConnectionsToolStripMenuItem, ClearSelectionToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 21);
            toolsToolStripMenuItem.Text = "&Tools";
            // 
            // ShowBoundingBoxesToolStripMenuItem
            // 
            ShowBoundingBoxesToolStripMenuItem.CheckOnClick = true;
            ShowBoundingBoxesToolStripMenuItem.Name = "ShowBoundingBoxesToolStripMenuItem";
            ShowBoundingBoxesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            ShowBoundingBoxesToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            ShowBoundingBoxesToolStripMenuItem.Text = "&Show Bounding Boxes";
            ShowBoundingBoxesToolStripMenuItem.Click += ShowBoundingBoxesToolStripMenuItem_Click;
            // 
            // FindConnectionsToolStripMenuItem
            // 
            FindConnectionsToolStripMenuItem.CheckOnClick = true;
            FindConnectionsToolStripMenuItem.Enabled = false;
            FindConnectionsToolStripMenuItem.Name = "FindConnectionsToolStripMenuItem";
            FindConnectionsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F;
            FindConnectionsToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            FindConnectionsToolStripMenuItem.Text = "&Find Connections";
            FindConnectionsToolStripMenuItem.Click += FindConnectionsToolStripMenuItem_Click;
            // 
            // ClearSelectionToolStripMenuItem
            // 
            ClearSelectionToolStripMenuItem.Enabled = false;
            ClearSelectionToolStripMenuItem.Name = "ClearSelectionToolStripMenuItem";
            ClearSelectionToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            ClearSelectionToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            ClearSelectionToolStripMenuItem.Text = "&Clear Selected Model/Models";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { contentsToolStripMenuItem, toolStripSeparator5, aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            contentsToolStripMenuItem.Text = "&Contents";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            aboutToolStripMenuItem.Text = "&About...";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // ShowHide
            // 
            ShowHide.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            ShowHide.AutoToolTip = true;
            ShowHide.BackColor = System.Drawing.SystemColors.AppWorkspace;
            ShowHide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            ShowHide.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            ShowHide.Name = "ShowHide";
            ShowHide.Size = new System.Drawing.Size(29, 21);
            ShowHide.Text = ">";
            ShowHide.ToolTipText = "Button to show or hide the Process info text";
            ShowHide.Click += ShowHide_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(3, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = System.Drawing.Color.Black;
            splitContainer1.Panel2.Controls.Add(glControl1);
            splitContainer1.Size = new System.Drawing.Size(875, 420);
            splitContainer1.SplitterDistance = 129;
            splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(JsonTreeView);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(LogTextBox);
            splitContainer2.Size = new System.Drawing.Size(129, 420);
            splitContainer2.SplitterDistance = 309;
            splitContainer2.TabIndex = 0;
            // 
            // JsonTreeView
            // 
            JsonTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            JsonTreeView.Location = new System.Drawing.Point(0, 0);
            JsonTreeView.Name = "JsonTreeView";
            JsonTreeView.Size = new System.Drawing.Size(129, 309);
            JsonTreeView.TabIndex = 0;
            // 
            // LogTextBox
            // 
            LogTextBox.BackColor = System.Drawing.Color.LemonChiffon;
            LogTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            LogTextBox.Location = new System.Drawing.Point(0, 0);
            LogTextBox.Multiline = true;
            LogTextBox.Name = "LogTextBox";
            LogTextBox.Size = new System.Drawing.Size(129, 107);
            LogTextBox.TabIndex = 0;
            // 
            // glControl1
            // 
            glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl1.APIVersion = new System.Version(3, 3, 0, 0);
            glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl1.IsEventDriven = true;
            glControl1.Location = new System.Drawing.Point(0, 0);
            glControl1.Name = "glControl1";
            glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            glControl1.SharedContext = null;
            glControl1.Size = new System.Drawing.Size(742, 420);
            glControl1.TabIndex = 0;
            // 
            // ProcessInfoTextBox
            // 
            ProcessInfoTextBox.BackColor = System.Drawing.Color.Cornsilk;
            ProcessInfoTextBox.Dock = System.Windows.Forms.DockStyle.Right;
            ProcessInfoTextBox.Location = new System.Drawing.Point(626, 3);
            ProcessInfoTextBox.Multiline = true;
            ProcessInfoTextBox.Name = "ProcessInfoTextBox";
            ProcessInfoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            ProcessInfoTextBox.Size = new System.Drawing.Size(252, 420);
            ProcessInfoTextBox.TabIndex = 1;
            ProcessInfoTextBox.Text = resources.GetString("ProcessInfoTextBox.Text");
            ProcessInfoTextBox.WordWrap = false;
            // 
            // timer2
            // 
            timer2.Tick += timer2_Tick;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageOBJconversion);
            tabControl1.Controls.Add(tabPageXMLconversion);
            tabControl1.Controls.Add(tabPageViewer);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(3, 28);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(889, 454);
            tabControl1.TabIndex = 2;
            // 
            // tabPageOBJconversion
            // 
            tabPageOBJconversion.Controls.Add(textBoxOBJProcessingText);
            tabPageOBJconversion.Controls.Add(ProcessInfoTextBox);
            tabPageOBJconversion.Location = new System.Drawing.Point(4, 24);
            tabPageOBJconversion.Name = "tabPageOBJconversion";
            tabPageOBJconversion.Padding = new System.Windows.Forms.Padding(3);
            tabPageOBJconversion.Size = new System.Drawing.Size(881, 426);
            tabPageOBJconversion.TabIndex = 0;
            tabPageOBJconversion.Text = "IFC->Export obj file";
            tabPageOBJconversion.UseVisualStyleBackColor = true;
            // 
            // textBoxOBJProcessingText
            // 
            textBoxOBJProcessingText.BackColor = System.Drawing.SystemColors.InfoText;
            textBoxOBJProcessingText.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxOBJProcessingText.ForeColor = System.Drawing.SystemColors.HighlightText;
            textBoxOBJProcessingText.Location = new System.Drawing.Point(3, 3);
            textBoxOBJProcessingText.Multiline = true;
            textBoxOBJProcessingText.Name = "textBoxOBJProcessingText";
            textBoxOBJProcessingText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            textBoxOBJProcessingText.Size = new System.Drawing.Size(623, 420);
            textBoxOBJProcessingText.TabIndex = 2;
            // 
            // tabPageXMLconversion
            // 
            tabPageXMLconversion.Controls.Add(textBoxXMLProcessingText);
            tabPageXMLconversion.Location = new System.Drawing.Point(4, 24);
            tabPageXMLconversion.Name = "tabPageXMLconversion";
            tabPageXMLconversion.Padding = new System.Windows.Forms.Padding(3);
            tabPageXMLconversion.Size = new System.Drawing.Size(881, 426);
            tabPageXMLconversion.TabIndex = 1;
            tabPageXMLconversion.Text = "IFC->Export xml file";
            tabPageXMLconversion.UseVisualStyleBackColor = true;
            // 
            // textBoxXMLProcessingText
            // 
            textBoxXMLProcessingText.BackColor = System.Drawing.SystemColors.InfoText;
            textBoxXMLProcessingText.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxXMLProcessingText.ForeColor = System.Drawing.SystemColors.HighlightText;
            textBoxXMLProcessingText.Location = new System.Drawing.Point(3, 3);
            textBoxXMLProcessingText.Multiline = true;
            textBoxXMLProcessingText.Name = "textBoxXMLProcessingText";
            textBoxXMLProcessingText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            textBoxXMLProcessingText.Size = new System.Drawing.Size(875, 420);
            textBoxXMLProcessingText.TabIndex = 4;
            // 
            // tabPageViewer
            // 
            tabPageViewer.Controls.Add(splitContainer1);
            tabPageViewer.Location = new System.Drawing.Point(4, 24);
            tabPageViewer.Name = "tabPageViewer";
            tabPageViewer.Padding = new System.Windows.Forms.Padding(3);
            tabPageViewer.Size = new System.Drawing.Size(881, 426);
            tabPageViewer.TabIndex = 2;
            tabPageViewer.Text = "IFC->obj viewer";
            tabPageViewer.UseVisualStyleBackColor = true;
            // 
            // IheepIFCPrototype
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(895, 485);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            Name = "IheepIFCPrototype";
            Padding = new System.Windows.Forms.Padding(3);
            Text = "IheepIFCPrototype";
            FormClosing += IheepIFCPrototype_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPageOBJconversion.ResumeLayout(false);
            tabPageOBJconversion.PerformLayout();
            tabPageXMLconversion.ResumeLayout(false);
            tabPageXMLconversion.PerformLayout();
            tabPageViewer.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion 
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowHide;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel SlidingPanel;
        private System.Windows.Forms.TextBox ProcessInfoTextBox;
        internal OpenTK.GLControl.GLControl glControl1;
        internal System.Windows.Forms.TreeView JsonTreeView;
        internal System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageOBJconversion;
        private System.Windows.Forms.TabPage tabPageXMLconversion;
        private System.Windows.Forms.TabPage tabPageViewer;
        private System.Windows.Forms.TextBox textBoxOBJProcessingText;
        private System.Windows.Forms.TextBox textBoxXMLProcessingText;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowBoundingBoxesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FindConnectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearSelectionToolStripMenuItem;
    }
}