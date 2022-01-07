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
        public static bool NormalizeFileName(FileInfo fileInfo, NormalizationForm form)
        {
            if (fileInfo == null)
            {
                return false;
            }

            var name = NormalizeText(fileInfo.FullName, form);

            try
            {
                fileInfo.MoveTo(name);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
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
