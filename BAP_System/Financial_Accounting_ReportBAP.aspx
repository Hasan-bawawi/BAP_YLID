
<%@ Page Title="Financial Accounting Report" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Financial_Accounting_ReportBAP.aspx.cs" Inherits="BAP_System.Financial_Accounting_ReportBAP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
      <script type="text/javascript" src="date/JS/jquery-1.10.2.min.js"></script>
      <link href="Content/Ionicons/css/ionicons.min.css" rel="stylesheet" />
      <link href="vendors/sweetalert.css" rel="stylesheet" />
      <link href="css/sweetalert.css" rel="stylesheet" />
      <script src="js/sweetalert.min.js"></script>
      <script src="quixlab/plugins/tables/js/datatable/dataTables.fixedColumns.min.js"></script>
      <script src="quixlab/plugins/tables/js/datatable/dataTables.fixedColumns.js"></script>
      <script src="quixlab/plugins/timepicker/bootstrap-timepicker.min.js"></script>
      <script src="quixlab/plugins/bootstrap-daterangepicker/daterangepicker.js"></script>
      <script src="quixlab/plugins/moment/moment.js"></script>
      <script src="quixlab/plugins/bootstrap-material-datetimepicker/js/bootstrap-material-datetimepicker.js"></script>
      <link href="vendors/toastr/toastr.min.css" rel="stylesheet" />
      <script src="vendors/toastr/toastr.min.js"></script>

      <script src="css/css_intro/intro.min.js"></script>
      <link href="css/css_intro/introjs.min.css" rel="stylesheet" />
     <script>

      



         $(document).ready(function () {
             //var $startPicker = $('.datepicker1.start');
             //var $endPicker = $('.datepicker1.end');

             //function parseToMoment(value) {
             //    if (!value) return null;
             //    return moment("01-" + value, "DD-MM-YYYY"); /
             //}

             $('.datepicker1').bootstrapMaterialDatePicker({
                 format: 'MM-YYYY',
                 time: false,
                 clearButton: true,
                 okText: 'Pilih',
                 cancelText: 'Batal',
                 date: true,
                 monthPicker: true,
                 year: true
             }).on('beforeShow', function () {

                 setTimeout(hideDateElements, 50);

             }).on('change', function () {

                 setTimeout(hideDateElements, 50);

   
             }).on('open', function () {

                 setTimeout(hideDateElements, 50);
                



             });

             function hideDateElements() {
                 $('.dtp-actual-num').hide();
                 $('.dtp-picker-days').hide();
                 $('.dtp-calendar').hide();
             }
         });

     </script>

<%--   
    <script type="text/javascript">


        let checkInterval = null;

        function Errorsave(message) {

            debugger
            console.log("Errorsave called:", message);

            if (checkInterval) {
                console.log("clearing checkInterval...");
                clearInterval(checkInterval);
                checkInterval = null;
            } else {
                console.log("checkInterval kosong atau sudah null");
            }

            hideLoading();

            swal({
                title: 'Failed!',
                text: message,
                icon: 'error',
                buttons: false,
                timer: 3000
            });
        }




        function ShowLoading() {

            console.log("ShowLoading called"); // debug

            document.getElementById("loadingOverlay").style.display = "block";
            // Cek cookie setiap 500ms
            let checkInterval = setInterval(function () {
                if (document.cookie.indexOf("fileDownload=true") !== -1) {
                    clearInterval(checkInterval);

                    hideLoading();

                    // Hapus cookie
                    document.cookie = "fileDownload=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT";

                    location.reload();
                }
            }, 500);
            
        }

        function hideLoading() {
            debugger    
            //console.log("hideLoading called"); // debug
            //document.getElementById("loadingOverlay").style.display = "none";
            console.log("hideLoading dipanggil");

            const overlay = document.getElementById("loadingOverlay");

            if (!overlay) {
                console.warn("❌ Element #loadingOverlay tidak ditemukan!");
                return;
            }

            console.log("Sebelum hide: display =", overlay.style.display);

            overlay.style.display = "none";

            console.log("Setelah hide: display =", overlay.style.display);

            // Tambahan: langsung paksa pakai inline style
            overlay.style.setProperty("display", "none", "important");

        }


        function handleClientClick() {

        var code = document.getElementById("<%= ddlReport.ClientID %>");

            if (code && (code.value === "0" || code.value === "")) {
            code.style.setProperty("border", "3px solid red", "important");
            var visual = code.closest(".bootstrap-select");
            if (visual) {
                var btn = visual.querySelector("button");
                if (btn) btn.style.setProperty("border", "3px solid red", "important");
            }
            code.focus();
            return;
        } else if (code) {
            code.style.setProperty("border", "", "important");
            var visual = code.closest(".bootstrap-select");
            if (visual) {
                var btn = visual.querySelector("button");
                if (btn) btn.style.setProperty("border", "", "important");
            }
        }

            ShowLoading();
            document.forms[0].target = "downloadFrame";


        var btnSubmit = document.getElementById("<%= btnSubmit.ClientID %>");
            if (btnSubmit) {
                btnSubmit.click();
            }
        }


        function handleClientClickBS() {
           
        ShowLoading();
        document.forms[0].target = "downloadFrame";


            var btnSubmit = document.getElementById("<%= btnSubmit2.ClientID %>");
            if (btnSubmit) {
                btnSubmit.click();
            }
        }

    </script>--%>

  <script type="text/javascript">

      let checkInterval = null;

      function ShowLoading() {
          console.log("ShowLoading called");

          const overlay = document.getElementById("loadingOverlay");
          overlay.style.display = "block";


          checkInterval = setInterval(function () {
              const cookies = document.cookie;

              if (cookies.includes("fileDownload=success")) {
                  //console.log("✅ Download sukses");
                  clearInterval(checkInterval);
                  checkInterval = null;

                  hideLoading();
                  document.cookie = "fileDownload=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT";

                  // Optional reload, kalau memang perlu
                  location.reload();
              }

              if (cookies.includes("fileDownload=error")) {
                  //console.log("❌ Download gagal");
                  clearInterval(checkInterval);
                  checkInterval = null;

                  hideLoading();
                  document.cookie = "fileDownload=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT";

                  Errorsave("Periksa kembali bulan yang dipilih, sesuaikan dengan periode yang aktif!");

                  /* location.reload();*/
                  setTimeout(() => {
                      location.reload();
                  }, 3000);

              }
          }, 500);

      }

      function hideLoading() {

          console.log("hideLoading dipanggil");

          const overlay = document.getElementById("loadingOverlay");

          if (!overlay) {
              //console.warn("❌ #loadingOverlay tidak ditemukan");
              return;
          }

          overlay.style.setProperty("display", "none", "important");
      }

      function Errorsave(message) {

          swal({

              title: 'Failed!',
              text: message,
              type: 'error',
              showConfirmButton: false,
              timer: 3000

          });
      }

      function handleClientClick() {
          var ddl = document.getElementById("<%= ddlReport.ClientID %>");
        if (ddl && (ddl.value === "0" || ddl.value === "")) {
            ddl.style.setProperty("border", "3px solid red", "important");
            var visual = ddl.closest(".bootstrap-select");
            if (visual) {
                var btn = visual.querySelector("button");
                if (btn) btn.style.setProperty("border", "3px solid red", "important");
            }
            ddl.focus();
            return;
        }

        // Reset border jika valid
        ddl.style.setProperty("border", "", "important");
        var visual = ddl.closest(".bootstrap-select");
        if (visual) {
            var btn = visual.querySelector("button");
            if (btn) btn.style.setProperty("border", "", "important");
        }

        ShowLoading();
        document.forms[0].target = "downloadFrame";

        // Klik tombol ASP.NET tersembunyi
        const btnSubmit = document.getElementById("<%= btnSubmit.ClientID %>");
        if (btnSubmit) btnSubmit.click();
    }

    function handleClientClickBS() {
        ShowLoading();
        document.forms[0].target = "downloadFrame";

        const btnSubmit2 = document.getElementById("<%= btnSubmit2.ClientID %>");
          if (btnSubmit2) btnSubmit2.click();
      }
  </script>



    <style>
        

        fieldset.scheduler-border {
            border: 1px groove #ff6d10 !important;
            padding: 0 1.4em 1.4em 1.4em !important;
            margin: 0 0 1.5em 0 !important;
            box-shadow: 0px 0px 0px 0px #000;
        }

        legend.scheduler-border {
            width: inherit;
            padding: 0 10px;
            border-bottom: none;
            color: #06183d;
        }

        .buttonColor {
            background-color: #06183d;
            color: white;
        }

        .buttonColor:hover {
            background-color: #ff6d10;
            color: white;
        }

        .buttonColorGridview {
            background-color: #ff6d10;
            color: white;
        }

        .buttonColorGridview:hover {
            background-color: #06183d;
            color: white;
        }

        .page-head {
            background-color: #06183d;
            color: white;
            padding: 1px 2.938rem;
            height: 182px;
            position: relative;
            margin-top: 0px;
            margin-left: 0px;
        }

        .custom-card-body {
            position: relative;
            top: -99px;
        }

        .dtp-actual-num,
        .dtp-picker-days,
        .dtp-calendar {
            display: none !important;
        }

        .dtp-header {
            background-color: #ff6d10 !important;
        }


        .dtp-btn-ok, .dtp-btn-cancel, .dtp-btn-clear{
            background-color: #06183d !important;
            color: #ff6d10 !important;
        }

        .dtp-btn-ok,
        .dtp-btn-cancel,
        .dtp-btn-clear {
            border: 1px solid #ff6d10 !important;
            outline: none !important;
            box-shadow: none !important;
            margin-left: 8px;
        }

        .dtp-date{
            background-color: #ff6d10 !important;
            color: black !important;

        }


        .dtp-content {
                background-color: #06183d !important;
            }

        .dtp-select-month-before,
        .dtp-select-month-after,
        .dtp-select-year-before,
        .dtp-select-year-after {
            color: black !important;
        }

        .dtp-header .dtp-actual-day {
            display: none !important;
        }

        .dtp-header .dtp-close {
            top: -1px;
            right: 10px;
            position: absolute;
        }

        .dtp-header {
            height: 25px !important; 
            position: relative;
            background-color: #ff6d10 !important; 
        }


        .wide-input {
        width: 300px;
        }

       .bootstrap-select .dropdown-toggle {
            background-color: #ff6d10 !important;
            color: white !important;
            border: 1px solid #ff6d10 !important;
            height: 40px !important;
            padding-top: 8px !important;
            padding-bottom: 8px !important;
            padding-left: 12px !important;
            padding-right: 30px !important; /* Tambahkan ini agar arrow tidak nempel */
            display: flex !important;
            align-items: center !important;
            justify-content: space-between !important; /* Distribusi teks dan ikon */
             border-radius: 10px !important;
        }

        .bootstrap-select .dropdown-toggle:hover {
            background-color: #e65c00 !important;
            border-color: #e65c00 !important;
        }

    
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-head">
        <div id="page-title">
            <h1 class="page-header text-overflow" style="color: white;">Financial Accounting Report</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="Home.aspx">Home</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Report</li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Financial Accounting Report</li>
        </ol>
    </div>


<div class="container-fluid">
    <div class="row">
        <div class="col-md-12 col-sm-12">
            <div class="card custom-card-body">
                <div class="card-body">
                    <asp:HiddenField ID="BranchCode" runat="server" />
                    <asp:HiddenField ID="BranchCode2" runat="server" />
                    <div class="row">
                        <div class="col-xl-12">
                            <div class="card border-left-primary shadow h-100 py-2">
                                <div class="card-body"> <!-- card 1  -->
                                    <div class="row no-gutters align-items-center">
                                        <div class="col-12">
                                            <div class="form-group">
                                             <div style="display: inline-block; background-color: #06183d; transform: skew(-10deg); padding: 5px 10px;">
                                                        <label for="txtDate4" 
                                                               class="font-weight-bold" 
                                                               style="font-size: 30px; font-style: italic; color: white; display: inline-block; transform: skew(10deg);">
                                                            Profit & Loss
                                                        </label>
                                                    </div>
                                                <div class="row mt-4">
                                                    <!-- Periode -->
                                              
                                                    <div class="col-md-3">
                                                        <label for="txtDate2" class="font-weight-bold" style="font-size: 20px;">Start Periode</label>
                                                        <div class="input-group">
                                                            <input type="text"
                                                                   id="txtDate2"
                                                                   class="form-control shadow-sm datepicker1 start"
                                                                   runat="server"
                                                                   placeholder="Select Month & Year"
                                                                   style="height: 40px;" />
                                                            <div class="input-group-append">
                                                                <span class="input-group-text" style="height: 40px;"><i class="fa fa-calendar"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>

                                                   <div class="col-md-3">
                                                        <label for="txtDate3" class="font-weight-bold" style="font-size: 20px;">End Periode</label>
                                                        <div class="input-group">
                                                            <input type="text"
                                                                   id="txtDate3"
                                                                   class="form-control shadow-sm datepicker1 end"
                                                                   runat="server"
                                                                   placeholder="Select Month & Year"
                                                                   style="height: 40px;" />
                                                            <div class="input-group-append">
                                                                <span class="input-group-text" style="height: 40px;"><i class="fa fa-calendar"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <!-- Branch -->
                                                    <div class="col-md-2">
                                                        <label for="ddlBranch" class="font-weight-bold" style="font-size: 20px;">Branch</label>
                                                        <div class="input-group">
                                                            <asp:DropDownList ID="ddlBranch"
                                                                              class="selectpicker form-control"
                                                                              data-show-subtext="true" 
                                                                              data-live-search="true"
                                                                              AppendDataBoundItems="true"
                                                                              runat="server"
                                                                              ValidateRequestMode="Enabled"
                                                                              OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                                              AutoPostBack="true"
                                                                              >
                                                              <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>   
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                     <div class="col-md-2">
                                                        <label for="ddlReport" class="font-weight-bold" style="font-size: 20px;">Report Type</label>
                                                        <div class="input-group">
                                                             <asp:DropDownList ID="ddlReport" class="selectpicker form-control" AppendDataBoundItems="true"
                                                                 runat="server" OnSelectedIndexChanged="ddReport_SelectedIndexChanged" data-show-subtext="true" 
                                                                 data-live-search="true">
                                                                 <asp:ListItem Text="Select Report" Value=""></asp:ListItem>
                                                                 <asp:ListItem Text="FIN SEG PL" Value="FIN_Seg_PL"></asp:ListItem>
                                                                 <asp:ListItem Text="PL" Value="PL"></asp:ListItem>
                                                                 <%--<asp:ListItem Text="BS" Value="BS"></asp:ListItem>--%>
                                                             </asp:DropDownList>
                                                        </div>
                                                    </div>
                                               </div>
                                             </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800 mt-4">
                                                        <!-- Tombol utama yang diklik user -->
                                                        <button type="button"
                                                                class="btn buttonColor align-middle"
                                                                style="height: 40px; line-height: 1.2;"
                                                                onclick="handleClientClick()">
                                                            <i class="fa fa-check" aria-hidden="true"></i>
                                                            Generate To Excel PL
                                                        </button>

                                                        <!-- Tombol ASP.NET tersembunyi yang akan diklik melalui JS -->
                                                        <asp:Button runat="server"
                                                                    Style="display: none;"
                                                                    ID="btnSubmit"
                                                                    OnClick="btnSubmit_Click" 
                                                                    CommandArgument="PL"/>
                                                </div>
                                              
                                            </div>
                                        </div> <!-- /.col-12 -->
                                    </div> <!-- /.row no-gutters -->              
                            </div> <!-- /.card-body -->
                           </div> <!-- /.card -->
                         <!-- Tambahan card kedua -->
                             <div class="col-xl-12 mt-4"> 
                                <div class="card border-left-primary shadow h-100 py-2">
                                    <div class="card-body"> <!-- card 2  -->
                                        <div class="row no-gutters align-items-center">
                                            <div class="col-12">
                                                <div class="form-group">
                                                    <div style="display: inline-block; background-color: #06183d; transform: skew(-10deg); padding: 5px 10px;">
                                                        <label for="txtDate4" 
                                                               class="font-weight-bold" 
                                                               style="font-size: 30px; font-style: italic; color: white; display: inline-block; transform: skew(10deg);">
                                                            Balance Sheet
                                                        </label>
                                                    </div>
                                                    <div class="row mt-4">
                                                        <!-- Start Periode -->
                                                         
                                                        <div class="col-md-3">
                                                            <label for="txtDate4" class="font-weight-bold" style="font-size: 20px;">Periode</label>
                                                            <div class="input-group">
                                                                <input type="text"
                                                                       id="txtDate4"
                                                                       class="form-control shadow-sm datepicker1 start"
                                                                       runat="server"
                                                                       placeholder="Select Month & Year"
                                                                       style="height: 40px;" />
                                                                <div class="input-group-append">
                                                                    <span class="input-group-text" style="height: 40px;"><i class="fa fa-calendar"></i></span>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <!-- Branch -->
                                                        <div class="col-md-3">
                                                            <label for="ddlBranch2" class="font-weight-bold" style="font-size: 20px;">Branch</label>
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="ddlBranch2"
                                                                                  class="selectpicker form-control"
                                                                                  data-show-subtext="true"
                                                                                  data-live-search="true"
                                                                                  OnSelectedIndexChanged="ddlBranch2_SelectedIndexChanged"
                                                                                  AppendDataBoundItems="true"
                                                                                  runat="server">
                                                                    <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div> <!-- /.row -->
                                                </div> <!-- /.form-group -->

                                                   <div class="h5 mb-0 font-weight-bold text-gray-800 mt-4">
                                                           <!-- Tombol utama yang diklik user -->
                                                           <button type="button"
                                                                   class="btn buttonColor align-middle"
                                                                   style="height: 40px; line-height: 1.2;"
                                                                   onclick="handleClientClickBS()">
                                                               <i class="fa fa-check" aria-hidden="true"></i>
                                                               Generate To Excel BS
                                                           </button>
                                                           <!-- Tombol ASP.NET tersembunyi yang akan diklik melalui JS -->
                                                           <asp:Button runat="server"
                                                                       Style="display: none;"
                                                                       ID="btnSubmit2"
                                                                       OnClick="btnSubmit_Click" 
                                                                       CommandArgument="BS"/>
                                                   </div>
                                            </div> <!-- /.col-12 -->
                                        </div> <!-- /.row no-gutters -->
                                    </div> <!-- /.card-body -->
                                </div> <!-- /.card -->
                            </div> <!-- /.col-xl-12 -->
                            <iframe id="downloadFrame" name="downloadFrame" style="display: none;"></iframe>
                                 <div id="loadingOverlay"
                                     style="display:none;position:fixed;top:0;left:0;width:100%;height:100%;background:rgba(0,0,0,0.6);z-index:9999;text-align:center;">
                                     
                   
                                     <div style="position: absolute; top: 40%; left: 50%; transform: translate(-50%, -50%);">
                                        <img src="img/loadingylid.gif" alt="Loading..." style="width: 80px;" />
                                        <p style="font-size: 18px; font-weight: bold; margin-top: 10px;">Processing, please wait...</p>
                                    </div>  
                                </div>


                        </div> <!-- /.col-xl-12 -->
                    </div> <!-- /.row -->
                </div> <!-- /.card-body -->
            </div> <!-- /.card -->
        </div>
    </div>
      
</asp:Content>


