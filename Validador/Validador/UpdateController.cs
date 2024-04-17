using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.IO;
using Forms = System.Windows.Forms;

namespace Validador
{
    public class UpdateController
    {

        private static string tokenCriptografado = "Z2hwX0lqWFk1QTVFc1Z1dFZ2dHhUNmEybWtGbm5hYURxSjQzMmpuaw==";

        public static bool BaixarAtualizacao(JObject dadosVersao, bool perguntar = true, bool informarVersaoAtual = true, bool forcar = false)
        {
            var caminhoDownload = Path.GetTempPath().TrimEnd('\\');

            void Baixar()
            {
                try
                {
                    var token = Encoding.UTF8.GetString(Convert.FromBase64String(tokenCriptografado));

                    using (var httpClient = new WebClient())
                    {
                        httpClient.Headers.Add("Authorization", string.Concat("token ", token));
                        httpClient.Headers.Add("User-Agent", "Singularity");
                        httpClient.DownloadFile((string)dadosVersao["URL_Arquivo"], caminhoDownload + "\\" + dadosVersao["Nome_Arquivo"]);

                        Process p = new Process();
                        p.StartInfo.Arguments = "/silent /forcecloseapplications /norestart";
                        p.StartInfo.FileName = caminhoDownload + "\\" + dadosVersao["Nome_Arquivo"];
                        p.Start();
                    }
                }
                catch { }
            }

            if (ExisteAtualizacao(dadosVersao))
            {
                if (perguntar)
                {
                        var dr = Forms.MessageBox.Show("Existe uma nova versão do programa, deseja baixar e instalar?", "Atualização Disponível", Forms.MessageBoxButtons.YesNo, Forms.MessageBoxIcon.Question);
                        if (dr == Forms.DialogResult.Yes)
                        {
                            Baixar();
                            Environment.Exit(0);
                        }
                        else if (forcar)
                        {
                            Environment.Exit(0);
                        }
                }
                else
                {
                    Baixar();
                    Environment.Exit(0);
                }
            }
            else if (informarVersaoAtual)
            {
                    Forms.MessageBox.Show("O programa se encontra na última versão!", "", Forms.MessageBoxButtons.OK, Forms.MessageBoxIcon.Information);
            }

            return false;
        }

        public static JObject ObterInformacoesAtualizacao(bool exibirErros = false)
        {
            var dadosVersao = new JObject {
                { "Versao", "" },
                { "URL_Arquivo", "" },
                { "Nome_Arquivo", "" },
            };

            try
            {
                var token = Encoding.UTF8.GetString(Convert.FromBase64String(tokenCriptografado));

                using (var httpClient = new WebClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    httpClient.Headers.Add("Authorization", string.Concat("token ", token));
                    httpClient.Headers.Add("User-Agent", "Singularity");
                    var contentsUrl = $"https://api.github.com/repos/RanyelCerracena/Validador_EFD/contents/Updates?ref=main";
                    var contentData = httpClient.DownloadData(contentsUrl);
                    var contentsJson = Encoding.UTF8.GetString(contentData);
                    var contents = (JArray)JsonConvert.DeserializeObject(contentsJson);

                    foreach (var file in contents)
                    {
                        var fileType = (string)file["type"];
                        if (fileType == "file")
                        {
                            var downloadUrl = (string)file["download_url"];
                            if ((string)file["name"] == "Versão Atual.txt")
                            {
                                var versao_git = httpClient.DownloadString(downloadUrl);
                                dadosVersao["Versao"] = versao_git.Replace("\n", "").Replace("v", "");
                            }
                            else
                            {
                                dadosVersao["URL_Arquivo"] = downloadUrl;
                                dadosVersao["Nome_Arquivo"] = (string)file["name"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (exibirErros)
                {
                        Forms.MessageBox.Show($"Não foi possível obter as informações sobre atualizações!\nErro: {ex.Message}\n\nStack Trace: {ex.StackTrace}",
                            "Erro ao Obter Dados da Atualização", Forms.MessageBoxButtons.OK, Forms.MessageBoxIcon.Warning);

                }
            }
            return dadosVersao;
        }

        public static bool ExisteAtualizacao(JObject dadosVersao)
        {
            return (string)dadosVersao["Versao"] != "" && int.Parse(dadosVersao["Versao"].ToString().Replace("\n", "").Replace("v", "").Replace(".", "")) > int.Parse(Versao.Replace(".", ""));
        }

        public static string Versao
        {
            get
            {
                var versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return versao.Length == 7 && versao.EndsWith(".0") ? versao.TrimEnd('0').TrimEnd('.') : versao;
            }
        }
    }
}
