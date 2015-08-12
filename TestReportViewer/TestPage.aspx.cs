using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestReportViewer
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;



            
            System.Data.DataTable dt = null;
            Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("COR_Basic", dt);
            // rds.Name = "COR_Basic";
            // rds.Value = dt;

            Microsoft.Reporting.WebForms.LocalReport lr = new Microsoft.Reporting.WebForms.LocalReport();
            lr.DataSources.Add(rds);
            lr.ReportPath = "bla.rdlc";
            
            byte[] mybytes = lr.Render("WORD");


        }
    }
}