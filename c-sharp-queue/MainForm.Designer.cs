namespace c_sharp_queue
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.lblNbInQueue = new System.Windows.Forms.Label();
            this.lblNbLogged = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 57);
            this.button1.TabIndex = 0;
            this.button1.Text = "Enqueue n items";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "In Queue";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Logged";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(187, 13);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(251, 381);
            this.listBox1.TabIndex = 3;
            // 
            // lblNbInQueue
            // 
            this.lblNbInQueue.AutoSize = true;
            this.lblNbInQueue.Location = new System.Drawing.Point(108, 92);
            this.lblNbInQueue.Name = "lblNbInQueue";
            this.lblNbInQueue.Size = new System.Drawing.Size(13, 13);
            this.lblNbInQueue.TabIndex = 4;
            this.lblNbInQueue.Text = "0";
            // 
            // lblNbLogged
            // 
            this.lblNbLogged.AutoSize = true;
            this.lblNbLogged.Location = new System.Drawing.Point(108, 128);
            this.lblNbLogged.Name = "lblNbLogged";
            this.lblNbLogged.Size = new System.Drawing.Size(13, 13);
            this.lblNbLogged.TabIndex = 5;
            this.lblNbLogged.Text = "0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 406);
            this.Controls.Add(this.lblNbLogged);
            this.Controls.Add(this.lblNbInQueue);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "Queue Log ";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Leave += new System.EventHandler(this.MainForm_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label lblNbInQueue;
        private System.Windows.Forms.Label lblNbLogged;
    }
}

