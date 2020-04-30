﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace Exportador_LB_to_ES.AD.Models
{
    public class VideEntreNormas : IComparable<VideEntreNormas>, IEqualityComparer<VideEntreNormas>
    {
        public int idDaNorma { get; set; }

        /// <summary>
        /// Esse metodo serve apenas para montar a string de identificação. E apenas é usado no relatorio.
        /// Ele usa a fabrica de acesso a dados, mas esse codigo deve ir para outro canto. =/
        /// </summary>
        public string IdentificacaoParaRelatorio
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                if (TipoDeNorma != null) builder.Append(TipoDeNorma.Nome);
                else builder.Append("Tipo de norma não preenchido");

                builder.Append(" ");

                builder.Append(NumeroDaNormaPosterior);

                builder.Append(" - ");

                builder.Append(!string.IsNullOrEmpty(DataDaNormaPosterior) ? Convert.ToDateTime(DataDaNormaPosterior).ToString("dd/MM/yyyy") : "");

                return builder.ToString();
            }
        }

        /// <summary>
        /// Funcionalidade usada nos relatórios.
        /// </summary>
        public string DadosDessaNorma
        {
            get
            {
                StringBuilder strBuilder = new StringBuilder();

                if (!VideAlterador)
                {
                    #region Preenche dados da norma anterior como os dados da norma que deve ser exibida

                    if (!string.IsNullOrEmpty(ArtigoDaNormaAnterior) && !ArtigoDaNormaAnterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("Art.{0}, ", ArtigoDaNormaAnterior);
                    }

                    if (!string.IsNullOrEmpty(ParagrafoDaNormaAnterior) && !ParagrafoDaNormaAnterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("Par.{0}, ", ParagrafoDaNormaAnterior);
                    }

                    if (!string.IsNullOrEmpty(AnexoDaNormaAnterior) && !AnexoDaNormaAnterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("Anexo {0}, ", AnexoDaNormaAnterior);
                    }

                    if (!string.IsNullOrEmpty(ItemDaNormaAnterior) && !ItemDaNormaAnterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("Item {0}, ", ItemDaNormaAnterior);
                    }

                    if (!string.IsNullOrEmpty(IncisoDaNormaAnterior) && !IncisoDaNormaAnterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("{0}, ", IncisoDaNormaAnterior);
                    }

                    if (!string.IsNullOrEmpty(AlineaDaNormaAnterior) && !AlineaDaNormaAnterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("{0}, ", AlineaDaNormaAnterior);
                    }

                    if (!string.IsNullOrEmpty(CaputDaNormaAnterior) && CaputDaNormaAnterior.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        strBuilder.Append("Caput. ");
                    }

                    #endregion
                }
                else
                {
                    #region Preenche dados da norma posterior como os dados da norma que deve ser exibida

                    if (!string.IsNullOrEmpty(ArtigoDaNormaPosterior) && !ArtigoDaNormaPosterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("Art.{0}, ", ArtigoDaNormaPosterior);
                    }

                    if (!string.IsNullOrEmpty(ParagrafoDaNormaPosterior) && !ParagrafoDaNormaPosterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("Par.{0}, ", ParagrafoDaNormaPosterior);
                    }

                    if (!string.IsNullOrEmpty(AnexoDaNormaPosterior) && !AnexoDaNormaPosterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("Anexo {0}, ", AnexoDaNormaPosterior);
                    }

                    if (!string.IsNullOrEmpty(ItemDaNormaPosterior) && !ItemDaNormaPosterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("Item {0}, ", ItemDaNormaPosterior);
                    }

                    if (!string.IsNullOrEmpty(IncisoDaNormaPosterior) && !IncisoDaNormaPosterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("{0}, ", IncisoDaNormaPosterior);
                    }

                    if (!string.IsNullOrEmpty(AlineaDaNormaPosterior) && !AlineaDaNormaPosterior.Equals("0"))
                    {
                        strBuilder.AppendFormat("{0}, ", AlineaDaNormaPosterior);
                    }

                    if (!string.IsNullOrEmpty(CaputDaNormaPosterior) && CaputDaNormaPosterior.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        strBuilder.Append("Caput. ");
                    }

                    #endregion
                }
                return strBuilder.Length > 0 ? strBuilder.ToString(0, strBuilder.Length - 2) :
                    strBuilder.ToString();
            }
        }

        public string DadosDaOutraNorma
        {
            get
            {
                StringBuilder strBuilder = new StringBuilder();

                if (!VideAlterador)
                {
                    #region Preenche dados da norma anterior como os dados da norma que deve ser exibida

                    if (!string.IsNullOrEmpty(ArtigoDaNormaPosterior)) if (!ArtigoDaNormaPosterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("Art.{0}, ", ArtigoDaNormaPosterior);
                        }

                    if (!string.IsNullOrEmpty(ParagrafoDaNormaPosterior)) if (!ParagrafoDaNormaPosterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("Par.{0}, ", ParagrafoDaNormaPosterior);
                        }

                    if (!string.IsNullOrEmpty(AnexoDaNormaPosterior)) if (!AnexoDaNormaPosterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("Anexo {0}, ", AnexoDaNormaPosterior);
                        }

                    if (!string.IsNullOrEmpty(ItemDaNormaPosterior)) if (!ItemDaNormaPosterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("Item {0}, ", ItemDaNormaPosterior);
                        }

                    if (!string.IsNullOrEmpty(IncisoDaNormaPosterior)) if (!IncisoDaNormaPosterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("{0}, ", IncisoDaNormaPosterior);
                        }

                    if (!string.IsNullOrEmpty(AlineaDaNormaPosterior)) if (!AlineaDaNormaPosterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("{0}, ", AlineaDaNormaPosterior);
                        }

                    if (!string.IsNullOrEmpty(CaputDaNormaPosterior)) if (CaputDaNormaPosterior.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                        {
                            strBuilder.Append("Caput. ");
                        }
                    #endregion
                }
                else
                {

                    #region Preenche dados da norma posterior como os dados da norma que deve ser exibida

                    if (!string.IsNullOrEmpty(ArtigoDaNormaAnterior)) if (!ArtigoDaNormaAnterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("Art.{0}, ", ArtigoDaNormaAnterior);
                        }

                    if (!string.IsNullOrEmpty(ParagrafoDaNormaAnterior)) if (!ParagrafoDaNormaAnterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("Par.{0}, ", ParagrafoDaNormaAnterior);
                        }

                    if (!string.IsNullOrEmpty(AnexoDaNormaAnterior)) if (!AnexoDaNormaAnterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("Anexo {0}, ", AnexoDaNormaAnterior);
                        }

                    if (!string.IsNullOrEmpty(ItemDaNormaAnterior)) if (!ItemDaNormaAnterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("Item {0}, ", ItemDaNormaAnterior);
                        }

                    if (!string.IsNullOrEmpty(IncisoDaNormaAnterior)) if (!IncisoDaNormaAnterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("{0}, ", IncisoDaNormaAnterior);
                        }

                    if (!string.IsNullOrEmpty(AlineaDaNormaAnterior)) if (!AlineaDaNormaAnterior.Equals("0"))
                        {
                            strBuilder.AppendFormat("{0}, ", AlineaDaNormaAnterior);
                        }

                    if (!string.IsNullOrEmpty(CaputDaNormaAnterior)) if (CaputDaNormaAnterior.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                        {
                            strBuilder.Append("Caput. ");
                        }

                    #endregion
                }

                return strBuilder.Length > 0 ? strBuilder.ToString(0, strBuilder.Length - 2) :
                    strBuilder.ToString();
            }
        }

        public string TipoDeVinculoParaRelatorio
        {
            get
            {
                if (TipoDeVinculo != null)
                {
                    if (VideAlterador)
                    {

                        if (string.IsNullOrEmpty(TipoDeVinculo.TextoParaAlterado))
                        {
                            return TipoDeVinculo.Descricao;
                        }

                        return TipoDeVinculo.TextoParaAlterado;
                    }


                    if (string.IsNullOrEmpty(TipoDeVinculo.TextoParaAlterador))
                    {
                        return TipoDeVinculo.Descricao;
                    }

                    return TipoDeVinculo.TextoParaAlterador;
                }
                return "O vide está sem o vinculo definido.";
            }
        }

        public VideEntreNormas()
        {
            Id = Guid.NewGuid();
            TipoDeVinculo = new TipoDeRelacaoDeVinculo();
            TipoDeNorma = new TipoDeNorma();
            TipoDeFonte = new TipoDeFonteBO();


            IdDaNormaPosterior = 0;

            DataDaNormaPosterior = "";

            TipoDeNorma = new TipoDeNorma();

            TipoDeFonte = new TipoDeFonteBO();
            DataDePublicacaoPosterior = "";
            PaginaDaPublicacaoPosterior = "";
            ColunaDaPublicacaoPosterior = "";
            TipoDeVinculo.Oid = 0;
            ComentarioVide = "";
            ArtigoDaNormaPosterior = "";
            ParagrafoDaNormaPosterior = "";
            IncisoDaNormaPosterior = "";
            AlineaDaNormaPosterior = "";
            ItemDaNormaPosterior = "";
            CaputDaNormaPosterior = "";
            AnexoDaNormaPosterior = "";
        }



        public VideEntreNormas(DataRow row)
        {
            Id = new Guid(Convert.ToString(row.ItemArray[0]));
            IdDaNormaPosterior = Convert.ToInt32(row.ItemArray[1]);
            DataDaNormaPosterior = Convert.ToDateTime(row.ItemArray[2]).ToString(DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern);
            TipoDeNorma = new TipoDeNorma { Id = Convert.ToInt32(row.ItemArray[3]) };
            NumeroDaNormaPosterior = row.ItemArray[4] as string;
            TipoDeFonte = new TipoDeFonteBO(Convert.ToString(row.ItemArray[5]));

            if (!string.IsNullOrEmpty(row.ItemArray[6] as string))
                DataDePublicacaoPosterior = Convert.ToDateTime(row.ItemArray[6]).ToString(DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern);

            PaginaDaPublicacaoPosterior = row.ItemArray[7] as string;
            ColunaDaPublicacaoPosterior = row.ItemArray[8] as string;

            if (!string.IsNullOrEmpty(row.ItemArray[9] as string))
                TipoDeVinculo = new TipoDeRelacaoDeVinculo { Oid = Convert.ToInt32(row.ItemArray[9]) };

            ComentarioVide = row.ItemArray[10] as string;
            ArtigoDaNormaPosterior = row.ItemArray[11] as string;
            ParagrafoDaNormaPosterior = row.ItemArray[12] as string;
            IncisoDaNormaPosterior = row.ItemArray[13] as string;
            AlineaDaNormaPosterior = row.ItemArray[14] as string;
            ItemDaNormaPosterior = row.ItemArray[15] as string;
            CaputDaNormaPosterior = row.ItemArray[16] as string;
            AnexoDaNormaPosterior = row.ItemArray[17] as string;
        }
        public string TipoDeNormaParaRelatorio
        {
            get
            {
                return TipoDeNorma != null ? TipoDeNorma.Nome + " " + NumeroDaNormaPosterior + " /" + (!string.IsNullOrEmpty(DataDaNormaPosterior) ? Convert.ToDateTime(DataDaNormaPosterior).Year.ToString() : "") : "Tipo De Norma Nulo";
            }
        }
        #region Atributos do Vide

        public Guid Id { get; set; }
        public int IdDaNormaPosterior { get; set; }
        public string NumeroDaNormaPosterior { get; set; }
        public string ComentarioVide { get; set; }
        public string PaginaDaPublicacaoPosterior { get; set; }
        public string ColunaDaPublicacaoPosterior { get; set; }
        public string DataDaNormaPosterior { get; set; }
        public string DataDePublicacaoPosterior { get; set; }

        #endregion

        #region Informações usadas apenas se a norma alteradora não for cadastrada no sistema

        /// <summary>
        /// Identifica a denominação da norma com relação de vinculo.
        /// </summary>
        public TipoDeNorma TipoDeNorma { get; set; }

        /// <summary>
        /// Identifica a fonte publicadora da norma que tem a relação de vinculo.
        /// </summary>
        public TipoDeFonteBO TipoDeFonte { get; set; }

        #endregion

        /// <summary>
        /// Contem qual o tipo de relação de vinculo existe entre a norma anterior e a outra posterior.
        /// </summary>
        public TipoDeRelacaoDeVinculo TipoDeVinculo { get; set; }

        #region Dados da Norma Posterior

        /// <summary>
        /// Contem o numero do artigo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ArtigoDaNormaPosterior { get; set; }

        /// <summary>
        /// Contem o numero do parágrafo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ParagrafoDaNormaPosterior { get; set; }

        /// <summary>
        /// Contem o numero do Inciso que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string IncisoDaNormaPosterior { get; set; }

        /// <summary>
        /// Contem o numero da Alínea que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string AlineaDaNormaPosterior { get; set; }

        /// <summary>
        /// Contem o numero do Item que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ItemDaNormaPosterior { get; set; }

        /// <summary>
        /// Define que é o CAPUT que tem uma ralação de vinculo com outra norma anterior.
        /// </summary>
        public string CaputDaNormaPosterior { get; set; }

        /// <summary>
        ///	Contem a identificação do Anexo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string AnexoDaNormaPosterior { get; set; }

        /// <summary>
        /// Parametro auxiliar para comparar os vides.
        /// </summary>
        public bool VideAlterador { get; set; }

        #endregion

        #region Dados da Norma Anterior

        /// <summary>
        /// Contem o numero do artigo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ArtigoDaNormaAnterior { get; set; }

        /// <summary>
        /// Contem o numero do parágrafo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ParagrafoDaNormaAnterior { get; set; }

        /// <summary>
        /// Contem o numero do Inciso que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string IncisoDaNormaAnterior { get; set; }

        /// <summary>
        /// Contem o numero da Alínea que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string AlineaDaNormaAnterior { get; set; }

        /// <summary>
        /// Contem o numero do Item que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ItemDaNormaAnterior { get; set; }

        /// <summary>
        /// Define que é o CAPUT que tem uma ralação de vinculo com outra norma anterior.
        /// </summary>
        public string CaputDaNormaAnterior { get; set; }

        /// <summary>
        ///	Contem a identificação do Anexo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string AnexoDaNormaAnterior { get; set; }

        /// <summary>
        /// Retorna/Define a chave da norma posterior (<see cref="Norma.ChaveParaNaoDuplicacao"/>)
        /// </summary>
        public string ChaveDaNormaPosterior
        {
            get;
            set;
        }

        #endregion


        #region Implementation of IValidavel

        public void Valida()
        {

            if (!string.IsNullOrEmpty(CaputDaNormaPosterior))
            {
                if (string.IsNullOrEmpty(ArtigoDaNormaPosterior) && string.IsNullOrEmpty(ParagrafoDaNormaPosterior))
                    throw new Exception("Quando o Caput é preenchido, é obrigatório preencher Artigo ou Parágrafo.");


                if (!string.IsNullOrEmpty(IncisoDaNormaPosterior) || !string.IsNullOrEmpty(AlineaDaNormaPosterior) ||
                    !string.IsNullOrEmpty(AnexoDaNormaPosterior) || !string.IsNullOrEmpty(ItemDaNormaPosterior))
                    throw new Exception(
                        "Quando o Caput é preenchido Inciso, Anexo, Alinea e Item não podem ser preenchidos.");
            }

            if (!string.IsNullOrEmpty(IncisoDaNormaPosterior))
            {
                if (string.IsNullOrEmpty(ArtigoDaNormaPosterior) && string.IsNullOrEmpty(ParagrafoDaNormaPosterior))
                    throw new Exception("Quando o Inciso é peenchido, é obrigatório preencher Artigo ou Parágrafo.");
            }

            if (!string.IsNullOrEmpty(ParagrafoDaNormaPosterior))
            {
                if (string.IsNullOrEmpty(ArtigoDaNormaPosterior))
                    throw new Exception("Quando o Parágrafo é preenchido, é obrigatório preencher Artigo.");
            }

            if (!string.IsNullOrEmpty(AlineaDaNormaPosterior))
            {
                if (string.IsNullOrEmpty(IncisoDaNormaPosterior) || string.IsNullOrEmpty(ArtigoDaNormaPosterior) || string.IsNullOrEmpty(ParagrafoDaNormaPosterior))
                    throw new Exception("Quando a Alínea é preenchida, é obrigatório preencher Inciso, Artigo e Parágrafo.");
            }
        }

        #endregion

        public int CompareTo(VideEntreNormas other)
        {
            return DataDaNormaPosterior.CompareTo(other.DataDaNormaPosterior);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(VideEntreNormas)) return false;
            return Equals((VideEntreNormas)obj);
        }

        public bool Equals(VideEntreNormas obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.IdDaNormaPosterior == IdDaNormaPosterior;
        }

        #region IEqualityComparer<VideEntreNormas> Members

        /// <summary>
        /// Confere igualdade usando o ID do objeto.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(VideEntreNormas x, VideEntreNormas y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(VideEntreNormas obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
