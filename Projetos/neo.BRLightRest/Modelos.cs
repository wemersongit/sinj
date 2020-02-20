using System.Collections.Generic;

namespace neo.BRLightREST
{

    // Documento
    public class metadata {
        public metadata() {
            _metadata = new _metadata();
        }
        public _metadata _metadata { get; set; }
    }

    public class _metadata
    {
        public ulong id_doc { get; set; }
        public string dt_doc { get; set; }
        public string dt_last_up { get; set; }
        public string dt_del { get; set; }
        public string dt_idx  { get; set; }
    }

    // erro
    public class ErroOV
    {
        public ErroOV()
        {
            request = new request();
        }

        public ulong status { get; set; }
        public string error_message { get; set; }
        public request request { get; set; }
        public string type { get; set; }
    }

    public class request
    {
        public string body { get; set; }
        public string path { get; set; }
        public string method { get; set; }
        public string client_addr { get; set; }
        public string user_agent { get; set; }
    }

   // File
    public class FileOV : File
    {
        public byte[] file { get; set; }
    }

    public class ArquivoOV
    {
        public string id_file { get; set; }
        public string filename { get; set; }
        public string mimetype { get; set; }
        public ulong filesize { get; set; }
        public string uuid { get; set; }
    }

    public class File
    {
        public ulong id_doc { get; set; }
        public string id_file { get; set; }
        public string filename { get; set; }
        public string mimetype { get; set; }
        public string dt_ext_text { get; set; }
        public string filetext { get; set; }
        public ulong filesize { get; set; }
        public string download { get; set; }
    }

    // pesquisa
    public class Results<T>
    {
        public Results() {
            results = new List<T>();
        }
        public ulong result_count { get; set; }
        public List<T> results { get; set; }
        public ulong offset { get; set; }
        public ulong limit { get; set; }
    }

    public class Pesquisa
    {

        public string[] select { get; set; }
        public string limit { get; set; }
        public string offset { get; set; }
        public string literal { get; set; }
        public Order_By order_by { get; set; }
        public Pesquisa() {
            limit = null;
            offset = null;
            order_by = new Order_By();
        }
    }

    public class Order_By
    {
        public string[] asc { get; set; }
        public string[] desc { get; set; }
        public Order_By() {
            asc = new string[0];
            desc = new string[0];
        }
    }

    // base
    public class _Base
    {
        public _Base() {
            content = new List<Content>();
        }
        public List<Content> content { get; set; }
        public _MetaBase metadata { get; set; }
    }

    public class _MetaGroup
    {
        public bool multivalued { get; set; }
        public string alias { get; set; }
        public string description { get; set; }
        public string name { get; set; }
    }

    public class _MetaBase
    {
        public bool file_ext { get; set; }
        public bool idx_exp { get; set; }
        public string idx_exp_url { get; set; }
        public string idx_exp_time { get; set; }
        public string file_ext_time { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string password { get; set; }
        public string color { get; set; }
    }

    public class Field
    {
        public Field() {
            indices = new string[0];
        }
        public string description { get; set; }
        public string datatype { get; set; }
        public bool required { get; set; }
        public string alias { get; set; }
        public bool multivalued { get; set; }
        public string[] indices { get; set; }
        public string name { get; set; }
    }

    public class Content
    {
        public Content() {
            field = new Field();
            group = new Group();
        }
        public Field field { get; set; }
        public Group group { get; set; }
    }

    public class Group {
        public Group() {
            content = new List<Content>();
            metadata = new _MetaGroup();
        }
        public List<Content> content { get; set; }
        public _MetaGroup metadata { get; set; }
    }

    public class opMode<T> {
        public string path { get; set; }
        public string mode { get; set; }
        public string fn { get; set; }
        public T[] args { get; set; }
    }

    public class opResult
    {
        public ulong success { get; set; }
        public ulong failure { get; set; }
    }

    public class version
    {
        public string pyramid { get; set; }
        public string voluptuous { get; set; }
        public string psycopg2 { get; set; }
        public string bcrypt { get; set; }
        public string lbgenerator { get; set; }
        public string liblightbase { get; set; }
        public string sqlalchemy { get; set; }
        public string waitress { get; set; }
        //public string pyramid-restler { get; set; }
        //public string pyramid-who { get; set; }
        public string requests { get; set; }
        public string pymongo { get; set; }
        public string alembic { get; set; }
    }

    public class TokenOV : metadata
    {
        public string ch_aplicacao { get; set; }
        public string ch_origem { get; set; }
        public string token { get; set; }
    }
}
