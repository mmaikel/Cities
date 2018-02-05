﻿using System;
using System.Data;
using System.Data.OleDb;

namespace Cities_console {
    public class DatabaseConnection {
        
        private String connectionString;
        private String dbProvider;
        private String dataSourcePath;
        private String persistSecurityInfo;
        private String password;
        private OleDbConnection conn;


        public DatabaseConnection(String sourcePath, 
                                  String password, 
                                  String provider="Microsoft.Jet.OLEDB.4.0;", 
                                  String securityInfo="True;") {
            this.dataSourcePath = sourcePath;
            this.password = password;
            this.dbProvider = provider;
            this.persistSecurityInfo = securityInfo;
            this.conn = new OleDbConnection();
            this.connectionString = "Provider=" + this.dbProvider +
                                    "Data Source=" + this.dataSourcePath +
                                    "Persist Security Info=" + this.persistSecurityInfo +
                                    "Jet OLEDB:Database Password=" + this.password;
            conn.ConnectionString = this.connectionString;
        }


        public Boolean Connect() {
            try {
                conn.Open();
                return true;
            }
            catch(Exception e) {
                if(e is InvalidOperationException) {
                    Console.WriteLine("Connection FAILED after opening.\n\t" + e.Message);
                }
                else if(e is OleDbException) {
                    Console.WriteLine("Opening connection FAILED.\n\t" + e.Message);
                } 
            }

            return false;
        }


        public DataTable Execute(String queryCommand) {
            if (conn.State == ConnectionState.Open) {

                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = queryCommand;

                try {
                    OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    DataTable table = new DataTable();
                    table.Load(reader);
                    return table;
                }
                catch (InvalidOperationException e) {
                    Console.WriteLine("Execution of command FAILED." + e.Message);
                }

            }
            else {
                Console.WriteLine("Connection is not opened.");
            }

            return null;
        }
    }
}
