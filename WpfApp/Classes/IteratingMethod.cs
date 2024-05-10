using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp.Classes
{
    public class IteratingMethod
    {
        /// <summary>
        /// Перестановки массива
        /// </summary>
        /// <param name="priorPermutations">Список списков вещественных чисел</param>
        /// <param name="additions">Дополнения</param>
        /// <returns>Список перестановок</returns>
        static List<List<double>> AppendArray(List<List<double>> priorPermutations, double[] additions)
        {
            List<List<double>> newPermutationsResult = new List<List<double>>();
            foreach (List<double> priorPermutation in priorPermutations)
            {
                foreach (double addition in additions)
                {
                    List<double> priorWithAddition = new List<double>(priorPermutation);
                    priorWithAddition.Add(addition);
                    newPermutationsResult.Add(priorWithAddition);
                }
            }
            return newPermutationsResult;
        }

        /// <summary>
        /// Формирование списка с количеством продуктов
        /// </summary>
        /// <param name="datas">Список с данными</param>
        /// <param name="minWeight">Минимальный вес</param>
        /// <returns>Список с количеством продуктов каждого вида</returns>
        static List<double[]> MaxCountArray(List<DataClass> datas, out double minWeight)
        {
            List<double[]> myList = new List<double[]>();
            minWeight = 0;
            foreach (DataClass x in datas)
            {
                minWeight += x.Weight * x.MaxCount;
                double[] arr = new double[x.MaxCount + 1];
                for (int i = 0; i <= x.MaxCount; i++)
                {
                    arr[i] = i;
                }
                myList.Add(arr);
            }
            return myList;
        }

        /// <summary>
        /// Формирование всех перестановок
        /// </summary>
        /// <param name="myList">Список с количеством продуктов каждого вида</param>
        /// <returns>Все возможные перестановки</returns>
        static List<List<double>> FormingPermutations(List<double[]> myList)
        {
            List<List<double>> permutations = new List<List<double>>();

            foreach (double init in myList[0])
            {
                List<double> temp = new List<double>();
                temp.Add(init);
                permutations.Add(temp);
            }

            for (int i = 1; i < myList.Count; ++i)
            {
                permutations = AppendArray(permutations, myList[i]);
            }
            return permutations;
        }

        /// <summary>
        /// Формирование списка с оптимальным решением
        /// </summary>
        /// <param name="datas">Список с данными</param>
        /// <param name="permutations">Перестановки</param>
        /// <param name="minWeight">Минимальный вес</param>
        /// <param name="K">Минимальная суммарная калорийность</param>
        /// <returns>Список с оптимальным решением</returns>
        static List<double> FormingListSolution(List<DataClass> datas, List<List<double>> permutations, ref double minWeight, double K)
        {
            List<double> solution = new List<double>();
            foreach (List<double> list1 in permutations)
            {
                double kalor = 0;
                double weight = 0;
                for (int i = 0; i < list1.Count; i++)
                {
                    kalor += list1[i] * datas[i].Calories;
                    weight += list1[i] * datas[i].Weight;
                }
                if ((kalor >= K) && weight < minWeight)
                {
                    solution = list1;
                    minWeight = weight;
                }
            }
            return solution;
        }

        /// <summary>
        /// Формирование строки с ответом
        /// </summary>
        /// <param name="solution">Список с оптимальным решением</param>
        /// <param name="minWeight">Минимальный вес</param>
        /// <returns>Строка с ответом</returns>
        static string PrintData(List<double> solution, double minWeight)
        {
            string str = "Решение найдено!\n";
            for (int i = 0; i < solution.Count; i++)
            {
                str += $"{i + 1} вид продукции {solution[i]}\n";
            }
            str += $"Минимальный вес {minWeight}\n";
            if (solution.Count == 0)
            {
                return "Решения не существует";
            }
            return str;
        }

        /// <summary>
        /// Метод перебора
        /// </summary>
        /// <param name="datas">Список с данными</param>
        /// <param name="K">Минимальная суммарная калорийность</param>
        /// <returns>Строка с решением</returns>
        public static string Solve(List<DataClass> datas, double K)
        {
            try
            {
                double minWeight;
                List<double[]> myList = MaxCountArray(datas, out minWeight);
                List<List<double>> permutations = FormingPermutations(myList);
                List<double> solution = FormingListSolution(datas, permutations, ref minWeight, K);
                string str = PrintData(solution, minWeight);
                return str;
            }
            catch
            {
                MessageBox.Show("Ошибка! Недостаточно памяти для обработки информации", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return "Ошибка";
            }
        }

    }
}
