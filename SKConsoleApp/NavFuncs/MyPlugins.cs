using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKConsoleApp.NavFuncs
{
    public class MyPlugins
    {
        [KernelFunction, Description("描述這個function的用途")]
        public string NatvieFunSample()
        {
            return "SK Natvie Function Sample"; //do something
        }

        [KernelFunction, Description("轉換阿拉伯數字為國字大寫數字")]
        public static string ConvertToChineseNumber([Description("阿拉伯數字")] long number)
        {
            if (number == 0) return "零";

            string[] units = { "", "拾", "佰", "仟" };
            string[] bigUnits = { "", "萬", "億", "兆" };
            StringBuilder sb = new StringBuilder();
            bool hasNonZero = false;

            int unitIndex = 0;
            int bigUnitIndex = 0;

            while (number > 0)
            {
                int section = (int)(number % 10000);
                if (section > 0)
                {
                    sb.Insert(0, ConvertSectionToChinese(section) + bigUnits[bigUnitIndex]);
                    hasNonZero = true;
                }
                else if (hasNonZero)
                {
                    sb.Insert(0, "零");
                }

                number /= 10000;
                bigUnitIndex++;
                unitIndex = 0;
            }

            return sb.ToString();
        }

        private static string ConvertSectionToChinese(int number)
        {
            string[] digits = { "", "壹", "貳", "叁", "肆", "伍", "陸", "柒", "捌", "玖" };
            string[] units = { "", "拾", "佰", "仟" };

            StringBuilder sb = new StringBuilder();
            int unitIndex = 0;

            while (number > 0)
            {
                int digit = number % 10;
                if (digit > 0)
                {
                    sb.Insert(0, digits[digit] + units[unitIndex]);
                }
                number /= 10;
                unitIndex++;
            }

            return sb.ToString();
        }
    }
}
