﻿using Fintech.AppCode.DB;
using Fintech.AppCode.Interfaces;
using Fintech.AppCode.Model;
using Fintech.AppCode.StaticModel;
using RoundpayFinTech.AppCode.Model;
using RoundpayFinTech.AppCode.Model.ProcModel;
using System;
using System.Data.SqlClient;



namespace RoundpayFinTech.AppCode.DL
{
    public class ProcSaveAffiliateVendor : IProcedure
    {
        private readonly IDAL _dal;
        public ProcSaveAffiliateVendor(IDAL dal) => _dal = dal;

        public object Call(object obj)
        {
            var req = (CommonReq)obj;
            var res = new ResponseStatus
            {
                Statuscode = ErrorCodes.Minus1,
                Msg = ErrorCodes.TempError
            };
            SqlParameter[] param = {
                new SqlParameter("@LT",req.LoginTypeID),
                new SqlParameter("@LoginID",req.LoginID),
                new SqlParameter("@VendorName",req.CommonStr),
            //  new SqlParameter("@VendorIcon",req.CommonStr1??string.Empty),
                new SqlParameter("@VendorBanner",req.CommonStr2),
                new SqlParameter("@IsActive",req.CommonBool),
                new SqlParameter("@ID",req.CommonInt),
            };
            try
            {
                var dt = _dal.GetByProcedure(GetName(), param);
                if (dt.Rows.Count > 0)
                {
                    res.Statuscode = Convert.ToInt16(dt.Rows[0][0]);
                    res.Msg = dt.Rows[0]["Msg"] is DBNull ? "" : dt.Rows[0]["Msg"].ToString();
                    res.CommonInt = dt.Rows[0]["Id"] is DBNull ? 0 : Convert.ToInt32(dt.Rows[0]["Id"]);
                }
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLog
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
        public string GetName() => "Proc_SaveAffiliateVendor";
    }
}
