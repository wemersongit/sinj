using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.ManagerProcesses
{
    public class ManagerProcess
    {
        internal string extentAtualizados = Configuracao.LerValorChave("BaseForIndexerAndExporter");
        internal string extentExcluidos = Configuracao.LerValorChave("BaseForExcluder");
        internal string extentNormas = Configuracao.LerValorChave("BaseNormas");
        internal string extentDodfs = Configuracao.LerValorChave("BaseDodfs");
        internal string extentTiposDeNorma = Configuracao.LerValorChave("BaseTiposDeNorma");
        internal string extentOrgaos = Configuracao.LerValorChave("BaseOrgaos");
        internal string extentInteressados = Configuracao.LerValorChave("BaseInteressados");
        internal string extentAutorias = Configuracao.LerValorChave("BaseSinjAutorias");
        internal string extentRequerentes = Configuracao.LerValorChave("BaseRequerentes");
        internal string extentRequeridos = Configuracao.LerValorChave("BaseRequeridos");
        internal string extentPush = Configuracao.LerValorChave("BasePush");
        internal string extentTiposDeRelacao = Configuracao.LerValorChave("BaseTiposDeRelacao");
        internal string extentVocabularioControlado = Configuracao.LerValorChave("BaseVocabularioControlado");
        internal string quantidadeDeNormasPorProcesso = Configuracao.LerValorChave("QuantidadeDeNormasPorProcesso");
        internal string quantidadeDeDodfsPorProcesso = Configuracao.LerValorChave("QuantidadeDeDodfsPorProcesso");

        public void StartQuery(List<KeyValuePair<string, string>> listKeyValue, string exportarArquivos)
        {
            StartProcesses(listKeyValue, exportarArquivos);
        }

        public void StartAtlz()
        {
            List<KeyValuePair<string, string>> listKeyValue = MontaListKeyValue("atlz","");

            StartProcesses(listKeyValue, "true");
        }

        public void StartRegs(string tabela)
        {
            List<KeyValuePair<string, string>> listKeyValue = MontaListKeyValue("full", tabela);

            StartProcesses(listKeyValue, "false");
        }

        public void StartFiles(string tabela)
        {
            List<KeyValuePair<string, string>> listKeyValue = MontaListKeyValue("files", tabela);

            StartProcesses(listKeyValue, "true");
        }

        public void StartFull(string tabela)
        {
            List<KeyValuePair<string, string>> listKeyValue = MontaListKeyValue("full", tabela);

            StartProcesses(listKeyValue, "true");
        }

        private List<KeyValuePair<string, string>> MontaListKeyValue(string tipo, string tabela)
        {
            List<KeyValuePair<string, string>> listKeyValue = new List<KeyValuePair<string, string>>();
            KeyValuePair<string, string> keyValuePair;
            var iQuantidadeDeNormasPorProcesso = int.Parse(quantidadeDeNormasPorProcesso);
            var iQuantidadeDeDodfsPorProcesso = int.Parse(quantidadeDeDodfsPorProcesso);
            switch(tipo)
            {
                case "atlz":
                    keyValuePair = new KeyValuePair<string, string>(extentAtualizados, string.Format("select * from {0}", extentAtualizados));
                    listKeyValue.Add(keyValuePair);
                    break;
                case "full":
                    if (tabela.Equals(extentAutorias, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        keyValuePair = new KeyValuePair<string, string>(extentAutorias,string.Format("select * from {0}",extentAutorias));
                        listKeyValue.Add(keyValuePair);
                    }
                    if (tabela.Equals(extentOrgaos, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        keyValuePair = new KeyValuePair<string, string>(extentOrgaos,string.Format("select * from {0}", extentOrgaos));
                        listKeyValue.Add(keyValuePair);
                    }
                    if (tabela.Equals(extentTiposDeNorma, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        keyValuePair = new KeyValuePair<string, string>(extentTiposDeNorma,string.Format("select * from {0}",extentTiposDeNorma));
                        listKeyValue.Add(keyValuePair);
                    }
                    if (tabela.Equals(extentInteressados, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        keyValuePair = new KeyValuePair<string, string>(extentInteressados, string.Format("select * from {0}", extentInteressados));
                        listKeyValue.Add(keyValuePair);
                    }
                    if (tabela.Equals(extentRequerentes, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        keyValuePair = new KeyValuePair<string, string>(extentRequerentes, string.Format("select * from {0}", extentRequerentes));
                        listKeyValue.Add(keyValuePair);
                    }
                    if (tabela.Equals(extentRequeridos, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        keyValuePair = new KeyValuePair<string, string>(extentRequeridos, string.Format("select * from {0}", extentRequeridos));
                        listKeyValue.Add(keyValuePair);
                    }
                    if (tabela.Equals(extentPush, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        keyValuePair = new KeyValuePair<string, string>(extentPush, string.Format("select * from {0}", extentPush));
                        listKeyValue.Add(keyValuePair);
                    }
                    if (tabela.Equals(extentVocabularioControlado, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        keyValuePair = new KeyValuePair<string, string>(extentVocabularioControlado, string.Format("select * from {0}", extentVocabularioControlado));
                        listKeyValue.Add(keyValuePair);
                    }
                    if (tabela.Equals(extentTiposDeRelacao, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        keyValuePair = new KeyValuePair<string, string>(extentTiposDeRelacao, string.Format("select * from {0}", extentTiposDeRelacao));
                        listKeyValue.Add(keyValuePair);
                    }
                    if (tabela.Equals(extentNormas, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        var ids = new Exportador_LB_to_ES.AD.AD.AD().BuscarTodosOsIds(extentNormas, "Id");
                        var id_min = "";
                        var id_max = "";
                        var i_aux = 0;
                        for (int i = 0; i < ids.Count; i++)
                        {
                            if (i_aux == 0)
                            {
                                id_min = ids[i];
                                i_aux++;
                            }
                            else if (i_aux >= iQuantidadeDeNormasPorProcesso)
                            {
                                id_max = ids[i];
                                keyValuePair = new KeyValuePair<string, string>(extentNormas, string.Format("select * from {0} where id >= {1} and id <= {2}", extentNormas, id_min, id_max));
                                listKeyValue.Add(keyValuePair);
                                i_aux = 0;
                            }
                            else if(i + 1 == ids.Count){
                                keyValuePair = new KeyValuePair<string, string>(extentNormas, string.Format("select * from {0} where id >= {1}", extentNormas, id_min));
                                listKeyValue.Add(keyValuePair);
                            }
                            else
                            {
                                i_aux++;
                            }
                        }
                        //int i = 0;
                        //while(i <= iQuantidadeDeNormasParaDividirProcesso)
                        //{
                        //    keyValuePair = new KeyValuePair<string, string>(extentNormas, string.Format("select * from {0} where {1}", extentNormas, (i < iQuantidadeDeNormasPorProcesso ? "id <= " + iQuantidadeDeNormasPorProcesso : (i >= iQuantidadeDeNormasParaDividirProcesso ? "id > " + iQuantidadeDeNormasParaDividirProcesso : "id > " + i + " and id <= " + (i + iQuantidadeDeNormasPorProcesso)))));
                        //    listKeyValue.Add(keyValuePair);
                        //    i += iQuantidadeDeNormasPorProcesso;
                        //}
                    }
                    if (tabela.Equals(extentDodfs, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        var ids = new Exportador_LB_to_ES.AD.AD.AD().BuscarTodosOsIds(extentDodfs, "Id");
                        var id_min = "";
                        var id_max = "";
                        var i_aux = 0;
                        for (int i = 0; i < ids.Count; i++)
                        {
                            if (i_aux == 0)
                            {
                                id_min = ids[i];
                                i_aux++;
                            }
                            else if (i_aux >= iQuantidadeDeDodfsPorProcesso)
                            {
                                id_max = ids[i];
                                keyValuePair = new KeyValuePair<string, string>(extentDodfs, string.Format("select * from {0} where id >= {1} and id <= {2}", extentDodfs, id_min, id_max));
                                listKeyValue.Add(keyValuePair);
                                i_aux = 0;
                            }
                            else if (i + 1 == ids.Count)
                            {
                                keyValuePair = new KeyValuePair<string, string>(extentDodfs, string.Format("select * from {0} where id >= {1}", extentDodfs, id_min));
                                listKeyValue.Add(keyValuePair);
                            }
                            else
                            {
                                i_aux++;
                            }
                        }
                        //int i = 0;
                        //while (i <= iQuantidadeDeDodfsParaDividirProcesso)
                        //{
                        //    keyValuePair = new KeyValuePair<string, string>(extentDodfs, string.Format("select * from {0} where {1}", extentDodfs, (i < iQuantidadeDeDodfsPorProcesso ? "id <= " + iQuantidadeDeDodfsPorProcesso : (i >= iQuantidadeDeDodfsParaDividirProcesso ? "id > " + i : "id > " + i + " and id <= " + (i + iQuantidadeDeDodfsPorProcesso)))));
                        //    listKeyValue.Add(keyValuePair);
                        //    i += iQuantidadeDeDodfsPorProcesso;
                        //}
                    }
                    break;
                case "files":
                    if (tabela.Equals(extentNormas, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        var ids = new Exportador_LB_to_ES.AD.AD.AD().BuscarTodosOsIds(extentNormas, "Id");
                        var id_min = "";
                        var id_max = "";
                        var i_aux = 0;
                        for (int i = 0; i < ids.Count; i++)
                        {
                            if (i_aux == 0)
                            {
                                id_min = ids[i];
                                i_aux++;
                            }
                            else if (i_aux >= iQuantidadeDeNormasPorProcesso)
                            {
                                id_max = ids[i];
                                keyValuePair = new KeyValuePair<string, string>("ArquivosNormas", string.Format("select * from {0} where id >= {1} and id <= {2}", extentNormas, id_min, id_max));
                                listKeyValue.Add(keyValuePair);
                                i_aux = 0;
                            }
                            else if (i + 1 == ids.Count)
                            {
                                keyValuePair = new KeyValuePair<string, string>("ArquivosNormas", string.Format("select * from {0} where id >= {1}", extentNormas, id_min));
                                listKeyValue.Add(keyValuePair);
                            }
                            else
                            {
                                i_aux++;
                            }
                        }
                        //int i = 0;
                        //while (i <= iQuantidadeDeNormasParaDividirProcesso)
                        //{
                        //    keyValuePair = new KeyValuePair<string, string>("ArquivosNormas", string.Format("select * from {0} where {1}", extentNormas, (i < iQuantidadeDeNormasPorProcesso ? "id <= " + iQuantidadeDeNormasPorProcesso : (i >= iQuantidadeDeNormasParaDividirProcesso ? "id > " + iQuantidadeDeNormasParaDividirProcesso : "id > " + i + " and id <= " + (i + iQuantidadeDeNormasPorProcesso)))));
                        //    listKeyValue.Add(keyValuePair);
                        //    i += iQuantidadeDeNormasPorProcesso;
                        //}
                    }
                    if (tabela.Equals(extentDodfs, StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(tabela))
                    {
                        var ids = new Exportador_LB_to_ES.AD.AD.AD().BuscarTodosOsIds(extentDodfs, "Id");
                        var id_min = "";
                        var id_max = "";
                        var i_aux = 0;
                        for (int i = 0; i < ids.Count; i++)
                        {
                            if (i_aux == 0)
                            {
                                id_min = ids[i];
                                i_aux++;
                            }
                            else if (i_aux >= iQuantidadeDeDodfsPorProcesso)
                            {
                                id_max = ids[i];
                                keyValuePair = new KeyValuePair<string, string>("ArquivosDodfs", string.Format("select * from {0} where id >= {1} and id <= {2}", extentDodfs, id_min, id_max));
                                listKeyValue.Add(keyValuePair);
                                i_aux = 0;
                            }
                            else if (i + 1 == ids.Count)
                            {
                                keyValuePair = new KeyValuePair<string, string>("ArquivosDodfs", string.Format("select * from {0} where id >= {1}", extentDodfs, id_min));
                                listKeyValue.Add(keyValuePair);
                            }
                            else
                            {
                                i_aux++;
                            }
                        }
                        //int i = 0;
                        //while (i <= iQuantidadeDeDodfsParaDividirProcesso)
                        //{
                        //    keyValuePair = new KeyValuePair<string, string>("ArquivosDodfs", string.Format("select * from {0} where {1}", extentDodfs, (i < iQuantidadeDeDodfsPorProcesso ? "id <= " + iQuantidadeDeDodfsPorProcesso : (i >= iQuantidadeDeDodfsParaDividirProcesso ? "id > " + i : "id > " + i + " and id <= " + (i + iQuantidadeDeDodfsPorProcesso)))));
                        //    listKeyValue.Add(keyValuePair);
                        //    i += iQuantidadeDeDodfsPorProcesso;
                        //}
                    }
                    break;
            }

            return listKeyValue;
        }

        public void StartProcesses(List<KeyValuePair<string, string>> listKeyValue, string exportarArquivos)
        {
            List<Process> processos = new List<Process>();
            int i = 0;
            string sNumeroDeProcessos = Configuracao.LerValorChave("QuantidadeDeProcessos"); //Identifica a quantidade máxima de processos que podem ser executador de uma vez.
            int iNumeroDeProcessos = Convert.ToInt32(sNumeroDeProcessos);
            while(i < listKeyValue.Count)
            {
                if (processos.Count < iNumeroDeProcessos) //Se a quantidade de processos em execução for menor que a quantidade máxima eu crio mais um
                {
                    var processo = ExecuteProcess.ExecuteProcesses(listKeyValue[i].Key, listKeyValue[i].Value, exportarArquivos);
                    processos.Add(processo); //Mantenho o processo em uma lista para ter controle sobre quantos processo está em execução
                    i++;
                }
                foreach (var processo in processos) //Para cada processo verifico se já foi finalizado, se sim eu removo da lista para que o próximo processo possa ser executado.
                {
                    if(processo.HasExited)
                    {
                        processos.Remove(processo);
                        break;
                    }
                }
            }
        }

        public void StartMapping()
        {
            ExecuteProcess.ExecuteProcesses("mapping");
        }

        public void StartDelete()
        {
            ExecuteProcess.ExecuteProcesses("delete");
        }
    }
}
