using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp.Classes
{
    public class SimplexMethod
    {
        /// <summary>
        /// Формирование симплекс-таблицы
        /// </summary>
        /// <param name="n">Количество видов продуктов</param>
        /// <param name="listData">Список с данными</param>
        /// <param name="K">Минимальная суммарная калорийность продуктов</param>
        /// <returns>Симплекс-таблица</returns>
        static double[,] FormingSimplexTable(int n, List<DataClass> listData, double K)
        {
            double[,] simplex_table = new double[n + 4, n + n + 3];
            //Заполение C, каллорийности, ограничений, свободных коэффициентов
            for (int i = 1; i <= n; i++)
            {
                simplex_table[0, i] = listData[i-1].Weight;
                simplex_table[1, i] = i;
                simplex_table[2, i] = listData[i - 1].Calories * (-1);
                simplex_table[2 + i, i] = 1;
                simplex_table[2 + i, simplex_table.GetLength(1) - 1] = Convert.ToDouble(listData[i - 1].MaxCount);
            }
            simplex_table[2, simplex_table.GetLength(1) - 1] = K * (-1);
            //Заполнение базиса
            for (int i = 0; i <= n; i++)
            {
                simplex_table[i + 2, 0] = n + i + 1;
                simplex_table[1, n + i + 1] = n + i + 1;
                simplex_table[i + 2, n + i + 1] = 1;
            }
            return simplex_table;
        }

        /// <summary>
        /// Вывод симплекс-таблицы на экран
        /// </summary>
        /// <param name="simplex_table">Симплекс-таблица</param>
        static void PrintSimplexTable(double[,] simplex_table)
        {
            string str = "";
            for (int i = 0; i < simplex_table.GetLength(0); i++)
            {
                for (int j = 0; j < simplex_table.GetLength(1); j++)
                {
                    str += (Math.Round(simplex_table[i, j], 5) + "\t");
                }
                str += "\n";
            }
            Console.WriteLine(str);
        }

        /// <summary>
        /// Проверка на наличие отрицательных свободных коэффициентов
        /// </summary>
        /// <param name="simplex_table">Симплекс-таблица</param>
        /// <returns>True - есть отрицательные свободные коэффициенты, False - нет отрицательных свободных коэффициентов</returns>
        static bool CheckNegativeElements(double[,] simplex_table)
        {
            for (int i = 0; i < simplex_table.GetLength(0); i++)
            {
                if (simplex_table[i, simplex_table.GetLength(1) - 1] < 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Пересчет элементов
        /// </summary>
        /// <param name="simplex_table">Симплекс-таблица</param>
        /// <param name="resolving_element">Разрешающий элемент</param>
        /// <param name="resolution_line">Разрешающая строка</param>
        /// <param name="resolution_column">Разрешающий столбец</param>
        static void Calculation(double[,] simplex_table, double resolving_element, int resolution_line, int resolution_column)
        {
            //Меняем базис
            simplex_table[resolution_line, 0] = simplex_table[1, resolution_column];
            for (int i = 1; i < simplex_table.GetLength(1); i++)
            {
                simplex_table[resolution_line, i] /= resolving_element;
            }
            for (int i = 2; i < simplex_table.GetLength(0) - 1; i++)
            {
                if (i != resolution_line)
                {
                    double k = simplex_table[i, resolution_column];
                    for (int j = 1; j < simplex_table.GetLength(1); j++)
                    {
                        simplex_table[i, j] += simplex_table[resolution_line, j] * (-1) * k;
                    }
                }
            }
        }

        /// <summary>
        /// Избавление от отрицательных элементов
        /// </summary>
        /// <param name="simplex_table">Симплекс-таблица</param>
        /// <param name="n">Количество видов продукции</param>
        /// <returns>True - решение существует, False - решения не существует</returns>
        static bool NegativeElements(double[,] simplex_table, int n)
        {
            double minimal_element_b = 0;
            int index_resolving_line = 0;
            //Поиск строки с минимальным элементом
            for (int i = 0; i < simplex_table.GetLength(0); i++)
            {
                if (simplex_table[i, simplex_table.GetLength(1) - 1] < minimal_element_b)
                {
                    minimal_element_b = simplex_table[i, simplex_table.GetLength(1) - 1];
                    index_resolving_line = i;
                }
            }
            double resolving_element = 0;
            int index_resolution_column = 0;
            //Поиск минимального элемента в строке
            for (int i = 1; i <= n; i++)
            {
                if (simplex_table[index_resolving_line, i] < resolving_element)
                {
                    resolving_element = simplex_table[index_resolving_line, i];
                    index_resolution_column = i;
                }
            }
            //Если минимальный элемент отсутствует, то решения не существует
            if (resolving_element == 0)
            {
                return false;
            }
            Calculation(simplex_table, resolving_element, index_resolving_line, index_resolution_column);
            return true;
        }

        /// <summary>
        /// Расчет дельт
        /// </summary>
        /// <param name="simplex_table">Симплекс-таблица</param>
        static void CalculationOfDeltas(double[,] simplex_table)
        {
            for (int j = 1; j < simplex_table.GetLength(1); j++)
            {
                double delta = 0;
                for (int i = 2; i < simplex_table.GetLength(0) - 1; i++)
                {
                    delta += simplex_table[i, j] * simplex_table[0, (int)simplex_table[i, 0]];
                }
                delta -= simplex_table[0, j];
                simplex_table[simplex_table.GetLength(0) - 1, j] = delta;
            }
        }

        /// <summary>
        /// Проверка плана на оптимальность
        /// </summary>
        /// <param name="simplex_table">Симплекс-таблица</param>
        /// <returns>True - план оптимален, False - план не оптимален</returns>
        static bool CheckOptimalSolution(double[,] simplex_table)
        {
            for (int i = 1; i < simplex_table.GetLength(0) - 1; i++)
            {
                if (simplex_table[simplex_table.GetLength(0) - 1, i] > 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Поиск оптимального решения
        /// </summary>
        /// <param name="simplex_table">Симплекс-таблица</param>
        /// <returns>True - решение существует, False - решения не сущуствует</returns>
        static bool OptionalSolution(double[,] simplex_table)
        {
            double max_delta = 0;
            int index_resolving_column = 0;
            //поиск максимальной дельты
            for (int i = 1; i < simplex_table.GetLength(1) - 1; i++)
            {
                if (simplex_table[simplex_table.GetLength(0) - 1, i] > max_delta)
                {
                    max_delta = simplex_table[simplex_table.GetLength(0) - 1, i];
                    index_resolving_column = i;
                }
            }
            double[] Q = new double[simplex_table.GetLength(0) - 3];
            int count_zero = 0;
            //подсчет Q
            for (int i = 2; i < simplex_table.GetLength(0) - 1; i++)
            {
                if (simplex_table[i, index_resolving_column] > 0)
                {
                    Q[i - 2] = simplex_table[i, simplex_table.GetLength(1) - 1] / simplex_table[i, index_resolving_column];
                }
                else
                {
                    count_zero++;
                }
            }
            //если все элементы разрешающего столбца отрицательны то решения нет
            if (count_zero == Q.Length)
            {
                return false;
            }
            double minQ = Q[0];
            int index_resolving_line = 2;
            //поиск минимального значения Q, для определения разрешающей строки
            for (int i = 0; i < Q.Length; i++)
            {
                if (Q[i] < minQ && Q[i] != 0)
                {
                    minQ = Q[i];
                    index_resolving_line = i + 2;
                }
            }
            Calculation(simplex_table, simplex_table[index_resolving_line, index_resolving_column], index_resolving_line, index_resolving_column);
            return true;
        }

        /// <summary>
        /// Вывод данных
        /// </summary>
        /// <param name="solution">Решение (есть или нет)</param>
        /// <param name="n">Количество видов продукции</param>
        /// <param name="simplex_table">Симплекс-таблица</param>
        /// <returns>Строка с решением</returns>
        static string printData(bool solution, int n, double[,] simplex_table)
        {
            //проверка на наличие решения
            if (solution)
            {
                string str = "Решение найдено!\n";
                for (int i = 1; i <= n; i++)
                {
                    double x = 0;
                    for (int j = 2; j < simplex_table.GetLength(0) - 1; j++)
                    {
                        //если базис равен виду продукции решение есть
                        if (i == simplex_table[j, 0])
                        {
                            x = simplex_table[j, simplex_table.GetLength(1) - 1];
                        }
                    }
                    str += $"{i} вид продукции {Math.Round(x,2)}\n";
                }
                str += $"Минимальный вес {Math.Round(simplex_table[simplex_table.GetLength(0) - 1, simplex_table.GetLength(1) - 1], 2)}";
                return str;
            }
            else
            {
                return "Решения не существует";
            }
        }

        /// <summary>
        /// Решение задачи
        /// </summary>
        /// <param name="n">Количество видов продуктов</param>
        /// <param name="listData">Список с данными</param>
        /// <param name="K">Минимальная суммарная калорийность</param>
        /// <returns>Массив с решением</returns>
        public static string Solve(int n, List<DataClass> listData, double K)
        {
            //int n = 3;
            //double[] weight = { 120, 50, 200 }, calories = { 100, 300, 500 }, maxCount = { 10, 4, 4 };
            //double K = 4000;

            //int n = 3;
            //double[] weight = { 120, 50, 200 }, calories = { 100, 300, 500 }, maxCount = { 10, 4, 4 };
            //double K = 5000;

            //int n = 2;
            //double[] weight = { 10, 20 }, calories = { 30, 40 }, maxCount = { 5, 6 };
            //double K = 200;

            double[,] simplex_table = FormingSimplexTable(n, listData, K);
            //PrintSimplexTable(simplex_table);
            bool solution = true;
            while (true)
            {
                if (CheckNegativeElements(simplex_table) && solution)
                {
                    solution = NegativeElements(simplex_table, n);
                }
                else
                {
                    CalculationOfDeltas(simplex_table);
                    bool optional = CheckOptimalSolution(simplex_table);
                    //PrintSimplexTable(simplex_table);
                    if (optional)
                    {
                        break;
                    }
                    else
                    {
                        solution = OptionalSolution(simplex_table);
                    }
                }
            }
            return printData(solution, n, simplex_table);
        }
    }
}
