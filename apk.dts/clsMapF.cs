using apk.dts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apk.dts
{
    public static class clsMapF
    {

        public static void add(List<clsColMap> objMap
            , string sSource
            , string sDestination
            , string sDefaultValue)
        {

            clsColMap map = new clsColMap();

            map.Source = sSource;
            map.Dest = sDestination;
            map.defaultValue = sDefaultValue;
            objMap.Add(map);
        }

        public static void setColIndexes(List<clsColMap> objMap, DataRow rSource
            , DataRow rDest)
        {

            setColIndexes(objMap, rSource.Table, rDest.Table);
        }

        public static void setColIndexes(List<clsColMap> objMap, DataTable tSource, DataTable tDest)
        {

            foreach (clsColMap map in objMap)
            {

                map.iDest = tDest.Columns[map.Dest].Ordinal;

                if (string.IsNullOrWhiteSpace(map.Source))
                {
                    map.iSource = -1;
                }
                else
                {
                    map.iSource = tSource.Columns[map.Source].Ordinal;
                }
            }
        }

        public static void fillRowByIndex(List<clsColMap> objMap, DataRow rSource, DataRow rDest)
        {

            foreach (clsColMap map in objMap)
            {
                if (map.iSource > -1)
                {
                    rDest[map.iDest] = rSource[map.iSource];
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(map.defaultValue))
                    {
                        rDest[map.iDest] = map.defaultValue;
                    }
                }
            }
        }

        public static void moveData(List<clsColMap> map
            ,DataTable tSource
            ,DataTable tDest)
        {

            clsMapF.setColIndexes(map, tSource, tDest);

            foreach(DataRow rSource in tSource.Rows)
            {
                DataRow rDest = tDest.NewRow();
                fillRowByIndex(map,rSource,rDest);
                tDest.Rows.Add(rDest);
            }
        }

    }
}
