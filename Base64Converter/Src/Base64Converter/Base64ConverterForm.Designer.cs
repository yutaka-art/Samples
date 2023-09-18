namespace Base64Converter
{
    partial class Base64ConverterForm
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
            grpBox01 = new System.Windows.Forms.GroupBox();
            rdoDecode = new System.Windows.Forms.RadioButton();
            rdoEncode = new System.Windows.Forms.RadioButton();
            label1 = new System.Windows.Forms.Label();
            grpBox01.SuspendLayout();
            SuspendLayout();
            // 
            // grpBox01
            // 
            grpBox01.Controls.Add(rdoDecode);
            grpBox01.Controls.Add(rdoEncode);
            grpBox01.Location = new System.Drawing.Point(29, 12);
            grpBox01.Name = "grpBox01";
            grpBox01.Size = new System.Drawing.Size(543, 95);
            grpBox01.TabIndex = 0;
            grpBox01.TabStop = false;
            grpBox01.Text = "エンコードデコード";
            // 
            // rdoDecode
            // 
            rdoDecode.AutoSize = true;
            rdoDecode.Location = new System.Drawing.Point(172, 47);
            rdoDecode.Name = "rdoDecode";
            rdoDecode.Size = new System.Drawing.Size(89, 29);
            rdoDecode.TabIndex = 1;
            rdoDecode.Text = "デコード";
            rdoDecode.UseVisualStyleBackColor = true;
            // 
            // rdoEncode
            // 
            rdoEncode.AutoSize = true;
            rdoEncode.Checked = true;
            rdoEncode.Location = new System.Drawing.Point(34, 47);
            rdoEncode.Name = "rdoEncode";
            rdoEncode.Size = new System.Drawing.Size(102, 29);
            rdoEncode.TabIndex = 0;
            rdoEncode.TabStop = true;
            rdoEncode.Text = "エンコード";
            rdoEncode.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(28, 124);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(489, 25);
            label1.TabIndex = 1;
            label1.Text = "エンコード・デコードを指定し、ファイルをドラッグ＆ドロップしてください。";
            // 
            // Base64ConverterForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(602, 187);
            Controls.Add(label1);
            Controls.Add(grpBox01);
            Name = "Base64ConverterForm";
            Text = "Base64ConverterForm";
            Load += Base64ConverterForm_Load;
            DragDrop += Base64ConverterForm_DragDrop;
            DragEnter += Base64ConverterForm_DragEnter;
            grpBox01.ResumeLayout(false);
            grpBox01.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox01;
        private System.Windows.Forms.RadioButton rdoDecode;
        private System.Windows.Forms.RadioButton rdoEncode;
        private System.Windows.Forms.Label label1;
    }
}
