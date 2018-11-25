using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BaiduExporter
{
    public partial class LogForm : Form
    {
        private Dictionary<string, Color> colors = new Dictionary<string, Color>
        {
            { "[0m", Color.White },
            { "[0;30m", Color.Black },
            { "[1;30m", Color.DarkGray },
            { "[0;34m", Color.Blue },
            { "[1;34m", Color.LightBlue },
            { "[0;32m", Color.Green },
            { "[1;32m", Color.LightGreen },
            { "[0;36m", Color.Cyan },
            { "[1;36m", Color.LightCyan },
            { "[0;31m", Color.Red },
            { "[1;31m", Color.OrangeRed },
            { "[0;35m", Color.Purple },
            { "[1;35m", Color.Orchid },
            { "[0;33m", Color.Brown },
            { "[1;33m", Color.Yellow },
            { "[0;37m", Color.LightGray },
            { "[1;37m", Color.White },
        };
        public LogForm()
        {
            InitializeComponent();
        }

        public void Log(string log)
        {
            this.Invoke(() =>
            {
                var rtBox = this.richTextBox1;
                var s = log.Split('');
                rtBox.AppendText(s[0]);
                for (var i = 1; i < s.Length; i++)
                {
                    var color = s[i].Substring(0, s[i].IndexOf('m') + 1);
                    rtBox.SelectionStart = rtBox.TextLength;
                    rtBox.SelectionLength = 0;
                    rtBox.SelectionColor = colors[color];
                    rtBox.AppendText(s[i].Substring(s[i].IndexOf('m') + 1));
                    rtBox.SelectionColor = rtBox.ForeColor;
                }
                rtBox.AppendText("\r\n\r\n");
            });
        }

        public void Log(object sender, string str)
        {
            this.Log(str);
        }
    }
}
