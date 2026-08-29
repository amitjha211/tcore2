using apk;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static apk.clsAPKF_FS_F;

namespace g.progress
{
    public static class progress_f
    {

        public static progress_model progress_create(clsAPK _apk)
        {
            progress_model _p = new progress_model();
            JArray _arr = f_getJArray(_apk, "@/common/g/progress/sample.json");
            _p.tProgress = _arr.ToObject<DataTable>();
            return _p;
        }


        public static void progress_apply_console(progress_model _p)
        {

            _p.onAction = (object sender, string sAction) =>
            {
                progress_model p = (progress_model)sender;

                switch (sAction)
                {
                    case "start":
                        Console.WriteLine($"{p.progressTitle}   ---- [Started]");
                        Console.BackgroundColor = ConsoleColor.Green;
                        break;
                    case "working":
                        Console.Write($" ");
                        break;
                    case "end":
                        Console.ResetColor();
                        Console.WriteLine("");
                        Console.WriteLine($"{p.progressTitle}   ---- [Done]");
                        break;
                }

            };
        }
        public static void progress_start(progress_model _p, string sName, string sTitle)
        {
            if (_p == null) return;

            _p.progressName = sName;
            _p.progressTitle = sTitle;
            _p.per = 0;

            raise_event(_p, "start");
        }

        public static void progress_working(progress_model _p, int dTotal, int dComplete)
        {
            if (_p == null) return;
            if (dTotal > 0 && dComplete > 0)
            {
                decimal _per = (dComplete * 100) / dTotal;
                //_p.per = _per;
                if (_per > Math.Round(_p.per, 0))
                {
                    _p.per = _per;
                    raise_event(_p, "working");
                }
            }
        }

        public static void progress_working(progress_model _p, decimal per)
        {
            if (_p == null) return;
            _p.per = per;

            raise_event(_p, "working");
        }

        public static void progress_end(progress_model _p)
        {
            if (_p == null) return;
            _p.currentStatus = "end";
            _p.per = 100;

            raise_event(_p, "working");

            DataRow r = _p.tProgress.NewRow();
            r["progress_name"] = _p.progressName;
            r["progress_title"] = _p.progressTitle;
            r["progress_per"] = _p.per;
            r["progress_status"] = "done";
            _p.tProgress.Rows.Add(r);

            raise_event(_p, "end");

        }



        private static void raise_event(progress_model _p, string sName)
        {
            if (_p != null && _p.onAction != null)
            {
                _p.onAction(_p, sName);
            }
        }

    }
}
