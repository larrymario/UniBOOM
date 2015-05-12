using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Uniboom.Utility {

    // CSVOperator.cs
    // Author: ScottFoH
    // For operating data from csv.

    public class CSVOperator
    {
        public void SaveCSV(DataTable dt, string path)
        {
            FileInfo fi = new FileInfo(path);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(path, System.IO.FileMode.Create,
                System.IO.FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);


            string dataLine = "";
            for(int i = 0; i < dt.Columns.Count; i++)
            {
                dataLine += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    dataLine += ",";
                }
            }
            sw.WriteLine(dataLine);

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                dataLine = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    dataLine += dt.Rows[i][j].ToString();
                    if (j < dt.Columns.Count - 1) 
                    {
                        dataLine += ",";
                    }
                }
                sw.WriteLine(dataLine);
            }

            sw.Close();
            fs.Close();

        }

        public DataTable ReadCSV(string path)
        {
            DataTable dt = new DataTable();
            Encoding encoding = Encoding.UTF8;
            FileStream fs = new FileStream(path, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);

            StreamReader sr = new StreamReader(fs, encoding);

            string dataLine = "";

            string[] currentLine = null;
            string[] tableHead = null;

            int columnCount = 0;
            bool isFirst = true;

            while ((dataLine = sr.ReadLine()) != null)
            {
                if (isFirst == true)
                {
                    tableHead = dataLine.Split(',');
                    columnCount = tableHead.Length;

                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                    isFirst = false;
                }
                else
                {
                    currentLine = dataLine.Split(',');
                    DataRow dr = dt.NewRow();

                    for (int i = 0; i < columnCount; i++)
                    {
                        dr[i] = currentLine[i];
                    }
                    dt.Rows.Add(dr);
                }

            }

            sr.Close();
            fs.Close();
            return dt;
        }

        public DataTable ReadCsvFromString(string content) {
            DataTable dt = new DataTable();

            string[] currentLine = null;
            string[] tableHead = null;
            int columnCount = 0;
            bool isFirst = true;

            string[] lineArray = content.Split('\n');
            int entryCount = lineArray.Length;
            for (int i = 0; i < entryCount; i++) {
                if (isFirst == true) {
                    tableHead = lineArray[i].Split(',');
                    columnCount = tableHead.Length;

                    for (int j = 0; j < columnCount; j++) {
                        DataColumn dc = new DataColumn(tableHead[j]);
                        dt.Columns.Add(dc);
                    }
                    isFirst = false;
                }
                else {
                    currentLine = lineArray[i].Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++) {
                        dr[j] = currentLine[j];
                    }
                    dt.Rows.Add(dr);
                }

            }
            
            return dt;
        }

        public List<string> GetPatternList(string content) {
            DataTable data = ReadCsvFromString(content);
            DataRow[] rows = data.Select();

            List<string> result = new List<string>();
            for (int i = 0; i < rows.Length; i++) {
                string pattern = rows[i]["Pattern"].ToString();
                result.Add(pattern);
            }
            return result;
        }
    }

}