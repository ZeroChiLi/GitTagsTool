using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TagsTool
{
    public partial class MainForm : Form
    {
        public string GitWorkspacePath;
        public string CurRegion;
        public string CurBranch;
        public string CurTagPrefix;
        public string CurVersion;
        public string CurTag { set { TagTextBox.Text = value; } get { return TagTextBox.Text; } }
        private ulong _statusIdx = 0;

        public MainForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            UpdateStatusLabel("初始化中...", Color.Red);

            // 不给调整UI大小，没做适配
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Tag列表初始化
            InitTagListView();

            // 加个日志异常回调
            DebugLogger.ExceptionAction = (e) => MessageBox.Show(e.ToString(), "日志写入异常");

            // 初始化工作区
            CheckWorkspace();

            // 初始化配置
            InitConfig();

            // 初始化地区列表
            InitRegionList();

            // 设置上次选的地区
            var lastRegion = GetLastRegion();
            SelectRegion(string.IsNullOrEmpty(lastRegion) ? Config.RegionArr[0] : lastRegion);

            // 设置上次选的打包类型
            CurTagPrefix = GetLastBuildType();
            UpdateTagList();

            // 设置上次打的tag
            CurTag = GetLastTag();

            // 清空状态栏
            UpdateStatusLabel("");
        }

        // Tag列表初始化
        private void InitTagListView()
        {
            TagListView.ListViewItemSorter = new TagListViewTimeComparer();
            TagListView.ColumnWidthChanged += TagListView_ColumnWidthChanged;
            if (GetTagListViewColWidth(out int w1, out int w2, out int w3, out int w4))
            {
                TagListView.Columns[0].Width = w1;
                TagListView.Columns[1].Width = w2;
                TagListView.Columns[2].Width = w3;
                TagListView.Columns[3].Width = w4;
            }
        }

        private void TagListView_ColumnWidthChanged(object? sender, ColumnWidthChangedEventArgs e)
        {
            var col = TagListView.Columns;
            SaveTagListViewColWidth(col[0].Width, col[1].Width, col[2].Width, col[3].Width);
        }

        // 取工作区
        private void CheckWorkspace()
        {
            GitWorkspacePath = GetLastWorkspace();
            if (string.IsNullOrEmpty(GitWorkspacePath) || !Directory.Exists($"{GitWorkspacePath}\\.git"))
            {
                DebugError("请选择git工程目录(包含.git的目录)");
                SelectWorkspace();
            }
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Text = $"打Tag工具({TOOL_VERSION}) [{GitWorkspacePath}]";
        }

        // 初始化地区列表
        private void InitRegionList()
        {
            SelectRegionMenu.DropDownItems.Clear();
            foreach (var regionKey in Config.RegionArr)
            {
                var cfg = Config.RegionCfg[regionKey];
                var item = new ToolStripMenuItem()
                {
                    Name = regionKey,
                    Size = new Size(148, 22),
                    Tag = regionKey,
                    Text = cfg.Name,

                };
                item.Click += (sender, e) =>
                {
                    SelectRegion(regionKey);
                };
                SelectRegionMenu.DropDownItems.Add(item);

            }
        }

        private void WorkspacrMenuItem_Click(object sender, EventArgs e)
        {
            SelectWorkspace();
        }

        // 弹窗选择工作区
        public void SelectWorkspace()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var gitPath = $"{fbd.SelectedPath}\\.git";
                    if (!Directory.Exists(gitPath))
                    {
                        DebugError($"无效的git工程【{gitPath}】，请重新选择");
                        SelectWorkspace();
                        return;
                    }
                    GitWorkspacePath = fbd.SelectedPath;
                    SaveLastWorkspace(GitWorkspacePath);
                    UpdateTitle();
                }
            }
        }

        private void ToolStripMenuItem_TW_Click(object sender, EventArgs e)
        {
            SelectRegion("RegionTW");
        }

        // 切换地区
        private void SelectRegion(string strRegion)
        {
            if (CurRegion == strRegion)
                return;

            if (!Config.RegionArr.Contains(strRegion))
            {
                DebugError($"无效地区输入：[{strRegion}]");
                return;
            }
            CurRegion = strRegion;

            CurBranch = GetLastBranch();

            var regionList = SelectRegionMenu.DropDownItems;
            foreach (var regionItem in regionList)
            {
                var item = (regionItem as ToolStripMenuItem);
                if (item.Tag.ToString() == strRegion)
                {
                    SelectRegionMenu.Text = $"地区：{item.Text}";
                    item.Checked = true;
                    SaveLastRegion(strRegion);
                    UpdateRegionBranch(strRegion);

                    var buildType = GetLastBuildType();
                }
                else
                {
                    item.Checked = false;
                }
            }

            InitRegionBuildType();
            TagListView.Items.Clear();
        }

        // 更新地区的分支信息
        private void UpdateRegionBranch(string strRegion)
        {
            DropDownBranch.Items.Clear();
            var branchList = Config.RegionCfg[strRegion].BranchList;
            DropDownBranch.Items.AddRange(branchList.ToArray());

            if (!branchList.Contains(CurBranch) && branchList.Count > 0)
            {
                CurBranch = branchList[0];
            }
            DropDownBranch.Text = CurBranch;
        }

        private void DropDownBranch_TextChanged(object sender, EventArgs e)
        {
            CurBranch = DropDownBranch.Text.ToString();
            SaveLastBranch(CurBranch);
            CurVersion = GetBranchVersion();
            VersionText.Text = CurVersion ?? "";

            BranchTips.Visible = !IsValidBranch(CurBranch);
            VersionTips.Visible = string.IsNullOrEmpty(CurVersion);
        }

        private void SaveBranchBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CurRegion) || string.IsNullOrEmpty(CurBranch))
            {
                DebugError("保存失败！无效地区或分支");
                return;
            }
            SaveRegionBranch(CurRegion, CurBranch);
            UpdateRegionBranch(CurRegion);
        }

        private void DeleteBranchBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CurRegion) || string.IsNullOrEmpty(CurBranch))
            {
                DebugError("保存失败！无效地区或分支");
                return;
            }
            DeleteRegionBranch(CurRegion, CurBranch);
            UpdateRegionBranch(CurRegion);
        }

        // 初始化地区打包类型
        private void InitRegionBuildType()
        {
            if (string.IsNullOrEmpty(CurRegion)) { return; }
            InitRegionBuildTypeList(Config.RegionCfg[CurRegion].BuildPatchTagDic, BuildPatchTableLayout);
            InitRegionBuildTypeList(Config.RegionCfg[CurRegion].BuildFullTagDic, BuildFullTableLayout);
        }

        private void InitRegionBuildTypeList(Dictionary<string, string> data, TableLayoutPanel parent)
        {
            parent.Controls.Clear();
            int index = 0;
            foreach (var item in data)
            {
                var btn = new Button();
                btn.BackColor = Color.Transparent;
                btn.ForeColor = SystemColors.ControlText;
                btn.Location = new Point(3, 3);
                btn.Name = item.Value;
                btn.Size = new Size(151, 40);
                btn.TabIndex = 0;
                btn.Text = item.Value;
                btn.Tag = item.Key;
                btn.UseVisualStyleBackColor = false;
                btn.Click += Build_Type_Click;

                parent.Controls.Add(btn, index % parent.ColumnCount, index / parent.ColumnCount);
                ++index;
            }
        }

        private void Build_Type_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            CurTagPrefix = btn.Tag.ToString();
            SaveLastBuildType(CurTagPrefix);
            UpdateTagList();
        }

        // 根据打包类型更新类似tag列表
        private void UpdateTagList()
        {
            if (string.IsNullOrEmpty(CurTagPrefix))
                return;
            var tagList = GetRecentlyTag(CurTagPrefix);

            if (!string.IsNullOrEmpty(CurVersion))
            {
                var newTag = TryFindNextTag($"{CurTagPrefix}_{CurVersion.Split('.').Last()}");
                CurTag = newTag;
            }

            try
            {
                UpdateTagListAsync(tagList);
            }
            catch (Exception e)
            {
                DebugError(e.ToString(), "刷新tag列表异常");
            }
        }

        private CancellationTokenSource _cts;
        private async void UpdateTagListAsync(List<string> tagList)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;

            await ThreadSleepWithTime();
            TagListView.Items.Clear();
            foreach (var tag in tagList)
            {
                TagListView.Items.Add(new ListViewItem(new string[] { tag, "...", "...", "..." }, -1));
            }
            await ThreadSleepWithTime();
            ulong fetchStatusIdx = 0;
            if (!_fetchGitDetail)
            {
                // 拉取所有tag详细信息
                fetchStatusIdx = UpdateStatusLabel($"[不影响打tag] 正在拉取tag提交日志[git fetch --tags]");
                await ThreadSleepWithTime();
                await Task.Run(() =>
                {
                    if (token.IsCancellationRequested) return;
                    RunGitCommand("git fetch --tags", out string _);
                }, token);
                _fetchGitDetail = true;
            }


            var count = tagList.Count;
            for (int i = 0; i < count; i++)
            {
                if (token.IsCancellationRequested) return;
                int index = i;  // 避免闭包问题
                //UpdateStatusLabel($"正在加载tag({CurTagPrefix})信息[{i + 1}/{count}]");
                await Task.Run(() =>
                {
                    if (token.IsCancellationRequested) return;

                    // 耗时操作
                    if (!GetTagDetail(tagList[index], out string time, out string author, out string message))
                        return;

                    if (!token.IsCancellationRequested)
                    {
                        BeginInvoke((Action)(() =>
                        {
                            if (!token.IsCancellationRequested) // 确保 UI 仍然有效
                            {
                                if (TagListView.Items.Count <= index)
                                    return;
                                TagListView.Items[index].SubItems[1].Text = time;
                                TagListView.Items[index].SubItems[2].Text = author;
                                TagListView.Items[index].SubItems[3].Text = message;
                            }
                        }));
                    }
                }, token);
            }
            TagListView.Sort();
            // 暴力判断状态是否有被打断
             if (fetchStatusIdx == _statusIdx)
                UpdateStatusLabel($"加载tag({CurTagPrefix})信息[{count}]完成", Color.Green);
        }

        // 8个数字 “25021707” -> “25021708”
        private string TryFindNextTag(string tag)
        {
            var allTags = GetGitTags();
            var newTag = tag;
            var regex = new Regex(@"\d{8}");
            var match = regex.Match(tag);
            if (!match.Success)
                return tag;

            for (int i = 0; i < 98; i++)
            {
                if (!allTags.Contains(newTag))
                    return newTag;

                // 提取数字并递增
                match = regex.Match(newTag);
                int currentNumber = int.Parse(match.Value);
                int nextNumber = currentNumber + 1;

                // 替换旧数字为递增后的新数字
                newTag = regex.Replace(tag, nextNumber.ToString());
            }
            return tag;
        }

        private void TagListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TagListView.SelectedItems == null || TagListView.SelectedItems.Count <= 0)
                return;
            var selectTag = TagListView.SelectedItems[0].Text;
            var newTag = TryFindNextTag(selectTag);
            CurTag = newTag;
        }

        private void TagTextBox_TextChanged(object sender, EventArgs e)
        {
            TagTips.Visible = !IsValidTag(CurTag);
        }

        private void RefreshToolMenu_Click(object sender, EventArgs e)
        {
            RefreshRemoteGit();
        }

        // 刷新git拉取信息缓存
        private void RefreshRemoteGit()
        {
            CleanGitCache();
            UpdateTagList();
        }

        // 检查tag合法性
        private bool CheckTagOK()
        {
            if (!IsValidBranch(CurBranch))
            {
                DebugError($"找不到分支【{CurBranch}】，尝试点击菜单‘刷新’。");
                return false;
            }
            if (!IsValidTag(CurTag))
            {
                DebugError($"已存在或无效的Tag【{CurTag}】");
                return false;
            }
            return true;
        }

        private void PushTagBtn_Click(object sender, EventArgs e)
        {
            if (!CheckTagOK())
                return;

            var info = $"分支：[{CurBranch}]\nTag:[{CurTag}]";
            var result = MessageBox.Show(info, "提交确认", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes)
                return;

            SaveLastTag(CurTag);

            PushTagAsync();
        }

        // 打tag，async主要是要刷新界面，避免表现卡死
        private async void PushTagAsync()
        {
            UpdateStatusLabel($"开始打Tag：分支：[{CurBranch}] Tag：[{CurTag}]", Color.Blue);

            await ThreadSleepWithTime();

            Func<string, bool> runGitAndUpdateStatus = (string args) =>
            {
                UpdateStatusLabel($"打Tag中：[{args}]", Color.Red);
                if (RunGitCommand(args, out string error))
                    return true;

                PushTagFail();
                return false;
            };
            if (!runGitAndUpdateStatus($"git fetch origin {CurBranch}")) return;
            await ThreadSleepWithTime();
            if (!runGitAndUpdateStatus($"git tag {CurTag} origin/{CurBranch}")) return;
            await ThreadSleepWithTime();
            if (!runGitAndUpdateStatus($"git push origin {CurTag}")) return;

            UpdateStatusLabel($"打Tag成功：分支：[{CurBranch}] Tag：[{CurTag}]", Color.Green);
            OpenWebsite($"{Config.GitURL.Replace(".git", "/-/tags")}?sort=updated_desc&search={CurTag}");

            // 保存分支
            SaveRegionBranch(CurRegion, CurBranch);
            UpdateRegionBranch(CurRegion);

            await ThreadSleepWithTime(1000);
            RefreshRemoteGit();
        }

        private async Task ThreadSleepWithTime(int time = 10)
        {
            await Task.Delay(time);
        }

        private void PushTagFail()
        {
            UpdateStatusLabel($"打Tag失败：分支：[{CurBranch}] Tag：[{CurTag}]，点击菜单[帮助/日志]查看详情", Color.Red);
        }

        private ulong UpdateStatusLabel(string info)
        {
            return UpdateStatusLabel(info, SystemColors.ControlText);
        }

        // 刷新状态栏信息
        private ulong UpdateStatusLabel(string info, Color color)
        {
            ++_statusIdx;
            StatusLabel.ForeColor = color;
            StatusLabel.Text = info;
            DebugLog(info);
            return _statusIdx;
        }

        // 获取当前分支版本号
        private string GetBranchVersion()
        {
            if (string.IsNullOrEmpty(CurBranch))
                return null;

            var versionFile = Config.RegionCfg[CurRegion].VersionFile;
            if (RunGitCommand($"git show origin/{CurBranch}:{versionFile}", out string version))
                return version;

            return null;
        }

        #region 帮助菜单

        private void HelpDebugLogMenuItem_Click(object sender, EventArgs e)
        {
            RevealFile(DebugLogger.LogFilePath);
        }

        private void HelpOpenGitWebTagMenuItem_Click(object sender, EventArgs e)
        {
            var url = Config.GitURL.Replace(".git", "/-/tags");
            OpenWebsite(url);
        }

        private void HelpOpenPropsPath_Click(object sender, EventArgs e)
        {
            RevealFolder(PROPS_FOLDER_PATH);
        }

        private void HelpOpenHelpDoc_Click(object sender, EventArgs e)
        {
            RevealFile(README_PATH);
        }

        private void HelpOpenConfigMenuItem_Click(object sender, EventArgs e)
        {
            RevealFile(CONFIG_FILE_PATH);
        }

        private void HelpCleanLogMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(DebugLogger.LogFilePath))
                File.Delete(DebugLogger.LogFilePath);
        }

        private void HelpCleanCacheMenuItem_Click(object sender, EventArgs e)
        {
            string expandedPath = Environment.ExpandEnvironmentVariables(PROPS_FOLDER_PATH);
            if (Directory.Exists(expandedPath))
                Directory.Delete(expandedPath, true);
        }

        private void RevealFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                // 打开文件所在目录，并选中该文件
                Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                if (filePath.EndsWith(".md") || filePath.EndsWith(".txt") || filePath.EndsWith(".json"))
                {
                    Process.Start("notepad.exe", filePath);
                }
            }
            else
            {
                DebugError(filePath, "跳转文件失败，不存在", false);
            }
        }

        private void RevealFolder(string folderPath)
        {
            string expandedPath = Environment.ExpandEnvironmentVariables(folderPath);
            if (Directory.Exists(expandedPath))
            {
                Process.Start("explorer.exe", expandedPath);
            }
            else
            {
                DebugError(expandedPath, "跳转目录失败，不存在", false);
            }
        }

        private void OpenWebsite(string url)
        {
            try
            {
                // 打开默认浏览器并导航到指定 URL
                ProcessStartInfo psInfo = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psInfo);
            }
            catch (Exception e)
            {
                DebugError($"无法打开网页[{url}]:{e}");
            }
        }

        #endregion
    }
}
