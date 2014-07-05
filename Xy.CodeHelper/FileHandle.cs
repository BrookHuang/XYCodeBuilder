using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.CodeHelper {
    public sealed class FileHandle {
        string _fullPath;
        public string FullPath { get { return _fullPath; } }
        string _namespace;
        public string Namespace { get { return _namespace; } }
        string _fileName;
        public string FileName { get { return _fileName; } }
        string _filePath;
        public string FilePath { get { return _filePath; } }

        public FileHandle(string inFilePath, string inNamespace) {
            _fullPath = inFilePath;
            _namespace = inNamespace;
            _fileName = _fullPath.Substring(_fullPath.LastIndexOf('\\') + 1, _fullPath.LastIndexOf('.') - _fullPath.LastIndexOf('\\') - 1);
            _filePath = _fullPath.Substring(0, _fullPath.LastIndexOf('\\') + 1);
        }

        public void CreateFile(string inClassName) {
            CreateFile(inClassName, false);
        }

        public void CreateFile(string inClassName, bool IsAppend) {
            FileBuilder _fb = new FileBuilder(this, inClassName);
            _fb.CreateDefaultFile();
            _fb.CreateBaseFile();
            _fb.CreateFunctionFile();
            if (IsAppend && _fullPath.ToLower().Contains("csproj")) {
                ModifyCsprojFile(inClassName);
            }
        }

        public void ModifyCsprojFile(string inClassName) {
            System.Xml.XmlDocument _xml = new System.Xml.XmlDocument();
            _xml.Load(_fullPath);
            if (_xml.GetElementsByTagName("Compile").Count > 0) {
                System.Xml.XmlNode _xn = _xml.GetElementsByTagName("Compile").Item(0).ParentNode;
                bool _hasEntitycs = false;
                bool _hasEntityBasecs = false;
                bool _hasEntityBaseDependent = false;
                bool _hasEntityFunctioncs = false;
                bool _hasEntityFunctionDependent = false;
                System.Xml.XmlNode _entitynode = null;
                System.Xml.XmlNode _entityBaseNode = null;
                System.Xml.XmlNode _entityFunctionNode = null;
                foreach (System.Xml.XmlNode _item in _xn.ChildNodes) {
                    if (_item.Attributes["Include"] != null && string.Compare(_item.Attributes["Include"].Value, inClassName + ".cs", true) == 0) {
                        _hasEntitycs = true;
                        _entitynode = _item;
                    }
                    if (_item.Attributes["Include"] != null && string.Compare(_item.Attributes["Include"].Value, inClassName + "_XYBase.cs", true) == 0) {
                        _hasEntityBasecs = true;
                        _entityBaseNode = _item;
                        if (_item.ChildNodes.Count > 0
                            && string.Compare(_item.ChildNodes.Item(0).Name, "DependentUpon", true) == 0) {
                            _hasEntityBaseDependent = true;
                        }
                    }
                    if (_item.Attributes["Include"] != null && string.Compare(_item.Attributes["Include"].Value, inClassName + "_XYFunction.cs", true) == 0) {
                        _hasEntityFunctioncs = true;
                        _entityFunctionNode = _item;
                        if (_item.ChildNodes.Count > 0
                            && string.Compare(_item.ChildNodes.Item(0).Name, "DependentUpon", true) == 0) {
                            _hasEntityFunctionDependent = true;
                        }
                    }
                }
                if (_hasEntitycs && _hasEntityBasecs && _hasEntityBaseDependent && _hasEntityFunctioncs && _hasEntityFunctionDependent) return;
                if (_entitynode != null) { _xn.RemoveChild(_entitynode); }
                if (_entityBaseNode != null) { _xn.RemoveChild(_entityBaseNode); }
                if (_entityFunctionNode != null) { _xn.RemoveChild(_entityFunctionNode); }

                System.Xml.XmlElement _newEntity = _xml.CreateElement("Compile", _xml.DocumentElement.NamespaceURI);
                _newEntity.SetAttribute("Include", inClassName + ".cs");

                System.Xml.XmlElement _newEntityBase = _xml.CreateElement("Compile", _xml.DocumentElement.NamespaceURI);
                _newEntityBase.SetAttribute("Include", inClassName + "_XYBase.cs");
                System.Xml.XmlElement _newEntityBaseDependent = _xml.CreateElement("DependentUpon", _xml.DocumentElement.NamespaceURI);
                _newEntityBaseDependent.InnerText = inClassName + ".cs";
                _newEntityBase.AppendChild(_newEntityBaseDependent);

                System.Xml.XmlElement _newEntityFunction = _xml.CreateElement("Compile", _xml.DocumentElement.NamespaceURI);
                _newEntityFunction.SetAttribute("Include", inClassName + "_XYFunction.cs");
                System.Xml.XmlElement _newEntityFunctionDependent = _xml.CreateElement("DependentUpon", _xml.DocumentElement.NamespaceURI);
                _newEntityFunctionDependent.InnerText = inClassName + ".cs";
                _newEntityFunction.AppendChild(_newEntityFunctionDependent);

                _xn.AppendChild(_newEntity);
                _xn.AppendChild(_newEntityBase);
                _xn.AppendChild(_newEntityFunction);
                _xml.Save(_fullPath);
            }
        }
    }
}
