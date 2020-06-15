using System;
using System.Security.Cryptography;

namespace DigitalSignatureDemo
{
	public class DigitalSignatureHelper
	{

		public String createPrivateKeyUser(RSACryptoServiceProvider RSA)
		{
			return RSA.ToXmlString(true); 
		}

		public String createPublicKeyUser(RSACryptoServiceProvider RSA)
		{
			return RSA.ToXmlString(false); 
		}

		public byte[] CreateSignature(byte[] hash,string privateKey)
		{
			RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
			RSA.FromXmlString(privateKey);
			RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
			RSAFormatter.SetHashAlgorithm("MD5");
			return RSAFormatter.CreateSignature(hash);
		}

		public bool VerifySignature(byte[] hash,byte[] signedhash, string publicKey)
		{
			RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
			RSA.FromXmlString(publicKey);
			RSAPKCS1SignatureDeformatter RSADeformatter = new RSAPKCS1SignatureDeformatter(RSA);
			RSADeformatter.SetHashAlgorithm("MD5");
			return RSADeformatter.VerifySignature(hash, signedhash);
		}
	}
}
