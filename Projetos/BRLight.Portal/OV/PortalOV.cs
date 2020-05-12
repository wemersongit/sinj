using System.Collections.Generic;

namespace BRLight.Portal.OV
{
    public class PortalOV
    {
        public PortalOV()
        {
            portal = new List<BasePortal>();
        }
        public string nm_portal { get; set; }
        public string ds_portal { get; set; }
        public string id_user { get; set; }
        public List<BasePortal> portal { get; set; }
    }

    public class BasePortal
    {
        public BasePortal()
        {
            field = new List<Field>();
            _metadata = new Metadata();
        }
        public string url_index { get; set; }
        public string nm_base { get; set; }
        public List<Field> field { get; set; }
        public Metadata _metadata { get; set; }
        public string url_detalhes { get; set; }
        public string url_app { get; set; }
        public int nr_order { get; set; }
        public string ds_base { get; set; }
    }

    public class Metadata
    {
        public object dt_del { get; set; }
        public string dt_doc { get; set; }
        public int id_doc { get; set; }
        public object dt_idx { get; set; }
        public string dt_last_up { get; set; }
    }

    public class Field
    {
        public Field()
        {
            groups_can_view = new List<string>();
            inf_direct_search = new InfDirectSearch();
            inf_field_tabulated = new InfFieldTabulated();
            inf_listed_search = new InfListedSearch();
            inf_advanced_search = new InfAdvancedSearch();
            inf_detailed_search = new InfDetailedSearch();
        }
        public List<string> groups_can_view { get; set; }
        public string nm_field { get; set; }
        public string ds_field { get; set; }
        public string nm_type_field { get; set; }
        public InfDirectSearch inf_direct_search { get; set; }
        public InfFieldTabulated inf_field_tabulated { get; set; }
        public InfListedSearch inf_listed_search { get; set; }
        public InfAdvancedSearch inf_advanced_search { get; set; }
        public InfDetailedSearch inf_detailed_search { get; set; }
    }

    public class InfDirectSearch
    {
        public int nr_position_direct { get; set; }
        public string nm_display_direct { get; set; }
        public string script_direct { get; set; }
        public string nm_type_control_direct { get; set; }
        public bool in_display_direct { get; set; }
    }

    public class InfFieldTabulated
    {
        public string nm_field_key_tabulated { get; set; }
        public string nm_table_tabulated { get; set; }
        public string relational_key_tabulated { get; set; }
        public string data_tabulated { get; set; }
        public string nm_field_value_tabulated { get; set; }
    }

    public class InfListedSearch
    {
        public bool in_search_listed { get; set; }
        public string nm_display_listed { get; set; }
        public bool in_sortable_listed { get; set; }
        public int nr_position_listed { get; set; }
        public string script_listed { get; set; }
        public bool in_display_listed { get; set; }

    }

    public class InfAdvancedSearch
    {
        public bool in_fixed_advanced { get; set; }
        public bool in_display_advanced { get; set; }
        public string nm_type_control_advanced { get; set; }
        public string nm_display_advanced { get; set; }
        public string script_advanced { get; set; }
    }

    public class InfDetailedSearch
    {
        public string nm_display_detailed { get; set; }
        public bool in_search_detailed { get; set; }
        public bool in_display_detailed { get; set; }
        public string script_detailed { get; set; }
        public int nr_position_detailed { get; set; }
    }
}