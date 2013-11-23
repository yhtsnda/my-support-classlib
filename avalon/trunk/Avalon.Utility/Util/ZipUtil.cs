using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public static class ZipUtil
    {
        public static void CompressFiles(string zipFileName, IEnumerable<string> fileNames)
        {
            foreach (string file in fileNames)
            {
                CompressFile(zipFileName, file);
            }
        }

        public static void CompressDirectory(string zipFileName, string path)
        {
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToArray();
            foreach (string file in files)
            {
                CompressFile(zipFileName, file, path);
            }
        }

        public static void CompressFile(string zipFilename, string fileName)
        {
            CompressFile(zipFilename, fileName, null);
        }

        static void CompressFile(string zipFilename, string fileName, string directory = null)
        {
            using (Package zip = Package.Open(zipFilename, FileMode.OpenOrCreate))
            {
                string relativePath = ".\\" + Path.GetFileName(fileName);
                if (!String.IsNullOrEmpty(directory))
                {
                    if (fileName.StartsWith(directory))
                    {
                        relativePath = ".\\" + fileName.Substring(directory.Length + 1);
                    }
                }
                Uri uri = PackUriHelper.CreatePartUri(new Uri(relativePath, UriKind.Relative));

                if (zip.PartExists(uri))
                {
                    zip.DeletePart(uri);
                }
                PackagePart part = zip.CreatePart(uri, "", CompressionOption.Normal);
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (Stream dest = part.GetStream())
                    {
                        fileStream.CopyTo(dest);
                    }
                }
            }
        }

        public static void DecompressFile(string zipFilename, string outPath)
        {
            using (Package zip = Package.Open(zipFilename, FileMode.Open))
            {
                foreach (PackagePart part in zip.GetParts())
                {
                    string outFileName = Path.Combine(outPath, part.Uri.OriginalString.Substring(1));
                    var dir = Path.GetDirectoryName(outFileName);

                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    using (FileStream outFileStream = new FileStream(outFileName, FileMode.Create))
                    {
                        using (Stream inFileStream = part.GetStream())
                        {
                            inFileStream.CopyTo(outFileStream);
                        }
                    }
                }
            }
        }
    }
}
