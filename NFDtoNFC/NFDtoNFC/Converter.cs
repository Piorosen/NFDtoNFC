using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFDtoNFC
{
    public static class Converter
    {
        public static bool NormalizeDirectoryName(DirectoryInfo directoryInfo, NormalizationForm form)
        {
            if (directoryInfo == null)
            {
                return false;
            }

            var name = NormalizeText(directoryInfo.Name, form);

            if (name == directoryInfo.Name)
            {
                return true;
            }
            try
            {
                directoryInfo.MoveTo(directoryInfo.Parent.FullName + "/" + name);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static FileInfo NormalizeFileName(FileInfo fileInfo, NormalizationForm form)
        {
            if (fileInfo == null)
            {
                return fileInfo;
            }

            var name = NormalizeText(fileInfo.Name, form);
            if (name == fileInfo.Name)
            {
                return fileInfo;
            }

            try
            {
                fileInfo.MoveTo(fileInfo.Directory + "/" + name);
                return new FileInfo(fileInfo.Directory + "/" + name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return fileInfo;
            }
        }

        public static bool NormalizeFileContent(FileInfo fileInfo, NormalizationForm form)
        {
            if (fileInfo == null)
            {
                return false;
            }

            using StreamReader sr = new StreamReader(fileInfo.FullName);
            var content = sr.ReadToEnd();
            sr.Close();

            using StreamWriter sw = new StreamWriter(fileInfo.FullName);
            content = NormalizeText(content, form);

            try
            {
                sw.Write(content);
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public static string NormalizeText(string text, NormalizationForm form)
        {
            try
            {
                if (text == null)
                {
                    return null;
                }
                return text.Normalize(form);
            }
            catch
            {
                return null;
            }
        }
    }
}
