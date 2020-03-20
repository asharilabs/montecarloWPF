using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace Montecarlo_FORM
{
    class DataMentahViewModel
    {
        private MySqlConnection con;
        private MySqlDataAdapter adp;
        private MySqlCommand com;

        MonteCarlo mc;

        private List<int> dataTunggal = new List<int>();
        public List<int> DataTunggal
        {
            get { return this.dataTunggal; }
            set { this.dataTunggal = value; }
        }

        public DataMentahViewModel()
        {
            con = Connection.InitConnection();            
        }
        public string GetLastMinggu()
        {
            string result = "";

            string query = "SELECT mingguke FROM m_penjualanlaptop ORDER BY mingguke DESC LIMIT 0,1";
            com = new MySqlCommand(query, con);

            try
            {
                con.Open();

                DataTable dt = new DataTable();
                adp = new MySqlDataAdapter(com);
                adp.Fill(dt);

                if( dt.Rows.Count == 1)
                {
                    result = dt.Rows[0][0].ToString();
                }

                con.Close();
            }
            catch (Exception ex)
            {

            }

            int lastIndex = int.Parse(result);
            lastIndex++;

            result = lastIndex.ToString();

            return result;
        }
        public List<DataMentah> GetAllDataMentah()
        {            
            List<DataMentah> finalList = new List<DataMentah>();
            this.dataTunggal.Clear();

            string query = "SELECT * FROM m_penjualanlaptop ORDER BY mingguke DESC";
            com = new MySqlCommand(query, con);

            try
            {
                con.Open();

                DataTable dt = new DataTable();
                adp = new MySqlDataAdapter(com);
                adp.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    DataMentah dm = new DataMentah(
                        dr[1].ToString(), 
                        dr[2].ToString());
                    finalList.Add(dm);

                    // input ke data tunggal
                    this.dataTunggal.Add(int.Parse(dr[2].ToString()));
                }
                
                // sorting data tunggal dari yang terkecil
                this.dataTunggal.Sort();
                
                con.Close();
            }
            catch (Exception ex)
            {
                
            }

            return finalList;
        }
        
        public bool InputDataBaru(string _mingguke, string _frekuensi)
        {
            bool isResult = false;

            string query = "INSERT INTO m_penjualanlaptop(mingguke, banyak) VALUES ('" + _mingguke + "', '" + _frekuensi + "')";
            com = new MySqlCommand(query, con);

            try
            {
                con.Open();

                if (com.ExecuteNonQuery() > 0)
                    isResult = true;

                con.Close();
            }
            catch (Exception ex)
            {

            }

            return isResult;
        }
    }
}