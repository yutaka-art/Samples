﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Base64Converter
{
    public partial class Base64ConverterForm : Form
    {
        /// <summary>
        /// InitializeComponent
        /// </summary>
        public Base64ConverterForm()
        {
            InitializeComponent();
            this.toolStripStatusLabel1.Text = string.Empty;
        }

        /// <summary>
        /// Form_Load Initialize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Base64ConverterForm_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Base64ConverterForm_DragDrop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Base64ConverterForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            // 日時ベースのフォルダ名を生成
            var dateTimeFolder = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var outputPathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dateTimeFolder);
            Directory.CreateDirectory(outputPathDirectory); // フォルダが存在しない場合、作成

            var counter = 0;

            foreach (string file in files)
            {
                try
                {
                    if (rdoDecode.Checked)
                    {
                        var jsonContent = File.ReadAllText(file);
                        var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonContent);
                        var base64String = jsonObject?.inputParam_Base64.ToString();

                        if (string.IsNullOrEmpty(base64String))
                        {
                            throw new Exception("inputParam_Base64 が見つかりませんでした");
                        }

                        var decodedContent = Encoding.UTF8.GetString(Convert.FromBase64String(base64String));

                        var outputPath = Path.Combine(outputPathDirectory, Path.GetFileNameWithoutExtension(file) + "_Base64decode.json");
                        var outputPath2 = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "_Base64decode.json");

                        File.WriteAllText(outputPath, decodedContent);
                        File.WriteAllText(outputPath2, decodedContent);
                        //MessageBox.Show($"デコード成功: {outputPath}");
                        counter++;
                    }
                    else
                    {
                        var fileContent = File.ReadAllText(file);
                        var base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileContent));

                        var jsonObject = new { inputParam_Base64 = base64Encoded };
                        var json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

                        var outputPath = Path.Combine(outputPathDirectory, Path.GetFileNameWithoutExtension(file) + "_Base64encode.json");
                        var outputPath2 = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "_Base64encode.json");

                        File.WriteAllText(outputPath, json);
                        File.WriteAllText(outputPath2, json);
                        //MessageBox.Show($"エンコード成功: {outputPath}");
                        counter++;
                    }
                }
                catch (Exception ex)
                {
                    this.toolStripStatusLabel1.Text = $"エラー: {ex.Message}";
                }
            }

            this.toolStripStatusLabel1.Text = $"{counter.ToString()} 件処理しました。";
        }

        /// <summary>
        /// Base64ConverterForm_DragEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Base64ConverterForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

    }
}
