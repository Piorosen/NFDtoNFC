using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace NFDtoNFC
{
    public class Program
    {
        enum ErrorType
        {
            OverlapConvertMode,
            ParsingError,
            Help,
            NotFoundParam
        }
        //file
        // directory
        //recursive
        //content
        //normalize
        static RequestParams ParsingParameter(string[] args)
        {
            RequestParams param = new RequestParams();

            // get parameter parsing
            for (int i = 1; i < args.Length; i += 2)
            {
                switch (args[i])
                {
                    case "--filter":
                        if (param.FileName != null)
                        {
                            // error! 
                            PrintHelp(ErrorType.OverlapConvertMode);
                        }
                        else
                        {
                            if (param.Directory != null)
                            {

                                param.Directory.Extension = args[i + 1].Split("|").ToList();
                            }
                            else
                            {
                                param.Directory = new RequestDirectory
                                {
                                    Extension = args[i + 1].Split("|").ToList()
                                };
                            }
                        }
                        break;
                    // only convert file
                    case "--file":
                        if (param.Directory != null)
                        {
                            // error!
                            PrintHelp(ErrorType.OverlapConvertMode);
                        }
                        else
                        {
                            param.FileName = args[i + 1];
                        }
                        break;
                    // convert directory (recursive)
                    case "--directory":
                        if (param.FileName != null)
                        {
                            // error! 
                            PrintHelp(ErrorType.OverlapConvertMode);
                        }
                        else
                        {
                            if (param.Directory != null)
                            {
                                param.Directory.DirectoryName = args[i + 1];
                            }
                            else
                            {
                                param.Directory = new RequestDirectory
                                {
                                    DirectoryName = args[i + 1]
                                };
                            }
                        }
                        break;
                    case "--recursive":
                        if (param.FileName != null)
                        {
                            // error! 
                            PrintHelp(ErrorType.OverlapConvertMode);
                        }
                        else
                        {
                            if (bool.TryParse(args[i + 1], out bool recursive))
                            {
                                if (param.Directory != null)
                                {
                                    param.Directory.Recursive = recursive;
                                }
                                else
                                {
                                    param.Directory = new RequestDirectory
                                    {
                                        Recursive = recursive
                                    };
                                }
                            }
                            else
                            {
                                // parsing error!
                                PrintHelp(ErrorType.ParsingError);
                            }
                        }
                        break;

                    case "--content":
                        if (bool.TryParse(args[i + 1], out bool content))
                        {
                            param.ContentText = content;
                        }
                        else
                        {
                            // parsing error!
                            PrintHelp(ErrorType.ParsingError);
                        }
                        break;
                    case "--normalize":
                        var arg = args[i + 1].ToLower();
                        var items = new Dictionary<string, NormalizationForm>
                        {
                            ["nfc"] = NormalizationForm.FormC,
                            ["nfd"] = NormalizationForm.FormD,
                            ["nfkc"] = NormalizationForm.FormKC,
                            ["nfkd"] = NormalizationForm.FormKD,
                        };
                        if (items.ContainsKey(arg))
                        {
                            param.Form = items[arg];
                        }
                        else
                        {
                            // parsing error!
                            PrintHelp(ErrorType.ParsingError);
                        }
                        break;

                    case "--help":
                        PrintHelp();
                        break;
                    default:
                        PrintHelp(ErrorType.NotFoundParam);
                        break;
                }
            }
            return param;
        }

        static void PrintHelp(ErrorType type = ErrorType.Help)
        {
            Console.WriteLine("Normalization Form Text Help");
            Console.WriteLine();
            Console.WriteLine("Two Mode :");
            Console.WriteLine("1 : File Convert Mode");
            Console.WriteLine("2: Directory Convert Mode");
            Console.WriteLine();
            Console.WriteLine("File Mode Parameters");
            Console.WriteLine("type: String");
            Console.WriteLine("--file \"path\"");
            Console.WriteLine();
            Console.WriteLine("Directory Mode Parameters");
            Console.WriteLine("type: String");
            Console.WriteLine("--directory \"path\"");
            Console.WriteLine();
            Console.WriteLine("type: Bool");
            Console.WriteLine("--recursive true or false");
            Console.WriteLine();
            Console.WriteLine("common parameters");
            Console.WriteLine("type: Bool");
            Console.WriteLine("--content true or false");
            Console.WriteLine();
            Console.WriteLine("type: String");
            Console.WriteLine("--normalize type");
            Console.WriteLine("nfc, nfd, nfkc, nfkd");
        }
        static void RecursiveDirectory(DirectoryInfo di, NormalizationForm form, bool content, bool recursive, List<String> filter)
        {
            var files = di.GetFiles();
            foreach (var fi in files)
            {
                if (!fi.Name.StartsWith("."))
                {
                    var p = Converter.NormalizeFileName(fi, form);
                    if (content == true && filter.Contains(p.Extension))
                    {
                        Converter.NormalizeFileContent(p, form);
                    }
                }
            }

            if (recursive == true)
            {
                foreach (var dilist in di.GetDirectories())
                {
                    if (!dilist.Name.StartsWith("."))
                    {
                        RecursiveDirectory(dilist, form, content, recursive, filter);
                        Converter.NormalizeDirectoryName(dilist, form);
                    }
                }
            }
        }

        public static void EntryPoint(string[] args)
        {
            Main(args);
        }

        internal static void Main(string[] args)
        {
            var param = ParsingParameter(args);
            
            if (param.FileName != null)
            {
                var fi = new FileInfo(param.FileName);
                Converter.NormalizeFileName(fi, param.Form);
                if (param.ContentText == true)
                {
                    Converter.NormalizeFileContent(fi, param.Form);
                }
            }
            else if (param.Directory != null)
            {
                var di = new DirectoryInfo(param.Directory.DirectoryName);
                RecursiveDirectory(di, param.Form, param.ContentText, param.Directory.Recursive, param.Directory.Extension);
            }
        }
    }
}
