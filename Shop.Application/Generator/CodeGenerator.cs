using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Generator
{
    public class CodeGenerator
    {
        public static string GenerateUniqCode()
        {
            Random rand = new Random();
            return rand.Next(10000,99999).ToString();
        }

        public static string GenerateFileName(string fileName)
        {
            return  Guid.NewGuid().ToString("N") + Path.GetExtension(fileName);
        }
    }
}
