using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device001.Port
{
    /// <summary>
    /// Набор пораметров используемых манахраматоров или используемых с ними
    /// </summary>
    public static class C_ParameterListsD02
    {
        /// <summary>
        /// Список шагов
        /// </summary>
        public static List<double> F_NumShiftGet()
        {
            return new List<double> { 0.002, 0.01, 0.05, 0.1, 0.5, 1, 5 };
        }
        /// <summary>
        /// Список скоростей
        /// </summary>
        public static List<double> F_NumSpeedGet()
        {
            return new List<double> { 200, 70, 30, 10, 3.3, 1.0, 0.6 };
        }
        /// <summary>
        /// Список параметров решёток
        /// </summary>
        public static List<C_ParameterGrid> F_NumGridGet()
        {
            return new List<C_ParameterGrid> { 
            new C_ParameterGrid (new int[] { 200, 500 },3000), 
            new C_ParameterGrid (new int[] { 400, 1000 },1500) };
        }
    }
    /// <summary>
    /// Список параметров решётки
    /// </summary>
    public class C_ParameterGrid
    {
        public int[] V_FSLenght { get; private set; }
        public int V_NumberStrokes { get; private set; }
        public C_ParameterGrid(int[] v_FSLenght,int v_NumberStrokes)
        {
            V_FSLenght = v_FSLenght;
            V_NumberStrokes = v_NumberStrokes;
        }
    }
}
