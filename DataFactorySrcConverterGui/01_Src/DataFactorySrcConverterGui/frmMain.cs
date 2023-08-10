using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using System.IO.Compression;

namespace DataFactorySrcConverterGui
{
    public partial class frmMain : Form
    {
        #region ■変数・定数宣言部
        enum _LogLevel { Info, Warn, Err }

        private StringBuilder _LogFlName = null;
        private StringBuilder _LogDirectory = null;
        private StringBuilder _ExecuteDirectory = null;
        #endregion

        #region ■初期処理
        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = Environment.CurrentDirectory;

            this.toolStripStatusLabel1.Text = "";
            this.toolStripSplitButton1.Visible = false;

            StringBuilder dirLogCus = new StringBuilder();
            dirLogCus.Append(Environment.MachineName + "_");
            //dirLogCus.Append(DateTime.Now.ToString("yyyyMMdd-HHmmss.fff"));
            dirLogCus.Append(DateTime.Now.ToString("yyyyMMdd"));

            // ■ログ用ファイル名称設定
            _LogDirectory = new StringBuilder();
            _LogDirectory.Append(AppDomain.CurrentDomain.BaseDirectory);
            _LogDirectory.Append(@"Log\" + dirLogCus.ToString() + @"\");

            _LogFlName = new StringBuilder();
            _LogFlName.Append(_LogDirectory + @"\");
            _LogFlName.Append($"{DateTime.Now.ToString("yyyyMMdd-HHmmss")}_{Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".exe", ".log")}");

            _ExecuteDirectory = new StringBuilder();
            _ExecuteDirectory.Append(_LogDirectory.ToString());

            if (Directory.Exists(_LogDirectory.ToString()) == false)
            {
                Directory.CreateDirectory(_LogDirectory.ToString());
            }

            try
            {
                // LOG:フォームロード処理を開始しました。
                CreateLog(_LogLevel.Info, "フォームロード処理を開始しました。");

            }
            catch (Exception ex)
            {
                CreateLog(_LogLevel.Err, ex.ToString());
                MessageBox.Show("フォームロードで予期せぬエラーが発生しました。\r\n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        #endregion

        #region ■ログ出力処理
        /// <summary>
        /// ログ出力　自前
        /// </summary>
        /// <param name="pMsg"></param>
        private void CreateLog(_LogLevel pLv, String pMsg)
        {

            //書き込むファイルが既に存在している場合は、ファイルの末尾に追加する
            StreamWriter sw = new StreamWriter(_LogFlName.ToString(), true, Encoding.GetEncoding("shift_jis"));

            StringBuilder sb = new StringBuilder();
            //sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ");
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " ");

            switch (pLv)
            {
                case _LogLevel.Info:
                    sb.Append("[Info]" + " ");
                    break;
                case _LogLevel.Warn:
                    sb.Append("[Warn]" + " ");
                    break;
                case _LogLevel.Err:
                    sb.Append("[Err ]" + " ");
                    break;
            }
            sb.AppendLine(pMsg);

            sw.Write(sb.ToString());
            sw.Close();
        }

        #endregion

        #region ■終了処理
        /// <summary>
        /// 終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // LOG:終了処理を終了しました。
                CreateLog(_LogLevel.Info, "フォームロードを終了します。");
            }
            catch (Exception)
            {
                // 処理なし
            }
        }
        #endregion

        #region ■参照テキストボックス-ドラッグ＆ドロップ
        private void txtRefPath_DragEnter(object sender, DragEventArgs e)
        {
            // ドラッグ中のファイルやディレクトリの取得
            string[] sFileName = (string[])e.Data.GetData(DataFormats.FileDrop);

            //ファイルがドラッグされている場合、
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // 配列分ループ
                foreach (string sTemp in sFileName)
                {
                    // ファイルパスかチェック
                    if (File.Exists(sTemp) == false)
                    {
                        // ファイルパス以外なので何もしない
                        return;
                    }
                    else
                    {
                        break;
                    }
                }

                // カーソルを[+]へ変更する
                // ここでEffectを変更しないと、以降のイベント（Drop）は発生しない
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void txtRefPath_DragDrop(object sender, DragEventArgs e)
        {
            //ドロップされたファイルの一覧を取得
            string[] sFileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (sFileName.Length <= 0)
            {
                return;
            }

            // ドロップ先がTextBoxであるかチェック
            System.Windows.Forms.TextBox TargetTextBox = sender as System.Windows.Forms.TextBox;

            if (TargetTextBox == null)
            {
                // TextBox以外のためイベントを何もせずイベントを抜ける。
                return;
            }

            // 現状のTextBox内のデータを削除
            //TargetTextBox.Text = "";

            // TextBoxドラックされた文字列を設定
            //TargetTextBox.Text = sFileName[0]; // 配列の先頭文字列を設定

            string zipPath = sFileName[0];
            Extract(zipPath);
        }
        #endregion

        #region ■メイン処理
        /// <summary>
        /// 解凍
        /// </summary>
        /// <param name="zipPath"></param>
        private void Extract(string zipPath)
        {
            _ExecuteDirectory = new StringBuilder();
            _ExecuteDirectory.Append(_LogDirectory.ToString());
            _ExecuteDirectory.Append(DateTime.Now.ToString("HHmmss"));

            if (!Directory.Exists(_ExecuteDirectory.ToString()))
            {
                Directory.CreateDirectory(_ExecuteDirectory.ToString());
            }


            string extractPath = $"{_ExecuteDirectory.ToString()}\\Extract";
            if (Directory.Exists(extractPath))
            {
                DeleteDirectory(extractPath);
            }

            string targetPath = "";

            // ZIPファイルを開いてZipArchiveオブジェクトを作る
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                // 展開するファイルを選択する（ここでは、拡張子が".txt"のものとする）
                var allJsonFiles
                  = archive.Entries
                    .Where(a => a.FullName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(a => a.FullName);
                Console.WriteLine("全{0}ファイル（txtファイルは{1}）",
                                  archive.Entries.Count, allJsonFiles.Count());

                // 選択したファイルを指定したフォルダーに書き出す
                foreach (ZipArchiveEntry entry in allJsonFiles)
                {
                    // 除外リストに存在する場合スキップ
                    if (Properties.Settings.Default.ExclusionAry.Contains(entry.Name)) { continue; }

                    targetPath = Path.Combine(extractPath, entry.FullName);
                    if (!Directory.Exists(Path.GetDirectoryName(targetPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                    }

                    // ZipArchiveEntryオブジェクトのExtractToFileメソッドにフルパスを渡す
                    entry.ExtractToFile(targetPath);
                    Console.WriteLine("展開: {0}", entry.FullName);
                }
            }


            OutPut();
        }

        /// <summary>
        /// 出力
        /// </summary>
        private void OutPut()
        {
            var targetPath = _ExecuteDirectory.ToString();

            try
            {
                IEnumerable<string> files = Directory.EnumerateFiles(targetPath, "ArmTemplate_*.json", SearchOption.AllDirectories);

                foreach (string str in files)
                {
                    Execute(str);
                }

                MessageBox.Show("Finish!!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Console.ReadKey();
                System.Diagnostics.Process.Start(_ExecuteDirectory.ToString());


            }
            catch (Exception ex)
            {
                CreateLog(_LogLevel.Err, ex.ToString());
                MessageBox.Show("Fatal\r\n" + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="targetPath"></param>
        private void Execute(string targetPath)
        {

            using (var sr = new StreamReader(targetPath))
            {
                var jsonData = sr.ReadToEnd();
                var targetObj = JObject.Parse(jsonData);

                foreach (var item in targetObj)
                {
                    if (item.Key.ToString().Equals("resources"))
                    {
                        string outFileName = string.Empty;
                        string outDirectoryName = string.Empty;
                        string outResultWk = string.Empty;
                        var outResult = new System.Text.StringBuilder();

                        foreach (var item2 in item.Value)
                        {
                            foreach (var item3 in item2)
                            {
                                string target = string.Empty;

                                target = item3.ToString();

                                if (target.Contains("name"))
                                {
                                    var ary = item3.First.ToString().Split(',');

                                    outFileName = ary[1];
                                    outFileName = outFileName.Replace(" '/", "");
                                    outFileName = outFileName.Replace("')]", "");

                                    var ary2 = outFileName.Split('/');
                                    if (ary2.Length > 0)
                                    {
                                        outFileName = ary2[ary2.Length - 1];
                                    }

                                }

                                if (target.Contains("type"))
                                {
                                    var ary = item3.First.ToString().Split('/');

                                    outDirectoryName = ary[ary.Length - 1];
                                    break;
                                }
                            }

                            outResultWk = item2.ToString();
                            outResult = new System.Text.StringBuilder();

                            //string wkPath = $"{System.IO.Directory.GetCurrentDirectory()}\\{outDirectoryName}";
                            string wkPath = $"{_ExecuteDirectory.ToString()}\\Output\\{outDirectoryName}";

                            if (!Directory.Exists(wkPath))
                            {
                                Directory.CreateDirectory(wkPath);
                            }
                            wkPath = $"{wkPath}\\{outFileName}.json";

                            var outAry = outResultWk.Split('\n');

                            foreach (var t in outAry)
                            {
                                if (t.Contains("[concat(parameters('factoryName'), '/"))
                                {
                                    outResult.Append(Convert(t));
                                }
                                //else if (t.Contains("annotations"))
                                //{
                                //    //
                                //}
                                else if (t.Contains("lastPublishTime"))
                                {
                                    string wkText = t;
                                    wkText = wkText.Substring(0, 24);
                                    wkText = wkText + @"""" + "\n";
                                    outResult.Append(wkText);
                                }
                                else
                                {
                                    outResult.Append(t);
                                }

                            }

                            File.WriteAllText(wkPath, outResult.ToString());
                            //Console.WriteLine(wkPath);

                        }
                    }

                }

            }

        }

        /// <summary>
        /// 変換
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private string Convert(string target)
        {
            string result = string.Empty;

            result = target.Replace("[concat(parameters('factoryName'), '/", "");
            result = result.Replace("')]", "");

            return result;
        }

        /// <summary>
        /// 指定ファイル削除
        /// </summary>
        /// <param name="stFilePath">削除対象フルパス</param>
        private void DeleteFile(string pStFilePath)
        {
            FileInfo cFileInfo = new FileInfo(pStFilePath);

            // ファイルが存在しているか判断する
            if (cFileInfo.Exists == true)
            {
                // 読み取り専用属性がある場合は、読み取り専用属性を解除する
                if ((cFileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    cFileInfo.Attributes = FileAttributes.Normal;
                }

                // ファイルを削除する
                cFileInfo.Delete();
            }
        }

        /// <summary>
        /// フォルダを根こそぎ削除する（ReadOnlyでも削除）
        /// </summary>
        /// <param name="dir">削除するフォルダ</param>
        private void DeleteDirectory(string dir)
        {
            //DirectoryInfoオブジェクトの作成
            DirectoryInfo di = new DirectoryInfo(dir);

            //フォルダ以下のすべてのファイル、フォルダの属性を削除
            RemoveReadonlyAttribute(di);

            //フォルダを根こそぎ削除
            di.Delete(true);
        }

        private void RemoveReadonlyAttribute(DirectoryInfo dirInfo)
        {
            //基のフォルダの属性を変更
            if ((dirInfo.Attributes & FileAttributes.ReadOnly) ==
                FileAttributes.ReadOnly)
                dirInfo.Attributes = FileAttributes.Normal;
            //フォルダ内のすべてのファイルの属性を変更
            foreach (FileInfo fi in dirInfo.GetFiles())
                if ((fi.Attributes & FileAttributes.ReadOnly) ==
                    FileAttributes.ReadOnly)
                    fi.Attributes = FileAttributes.Normal;
            //サブフォルダの属性を回帰的に変更
            foreach (DirectoryInfo di in dirInfo.GetDirectories())
                RemoveReadonlyAttribute(di);
        }
        #endregion

    }
}
