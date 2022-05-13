﻿using Fintech.AppCode.DB;
using Fintech.AppCode.Interfaces;
using Fintech.AppCode.Model;
using Fintech.AppCode.StaticModel;
using RoundpayFinTech.AppCode.Model;
using RoundpayFinTech.AppCode.Model.ProcModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RoundpayFinTech.AppCode.DL
{
    public class ProcUpdatePaymentGateway : IProcedure
    {
        private readonly IDAL _dal;
        public ProcUpdatePaymentGateway(IDAL dal) => _dal = dal;
        public object Call(object obj)
        {
            var req = (PaymentGateway)obj;
            var res = new ResponseStatus
            {
                Statuscode = ErrorCodes.Minus1,
                Msg = ErrorCodes.TempError
            };
            SqlParameter[] param = {
                new SqlParameter("@LT", req.EntryByLT),
                new SqlParameter("@LoginID", req.EntryBy),
                new SqlParameter("@PGAtewayID", req.ID),
                new SqlParameter("@MerchantID", req.MerchantID),
                new SqlParameter("@MerchantKey", req.MerchantKey),
                new SqlParameter("@PGID", req.PGID),
                new SqlParameter("@IsActive", req.IsActive)
            };
            
            try
            {
                var dt = _dal.GetByProcedure(GetName(), param);
                if (dt.Rows.Count > 0)
                {
                    res.Statuscode = Convert.ToInt16(dt.Rows[0][0]);
                    res.Msg = dt.Rows[0]["Msg"] is DBNull ? "" : dt.Rows[0]["Msg"].ToString();
                }
            }

            catch (Exception ex)
            {
                var _ = new ProcPageErrorLog(_dal).Call(new ErrorLog
                {
                    ClassName = GetType().Name,
                    FuncName = "Call",
                    Error = ex.Message
                });
            }
            return res;
        }

        public object Call() => throw new NotImplementedException();

        public string GetName() => "proc_UpdatePGatewy";

    }
}


