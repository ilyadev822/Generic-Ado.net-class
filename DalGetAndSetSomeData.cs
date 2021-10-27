using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace It_tools
{
    class DalGetAndSetSomeData : ConnString
    {
        private  DataSet returnData = new DataSet();
        private  MySqlCommand cmd = new MySqlCommand(); 
        private  MySqlDataAdapter adap = new MySqlDataAdapter();
        
      
       private   MySqlParameter ParamBuilder(MySqlParameter[] @params,string[] paramname, object[] paramval)
        {
            int leng = paramname.Length;
            for (int i = 0; i < leng; i++)
            {
                if (paramval[i] is Int32)
                {
                    @params[i] = new MySqlParameter(paramname[i], MySqlDbType.Int32);
                    @params[i].Value = paramval[i];
                }
                //else
                //{
                //    @params[i] = new MySqlParameter(paramname[i], MySqlDbType.VarChar);
                //    @params[i].Value = paramval[i];
                //}
             else   if(paramval[i] is DateTime)
                {
                    @params[i] = new MySqlParameter(paramname[i], MySqlDbType.Date);
                    @params[i].Value = paramval[i];
                }
                else
                {
                  @params[i] = new MySqlParameter(paramname[i], MySqlDbType.VarChar);
                 @params[i].Value = paramval[i];
               }
            }
            return @params[leng-1];
        } 

      public DataSet GetSomeData(string str_prod)
        {   
            SetConnStrForMysql();
            connectTomysql.Open();
            cmd.Connection = connectTomysql;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = str_prod;
            adap.SelectCommand = cmd; 
            adap.Fill(returnData);
            connectTomysql.Close();
            return returnData ;
        }

        public DataSet GetSomeData(string str_prod, string[] paramname, object[] paramval)
        {
            int lengh = paramname.Length;
            MySqlParameter[] @params = new MySqlParameter[lengh];
            ParamBuilder(@params,paramname, paramval);
            SetConnStrForMysql();
            connectTomysql.Open();
            cmd.Connection = connectTomysql;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(@params);
            cmd.CommandText = str_prod;
            adap.SelectCommand = cmd;
            adap.Fill(returnData);
            connectTomysql.Close();
            return returnData;
        }

        public bool SetSomeData(string str_prod, string[] paramname, object[] paramvalue)
        {
            bool result = false;
            int lengh = paramname.Length;
            MySqlParameter[] @params = new MySqlParameter[lengh];
            ParamBuilder(@params, paramname, paramvalue);
            SetConnStrForMysql();
            try
            {
                connectTomysql.Open();
                cmd.Connection = connectTomysql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(@params);
                cmd.CommandText = str_prod;
                if (cmd.ExecuteNonQuery() == 1)
                    result = true;

            }
            catch (MySqlException ex)
            {

                int errorcode = ex.Number;
                if (errorcode == 1062)
                    MessageBox.Show("קיימת רשומה זהה במערכת");
                else
                    MessageBox.Show(ex.ToString());
            }

            return result;
        }
        public string update_data_with_ans(string str_prod, string[] paramname, object[] paramvalue)
        {
            string result="";
            int lengh = paramname.Length;
            MySqlParameter[] @params = new MySqlParameter[lengh];
            ParamBuilder(@params, paramname, paramvalue);
            SetConnStrForMysql();
            try
            {
                connectTomysql.Open();
                cmd.Connection = connectTomysql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(@params);
                cmd.CommandText = str_prod;
                if (cmd.ExecuteNonQuery() == 1)
                    result = "העדכון בוצע";

            }
            catch (MySqlException ex)
            {
                string MysqlError = ex.ToString();

                int errorcode = ex.Number;
                if (errorcode == 1062)
                    result = "העדכון לא בוצע.סיבה : רשומה כפולה";
                else
                    result = "העדכון לא בוצע"+ MysqlError;
            }

            return result;
        }
    }
}
