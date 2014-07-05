using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.CodeEntity {
    public enum ProcedureType {
        Add,
        Delete,
        Get,
        Update,
        Unknow
    }
    public class Procedure {
        private string _name;
        public string Name { get { return _name; } }
        private ProcedureType _type;
        public ProcedureType Type { get { return _type; } }
        private bool _hasOut = false;
        public bool HasOut { get { return _hasOut; } }

        private List<Parameter> _parameters = new List<Parameter>();
        public List<Parameter> Parameters { get { return _parameters; } }

        private Table _parentTable;

        public Procedure(string name, Table table) {
            _parentTable = table;
            _name = name.Substring(name.LastIndexOf('_') + 1);
            System.Data.DataTable _dt = CodeHelper.DataGet.Get(CodeHelper.DataGet.ContentType.ProcedureInfo, name);
            foreach (System.Data.DataRow _row in _dt.Rows) {
                Parameter _temp = new Parameter(_row);
                _parameters.Add(_temp);
                if (_temp.IsOut) _hasOut = true;
            }
            if (_name.IndexOf("Add") == 0 || _name.IndexOf("Insert") == 0 || _name.IndexOf("Create") == 0) {
                _type = ProcedureType.Add;
            } else if (_name.IndexOf("Del") == 0 || _name.IndexOf("Remove") == 0) {
                _type = ProcedureType.Delete;
            } else if (_name.IndexOf("Get") == 0 || _name.IndexOf("Search") == 0) {
                _type = ProcedureType.Get;
            } else if (_name.IndexOf("Edit") == 0 || _name.IndexOf("Update") == 0) {
                _type = ProcedureType.Update;
            } else {
                _type = ProcedureType.Unknow;
            }
            
        }

        internal string GetCSharpReturn() {
            switch (_type) {
                case ProcedureType.Add:
                    return _parentTable.PK.CSharpType;
                case ProcedureType.Delete:
                case ProcedureType.Update:
                    return "int";
                case ProcedureType.Get:
                    return "System.Data.DataTable";
            }
            return string.Empty;
        }

        internal Field GetPK() {
            return _parentTable.PK;
        }

        internal string GetTableName() {
            return _parentTable.Name;
        }

        internal string GetCSharpParameter() {
            StringBuilder _sb = new StringBuilder();
            for (int i = 0; i < _parameters.Count; i++) {
                Parameter _item = _parameters[i];
                _sb.Append((_item.IsOut ? "ref " : string.Empty) + _item.CSharpType + " in" + _item.Name + ", ");
            }
            _sb.Append("Xy.Data.DataBase DB = null");
            return _sb.ToString();
        }

        internal CodeHelper.FileBuilder.BuildFunction GetCSharpBuildFunction() {
            switch (_type) {
                case ProcedureType.Add:
                    return CodeHelper.FunctionBuilder.buildFunction_Procudure_Add;
                case ProcedureType.Delete:
                    return CodeHelper.FunctionBuilder.buildFunction_Procudure_Del;
                case ProcedureType.Get:
                    return CodeHelper.FunctionBuilder.buildFunction_Procudure_Get;
                case ProcedureType.Update:
                    return CodeHelper.FunctionBuilder.buildFunction_Procudure_Update;
            }
            return CodeHelper.FunctionBuilder.buildFunction_Empty;
        }
    }
}
