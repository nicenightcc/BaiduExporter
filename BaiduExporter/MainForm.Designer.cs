namespace BaiduExporter
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.txt_url = new System.Windows.Forms.ToolStripTextBox();
            this.btn_open = new System.Windows.Forms.ToolStripButton();
            this.btn_close = new System.Windows.Forms.ToolStripButton();
            this.btn_refresh = new System.Windows.Forms.ToolStripButton();
            this.btn_debug = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txt_url,
            this.btn_open,
            this.btn_close,
            this.btn_refresh,
            this.btn_debug});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1262, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // txt_url
            // 
            this.txt_url.Name = "txt_url";
            this.txt_url.Size = new System.Drawing.Size(500, 27);
            // 
            // btn_open
            // 
            this.btn_open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_open.Image = ((System.Drawing.Image)(resources.GetObject("btn_open.Image")));
            this.btn_open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(43, 24);
            this.btn_open.Text = "打开";
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // btn_close
            // 
            this.btn_close.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_close.Image = ((System.Drawing.Image)(resources.GetObject("btn_close.Image")));
            this.btn_close.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(43, 24);
            this.btn_close.Text = "关闭";
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_refresh.Image = ((System.Drawing.Image)(resources.GetObject("btn_refresh.Image")));
            this.btn_refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(43, 24);
            this.btn_refresh.Text = "刷新";
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_debug
            // 
            this.btn_debug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_debug.Image = ((System.Drawing.Image)(resources.GetObject("btn_debug.Image")));
            this.btn_debug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_debug.Name = "btn_debug";
            this.btn_debug.Size = new System.Drawing.Size(61, 24);
            this.btn_debug.Text = "debug";
            this.btn_debug.Click += new System.EventHandler(this.btn_debug_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1262, 646);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripTextBox txt_url;
        private System.Windows.Forms.ToolStripButton btn_open;
        private System.Windows.Forms.ToolStripButton btn_close;
        private System.Windows.Forms.ToolStripButton btn_refresh;
        private System.Windows.Forms.ToolStripButton btn_debug;
    }
}