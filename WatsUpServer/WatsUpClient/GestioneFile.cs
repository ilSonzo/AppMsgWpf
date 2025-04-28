using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatsUpClient
{
    internal static class GestioneFile
    {
        public static byte[] ReadImage(string path, int dim)
        {
            byte[] bytes = new byte[dim];
            try
            {
                if (File.Exists(path))
                {
                    FileStream fs = new FileStream(path, FileMode.Open);
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return bytes;
        }

        public static void SaveImage(string filename, int filesize, byte[] img_bytes)
        {
            try
            {
                if (!Directory.Exists("imgs"))
                    Directory.CreateDirectory("imgs");
                string path = Path.Combine("imgs", filename);
                FileStream fs = new FileStream(path, FileMode.Create);
                fs.Write(img_bytes, 0, filesize);
                fs.Close();
            }
            catch (Exception e) { Console.WriteLine("Errore nel salvataggio dell'immagine: " + e.ToString()); }
        }

        public static long GetFileDim(string path)
        {
            return new System.IO.FileInfo(path).Length;
        }
    }
}
