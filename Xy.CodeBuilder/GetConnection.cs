using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Xy.CodeBuilder {
    public partial class GetConnection : Form {

        private System.Xml.XmlDocument _cfg;
        public GetConnection() {
            InitializeComponent();
            string _configPath = Application.StartupPath + "\\config.xml";
            if (!System.IO.File.Exists(_configPath)) {
                System.IO.FileStream _fs = System.IO.File.Create(_configPath);
                byte[] _buffer = System.Text.Encoding.UTF8.GetBytes("<config><connectionStrings></connectionStrings></config>");
                _fs.Write(_buffer, 0, _buffer.Length);
                _fs.Flush();
                _fs.Close();
            }
            _cfg = new System.Xml.XmlDocument();
            _cfg.Load(Application.StartupPath + "\\config.xml");
            DateTime mostRecentlyTime = DateTime.MinValue;
            foreach (System.Xml.XmlNode _xn in _cfg.SelectNodes("config/connectionStrings/Item")) {
                comboBox1.Items.Add(_xn.Attributes["name"].Value);
                if (_xn.Attributes["lastUseTime"] != null) {
                    DateTime tempTime = Convert.ToDateTime(_xn.Attributes["lastUseTime"].Value);
                    if (tempTime > mostRecentlyTime) {
                        mostRecentlyTime = tempTime;
                        comboBox1.SelectedItem = _xn.Attributes["name"].Value;
                    }

                }
            }
            if (comboBox1.Items.Count == 0) return;
            if (comboBox1.SelectedIndex == -1) {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            label2.Text = "Connecting...";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection();
            try {
                string constr = string.Format("Data Source={0};Database={1};User ID={2};Password={3}", textBox1.Text, textBox4.Text, textBox2.Text, textBox3.Text);
                con.ConnectionString = constr;
                con.Open();
                con.Close();
                label2.ForeColor = Color.Green;
                label2.Text = "Success";
                Main _main = new Main();
                foreach (System.Xml.XmlNode _xn in _cfg.SelectNodes("config/connectionStrings/Item")) {
                    if (string.Compare(_xn.Attributes["connectionString"].Value, constr, true) == 0) {
                        if (_xn.Attributes["lastUseTime"] == null) {
                            _xn.Attributes.Append(_cfg.CreateAttribute("lastUseTime"));
                        }
                        _xn.Attributes["lastUseTime"].Value = DateTime.Now.ToString();
                        _cfg.Save(Application.StartupPath + "\\config.xml");
                        _main.SetNamespace(textBox5.Text);
                        _main.SetConnection(con);
                        _main.Show();
                        this.Visible = false;
                        return;
                    }
                }
                System.Xml.XmlElement _newcon = _cfg.CreateElement("Item", _cfg.NamespaceURI);
                _newcon.SetAttribute("name", textBox2.Text + "@" + textBox1.Text + " use " + textBox4.Text);
                _newcon.SetAttribute("connectionString", constr);
                _newcon.SetAttribute("baseNamespace", textBox5.Text);
                _newcon.SetAttribute("lastUseTime", DateTime.Now.ToString());
                _cfg.SelectSingleNode("config/connectionStrings").AppendChild(_newcon);
                _cfg.Save(Application.StartupPath + "\\config.xml");
                _main.SetNamespace(textBox5.Text);
                _main.SetConnection(con);
                _main.Show();
                this.Visible = false;
            } catch (Exception ex) {
                label2.ForeColor = Color.Red;
                label2.Text = ex.Message;
            } finally {
                con.Close();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                button1_Click(sender, new EventArgs());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            System.Xml.XmlNode _selectedNode = _cfg.SelectSingleNode(string.Format(@"config/connectionStrings/Item[@name='{0}']", comboBox1.SelectedItem.ToString()));
            string _setConStr = _selectedNode.Attributes["connectionString"].Value;
            System.Text.RegularExpressions.Regex _reg = new System.Text.RegularExpressions.Regex(@"Data Source=(?<Host>.+);database=(?<Database>.+);User ID=(?<Username>.+);Password=(?<Password>.+)", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (_reg.IsMatch(_setConStr)) {
                textBox1.Text = _reg.Match(_setConStr).Groups["Host"].Value;
                textBox2.Text = _reg.Match(_setConStr).Groups["Username"].Value;
                textBox3.Text = _reg.Match(_setConStr).Groups["Password"].Value;
                textBox4.Text = _reg.Match(_setConStr).Groups["Database"].Value;
                
            }
            textBox5.Text = _selectedNode.Attributes["baseNamespace"].Value;
        }

        private void button2_Click(object sender, EventArgs e) {
            if (comboBox1.SelectedIndex < 0) return;
            _cfg.SelectSingleNode("config/connectionStrings").RemoveChild(_cfg.SelectSingleNode(string.Format(@"config/connectionStrings/Item[@name='{0}']", comboBox1.SelectedItem.ToString())));
            _cfg.Save(Application.StartupPath + "\\config.xml");
            comboBox1.Items.Remove(comboBox1.SelectedItem);
            if (comboBox1.Items.Count > 0) {
                comboBox1.SelectedIndex = 0;

            }
        }
    }
}
