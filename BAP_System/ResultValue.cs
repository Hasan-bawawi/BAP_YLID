using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace BAP_System.Models
{
    public class ResultValue
    {
        public bool IsSuccess { get; set; }
        public string ErrorRet { get; set; }
        public string Identity { get; set; }
        public DataTable Data { get; set; }

        public ResultValue()
        {
            IsSuccess = false;
            ErrorRet = string.Empty;
            Identity = string.Empty;
            Data = new DataTable();
        }
    }

    public class RMA
    {

        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }


    }

    public class Submenu
    {

        public bool Submenu1 { get; set; }
        public bool Submenu2 { get; set; }
        public bool Submenu3 { get; set; }




    }



}