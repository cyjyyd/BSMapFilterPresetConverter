namespace FilterPresetConverter;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        panelTop = new Panel();
        btnBatchConvert = new Button();
        btnSave = new Button();
        btnConvert = new Button();
        label1 = new Label();
        txtBrsetPath = new TextBox();
        btnLoadBrset = new Button();
        tabControl = new TabControl();
        tabInput = new TabPage();
        txtBrsetContent = new TextBox();
        tabOutput = new TabPage();
        txtBsfContent = new TextBox();
        tabLog = new TabPage();
        txtLog = new TextBox();
        panelTop.SuspendLayout();
        tabControl.SuspendLayout();
        tabInput.SuspendLayout();
        tabOutput.SuspendLayout();
        tabLog.SuspendLayout();
        SuspendLayout();
        // 
        // panelTop
        // 
        panelTop.Controls.Add(btnBatchConvert);
        panelTop.Controls.Add(btnSave);
        panelTop.Controls.Add(btnConvert);
        panelTop.Controls.Add(label1);
        panelTop.Controls.Add(txtBrsetPath);
        panelTop.Controls.Add(btnLoadBrset);
        panelTop.Dock = DockStyle.Top;
        panelTop.Location = new Point(0, 0);
        panelTop.Margin = new Padding(5, 4, 5, 4);
        panelTop.Name = "panelTop";
        panelTop.Padding = new Padding(16, 14, 16, 14);
        panelTop.Size = new Size(1257, 113);
        panelTop.TabIndex = 0;
        // 
        // btnBatchConvert
        // 
        btnBatchConvert.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBatchConvert.Location = new Point(990, 64);
        btnBatchConvert.Margin = new Padding(5, 4, 5, 4);
        btnBatchConvert.Name = "btnBatchConvert";
        btnBatchConvert.Size = new Size(236, 35);
        btnBatchConvert.TabIndex = 5;
        btnBatchConvert.Text = "批量转换...";
        btnBatchConvert.UseVisualStyleBackColor = true;
        btnBatchConvert.Click += btnBatchConvert_Click;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.Enabled = false;
        btnSave.Location = new Point(801, 64);
        btnSave.Margin = new Padding(5, 4, 5, 4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(173, 35);
        btnSave.TabIndex = 4;
        btnSave.Text = "保存BSF...";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // btnConvert
        // 
        btnConvert.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnConvert.Location = new Point(613, 64);
        btnConvert.Margin = new Padding(5, 4, 5, 4);
        btnConvert.Name = "btnConvert";
        btnConvert.Size = new Size(173, 35);
        btnConvert.TabIndex = 3;
        btnConvert.Text = "转换 (Convert)";
        btnConvert.UseVisualStyleBackColor = true;
        btnConvert.Click += btnConvert_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(20, 28);
        label1.Margin = new Padding(5, 0, 5, 0);
        label1.Name = "label1";
        label1.Size = new Size(103, 24);
        label1.TabIndex = 1;
        label1.Text = "BRSET文件:";
        // 
        // txtBrsetPath
        // 
        txtBrsetPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBrsetPath.Location = new Point(157, 24);
        txtBrsetPath.Margin = new Padding(5, 4, 5, 4);
        txtBrsetPath.Name = "txtBrsetPath";
        txtBrsetPath.ReadOnly = true;
        txtBrsetPath.Size = new Size(941, 30);
        txtBrsetPath.TabIndex = 2;
        // 
        // btnLoadBrset
        // 
        btnLoadBrset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLoadBrset.Location = new Point(1116, 21);
        btnLoadBrset.Margin = new Padding(5, 4, 5, 4);
        btnLoadBrset.Name = "btnLoadBrset";
        btnLoadBrset.Size = new Size(118, 35);
        btnLoadBrset.TabIndex = 0;
        btnLoadBrset.Text = "浏览...";
        btnLoadBrset.UseVisualStyleBackColor = true;
        btnLoadBrset.Click += btnLoadBrset_Click;
        // 
        // tabControl
        // 
        tabControl.Controls.Add(tabInput);
        tabControl.Controls.Add(tabOutput);
        tabControl.Controls.Add(tabLog);
        tabControl.Dock = DockStyle.Fill;
        tabControl.Location = new Point(0, 113);
        tabControl.Margin = new Padding(5, 4, 5, 4);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(1257, 593);
        tabControl.TabIndex = 1;
        // 
        // tabInput
        // 
        tabInput.Controls.Add(txtBrsetContent);
        tabInput.Location = new Point(4, 33);
        tabInput.Margin = new Padding(5, 4, 5, 4);
        tabInput.Name = "tabInput";
        tabInput.Padding = new Padding(8, 7, 8, 7);
        tabInput.Size = new Size(1249, 556);
        tabInput.TabIndex = 0;
        tabInput.Text = "输入 (BRSET)";
        tabInput.UseVisualStyleBackColor = true;
        // 
        // txtBrsetContent
        // 
        txtBrsetContent.Dock = DockStyle.Fill;
        txtBrsetContent.Font = new Font("Consolas", 9F);
        txtBrsetContent.Location = new Point(8, 7);
        txtBrsetContent.Margin = new Padding(5, 4, 5, 4);
        txtBrsetContent.Multiline = true;
        txtBrsetContent.Name = "txtBrsetContent";
        txtBrsetContent.ScrollBars = ScrollBars.Both;
        txtBrsetContent.Size = new Size(1233, 542);
        txtBrsetContent.TabIndex = 0;
        txtBrsetContent.WordWrap = false;
        // 
        // tabOutput
        // 
        tabOutput.Controls.Add(txtBsfContent);
        tabOutput.Location = new Point(4, 33);
        tabOutput.Margin = new Padding(5, 4, 5, 4);
        tabOutput.Name = "tabOutput";
        tabOutput.Padding = new Padding(8, 7, 8, 7);
        tabOutput.Size = new Size(1249, 556);
        tabOutput.TabIndex = 1;
        tabOutput.Text = "输出 (BSF)";
        tabOutput.UseVisualStyleBackColor = true;
        // 
        // txtBsfContent
        // 
        txtBsfContent.Dock = DockStyle.Fill;
        txtBsfContent.Font = new Font("Consolas", 9F);
        txtBsfContent.Location = new Point(8, 7);
        txtBsfContent.Margin = new Padding(5, 4, 5, 4);
        txtBsfContent.Multiline = true;
        txtBsfContent.Name = "txtBsfContent";
        txtBsfContent.ReadOnly = true;
        txtBsfContent.ScrollBars = ScrollBars.Both;
        txtBsfContent.Size = new Size(1233, 542);
        txtBsfContent.TabIndex = 0;
        txtBsfContent.WordWrap = false;
        // 
        // tabLog
        // 
        tabLog.Controls.Add(txtLog);
        tabLog.Location = new Point(4, 33);
        tabLog.Margin = new Padding(5, 4, 5, 4);
        tabLog.Name = "tabLog";
        tabLog.Padding = new Padding(8, 7, 8, 7);
        tabLog.Size = new Size(1249, 556);
        tabLog.TabIndex = 2;
        tabLog.Text = "日志";
        tabLog.UseVisualStyleBackColor = true;
        // 
        // txtLog
        // 
        txtLog.Dock = DockStyle.Fill;
        txtLog.Font = new Font("Consolas", 9F);
        txtLog.Location = new Point(8, 7);
        txtLog.Margin = new Padding(5, 4, 5, 4);
        txtLog.Multiline = true;
        txtLog.Name = "txtLog";
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Both;
        txtLog.Size = new Size(1233, 542);
        txtLog.TabIndex = 0;
        txtLog.WordWrap = false;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1257, 706);
        Controls.Add(tabControl);
        Controls.Add(panelTop);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(5, 4, 5, 4);
        MinimumSize = new Size(930, 542);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "FilterPresetConverter - BRSET转BSF";
        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        tabControl.ResumeLayout(false);
        tabInput.ResumeLayout(false);
        tabInput.PerformLayout();
        tabOutput.ResumeLayout(false);
        tabOutput.PerformLayout();
        tabLog.ResumeLayout(false);
        tabLog.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel panelTop;
    private System.Windows.Forms.Button btnLoadBrset;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtBrsetPath;
    private System.Windows.Forms.Button btnConvert;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnBatchConvert;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabInput;
    private System.Windows.Forms.TabPage tabOutput;
    private System.Windows.Forms.TabPage tabLog;
    private System.Windows.Forms.TextBox txtBrsetContent;
    private System.Windows.Forms.TextBox txtBsfContent;
    private System.Windows.Forms.TextBox txtLog;
}