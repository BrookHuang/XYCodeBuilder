using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Xy.CodeHelper {
    public class SectionBuilder {
        public static string buildAttribute(DataTable inSource) {
            StringBuilder _sb = new StringBuilder();
            foreach (System.Data.DataRow _item in inSource.Rows) {
                _sb.AppendLine(string.Format("        public {0} {1} {{ get; set; }}", CodeTransform.toCsharp(_item["Type"].ToString()), _item["Name"].ToString()));
            }
            return _sb.ToString();
        }

        //public static string buildFill(DataTable inSource) {
        //    StringBuilder _sb = new StringBuilder();
        //    foreach (DataRow _item in inSource.Rows) {
        //        _sb.AppendLine(string.Format("            if (cols[\"{0}\"] != null) {{ this.{0} = {1}; }}",
        //            _item["Name"].ToString(),
        //            CodeTransform.toCsharpConvert("object", _item["Type"].ToString(), "_tempRow[\"" + _item["Name"].ToString() + "\"]")));
        //    }
        //    return _sb.ToString();
        //}

        //public static string buildtoStringCollection(DataTable inSource) {
        //    StringBuilder _sb = new StringBuilder();
        //    foreach (DataRow _item in inSource.Rows) {
        //        if (string.Compare(CodeTransform.toCsharp(_item["Type"].ToString()), "string") == 0) {
        //            _sb.AppendLine(string.Format("            _nvc.Add(\"{0}\", {1});", _item["Name"].ToString(), "this." + _item["Name"].ToString()));
        //        } else if (string.Compare(CodeTransform.toCsharp(_item["Type"].ToString()), "string") == 0) {
        //            _sb.AppendLine(string.Format("            _nvc.Add(\"{0}\", {1} == DateTime.MinValue ? string.Empty : {1}.ToString(\"M/d/yyyy HH:mm:ss\"));", _item["Name"].ToString(), "this." + _item["Name"].ToString()));
        //        } else {
        //            _sb.AppendLine(string.Format("            _nvc.Add(\"{0}\", Convert.ToString({1}));", _item["Name"].ToString(), "this." + _item["Name"].ToString()));
        //        }
        //    }
        //    return _sb.ToString();
        //}

        //public static string buildFillProcedure(DataTable inSource) {
        //    StringBuilder _sb = new StringBuilder();
        //    foreach (DataRow _item in inSource.Rows) {
        //        _sb.AppendLine(string.Format("            item.SetItem(\"{0}\", this.{0});", _item["Name"].ToString()));
        //    }
        //    return _sb.ToString();
        //}

        public static string buildCSharpHelpCode(DataTable inSource) {
            StringBuilder _sb = new StringBuilder();
            _sb.AppendLine("            Xy.Data.Procedure item = new Xy.Data.Procedure(R(\"itemname\"));");
            foreach (DataRow _item in inSource.Rows) {
                _sb.AppendLine(string.Format("            item.AddItem(\"{0}\", System.Data.DbType.{1});", _item["Name"].ToString(), CodeTransform.toDbType(_item["Type"].ToString())));
            }
            _sb.AppendLine("            AddProcedure(item);");
            _sb.AppendLine();
            int i = 1;
            int max = inSource.Rows.Count;
            foreach (DataRow _item in inSource.Rows) {
                if (i == 1) {
                    _sb.Append(string.Format("            {0} form{1}", CodeTransform.toCsharp(_item["Type"].ToString()), _item["Name"].ToString()));
                } else {
                    _sb.Append(string.Format("{0} form{1}", CodeTransform.toCsharp(_item["Type"].ToString()), _item["Name"].ToString()));
                }
                if (i != max) { _sb.Append(", "); }
                i++;
            }
            _sb.AppendLine();
            i = 1;
            foreach (DataRow _item in inSource.Rows) {
                if (i == 1) {
                    _sb.Append(string.Format("            form{0}", _item["Name"].ToString()));
                } else {
                    _sb.Append(string.Format("form{0}", _item["Name"].ToString()));
                }
                if (i != max) { _sb.Append(", "); }
                i++;
            }
            _sb.AppendLine();
            foreach (DataRow _item in inSource.Rows) {
                _sb.AppendLine(string.Format("            protected {1} form{0};", _item["Name"].ToString(), CodeTransform.toCsharp(_item["Type"].ToString())));
            }
            _sb.AppendLine();
            foreach (DataRow _item in inSource.Rows) {
                _sb.AppendLine(string.Format("            form{0} = {1};",
                    _item["Name"].ToString(),
                    CodeTransform.TypeConvert("string", CodeTransform.toCsharp(_item["Type"].ToString()), "Request.Form[\"" + _item["Name"].ToString() + "\"]")));
            }
            _sb.AppendLine();
            i = 1;
            foreach (DataRow _item in inSource.Rows) {
                if (i == 1) {
                    _sb.Append(string.Format("            {0} in{1}", CodeTransform.toCsharp(_item["Type"].ToString()), _item["Name"].ToString()));
                } else {
                    _sb.Append(string.Format("{0} in{1}", CodeTransform.toCsharp(_item["Type"].ToString()), _item["Name"].ToString()));
                }
                if (i != max) { _sb.Append(", "); }
                i++;
            }
            _sb.AppendLine();
            i = 1;
            foreach (DataRow _item in inSource.Rows) {
                if (i == 1) {
                    _sb.Append(string.Format("            in{0}", _item["Name"].ToString()));
                } else {
                    _sb.Append(string.Format("in{0}", _item["Name"].ToString()));
                }
                if (i != max) { _sb.Append(", "); }
                i++;
            }
            _sb.AppendLine();
            foreach (DataRow _item in inSource.Rows) {
                _sb.AppendLine(string.Format("            item.SetItem(\"{0}\", in{0});", _item["Name"].ToString()));
            }
            _sb.AppendLine();
            i = 1;
            foreach (DataRow _item in inSource.Rows) {
                if (i == 1) {
                    _sb.Append(string.Format("            {0}", _item["Name"].ToString()));
                } else {
                    _sb.Append(string.Format("{0}", _item["Name"].ToString()));
                }
                if (i != max) { _sb.Append(", "); }
                i++;
            }
            _sb.AppendLine();
            foreach (DataRow _item in inSource.Rows) {
                _sb.AppendLine(string.Format("            {0} = _item.{0};", _item["Name"].ToString()));
            }
            _sb.AppendLine();
            return _sb.ToString();
        }

        public static string buildSQLHelpCode(DataTable inSource) {
            StringBuilder _sb = new StringBuilder();
            int i = 1;
            int max = inSource.Rows.Count;
            foreach (DataRow _item in inSource.Rows) {
                if (i == max) {
                    _sb.AppendLine(string.Format("\t@{0} {1}", _item["Name"].ToString(), CodeTransform.toSqlDeclare(_item["Type"].ToString(), _item["Length"].ToString(), _item["Precision"].ToString(), _item["Scale"].ToString())));
                } else {
                    _sb.AppendLine(string.Format("\t@{0} {1},", _item["Name"].ToString(), CodeTransform.toSqlDeclare(_item["Type"].ToString(), _item["Length"].ToString(), _item["Precision"].ToString(), _item["Scale"].ToString())));
                }
                i++;
            }
            _sb.AppendLine();
            i = 1;
            foreach (DataRow _item in inSource.Rows) {
                if (i == 1) {
                    _sb.Append(string.Format("\t[{0}]", _item["Name"].ToString()));
                } else {
                    _sb.Append(string.Format("[{0}]", _item["Name"].ToString()));
                }
                if (i != max) { _sb.Append(" , "); }
                i++;
            }
            _sb.AppendLine();
            i = 1;
            foreach (DataRow _item in inSource.Rows) {
                if (i == 1 && max > 1) {
                    _sb.Append(string.Format("\t@{0}", _item["Name"].ToString()));
                } else {
                    _sb.Append(string.Format("@{0}", _item["Name"].ToString()));
                }
                if (i != max) { _sb.Append(" , "); }
                i++;
            }
            _sb.AppendLine();
            i = 1;
            foreach (DataRow _item in inSource.Rows) {
                if (i == max) {
                    _sb.AppendLine(string.Format("\t[{0}] = @{0}", _item["Name"].ToString()));
                } else {
                    _sb.AppendLine(string.Format("\t[{0}] = @{0},", _item["Name"].ToString()));
                }
                i++;
            }
            return _sb.ToString();
        }

        //public static string buildRegisterProcedure(FileHandle inFile, string inClassName) {
        //    StringBuilder _sb = new StringBuilder();
        //    string _stringFront = (inFile.Namespace + "." + inClassName + ".").Replace('.', '_');
        //    foreach (DataRow _row in DataGet.Get(DataGet.ContentType.AllProcedure).Rows) {
        //        string _currentProcedureName = _row["name"].ToString();
        //        if (_currentProcedureName.Contains(_stringFront) && _currentProcedureName.IndexOf(_stringFront) == 0) {
        //            string _procedureName = _currentProcedureName.Substring(_stringFront.Length);
        //            _sb.AppendLine(string.Format("            Xy.Data.Procedure {0} = new Xy.Data.Procedure(R(\"{0}\"));", _procedureName));
        //            DataTable _dt = DataGet.Get(DataGet.ContentType.ProcedureInfo, _currentProcedureName);
        //            foreach (DataRow _inrow in _dt.Rows) {
        //                _sb.AppendLine(string.Format("            {0}.AddItem(\"{1}\", System.Data.DbType.{2}{3});", _procedureName, _inrow["p_name"].ToString().Substring(1), CodeTransform.toDbType(_inrow["p_type"].ToString()), Convert.ToInt32(_inrow["p_isout"]) == 0 ? string.Empty : ", System.Data.ParameterDirection.InputOutput"));
        //            }
        //            _sb.AppendLine(string.Format("            AddProcedure({0});", _procedureName));
        //            _sb.AppendLine();
        //        }
        //    }
        //    return _sb.ToString();
        //}

        //public static string buildProcedureFunction(FileHandle inFile, string inClassName) {
        //    StringBuilder _sb = new StringBuilder();
        //    string _stringFront = (inFile.Namespace + "." + inClassName + ".").Replace('.', '_');
        //    foreach (DataRow _row in DataGet.Get(DataGet.ContentType.AllProcedure).Rows) {
        //        string _currentProcedureName = _row["name"].ToString();
        //        if (_currentProcedureName.Contains(_stringFront) && _currentProcedureName.IndexOf(_stringFront) == 0) {
        //            string _procedureName = _currentProcedureName.Substring(_stringFront.Length);

        //            DataTable _dt = DataGet.Get(DataGet.ContentType.ProcedureInfo, _currentProcedureName);

        //            DataTable _dtTable = DataGet.Get(DataGet.ContentType.TabelInfo, inClassName);

        //            string _keyType = string.Empty;
        //            foreach (DataRow _tablerow in _dtTable.Rows) {
        //                if (string.Compare(_tablerow["Name"].ToString(), "id") == 0) {
        //                    _keyType = _tablerow["Type"].ToString();
        //                    break;
        //                }
        //            }
        //            if (string.IsNullOrEmpty(_keyType)) {
        //                foreach (DataRow _tablerow in _dtTable.Rows) {
        //                    if (_tablerow["Name"].ToString().ToLower().Contains("id")) {
        //                        _keyType = _tablerow["Type"].ToString();
        //                        break;
        //                    }
        //                }
        //            }

        //            if ((_procedureName.Contains("Add") && _procedureName.IndexOf("Add") == 0) || (_procedureName.Contains("Insert") && _procedureName.IndexOf("Insert") == 0) || (_procedureName.Contains("Create") && _procedureName.IndexOf("Create") == 0)) {
        //                _sb.Append(buildFunction("Add", _currentProcedureName, _keyType, inFile, inClassName));
        //            } else if ((_procedureName.Contains("Del") && _procedureName.IndexOf("Del") == 0) || (_procedureName.Contains("Remove") && _procedureName.IndexOf("Remove") == 0)) {
        //                _sb.Append(buildFunction("Del", _currentProcedureName, _keyType, inFile, inClassName));
        //            } else if ((_procedureName.Contains("Get") && _procedureName.IndexOf("Get") == 0) || (_procedureName.Contains("Search") && _procedureName.IndexOf("Search") == 0)) {
        //                _sb.Append(buildFunction("Get", _currentProcedureName, _keyType, inFile, inClassName));
        //                if (string.Compare(_procedureName, "get", true) == 0) {
        //                    _sb.Append(buildFunction("GetInstance", _currentProcedureName, _keyType, inFile, inClassName));
        //                }
        //            } else if ((_procedureName.Contains("Edit") && _procedureName.IndexOf("Edit") == 0) || (_procedureName.Contains("Update") && _procedureName.IndexOf("Update") == 0)) {
        //                _sb.Append(buildFunction("Edit", _currentProcedureName, _keyType, inFile, inClassName));
        //            }
        //        }
        //    }
        //    return _sb.ToString();
        //}

        //public static string buildFunction(string inBuildKey, string inProcedureName, string inKeyType, FileHandle inFile, string inClassName) {
        //    string _stringFront = (inFile.Namespace + "." + inClassName + ".").Replace('.', '_');
        //    string _customerFile = string.Empty;
        //    string _procedureName = inProcedureName.Substring(_stringFront.Length);
        //    bool _hasOut = false; int i = 0;
        //    DataTable _paramTable = DataGet.Get(DataGet.ContentType.ProcedureInfo, inProcedureName);
        //    string _keyType = string.Empty;
        //    string _returnObjType = string.Empty;
        //    string _returnFunction = string.Empty;


        //    _keyType = Xy.CodeHelper.CodeTransform.toCsharp(inKeyType);

        //    foreach (DataRow _inrow in _paramTable.Rows) {
        //        if (Convert.ToInt32(_inrow["p_isout"]) == 1) _hasOut = true;
        //    }
        //    bool _afterCode = _hasOut;
        //    if (string.Compare(inBuildKey, "add", true) == 0) {
        //        _returnObjType = _keyType;
        //        _returnFunction = "InvokeProcedureResult(DB)";
        //    } else if (string.Compare(inBuildKey, "del", true) == 0) {
        //        _returnObjType = "int";
        //        _returnFunction = "InvokeProcedure(DB)";
        //    } else if (string.Compare(inBuildKey, "get", true) == 0) {
        //        _returnObjType = "System.Data.DataTable";
        //        _returnFunction = "InvokeProcedureFill(DB)";
        //    } else if (string.Compare(inBuildKey, "getinstance", true) == 0) {
        //        _returnObjType = "System.Data.DataTable";
        //        _returnFunction = "InvokeProcedureFill(DB)";
        //        _afterCode = true;
        //    } else if (string.Compare(inBuildKey, "edit", true) == 0) {
        //        _returnObjType = "int";
        //        _returnFunction = "InvokeProcedure(DB)";
        //    }

        //    if (System.IO.File.Exists(inFile.FilePath + inClassName + ".cs")) {
        //        using (System.IO.FileStream _fs = new System.IO.FileStream(inFile.FilePath + inClassName + ".cs", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read)) {
        //            using (System.IO.StreamReader _sr = new System.IO.StreamReader(_fs)) {
        //                _customerFile = _sr.ReadToEnd();
        //                _sr.Close();
        //            }
        //            _fs.Close();
        //        }
        //    }

        //    StringBuilder _sb = new StringBuilder();
        //    if (string.Compare(inBuildKey, "getinstance", true) != 0) {
        //        _sb.Append(string.Format("        //private static void before_{0}(", _procedureName));
        //        i = 0;
        //        foreach (DataRow _inrow in _paramTable.Rows) {
        //            if (i > 0) _sb.Append(", ");
        //            _sb.Append(string.Format("ref {0} in{1}", CodeTransform.toCsharp(_inrow["p_type"].ToString()), _inrow["p_name"].ToString().Substring(1)));
        //            i++;
        //        }
        //        _sb.AppendLine(") { }");
        //    }

        //    if (string.Compare(inBuildKey, "getinstance", true) != 0) {
        //        _sb.AppendLine(string.Format("        //private static {0} after_{1}({0} result) {{ }};", _returnObjType, _procedureName));
        //    }
            
        //    if (string.Compare(inBuildKey, "add", true) == 0) {
        //        _sb.Append(string.Format("        public static {0} {1}(", _keyType, _procedureName));
        //    } else if (string.Compare(inBuildKey, "del", true) == 0) {
        //        _sb.Append(string.Format("        public static int {0}(", _procedureName));
        //    } else if (string.Compare(inBuildKey, "get", true) == 0) {
        //        _sb.Append(string.Format("        public static System.Data.DataTable {0}(", _procedureName));
        //    } else if (string.Compare(inBuildKey, "getinstance", true) == 0) {
        //        _sb.Append(string.Format("        public static " + inClassName + " {0}Instance(", _procedureName));
        //    } else if (string.Compare(inBuildKey, "edit", true) == 0) {
        //        _sb.Append(string.Format("        public static int {0}(", _procedureName));
        //    }

        //    i = 0;
        //    foreach (DataRow _inrow in _paramTable.Rows) {
        //        _sb.Append(string.Format("{0}{1} in{2}, ", Convert.ToInt32(_inrow["p_isout"]) == 0 ? string.Empty : "ref ", CodeTransform.toCsharp(_inrow["p_type"].ToString()), _inrow["p_name"].ToString().Substring(1)));
        //    }
        //    _sb.Append("Xy.Data.DataBase DB = null)");
        //    if (!string.IsNullOrEmpty(_customerFile)) {
        //        if (_customerFile.IndexOf(_sb.ToString()) > 0) {
        //            System.Windows.Forms.DialogResult _dr = System.Windows.Forms.MessageBox.Show(string.Format("\"{0}\" already existed, overwrite?", _sb.ToString().Trim()), "Confirm", System.Windows.Forms.MessageBoxButtons.YesNo);
        //            if (_dr == System.Windows.Forms.DialogResult.No) return string.Empty;
        //        }
        //    }
        //    _sb.AppendLine(" {");

        //    if (!string.IsNullOrEmpty(_customerFile)) {
        //        System.Text.RegularExpressions.Regex _beforeReg = new System.Text.RegularExpressions.Regex("static void before_" + _procedureName, System.Text.RegularExpressions.RegexOptions.Compiled);
        //        if (_beforeReg.IsMatch(_customerFile) && string.Compare(inBuildKey, "getinstance", true) != 0) {
        //            _sb.Append(Environment.NewLine + "            " + inFile.Namespace + "." + inClassName + ".before_" + _procedureName + "(");
        //            i = 0;
        //            foreach (DataRow _inrow in _paramTable.Rows) {
        //                if (i > 0) _sb.Append(", ");
        //                _sb.Append(string.Format("ref in{0}", _inrow["p_name"].ToString().Substring(1)));
        //                i++;
        //            }
        //            _sb.AppendLine(");" + Environment.NewLine);
        //        }
        //    }

        //    _sb.AppendLine(string.Format("            Xy.Data.Procedure item = {0}.{1}.GetProcedure(R(\"{2}\"));", inFile.Namespace, inClassName, _procedureName));
        //    foreach (DataRow _inrow in _paramTable.Rows) {
        //        _sb.AppendLine(string.Format("            item.SetItem(\"{0}\", in{0});", _inrow["p_name"].ToString().Substring(1)));
        //    }

        //    System.Text.RegularExpressions.Regex _afterReg = new System.Text.RegularExpressions.Regex("static " + _returnObjType + " after_" + _procedureName, System.Text.RegularExpressions.RegexOptions.Compiled);

        //    if (_afterCode) {
        //        if (!string.IsNullOrEmpty(_customerFile) && _afterReg.IsMatch(_customerFile) && string.Compare(inBuildKey, "getinstance", true) != 0) {
        //            _sb.AppendLine(string.Format("            {0} result = {1}.{2}.{3}(item.{4});", _returnObjType, inFile.Namespace, inClassName, "after_" + _procedureName, _returnFunction));
        //        } else {
        //            _sb.AppendLine(string.Format("            {0} result = item.{1};", _returnObjType, _returnFunction));
        //        }
        //        if (_hasOut) {
        //            foreach (DataRow _inrow in _paramTable.Rows) {
        //                if (Convert.ToInt32(_inrow["p_isout"]) == 1) {
        //                    _sb.AppendLine(string.Format("            in{0} = {1};",
        //                        _inrow["p_name"].ToString().Substring(1),
        //                        CodeHelper.CodeTransform.DBConvert("object", _inrow["p_type"].ToString(), "item.GetItem(\"" + _inrow["p_name"].ToString().Substring(1) + "\")")));
        //                }
        //            }
        //        }
        //        if (string.Compare(inBuildKey, "getinstance", true) == 0) {
        //            _sb.AppendLine("            if (result.Rows.Count > 0) {");
        //            _sb.AppendLine("                " + inClassName + " temp = new " + inClassName + "();");
        //            _sb.AppendLine("                temp.Fill(result.Rows[0]);");
        //            _sb.AppendLine("                return temp;");
        //            _sb.AppendLine("            }");
        //            _sb.AppendLine("            return null;");
        //        } else {
        //            _sb.AppendLine("            return result;");
        //        }
        //        _sb.AppendLine("        }");
        //    } else {
        //        if (!string.IsNullOrEmpty(_customerFile) && _afterReg.IsMatch(_customerFile) && string.Compare(inBuildKey, "getinstance", true) != 0) {
        //            _sb.AppendLine(string.Format("            return {0}.{1}.{2}(item.{3});", inFile.Namespace, inClassName, "after_" + _procedureName, _returnFunction));
        //        } else {
        //            _sb.AppendLine(string.Format("            return item.{0};", _returnFunction));
        //        }
        //        _sb.AppendLine("        }");
        //    }
        //    if (string.Compare(inBuildKey, "add", true) == 0){// && string.Compare(_keyType, "long", true) != 0) {
        //        _sb.Replace(string.Format("item.{0}", _returnFunction), CodeTransform.DBConvert("object", inKeyType, "item." + _returnFunction));
        //    }
        //    //if (!string.IsNullOrEmpty(_customerFile) && _afterReg.IsMatch(_customerFile)) {
        //    //    _sb.AppendLine();
        //    //    if (string.Compare(inBuildKey, "add", true) == 0) {
        //    //        _sb.AppendLine(string.Format("            return {0}.{1}.{2}(item.InvokeProcedureResult());", inFile.Namespace, inClassName, "after_" + _procedureName));
        //    //        _sb.AppendLine("        }");
        //    //    } else if (string.Compare(inBuildKey, "del", true) == 0) {
        //    //        _sb.AppendLine(string.Format("            return {0}.{1}.{2}(item.InvokeProcedure());", inFile.Namespace, inClassName, "after_" + _procedureName));
        //    //        _sb.AppendLine("        }");
        //    //    } else if (string.Compare(inBuildKey, "get", true) == 0) {
        //    //        _sb.AppendLine(string.Format("            return {0}.{1}.{2}(item.InvokeProcedureFill());", inFile.Namespace, inClassName, "after_" + _procedureName));
        //    //        _sb.AppendLine("        }");
        //    //    } else if (string.Compare(inBuildKey, "getinstance", true) == 0) {
        //    //        _sb.AppendLine(string.Format("            System.Data.DataTable _dt = {0}.{1}.{2}(item.InvokeProcedureFill());", inFile.Namespace, inClassName, "after_" + _procedureName));
        //    //        _sb.AppendLine("            if (_dt.Rows.Count > 0) {");
        //    //        _sb.AppendLine("                User temp = new User();");
        //    //        _sb.AppendLine("                temp.Fill(_dt.Rows[0]);");
        //    //        _sb.AppendLine("                return temp;");
        //    //        _sb.AppendLine("            }");
        //    //        _sb.AppendLine("            return null;");
        //    //        _sb.AppendLine("        }");
        //    //    } else if (string.Compare(inBuildKey, "edit", true) == 0) {
        //    //        _sb.AppendLine(string.Format("            return {0}.{1}.{2}(item.InvokeProcedure());", inFile.Namespace, inClassName, "after_" + _procedureName));
        //    //        _sb.AppendLine("        }");
        //    //    }
        //    //    _sb.AppendLine();
        //    //} else {
        //    //    if (string.Compare(inBuildKey, "add", true) == 0) {
        //    //        _sb.AppendLine("            return item.InvokeProcedureResult();");
        //    //        _sb.AppendLine("        }");
        //    //    } else if (string.Compare(inBuildKey, "del", true) == 0) {
        //    //        _sb.AppendLine("            return item.InvokeProcedure();");
        //    //        _sb.AppendLine("        }");
        //    //    } else if (string.Compare(inBuildKey, "get", true) == 0) {
        //    //        _sb.AppendLine("            return item.InvokeProcedureFill();");
        //    //        _sb.AppendLine("        }");
        //    //    } else if (string.Compare(inBuildKey, "getinstance", true) == 0) {
        //    //        _sb.AppendLine("            System.Data.DataTable _dt = item.InvokeProcedureFill();");
        //    //        _sb.AppendLine("            if (_dt.Rows.Count > 0) {");
        //    //        _sb.AppendLine("                " + inClassName + " temp = new " + inClassName + "();");
        //    //        _sb.AppendLine("                temp.Fill(_dt.Rows[0]);");
        //    //        _sb.AppendLine("                return temp;");
        //    //        _sb.AppendLine("            }");
        //    //        _sb.AppendLine("            return null;");
        //    //        _sb.AppendLine("        }");
        //    //    } else if (string.Compare(inBuildKey, "edit", true) == 0) {
        //    //        _sb.AppendLine("            return item.InvokeProcedure();");
        //    //        _sb.AppendLine("        }");
        //    //    }
        //    //}
        //    if (_paramTable.Rows.Count == 0) return _sb.ToString();
        //    if (string.Compare(inBuildKey, "add", true) == 0) {
        //        _sb.Append(string.Format("        public static {0} {1}(", _keyType, _procedureName));
        //    } else if (string.Compare(inBuildKey, "del", true) == 0) {
        //        _sb.Append(string.Format("        public static int {0}(", _procedureName));
        //    } else if (string.Compare(inBuildKey, "get", true) == 0) {
        //        _sb.Append(string.Format("        public static System.Data.DataTable {0}(", _procedureName));
        //    } else if (string.Compare(inBuildKey, "getinstance", true) == 0) {
        //        _sb.Append(string.Format("        public static " + inClassName + " {0}Instance(", _procedureName));
        //    } else if (string.Compare(inBuildKey, "edit", true) == 0) {
        //        _sb.Append(string.Format("        public static int {0}(", _procedureName));
        //    }
        //    _sb.Append("System.Collections.Specialized.NameValueCollection values,");
        //    if(_hasOut){
        //        foreach (DataRow _inrow in _paramTable.Rows) {
        //            if (Convert.ToInt32(_inrow["p_isout"]) == 1) {
        //                _sb.AppendFormat(" ref {0} {1},", CodeTransform.toCsharp(_inrow["p_type"].ToString()), _inrow["p_name"].ToString().Substring(1, 1).ToLower() + _inrow["p_name"].ToString().Substring(2));
        //            }
        //        }
        //    }
        //    _sb.AppendLine(" Xy.Data.DataBase DB = null) {");

        //    i = 0;
        //    if (_afterCode) {
        //        //if (_hasOut) {
        //        //    foreach (DataRow _inrow in _paramTable.Rows) {
        //        //        if (Convert.ToInt32(_inrow["p_isout"]) == 1) {
        //        //            _sb.AppendLine(string.Format("            {0} _{1} = {2};",
        //        //                CodeHelper.CodeTransform.toCsharp(_inrow["p_type"].ToString()),
        //        //                _inrow["p_name"].ToString().Substring(1, 1).ToLower() + _inrow["p_name"].ToString().Substring(2),
        //        //                CodeHelper.CodeTransform.toCsharpConvert("string", _inrow["p_type"].ToString(), "values[\"" + _inrow["p_name"].ToString().Substring(1) + "\"]")));
        //        //        }
        //        //    }
        //        //}
        //        if (string.Compare(inBuildKey, "getinstance", true) == 0) {
        //            _sb.Append(string.Format("            {0} result = {1}Instance(", inClassName , _procedureName));
        //        } else {
        //            _sb.Append(string.Format("            {0} result = {1}(", _returnObjType,_procedureName));
        //        }
        //        i = 0;
        //        foreach (DataRow _inrow in _paramTable.Rows) {
        //            if (Convert.ToInt32(_inrow["p_isout"]) == 1) {
        //                _sb.Append(string.Format("ref {0}, ", _inrow["p_name"].ToString().Substring(1, 1).ToLower() + _inrow["p_name"].ToString().Substring(2)));
        //            } else {
        //                _sb.Append(CodeTransform.DBConvert("string", _inrow["p_type"].ToString(), "values[\"" + _inrow["p_name"].ToString().Substring(1) + "\"]") + ", ");
        //            }
        //        }
        //        _sb.AppendLine("DB);");
        //        //if (_hasOut) {
        //        //    foreach (DataRow _inrow in _paramTable.Rows) {
        //        //        if (Convert.ToInt32(_inrow["p_isout"]) == 1) {
        //        //            _sb.AppendLine(string.Format("            values[\"{0}\"] = _{1}.ToString();", _inrow["p_name"].ToString().Substring(1), _inrow["p_name"].ToString().Substring(1, 1).ToLower() + _inrow["p_name"].ToString().Substring(2)));
        //        //        }
        //        //    }
        //        //}
        //        _sb.AppendLine("            return result;");
        //    } else {
        //        if (string.Compare(inBuildKey, "getinstance", true) == 0) {
        //            _sb.Append(string.Format("            return {0}Instance(", _procedureName));
        //        } else {
        //            _sb.Append(string.Format("            return {0}(", _procedureName));
        //        }
        //        foreach (DataRow _inrow in _paramTable.Rows) {
        //            _sb.Append(CodeTransform.DBConvert("string", _inrow["p_type"].ToString(), "values[\"" + _inrow["p_name"].ToString().Substring(1) + "\"]") + ", ");
        //        }
        //        _sb.AppendLine("DB);");
        //    }
        //    _sb.AppendLine("        }");

        //    return _sb.ToString();
        //}
    }
}
