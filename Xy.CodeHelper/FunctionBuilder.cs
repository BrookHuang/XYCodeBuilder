using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.CodeHelper {
    public class FunctionBuilder {
        public static string buildFunction_R(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.AppendLine(T(2) + "    return \"" + (fileBuilder.File.Namespace + "." + fileBuilder.ClassName + ".").Replace('.', '_') + "\" + name;");
            _code.AppendLine(T(2) + "}");
            return _code.ToString();
        }

        public static string buildFunction_AddProcedure(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.AppendLine(T(3) + "if (_procedures == null) _procedures = new Dictionary<string, Xy.Data.Procedure>();");
            _code.AppendLine(T(3) + "_procedures.Add(_inValue.Name, _inValue);");
            _code.AppendLine(T(2) + "}");
            return _code.ToString();
        }

        public static string buildFunction_GetProcedure(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.AppendLine(T(3) + "if (_procedures == null) _procedures = new Dictionary<string,Xy.Data.Procedure>();");
            _code.AppendLine(T(3) + "if (_procedures.ContainsKey(name)) {");
            _code.AppendLine(T(4) + "return _procedures[name].Clone();");
            _code.AppendLine(T(3) + "} else {");
            _code.AppendLine(T(4) + "throw new Exception(string.Format(\"can not found \\\"{0}\\\" in procedure list\", name));");
            _code.AppendLine(T(3) + "}");
            _code.AppendLine(T(2) + "}");
            return _code.ToString();
        }

        public static string buildFunction_Fill(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.AppendLine(T(3) + "System.Data.DataColumnCollection cols = inTempRow.Table.Columns;");
            foreach (CodeEntity.Field _field in fileBuilder.Table.FieldList) {
                _code.AppendLine(string.Format(T(3) + @"if (cols[""{0}""] != null) {{ this.{0} = {1}; }}", _field.Name, _field.ConvertFrom("inTempRow[\"{name}\"]", "object")));
            }
            _code.AppendLine(T(2) + "}");
            return _code.ToString();
        }

        public static string buildFunction_FillRow(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            foreach (CodeEntity.Field _field in fileBuilder.Table.FieldList) {
                _code.AppendLine(string.Format(T(3) + @"inTempRow[""{0}""] = this.{0};", _field.Name));
            }
            _code.AppendLine(T(2) + "}");
            return _code.ToString();
        }

        public static string buildFunction_FillProcedure(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            foreach (CodeEntity.Field _field in fileBuilder.Table.FieldList) {
                _code.AppendLine(string.Format(T(3) + @"inItem.SetItem(""{0}"", this.{0});", _field.Name));
            }
            _code.AppendLine(T(3) + "return inItem;");
            _code.AppendLine(T(2) + "}");
            return _code.ToString();
        }

        public static string buildFunction_CreateEmptyTable(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.AppendLine(T(3) + "System.Data.DataTable _table = new System.Data.DataTable();");
            foreach (CodeEntity.Field _field in fileBuilder.Table.FieldList) {
                _code.AppendLine(string.Format(T(3) + @"_table.Columns.Add(""{0}"", typeof({1}));", _field.Name, _field.GetCsharpClass()));
            }
            _code.AppendLine(T(3) + "return _table;");
            _code.AppendLine(T(2) + "}");
            return _code.ToString();
        }

        //public static string buildFunction_toStringCollection(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
        //    StringBuilder _code = new StringBuilder();
        //    _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
        //    _code.AppendLine(T(3) + "System.Collections.Specialized.NameValueCollection _nvc = new System.Collections.Specialized.NameValueCollection();");
        //    foreach (CodeEntity.Field _field in fileBuilder.Table.FieldList) {
        //        _code.AppendLine(string.Format(T(3) + @"_nvc.Add(""{0}"", {1});", _field.Name, _field.ConvertTo("this.{name}", "string")));
        //    }
        //    _code.AppendLine(T(3) + "return _nvc;");
        //    _code.AppendLine(T(2) + "}");
        //    return _code.ToString();
        //}

        public static string buildFunction_GetAttributesName(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.Append(T(3) + "return new string[]{");
            for (int i = 0; i < fileBuilder.Table.FieldList.Count; i++) {
                CodeEntity.Field _field = fileBuilder.Table.FieldList[i];
                if (i > 0) _code.Append(',');
                _code.Append(" \"" + _field.Name + "\"");
            }
            _code.AppendLine(" };");
            _code.AppendLine(T(2) + "}");
            return _code.ToString();
        }

        public static string buildFunction_GetAttributesValue(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.AppendLine(T(3) + "switch (inName) {");
            foreach (CodeEntity.Field _field in fileBuilder.Table.FieldList) {
                _code.AppendLine(T(4) + "case \"" + _field.Name + "\":");
                _code.AppendLine(T(5) + "return this." + _field.Name + ";");
            }
            _code.AppendLine(T(4) + "default:");
            _code.AppendLine(T(5) + "return null;");
            _code.AppendLine(T(3) + "}");
            _code.AppendLine(T(2) + "}");
            return _code.ToString();
        }

        public static string buildFunction_GetXml(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.AppendLine(T(3) + "StringBuilder _xmlBuilder = new StringBuilder();");
            _code.AppendLine(T(3) + "string[] _attrs = GetAttributesName();");
            _code.AppendLine(T(3) + "_xmlBuilder.Append(\"<" + fileBuilder.Table.Name + ">\");");
            _code.AppendLine(T(3) + "foreach (string _attr in _attrs) {");
            _code.AppendLine(T(4) + "_xmlBuilder.AppendFormat(\"<{0}>{1}</{0}>\", _attr, GetAttributesValue(_attr));");
            _code.AppendLine(T(3) + "}");
            _code.AppendLine(T(3) + "_xmlBuilder.Append(\"</" + fileBuilder.Table.Name + ">\");");
            _code.AppendLine(T(3) + "return new System.Xml.XPath.XPathDocument(new System.IO.StringReader(_xmlBuilder.ToString()));");
            _code.AppendLine(T(2) + "}");
            return _code.ToString();
        }

        public static string buildFunction_RegisterProcedures(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            foreach (CodeEntity.Procedure _procedure in fileBuilder.Table.ProcedureList) {
                _code.AppendLine(string.Format(T(3) + @"Xy.Data.Procedure {0} = new Xy.Data.Procedure(R(""{0}""));", _procedure.Name));
                foreach (CodeEntity.Parameter _parameter in _procedure.Parameters) {
                    _code.AppendLine(string.Format(T(3) + @"{0}.AddItem(""{1}"", {2}{3});", _procedure.Name, _parameter.Name, _parameter.GetDBType(), _parameter.IsOut ? ", System.Data.ParameterDirection.Output" : string.Empty));
                }
                _code.AppendLine(string.Format(T(3) + @"AddProcedure({0});", _procedure.Name));
                _code.AppendLine();
            }
            _code.AppendLine(T(2) + "}");
            return _code.ToString();
        }

        public static string buildFunction_Procudure_Add(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            CodeEntity.Procedure _currentProcedure = (CodeEntity.Procedure)param[0];
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + "#region " + returnType + " " + name + "(" + parameter + ")");
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.AppendLine(T(3) + "Xy.Data.Procedure item = " + fileBuilder.File.Namespace + "." + _currentProcedure.GetTableName() + ".GetProcedure(R(\"" + _currentProcedure.Name + "\"));");
            foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                _code.AppendLine(T(3) + "item.SetItem(\"" + _parameter.Name + "\", in" + _parameter.Name + ");");
            }
            if (_currentProcedure.HasOut) {
                _code.AppendLine(T(3) + returnType + " result = " + CodeHelper.CodeTransform.TypeConvert("object", returnType, "item.InvokeProcedureResult(DB)") + ";");
                foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                    if (_parameter.IsOut) {
                        _code.AppendLine(T(3) + "in" + _parameter.Name + " = " + _parameter.ConvertFrom("item.GetItem(\"" + _parameter.Name + "\")", "object") + ";");
                    }
                }
                _code.AppendLine(T(3) + "return result;");
            } else {
                _code.AppendLine(T(3) + "return " + CodeHelper.CodeTransform.TypeConvert("object", returnType, "item.InvokeProcedureResult(DB)") + ";");
            }
            _code.AppendLine(T(2) + "}");
            _code.Append(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(System.Collections.Specialized.NameValueCollection values, ");
            if (_currentProcedure.HasOut) {
                foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                    if (_parameter.IsOut) {
                        _code.AppendLine("ref " + _parameter.CSharpType + " in" + _parameter.Name);
                    }
                }
            }
            _code.AppendLine("Xy.Data.DataBase DB = null) {");
            _code.Append(T(3) + "return " + _currentProcedure.Name + "(");
            foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                _code.Append(_parameter.ConvertFrom("values[\"" + _parameter.Name + "\"]", "string") + ", ");
            }
            _code.AppendLine("DB);");
            _code.AppendLine(T(2) + "}");
            _code.AppendLine(T(2) + "#endregion");
            return _code.ToString();
        }

        public static string buildFunction_Procudure_Del(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            CodeEntity.Procedure _currentProcedure = (CodeEntity.Procedure)param[0];
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + "#region " + returnType + " " + name + "(" + parameter + ")");
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.AppendLine(T(3) + "Xy.Data.Procedure item = " + fileBuilder.File.Namespace + "." + _currentProcedure.GetTableName() + ".GetProcedure(R(\"" + _currentProcedure.Name + "\"));");
            foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                _code.AppendLine(T(3) + "item.SetItem(\"" + _parameter.Name + "\", in" + _parameter.Name + ");");
            }
            if (_currentProcedure.HasOut) {
                _code.AppendLine(T(3) + returnType + " result = item.InvokeProcedure(DB);");
                foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                    if (_parameter.IsOut) {
                        _code.AppendLine(T(3) + "in" + _parameter.Name + " = " + _parameter.ConvertFrom("item.GetItem(\"" + _parameter.Name + "\")", "object") + ";");
                    }
                }
                _code.AppendLine(T(3) + "return result;");
            } else {
                _code.AppendLine(T(3) + "return item.InvokeProcedure(DB);");
            }
            _code.AppendLine(T(2) + "}");
            _code.Append(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(System.Collections.Specialized.NameValueCollection values, ");
            if (_currentProcedure.HasOut) {
                foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                    if (_parameter.IsOut) {
                        _code.Append("ref " + _parameter.CSharpType + " in" + _parameter.Name + ", ");
                    }
                }
            }
            _code.AppendLine("Xy.Data.DataBase DB = null) {");
            _code.Append(T(3) + "return " + _currentProcedure.Name + "(");
            foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                _code.Append(_parameter.ConvertFrom("values[\"" + _parameter.Name + "\"]", "string") + ", ");
            }
            _code.AppendLine("DB);");
            _code.AppendLine(T(2) + "}");
            _code.AppendLine(T(2) + "#endregion");
            return _code.ToString();
        }

        public static string buildFunction_Procudure_Get(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            CodeEntity.Procedure _currentProcedure = (CodeEntity.Procedure)param[0];
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + "#region " + returnType + " " + name + "(" + parameter + ")");
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.AppendLine(T(3) + "Xy.Data.Procedure item = " + fileBuilder.File.Namespace + "." + _currentProcedure.GetTableName() + ".GetProcedure(R(\"" + _currentProcedure.Name + "\"));");
            foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                _code.AppendLine(T(3) + "item.SetItem(\"" + _parameter.Name + "\", in" + _parameter.Name + ");");
            }
            if (_currentProcedure.HasOut) {
                _code.AppendLine(T(3) + returnType + " result = item.InvokeProcedureFill(DB);");
                foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                    if (_parameter.IsOut) {
                        _code.AppendLine(T(3) + "in" + _parameter.Name + " = " + _parameter.ConvertFrom("item.GetItem(\"" + _parameter.Name + "\")", "object") + ";");
                    }
                }
                _code.AppendLine(T(3) + "return result;");
            } else {
                _code.AppendLine(T(3) + "return item.InvokeProcedureFill(DB);");
            }
            _code.AppendLine(T(2) + "}");
            _code.Append(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(System.Collections.Specialized.NameValueCollection values, ");
            if (_currentProcedure.HasOut) {
                foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                    if (_parameter.IsOut) {
                        _code.Append("ref " + _parameter.CSharpType + " in" + _parameter.Name + ", ");
                    }
                }
            }
            _code.AppendLine("Xy.Data.DataBase DB = null) {");
            _code.Append(T(3) + "return " + _currentProcedure.Name + "(");
            foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                if (_parameter.IsOut) {
                    _code.Append("ref in" + _parameter.Name + ", ");
                } else {
                    _code.Append(_parameter.ConvertFrom("values[\"" + _parameter.Name + "\"]", "string") + ", ");
                }
            }
            _code.AppendLine("DB);");
            _code.AppendLine(T(2) + "}");
            if (string.Compare(name, "Get", true) == 0) {
                _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + _currentProcedure.GetTableName() + " " + name + "Instance(" + parameter + ") {");
                _code.Append(T(3) + "System.Data.DataTable result = " + _currentProcedure.Name + "(");
                foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                    _code.Append((_parameter.IsOut ? "ref " : string.Empty) + "in" + _parameter.Name + ", ");
                }
                _code.AppendLine("DB);");
                _code.AppendLine(T(3) + "if (result.Rows.Count > 0) {");
                _code.AppendLine(T(4) + _currentProcedure.GetTableName() + " temp = new " + _currentProcedure.GetTableName() + "();");
                _code.AppendLine(T(4) + "temp.Fill(result.Rows[0]);");
                _code.AppendLine(T(4) + "return temp;");
                _code.AppendLine(T(3) + "}");
                _code.AppendLine(T(3) + "return null;");
                _code.AppendLine(T(2) + "}");
                _code.Append(T(2) + access + " " + (isStatic ? "static " : string.Empty) + _currentProcedure.GetTableName() + " " + name + "Instance(System.Collections.Specialized.NameValueCollection values, ");
                if (_currentProcedure.HasOut) {
                    foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                        if (_parameter.IsOut) {
                            _code.Append("ref " + _parameter.CSharpType + " in" + _parameter.Name + ", ");
                        }
                    }
                }
                _code.AppendLine("Xy.Data.DataBase DB = null) {");
                _code.Append(T(3) + "return " + _currentProcedure.Name + "Instance(");
                foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                    if (_parameter.IsOut) {
                        _code.Append("ref in" + _parameter.Name + ", ");
                    } else {
                        _code.Append(_parameter.ConvertFrom("values[\"" + _parameter.Name + "\"]", "string") + ", ");
                    }
                }
                _code.AppendLine("DB);");
                _code.AppendLine(T(2) + "}");
            }
            _code.AppendLine(T(2) + "#endregion");
            return _code.ToString();
        }

        public static string buildFunction_Procudure_Update(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            CodeEntity.Procedure _currentProcedure = (CodeEntity.Procedure)param[0];
            StringBuilder _code = new StringBuilder();
            _code.AppendLine(T(2) + "#region " + returnType + " " + name + "(" + parameter + ")");
            _code.AppendLine(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ") {");
            _code.AppendLine(T(3) + "Xy.Data.Procedure item = " + fileBuilder.File.Namespace + "." + _currentProcedure.GetTableName() + ".GetProcedure(R(\"" + _currentProcedure.Name + "\"));");
            foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                _code.AppendLine(T(3) + "item.SetItem(\"" + _parameter.Name + "\", in" + _parameter.Name + ");");
            }
            if (_currentProcedure.HasOut) {
                _code.AppendLine(T(3) + returnType + " result = item.InvokeProcedure(DB);");
                foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                    if (_parameter.IsOut) {
                        _code.AppendLine(T(3) + "in" + _parameter.Name + " = " + _parameter.ConvertFrom("item.GetItem(\"" + _parameter.Name + "\")", "object") + ";");
                    }
                }
                _code.AppendLine(T(3) + "return result;");
            } else {
                _code.AppendLine(T(3) + "return item.InvokeProcedure(DB);");
            }
            _code.AppendLine(T(2) + "}");
            _code.Append(T(2) + access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(System.Collections.Specialized.NameValueCollection values, ");
            if (_currentProcedure.HasOut) {
                foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                    if (_parameter.IsOut) {
                        _code.Append("ref " + _parameter.CSharpType + " in" + _parameter.Name + ", ");
                    }
                }
            }
            _code.AppendLine("Xy.Data.DataBase DB = null) {");
            _code.Append(T(3) + "return " + _currentProcedure.Name + "(");
            foreach (CodeEntity.Parameter _parameter in _currentProcedure.Parameters) {
                if (_parameter.IsOut) {
                    _code.Append("ref in" + _parameter.Name + ", ");
                } else {
                    _code.Append(_parameter.ConvertFrom("values[\"" + _parameter.Name + "\"]", "string") + ", ");
                }
            }
            _code.AppendLine("DB);");
            _code.AppendLine(T(2) + "}");
            _code.AppendLine(T(2) + "#endregion");
            return _code.ToString();
        }

        public static string buildFunction_Empty(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param) {
            return string.Empty;
        }

        public static string T(int n) {
            char[] _tabs = new char[n];
            for (int i = 0; i < n; i++) {
                _tabs[i] = '\t';
            }
            return new string(_tabs);

        }
    }
}
