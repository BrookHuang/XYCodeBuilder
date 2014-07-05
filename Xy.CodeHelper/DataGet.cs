using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.CodeHelper {
    public class DataGet {
        public enum ContentType {
            TabelInfo
            , ProcedureInfo
            , ProcedureContent
            , AllProcedure
            , AllTable
            , PrimaryKey
        }

        private static Dictionary<string, System.Data.DataTable> _collection;
        private static System.Data.SqlClient.SqlConnection _con;

        public static void SetConnection(System.Data.SqlClient.SqlConnection con) {
            _con = con;
        }

        public static System.Data.DataTable Get(ContentType inType) {
            return Get(inType, string.Empty);
        }

        public static System.Data.DataTable Get(ContentType inType, string inKey) {
            string _key = string.Format("{0}_{1}", inType.ToString(), inKey);
            if (_collection == null) _collection = new Dictionary<string, System.Data.DataTable>();
            if (_collection.ContainsKey(_key)) return _collection[_key];
            System.Data.DataTable _newDT = new System.Data.DataTable();
            System.Data.SqlClient.SqlCommand _cmd = new System.Data.SqlClient.SqlCommand();
            _cmd.Connection = _con;
            _cmd.CommandType = System.Data.CommandType.Text;
            switch (inType) {
                case ContentType.AllProcedure:
                    _cmd.CommandText = @"SELECT 
                                        [name] 
                                        FROM sys.all_objects 
                                        WHERE ([type] = 'P' OR [type] = 'PC' OR [type] = 'X') and [is_ms_shipped] = 0 
                                        ORDER BY [name]";
                    break;
                case ContentType.TabelInfo:
                    _cmd.CommandText = @"Select 
                                        col.[name] as 'Name',col.[length] as 'Length',col.[prec] as 'Precision',col.[scale] as 'Scale',type.[name] as 'Type',pro.value as 'Description'  
                                        From syscolumns as col  
                                        Left Join systypes as type on col.xtype = type.xtype  
                                        Left Join sys.extended_properties as pro on col.id = pro.major_id and col.colid = pro.minor_id
                                        where col.id = (Select id From Sysobjects Where [name] = @Name) and type.[status] = 0";
                    _cmd.Parameters.AddWithValue("Name", inKey);
                    break;
                case ContentType.ProcedureInfo:
                    _cmd.CommandText = @"Select 
                                        a.name AS p_name,b.name AS p_type,a.length AS p_length,a.isoutparam AS p_isout  
                                        FROM syscolumns a, systypes b 
                                        WHERE a.xtype=b.xtype 
                                        AND b.name<>'sysname' 
                                        AND id = (select id from sysobjects where name = @Name)
                                        ORDER BY [colid]";
                    _cmd.Parameters.AddWithValue("Name", inKey);
                    break;
                case ContentType.ProcedureContent:
                    _cmd.CommandText = @"SELECT text 
                                        FROM syscomments 
                                        WHERE id = ( SELECT id FROM sysobjects WHERE name = @Name)";
                    _cmd.Parameters.AddWithValue("Name", inKey);
                    break;
                case ContentType.AllTable:
                    _cmd.CommandText = @"SELECT name FROM sysobjects WHERE xtype = 'U' AND OBJECTPROPERTY (id, 'IsMSShipped') = 0 order by name";
                    break;
                case ContentType.PrimaryKey:
                    _cmd.CommandText = @"SELECT TABLE_NAME,COLUMN_NAME as 'Name' from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where TABLE_NAME = @Name";
                    _cmd.Parameters.AddWithValue("Name", inKey);
                    break;
            }
            System.Data.SqlClient.SqlDataAdapter _sdr = new System.Data.SqlClient.SqlDataAdapter(_cmd);
            _sdr.Fill(_newDT);
            _collection.Add(_key, _newDT);
            return _newDT;
        }

        public static void Refresh() {
            _collection.Clear();
        }

    }
}
