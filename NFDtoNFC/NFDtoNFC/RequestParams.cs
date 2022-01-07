using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFDtoNFC
{
    public struct RequestParams
    {
        public string FileName;
        public string DirectoryName;
        public bool ContentText;
        public NormalizationForm Form;
    }
}
