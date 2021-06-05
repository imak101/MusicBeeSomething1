﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using MusicBeePlugin.Form.Popup;

namespace MusicBeePlugin.Form.Configure
{
    public partial class Form_Configure : System.Windows.Forms.Form
    {
        private string _filePath = string.Empty;
        private string _fileName = string.Empty;
        private string _username = string.Empty;
        private PluginSettings _settings;


        public Form_Configure(ref PluginSettings settings)
        {
            InitializeComponent();
            _settings = settings;
        }

        private void Form_Configure_Load(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void button_submit_Click(object sender, EventArgs e)
        {
            if (textbox_username.Text == string.Empty)
            {
                SystemSounds.Asterisk.Play();
                throw new Exception("Please enter a username.\n\n\n");
            }
            if (picbox_pfp.Image == null)
            {
                SystemSounds.Asterisk.Play();
                throw new Exception("Please select a picture.\n\n\n");
            }
            //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            _username = textbox_username.Text;
            Form_Popup popup = new Form_Popup(_settings.GetFromKey("username"), "key test");
            popup.Show();

        }
        
        private static string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private static void SetSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Debug.WriteLine(configuration.FilePath);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection(configuration.AppSettings.SectionInformation.Name);
            
            ExeConfigurationFileMap ds = new ExeConfigurationFileMap();
            
        }

        private void ffff(string key, string val)
        {
            var con = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationManager.AppSettings[key] = val;
            con.Save(ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection("appSettings");
            label_pfp.Text = ConfigurationManager.AppSettings.Get("pfpPath");

        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
        
        private void button_pfp_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = openFileDialog_pfp)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _filePath = dialog.FileName;
                    _fileName = dialog.SafeFileName;
                    
                    var picStream = dialog.OpenFile(); // ONLY PictureBoxSizeMode.Zoom WORKS

                    try
                    {
                        using (var picBitmap = new Bitmap(picStream))
                        {
                            picbox_pfp.Image = Image.FromHbitmap(picBitmap.GetHbitmap());
                        }
                    }
                    catch (ArgumentException)
                    {
                        SystemSounds.Asterisk.Play();

                        _filePath = string.Empty;
                        _fileName = string.Empty;

                        picbox_pfp.Image = null;
                        
                        throw new Exception("This file is invalid. Your file's dimensions may be too large or not in valid .jpg or .png form.");
                    }
                }

            }
        }

        private void button_pfp_DragDrop(object sender, DragEventArgs e)
        {
            throw new System.NotImplementedException();
        }
        public bool CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;

            foreach (System.Windows.Forms.Form frm in fc)
            {
                if (frm.Text == name)
                {
                    return true; 
                }
            }
            return false;
        }
    }
}