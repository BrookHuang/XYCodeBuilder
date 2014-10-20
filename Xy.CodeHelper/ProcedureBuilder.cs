using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.CodeHelper {
    public class ProcedureBuilder {

        string _namespace = string.Empty;

        public ProcedureBuilder(string inNamesapce) {
            _namespace = inNamesapce;
        }

        public void CreateProcedure(string tableName, System.Data.SqlClient.SqlConnection connection) {
            System.Data.DataTable _fieldRows = CodeHelper.DataGet.Get(CodeHelper.DataGet.ContentType.TabelInfo, tableName);
            System.Data.DataTable _PK = CodeHelper.DataGet.Get(CodeHelper.DataGet.ContentType.PrimaryKey, tableName);
            string _pkName = _PK.Rows.Count > 0 ? _PK.Rows[0]["Name"].ToString() : string.Empty;
            if (string.IsNullOrEmpty(_pkName)) throw new Exception("missing primary key");

            Xy.CodeEntity.Field _pkField = null;
            Xy.CodeEntity.Field[] _fields = new CodeEntity.Field[_fieldRows.Rows.Count];
            for (int i = 0; i < _fieldRows.Rows.Count; i++) {
                System.Data.DataRow _row = _fieldRows.Rows[i];
                bool _isPK = string.Compare(_row["Name"].ToString(), _pkName) == 0;
                _fields[i] = new CodeEntity.Field(_row, _isPK);
                if (_isPK) _pkField = _fields[i];
            }
            StringBuilder _cmd = new StringBuilder();

            string _name_base = _namespace.Replace('.', '_') + '_' + tableName + "_";

            #region add procedure
            string _name_add = _name_base + "Add";
            _cmd.AppendLine("IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[" + _name_add + "]') AND type in (N'P', N'PC'))");
            _cmd.AppendLine("BEGIN");
            _cmd.AppendLine("EXEC dbo.sp_executesql @statement = N'CREATE proc [" + _name_add + "](");
            for (int i = 0; i < _fields.Length; i++) {
                Xy.CodeEntity.Field _item = _fields[i];
                if (!_item.IsPK) {
                    _cmd.AppendLine("\t@" + _item.Name + " " + _item.GetSQLDecalre() + (i < _fields.Length - 1 ? "," : string.Empty));
                }
            }
            _cmd.AppendLine(") as");
            _cmd.AppendLine("BEGIN");
            _cmd.AppendLine("Insert into [" + tableName + "](");
            _cmd.Append("\t");
            for (int i = 0; i < _fields.Length; i++) {
                Xy.CodeEntity.Field _item = _fields[i];
                if (!_item.IsPK) {
                    _cmd.Append("[" + _item.Name + "]" + (i < _fields.Length - 1 ? "," : Environment.NewLine));
                }
            }
            _cmd.AppendLine(")Values(");
            _cmd.Append("\t");
            for (int i = 0; i < _fields.Length; i++) {
                Xy.CodeEntity.Field _item = _fields[i];
                if (!_item.IsPK) {
                    _cmd.Append("@" + _item.Name + "" + (i < _fields.Length - 1 ? "," : Environment.NewLine));
                }
            }
            _cmd.AppendLine(")");
            _cmd.AppendLine("return SCOPE_IDENTITY()");
            _cmd.AppendLine("End'");
            _cmd.AppendLine("End");
            #endregion

            #region edit procedure
            string _name_edit = _name_base + "Edit";
            _cmd.AppendLine("IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[" + _name_edit + "]') AND type in (N'P', N'PC'))");
            _cmd.AppendLine("BEGIN");
            _cmd.AppendLine("EXEC dbo.sp_executesql @statement = N'CREATE proc [" + _name_edit + "](");
            for (int i = 0; i < _fields.Length; i++) {
                Xy.CodeEntity.Field _item = _fields[i];
                _cmd.AppendLine("\t@" + _item.Name + " " + _item.GetSQLDecalre() + (i < _fields.Length - 1 ? "," : string.Empty));
            }
            _cmd.AppendLine(") as");
            _cmd.AppendLine("BEGIN");
            _cmd.AppendLine("Update [" + tableName + "] set");
            for (int i = 0; i < _fields.Length; i++) {
                Xy.CodeEntity.Field _item = _fields[i];
                if (!_item.IsPK) {
                    _cmd.AppendLine("\t[" + _item.Name + "] = @" + _item.Name + (i < _fields.Length - 1 ? "," : string.Empty));
                }
            }
            _cmd.AppendLine("Where");
            for (int i = 0; i < _fields.Length; i++) {
                Xy.CodeEntity.Field _item = _fields[i];
                if (_item.IsPK) {
                    _cmd.AppendLine("\t[" + _item.Name + "] = @" + _item.Name);
                }
            }
            _cmd.AppendLine("End'");
            _cmd.AppendLine("End");
            #endregion

            #region del procedure
            string _name_del = _name_base + "Del";
            _cmd.AppendLine("IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[" + _name_del + "]') AND type in (N'P', N'PC'))");
            _cmd.AppendLine("BEGIN");
            _cmd.AppendLine("EXEC dbo.sp_executesql @statement = N'CREATE proc [" + _name_del + "](");
            _cmd.AppendLine("\t@" + _pkField.Name + " " + _pkField.GetSQLDecalre());
            _cmd.AppendLine(") as");
            _cmd.AppendLine("BEGIN");
            _cmd.AppendLine("Delete from [" + tableName + "] where [" + _pkField.Name + "] = @" + _pkField.Name);
            _cmd.AppendLine("End'");
            _cmd.AppendLine("End");
            #endregion

            #region get procedure
            string _name_get = _name_base + "Get";
            _cmd.AppendLine("IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[" + _name_get + "]') AND type in (N'P', N'PC'))");
            _cmd.AppendLine("BEGIN");
            _cmd.AppendLine("EXEC dbo.sp_executesql @statement = N'CREATE proc [" + _name_get + "](");
            _cmd.AppendLine("\t@" + _pkField.Name + " " + _pkField.GetSQLDecalre());
            _cmd.AppendLine(") as");
            _cmd.AppendLine("BEGIN");
            _cmd.AppendLine("Select * from [" + tableName + "] where [" + _pkField.Name + "] = @" + _pkField.Name);
            _cmd.AppendLine("End'");
            _cmd.AppendLine("End");
            #endregion

            #region GetList procedure
            string _name_getlist = _name_base + "GetList";
            _cmd.AppendLine("IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[" + _name_getlist + "]') AND type in (N'P', N'PC'))");
            _cmd.AppendLine("BEGIN");
            _cmd.AppendLine("EXEC dbo.sp_executesql @statement = N'CREATE proc [" + _name_getlist + "](");
            _cmd.AppendLine(string.Format(
@"  @PageIndex int,
	@PageSize int,
    @Where nvarchar(2048),
	@TotalCount int out
)
as
BEGIN
	DECLARE @Count INT
	EXEC	@Count= [SplitPage] 
			@Select=''*'',
			@TableName=''[{0}]'',
			@Where=@Where,
			@Order=''[{1}] DESC'',
			@PageIndex=@PageIndex,
			@PageSize=@PageSize,
			@Orderby=''[{1}] DESC''
	set @TotalCount = @Count
	RETURN(@TotalCount)
END
", tableName, _pkField.Name));
            _cmd.AppendLine("'");
            _cmd.AppendLine("End");
            #endregion

            System.Data.SqlClient.SqlCommand _com = new System.Data.SqlClient.SqlCommand(_cmd.ToString(), connection);
            try {
                connection.Open();
                _com.ExecuteNonQuery();
            } catch {
                throw;
            } finally {
                connection.Close();
            }
        }
    }
}
