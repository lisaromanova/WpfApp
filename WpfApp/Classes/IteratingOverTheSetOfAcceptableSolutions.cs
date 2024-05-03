using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp.Classes
{
    public class IteratingOverTheSetOfAcceptableSolutions
    {
        /// <summary>
        /// Перестановки массива
        /// </summary>
        /// <param name="priorPermutations">Список списков вещественных чисел</param>
        /// <param name="additions">Дополнения</param>
        /// <returns>Список всех возможных перестановок</returns>
        static List<List<double>> RecursiveAppend(List<List<double>> priorPermutations, double[] additions)
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
        /// Перебор множества допустимый решений
        /// </summary>
        /// <param name="datas">Список с данными</param>
        /// <param name="K">Минимальная суммарная калорийность</param>
        /// <returns>Строка с решением</returns>
        public static string Method(List<DataClass> datas, double K)
        {
            List<double[]> myList = new List<double[]>();
            double minWeight = 0;
            List<double> solution = new List<double>();
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

            List<List<double>> permutations = new List<List<double>>();

            foreach (double init in myList[0])
            {
                List<double> temp = new List<double>();
                temp.Add(init);
                permutations.Add(temp);
            }

            for (int i = 1; i < myList.Count; ++i)
            {
                permutations = RecursiveAppend(permutations, myList[i]);
            }

            //На данный момент переменная permutations содержит все перестановки
            foreach (List<double> list1 in permutations)
            {
                double kalor = 0;
                double weight = 0;
                for(int i = 0; i < list1.Count; i++)
                {
                    kalor += list1[i] * datas[i].Calories;
                    weight += list1[i] * datas[i].Weight;
                }
                if((kalor >= K) && weight < minWeight)
                {
                    solution = list1;
                    minWeight = weight;
                }
            }
            string str = "Решение найдено!\n";
            for (int i = 0; i < solution.Count; i++)
            {
                str += $"{i+1} вид продукции {solution[i]}\n";
            }
            str += $"Минимальный вес {minWeight}\n";
            if(solution.Count == 0)
            {
                return "Решения не существует";
            }
            return str;
        }

    }
}
