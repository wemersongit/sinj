using System;
using System.IO;
using System.Text;

namespace BRLight.Util
{
    public static class Files
    {

        public static int ApagaArquivo(string pathArquivo)
        {
            try
            {
                if (File.Exists(pathArquivo))
                    File.Delete(pathArquivo);
            }
            catch (Exception)
            {
            }

            return File.Exists(pathArquivo) ? 0 : 1;
        }

        public static bool renomearArquivo(string pathArquivo, string pathArquivoNovo)
        {
            try
            {
                if (File.Exists(pathArquivo))
                    File.Move(pathArquivo, pathArquivoNovo);
            }
            catch (Exception)
            {
            }

            return File.Exists(pathArquivoNovo) ? true : false;
        }

        public static int CriaArquivo(string pathArquivo, byte[] bytesArquivo)
        {
            FileStream fStream = null;

            try
            {
                fStream = new FileStream(pathArquivo, FileMode.Create);
                fStream.Write(bytesArquivo, 0, bytesArquivo.Length);
            }
            catch (Exception ex)
            {
                throw new Exception("CriaArquivo" + pathArquivo, ex);
            }
            finally
            {
                if (fStream != null)
                {
                    fStream.Close();
                    fStream.Dispose();
                }
            }
            return File.Exists(pathArquivo) ? 1 : 0;
        }

        public static int CriaArquivo(string pathArquivo, string stringArquivo)
        {
            return CriaArquivo(pathArquivo, stringArquivo, false);
        }

        public static int CriaArquivo(string pathArquivo, string stringArquivo, bool append)
        {
            TextWriter tw = null;

            try
            {
                tw = new StreamWriter(pathArquivo, append, Encoding.Default);
                tw.Write(stringArquivo);
            }
            catch (Exception ex)
            {
                throw new Exception("CriaArquivo - " + pathArquivo, ex);
            }
            finally
            {
                if (tw != null)
                {
                    tw.Close();
                    tw.Dispose();
                }
            }
            return File.Exists(pathArquivo) ? 1 : 0;
        }

        public static byte[] LeArquivo(string pathArquivo)
        {
            byte[] retorno;
            FileStream fStream = null;

            try
            {
                fStream = new FileStream(pathArquivo, FileMode.Open);
                retorno = new byte[fStream.Length];
                fStream.Read(retorno, 0, (int)fStream.Length);
            }
            catch (Exception ex)
            {
                throw new Exception("LeArquivo - " + pathArquivo, ex);
            }
            finally
            {
                if (fStream != null)
                {
                    fStream.Close();
                    fStream.Dispose();
                }
            }
            return retorno;
        }

        public static string strLeArquivo(string pathArquivo)
        {
            string retorno = null;
            if (!File.Exists(pathArquivo))
            {
                return retorno;
            }

            StreamReader fStream = null;
            try
            {
                fStream = new StreamReader(pathArquivo, EncodingDetector.GetFileEncoding(pathArquivo));
                retorno = fStream.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("strLeArquivo - " + pathArquivo, ex);
            }
            finally
            {
                if (fStream != null)
                {
                    fStream.Close();
                    fStream.Dispose();
                }
            }
            return retorno;
        }

        public static void ArquivoApend(string corpo, string pathArquivo, string stringArquivo)
        {

            //le o texto do arquivo
            using (var sr = new StreamReader(pathArquivo + stringArquivo, Encoding.UTF8))
            {
                string textoArq = sr.ReadToEnd();
                sr.Close();
                corpo = textoArq + corpo;
            }

            //escrever texto no arquivo
            using (var sw = new StreamWriter(pathArquivo + stringArquivo, false, Encoding.UTF8))
            {
                sw.Write(corpo);
                sw.Close();
            }

        }

        public static void CriarDiretorio(string pathDir)
        {
            var info = new DirectoryInfo(pathDir);
            if (!info.Exists)
                info.Create();
        }

    }
}