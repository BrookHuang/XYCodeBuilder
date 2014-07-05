using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.CodeEntity {
    public class Table {
        private List<Field> _fieldList = new List<Field>();
        public List<Field> FieldList { get { return _fieldList; } }

        private List<Procedure> _procedureList = new List<Procedure>();
        public List<Procedure> ProcedureList { get { return _procedureList; } }

        private string _name;
        public string Name { get { return _name; } }

        private int _pkIndex = -1;
        public Field PK {
            get {
                if (_pkIndex < 0) {
                    _pkIndex = 0;
                    for (int i = 0; i < _fieldList.Count; i++) {
                        if (_fieldList[i].IsPK) _pkIndex = i;
                    }
                }
                return _fieldList[_pkIndex];
            }
        }

        public Table(Xy.CodeHelper.FileHandle File, string TableName) {
            _name = TableName;
            System.Data.DataTable _fields = CodeHelper.DataGet.Get(CodeHelper.DataGet.ContentType.TabelInfo, TableName);
            System.Data.DataTable _PK = CodeHelper.DataGet.Get(CodeHelper.DataGet.ContentType.PrimaryKey, TableName);
            string _pkName = string.Empty;
            if (_PK.Rows.Count > 0) _pkName = _PK.Rows[0]["Name"].ToString();
            foreach (System.Data.DataRow _row in _fields.Rows) {
                _fieldList.Add(new Field(_row, (string.Compare(_row["Name"].ToString(), _pkName) == 0 ? true : false)));
            }
            System.Data.DataTable _procedures = CodeHelper.DataGet.Get(CodeHelper.DataGet.ContentType.AllProcedure);
            string _nameStart = (File.Namespace + "." + TableName + ".").Replace('.', '_');
            foreach (System.Data.DataRow _row in _procedures.Rows) {
                string _procedureName = _row["name"].ToString();
                if (_procedureName.IndexOf(_nameStart) == 0 && _procedureName.IndexOf('_',_nameStart.Length) == -1) {
                    _procedureList.Add(new Procedure(_procedureName, this));
                }
            }
        }


    }
}
