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
            UpdateStatusLabel("��ʼ����...", Color.Red);

            // ��������UI��С��û������
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // Tag�б��ʼ��
            InitTagListView();

            // �Ӹ���־�쳣�ص�
            DebugLogger.ExceptionAction = (e) => MessageBox.Show(e.ToString(), "��־д���쳣");

            // ��ʼ��������
            CheckWorkspace();

            // ��ʼ������
            InitConfig();

            // ��ʼ�������б�
            InitRegionList();

            // �����ϴ�ѡ�ĵ���
            var lastRegion = GetLastRegion();
            SelectRegion(string.IsNullOrEmpty(lastRegion) ? Config.RegionArr[0] : lastRegion);

            // �����ϴ�ѡ�Ĵ������
            CurTagPrefix = GetLastBuildType();
            UpdateTagList();

            // �����ϴδ��tag
            CurTag = GetLastTag();

            // ���״̬��
            UpdateStatusLabel("");
        }

        // Tag�б��ʼ��
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

        // ȡ������
        private void CheckWorkspace()
        {
            GitWorkspacePath = GetLastWorkspace();
            if (string.IsNullOrEmpty(GitWorkspacePath) || !Directory.Exists($"{GitWorkspacePath}\\.git"))
            {
                DebugError("��ѡ��git����Ŀ¼(����.git��Ŀ¼)");
                SelectWorkspace();
            }
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Text = $"��Tag����({TOOL_VERSION}) [{GitWorkspacePath}]";
        }

        // ��ʼ�������б�
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

        // ����ѡ������
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
                        DebugError($"��Ч��git���̡�{gitPath}����������ѡ��");
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

        // �л�����
        private void SelectRegion(string strRegion)
        {
            if (CurRegion == strRegion)
                return;

            if (!Config.RegionArr.Contains(strRegion))
            {
                DebugError($"��Ч�������룺[{strRegion}]");
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
                    SelectRegionMenu.Text = $"������{item.Text}";
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

        // ���µ����ķ�֧��Ϣ
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
                DebugError("����ʧ�ܣ���Ч�������֧");
                return;
            }
            SaveRegionBranch(CurRegion, CurBranch);
            UpdateRegionBranch(CurRegion);
        }

        private void DeleteBranchBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CurRegion) || string.IsNullOrEmpty(CurBranch))
            {
                DebugError("����ʧ�ܣ���Ч�������֧");
                return;
            }
            DeleteRegionBranch(CurRegion, CurBranch);
            UpdateRegionBranch(CurRegion);
        }

        // ��ʼ�������������
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

        // ���ݴ�����͸�������tag�б�
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
                DebugError(e.ToString(), "ˢ��tag�б��쳣");
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
                // ��ȡ����tag��ϸ��Ϣ
                fetchStatusIdx = UpdateStatusLabel($"[��Ӱ���tag] ������ȡtag�ύ��־[git fetch --tags]");
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
                int index = i;  // ����հ�����
                //UpdateStatusLabel($"���ڼ���tag({CurTagPrefix})��Ϣ[{i + 1}/{count}]");
                await Task.Run(() =>
                {
                    if (token.IsCancellationRequested) return;

                    // ��ʱ����
                    if (!GetTagDetail(tagList[index], out string time, out string author, out string message))
                        return;

                    if (!token.IsCancellationRequested)
                    {
                        BeginInvoke((Action)(() =>
                        {
                            if (!token.IsCancellationRequested) // ȷ�� UI ��Ȼ��Ч
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
            // �����ж�״̬�Ƿ��б����
             if (fetchStatusIdx == _statusIdx)
                UpdateStatusLabel($"����tag({CurTagPrefix})��Ϣ[{count}]���", Color.Green);
        }

        // 8������ ��25021707�� -> ��25021708��
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

                // ��ȡ���ֲ�����
                match = regex.Match(newTag);
                int currentNumber = int.Parse(match.Value);
                int nextNumber = currentNumber + 1;

                // �滻������Ϊ�������������
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

        // ˢ��git��ȡ��Ϣ����
        private void RefreshRemoteGit()
        {
            CleanGitCache();
            UpdateTagList();
        }

        // ���tag�Ϸ���
        private bool CheckTagOK()
        {
            if (!IsValidBranch(CurBranch))
            {
                DebugError($"�Ҳ�����֧��{CurBranch}�������Ե���˵���ˢ�¡���");
                return false;
            }
            if (!IsValidTag(CurTag))
            {
                DebugError($"�Ѵ��ڻ���Ч��Tag��{CurTag}��");
                return false;
            }
            return true;
        }

        private void PushTagBtn_Click(object sender, EventArgs e)
        {
            if (!CheckTagOK())
                return;

            var info = $"��֧��[{CurBranch}]\nTag:[{CurTag}]";
            var result = MessageBox.Show(info, "�ύȷ��", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes)
                return;

            SaveLastTag(CurTag);

            PushTagAsync();
        }

        // ��tag��async��Ҫ��Ҫˢ�½��棬������ֿ���
        private async void PushTagAsync()
        {
            UpdateStatusLabel($"��ʼ��Tag����֧��[{CurBranch}] Tag��[{CurTag}]", Color.Blue);

            await ThreadSleepWithTime();

            Func<string, bool> runGitAndUpdateStatus = (string args) =>
            {
                UpdateStatusLabel($"��Tag�У�[{args}]", Color.Red);
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

            UpdateStatusLabel($"��Tag�ɹ�����֧��[{CurBranch}] Tag��[{CurTag}]", Color.Green);
            OpenWebsite($"{Config.GitURL.Replace(".git", "/-/tags")}?sort=updated_desc&search={CurTag}");

            // �����֧
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
            UpdateStatusLabel($"��Tagʧ�ܣ���֧��[{CurBranch}] Tag��[{CurTag}]������˵�[����/��־]�鿴����", Color.Red);
        }

        private ulong UpdateStatusLabel(string info)
        {
            return UpdateStatusLabel(info, SystemColors.ControlText);
        }

        // ˢ��״̬����Ϣ
        private ulong UpdateStatusLabel(string info, Color color)
        {
            ++_statusIdx;
            StatusLabel.ForeColor = color;
            StatusLabel.Text = info;
            DebugLog(info);
            return _statusIdx;
        }

        // ��ȡ��ǰ��֧�汾��
        private string GetBranchVersion()
        {
            if (string.IsNullOrEmpty(CurBranch))
                return null;

            var versionFile = Config.RegionCfg[CurRegion].VersionFile;
            if (RunGitCommand($"git show origin/{CurBranch}:{versionFile}", out string version))
                return version;

            return null;
        }

        #region �����˵�

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
                // ���ļ�����Ŀ¼����ѡ�и��ļ�
                Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                if (filePath.EndsWith(".md") || filePath.EndsWith(".txt") || filePath.EndsWith(".json"))
                {
                    Process.Start("notepad.exe", filePath);
                }
            }
            else
            {
                DebugError(filePath, "��ת�ļ�ʧ�ܣ�������", false);
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
                DebugError(expandedPath, "��תĿ¼ʧ�ܣ�������", false);
            }
        }

        private void OpenWebsite(string url)
        {
            try
            {
                // ��Ĭ���������������ָ�� URL
                ProcessStartInfo psInfo = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psInfo);
            }
            catch (Exception e)
            {
                DebugError($"�޷�����ҳ[{url}]:{e}");
            }
        }

        #endregion
    }
}
