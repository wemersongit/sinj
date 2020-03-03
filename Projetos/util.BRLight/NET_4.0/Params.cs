using System;
using System.Collections.Generic;
using System.Linq;

namespace util.BRLight
{
    public static class Params
    {
        /// <summary>
        /// 
        /// Verifica se o parâmetro é nulo ou vazio
        /// </summary>
        /// <param name="nome">nome do parâmetro</param>
        /// <param name="target">valor</param>
        public static void CheckNotNullOrEmpty(string nome, string target)
        {
            if (string.IsNullOrEmpty(target))
                throw new ParametroInvalidoException(nome);
        }

        /// <summary>
        /// 
        /// Verifica se o parâmetro é nulo ou vazio
        /// </summary>
        /// <param name="nome">nome do parâmetro</param>
        /// <param name="target">valor</param>
        public static void CheckCharIsMin(string nome, char target)
        {
            if (char.MinValue == target)
                throw new ParametroInvalidoException(nome);
        }

        /// <summary>
        /// 
        /// Verifica se o parâmetro é igual a zero
        /// </summary>
        /// <param name="nome">nome do parametro</param>
        /// <param name="target">valor do parametro</param>
        public static void CheckNotZero(string nome, int target)
        {
            if (target == 0)
                throw new ParametroInvalidoException(nome);
        }

        /// <summary>
        /// 
        /// Verifica se o parâmetro é igual a null
        /// </summary>
        /// <param name="nome">nome do parametro</param>
        /// <param name="target">valor do parametro</param>
        public static void CheckNotNull(string nome, object target)
        {
            if (target == null)
                throw new ParametroInvalidoException(nome);

        }

        public static void CheckIsNullOrEmpty(string nome, string target)
        {
            if (!string.IsNullOrEmpty(target))
                throw new ParametroInvalidoException(nome);
        }

        /// <summary>
        /// 
        /// Verifica se o parâmetro é igual a null
        /// </summary>
        /// <param name="nome">nome do parametro</param>
        /// <param name="target">valor do parametro</param>
        public static void CheckIsNull(string nome, object target) 
        {
            if(target != null)
                throw new ParametroInvalidoException(nome);
                
        }

        /// <summary>
        /// 
        /// Verifica se o parâmetro é igual a zero
        /// </summary>
        /// <param name="nome">nome do parametro</param>
        /// <param name="target">valor do parametro</param>
        public static void CheckNotZero(string nome, ulong target)
        {
            if (target == 0)
                throw new ParametroInvalidoException(nome);
        }

        /// <summary>
        /// 
        /// Verifica se o parâmetro é igual a DateTime.MinValue
        /// </summary>
        /// <param name="nome">nome do parametro</param>
        /// <param name="target">valor do parametro</param>
        public static void CheckIsDateMin(string nome, DateTime target)
        {
            if (target == DateTime.MinValue)
                throw new ParametroInvalidoException(nome);

        }

        /// <summary>
        /// Valida se o número de dias entre a subtração de targetOp1 por targetOp2 é maior que zero
        /// </summary>
        /// <param name="nome">nome do parametro que aparecerá na exceção</param>
        /// <param name="targetOp1">Data que deve maior que targetOp2 para ser válida</param>
        /// <param name="targetOp2">Data que deve menor que targetOp1 para ser válida</param>
        public static void CheckIsDateHigher(string nome, DateTime targetOp1, DateTime targetOp2)
        {
            TimeSpan timeSpan = targetOp1 - targetOp2;
            if(timeSpan.Days > 0)
                throw new ParametroInvalidoException(nome);
        }

        /// <summary>
        /// 
        /// Verifica se o Array é vazio
        /// </summary>
        /// <param name="nome">nome do parametro</param>
        /// <param name="target">valor do parametro</param>
        public static void CheckIsEmptyByteArray(string nome, byte[] target)
        {
            if (target.Length <= 0)
                throw new ParametroInvalidoException(nome);
        }

        /// <summary>
        /// 
        /// Verifica se o Array é vazio
        /// </summary>
        /// <param name="nome">nome do parametro</param>
        /// <param name="target">valor do parametro</param>
        public static void CheckIsEmptyArray(string nome, object[] target)
        {
            if (target.Length <= 0)
                throw new ParametroInvalidoException(nome);
        }

        /// <summary>
        /// 
        /// Verifica se o parâmetro é igual a zero ou null
        /// </summary>
        /// <param name="nome">nome do parametro</param>
        /// <param name="target">valor do parametro</param>
        public static void CheckNotZeroOrNull(string nome, UInt64? target) {
            if (target == 0 || target == null)
                throw new ParametroInvalidoException(nome);
        }

        /// <summary>
        /// 
        /// Verifica se existe um parametro com o nome no Dictionary
        /// </summary>
        /// <param name="nome">nome do parametro</param>
        /// <param name="Parameters"> Dictionary de parametro</param>
        public static void CheckExistKeyInDictionary(string nome, Dictionary<string, object> Parameters)
        {
            if (!Parameters.Any(param => param.Key.ToUpper() == nome.ToUpper()))
                throw new ParametroInvalidoException(nome);
        }

        /// <summary>
        /// 
        /// Verifica se existe um parametro do tipo FileParameter no Dictionary
        /// </summary>
        /// <param name="nome">nome do parametro</param>
        /// <param name="Parameters"> Dictionary de parametro</param>
        public static void CheckExistValueFileParameter(string nome, Dictionary<string, object> Parameters)
        {
            if (!Parameters.Any(param2 => param2.Value is FileParameter))
                throw new ParametroInvalidoException(nome);
        }
    }
}