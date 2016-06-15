using System;
using Android;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;

namespace Watch1159
{
	public class XMLwriter
	{
		public XMLwriter ()
		{
		}
		public static void WriteXML(String filename, List<List<Vector3>> list) {
			var document = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var path = Path.Combine(document.ToString(), filename);
			XmlSerializer serializer = new XmlSerializer(typeof(List<List<Vector3>>));//initialises the serialiser
			Stream writer = new FileStream(path, FileMode.Create);//initialises the writer

			serializer.Serialize(writer, list);//Writes to the file
			writer.Close ();//Closes the writer
		}

		public static void Write() {
			var path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var filename = Path.Combine(path.ToString(), "myfile.txt");
			using (var streamWriter = new StreamWriter(filename, true))
			{
				streamWriter.WriteLine("I am working!");
			}
		}
	}
}

