using System;
using System.IO;
using System.Windows.Forms;
using FilterPresetConverter.Models;
using FilterPresetConverter.Services;

namespace FilterPresetConverter;

public partial class Form1 : Form
{
    private readonly BrsetToBsfConverter _converter = new();
    private BrsetPreset? _currentBrset;
    private FilterPreset? _currentBsf;
    private string? _currentFilePath;

    public Form1()
    {
        InitializeComponent();
        Log("程序已启动，请选择BRSET文件或直接粘贴内容进行转换。");
    }

    private void Log(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => Log(message));
            return;
        }
        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
    }

    private void btnLoadBrset_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "BRSET文件 (*.brset)|*.brset|JSON文件 (*.json)|*.json|所有文件 (*.*)|*.*",
            Title = "选择BRSET预设文件",
            CheckFileExists = true
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            try
            {
                _currentFilePath = ofd.FileName;
                txtBrsetPath.Text = ofd.FileName;

                var content = File.ReadAllText(ofd.FileName);
                txtBrsetContent.Text = content;

                _currentBrset = _converter.LoadBrsetFromJson(content);
                if (_currentBrset != null)
                {
                    Log($"成功加载: {Path.GetFileName(ofd.FileName)}");
                    btnSave.Enabled = false;
                    _currentBsf = null;
                    txtBsfContent.Text = "";
                }
                else
                {
                    Log($"错误: 无法解析BRSET文件格式");
                }
            }
            catch (Exception ex)
            {
                Log($"加载文件失败: {ex.Message}");
            }
        }
    }

    private void btnConvert_Click(object sender, EventArgs e)
    {
        // 如果没有从文件加载，尝试从文本框读取
        if (_currentBrset == null && !string.IsNullOrWhiteSpace(txtBrsetContent.Text))
        {
            try
            {
                _currentBrset = _converter.LoadBrsetFromJson(txtBrsetContent.Text);
            }
            catch (Exception ex)
            {
                Log($"解析输入内容失败: {ex.Message}");
                return;
            }
        }

        if (_currentBrset == null)
        {
            MessageBox.Show("请先加载BRSET文件或在输入框中粘贴BRSET内容", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            // 从文件路径提取名称
            var presetName = !string.IsNullOrEmpty(_currentFilePath)
                ? Path.GetFileNameWithoutExtension(_currentFilePath)
                : "Converted Preset";

            var result = _converter.Convert(_currentBrset, presetName);

            if (result.Success)
            {
                _currentBsf = result.Preset;
                txtBsfContent.Text = _converter.ToBsfJson(result.Preset);
                btnSave.Enabled = true;
                tabControl.SelectedTab = tabOutput;

                Log($"转换成功: {result.Message}");

                if (result.SkippedItems.Count > 0)
                {
                    Log($"跳过的项目 (BSF不支持):");
                    foreach (var item in result.SkippedItems)
                    {
                        Log($"  - {item}");
                    }
                }

                if (result.Warnings.Count > 0)
                {
                    Log($"警告:");
                    foreach (var warning in result.Warnings)
                    {
                        Log($"  - {warning}");
                    }
                }
            }
            else
            {
                Log($"转换失败: {result.Message}");
            }
        }
        catch (Exception ex)
        {
            Log($"转换过程中发生错误: {ex.Message}");
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (_currentBsf == null)
        {
            MessageBox.Show("请先进行转换", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var defaultName = _currentBsf.Name;
        if (string.IsNullOrEmpty(defaultName))
            defaultName = "converted_preset";

        defaultName = SanitizeFileName(defaultName) + ".bsf";

        using var sfd = new SaveFileDialog
        {
            Filter = "BSF预设文件 (*.bsf)|*.bsf|JSON文件 (*.json)|*.json",
            Title = "保存BSF预设文件",
            FileName = defaultName
        };

        if (sfd.ShowDialog() == DialogResult.OK)
        {
            try
            {
                _converter.SaveBsfFile(_currentBsf, sfd.FileName);
                Log($"已保存: {sfd.FileName}");
            }
            catch (Exception ex)
            {
                Log($"保存失败: {ex.Message}");
            }
        }
    }

    private static string SanitizeFileName(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        // 移除一些特殊字符
        name = name.Replace("[", "").Replace("]", "");
        return name.Trim();
    }

    private void btnBatchConvert_Click(object sender, EventArgs e)
    {
        // 选择源文件夹
        using var fbd = new FolderBrowserDialog
        {
            Description = "选择包含BRSET文件的文件夹",
            ShowNewFolderButton = false
        };

        if (fbd.ShowDialog() != DialogResult.OK)
            return;

        var sourceFolder = fbd.SelectedPath;

        // 选择目标文件夹
        using var fbd2 = new FolderBrowserDialog
        {
            Description = "选择保存BSF文件的目标文件夹",
            ShowNewFolderButton = true
        };

        if (fbd2.ShowDialog() != DialogResult.OK)
            return;

        var targetFolder = fbd2.SelectedPath;

        // 查找所有brset文件
        var brsetFiles = Directory.GetFiles(sourceFolder, "*.brset", SearchOption.TopDirectoryOnly);

        if (brsetFiles.Length == 0)
        {
            MessageBox.Show("未找到BRSET文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show(
            $"找到 {brsetFiles.Length} 个BRSET文件，是否开始批量转换？\n\n源文件夹: {sourceFolder}\n目标文件夹: {targetFolder}",
            "确认批量转换",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result != DialogResult.Yes)
            return;

        Log($"开始批量转换，共 {brsetFiles.Length} 个文件...");
        tabControl.SelectedTab = tabLog;

        int successCount = 0;
        int failCount = 0;

        foreach (var filePath in brsetFiles)
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                Log($"处理: {fileName}");

                var brset = _converter.LoadBrsetFile(filePath);
                if (brset == null)
                {
                    Log($"  跳过: 无法解析文件");
                    failCount++;
                    continue;
                }

                var convertResult = _converter.Convert(brset, fileName);
                if (!convertResult.Success)
                {
                    Log($"  失败: {convertResult.Message}");
                    failCount++;
                    continue;
                }

                var targetPath = Path.Combine(targetFolder, SanitizeFileName(fileName) + ".bsf");
                _converter.SaveBsfFile(convertResult.Preset!, targetPath);
                Log($"  成功: {convertResult.Message} -> {Path.GetFileName(targetPath)}");
                successCount++;
            }
            catch (Exception ex)
            {
                Log($"  错误: {ex.Message}");
                failCount++;
            }
        }

        Log($"批量转换完成: 成功 {successCount}，失败 {failCount}");
        MessageBox.Show(
            $"批量转换完成！\n成功: {successCount}\n失败: {failCount}\n\n输出目录: {targetFolder}",
            "批量转换完成",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}