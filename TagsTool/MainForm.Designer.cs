namespace TagsTool
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ListViewItem listViewItem1 = new ListViewItem(new string[] { "这里是tag名字", "这里是tag时间", "这里是tag对应提交的作者", "这里是tag对应提交信息" }, -1);
            contextMenuStrip1 = new ContextMenuStrip(components);
            menuStrip2 = new MenuStrip();
            WorkspaceMenuItem = new ToolStripMenuItem();
            SelectRegionMenu = new ToolStripMenuItem();
            ToolStripMenuItem_TW = new ToolStripMenuItem();
            RefreshToolMenu = new ToolStripMenuItem();
            帮助ToolStripMenuItem = new ToolStripMenuItem();
            HelpOpenConfigMenuItem = new ToolStripMenuItem();
            HelpDebugLogMenuItem = new ToolStripMenuItem();
            HelpOpenGitWebTagMenuItem = new ToolStripMenuItem();
            HelpOpenPropsPathMenuItem = new ToolStripMenuItem();
            HelpCleanLogMenuItem = new ToolStripMenuItem();
            HelpCleanCacheMenuItem = new ToolStripMenuItem();
            HelpOpenHelpDocMenuItem = new ToolStripMenuItem();
            TabBuild = new TabControl();
            TabBuildPatch = new TabPage();
            BuildPatchTableLayout = new TableLayoutPanel();
            button1 = new Button();
            TabBuildFull = new TabPage();
            BuildFullTableLayout = new TableLayoutPanel();
            button2 = new Button();
            label3 = new Label();
            DropDownBranch = new ComboBox();
            SaveBranchBtn = new Button();
            DeleteBranchBtn = new Button();
            statusStrip1 = new StatusStrip();
            StatusLabel = new ToolStripStatusLabel();
            VersionLabel = new Label();
            TagTextLabel = new Label();
            TagTextBox = new TextBox();
            PushTagBtn = new Button();
            VersionText = new TextBox();
            BranchTips = new Label();
            VersionTips = new Label();
            TagTips = new Label();
            TagListView = new ListView();
            TitleTagName = new ColumnHeader();
            TitleTagTime = new ColumnHeader();
            TitleTagAuthor = new ColumnHeader();
            TitleTagMessage = new ColumnHeader();
            menuStrip2.SuspendLayout();
            TabBuild.SuspendLayout();
            TabBuildPatch.SuspendLayout();
            BuildPatchTableLayout.SuspendLayout();
            TabBuildFull.SuspendLayout();
            BuildFullTableLayout.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // menuStrip2
            // 
            menuStrip2.Items.AddRange(new ToolStripItem[] { WorkspaceMenuItem, SelectRegionMenu, RefreshToolMenu, 帮助ToolStripMenuItem });
            menuStrip2.Location = new Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(802, 25);
            menuStrip2.TabIndex = 2;
            menuStrip2.Text = "menuStrip2";
            // 
            // WorkspaceMenuItem
            // 
            WorkspaceMenuItem.Name = "WorkspaceMenuItem";
            WorkspaceMenuItem.Size = new Size(92, 21);
            WorkspaceMenuItem.Text = "选择工程目录";
            WorkspaceMenuItem.Click += WorkspacrMenuItem_Click;
            // 
            // SelectRegionMenu
            // 
            SelectRegionMenu.DropDownItems.AddRange(new ToolStripItem[] { ToolStripMenuItem_TW });
            SelectRegionMenu.ImageAlign = ContentAlignment.MiddleLeft;
            SelectRegionMenu.ImageScaling = ToolStripItemImageScaling.None;
            SelectRegionMenu.Name = "SelectRegionMenu";
            SelectRegionMenu.Size = new Size(92, 21);
            SelectRegionMenu.Text = "地区：东南亚";
            // 
            // ToolStripMenuItem_TW
            // 
            ToolStripMenuItem_TW.Name = "ToolStripMenuItem_TW";
            ToolStripMenuItem_TW.Size = new Size(100, 22);
            ToolStripMenuItem_TW.Tag = "RegionTW";
            ToolStripMenuItem_TW.Text = "港台";
            ToolStripMenuItem_TW.Click += ToolStripMenuItem_TW_Click;
            // 
            // RefreshToolMenu
            // 
            RefreshToolMenu.Name = "RefreshToolMenu";
            RefreshToolMenu.Size = new Size(44, 21);
            RefreshToolMenu.Text = "刷新";
            RefreshToolMenu.Click += RefreshToolMenu_Click;
            // 
            // 帮助ToolStripMenuItem
            // 
            帮助ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { HelpOpenConfigMenuItem, HelpDebugLogMenuItem, HelpOpenGitWebTagMenuItem, HelpOpenPropsPathMenuItem, HelpCleanLogMenuItem, HelpCleanCacheMenuItem, HelpOpenHelpDocMenuItem });
            帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            帮助ToolStripMenuItem.Size = new Size(44, 21);
            帮助ToolStripMenuItem.Text = "帮助";
            // 
            // HelpOpenConfigMenuItem
            // 
            HelpOpenConfigMenuItem.Name = "HelpOpenConfigMenuItem";
            HelpOpenConfigMenuItem.Size = new Size(185, 22);
            HelpOpenConfigMenuItem.Text = "打开配置文件";
            HelpOpenConfigMenuItem.Click += HelpOpenConfigMenuItem_Click;
            // 
            // HelpDebugLogMenuItem
            // 
            HelpDebugLogMenuItem.Name = "HelpDebugLogMenuItem";
            HelpDebugLogMenuItem.Size = new Size(185, 22);
            HelpDebugLogMenuItem.Text = "打开日志文件";
            HelpDebugLogMenuItem.Click += HelpDebugLogMenuItem_Click;
            // 
            // HelpOpenGitWebTagMenuItem
            // 
            HelpOpenGitWebTagMenuItem.Name = "HelpOpenGitWebTagMenuItem";
            HelpOpenGitWebTagMenuItem.Size = new Size(185, 22);
            HelpOpenGitWebTagMenuItem.Text = "打开Gitlab/Tag页面";
            HelpOpenGitWebTagMenuItem.Click += HelpOpenGitWebTagMenuItem_Click;
            // 
            // HelpOpenPropsPathMenuItem
            // 
            HelpOpenPropsPathMenuItem.Name = "HelpOpenPropsPathMenuItem";
            HelpOpenPropsPathMenuItem.Size = new Size(185, 22);
            HelpOpenPropsPathMenuItem.Text = "打开缓存目录";
            HelpOpenPropsPathMenuItem.Click += HelpOpenPropsPath_Click;
            // 
            // HelpCleanLogMenuItem
            // 
            HelpCleanLogMenuItem.Name = "HelpCleanLogMenuItem";
            HelpCleanLogMenuItem.Size = new Size(185, 22);
            HelpCleanLogMenuItem.Text = "清除日志";
            HelpCleanLogMenuItem.Click += HelpCleanLogMenuItem_Click;
            // 
            // HelpCleanCacheMenuItem
            // 
            HelpCleanCacheMenuItem.Name = "HelpCleanCacheMenuItem";
            HelpCleanCacheMenuItem.Size = new Size(185, 22);
            HelpCleanCacheMenuItem.Text = "清除缓存";
            HelpCleanCacheMenuItem.Click += HelpCleanCacheMenuItem_Click;
            // 
            // HelpOpenHelpDocMenuItem
            // 
            HelpOpenHelpDocMenuItem.Name = "HelpOpenHelpDocMenuItem";
            HelpOpenHelpDocMenuItem.Size = new Size(185, 22);
            HelpOpenHelpDocMenuItem.Text = "帮助文档";
            HelpOpenHelpDocMenuItem.Click += HelpOpenHelpDoc_Click;
            // 
            // TabBuild
            // 
            TabBuild.Controls.Add(TabBuildPatch);
            TabBuild.Controls.Add(TabBuildFull);
            TabBuild.Location = new Point(0, 28);
            TabBuild.Name = "TabBuild";
            TabBuild.SelectedIndex = 0;
            TabBuild.Size = new Size(800, 128);
            TabBuild.TabIndex = 3;
            // 
            // TabBuildPatch
            // 
            TabBuildPatch.Controls.Add(BuildPatchTableLayout);
            TabBuildPatch.Location = new Point(4, 26);
            TabBuildPatch.Name = "TabBuildPatch";
            TabBuildPatch.Padding = new Padding(3);
            TabBuildPatch.Size = new Size(792, 98);
            TabBuildPatch.TabIndex = 0;
            TabBuildPatch.Text = "补丁包";
            TabBuildPatch.UseVisualStyleBackColor = true;
            // 
            // BuildPatchTableLayout
            // 
            BuildPatchTableLayout.ColumnCount = 5;
            BuildPatchTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            BuildPatchTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            BuildPatchTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            BuildPatchTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            BuildPatchTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            BuildPatchTableLayout.Controls.Add(button1, 0, 0);
            BuildPatchTableLayout.Location = new Point(3, 3);
            BuildPatchTableLayout.Name = "BuildPatchTableLayout";
            BuildPatchTableLayout.RowCount = 3;
            BuildPatchTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            BuildPatchTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            BuildPatchTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            BuildPatchTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            BuildPatchTableLayout.Size = new Size(786, 92);
            BuildPatchTableLayout.TabIndex = 10;
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.ForeColor = SystemColors.ControlText;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(151, 24);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = false;
            button1.Click += Build_Type_Click;
            // 
            // TabBuildFull
            // 
            TabBuildFull.Controls.Add(BuildFullTableLayout);
            TabBuildFull.Location = new Point(4, 26);
            TabBuildFull.Name = "TabBuildFull";
            TabBuildFull.Padding = new Padding(3);
            TabBuildFull.Size = new Size(792, 98);
            TabBuildFull.TabIndex = 1;
            TabBuildFull.Text = "完整包";
            TabBuildFull.UseVisualStyleBackColor = true;
            // 
            // BuildFullTableLayout
            // 
            BuildFullTableLayout.ColumnCount = 5;
            BuildFullTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            BuildFullTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            BuildFullTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            BuildFullTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            BuildFullTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            BuildFullTableLayout.Controls.Add(button2, 0, 0);
            BuildFullTableLayout.Location = new Point(3, 3);
            BuildFullTableLayout.Name = "BuildFullTableLayout";
            BuildFullTableLayout.RowCount = 3;
            BuildFullTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            BuildFullTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            BuildFullTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            BuildFullTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            BuildFullTableLayout.Size = new Size(786, 92);
            BuildFullTableLayout.TabIndex = 11;
            // 
            // button2
            // 
            button2.BackColor = Color.Transparent;
            button2.ForeColor = SystemColors.ControlText;
            button2.Location = new Point(3, 3);
            button2.Name = "button2";
            button2.Size = new Size(151, 24);
            button2.TabIndex = 0;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.CausesValidation = false;
            label3.Location = new Point(12, 302);
            label3.Name = "label3";
            label3.Size = new Size(44, 17);
            label3.TabIndex = 4;
            label3.Text = "分支：";
            // 
            // DropDownBranch
            // 
            DropDownBranch.FormattingEnabled = true;
            DropDownBranch.Items.AddRange(new object[] { "a", "s" });
            DropDownBranch.Location = new Point(50, 299);
            DropDownBranch.Name = "DropDownBranch";
            DropDownBranch.Size = new Size(566, 25);
            DropDownBranch.TabIndex = 5;
            DropDownBranch.TextChanged += DropDownBranch_TextChanged;
            // 
            // SaveBranchBtn
            // 
            SaveBranchBtn.Location = new Point(622, 299);
            SaveBranchBtn.Name = "SaveBranchBtn";
            SaveBranchBtn.Size = new Size(83, 23);
            SaveBranchBtn.TabIndex = 6;
            SaveBranchBtn.Text = "保存分支名";
            SaveBranchBtn.UseVisualStyleBackColor = true;
            SaveBranchBtn.Click += SaveBranchBtn_Click;
            // 
            // DeleteBranchBtn
            // 
            DeleteBranchBtn.Location = new Point(711, 299);
            DeleteBranchBtn.Name = "DeleteBranchBtn";
            DeleteBranchBtn.Size = new Size(85, 23);
            DeleteBranchBtn.TabIndex = 7;
            DeleteBranchBtn.Text = "删除分支名";
            DeleteBranchBtn.UseVisualStyleBackColor = true;
            DeleteBranchBtn.Click += DeleteBranchBtn_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { StatusLabel });
            statusStrip1.Location = new Point(0, 400);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(802, 22);
            statusStrip1.TabIndex = 8;
            statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            StatusLabel.ForeColor = SystemColors.ControlText;
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new Size(56, 17);
            StatusLabel.Text = "状态信息";
            // 
            // VersionLabel
            // 
            VersionLabel.AutoSize = true;
            VersionLabel.Location = new Point(12, 336);
            VersionLabel.Name = "VersionLabel";
            VersionLabel.Size = new Size(44, 17);
            VersionLabel.TabIndex = 9;
            VersionLabel.Text = "版本：";
            // 
            // TagTextLabel
            // 
            TagTextLabel.AutoSize = true;
            TagTextLabel.Location = new Point(12, 368);
            TagTextLabel.Name = "TagTextLabel";
            TagTextLabel.Size = new Size(42, 17);
            TagTextLabel.TabIndex = 10;
            TagTextLabel.Text = "Tag：";
            // 
            // TagTextBox
            // 
            TagTextBox.Location = new Point(50, 365);
            TagTextBox.Name = "TagTextBox";
            TagTextBox.Size = new Size(566, 23);
            TagTextBox.TabIndex = 11;
            TagTextBox.TextChanged += TagTextBox_TextChanged;
            // 
            // PushTagBtn
            // 
            PushTagBtn.Location = new Point(622, 365);
            PushTagBtn.Name = "PushTagBtn";
            PushTagBtn.Size = new Size(83, 23);
            PushTagBtn.TabIndex = 12;
            PushTagBtn.Text = "提交";
            PushTagBtn.UseVisualStyleBackColor = true;
            PushTagBtn.Click += PushTagBtn_Click;
            // 
            // VersionText
            // 
            VersionText.Location = new Point(50, 333);
            VersionText.Name = "VersionText";
            VersionText.ReadOnly = true;
            VersionText.Size = new Size(566, 23);
            VersionText.TabIndex = 13;
            // 
            // BranchTips
            // 
            BranchTips.AutoSize = true;
            BranchTips.BackColor = SystemColors.Control;
            BranchTips.BorderStyle = BorderStyle.FixedSingle;
            BranchTips.ForeColor = SystemColors.Highlight;
            BranchTips.Location = new Point(525, 302);
            BranchTips.Name = "BranchTips";
            BranchTips.Size = new Size(70, 19);
            BranchTips.TabIndex = 15;
            BranchTips.Text = "分支不存在";
            // 
            // VersionTips
            // 
            VersionTips.AutoSize = true;
            VersionTips.BackColor = SystemColors.Control;
            VersionTips.BorderStyle = BorderStyle.FixedSingle;
            VersionTips.ForeColor = SystemColors.Highlight;
            VersionTips.Location = new Point(513, 335);
            VersionTips.Name = "VersionTips";
            VersionTips.Size = new Size(82, 19);
            VersionTips.TabIndex = 16;
            VersionTips.Text = "版本获取失败";
            // 
            // TagTips
            // 
            TagTips.AutoSize = true;
            TagTips.BackColor = SystemColors.Control;
            TagTips.BorderStyle = BorderStyle.FixedSingle;
            TagTips.ForeColor = SystemColors.Highlight;
            TagTips.Location = new Point(527, 367);
            TagTips.Name = "TagTips";
            TagTips.Size = new Size(68, 19);
            TagTips.TabIndex = 17;
            TagTips.Text = "Tag已存在";
            // 
            // TagListView
            // 
            TagListView.Columns.AddRange(new ColumnHeader[] { TitleTagName, TitleTagTime, TitleTagAuthor, TitleTagMessage });
            TagListView.FullRowSelect = true;
            listViewItem1.ToolTipText = "创建时间";
            TagListView.Items.AddRange(new ListViewItem[] { listViewItem1 });
            TagListView.Location = new Point(7, 158);
            TagListView.MultiSelect = false;
            TagListView.Name = "TagListView";
            TagListView.Size = new Size(789, 135);
            TagListView.TabIndex = 18;
            TagListView.UseCompatibleStateImageBehavior = false;
            TagListView.View = View.Details;
            TagListView.SelectedIndexChanged += TagListView_SelectedIndexChanged;
            // 
            // TitleTagName
            // 
            TitleTagName.Text = "Tag名字";
            TitleTagName.Width = 300;
            // 
            // TitleTagTime
            // 
            TitleTagTime.Text = "提交时间";
            TitleTagTime.Width = 150;
            // 
            // TitleTagAuthor
            // 
            TitleTagAuthor.Text = "提交作者";
            TitleTagAuthor.Width = 70;
            // 
            // TitleTagMessage
            // 
            TitleTagMessage.Text = "提交信息";
            TitleTagMessage.Width = 320;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(802, 422);
            Controls.Add(TagListView);
            Controls.Add(TagTips);
            Controls.Add(VersionTips);
            Controls.Add(BranchTips);
            Controls.Add(VersionText);
            Controls.Add(PushTagBtn);
            Controls.Add(TagTextBox);
            Controls.Add(TagTextLabel);
            Controls.Add(VersionLabel);
            Controls.Add(statusStrip1);
            Controls.Add(DeleteBranchBtn);
            Controls.Add(SaveBranchBtn);
            Controls.Add(DropDownBranch);
            Controls.Add(label3);
            Controls.Add(TabBuild);
            Controls.Add(menuStrip2);
            MaximizeBox = false;
            Name = "MainForm";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "打Tag工具";
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            TabBuild.ResumeLayout(false);
            TabBuildPatch.ResumeLayout(false);
            BuildPatchTableLayout.ResumeLayout(false);
            TabBuildFull.ResumeLayout(false);
            BuildFullTableLayout.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ContextMenuStrip contextMenuStrip1;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem SelectRegionMenu;
        private ToolStripMenuItem ToolStripMenuItem_TW;
        private TabControl TabBuild;
        private TabPage TabBuildPatch;
        private TabPage TabBuildFull;
        private Label label3;
        private ComboBox DropDownBranch;
        private Button SaveBranchBtn;
        private Button DeleteBranchBtn;
        private TableLayoutPanel BuildPatchTableLayout;
        private Button button1;
        private TableLayoutPanel BuildFullTableLayout;
        private Button button2;
        private StatusStrip statusStrip1;
        private Label VersionLabel;
        private Label TagTextLabel;
        private TextBox TagTextBox;
        private Button PushTagBtn;
        private TextBox VersionText;
        private ToolStripMenuItem RefreshToolMenu;
        private ToolStripMenuItem WorkspaceMenuItem;
        private ToolStripStatusLabel StatusLabel;
        private Label BranchTips;
        private Label VersionTips;
        private Label TagTips;
        private ToolStripMenuItem 帮助ToolStripMenuItem;
        private ToolStripMenuItem HelpDebugLogMenuItem;
        private ToolStripMenuItem HelpOpenGitWebTagMenuItem;
        private ToolStripMenuItem HelpOpenPropsPathMenuItem;
        private ToolStripMenuItem HelpOpenHelpDocMenuItem;
        private ToolStripMenuItem HelpOpenConfigMenuItem;
        private ListView TagListView;
        private ColumnHeader TitleTagName;
        private ColumnHeader TitleTagTime;
        private ColumnHeader TitleTagMessage;
        private ToolStripMenuItem HelpCleanCacheMenuItem;
        private ToolStripMenuItem HelpCleanLogMenuItem;
        private ColumnHeader TitleTagAuthor;
    }
}
