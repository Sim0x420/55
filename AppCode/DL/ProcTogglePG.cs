using Fintech.AppCode.DB;
using Fintech.AppCode.Interfaces;
using Fintech.AppCode.Model;
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
    public class ProcTogglePG : IProcedure
    {
        private readonly IDAL _dal;
        public ProcTogglePG(IDAL dal) => _dal = dal;
        public object Call(object obj)
        {
            var req = (CommonReq)obj;

            var res = new ResponseStatus
            {
                Statuscode = ErrorCodes.Minus1,
                Msg = ErrorCodes.TempError
            };
            SqlParameter[] param = {
                new SqlParameter("@LT", req.LoginTypeID),
                new SqlParameter("@LoginID", req.LoginID),
                new SqlParameter("@ID", req.CommonInt)
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

        public object Call()
        {
            throw new NotImplementedException();
        }

        public string GetName() => "proc_TogglePG";
    }
}
