using System;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace Watch1159
{
	public class WebServiceHandler
	{
		private String url = "http://utswatch.comlu.com/index.php?req=req";
		//private String url = "http://45.32.189.91/";
		private String user = "root";
		private String pwd = "passwordpasswordpassword";

		public WebServiceHandler ()
		{
		}

		public async Task postWithData(String path, String data) {
			var client = new HttpClient ();

			var uri = new Uri (url + path);
			try {
				var document = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
				var fileLoc = Path.Combine(document.ToString(), "trilist.xml");
				var xml = File.ReadAllText(fileLoc);
				var content = new StringContent (xml);

				HttpResponseMessage response = null;
				//if (isNewItem) {
				response = await client.PostAsync (uri, content);
				//} else {
				//	response = await client.PutAsync (uri, content);
				//}

				if (response.IsSuccessStatusCode) {
					Android.Util.Log.Debug("REST", "success");
				}

			} catch (Exception ex) {
				Android.Util.Log.Debug("REST",  ex.Message);
			}

		}

		public async Task postWithDataFtp() {
			FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create ("ftp://45.32.189.91/");
			ftp.Credentials = new NetworkCredential (user, pwd);
			ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
			ftp.UseBinary = true;
			var document = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var fileLoc = Path.Combine(document.ToString(), "trilist.xml");
			using (FileStream fs = File.OpenRead(fileLoc))
			{
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);
				fs.Close();
				Stream requestStream = ftp.GetRequestStream();
				requestStream.Write(buffer, 0, buffer.Length);
				requestStream.Flush();
				requestStream.Close();
				FtpWebResponse ftpResponse = (FtpWebResponse)ftp.GetResponse();
				if (ftpResponse != null) {
					ftpResponse.Close ();
					Android.Util.Log.Debug ("FTP", ftpResponse.StatusCode.ToString());
				}
				Android.Util.Log.Debug ("FTP", "NO");
			}
		}
	}
}

