using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Net20ReportViewr
{
    public partial class _Default : System.Web.UI.Page
    {

        System.Collections.Generic.List<System.IO.Stream> m_streams = new System.Collections.Generic.List<System.IO.Stream>();
        


        private System.IO.Stream CreateStream(string name, string fileNameExtension, System.Text.Encoding encoding, string mimeType, bool willSeek)
        {
            System.IO.Stream stream = new System.IO.FileStream(name + "." + fileNameExtension,
              System.IO.FileMode.Create);
            m_streams.Add(stream);
            return stream;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // ReportViewer1.ShowExportControls = false
            // ReportViewer1.ShowFindControls = false
            // ReportViewer1.ShowPrintButton = false
            // ReportViewer1.ShowPromptAreaButton = false
            // ReportViewer1.ShowParameterPrompts = false
            // ReportViewer1.ShowRefreshButton = false
            // ReportViewer1.ShowPageNavigationControls = false
            // ReportViewer1.ShowZoomControl = true
            // ReportViewer1.ShowToolBar = false
            // ReportViewer1.ShowDocumentMapButton = false
            // ReportViewer1.ShowBackButton = false
            // ReportViewer1.ToolBarItemBorderStyle = BorderStyle.None

            ReportViewer1.ShowToolBar = false;
            ReportViewer1.ShowParameterPrompts = true;
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;




            ReportViewer1.LocalReport.ReportPath = "GM_Gebaeudestammdaten_Wincasa.rdl"; //rdlc ?
            ReportViewer1.LocalReport.EnableHyperlinks = true;
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);

            
            


            Microsoft.Reporting.WebForms.ReportParameterInfoCollection x = ReportViewer1.LocalReport.GetParameters();


            /*

            parameters.Add(new ParameterValue
            {
                Name = "in_stichtag",
                Value = System.DateTime.Now.ToString("MM'/'dd'/'yyyy")
            });
            parameters.Add(new ParameterValue
            {
                Name = "in_mandant",
                Value = "0"
            });
            // in use
            parameters.Add(new ParameterValue
            {
                Name = "in_sprache",
                Value = "DE"
            });
            parameters.Add(new ParameterValue
            {
                Name = "in_standort",
                Value = "00000000-0000-0000-0000-000000000000"
            });
            parameters.Add(new ParameterValue
            {
                Name = "in_gebaeude",
                Value = "00000000-0000-0000-0000-000000000000"
            });

            parameters.Add(new ParameterValue
            {
                Name = "def_logo",
                Value = "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"
            });
            parameters.Add(new ParameterValue
            {
                Name = "def_padding_left",
                Value = 70
            });
            parameters.Add(new ParameterValue
            {
                Name = "def_padding_right",
                Value = 0
            });
            parameters.Add(new ParameterValue
            {
                Name = "def_padding_top",
                Value = 0
            });
            parameters.Add(new ParameterValue
            {
                Name = "def_padding_bottom",
                Value = 0
            });
            parameters.Add(new ParameterValue
            {
                Name = "def_mime",
                Value = "image/gif"
            });

            parameters.Add(new ParameterValue
            {
                Name = "def_HideLogo",
                Value = false
            });
            */

            foreach (Microsoft.Reporting.WebForms.ReportParameterInfo pi in x)
            {
                System.Console.WriteLine(pi.Prompt);
            }

            System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter> parms = new System.Collections.Generic.List<Microsoft.Reporting.WebForms.ReportParameter>();
            // Add Parameters of type Microsoft.Reporting.Windows.ReportParameter
            parms.Add(new Microsoft.Reporting.WebForms.ReportParameter("Test", "Whatever"));
            parms.Add(new Microsoft.Reporting.WebForms.ReportParameter("CustomerID", new string[] { "14335", "15094" }));

            //  http://forums.asp.net/t/1656685.aspx/1
            //  Clears the source data (below) before a new request is sent 
            // ReportViewer1.LocalReport.DataSources.Clear();
            // ReportViewer1.ShowReportBody = true;

            ReportViewer1.ServerReport.SetParameters(parms);



            System.Data.DataTable dt = null;
            Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("COR_Basic", dt);
            // rds.Name = "COR_Basic";
            // rds.Value = dt;


            // https://msdn.microsoft.com/en-US/library/ms251837(v=VS.90).aspx
            Microsoft.Reporting.WebForms.LocalReport lr = new Microsoft.Reporting.WebForms.LocalReport();
            lr.DataSources.Add(rds);
            lr.ReportPath = "bla.rdlc";



            string deviceInfo = null;

            /*
            deviceInfo =
          "<DeviceInfo>" +
          "  <OutputFormat>EMF</OutputFormat>" +
          "  <PageWidth>8.5in</PageWidth>" +
          "  <PageHeight>11in</PageHeight>" +
          "  <MarginTop>0.25in</MarginTop>" +
          "  <MarginLeft>0.25in</MarginLeft>" +
          "  <MarginRight>0.25in</MarginRight>" +
          "  <MarginBottom>0.25in</MarginBottom>" +
          "</DeviceInfo>";
            */
              
            Microsoft.Reporting.WebForms.CreateStreamCallback cb = CreateStream;
            Microsoft.Reporting.WebForms.Warning[] warnings;

            //lr.Render("WORD", deviceInfo, cb, out warnings);
            lr.Render("PDF", deviceInfo, cb, out warnings);

        }
    }
}