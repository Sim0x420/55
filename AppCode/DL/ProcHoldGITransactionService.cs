using Fintech.AppCode.DB;
using Fintech.AppCode.Interfaces;
using Fintech.AppCode.StaticModel;
using RoundpayFinTech.AppCode.Model;
using RoundpayFinTech.AppCode.Model.ProcModel;
using System;
using System.Data.SqlClient;

namespace RoundpayFinTech.AppCode.DL
{
    public class ProcHoldGITransactionService : IProcedure
    {
        private readonly IDAL _dal;
        public ProcHoldGITransactionService(IDAL dal)=>_dal=dal;
        public object Call(object obj)
        {
            var req = (GIUpdateRequestModel)obj;
            SqlParameter[] param = {
            new SqlParameter("@APICode",req.APICode??string.Empty),
            new SqlParameter("@APIOpCode",req.APIOpCode??string.Empty),
            new SqlParameter("@TransactionID",req.TransactionID??string.Empty),
            new SqlParameter("@APIOutletID",req.APIOutletID??string.Empty),
            new SqlParameter("@OutletID",req.OutletID),
            new SqlParameter("@RechType",req.RechType??string.Empty),
            new SqlParameter("@VendorID",req.VendorID??string.Empty),
            new SqlParameter("@ActualAmount",req.ActualAmount),
            new SqlParameter("@AccountNo",req.AccountNo??string.Empty),
            new SqlParameter("@RequestIP",req.RequestIP??string.Empty)
            };
            var res = new ResponseStatus
            {
                Statuscode = ErrorCodes.Minus1,
                Msg = ErrorCodes.TempError
            };
            try
            {
                var dt = _dal.GetByProcedure(GetName(), param);
                if (dt.Rows.Count > 0)
                {
                    res.Statuscode = dt.Rows[0][0] is DBNull ? res.Statuscode : Convert.ToInt16(dt.Rows[0][0]);
                    res.Msg = Convert.ToString(dt.Rows[0]["Msg"]);
                    if (res.Statuscode == ErrorCodes.One)
                        res.CommonInt = dt.Rows[0]["_TabID"] is DBNull ? 0 : Convert.ToInt16(dt.Rows[0]["_TabID"]);
                }
            }
            catch (Exception ex)
            {
                var _ = new ProcPageErrorLog(_dal).Call(new ErrorLog
                {
                    ClassName = GetType().Name,
                    FuncName = "Call",
                    Error = ex.Message,
                    UserId = req.OutletID
                });
            }
            return res;
        }

        public object Call()
        {
            throw new System.NotImplementedException();
        }

        public string GetName() => "proc_HoldGITransactionService";
    }
}
