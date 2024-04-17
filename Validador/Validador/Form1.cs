using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.VisualBasic.Devices;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Metadata;

namespace Validador
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.Text += $" - v{Atualizacao.Versao}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var caminhoAtualizacao = (@"S:\TI\Ranyel\Updates programas\Singularity Installer.exe");
            var versaoAtualizador = FileVersionInfo.GetVersionInfo(caminhoAtualizacao).ProductVersion.Replace(".", "");
            var versaoAtualizadorInt = int.Parse(versaoAtualizador);
            if (versaoAtualizadorInt > int.Parse(Atualizacao.Versao.Replace(".", "")))
            {
                var resposta = MessageBox.Show("Existe uma nova versão do programa, deseja instalar?", "Atualização disponível", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (resposta != DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
                else
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(caminhoAtualizacao);
                    startInfo.Arguments = "/silent";
                    Process.Start(startInfo);
                    Environment.Exit(0);
                }
            }
        }

        private void selecionarCaminhoArquivo_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fdb = new FolderBrowserDialog();
            var resposta = fdb.ShowDialog();
            if (resposta == DialogResult.OK)
            {
                txtCaminhoArquivo.Text = fdb.SelectedPath;
                periodoInicial.Focus();
            }
        }
        private void periodo_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            txtBox.MaxLength = 10;
            var d = e.KeyChar.ToString();
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '\u0001')
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar != '\b')
            {
                switch (txtBox.Text.Length)
                {
                    case 2:
                        txtBox.Text += "/";
                        txtBox.Select(txtBox.Text.Length, 0);
                        break;
                    case 5:
                        txtBox.Text += "/";
                        txtBox.Select(txtBox.Text.Length, 0);
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selecaoTipoArquivo.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione o tipo de Arquivo a ser validado", "Erro de preenchimento!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtCaminhoArquivo.Text == "")
            {
                MessageBox.Show("A pasta selecionada não é valida, por favor, selecione um diretório válido!", "Erro de preenchimento!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(txtCaminhoArquivo.Text))
            {
                MessageBox.Show("O caminho das escriturações preenchido é invalido!", "Erro de preenchimento!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var arquivos = Directory.GetFiles(txtCaminhoArquivo.Text, "*.txt", SearchOption.AllDirectories);
            List<DadosComplementares> dadosGeral = new List<DadosComplementares>();
            List<DateTime> periodos = new List<DateTime>();
            DateTime inicioPeriodo;
            DateTime finalPeriodo;
            try
            {
                inicioPeriodo = DateTime.Parse(periodoInicial.Text);
                finalPeriodo = DateTime.Parse(periodoFinal.Text);
            }
            catch
            {
                MessageBox.Show("Período invalido!", "Erro de preenchimento!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (inicioPeriodo > finalPeriodo)
            {
                MessageBox.Show("Período invalido, a data final é posterior a data inicial!", "Erro de preenchimento!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (string caminhoArquivo in arquivos)
            {
                DadosComplementares dadosEmpresa = new DadosComplementares();
                var dadosArquivo = File.ReadAllLines(caminhoArquivo);
                string primeiraLinha = dadosArquivo[0];
                string[] dadosPrimeiraLinha = primeiraLinha.Split('|');


                if (selecaoTipoArquivo.SelectedIndex == 0)
                {
                    dadosEmpresa.RazaoSocial = dadosPrimeiraLinha[8];
                    dadosEmpresa.CnpjEmpresa = dadosPrimeiraLinha[9];
                    try
                    {
                        dadosEmpresa.Periodo = DateTime.ParseExact(dadosPrimeiraLinha[6], "ddMMyyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        MessageBox.Show("Existe mais de um tipo de arquivo no caminho selecionado", "Erro de preenchimento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    dadosEmpresa.RazaoSocial = dadosPrimeiraLinha[6];
                    dadosEmpresa.CnpjEmpresa = dadosPrimeiraLinha[7];
                    dadosEmpresa.Periodo = DateTime.ParseExact(dadosPrimeiraLinha[4], "ddMMyyyy", CultureInfo.InvariantCulture);

                    if (dadosArquivo.FirstOrDefault(x => x.StartsWith("|1300|")) != null)
                    {
                        var produtos = new List<Produto>();
                        string[] ncms = new string[] { "27101259", "22071090", "27101921" };

                        foreach (string linhaProduto in dadosArquivo.Where(x => x.StartsWith("|0200|")))
                        {
                            var dadosLinha = linhaProduto.Split('|');
                            if (ncms.Contains(dadosLinha[8]))
                            {
                                var produto = new Produto();
                                produto.Codigo = dadosLinha[2];
                                produto.Descricao = dadosLinha[3];
                                produto.NCM = dadosLinha[8];
                                produtos.Add(produto);
                            }
                        }
                        dadosEmpresa.TemMovimentoLMC = true;

                        foreach (Produto produto in produtos)
                        {
                            var registrosLMC = dadosArquivo.Where(x => x.StartsWith($"|1300|{produto.Codigo}")).ToList();
                            bool temMovimento = registrosLMC.Select(x => x.Split('|')[11]).Distinct().Count() > 1;
                            if (!temMovimento)
                            {
                                dadosEmpresa.TemMovimentoLMC = false;
                            }
                        }
                    }
                    else
                    {
                        dadosEmpresa.TemMovimentoLMC = false;
                    }
                }

                if (dadosEmpresa.Periodo < inicioPeriodo || dadosEmpresa.Periodo > finalPeriodo)
                {
                    continue;
                }
                dadosEmpresa.Existe = true;
                dadosEmpresa.Zerada = dadosArquivo.FirstOrDefault(x => x.StartsWith("|M100|") || x.StartsWith("|C100|") || x.StartsWith("|C490|")) == null;
                dadosGeral.Add(dadosEmpresa);
            }
            if (dadosGeral.Count == 0)
            {
                MessageBox.Show("O caminho selecionado não possui o arquivo do périodo informado", "Erro de preenchimento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime dataAtual = inicioPeriodo;
            while (dataAtual <= finalPeriodo)
            {
                periodos.Add(dataAtual);
                dataAtual = dataAtual.AddMonths(1);


            }
            foreach (DateTime periodo in periodos)
            {
                var dadoExistente = dadosGeral.FirstOrDefault(x => x.Periodo == periodo);
                if (dadoExistente == null)
                {
                    DadosComplementares existe = new DadosComplementares();
                    existe.Periodo = periodo;
                    existe.Existe = false;
                    dadosGeral.Add(existe);
                }
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Dados Validados");
                int colunaDados = 3;
                worksheet.Columns(3, 3 + periodos.Count).Width = 25;
                foreach (DateTime periodo in periodos)
                {
                    worksheet.Cell(1, colunaDados).Value = "'" + periodo.ToString("MM-yyyy");
                    colunaDados++;
                }
                int linha = 2;
                worksheet.Cell("A1").Value = "Razão Social";
                worksheet.Cell("B1").Value = "CNPJ";
                worksheet.Column(2).Width = 18;
                worksheet.Column(1).Width = 40;
                foreach (var cnpj in dadosGeral.Select(x => x.CnpjEmpresa).Distinct())
                {
                    colunaDados = 3;
                    worksheet.Cell(linha, 2).Style.NumberFormat.Format = "00\".\"000\".\"000\"/\"0000\"-\"00";
                    var razaoAtual = dadosGeral.FirstOrDefault(x => x.CnpjEmpresa == cnpj).RazaoSocial;
                    worksheet.Cell(linha, 2).Value = cnpj;
                    worksheet.Cell(linha, 1).Value = razaoAtual;

                    foreach (DateTime periodo in periodos)
                    {
                        if (cnpj == null)
                        {
                            continue;
                        }
                        var validacao = dadosGeral.FirstOrDefault(x => x.CnpjEmpresa == cnpj && periodo == x.Periodo);
                        if (validacao != null)
                        {
                            worksheet.Cell(linha, colunaDados).Value = validacao.Zerada ? "Escrituração Zerada" : "Escrituração OK";
                            if (selecaoTipoArquivo.SelectedIndex == 1 && !validacao.TemMovimentoLMC)
                            {
                                worksheet.Cell(linha, colunaDados).Value = "Não tem movimento LMC";
                            }
                        }
                        else
                        {
                            worksheet.Cell(linha, colunaDados).Value = "Escrituração Não transmitida";
                        }
                        worksheet.Range(1, 1, linha, colunaDados).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Range(1, 1, linha, colunaDados).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Range(linha, colunaDados, linha, colunaDados).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        colunaDados++;
                    }
                    linha++;
                }
                try
                {
                    workbook.SaveAs(txtCaminhoPlanilha.Text);
                }
                catch
                {
                    MessageBox.Show("Você está com outra planilha aberta! Feche-a para continuar", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            MessageBox.Show("Seu arquivo foi gerado com sucesso!", "Concluido", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void salvarComo_Click(object sender, EventArgs e)
        {
            SaveFileDialog salvarComo = new SaveFileDialog();
            salvarComo.Filter = "Planilha de Validação |*.xlsx";
            salvarComo.FileName = "Validação";
            var resposta = salvarComo.ShowDialog();
            if (resposta == DialogResult.OK)
            {
                txtCaminhoPlanilha.Text = salvarComo.FileName;
            }
            else
            {
                MessageBox.Show("Caminho invalido, selecione um caminho válido", "Erro de preenchimento!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button1.Focus();
        }

        private void periodoInicial_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.A)
            {
                return;
            }
            if (periodoInicial.Text.Length == 10)
            {
                periodoFinal.Focus();
            }
        }

        private void periodoFinal_KeyUp(object sender, KeyEventArgs e)
        {
            if (periodoFinal.Text.Length == 10)
            {
                salvarComo.Focus();
            }
        }
    }
}