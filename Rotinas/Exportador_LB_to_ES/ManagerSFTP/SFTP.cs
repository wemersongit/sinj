using System;
using BRLight.Logger;
using Tamir.SharpSsh;
using Tamir.SharpSsh.jsch;

namespace ManagerSFTP
{
    public class SFTP
    {
        public static Sftp SftpHolder { get; set; }

        /// <summary>
        /// Retorna uma conexão ao SFTP. A conexão será única! Se já conectado, para abrir uma conexão nova é necessário primeiro fechar a anterior! By Questor
        /// </summary>
        /// <returns></returns>
        public static bool conect_SFTP_UploadFile(string ServerSFTP, string UserSFTP, string PassSFTP, string PortSFTP)
        {
            bool conect_SFTP_UploadFile = true;

            if (SftpHolder == null)
            {
                try
                {
                    SftpHolder = new Sftp(ServerSFTP, UserSFTP, PassSFTP);
                }
                catch (Exception ex)
                {
                    ManagerLog.GravaLog(LogType.Error, LogLayer.Control, UserSFTP, "", "SFTP", "", "Erro na criação do objeto SFTP.", ex);
                    conect_SFTP_UploadFile = false;
                }

                if (conect_SFTP_UploadFile)
                {
                    try
                    {
                        //SftpHolder.Connect();
                        SftpHolder.Connect(Convert.ToInt16(PortSFTP));
                    }
                    catch (Exception ex)
                    {
                        ManagerLog.GravaLogSync(LogType.Error, LogLayer.Control, UserSFTP, "", "SFTP", "", "Houve erro ao conectar o servidor SFTP", ex);
                        conect_SFTP_UploadFile = false;
                    }
                }

                if (conect_SFTP_UploadFile)
                {
                    if (!SftpHolder.Connected)
                    {
                        ManagerLog.GravaLogSync(LogType.Error, LogLayer.Control, UserSFTP, "", "SFTP", "", "Houve erro ao conectar o servidor SFTP", null);
                        conect_SFTP_UploadFile = false;
                    }
                }
            }

            return conect_SFTP_UploadFile;

        }

        /// <summary>
        /// Upload files to an SFTP server. Returns false if it fails! Call "conect_SFTP_UploadFile()" before! By Questor
        /// </summary>
        public static bool SFTP_UploadFile(String pathAndFileName, String pathOnSFTP, String fileToSaveOnSFTP)
        {
            //Note: DebugLogsSistema.EnableProcessToLog(); //For debugging! By Questor
            //Note: DebugLogsSistema.UpdateLog("reached_29", "", ""); //For debugging! By Questor
            //Note: DebugLogsSistema.DisableProcessToLog(); //For debugging! By Questor

            if (!SftpHolder.Connected)
            {
                SftpHolder.Connect();
            }

            //Note: DebugLogsSistema.EnableProcessToLog(); //For debugging! By Questor
            //Note: DebugLogsSistema.UpdateLog("reached_30", "", ""); //For debugging! By Questor
            //Note: DebugLogsSistema.DisableProcessToLog(); //For debugging! By Questor

            if (pathOnSFTP != "")
            {

                if (pathOnSFTP.Length >= 3 && pathOnSFTP.Contains("../"))
                {
                    pathOnSFTP = pathOnSFTP.Replace("../", "/");
                }

                if (pathOnSFTP.LastIndexOf('/') != pathOnSFTP.Length - 1)
                {
                    pathOnSFTP = pathOnSFTP + "/";
                }
            }
            else
            {
                pathOnSFTP = "/";
            }

            //ToDo: To testing purpose! By Questor
            //pathOnSFTP = "../";

            bool SFTP_UploadFileReturn = true;

            //Note: DebugLogsSistema.EnableProcessToLog(); //For debugging! By Questor
            //Note: DebugLogsSistema.UpdateLog("reached_31", "", ""); //For debugging! By Questor
            //Note: DebugLogsSistema.DisableProcessToLog(); //For debugging! By Questor

            try
            {
                SftpHolder.Put(pathAndFileName, pathOnSFTP + fileToSaveOnSFTP);
            }
            catch (SftpException ex)
            {
                ManagerLog.GravaLog(LogType.Error, LogLayer.Control, SftpHolder.Username, "", "SFTP", "", "Erro ao enviar arquivo para servidor SFTP. Arquivo: " + pathAndFileName, ex);
                SFTP_UploadFileReturn = false;
            }

            //Note: DebugLogsSistema.EnableProcessToLog(); //For debugging! By Questor
            //Note: DebugLogsSistema.UpdateLog("reached_32", "", ""); //For debugging! By Questor
            //Note: DebugLogsSistema.DisableProcessToLog(); //For debugging! By Questor

            return SFTP_UploadFileReturn;

        }
    }
}
