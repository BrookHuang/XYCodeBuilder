using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.CodeEntity {
    public class Parameter {
        private string _name;
        public string Name { get { return _name; } }
        private string _type;
        public string Type { get { return _type; } }
        private string _cSharpType;
        public string CSharpType { get { return _cSharpType; } }
        private bool _isOut;
        public bool IsOut { get { return _isOut; } }

        public Parameter(System.Data.DataRow row) {
            _name = row["p_name"].ToString().TrimStart('@');
            _type = row["p_type"].ToString();
            _isOut = Convert.ToBoolean(row["p_isout"]);
            _cSharpType = CodeHelper.CodeTransform.toCsharp(_type);
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

        public string GetDBType() {
            return CodeHelper.CodeTransform.toDbType(_type);
        }
    }
}
