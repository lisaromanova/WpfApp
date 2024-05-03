using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Classes
{
    public class DataClass
    {
        /// <summary>
        /// Вид продукции
        /// </summary>
        public int Id { get; set; } 
        /// <summary>
        /// Вес
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// Калории
        /// </summary>
        public double Calories { get; set; }
        /// <summary>
        /// Максимальное количество
        /// </summary>
        public int MaxCount { get; set; }
    }
}
