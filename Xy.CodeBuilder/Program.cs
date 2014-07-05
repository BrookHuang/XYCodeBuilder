using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Xy.CodeBuilder {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _getConnection = new GetConnection();
            _main = new Main();
            Application.Run(new GetConnection());
        }

        private static GetConnection _getConnection;
        private static Main _main;

        public static void showGetConnection() {
            _getConnection.Show();
            _main.Hide();
        }

        public static void showMain() {
            _getConnection.Hide();
            _main.Show();
        }

        public static void shutdown() {
            _main.Close();
            _getConnection.Close();
            Application.Exit();
        }
    }
}
