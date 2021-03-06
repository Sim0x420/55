using Fintech.AppCode.DB;
using Fintech.AppCode.Interfaces;
using Fintech.AppCode.StaticModel;
using RoundpayFinTech.AppCode.Model;
using RoundpayFinTech.AppCode.Model.ProcModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RoundpayFinTech.AppCode.DL
{
    public class ProcUpdateCustomerSmartAccountDetail : IProcedure
    {
        private readonly IDAL _dal;
        public ProcUpdateCustomerSmartAccountDetail(IDAL dal) => _dal = dal;
        public object Call(object obj)
        {
            var req = (UpdateSmartCollectRequestModel)obj;
            if (req.tp_SmartCollect == null) {
                req.tp_SmartCollect = new System.Data.DataTable();
                req.tp_SmartCollect.Columns.Add("_BankCode", typeof(string));
                req.tp_SmartCollect.Columns.Add("_AccountNo", typeof(string));
                req.tp_SmartCollect.Columns.Add("_IFSC", typeof(string));
            }
            
            SqlParameter[] param = {
                new SqlParameter("@LoginID",req.LoginID),
                new SqlParameter("@UserID",req.UserID),
                new SqlParameter("@SmartCollectTypeID",req.SmartCollectTypeID),
                new SqlParameter("@CustomerID",req.CustomerID??string.Empty),
                new SqlParameter("@SmartAccountNo",req.SmartAccountNo??string.Empty),
                new SqlParameter("@SmartVPA",req.SmartVPA??string.Empty),
                new SqlParameter("@SmartQRShortURL",req.SmartQRShortURL??string.Empty),
                new SqlParameter("@tp_SmartCollect",req.tp_SmartCollect),
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
                    res.Statuscode = dt.Rows[0][0] is DBNull ? 0 : Convert.ToInt16(dt.Rows[0][0]);
                    res.Msg = dt.Rows[0]["Msg"] is DBNull ? string.Empty : dt.Rows[0]["Msg"].ToString();
                }
            }
            catch (Exception ex)
            {
                var _ = new ProcPageErrorLog(_dal).Call(new ErrorLog
                {
                    ClassName = GetType().Name,
                    FuncName = "Call",
                    Error = ex.Message,
                    LoginTypeID = LoginType.ApplicationUser,
                    UserId = req.LoginID
                });
            }
            return res;
        }

        public object Call() => throw new NotImplementedException();

        public string GetName() => "Proc_UpdateCustomerSmartAccountDetail";
    }
}
