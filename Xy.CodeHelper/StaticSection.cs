using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.CodeHelper {
    public class StaticSection {
        public static string InsertCopyright() {
            StringBuilder _sb = new StringBuilder();
            _sb.AppendLine("/****************************************************************************");
            _sb.AppendLine(" * ");
            _sb.AppendLine(" *  all code on the blow is builded by XyFrameDataModuleBuilder 1.0.0.0 version");
            _sb.AppendLine(" * ");
            _sb.AppendLine(" ****************************************************************************/");
            return _sb.ToString();
        }

        public static string InsertUsing() {
            StringBuilder _sb = new StringBuilder();
            _sb.AppendLine("using System;");
            _sb.AppendLine("using System.Collections.Generic;");
            _sb.AppendLine("using System.Text;");
            return _sb.ToString();
        }
    }
}
