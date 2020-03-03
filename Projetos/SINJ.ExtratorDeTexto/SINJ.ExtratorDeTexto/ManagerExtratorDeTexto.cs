using BRLight.Extractor;

namespace SINJ.ExtratorDeTexto
{
    public class ManagerExtratorDeTexto
    {
        private AD.AcessaDados _acessaDados;

        /// <summary>
        /// Instancía a classe
        /// </summary>
        /// <param name="stringConnection">string de conexao do lightbase</param>
        public ManagerExtratorDeTexto(string stringConnection)
        {
            _acessaDados = new AD.AcessaDados(stringConnection);
        }

        /// <summary>
        /// Extrai o texto do arquivo do registro informado e salva no lightbase no campo texto
        /// </summary>
        /// <param name="id_reg">id ou chave do registro na base.</param>
        /// <param name="nm_base">nome da base.</param>
        /// <param name="nm_coluna_id">nome do campo que identifica o registro. Ex: id, chave, uuid...</param>
        /// <param name="nm_coluna_texto">nome do campo onde o texto será salvo.</param>
        /// <param name="nm_coluna_path_file">nome do campo onde se encontra salvo o local do arquivo.</param>
        /// <param name="path_repository_files">caminho absoluto do local onde ficam armazenados os arquivos.</param>
        /// <returns>Retorna o texto extraído</returns>
        public string ExtrairTexto(string id_reg, string nm_base, string nm_coluna_id, string nm_coluna_texto, string nm_coluna_path_file, string path_repository_files)
        {
            string textFile = ExtrairTexto(id_reg, nm_base, nm_coluna_id, nm_coluna_path_file, path_repository_files);
            BRLight.Util.Files.CriaArquivo(path_repository_files + @"teste", textFile, true);
            if(string.IsNullOrEmpty(textFile))
            {
                return "";
            }
            int updated = SalvarTexto(id_reg, nm_base, nm_coluna_id, nm_coluna_texto, textFile);
            if(updated <= 0)
            {
                throw new SinjExtratorDeTextoException(string.Format("Nenhum registro foi atualizado ao salvar texto."));
            }
            return textFile;
        }

        /// <summary>
        /// Extrai o texto do arquivo do registro informado
        /// </summary>
        /// <param name="id_reg">id ou chave do registro na base.</param>
        /// <param name="nm_base">nome da base.</param>
        /// <param name="nm_coluna_id">nome do campo que identifica o registro. Ex: id, chave, uuid...</param>
        /// <param name="nm_coluna_path_file">nome do campo onde se encontra salvo o local do arquivo.</param>
        /// <param name="path_repository_files">caminho absoluto do local onde ficam armazenados os arquivos.</param>
        /// <returns>Retorna o texto extraído</returns>
        public string ExtrairTexto(string id_reg, string nm_base, string nm_coluna_id, string nm_coluna_path_file, string path_repository_files)
        {
            string pathFile = _acessaDados.BuscarCaminhoArquivo(id_reg, nm_base, nm_coluna_id, nm_coluna_path_file);
            return new ManagerExtractor().ExtrairTextoDoArquivo(path_repository_files + pathFile);
        }

        /// <summary>
        /// Verifica na base informada os registros que possuem o campo texto vazio. Obs.: O campo da base deve estar configurado para aceitar consulta vazia, caso contrário trará todos os registros.
        /// </summary>
        /// <param name="nm_base">nome da base que será consultada</param>
        /// <param name="nm_coluna_id">nome do campo que identifica o registro. Ex: id, chave, uuid...</param>
        /// <param name="nm_coluna_texto">nome do campo de texto que será verificado.</param>
        /// <returns>retorna uma lista com os ids dos registros que estão com o campo, informado em nm_coluna_texto, vazio.</returns>
        public string[] ListarIdsSemTexto(string nm_base, string nm_coluna_id, string nm_coluna_texto)
        {
            return _acessaDados.ListarIdsSemTexto(nm_base, nm_coluna_id, nm_coluna_texto);
        }

        /// <summary>
        /// Atualiza o campo texto com o texto passado
        /// </summary>
        /// <param name="id_reg">id ou chave do registro na base.</param>
        /// <param name="nm_base">nome da base.</param>
        /// <param name="nm_coluna_id">nome do campo que identifica o registro. Ex: id, chave, uuid...</param>
        /// <param name="nm_coluna_texto">nome do campo de texto que será verificado.</param>
        /// <param name="textFile">texto que será salvo.</param>
        /// <returns>returna o número de registros atualizados. Se igual a zero, então nenhum registro foi atualizado. Se maior que um, então mais de um registro foi atualizado. Se igual a 1, ótimo, só o registro informado foi atualizado.</returns>
        private int SalvarTexto(string id_reg, string nm_base, string nm_coluna_id, string nm_coluna_texto, string textFile)
        {
            return _acessaDados.SalvarTextoArquivo(id_reg, nm_base, textFile, nm_coluna_id, nm_coluna_texto);
        }
    }
}
