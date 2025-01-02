namespace WinApp
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            lb_msg = new ListBox();
            groupBox1 = new GroupBox();
            btn_test = new Button();
            label1 = new Label();
            labClose = new Label();
            labelMini = new Label();
            toolTip1 = new ToolTip(components);
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lb_msg
            // 
            lb_msg.Dock = DockStyle.Fill;
            lb_msg.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lb_msg.FormattingEnabled = true;
            lb_msg.ItemHeight = 12;
            lb_msg.Location = new Point(3, 28);
            lb_msg.Name = "lb_msg";
            lb_msg.Size = new Size(956, 645);
            lb_msg.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Transparent;
            groupBox1.Controls.Add(lb_msg);
            groupBox1.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(12, 67);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(962, 676);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "日志";
            // 
            // btn_test
            // 
            btn_test.BackColor = Color.White;
            btn_test.FlatStyle = FlatStyle.Flat;
            btn_test.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            btn_test.ForeColor = Color.FromArgb(30, 159, 255);
            btn_test.Location = new Point(455, 36);
            btn_test.Name = "btn_test";
            btn_test.Size = new Size(117, 37);
            btn_test.TabIndex = 2;
            btn_test.Text = "停止调度";
            btn_test.UseVisualStyleBackColor = false;
            btn_test.Click += btn_test_ClickAsync;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(8, 5);
            label1.Name = "label1";
            label1.Size = new Size(107, 20);
            label1.TabIndex = 3;
            label1.Text = "新云报后台工具";
            // 
            // labClose
            // 
            labClose.AutoSize = true;
            labClose.BackColor = Color.Transparent;
            labClose.Cursor = Cursors.Hand;
            labClose.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            labClose.ForeColor = Color.White;
            labClose.Location = new Point(957, 1);
            labClose.Name = "labClose";
            labClose.Size = new Size(26, 26);
            labClose.TabIndex = 4;
            labClose.Text = "×";
            toolTip1.SetToolTip(labClose, "关闭");
            labClose.Click += labClose_Click;
            // 
            // labelMini
            // 
            labelMini.AutoSize = true;
            labelMini.BackColor = Color.Transparent;
            labelMini.Cursor = Cursors.Hand;
            labelMini.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            labelMini.ForeColor = Color.White;
            labelMini.Location = new Point(941, 1);
            labelMini.Name = "labelMini";
            labelMini.Size = new Size(20, 26);
            labelMini.TabIndex = 4;
            labelMini.Text = "-";
            toolTip1.SetToolTip(labelMini, "最小化到右下角");
            labelMini.Click += labelMini_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGreen;
            ClientSize = new Size(986, 755);
            Controls.Add(labelMini);
            Controls.Add(labClose);
            Controls.Add(label1);
            Controls.Add(btn_test);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "新云报后台工具";
            Load += MainForm_Load;
            MouseDown += MainForm_MouseDown;
            Resize += MainForm_Resize;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lb_msg;
        private GroupBox groupBox1;
        private Button btn_test;
        private Label label1;
        private Label labClose;
        private Label labelMini;
        private ToolTip toolTip1;
    }
}