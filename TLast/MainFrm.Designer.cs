
namespace TLast
{
    partial class MainFrm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblConnectedAccounts = new System.Windows.Forms.Label();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbboxGender = new System.Windows.Forms.ComboBox();
            this.txtFigure = new System.Windows.Forms.TextBox();
            this.btnCopyFigureString = new System.Windows.Forms.Button();
            this.txtRoomId = new System.Windows.Forms.TextBox();
            this.btnJoinRoom = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(454, 73);
            this.panel1.TabIndex = 0;
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(419, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 22);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "Fechar");
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblTitle.Font = new System.Drawing.Font("Roboto", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(242, 21);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(109, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "T L A S T";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTitle, "Clique para ver os Logs.");
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            this.panel3.Controls.Add(this.lblConnectedAccounts);
            this.panel3.Controls.Add(this.lblServerStatus);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(13, 17);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(158, 36);
            this.panel3.TabIndex = 0;
            this.panel3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // lblConnectedAccounts
            // 
            this.lblConnectedAccounts.AutoSize = true;
            this.lblConnectedAccounts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(64)))), ((int)(((byte)(92)))));
            this.lblConnectedAccounts.Location = new System.Drawing.Point(0, 18);
            this.lblConnectedAccounts.Name = "lblConnectedAccounts";
            this.lblConnectedAccounts.Size = new System.Drawing.Size(131, 15);
            this.lblConnectedAccounts.TabIndex = 3;
            this.lblConnectedAccounts.Text = "Contas Conectadas: 0";
            this.lblConnectedAccounts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblConnectedAccounts.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblServerStatus.Font = new System.Drawing.Font("Roboto", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblServerStatus.ForeColor = System.Drawing.Color.Red;
            this.lblServerStatus.Location = new System.Drawing.Point(88, 0);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(30, 15);
            this.lblServerStatus.TabIndex = 2;
            this.lblServerStatus.Text = "Não";
            this.lblServerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.lblServerStatus, "Clique para Ligar/Desligar a Proxy Local.");
            this.lblServerStatus.Click += new System.EventHandler(this.lblServerStatus_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(64)))), ((int)(((byte)(92)))));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Interceptando:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(47)))), ((int)(((byte)(61)))));
            this.panel2.Controls.Add(this.cbboxGender);
            this.panel2.Controls.Add(this.txtFigure);
            this.panel2.Controls.Add(this.btnCopyFigureString);
            this.panel2.Controls.Add(this.txtRoomId);
            this.panel2.Controls.Add(this.btnJoinRoom);
            this.panel2.Location = new System.Drawing.Point(12, 72);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(428, 162);
            this.panel2.TabIndex = 1;
            // 
            // cbboxGender
            // 
            this.cbboxGender.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(47)))), ((int)(((byte)(61)))));
            this.cbboxGender.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbboxGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbboxGender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbboxGender.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.cbboxGender.FormattingEnabled = true;
            this.cbboxGender.Items.AddRange(new object[] {
            "M",
            "F"});
            this.cbboxGender.Location = new System.Drawing.Point(97, 92);
            this.cbboxGender.Name = "cbboxGender";
            this.cbboxGender.Size = new System.Drawing.Size(52, 23);
            this.cbboxGender.TabIndex = 4;
            // 
            // txtFigure
            // 
            this.txtFigure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(47)))), ((int)(((byte)(61)))));
            this.txtFigure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFigure.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtFigure.Location = new System.Drawing.Point(155, 93);
            this.txtFigure.Name = "txtFigure";
            this.txtFigure.Size = new System.Drawing.Size(170, 23);
            this.txtFigure.TabIndex = 3;
            this.txtFigure.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnCopyFigureString
            // 
            this.btnCopyFigureString.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopyFigureString.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyFigureString.Location = new System.Drawing.Point(97, 122);
            this.btnCopyFigureString.Name = "btnCopyFigureString";
            this.btnCopyFigureString.Size = new System.Drawing.Size(228, 23);
            this.btnCopyFigureString.TabIndex = 2;
            this.btnCopyFigureString.Text = "Copiar Roupa";
            this.btnCopyFigureString.UseVisualStyleBackColor = true;
            this.btnCopyFigureString.Click += new System.EventHandler(this.btnCopyFigureString_Click);
            // 
            // txtRoomId
            // 
            this.txtRoomId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(47)))), ((int)(((byte)(61)))));
            this.txtRoomId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRoomId.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtRoomId.Location = new System.Drawing.Point(97, 19);
            this.txtRoomId.Name = "txtRoomId";
            this.txtRoomId.Size = new System.Drawing.Size(228, 23);
            this.txtRoomId.TabIndex = 1;
            this.txtRoomId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnJoinRoom
            // 
            this.btnJoinRoom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnJoinRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoinRoom.Location = new System.Drawing.Point(97, 48);
            this.btnJoinRoom.Name = "btnJoinRoom";
            this.btnJoinRoom.Size = new System.Drawing.Size(228, 23);
            this.btnJoinRoom.TabIndex = 0;
            this.btnJoinRoom.Text = "Entrar no Quarto";
            this.btnJoinRoom.UseVisualStyleBackColor = true;
            this.btnJoinRoom.Click += new System.EventHandler(this.btnJoinRoom_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(47)))), ((int)(((byte)(61)))));
            this.toolTip1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Informação";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(452, 246);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Roboto", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TLast";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblConnectedAccounts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblTitlee;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRoomId;
        private System.Windows.Forms.Button btnJoinRoom;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtFigure;
        private System.Windows.Forms.Button btnCopyFigureString;
        private System.Windows.Forms.ComboBox cbboxGender;
    }
}

