namespace ScottReece.BorderCompare.App
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
            timer1 = new System.Windows.Forms.Timer(components);
            pnlTop = new Panel();
            pnlEarthDisplay = new DoubleBufferedPanel();
            label1 = new Label();
            pnlTop.SuspendLayout();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Interval = 50;
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(label1);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(800, 52);
            pnlTop.TabIndex = 0;
            // 
            // panel1
            // 
            pnlEarthDisplay.Dock = DockStyle.Fill;
            pnlEarthDisplay.Location = new Point(0, 52);
            pnlEarthDisplay.Name = "pnlEarthDisplay";
            pnlEarthDisplay.Size = new Size(800, 398);
            pnlEarthDisplay.TabIndex = 1;
            // 
            // label1
            // 
            label1.BackColor = Color.White;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(800, 52);
            label1.TabIndex = 0;
            label1.Text = "Directions: click and drag to move the regular Earth around. Right-click and drag to move the magenta borders around. The scroll wheel can zoom in and out.";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlEarthDisplay);
            Controls.Add(pnlTop);
            Name = "MainForm";
            Text = "BorderCompare";
            Load += Form1_Load;
            pnlTop.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private Panel pnlTop;
        private Label label1;
        private DoubleBufferedPanel pnlEarthDisplay;
    }
}
