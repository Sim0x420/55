using Fintech.AppCode.DB;
using Fintech.AppCode.Interfaces;
using Fintech.AppCode.StaticModel;
using RoundpayFinTech.AppCode.Model;
using RoundpayFinTech.AppCode.Model.ProcModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RoundpayFinTech.AppCode.DL.Employee
{
    public class ProcPSTDetailReportForEmp : IProcedure
    {
        private readonly IDAL _dal;
        public ProcPSTDetailReportForEmp(IDAL dal) => _dal = dal;
        
        public object Call(object obj)
        {
            var _req = (CommonFilter)obj;
            SqlParameter[] param = {
                new SqlParameter("@LoginID", _req.LoginID),
                new SqlParameter("@UserID", _req.UserID),
                new SqlParameter("@FromDate", _req.FromDate ?? DateTime.Now.ToString("dd MMM yyyy")),
                new SqlParameter("@ToDate", _req.ToDate?? DateTime.Now.ToString("dd MMM yyyy"))
            };
            var _alist = new List<PSTReport>();
            try
            {
                var dt = _dal.GetByProcedure(GetName(), param);
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new PSTReport
                    {
                        Statuscode = ErrorCodes.One,
                        Msg = "Success",
                        PriAmount = dr["_Primary"] is DBNull ? 0 : Convert.ToDouble(dr["_Primary"]),
                        SecAmount = dr["_Secoundary"] is DBNull ? 0 : Convert.ToDouble(dr["_Secoundary"]),
                        Recharge = dr["_Recharge"] is DBNull ? 0 : Convert.ToDouble(dr["_Recharge"]),
                        MoneyTransfer = dr["_MoneyTransfer"] is DBNull ? 0 : Convert.ToDouble(dr["_MoneyTransfer"]),
                        BillPayment = dr["_BillPayment"] is DBNull ? 0 : Convert.ToDouble(dr["_BillPayment"]),
                        AEPS = dr["_AEPS"] is DBNull ? 0 : Convert.ToDouble(dr["_AEPS"]),
                        GenralInsurance = dr["_GenralInsurance"] is DBNull ? 0 : Convert.ToDouble(dr["_GenralInsurance"]),
                        Shopping = dr["_Shopping"] is DBNull ? 0 : Convert.ToDouble(dr["_Shopping"]),
                        EServices = dr["_EServices"] is DBNull ? 0 : Convert.ToDouble(dr["_EServices"]),
                        PSAService = dr["_PSAService"] is DBNull ? 0 : Convert.ToDouble(dr["_PSAService"]),
                        DTHSubscription = dr["_DTHSubscription"] is DBNull ? 0 : Convert.ToDouble(dr["_DTHSubscription"]),
                        TransactionDate = Convert.ToString(dr["_TransactionDate"])
                    };
                    _alist.Add(item);
                }
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLog
                {
                    ClassName = GetType().Name,
                    FuncName = "Call",
                    Error = ex.Message,
                    LoginTypeID = _req.LT,
                    UserId = _req.LoginID
                };
                var _ = new ProcPageErrorLog(_dal).Call(errorLog);
            }
            return _alist;
        }

        public object Call() => throw new NotImplementedException();
        public string GetName() => "proc_PSTDetailReportForEmp";
    }
}
