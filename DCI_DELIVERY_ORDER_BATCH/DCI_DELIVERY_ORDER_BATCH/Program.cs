using DCI_DELIVERY_ORDER_BATCH.Contexts;
using DCI_DELIVERY_ORDER_BATCH.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCI_DELIVERY_ORDER_BATCH
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BackupStock();
        }

        static void BackupStock()
        {
            DateTime dtNow = DateTime.Now;
            using (var db = new DBSCM())
            {
                List<DoDictMstr> DictVenders = db.DoDictMstr.Where(x => x.DictType == "BUYER" && x.Code == "41256" && x.DictStatus == "999").ToList();
                foreach (DoDictMstr itemVender in DictVenders)
                {
                    //string[] venders = (from x in DictVenders select x.RefCode).ToArray();
                    List<AlPart> Parts = db.AlPart.Where(x => x.VenderCode == itemVender.RefCode).ToList();
                    string partJoin = string.Join("','", (from x in Parts select x.DrawingNo).ToArray());
                    List<MStockAlpha> StockAlpha = GetStockAlpha(DateTime.Now, partJoin);
                    if (StockAlpha.Count > 0)
                    {
                        foreach (MStockAlpha StockLoop in StockAlpha)
                        {
                            DoStockAlpha itemStock = new DoStockAlpha();
                            itemStock.DatePd = DateTime.Now.Date;
                            itemStock.Partno = StockLoop.Part;
                            itemStock.Cm = StockLoop.Cm;
                            itemStock.Vdcode = itemVender.RefCode;
                            itemStock.Stock = StockLoop.Stock;
                            itemStock.Rev = 999;
                            itemStock.InsertDt = DateTime.Now;
                            db.DoStockAlpha.Add(itemStock);
                        }
                    }
                }
                int insert = db.SaveChanges();
                if (insert > 0)
                {
                    List<DoStockAlpha> StockPrev = db.DoStockAlpha.Where(x => x.DatePd.Date < dtNow.Date && x.Rev == 999).ToList();
                    if (StockPrev.Count > 0)
                    {
                        StockPrev.ForEach(a => a.Rev = 1);
                        db.SaveChanges();
                    }
                }
            }
        }

        static List<MStockAlpha> GetStockAlpha(DateTime _DateRunProcess, string PARTS = "")
        {
            OraConnectDB alpha02 = new OraConnectDB("ALPHA02");
            List<MStockAlpha> res = new List<MStockAlpha>();
            string _PART_JOIN_STRING = "";
            string _Year = _DateRunProcess.Year.ToString();
            string _Month = _DateRunProcess.Month.ToString("00");
            string date = _Year + "" + _Month;
            OracleCommand cmd = new OracleCommand();
            PARTS = PARTS != "" ? PARTS : _PART_JOIN_STRING;
            cmd.CommandText = @"SELECT '" + _DateRunProcess.ToString("yyyyMMdd") + "',MC1.PARTNO, MC1.CM, DECODE(SB1.DSBIT,'1','OBSOLETE','2','DEAD STOCK','3',CASE WHEN TRIM(SB1.STOPDATE) IS NOT NULL AND SB1.STOPDATE <= TO_CHAR(SYSDATE,'YYYYMMDD') THEN 'NOT USE ' || SB1.STOPDATE ELSE ' ' END, ' ') PART_STATUS, MC1.LWBAL, NVL(AC1.ACQTY,0) AS ACQTY, NVL(PID.ISQTY,0) AS ISQTY, MC1.LWBAL + NVL(AC1.ACQTY,0) - NVL(PID.ISQTY,0) AS WBAL,NVL(RT3.LREJ,0) + NVL(PID.REJIN,0) - NVL(AC1.REJOUT,0) AS REJQTY, MC2.QC, MC2.WH1, MC2.WH2, MC2.WH3, MC2.WHA, MC2.WHB, MC2.WHC, MC2.WHD, MC2.WHE,ZUB.HATANI AS UNIT, EPN.KATAKAN AS DESCR, F_GET_HTCODE_RATIO(MC1.JIBU,MC1.PARTNO, '" + _DateRunProcess.ToString("yyyyMMdd") + "') AS HTCODE, F_GET_MSTVEN_VDABBR(MC1.JIBU,F_GET_HTCODE_RATIO(MC1.JIBU,MC1.PARTNO,'" + _DateRunProcess.ToString("yyyyMMdd") + "')) SUPPLIER, SB1.LOCA1, SB1.LOCA2, SB1.LOCA3, SB1.LOCA4, SB1.LOCA5, SB1.LOCA6, SB1.LOCA7, SB1.LOCA8 FROM	(SELECT	* FROM	DST_DATMC1 WHERE	TRIM(YM) = :YM AND TRIM(PARTNO) IN ('" + PARTS + "') AND CM LIKE '%'";
            cmd.CommandText = cmd.CommandText + @") MC1, 
        		(SELECT	PARTNO, CM, SUM(WQTY) AS ACQTY, SUM(CASE WHEN WQTY < 0 THEN -1 * WQTY ELSE 0 END) AS REJOUT 
        		 FROM	DST_DATAC1 
        		 WHERE	ACDATE >= :DATE_START 
        			AND	ACDATE <= :DATE_RUN  AND TRIM(PARTNO) LIKE '%'  AND CM LIKE '%'
        		 GROUP BY PARTNO, CM 
        		) AC1, 
        		(SELECT	PARTNO, BRUSN AS CM, SUM(FQTY) AS ISQTY, SUM(DECODE(REJBIT,'R',-1*FQTY,0)) AS REJIN 
        		 FROM	MASTER.GST_DATPID@ALPHA01 
        		 WHERE	IDATE >= :DATE_START 
        			AND	IDATE <= :DATE_RUN  AND TRIM(PARTNO) LIKE '%'  AND BRUSN LIKE '%'
        		 GROUP BY PARTNO, BRUSN 
        		) PID, 
        		(SELECT    PARTNO, CM, SUM(DECODE(WHNO,'QC',BALQTY)) AS QC,SUM(DECODE(WHNO,'W1',BALQTY)) AS WH1,SUM(DECODE(WHNO,'W2',BALQTY)) AS WH2,SUM(DECODE(WHNO,'W3',BALQTY)) AS WH3, 
                           SUM(DECODE(WHNO,'WA',BALQTY)) AS WHA,SUM(DECODE(WHNO,'WB',BALQTY)) AS WHB,SUM(DECODE(WHNO,'WC',BALQTY)) AS WHC,SUM(DECODE(WHNO,'WD',BALQTY)) AS WHD,SUM(DECODE(WHNO,'WE',BALQTY)) AS WHE 
                    FROM    (SELECT    MC2.PARTNO, MC2.CM, MC2.WHNO, MC2.LWBAL, NVL(AC1.ACQTY,0) AS ACQTY, NVL(PID.ISQTY,0) AS ISQTY, MC2.LWBAL + NVL(AC1.ACQTY,0) - NVL(PID.ISQTY,0) AS BALQTY 
                            FROM    (SELECT    * 
                                    FROM    DST_DATMC2 
                                    WHERE    YM = :YM  AND TRIM(PARTNO) LIKE '%' AND CM LIKE '%'
                                   ) MC2, 
                                   (SELECT    PARTNO, CM, WHNO, SUM(WQTY) AS ACQTY 
                                    FROM    DST_DATAC1 
                                    WHERE    ACDATE >= :DATE_START 
                                       AND    ACDATE <= :DATE_RUN  AND TRIM(PARTNO) LIKE '%' AND CM LIKE '%'
                                    GROUP BY PARTNO, CM, WHNO 
                                   ) AC1, 
                                   (SELECT    PARTNO, BRUSN AS CM, WHNO, SUM(FQTY) AS ISQTY 
                                    FROM    (SELECT    * 
                                             FROM    MASTER.GST_DATPID@ALPHA01 
                                             WHERE    IDATE >= :DATE_START 
                                               AND    IDATE <= :DATE_RUN  AND TRIM(PARTNO) LIKE '%' AND BRUSN LIKE '%'
                                            UNION ALL 
                                             SELECT    * 
                                             FROM    DST_DATPID3 
                                             WHERE    IDATE >= :DATE_START 
                                               AND    IDATE <= :DATE_RUN  AND TRIM(PARTNO) LIKE '%' AND BRUSN LIKE '%'
                                           ) 
                                    GROUP BY PARTNO, BRUSN, WHNO 
                                   ) PID 
                            WHERE    MC2.PARTNO    = AC1.PARTNO(+) 
                               AND    MC2.CM        = AC1.CM(+) 
                               AND    MC2.WHNO    = AC1.WHNO(+) 
                               AND    MC2.PARTNO    = PID.PARTNO(+) 
                               AND    MC2.CM        = PID.CM(+) 
                               AND    MC2.WHNO    = PID.WHNO(+) 
                           ) 
                    GROUP BY PARTNO, CM 
                   ) MC2, 
                   MASTER.ND_EPN_TBL_V1@ALPHA01 EPN, DST_MSTSB1 SB1, MASTER.ND_ZUB_TBL@ALPHA01 ZUB, DST_DATRT3 RT3 
           WHERE    MC1.PARTNO    = AC1.PARTNO(+) 
               AND    MC1.CM        = AC1.CM(+) 
               AND    MC1.PARTNO    = PID.PARTNO(+) 
               AND    MC1.CM        = PID.CM(+) 
               AND    MC1.YM        = RT3.YM(+) 
               AND    MC1.PARTNO    = RT3.PARTNO(+) 
               AND    MC1.CM        = RT3.CM(+) 
               AND    MC1.PARTNO    = EPN.PARTNO(+) 
               AND    MC1.PARTNO    = SB1.PARTNO(+) 
               AND    MC1.CM        = SB1.CM(+) 
               AND    MC1.PARTNO    = MC2.PARTNO(+) 
               AND    MC1.CM        = MC2.CM(+) 
               AND    MC1.PARTNO    = ZUB.PARTNO(+) 
               AND    ZUB.STRYMN(+) <= :DATE_START 
               AND    ZUB.ENDYMN(+) >  :DATE_RUN 
               AND    ZUB.KSNBIT(+) <> '2'";
            cmd.Parameters.Add(new OracleParameter(":YM", date));
            cmd.Parameters.Add(new OracleParameter(":DATE_START", _DateRunProcess.ToString("yyyyMM01")));
            cmd.Parameters.Add(new OracleParameter(":DATE_RUN", _DateRunProcess.ToString("yyyyMMdd")));
            DataTable dt = alpha02.Query(cmd);
            foreach (DataRow dr in dt.Rows)
            {
                res.Add(new MStockAlpha()
                {
                    Part = dr["PARTNO"].ToString().Trim(),
                    Stock = double.Parse(dr["WBAL"].ToString()),
                    Cm = dr["CM"].ToString().Trim()
                });
            }
            return res;
        }
    }
}
