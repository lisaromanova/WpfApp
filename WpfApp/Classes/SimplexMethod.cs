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
        /// Проверка на наличие отрицательных свободных коэффициентов
        /// </summary>
        /// <param name="simplexTable">Симплекс-таблица</param>
        /// <returns>True - есть отрицательные свободные коэффициенты, False - нет отрицательных свободных коэффициентов</returns>
        static bool CheckNegativeElements(double[,] simplexTable)
        {
            for (int i = 0; i < simplexTable.GetLength(0); i++)
            {
                if (simplexTable[i, simplexTable.GetLength(1) - 1] < 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Пересчет элементов
        /// </summary>
        /// <param name="simplexTable">Симплекс-таблица</param>
        /// <param name="resolvingElement">Разрешающий элемент</param>
        /// <param name="resolutionLine">Разрешающая строка</param>
        /// <param name="resolutionColumn">Разрешающий столбец</param>
        static void Calculation(double[,] simplexTable, double resolvingElement, int resolutionLine, int resolutionColumn)
        {
            //Меняем базис
            simplexTable[resolutionLine, 0] = simplexTable[1, resolutionColumn];
            for (int i = 1; i < simplexTable.GetLength(1); i++)
            {
                simplexTable[resolutionLine, i] /= resolvingElement;
            }
            for (int i = 2; i < simplexTable.GetLength(0) - 1; i++)
            {
                if (i != resolutionLine)
                {
                    double k = simplexTable[i, resolutionColumn];
                    for (int j = 1; j < simplexTable.GetLength(1); j++)
                    {
                        simplexTable[i, j] += simplexTable[resolutionLine, j] * (-1) * k;
                    }
                }
            }
        }

        /// <summary>
        /// Избавление от отрицательных элементов
        /// </summary>
        /// <param name="simplexTable">Симплекс-таблица</param>
        /// <param name="n">Количество видов продукции</param>
        /// <returns>True - решение существует, False - решения не существует</returns>
        static bool NegativeElements(double[,] simplexTable, int n)
        {
            double minimalElementB = 0;
            int indexResolvingLine = 0;
            //Поиск строки с минимальным элементом
            for (int i = 0; i < simplexTable.GetLength(0); i++)
            {
                if (simplexTable[i, simplexTable.GetLength(1) - 1] < minimalElementB)
                {
                    minimalElementB = simplexTable[i, simplexTable.GetLength(1) - 1];
                    indexResolvingLine = i;
                }
            }
            double resolvingElement = 0;
            int indexResolutionColumn = 0;
            //Поиск минимального элемента в строке
            for (int i = 1; i <= n; i++)
            {
                if (simplexTable[indexResolvingLine, i] < resolvingElement)
                {
                    resolvingElement = simplexTable[indexResolvingLine, i];
                    indexResolutionColumn = i;
                }
            }
            //Если минимальный элемент отсутствует, то решения не существует
            if (resolvingElement == 0)
            {
                return false;
            }
            Calculation(simplexTable, resolvingElement, indexResolvingLine, indexResolutionColumn);
            return true;
        }

        /// <summary>
        /// Расчет дельт
        /// </summary>
        /// <param name="simplexTable">Симплекс-таблица</param>
        static void CalculationOfDeltas(double[,] simplexTable)
        {
            for (int j = 1; j < simplexTable.GetLength(1); j++)
            {
                double delta = 0;
                for (int i = 2; i < simplexTable.GetLength(0) - 1; i++)
                {
                    delta += simplexTable[i, j] * simplexTable[0, (int)simplexTable[i, 0]];
                }
                delta -= simplexTable[0, j];
                simplexTable[simplexTable.GetLength(0) - 1, j] = delta;
            }
        }

        /// <summary>
        /// Проверка плана на оптимальность
        /// </summary>
        /// <param name="simplexTable">Симплекс-таблица</param>
        /// <returns>True - план оптимален, False - план не оптимален</returns>
        static bool CheckOptimalSolution(double[,] simplexTable)
        {
            for (int i = 1; i < simplexTable.GetLength(0) - 1; i++)
            {
                if (simplexTable[simplexTable.GetLength(0) - 1, i] > 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Поиск оптимального решения
        /// </summary>
        /// <param name="simplexTable">Симплекс-таблица</param>
        /// <returns>True - решение существует, False - решения не сущуствует</returns>
        static bool OptionalSolution(double[,] simplexTable)
        {
            double max_delta = 0;
            int index_resolving_column = 0;
            //поиск максимальной дельты
            for (int i = 1; i < simplexTable.GetLength(1) - 1; i++)
            {
                if (simplexTable[simplexTable.GetLength(0) - 1, i] > max_delta)
                {
                    max_delta = simplexTable[simplexTable.GetLength(0) - 1, i];
                    index_resolving_column = i;
                }
            }
            double[] Q = new double[simplexTable.GetLength(0) - 3];
            int count_zero = 0;
            //подсчет Q
            for (int i = 2; i < simplexTable.GetLength(0) - 1; i++)
            {
                if (simplexTable[i, index_resolving_column] > 0)
                {
                    Q[i - 2] = simplexTable[i, simplexTable.GetLength(1) - 1] / simplexTable[i, index_resolving_column];
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
            Calculation(simplexTable, simplexTable[index_resolving_line, index_resolving_column], index_resolving_line, index_resolving_column);
            return true;
        }

        /// <summary>
        /// Вывод данных
        /// </summary>
        /// <param name="solution">Решение (есть или нет)</param>
        /// <param name="n">Количество видов продукции</param>
        /// <param name="simplexTable">Симплекс-таблица</param>
        /// <returns>Строка с решением</returns>
        static string printData(bool solution, int n, double[,] simplexTable)
        {
            //проверка на наличие решения
            if (solution)
            {
                string str = "Решение найдено!\n";
                for (int i = 1; i <= n; i++)
                {
                    double x = 0;
                    for (int j = 2; j < simplexTable.GetLength(0) - 1; j++)
                    {
                        //если базис равен виду продукции решение есть
                        if (i == simplexTable[j, 0])
                        {
                            x = simplexTable[j, simplexTable.GetLength(1) - 1];
                        }
                    }
                    str += $"{i} вид продукции {Math.Round(x,2)}\n";
                }
                str += $"Минимальный вес {Math.Round(simplexTable[simplexTable.GetLength(0) - 1, simplexTable.GetLength(1) - 1], 2)}";
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
            double[,] simplex_table = FormingSimplexTable(n, listData, K);
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
