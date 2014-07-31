using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.CodeHelper {
    public class CodeTransform {
        public static string toCsharp(string originalType) {
            switch (originalType) {
                case "tinyint":
                    return "byte";
                case "smallint":
                    return "short";
                case "int":
                    return "int";
                case "bigint":
                    return "long";
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                    return "string";
                case "bit":
                    return "bool";
                case "date":
                case "datetime":
                    return "DateTime";
                case "decimal":
                    return "decimal";
                case "float":
                    return "float";
            }
            return "un_toCsharp_" + originalType;
        }

        public static string toCsharpClass(string originalType) {
            switch (originalType) {
                case "tinyint":
                    return "Byte";
                case "smallint":
                    return "Int16";
                case "int":
                    return "Int32";
                case "bigint":
                    return "Int64";
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                    return "String";
                case "bit":
                    return "Boolean";
                case "date":
                case "datetime":
                    return "DateTime";
                case "decimal":
                    return "Decimal";
                case "float":
                    return "Single";
            }
            return "un_toCsharpClass_" + originalType;
        }

        /// <summary>
        /// convert a CSharp type to another CSharp type
        /// </summary>
        /// <param name="original">string of original type name</param>
        /// <param name="target">string of targert type name</param>
        /// <param name="template">template in the convert</param>
        /// <returns></returns>
        public static string TypeConvert(string original, string target, string template) {
            if (string.Compare(original, target) == 0) return template;
            else
                switch (target) {
                    case "byte":
                        return "Convert.ToByte(" + template + ")";
                    case "short":
                    case "Int16":
                        return "Convert.ToInt16(" + template + ")";
                    case "int":
                    case "Int32":
                        return "Convert.ToInt32(" + template + ")";
                    case "long":
                    case "Int64":
                        return "Convert.ToInt64(" + template + ")";
                    case "string":
                        return "Convert.ToString(" + template + ")";
                    case "bool":
                        return "Convert.ToBoolean(" + template + ")";
                    case "DateTime":
                        return "Convert.ToDateTime(" + template + ")";
                    case "decimal":
                        return "Convert.ToDecimal(" + template + ")";
                    case "float":
                        return "Convert.ToSingle(" + template + ")";
                    default:
                        return "Convert.Unknow_" + original + "_to_" + target + "(" + template + ")";

                }
        }

        //public static string DBConvert(string originalType, string targetType, string value) {
        //    switch (targetType) {
        //        case "tinyint":
        //            if (originalType == "byte") return value;
        //            return "Convert.ToByte(" + value + ")";
        //        case "smallint":
        //            if (originalType == "short") return value;
        //            return "Convert.ToInt16(" + value + ")";
        //        case "int":
        //            if (originalType == "int") return value;
        //            return "Convert.ToInt32(" + value + ")";
        //        case "bigint":
        //            if (originalType == "long") return value;
        //            return "Convert.ToInt64(" + value + ")";
        //        case "varchar":
        //        case "nvarchar":
        //        case "text":
        //        case "ntext":
        //            if (originalType == "string") return value;
        //            return "Convert.ToString(" + value + ")";
        //        case "bit":
        //            return "Convert.ToBoolean(" + value + ")";
        //        case "date":
        //        case "datetime":
        //            return "Convert.ToDateTime(" + value + ")";
        //        case "decimal":
        //            return "Convert.ToDecimal(" + value + ")";
        //    }
        //    return "Convert.Un_toCsharpConvert_" + originalType + "_to_" + targetType + "(" + value + ")";
        //}

        //public static string CharpConvert(string originalType, string targetType, string value) {
        //    switch (targetType) {
        //        case "tinyint":
        //            if (originalType == "byte") return value;
        //            return "Convert.ToByte(" + value + ")";
        //        case "smallint":
        //            if (originalType == "short") return value;
        //            return "Convert.ToInt16(" + value + ")";
        //        case "int":
        //            if (originalType == "int") return value;
        //            return "Convert.ToInt32(" + value + ")";
        //        case "bigint":
        //            if (originalType == "long") return value;
        //            return "Convert.ToInt64(" + value + ")";
        //        case "varchar":
        //        case "nvarchar":
        //        case "text":
        //        case "ntext":
        //            if (originalType == "string") return value;
        //            return "Convert.ToString(" + value + ")";
        //        case "bit":
        //            return "Convert.ToBoolean(" + value + ")";
        //        case "date":
        //        case "datetime":
        //            return "Convert.ToDateTime(" + value + ")";
        //        case "decimal":
        //            return "Convert.ToDecimal(" + value + ")";
        //    }
        //    return "Convert.Un_toCsharpConvert_" + originalType + "_to_" + targetType + "(" + value + ")";
        //}

        public static string toSqlDeclare(string originalType, string length, string Precision, string Scale) {
            switch (originalType) {
                case "binary":
                case "varchar":
                case "char":
                case "datetime2":
                case "datetimeoffset":
                case "nchar":
                case "time":
                case "varbinary":
                    return originalType + "(" + length + ")";
                case "nvarchar":
                    return originalType + "(" + Precision + ")";
                case "numeric":
                case "decimal":
                    return originalType + "(" + Precision + "," + Scale + ")";
                default:
                    return originalType;
            }
        }

        public static string toDbType(string originalType) {
            switch (originalType) {
                case "tinyint":
                    return "System.Data.DbType.Byte";
                case "smallint":
                case "short":
                    return "System.Data.DbType.Int16";
                case "int":
                    return "System.Data.DbType.Int32";
                case "bigint":
                    return "System.Data.DbType.Int64";
                case "nvarchar":
                case "varchar":
                case "text":
                case "ntext":
                    return "System.Data.DbType.String";
                case "bit":
                    return "System.Data.DbType.Boolean";
                case "date":
                case "datetime":
                    return "System.Data.DbType.DateTime";
                case "decimal":
                    return "System.Data.DbType.Decimal";
                case "float":
                    return "System.Data.DbType.Single";
            }
            return "Un_toDbType_" + originalType;
        }
    }
}
