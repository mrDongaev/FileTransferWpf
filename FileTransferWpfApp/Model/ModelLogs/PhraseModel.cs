using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferWpfApp.Model.ModelLogs
{
    public static class PhraseModel
    {
        static Dictionary<int, string> phrasesRus = new Dictionary<int, string>()
        {
            { 0 , "Файла настроек не существует. Давайте создадим его" },

            { 1 , "Файл настроек найден. Чтение настроек" },

            { 3 , "4" },
        };

        public static string GetPhrase(int indexPhrase) 
        {
            return phrasesRus[indexPhrase];
        }


    }
}
