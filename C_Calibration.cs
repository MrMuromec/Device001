using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using Excel = Microsoft.Office.Interop.Excel; // Переобозначение
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Device001
{
    public class C_Calibration
    {
        private string V_Address;
        public string Fv_Address 
        {
            get
            {
                return V_Address;
            } 
            set
            {
                V_Address = value;
                using (FileStream fs = new FileStream(V_FileName, FileMode.OpenOrCreate)) // Подумать насчёт исключений
                {
                    V_formatter.Serialize(fs, V_Address);
                }
            }
        }
        private string V_FileName = ""; // Названия файла для сохранения настроек
        private BinaryFormatter V_formatter = new BinaryFormatter();  // Формат 

        private Excel.Application _ObjExcel = null; // Приложение
        private Excel.Workbook _ObjWorkBook = null; // Книга
        private Excel.Worksheet _ObjWorkSheet = null; // Листы
        private object _missingObj = System.Reflection.Missing.Value;

        public C_Calibration(string v_FileName)
        {
            V_FileName = v_FileName;
            try
            {
                using (FileStream fs = new FileStream(V_FileName, FileMode.Open))  // Подумать насчт исключений
                {
                    V_Address = (string)V_formatter.Deserialize(fs);
                }
            }
            catch (FileNotFoundException)
            {
                V_Address = "";
            }
        }

        public List<double> F_Laod(string v_SheetsName, char v_Colum)
        {
            List<double> v_Items = new List<double>();
            Excel.Range range = null;

            // Создаём приложение
            _ObjExcel = new Excel.Application();
            // Открываем книгу                                                                                                                                                        
            _ObjWorkBook = _ObjExcel.Workbooks.Open(Fv_Address, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            for (int i = 1; i <= _ObjWorkBook.Sheets.Count; i++)
                if (v_SheetsName == ((Excel.Worksheet)_ObjWorkBook.Sheets[i]).Name)
                    _ObjWorkSheet = _ObjWorkBook.Sheets[i];
            for (int z = 2; (range = _ObjWorkSheet.get_Range(v_Colum + z.ToString())).Text != ""; z++)
                v_Items.Add((double.Parse(range.Text, CultureInfo.InvariantCulture)));

            // Уборка неуправляемого мусора
            _ObjWorkBook.Close(false, _missingObj, _missingObj);
            _ObjExcel.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_ObjExcel);
            _ObjWorkBook = null;
            _ObjExcel = null;
            _ObjWorkSheet = null;
            System.GC.Collect(); 
            return v_Items;
        }
    }
}
