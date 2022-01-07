using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFDtoNFC
{
    public class RequestDirectory
    {
        public bool Recursive = false;
        public string DirectoryName = null;
        public List<string> Extension = new List<string>();
    }
    public class RequestParams
    {
        public string FileName = null;
        public RequestDirectory? Directory = null;
        public bool ContentText = false;
        public NormalizationForm Form = NormalizationForm.FormC;
    }
}
