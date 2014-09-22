using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Xy.CodeBuilder {
    public partial class Main : Form {

        internal static System.Data.SqlClient.SqlConnection _con;

        public Main() {
            InitializeComponent();
        }

        public void SetConnection(System.Data.SqlClient.SqlConnection con) {
            _con = con;
            Xy.CodeHelper.DataGet.SetConnection(_con);
            label1.ForeColor = Color.Green;
            label1.Text = "connect on:" + _con.ConnectionString;
            label1.Visible = true;
            this.Init();
        }

        public void SetNamespace(string space){
            textBox1.Text = space + ".Entity";
        }

        private void Init() {
            DataTable _dt = Xy.CodeHelper.DataGet.Get(CodeHelper.DataGet.ContentType.AllTable);
            List<string> _tableName = new List<string>();
            foreach (DataRow _item in _dt.Rows) {
                if (string.Compare(_item["name"].ToString(), "sysdiagrams", true) != 0) {
                    _tableName.Add(_item["name"].ToString());
                }
            }
            listBox1.DataSource = _tableName;
            _con.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            ListBox _thisListBox = (ListBox)sender;
            if (_thisListBox.SelectedValue != null) {
                dataGridView1.DataSource = Xy.CodeHelper.DataGet.Get(CodeHelper.DataGet.ContentType.TabelInfo, _thisListBox.SelectedValue.ToString());
                label2.Text = "Field in the table '" + _thisListBox.SelectedValue.ToString() + "'";
                if (_thisListBox.SelectedIndices.Count == 1) {
                    textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf('.') + 1) + _thisListBox.SelectedValue.ToString();
                } else {
                    List<string> _selectedItems = new List<string>();
                    foreach (string _item in _thisListBox.SelectedItems) {
                        _selectedItems.Add(_item);
                    }
                    _selectedItems.Sort(string.Compare);
                    string _shortName = _selectedItems[0];
                    for (int i = 0; i < _selectedItems.Count; i++) {
                        if (_selectedItems[i].IndexOf(_shortName) != 0) {
                            if (_shortName.Length <= 1) return;
                            _shortName = _shortName.Remove(_shortName.Length - 1);
                            i = 0;
                        }
                    }
                    textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.LastIndexOf('.') + 1) + _shortName;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            SaveFileDialog _sfd = new SaveFileDialog();
            _sfd.AddExtension = true;
            _sfd.DefaultExt = "cs";
            _sfd.Filter = "C# code file(*.cs)|*.cs";
            if (listBox1.SelectedIndices.Count > 1) {
                _sfd.FileName = "Multi-files";
            } else {
                _sfd.FileName = listBox1.SelectedItem.ToString();
            }
            DialogResult _dr = _sfd.ShowDialog();
            if (_dr.ToString() == "OK") {
                Xy.CodeHelper.FileHandle _fh = new CodeHelper.FileHandle(_sfd.FileName, textBox1.Text);
                foreach (int _selectIndex in listBox1.SelectedIndices) {
                    _fh.CreateFile(listBox1.Items[_selectIndex].ToString());
                }
                MessageBox.Show("Create success!");
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            OpenFileDialog _sfd = new OpenFileDialog();
            _sfd.AddExtension = true;
            _sfd.DefaultExt = "csproj";
            _sfd.Filter = "C# project file(*.csproj)|*.csproj|All file(*.*)|*.*";
            DialogResult _dr = _sfd.ShowDialog();
            if (_dr.ToString() == "OK") {
                Xy.CodeHelper.FileHandle _fh = new CodeHelper.FileHandle(_sfd.FileName, textBox1.Text);
                foreach (int _selectIndex in listBox1.SelectedIndices) {
                    _fh.CreateFile(listBox1.Items[_selectIndex].ToString(), true);
                }
                MessageBox.Show("Append success!");
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e) {
            Program.shutdown();
        }

        private void button1_Click(object sender, EventArgs e) {
            Xy.CodeHelper.DataGet.Refresh();
            Init();
        }

        private void button4_Click(object sender, EventArgs e) {
            Xy.CodeHelper.ProcedureBuilder _pb = new CodeHelper.ProcedureBuilder(textBox1.Text);
            foreach (int _selectIndex in listBox1.SelectedIndices) {
                _pb.CreateProcedure(listBox1.Items[_selectIndex].ToString(), _con);
            }
            MessageBox.Show("Create success!");
        }
    }
}
