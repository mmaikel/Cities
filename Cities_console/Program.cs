﻿using System;
using System.Data;
using System.Data.OleDb;


namespace Cities_console
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Data data = new Data();
            String path = "./db/Miasta.mdb";

            DatabaseConnection dbConn = new DatabaseConnection(path, "");
            if (dbConn.Connect()) {
                Console.WriteLine("DB connection established.");

                DataTable regionsTable = dbConn.Execute("SELECT * FROM `regions`");
                if (regionsTable != null)
                {
                    
                    foreach(DataRow row in regionsTable.Rows) {
                        int regionID = (int)row["ID"];
                        String name = row["Wojewodztwo"].ToString();
                        Region region = new Region(name, regionID);
                        data.addRegion(regionID, region);
                    }
                }

                DataTable citiesTable = dbConn.Execute("SELECT * FROM `city`");
                if (citiesTable != null)
                {

                    foreach (DataRow row in citiesTable.Rows)
                    {
                        int cityID = (int)row["ID"];
                        String name = row["Miasto"].ToString();
                        Double lon = (Double)row["Dl"];
                        Double lat = (Double)row["Szer"];
                        int regionID = (int)row["ID_woj"];

                        City city = new City(name, lon, lat, data.getRegion(regionID));
                        data.addCity(city);
                    }
                }

                //TODO do smth with cities and regions in 'data'.
            }

        }
    }
}
