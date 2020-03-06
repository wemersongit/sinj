﻿using System;

namespace Exportador_LB_to_ES.AD.Models
{
    [Serializable]
    public class Requerido
    {
        public int? Id { get; set; }
        public string Nome { get; set; }

        public string Texto
        {
            get { return string.Format("{0} ()", Nome); }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Requerido))
            {
                return false;
            }
            return Equals(((Requerido)obj).Id, Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #region Implementation of IValidavel

        public void Valida()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                throw new Exception("Nome é obrigatório");
            }
        }

        #endregion
    }
}
