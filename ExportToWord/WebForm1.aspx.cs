
using Microsoft.Reporting.WebForms;


namespace ExportToWord
{


    public partial class WebForm1 : System.Web.UI.Page
    {


        public System.Data.DataTable GetData()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Salary", typeof(float));
            dt.Columns.Add("EmployeeID", typeof(string));
            dt.Columns.Add("Designation", typeof(string));

            System.Data.DataRow dr = null;
            for (int i = 0; i < 10; ++i)
            {
                dr = dt.NewRow();
                dr["Name"] = "Person " + i.ToString();
                dr["Salary"] = 10.0 * i;
                dr["EmployeeID"] = i.ToString();
                dr["Designation"] = string.Format("{0} of 10", i);

                dt.Rows.Add(dr);
            }

            return dt;
        }


        protected void Page_Load(object sender, System.EventArgs e)
        {
            LocalReport report = new LocalReport();
            report.ReportPath = "Report1.rdlc";
            report.ReportPath = "SSRSReport.rdl";
            report.ReportPath = this.Server.MapPath("~/SSRSReport.rdl");

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";//This refers to the dataset name in the RDLC file
            // rds.Value = EmployeeRepository.GetAllEmployees();
            rds.Value = GetData();
            report.DataSources.Add(rds);

            string format = GetExportFormatString(ExportFormat.Word);
            //Byte[] mybytes = report.Render("WORD");
            byte[] mybytes = report.Render(format);


            string fileName = System.Environment.OSVersion.Platform == System.PlatformID.Unix ? @"/root/SalLip.doc" : @"D:\SalSlip.doc";


            //Byte[] mybytes = report.Render("PDF"); for exporting to PDF
            using (System.IO.FileStream fs = System.IO.File.Create(fileName))
            {
                fs.Write(mybytes, 0, mybytes.Length);
            } // End Using fs 

        } // End Sub Page_Load 


        /// </summary>
        public enum ExportFormat
        {
            /// <summary>XML</summary>
            XML,
            /// <summary>Comma Delimitted File</summary>
            CSV,
            /// <summary>TIFF image</summary>
            Image,
            /// <summary>PDF</summary>
            PDF,
            /// <summary>HTML (Web Archive)</summary>
            MHTML,
            /// <summary>HTML 4.0</summary>
            HTML4,
            /// <summary>HTML 3.2</summary>
            HTML32,
            /// <summary>Excel</summary>
            Excel,
            /// <summary>Excel 2003</summary>
            Excel_2003,
            /// <summary>Word</summary>
            Word,
            /// <summary>Word 2003</summary>
            Word_2003
        } // End Enum ExportFormat



        public static int SetSQLServerVersion()
        { 
            string strSQL = @"
DECLARE @tV AS integer; 
SET @tV = (SELECT TOP 1 FC_Value FROM T_FMS_Configuration WHERE FC_Key = N'ReportExecution_SSRSVersion'); 

IF (@tV IS NOT NULL AND @tV > 2000) 
	BEGIN 
		SELECT @tV 
	END 
ELSE 
	BEGIN 
		DECLARE @ver nvarchar(128); 
		SET @ver = CAST( SERVERPROPERTY('ProductVersion') AS nvarchar); 
		SET @ver = SUBSTRING(@ver, 1, CHARINDEX('.', @ver) - 1); 
		
		IF ( @ver = N'9' ) 
		   SELECT 2005 
		ELSE IF ( @ver = N'10' ) 
		   SELECT 2008 
		ELSE 
		   SELECT 2012 
	END 
";

            System.Diagnostics.Trace.WriteLine(strSQL);

            // return 2005;
            // return 2012;
            return 2008;
        }

        /// <summary>
        /// Gets the string export format of the specified enum.
        /// </summary>
        /// <param name="f">export format enum</param>
        /// <returns>enum equivalent string export format</returns>
        public static string GetExportFormatString(ExportFormat f)
        {
            int V_SQLServer = SetSQLServerVersion();
            
            switch (f)
            {
                case ExportFormat.XML:
                    return "XML";
                case ExportFormat.CSV:
                    return "CSV";
                case ExportFormat.Image:
                    return "IMAGE";
                case ExportFormat.PDF:
                    return "PDF";
                case ExportFormat.MHTML:
                    return "MHTML";
                case ExportFormat.HTML4:
                    return "HTML4.0";
                case ExportFormat.HTML32:
                    return "HTML3.2";
                case ExportFormat.Excel:
                    return V_SQLServer <= 2008 ? "EXCEL" : "EXCELOPENXML";
                case ExportFormat.Excel_2003:
                    return "EXCEL";
                case ExportFormat.Word:
                    return V_SQLServer <= 2008 ? "WORD" : "WORDOPENXML";
                case ExportFormat.Word_2003:
                    return "WORD";
                default:
                    return "PDF";
            } // End switch (f)

        } // End Function GetExportFormatString


    }


}
