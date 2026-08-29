using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using tcore2.progress;

namespace tcore2.global
{

    //[DebuggerStepThrough]
    public partial class clsGlobal
    {
        
        public static decimal parseDecimal(object sVal, int iRound = -1)
        {
            if (sVal == null) return 0;

            decimal iNum = 0;
            decimal.TryParse(sVal.ToString().Replace(",", ""), out iNum);

            if (iRound > -1)
                return Math.Round(iNum, iRound);
            return iNum;
        }
        public static double parseDouble(object sVal, int iRound = -1)
        {
            if (sVal == null) return 0;

            double iNum = 0;
            double.TryParse(sVal.ToString().Replace(",", ""), out iNum);

            if (iRound > -1)
                return Math.Round(iNum, iRound);
            return iNum;
        }


        public static int parseInt(object sVal)
        {
            if (sVal == null) return 0;

            int iNum = 0;
            int.TryParse(sVal.ToString().Replace(",", ""), out iNum);
            return iNum;
        }
        
        
        public static DateTime parseDate(string sDate, string sFormat)
        {
            try
            {
                return DateTime.ParseExact(sDate, sFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new Exception_Core(ex.Message);
            }
        }

        public static string getNextVoucherNo(string str1)
        {

            if (string.IsNullOrWhiteSpace(str1))
                return "001";

            //start
            string strNum = "";
            char[] cc = str1.ToCharArray();

            int iNumberCount = 0;

            for (int i = cc.Length; i > 0; i--)
            {
                if (char.IsNumber(cc[i - 1]))
                {
                    strNum = cc[i - 1] + strNum;
                    iNumberCount++;
                }
                else
                    break;
            }

            if (string.IsNullOrWhiteSpace(strNum))
                strNum = "0";

            int iNum = Convert.ToInt32(strNum) + 1;
            return str1.Substring(0, str1.Length - iNumberCount) + iNum.ToString(new String('0', iNumberCount));
        }

        public static clsMessageModel msg(string sMsg = "")
        {
            clsMessageModel model = new clsMessageModel();
            model.Message = sMsg;
            model.success = string.IsNullOrWhiteSpace(sMsg) ? true : false;
            return model;
        }


        public static string replaceString(string str1 ,clsCommand  cmd,char cPrefix='{' ,char cSuffix = '}')
        {

            StringBuilder sb1 = new StringBuilder(str1);
            int iCount = cmd.getCount();
            
            for(int i = 0; i < iCount;i++)
            {

                clsParam p = cmd.prm[i];
                sb1.Replace(cPrefix + p.name +  cSuffix, cmd.getString(i));
            }

            return sb1.ToString();
        }

    }
}
