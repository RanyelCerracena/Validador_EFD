namespace Validador
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label1 = new Label();
            txtCaminhoArquivo = new TextBox();
            selecionarCaminhoArquivo = new Button();
            label2 = new Label();
            label3 = new Label();
            button1 = new Button();
            periodoFinal = new TextBox();
            periodoInicial = new TextBox();
            salvarComo = new Button();
            txtCaminhoPlanilha = new TextBox();
            label4 = new Label();
            selecaoTipoArquivo = new ComboBox();
            label5 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 123);
            label1.Name = "label1";
            label1.Size = new Size(151, 20);
            label1.TabIndex = 0;
            label1.Text = "Caminho do arquivo :";
            // 
            // txtCaminhoArquivo
            // 
            txtCaminhoArquivo.Location = new Point(180, 116);
            txtCaminhoArquivo.Name = "txtCaminhoArquivo";
            txtCaminhoArquivo.Size = new Size(317, 27);
            txtCaminhoArquivo.TabIndex = 1;
            // 
            // selecionarCaminhoArquivo
            // 
            selecionarCaminhoArquivo.Location = new Point(517, 114);
            selecionarCaminhoArquivo.Name = "selecionarCaminhoArquivo";
            selecionarCaminhoArquivo.Size = new Size(52, 29);
            selecionarCaminhoArquivo.TabIndex = 4;
            selecionarCaminhoArquivo.Text = ". . .";
            selecionarCaminhoArquivo.UseVisualStyleBackColor = true;
            selecionarCaminhoArquivo.Click += selecionarCaminhoArquivo_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 176);
            label2.Name = "label2";
            label2.Size = new Size(67, 20);
            label2.TabIndex = 5;
            label2.Text = "Período :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(249, 176);
            label3.Name = "label3";
            label3.Size = new Size(17, 20);
            label3.TabIndex = 7;
            label3.Text = "a";
            // 
            // button1
            // 
            button1.Location = new Point(215, 255);
            button1.Name = "button1";
            button1.Size = new Size(170, 75);
            button1.TabIndex = 9;
            button1.Text = "Verificar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // periodoFinal
            // 
            periodoFinal.Location = new Point(287, 169);
            periodoFinal.Name = "periodoFinal";
            periodoFinal.Size = new Size(121, 27);
            periodoFinal.TabIndex = 8;
            periodoFinal.KeyPress += periodo_KeyPress;
            periodoFinal.KeyUp += periodoFinal_KeyUp;
            // 
            // periodoInicial
            // 
            periodoInicial.Location = new Point(96, 169);
            periodoInicial.Name = "periodoInicial";
            periodoInicial.Size = new Size(121, 27);
            periodoInicial.TabIndex = 6;
            periodoInicial.KeyPress += periodo_KeyPress;
            periodoInicial.KeyUp += periodoInicial_KeyUp;
            // 
            // salvarComo
            // 
            salvarComo.Location = new Point(517, 213);
            salvarComo.Name = "salvarComo";
            salvarComo.Size = new Size(52, 32);
            salvarComo.TabIndex = 10;
            salvarComo.Text = ". . .";
            salvarComo.UseVisualStyleBackColor = true;
            salvarComo.Click += salvarComo_Click;
            // 
            // txtCaminhoPlanilha
            // 
            txtCaminhoPlanilha.Location = new Point(129, 213);
            txtCaminhoPlanilha.Name = "txtCaminhoPlanilha";
            txtCaminhoPlanilha.Size = new Size(368, 27);
            txtCaminhoPlanilha.TabIndex = 11;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(23, 216);
            label4.Name = "label4";
            label4.Size = new Size(98, 20);
            label4.TabIndex = 12;
            label4.Text = "Salvar como :";
            // 
            // selecaoTipoArquivo
            // 
            selecaoTipoArquivo.DropDownStyle = ComboBoxStyle.DropDownList;
            selecaoTipoArquivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            selecaoTipoArquivo.ForeColor = SystemColors.ControlText;
            selecaoTipoArquivo.FormattingEnabled = true;
            selecaoTipoArquivo.Items.AddRange(new object[] { "EFD Contribuições", "ICMS/IPI" });
            selecaoTipoArquivo.Location = new Point(247, 65);
            selecaoTipoArquivo.Name = "selecaoTipoArquivo";
            selecaoTipoArquivo.Size = new Size(317, 28);
            selecaoTipoArquivo.TabIndex = 13;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(21, 68);
            label5.Name = "label5";
            label5.Size = new Size(200, 20);
            label5.TabIndex = 14;
            label5.Text = "Selecione o tipo do arquivo :";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(593, 358);
            Controls.Add(label5);
            Controls.Add(selecaoTipoArquivo);
            Controls.Add(label4);
            Controls.Add(txtCaminhoPlanilha);
            Controls.Add(salvarComo);
            Controls.Add(button1);
            Controls.Add(periodoFinal);
            Controls.Add(label3);
            Controls.Add(periodoInicial);
            Controls.Add(label2);
            Controls.Add(selecionarCaminhoArquivo);
            Controls.Add(txtCaminhoArquivo);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "Sigularity";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtCaminhoArquivo;
        private TextBox textBox2;
        private Label label2;
        private Button selecionarCaminhoArquivo;
        private Button button2;
        private Label label3;
        private Button button1;
        private TextBox periodoFinal;
        private TextBox periodoInicial;
        private Button salvarComo;
        private TextBox txtCaminhoPlanilha;
        private Label label4;
        private ComboBox selecaoTipoArquivo;
        private Label label5;
    }
}
