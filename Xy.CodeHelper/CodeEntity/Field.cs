using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.CodeEntity {
    public class Field {
        private string _name;
        public string Name { get { return _name; } }
        private string _type;
        public string Type { get { return _type; } }
        private string _cSharpType;
        public string CSharpType { get { return _cSharpType; } }
        private bool _isPK;
        public bool IsPK { get { return _isPK; } }

        private string _length;
        private string _precision;
        private string _scale;
        public Field(System.Data.DataRow row, bool isPK) {
            _name = row["Name"].ToString();
            _type = row["Type"].ToString();
            _length = row["Length"].ToString();
            _precision = row["Precision"].ToString();
            _scale = row["Scale"].ToString();
            _cSharpType = CodeHelper.CodeTransform.toCsharp(_type);
            _isPK = isPK;
        }

        public string ConvertTo(string template, string targetType) {
            if (string.IsNullOrEmpty(template)) template = _name;
            template = template.Replace("{name}", _name);
            return CodeHelper.CodeTransform.TypeConvert(CSharpType, targetType, template);
        }

        public string ConvertFrom(string template, string originalType) {
            if (string.IsNullOrEmpty(template)) template = _name;
            template = template.Replace("{name}", _name);
            return CodeHelper.CodeTransform.TypeConvert(originalType, CSharpType, template);
        }

        public string GetCsharpClass() {
            return CodeHelper.CodeTransform.toCsharpClass(_type);
        }

        public string GetDBType() {
            return CodeHelper.CodeTransform.toDbType(_type);
        }

        public string GetSQLDecalre() {
            return Xy.CodeHelper.CodeTransform.toSqlDeclare(_type, _length, _precision, _scale);
        }
    }
}
