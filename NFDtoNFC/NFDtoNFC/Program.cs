using System;
using System.Text;
using System.IO;

namespace NFDtoNFC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i < args.Length; i += 2)
            {
                switch (args[i])
                {
                    case "--file":

                        break;
                    case "--directory":

                        break;
                    case "--content": break;

                    case "--normalize": break;

                    case "--help": break;

                        default: break;
                }                 
            }
        }
    }
}
