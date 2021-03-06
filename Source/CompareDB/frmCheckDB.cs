using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CompareDB
{
    public partial class frmCheckDB : Form
    {
        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public frmCheckDB()
        {
            InitializeComponent();
        }

        private void frmCheckDB_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            //SqlDataAdapter ad = new SqlDataAdapter("select cl.name as 'Tên column', tp.name as 'Kiểu dữ liệu',cl.max_length from sys.all_columns cl join sys.types tp on cl.user_type_id = tp.user_type_id    ", con);
            string querystring = "SELECT CASE WHEN (GROUPING(sob.name)=1) THEN 'All_Tables' ELSE ISNULL(sob.name, 'unknown') END AS Table_Name,"
                + "SUM(sys.length) AS Byte_Length , ISNULL(i.rows,0) Records "
                + " FROM sysobjects sob, syscolumns sys , sysindexes i "
                + " WHERE sob.xtype='u' AND sys.id=sob.id and i.id = sob.id and i.indid in (0,1)"
                + " GROUP BY sob.name, i.rows WITH CUBE  ";
            SqlDataAdapter ad = new SqlDataAdapter(querystring, con);

            DataTable dtCheckRecord = new DataTable();
            ad.Fill(dtCheckRecord);
            grdCheck.DataSource = dtCheckRecord;
            Sizedatagrid();
        }

        public void Sizedatagrid()
        {

            grdCheck.Columns[0].Width = 350;
            grdCheck.Columns[1].Width = 200;
            grdCheck.Columns[2].Width = 200;
        }
    }
}