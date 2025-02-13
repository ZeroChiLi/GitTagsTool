using System.Collections;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace TagsTool
{
    public class ToolConfig
    {
        public string GitURL { get; set; }
        public string[] RegionArr { get; set; }

        public Dictionary<string, Cfg> RegionCfg { get; set; }

        public class Cfg
        {
            public string Name { get; set; }
            public string VersionFile { get; set; }
            public List<string> BranchList { get; set; }
            public Dictionary<string, string> BuildPatchTagDic { get; set; }
            public Dictionary<string, string> BuildFullTagDic { get; set; }
        }
    }

    public partial class MainForm
    {
        public const string TOOL_VERSION = "v1.0.250205";
        public const string CONFIG_FILE_PATH = "tags_tool_config.json";
        public const string PROPS_FOLDER_PATH = @"%userprofile%\appdata\local\TagsTool";
        public const string README_PATH = "ReadMe.md";


        public ToolConfig Config { get; private set; } 

        private string _remoteGitInfo;
        private List<string> _remoteTagList = new List<string>(64);
        private List<string> _remoteBranchList = new List<string>(64);
        private Dictionary<string, string> _tagDetailDic = new Dictionary<string, string>(64);
        private bool _fetchGitDetail = false;

        private void InitConfig()
        {
            try
            {
                var content = File.ReadAllText(CONFIG_FILE_PATH);
                Config = JsonSerializer.Deserialize<ToolConfig>(content);

            }
            catch (Exception e)
            {
                DebugError(e.ToString(), $"配置[{CONFIG_FILE_PATH}]文件解析失败");
                throw;
            }
        }

        private void SaveConfig()
        {
            string jsonContent = JsonSerializer.Serialize(Config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(CONFIG_FILE_PATH, jsonContent);
            Console.WriteLine($"JSON 已写入文件：[{CONFIG_FILE_PATH}]");
        }

        private bool SaveRegionBranch(string region, string branch)
        {
            var branchList = Config.RegionCfg[region].BranchList;
            if (!branchList.Contains(branch))
            {
                branchList.Add(branch);
                SaveConfig();
                return true;
            }

            return false;
        }

        private bool DeleteRegionBranch(string region, string branch)
        {
            var branchList = Config.RegionCfg[region].BranchList;
            if (branchList.Contains(branch))
            {
                branchList.Remove(branch);
                SaveConfig();
                return true;
            }

            return false;
        }

#region Properties 存本地的缓存变量

        private Properties.Settings Props = Properties.Settings.Default;

        private void SaveLastRegion(string value)
        {
            if (Props.LastSelectedRegion == value)
                return;
            Props.LastSelectedRegion = value;
            Props.Save();
        }

        private string GetLastRegion()
        {
            return Props.LastSelectedRegion;
        }

        private void SaveLastBranch(string value)
        {
            if (Props.LastSelectedBranch == value)
                return;
            Props.LastSelectedBranch = value;
            Props.Save();
        }

        private string GetLastBranch()
        {
            return Props.LastSelectedBranch;
        }

        private void SaveLastWorkspace(string value)
        {
            if (Props.LastWorkspace == value)
                return;
            Props.LastWorkspace = value;
            Props.Save();
        }

        private string GetLastWorkspace()
        {
            return Props.LastWorkspace;
        }

        private void SaveLastTag(string value)
        {
            if (Props.LastInputTag == value)
                return;
            Props.LastInputTag = value;
            Props.Save();
        }

        private string GetLastTag()
        {
            return Props.LastInputTag;
        }

        private void SaveLastBuildType(string value)
        {
            if (Props.LastSelectedBuildType == value)
                return;
            Props.LastSelectedBuildType = value;
            Props.Save();
        }

        private string GetLastBuildType()
        {
            return Props.LastSelectedBuildType;
        }

        private void SaveTagListViewColWidth(int width1, int width2, int width3, int width4)
        {
            if (Props.TagListViewColWidth1 == width1 && Props.TagListViewColWidth2 == width2 
                && Props.TagListViewColWidth3 == width3 && Props.TagListViewColWidth4 == width4)
                return;
            Props.TagListViewColWidth1 = width1;
            Props.TagListViewColWidth2 = width2;
            Props.TagListViewColWidth3 = width3;
            Props.TagListViewColWidth4 = width4;
            Props.Save();
        }

        private bool GetTagListViewColWidth(out int width1, out int width2, out int width3, out int width4)
        {
            width1 = Props.TagListViewColWidth1;
            width2 = Props.TagListViewColWidth2;
            width3 = Props.TagListViewColWidth3;
            width4 = Props.TagListViewColWidth4;
            if (width1 <= 0 || width2 <= 0 || width3 <= 0 || width4 <= 0)
                return false;
            return true;
        }
        #endregion

        private List<string> GetRecentlyTag(string prefix)
        {
            // 调用 git ls-remote 获取标签列表
            var tags = GetGitTags();

            // 筛选符合条件的标签
            var filteredTags = tags
                .Where(tag => tag.Contains(prefix)) // 筛选包含指定前缀的标签
                .Select(tag => new
                {
                    FullTag = tag,
                    TimeSuffix = ExtractTimeSuffix(tag) // 提取时间后缀
                })
                //.Where(tag => tag.TimeSuffix != null) // 过滤掉无法提取时间的标签
                .OrderByDescending(tag => tag.TimeSuffix) // 按时间降序排序
                //.Take(10) // 取前10个
                .Select(tag => tag.FullTag)
                .ToList();

            return filteredTags;
        }

        public void CleanGitCache()
        {
            _remoteGitInfo = null;
            _remoteTagList.Clear();
            _remoteBranchList.Clear();
            _cts?.Cancel();
            _cts?.Dispose();
            _fetchGitDetail = false;
        }

        private string GetRemoteGitInfo()
        {
            if (!string.IsNullOrEmpty(_remoteGitInfo))
            {
                return _remoteGitInfo;
            }
            UpdateStatusLabel("获取分支信息中...");
            RunGitCommand($"git ls-remote {Config.GitURL}", out string output);
            UpdateStatusLabel("");

            _remoteGitInfo = output;
            return output;
        }

        // 执行 Git 命令的通用方法
        public bool RunGitCommand(string arguments, out string output)
        {
            output = "";
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "git",
                        Arguments = arguments.Substring("git ".Length),
                        WorkingDirectory = GitWorkspacePath,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        //UseShellExecute = false,
                        CreateNoWindow = true,
                        //StandardOutputEncoding = 
                    }
                };

                DebugLog($"cmd:[{arguments}]");

                process.Start();

                // 输出 Git 命令结果
                output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                DebugLog($"output:[{output}]\nerror:[{error}]");
                var realError = GitOutputFilterErrors(error);
                if (process.ExitCode != 0 || !string.IsNullOrWhiteSpace(realError))
                {
                    DebugError($"error:[{realError}]");
                    return false;
                }

            }
            catch (Exception ex)
            {
                DebugError($"[{arguments}]异常：\n{ex}", "Git命令异常");
                return false;
            }
            return true;
        }

        private string GitOutputFilterErrors(string errorOutput)
        {
            // 按行分割错误输出
            string[] lines = errorOutput.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // 过滤出以 "fatal:" 或 "error:" 开头的行
            var errorLines = lines.Where(line => line.StartsWith("fatal:") || line.StartsWith("error:"));

            // 将过滤后的行重新组合成字符串
            return string.Join(Environment.NewLine, errorLines);
        }

        // 提取时间后缀（假设格式为 数字）
        private int? ExtractTimeSuffix(string tag)
        {
            var match = Regex.Match(tag, @"([0-9]+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int time))
            {
                return time;
            }

            return null;
        }

        // 调用 git ls-remote 并获取标签列表
        private List<string> GetGitTags()
        {
            if (_remoteTagList.Count > 0)
                return _remoteTagList;

            var output = GetRemoteGitInfo();
            _remoteTagList = GetGitListByFilter(output, "refs/tags/");

            return _remoteTagList;
        }

        // 调用 git ls-remote 并获取标签列表
        private List<string> GetGitBranchs()
        {
            if (_remoteBranchList.Count > 0)
                return _remoteBranchList;

            var output = GetRemoteGitInfo();
            _remoteBranchList = GetGitListByFilter(output, "refs/heads/");

            return _remoteBranchList;
        }

        private List<string> GetGitListByFilter(string src, string filter)
        {
            return src.Split('\n')
                .Select(line => line.Trim())
                .Where(line => line.Contains(filter))
                .Select(line => line.Split('/').Last()) // 获取标签路径部分
                .ToList();
        }

        private bool GetTagDetail(string tagName, out string time, out string author, out string message)
        {
            time = "...";
            message = "...";
            author = "...";
            if (_tagDetailDic.TryGetValue(tagName, out string tagDetail))
            {
                var tagInfo = tagDetail.Split('\n');
                time = tagInfo[0];
                author = tagInfo[1];
                message = tagInfo[2];
                return true;
            }
            
            var isOk = RunGitCommand($"git -c i18n.logOutputEncoding=GBK log -1 --format=\"%ad%n%an%n%s\" --date=format:\"%Y-%m-%d %H:%M:%S\" {tagName}", out string output);
            if (!isOk)
            {
                return false;
            }
            var info = output.Split('\n');
            time = info[0];
            author = info[1];
            message = info[2];
            _tagDetailDic.TryAdd(tagName, output);
            return true;
        }

        public bool IsValidBranch(string branch)
        {
            return !string.IsNullOrEmpty(branch) && GetGitBranchs().Contains(branch);
        }

        public bool IsValidTag(string tag)
        {
            return !string.IsNullOrEmpty(tag) && tag.StartsWith("tag_") && !GetGitTags().Contains(tag);
        }

        public void DebugLog(string msg)
        {
            DebugLogger.Log(msg);
        }

        public void DebugError(string msg, string title = null, bool showMsgBox = true)
        {
            DebugLogger.Error($"[{title}]{msg}");
            if (showMsgBox)
                MessageBox.Show(msg, title);
        }

    }
}
