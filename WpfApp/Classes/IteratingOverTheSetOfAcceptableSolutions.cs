using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Classes
{
    public class IteratingOverTheSetOfAcceptableSolutions
    {
        public static void Method(List<DataClass> datas, double K)
        {
            //for (int i = 0; i <= maxCount[0]; i++)
            //{
            //    for (int j = 0; j <= maxCount[1]; j++)
            //    {
            //        if ((calories[0] * i + calories[1] * j >= K) && (weight[0] * i + weight[1] * j < minWeight))
            //        {
            //            minWeight = weight[0] * i + weight[1] * j;
            //            resh[0] = i;
            //            resh[1] = j;
            //        }
            //    }
            //}
            //Console.WriteLine(resh[0] + " " + resh[1] + " " + minWeight);


            double minWeight = 0;
            double[] resh = new double[datas.Count];
            for (int i = 0; i < datas.Count; i++)
            {
                minWeight += datas[i].Weight * datas[i].MaxCount;
            }

            double kalor = 0;
            double weight = 0;
            for (int i = 0; i < datas.Count; i++)
            {
                kalor += datas[i].Calories * i + 1;
                weight += datas[i].Weight * i + 1;
            }
            //double[] time = new double[datas.Count];
            //while (true)
            //{
                //for (int i = 0; i < datas.Count; i++)
                //{
                //for (int j = 0; j <= datas[0].MaxCount; j++)
                //{
                //    for (int k = 1; k < datas.Count; k++)
                //    {
                //        for (int i = 0; i <= datas[k].MaxCount; i++)
                //        {
                            
                //            if ((datas[k].Calories * i + calories[i] * j >= K) && (weight[0] * i + weight[1] * j < minWeight))
                //            {

                //                minWeight = weight[0] * i + weight[1] * j;
                //                resh[0] = i;
                //                resh[1] = j;
                //            }
                //        }
                //    }
                //}
            //}
        }
    }
}
