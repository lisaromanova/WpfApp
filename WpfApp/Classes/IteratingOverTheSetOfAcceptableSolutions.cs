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
        public static void Method(List<DataClass> datas, double K)
        {
            List<double[]> myList = new List<double[]>();
            foreach (DataClass x in datas)
            {
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

            //at this point the permutations variable contains all permutations
            string str = "";
            foreach (List<double> list1 in permutations)
            {
                foreach (double item in list1)
                {
                    str += item + ":";

                }
                str += "\n";
            }
            MessageBox.Show(str);
        }

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


        //public static void Method(List<DataClass> datas, double K)
        //{
        //    //for (int i = 0; i <= maxCount[0]; i++)
        //    //{
        //    //    for (int j = 0; j <= maxCount[1]; j++)
        //    //    {
        //    //        if ((calories[0] * i + calories[1] * j >= K) && (weight[0] * i + weight[1] * j < minWeight))
        //    //        {
        //    //            minWeight = weight[0] * i + weight[1] * j;
        //    //            resh[0] = i;
        //    //            resh[1] = j;
        //    //        }
        //    //    }
        //    //}
        //    //Console.WriteLine(resh[0] + " " + resh[1] + " " + minWeight);


        //    double minWeight = 0;
        //    double[] resh = new double[datas.Count];
        //    for (int i = 0; i < datas.Count; i++)
        //    {
        //        minWeight += datas[i].Weight * datas[i].MaxCount;
        //    }

        //    double kalor = 0;
        //    double weight = 0;
        //    for (int i = 0; i < datas.Count; i++)
        //    {
        //        kalor += datas[i].Calories * i + 1;
        //        weight += datas[i].Weight * i + 1;
        //    }
        //    //double[] time = new double[datas.Count];
        //    //while (true)
        //    //{
        //        //for (int i = 0; i < datas.Count; i++)
        //        //{
        //        //for (int j = 0; j <= datas[0].MaxCount; j++)
        //        //{
        //        //    for (int k = 1; k < datas.Count; k++)
        //        //    {
        //        //        for (int i = 0; i <= datas[k].MaxCount; i++)
        //        //        {
                            
        //        //            if ((datas[k].Calories * i + calories[i] * j >= K) && (weight[0] * i + weight[1] * j < minWeight))
        //        //            {

        //        //                minWeight = weight[0] * i + weight[1] * j;
        //        //                resh[0] = i;
        //        //                resh[1] = j;
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //    //}
        //}
    }
}
