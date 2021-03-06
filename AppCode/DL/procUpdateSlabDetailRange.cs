using Fintech.AppCode.DB;
using Fintech.AppCode.Interfaces;
using Fintech.AppCode.StaticModel;
using RoundpayFinTech.AppCode.Model;
using RoundpayFinTech.AppCode.Model.ProcModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RoundpayFinTech.AppCode.DL
{
    public class procUpdateSlabDetailRange : IProcedure
    {
        private readonly IDAL _dal;
        public procUpdateSlabDetailRange(IDAL dal) => _dal = dal;
        public object Call(object obj)
        {
            RangeCommissionReq req = (RangeCommissionReq)obj;
            IResponseStatus res = new ResponseStatus
            {
                Statuscode = ErrorCodes.Minus1,
                Msg = ErrorCodes.TempError
            };
            SqlParameter[] param =
            {
                new SqlParameter("@LT",req.LoginTypeID),
                new SqlParameter("@LoginID",req.LoginID),
                new SqlParameter("@SlabID",req.Commission.SlabID),
                new SqlParameter("@OID",req.Commission.OID),
                new SqlParameter("@Comm",req.Commission.Comm),
                new SqlParameter("@CommType",req.Commission.CommType),
                new SqlParameter("@AmtType",req.Commission.AmtType),
                new SqlParameter("@IP",req.CommonStr??""),
                new SqlParameter("@Browser",req.CommonStr2??""),
                new SqlParameter("@SlabRoleID",req.Commission.RoleID),
                new SqlParameter("@RangeId",req.Commission.RangeId),
                new SqlParameter("@MaxComm",req.Commission.MaxComm),
                new SqlParameter("@FixedCharge",req.Commission.FixedCharge),
                 new SqlParameter("@RComm",req.Commission.RComm),
                new SqlParameter("@RCommType",req.Commission.RCommType),
                new SqlParameter("@RAmtType",req.Commission.RAmtType),
                 new SqlParameter("@RMaxComm",req.Commission.RMaxComm),
                new SqlParameter("@RFixedCharge",req.Commission.RFixedCharge)
            };

            try
            {
                DataTable dt = _dal.GetByProcedure(GetName(), param);
                if (dt.Rows.Count > 0)
                {
                    res.Statuscode = Convert.ToInt16(dt.Rows[0][0]);
                    res.Msg = dt.Rows[0]["Msg"] is DBNull ? "" : dt.Rows[0]["Msg"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog errorLog = new ErrorLog
                {
                    ClassName = GetType().Name,
                    FuncName = "Call",
                    Error = ex.Message,
                    LoginTypeID = req.LoginTypeID,
                    UserId = req.LoginID
                };
                var _ = new ProcPageErrorLog(_dal).Call(errorLog);
            }
            return res;
        }

        public object Call() => throw new NotImplementedException();

        public string GetName() => "proc_UpdateSlabDetail_Range";
    }
}

