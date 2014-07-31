using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.CodeHelper {
    public class FileBuilder {

        public string ClassName { get; set; }
        public FileHandle File { get; set; }
        public CodeEntity.Table Table { get { return _table; } }
        private string _originalContent = string.Empty;
        private Xy.CodeEntity.Table _table = null;
        public FileBuilder(FileHandle inFile, string inClassName) {
            ClassName = inClassName;
            File = inFile;
            if (System.IO.File.Exists(inFile.FilePath + inClassName + ".cs")) {
                _originalContent = System.IO.File.ReadAllText(inFile.FilePath + inClassName + ".cs");
            }
            _table = new CodeEntity.Table(inFile, inClassName);
        }

        public void CreateBaseFile() {
            StringBuilder _sbBase = new StringBuilder();
            _sbBase.AppendLine(StaticSection.InsertCopyright());
            _sbBase.AppendLine(StaticSection.InsertUsing());
            _sbBase.AppendLine();
            _sbBase.AppendLine("namespace " + File.Namespace + " {");
            _sbBase.AppendLine("    public partial class " + ClassName + " : Xy.Data.IDataModel, Xy.Data.IDataModelDisplay {");
            _sbBase.AppendLine(SectionBuilder.buildAttribute(DataGet.Get(DataGet.ContentType.TabelInfo, ClassName)));
            _sbBase.AppendLine(_buildFunction("R", "public", "string", "string name", true, FunctionBuilder.buildFunction_R));
            _sbBase.AppendLine("        private static Dictionary<string, Xy.Data.Procedure> _procedures;");
            _sbBase.AppendLine(_buildFunction("AddProcedure", "private", "void", "Xy.Data.Procedure _inValue", true, FunctionBuilder.buildFunction_AddProcedure));
            _sbBase.AppendLine(_buildFunction("GetProcedure", "private", "Xy.Data.Procedure", "string name", true, FunctionBuilder.buildFunction_GetProcedure));
            _sbBase.AppendLine("        static " + ClassName + "() {");
            _sbBase.AppendLine("            RegisterProcedures();");
            _sbBase.AppendLine("            RegisterEvents();");
            _sbBase.AppendLine("        }");
            _sbBase.AppendLine();
            _sbBase.AppendLine(_buildFunction("Fill", "public", "void", "System.Data.DataRow inTempRow", false, FunctionBuilder.buildFunction_Fill));
            _sbBase.AppendLine(_buildFunction("FillRow", "public", "void", "System.Data.DataRow inTempRow", false, FunctionBuilder.buildFunction_FillRow));
            _sbBase.AppendLine(_buildFunction("FillProcedure", "public", "Xy.Data.Procedure", "Xy.Data.Procedure inItem", false, FunctionBuilder.buildFunction_FillProcedure));
            //_sbBase.AppendLine(_buildFunction("toStringCollection", "public", "System.Collections.Specialized.NameValueCollection", string.Empty, false, FunctionBuilder.buildFunction_toStringCollection));
            _sbBase.AppendLine(_buildFunction("GetAttributesName", "public", "string[]", string.Empty, false, FunctionBuilder.buildFunction_GetAttributesName));
            _sbBase.AppendLine(_buildFunction("GetAttributesValue", "public", "object", "string inName", false, FunctionBuilder.buildFunction_GetAttributesValue));
            _sbBase.AppendLine(_buildFunction("GetXml", "public", "System.Xml.XPath.XPathDocument", string.Empty, false, FunctionBuilder.buildFunction_GetXml));
            _sbBase.AppendLine(_buildFunction("CreateEmptyTable", "public", "System.Data.DataTable", string.Empty, true, FunctionBuilder.buildFunction_CreateEmptyTable));
            _sbBase.AppendLine("    }");
            _sbBase.AppendLine("}");
            _sbBase.AppendLine();
            _sbBase.AppendLine();
            _sbBase.AppendLine("#region SQL Help Code");
            _sbBase.AppendLine("/*  --you can use below code in your sql procedures");
            _sbBase.Append(SectionBuilder.buildSQLHelpCode(DataGet.Get(DataGet.ContentType.TabelInfo, ClassName)));
            _sbBase.AppendLine("*/");
            _sbBase.AppendLine("#endregion");
            _sbBase.AppendLine();
            _sbBase.AppendLine("#region C# Help Code");
            _sbBase.AppendLine("/*  --you can use below code in your sql procedures");
            _sbBase.Append(SectionBuilder.buildCSharpHelpCode(DataGet.Get(DataGet.ContentType.TabelInfo, ClassName)));
            _sbBase.AppendLine("*/");
            _sbBase.AppendLine("#endregion");

            if (System.IO.Directory.Exists(File.FilePath)) { System.IO.Directory.CreateDirectory(File.FilePath); }
            System.IO.FileStream _fs;
            System.IO.StreamWriter _sw;
            using (_fs = new System.IO.FileStream(File.FilePath + ClassName + "_XYBase.cs", System.IO.FileMode.Create)) {
                using (_sw = new System.IO.StreamWriter(_fs)) {
                    _sw.Write(_sbBase.ToString());
                    _sw.Close();
                }
                _fs.Close();
            }
        }

        public void CreateFunctionFile() {
            StringBuilder _sbFunction = new StringBuilder();
            _sbFunction.AppendLine(StaticSection.InsertCopyright());
            _sbFunction.AppendLine(StaticSection.InsertUsing());
            _sbFunction.AppendLine();
            _sbFunction.AppendLine("namespace " + File.Namespace + " {");
            _sbFunction.AppendLine("    public partial class " + ClassName + " {");
            _sbFunction.AppendLine();
            _sbFunction.AppendLine(_buildFunction("RegisterProcedures", "protected", "void", string.Empty, true, FunctionBuilder.buildFunction_RegisterProcedures));
            _sbFunction.AppendLine();
            //_sbFunction.AppendLine(SectionBuilder.buildProcedureFunction(File, ClassName));
            foreach (CodeEntity.Procedure _procedure in _table.ProcedureList) {
                _sbFunction.AppendLine(_buildFunction(_procedure.Name, "public", _procedure.GetCSharpReturn(), _procedure.GetCSharpParameter(), true, _procedure.GetCSharpBuildFunction(), _procedure));
            }
            _sbFunction.AppendLine();
            _sbFunction.AppendLine("    }");
            _sbFunction.AppendLine("}");

            if (System.IO.Directory.Exists(File.FilePath)) { System.IO.Directory.CreateDirectory(File.FilePath); }
            System.IO.FileStream _fs;
            System.IO.StreamWriter _sw;
            using (_fs = new System.IO.FileStream(File.FilePath + ClassName + "_XYFunction.cs", System.IO.FileMode.Create)) {
                using (_sw = new System.IO.StreamWriter(_fs)) {
                    _sw.Write(_sbFunction.ToString());
                    _sw.Close();
                }
                _fs.Close();
            }
        }
        public void CreateDefaultFile() {
            if (System.IO.File.Exists(File.FilePath + ClassName + ".cs")) return;

            StringBuilder _sb = new StringBuilder();
            _sb.AppendLine(StaticSection.InsertUsing());
            _sb.AppendLine();
            _sb.AppendLine("namespace " + File.Namespace + " {");
            _sb.AppendLine("    public partial class " + ClassName + " : Xy.Data.IDataModel {");
            _sb.AppendLine();
            _sb.AppendLine("        static void RegisterEvents() { }");
            _sb.AppendLine();
            _sb.AppendLine("    }");
            _sb.AppendLine("}");
            if (!System.IO.Directory.Exists(File.FilePath)) { System.IO.Directory.CreateDirectory(File.FilePath); }
            System.IO.FileStream _fs;
            System.IO.StreamWriter _sw;
            using (_fs = new System.IO.FileStream(File.FilePath + ClassName + ".cs", System.IO.FileMode.Create)) {
                using (_sw = new System.IO.StreamWriter(_fs)) {
                    _sw.Write(_sb.ToString());
                    _sw.Close();
                }
                _fs.Close();
            }
        }

        private string _buildFunction(string name, string access, string returnType, string parameter, bool isStatic, BuildFunction codeBuildingFunction, params object[] param) {
            if(codeBuildingFunction == null) throw new NotImplementedException();
            if (!string.IsNullOrEmpty(_originalContent)) {
                StringBuilder _tempRegBuilder = new StringBuilder();
                _tempRegBuilder.Append("\\s+" + returnType.Replace(".", "\\.").Replace("[", "\\[").Replace("]", "\\]") + "\\s+" + name + "\\s*\\(\\s*");
                string[] _tempParameter = parameter.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < _tempParameter.Length; i++) {
                    string[] _sub = _tempParameter[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string _ptype;
                    if (string.Compare(_sub[0], "ref", true) == 0 || string.Compare(_sub[0], "out", true) == 0) {
                        _ptype = _sub[1];
                    } else {
                        _ptype = _sub[0];
                    }
                    _tempRegBuilder.Append("(ref\\s|out\\s)?([_a-zA-Z0-9]\\.)*" + _ptype.Replace(".", "\\.").Replace("[", "\\[").Replace("]", "\\]"));
                    _tempRegBuilder.Append("\\s+");
                    _tempRegBuilder.Append("[_a-zA-Z0-9=,\\s]+");
                    if (i == _tempParameter.Length - 1) {
                        _tempRegBuilder.Append("\\s*");
                    } else {
                        _tempRegBuilder.Append("\\s+");
                    }
                }
                _tempRegBuilder.Append("\\)\\s*\\{");
                System.Text.RegularExpressions.Regex _reg = new System.Text.RegularExpressions.Regex(_tempRegBuilder.ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (_reg.IsMatch(_originalContent)) {
                    string _originalFunction = _reg.Match(_originalContent).Value.TrimEnd(new char[] { '{', ' ' });
                    System.Windows.Forms.DialogResult _dr = System.Windows.Forms.MessageBox.Show(
                        string.Format("\"{0}\" already existed" + Environment.NewLine + "do you still want write \"{1}\" to the file?", _originalFunction, access + " " + (isStatic ? "static " : string.Empty) + returnType + " " + name + "(" + parameter + ")"), "Confirm", System.Windows.Forms.MessageBoxButtons.YesNo);
                    if (_dr == System.Windows.Forms.DialogResult.No) return string.Empty;
                }
            }
            return codeBuildingFunction(this, name, access, returnType, parameter, isStatic, param);
        }

        public delegate string BuildFunction(FileBuilder fileBuilder, string name, string access, string returnType, string parameter, bool isStatic, params object[] param);

        //public void CreateBaseFile(FileHandle inFile, string inClassName) {
        //    StringBuilder _sbBase = new StringBuilder();
        //    _sbBase.AppendLine(StaticSection.InsertCopyright());
        //    _sbBase.AppendLine(StaticSection.InsertUsing());
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine("namespace " + inFile.Namespace + " {");
        //    _sbBase.AppendLine("    public partial class " + inClassName + " : Xy.Data.IDataModel {");
        //    _sbBase.AppendLine(SectionBuilder.buildAttribute(DataGet.Get(DataGet.ContentType.TabelInfo,inClassName)));
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine("        private static string R(string name) {");
        //    _sbBase.AppendLine("            return \"" + (inFile.Namespace + "." + inClassName + ".").Replace('.', '_') + "\" + name;");
        //    _sbBase.AppendLine("        }");
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine("        private static Dictionary<string, Xy.Data.Procedure> _procedures;");
        //    _sbBase.AppendLine("        private static void AddProcedure(Xy.Data.Procedure _inValue) {");
        //    _sbBase.AppendLine("            if (_procedures == null) _procedures = new Dictionary<string, Xy.Data.Procedure>();");
        //    _sbBase.AppendLine("            _procedures.Add(_inValue.Name, _inValue);");
        //    _sbBase.AppendLine("        }");
        //    _sbBase.AppendLine("        private static Xy.Data.Procedure GetProcedure(string name) {");
        //    _sbBase.AppendLine("            if (_procedures == null) _procedures = new Dictionary<string,Xy.Data.Procedure>();");
        //    _sbBase.AppendLine("            if (_procedures.ContainsKey(name)) {");
        //    _sbBase.AppendLine("                return _procedures[name].Clone();");
        //    _sbBase.AppendLine("            } else {");
        //    _sbBase.AppendLine("                throw new Exception(string.Format(\"can not found \\\"{0}\\\" in procedure list\", name));");
        //    _sbBase.AppendLine("            }");
        //    _sbBase.AppendLine("        }");
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine("        static " + inClassName + "() {");
        //    _sbBase.AppendLine("            RegisterProcedures();");
        //    _sbBase.AppendLine("            RegisterEvents();");
        //    _sbBase.AppendLine("        }");
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine("        partial void Fill_Ext(System.Data.DataRow _tempRow);");
        //    _sbBase.AppendLine("        public void Fill(System.Data.DataRow _tempRow) {");
        //    _sbBase.AppendLine("            System.Data.DataColumnCollection cols = _tempRow.Table.Columns;");
        //    _sbBase.Append(SectionBuilder.buildFill(DataGet.Get(DataGet.ContentType.TabelInfo,inClassName)));
        //    _sbBase.AppendLine("            Fill_Ext(_tempRow);");
        //    _sbBase.AppendLine("        }");
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine("        partial void toStringCollection_Ext(System.Collections.Specialized.NameValueCollection _nvc);");
        //    _sbBase.AppendLine("        public System.Collections.Specialized.NameValueCollection toStringCollection() {");
        //    _sbBase.AppendLine("            System.Collections.Specialized.NameValueCollection _nvc = new System.Collections.Specialized.NameValueCollection();");
        //    _sbBase.Append(SectionBuilder.buildtoStringCollection(DataGet.Get(DataGet.ContentType.TabelInfo,inClassName)));
        //    _sbBase.AppendLine("            toStringCollection_Ext(_nvc);");
        //    _sbBase.AppendLine("            return _nvc;");
        //    _sbBase.AppendLine("        }");
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine("        partial void FillProcedure_Ext(Xy.Data.Procedure item);");
        //    _sbBase.AppendLine("        public Xy.Data.Procedure FillProcedure(Xy.Data.Procedure item) {");
        //    _sbBase.Append(SectionBuilder.buildFillProcedure(DataGet.Get(DataGet.ContentType.TabelInfo, inClassName)));
        //    _sbBase.AppendLine("            FillProcedure_Ext(item);");
        //    _sbBase.AppendLine("            return item;");
        //    _sbBase.AppendLine("        }");
        //    _sbBase.AppendLine("    }");
        //    _sbBase.AppendLine("}");
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine("#region SQL Help Code");
        //    _sbBase.AppendLine("/*  --you can use below code in your sql procedures");
        //    _sbBase.Append(SectionBuilder.buildSQLHelpCode(DataGet.Get(DataGet.ContentType.TabelInfo,inClassName)));
        //    _sbBase.AppendLine("*/");
        //    _sbBase.AppendLine("#endregion");
        //    _sbBase.AppendLine();
        //    _sbBase.AppendLine("#region C# Help Code");
        //    _sbBase.AppendLine("/*  --you can use below code in your sql procedures");
        //    _sbBase.Append(SectionBuilder.buildCSharpHelpCode(DataGet.Get(DataGet.ContentType.TabelInfo,inClassName)));
        //    _sbBase.AppendLine("*/");
        //    _sbBase.AppendLine("#endregion");

        //    if (System.IO.Directory.Exists(inFile.FilePath)) { System.IO.Directory.CreateDirectory(inFile.FilePath); }
        //    System.IO.FileStream  _fs;
        //    System.IO.StreamWriter  _sw;
        //    using (_fs = new System.IO.FileStream(inFile.FilePath + inClassName + "_XYBase.cs", System.IO.FileMode.Create)) {
        //        using (_sw = new System.IO.StreamWriter(_fs)) {
        //            _sw.Write(_sbBase.ToString());
        //            _sw.Close();
        //        }
        //        _fs.Close();
        //    }
        //}

        //public void CreateFunctionFile(FileHandle inFile, string inClassName) {
        //    StringBuilder _sbFunction = new StringBuilder();
        //    _sbFunction.AppendLine(StaticSection.InsertCopyright());
        //    _sbFunction.AppendLine(StaticSection.InsertUsing());
        //    _sbFunction.AppendLine();
        //    _sbFunction.AppendLine("namespace " + inFile.Namespace + " {");
        //    _sbFunction.AppendLine("    public partial class " + inClassName + " : Xy.Data.IDataModel {");
        //    _sbFunction.AppendLine();
        //    _sbFunction.AppendLine("        protected static void RegisterProcedures() {");
        //    _sbFunction.AppendLine();
        //    _sbFunction.Append(SectionBuilder.buildRegisterProcedure(inFile, inClassName));
        //    _sbFunction.AppendLine("        }");
        //    _sbFunction.AppendLine();
        //    _sbFunction.Append(SectionBuilder.buildProcedureFunction(inFile, inClassName));
        //    _sbFunction.AppendLine();
        //    _sbFunction.AppendLine("    }");
        //    _sbFunction.AppendLine("}");            

        //    if (System.IO.Directory.Exists(inFile.FilePath)) { System.IO.Directory.CreateDirectory(inFile.FilePath); }
        //    System.IO.FileStream _fs;
        //    System.IO.StreamWriter _sw;
        //    using (_fs = new System.IO.FileStream(inFile.FilePath + inClassName + "_XYFunction.cs", System.IO.FileMode.Create)) {
        //        using (_sw = new System.IO.StreamWriter(_fs)) {
        //            _sw.Write(_sbFunction.ToString());
        //            _sw.Close();
        //        }
        //        _fs.Close();
        //    }
        //}

        //public void CreateDefaultFile(FileHandle inFile, string inClassName) {
        //    if (System.IO.File.Exists(inFile.FilePath + inClassName + ".cs")) return;

        //    StringBuilder _sb = new StringBuilder();
        //    _sb.AppendLine(StaticSection.InsertUsing());
        //    _sb.AppendLine();
        //    _sb.AppendLine("namespace " + inFile.Namespace + " {");
        //    _sb.AppendLine("    public partial class " + inClassName + " : Xy.Data.IDataModel {");
        //    _sb.AppendLine();
        //    _sb.AppendLine("        static void RegisterEvents() { }");
        //    _sb.AppendLine();
        //    _sb.AppendLine("    }");
        //    _sb.AppendLine("}");

        //    if (!System.IO.Directory.Exists(inFile.FilePath)) { System.IO.Directory.CreateDirectory(inFile.FilePath); }
        //    System.IO.FileStream _fs;
        //    System.IO.StreamWriter _sw;
        //    using (_fs = new System.IO.FileStream(inFile.FilePath + inClassName + ".cs", System.IO.FileMode.Create)) {
        //        using (_sw = new System.IO.StreamWriter(_fs)) {
        //            _sw.Write(_sb.ToString());
        //            _sw.Close();
        //        }
        //        _fs.Close();
        //    }
        //}
    }
}
