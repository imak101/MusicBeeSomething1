﻿using System.ComponentModel;

namespace MusicBeePlugin.Form.Configure
{
    partial class Form_Configure
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Configure));
            this.label_username = new System.Windows.Forms.Label();
            this.textbox_username = new System.Windows.Forms.TextBox();
            this.label_pfp = new System.Windows.Forms.Label();
            this.button_pfp = new System.Windows.Forms.Button();
            this.picbox_pfp = new System.Windows.Forms.PictureBox();
            this.button_submit = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.openFileDialog_pfp = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog_pfp = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize) (this.picbox_pfp)).BeginInit();
            this.SuspendLayout();
            // 
            // label_username
            // 
            this.label_username.Location = new System.Drawing.Point(12, 9);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(72, 33);
            this.label_username.TabIndex = 0;
            this.label_username.Text = "Username:";
            this.label_username.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textbox_username
            // 
            this.textbox_username.Location = new System.Drawing.Point(77, 16);
            this.textbox_username.MaxLength = 30;
            this.textbox_username.Name = "textbox_username";
            this.textbox_username.Size = new System.Drawing.Size(135, 20);
            this.textbox_username.TabIndex = 1;
            // 
            // label_pfp
            // 
            this.label_pfp.Location = new System.Drawing.Point(12, 79);
            this.label_pfp.Name = "label_pfp";
            this.label_pfp.Size = new System.Drawing.Size(80, 35);
            this.label_pfp.TabIndex = 2;
            this.label_pfp.Text = "Profile Picture:";
            this.label_pfp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_pfp
            // 
            this.button_pfp.AllowDrop = true;
            this.button_pfp.Location = new System.Drawing.Point(98, 85);
            this.button_pfp.Name = "button_pfp";
            this.button_pfp.Size = new System.Drawing.Size(101, 23);
            this.button_pfp.TabIndex = 3;
            this.button_pfp.Text = "Upload Picture";
            this.button_pfp.UseVisualStyleBackColor = true;
            this.button_pfp.Click += new System.EventHandler(this.button_pfp_Click);
            this.button_pfp.DragDrop += new System.Windows.Forms.DragEventHandler(this.button_pfp_DragDrop);
            // 
            // picbox_pfp
            // 
            this.picbox_pfp.Location = new System.Drawing.Point(12, 114);
            this.picbox_pfp.Name = "picbox_pfp";
            this.picbox_pfp.Size = new System.Drawing.Size(200, 200);
            this.picbox_pfp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picbox_pfp.TabIndex = 4;
            this.picbox_pfp.TabStop = false;
            // 
            // button_submit
            // 
            this.button_submit.Location = new System.Drawing.Point(542, 265);
            this.button_submit.Name = "button_submit";
            this.button_submit.Size = new System.Drawing.Size(90, 40);
            this.button_submit.TabIndex = 5;
            this.button_submit.Text = "Submit";
            this.button_submit.UseVisualStyleBackColor = true;
            this.button_submit.Click += new System.EventHandler(this.button_submit_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(429, 265);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(90, 40);
            this.button_cancel.TabIndex = 6;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // openFileDialog_pfp
            // 
            this.openFileDialog_pfp.DefaultExt = "jpg";
            this.openFileDialog_pfp.Filter = "*.jpg |";
            this.openFileDialog_pfp.InitialDirectory = "C:\\";
            this.openFileDialog_pfp.Title = "Select Picture";
            // 
            // folderBrowserDialog_pfp
            // 
            this.folderBrowserDialog_pfp.Description = "Select Picture";
            this.folderBrowserDialog_pfp.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // Form_Configure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 316);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_submit);
            this.Controls.Add(this.picbox_pfp);
            this.Controls.Add(this.button_pfp);
            this.Controls.Add(this.label_pfp);
            this.Controls.Add(this.textbox_username);
            this.Controls.Add(this.label_username);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.Name = "Form_Configure";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "User Account";
            this.Load += new System.EventHandler(this.Form_Configure_Load);
            ((System.ComponentModel.ISupportInitialize) (this.picbox_pfp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_pfp;

        private System.Windows.Forms.OpenFileDialog openFileDialog_pfp;

        private System.Windows.Forms.Button button_cancel;

        private System.Windows.Forms.Button button_submit;

        private System.Windows.Forms.PictureBox picbox_pfp;

        private System.Windows.Forms.Button button_pfp;

        private System.Windows.Forms.Label label_pfp;

        private System.Windows.Forms.TextBox textbox_username;

        private System.Windows.Forms.Label label_username;

        #endregion
    }
}