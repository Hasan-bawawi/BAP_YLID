<%@ Page  Title="Master General Ledger"  MasterPageFile="~/Layout.Master"  Language ="C#" AutoEventWireup="true" CodeBehind="Master_GeneralLedgerBAP.aspx.cs" Inherits="BAP_System.Master_GeneralLedgerBAP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Untuk file CSS -->
<link href="Styles/sweetalert.css" rel="stylesheet" />
<!-- Untuk file JavaScript -->
<script src="Scripts/sweetalert.min.js"></script>
    <script type="text/javascript">

        function FuncSave() {
            swal({
                title: 'Save Success',
                text:  'Ledger Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'Master_GeneralLedgerBAP.aspx';
                }
            );
        }

        function Errorsave(message) {
         
            swal({
                title: 'Save Failed!',
                text: message,
                type: 'error',
                showConfirmButton: false,
                timer: 2000
            });


        }

        function Deletesave() {

            swal({
                title: 'Delete Succsess!',
                type: 'success',
                showConfirmButton: false,
                timer: 2000
            }, function redirect() {
                window.location.href = 'Master_GeneralLedgerBAP.aspx';
            }


            );


        }


        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Ledger Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'Master_GeneralLedgerBAP.aspx';
                }
            );
        }


        function confirmDelete(linkButton) {

            debugger

            var href = linkButton.getAttribute("href");
            var match = href.match(/__doPostBack\('([^']+)'/);
            if (match && match.length > 1) {
                var postBackTarget = match[1];
                console.log("PostBack Target:", postBackTarget);
            }
            swal({

                title: 'Are you sure?',
                text: "This data will be marked inactive.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'Cancel',
                confirmButtonColor: '#d33',
                showCloseButton: true,

            }, function (willDelete) {
                if (willDelete) {
                    __doPostBack(postBackTarget, '');
                }
            });

            return false;
        }

    </script>
   <!-- script baru  -->
  <script>
         document.addEventListener("DOMContentLoaded", function () {
         var  btnSubmit = document.getElementById('<%= btnSubmit.ClientID %>');
         btnSubmit.addEventListener('click', function (e) {
         let isValid = true;

          // Ambil semua input yang wajib diisi
          var requiredFields = document.querySelectorAll('.required-field');
          requiredFields.forEach(function (field) {
                if (!field.value.trim()) {
              field.classList.add('is-invalid');
          isValid = false;
                } else {
              field.classList.remove('is-invalid');
                }
          });



        // Validasi khusus untuk DropDownList ddlBussines
         var ddlTypeGL = document.getElementById('<%= ddlTypeGL.ClientID %>');
         if (ddlTypeGL.value === "0") {
                  ddlTypeGL.classList.add('is-invalid');
                isValid = false;
            } else {
                  ddlTypeGL.classList.remove('is-invalid');
            }

            if (!isValid) {
                e.preventDefault();
            }

         
        });
      });



      function validgl(btn) {
          var row = btn.closest("tr");

          var glname = row.querySelector("#txtGLName");
          var glaccount = row.querySelector("#txtGLAccount");
          var typegl = row.querySelector("#ddlTypeGL");

          var isValid = true;


          [glname, glaccount, typegl].forEach(function (input) {
              if (input) input.style.border = "";
          });

          if (typegl) {
              typegl.style.border = "";
              var btnVisual = row.querySelector(".bootstrap-select button");
              if (btnVisual) btnVisual.style.border = "";
          }

          if (!glname || glname.value.trim() === "") {
              glname.style.border = "2px solid red";
              isValid = false;
          }

          if (!glaccount || glaccount.value.trim() === "") {
              glaccount.style.border = "2px solid red";
              isValid = false;
          }


          if (!typegl || typegl.value === "") {
              typegl.style.border = "3px solid red";
              isValid = false;


              var btnVisual = row.querySelector(".bootstrap-select button");
              if (btnVisual) {
                  btnVisual.style.border = "3px solid red";
              }
          }

          return isValid;
      }

      function disableEditButtons() {

          var editButtons = document.querySelectorAll('.edit-btn');
          editButtons.forEach(function (btn) {
              btn.disabled = true;
              btn.classList.add('disabled');
              btn.style.pointerEvents = "none";
              btn.style.opacity = "0.5";
          });

          var deleteButtons = document.querySelectorAll('.delete-btn');
          deleteButtons.forEach(function (btn) {
              btn.disabled = true;
              btn.classList.add('disabled');
              btn.style.pointerEvents = "none";
              btn.style.opacity = "0.5";
          });

        
      }

  </script>

    <script>
        $(document).ready(function () {
            $('.selectpicker').selectpicker({
                dropupAuto: false
            }).on('shown.bs.select', function () {
                var menu = $(this).parent().find('.dropdown-menu');
                menu.css({
                    top: '100%',
                    bottom: 'auto',
                    transform: 'none'
                });
            });
        });
    </script>

   <style>


     /*
          style baru 
     */
     .is-invalid {
      border-color: red;
      }



     fieldset.scheduler-border {
         border: 1px groove #ff6d10 !important;
         padding: 0 1.4em 1.4em 1.4em !important;
         margin: 0 0 1.5em 0 !important;
         -webkit-box-shadow: 0px 0px 0px 0px #000;
         box-shadow: 0px 0px 0px 0px #000;
     }

     legend.scheduler-border {
         width: inherit; /* Or auto */
         padding: 0 10px; /* To give a bit of padding on the left and right */
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

         .custom-input {
             width: 100%;
             max-width: none;
             font-size: 14px;
             border-radius: 10px 10px;
             border-color:orangered;
             border-width:1px;
             padding:2px 6px;
             box-sizing: border-box;
             line-height: normal;
             height:28px;

         }

         .action-cell {
             display: flex;
             justify-content: center;   
             align-items: center;       
             height: 100%;
         }


                 .bootstrap-select .dropdown-toggle {
                background-color: #ff6d10; /* ungu (Tailwind indigo-600) */
                color: white;
                border: 1px solid #ff6d10;
                height: 45px;
                font-size: 0.85rem; 
                font-weight: 450;
                border-radius: 1rem;
                padding-right: 1.5rem !important;
                transition: background-color 0.3s ease, box-shadow 0.2s ease;
                width:458px;

              /* Koreksi vertikal dengan line-height */
              line-height: 45px;
              padding: 0 16px;
              text-align: left;

              /* Hapus padding-top/bottom default */
              padding-top: 0 !important;
              padding-bottom: 0 !important;

              /* Pastikan caret tetap muncul */
              display: inline-flex;
              align-items: center;
              justify-content: space-between;
     
        }

        .bootstrap-select .dropdown-toggle::after {
            border-top-color: white !important; /* Warna panah ke bawah */
        }

        .bootstrap-select.dropup .dropdown-menu {
            top: 100% !important;
            bottom: auto !important;
            transform: none !important;
        }

        /* Hover state */
        .bootstrap-select .dropdown-toggle:hover {
            background-color: #ff9754; /* lebih gelap saat hover */
            border-color: #ff9754;
            color: white;
            box-shadow: 0 0 0 0.2rem rgba(79, 70, 229, 0.25);
        }

      
        .bootstrap-select .dropdown-toggle:focus {
            box-shadow: 0 0 0 0.2rem rgba(79, 70, 229, 0.5);
        }
        .bootstrap-select .dropdown-menu {
            width: 458px !important;
            min-width: 458px !important;
            border-radius: 0.375rem;
            padding: 0;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            margin-top: 2px;
        }

      
        .bootstrap-select .dropdown-menu .dropdown-item {
            padding: 10px 16px;
            font-size: 1rem;
            color: #333;
        }

     
        .bootstrap-select .dropdown-menu .dropdown-item:hover {
            background-color: #f0f0f0;
            color: #000;
        }

        .bootstrap-select.table-dropdown,
        .bootstrap-select.table-dropdown .dropdown-toggle,
        .bootstrap-select.table-dropdown .dropdown-menu {
            width: 100% !important;
            min-width: 100% !important;
            box-sizing: border-box;
           
        }

        .table-dropdown .dropdown-menu {
            top: 100% !important;
            bottom: auto !important;
            transform: none !important;
            z-index: 1050; /* agar tidak ketutup elemen lain */
        }

        .table td {
            overflow: visible !important;
        }

 

 </style>
    <style>
        .page-head {
            background-color: #06183d; /* Change this to your desired background color */
            color: white; /* Text color for the header */
            padding: 1px 2.938rem; /* Optional padding for the header */
            height: 182px;
            position: relative;
            margin-top: 0px; /* Adjust top margin to move the header down */
            margin-left: 0px; /* Adjust left margin to move the header right */
        }

        .custom-card-body {
            position: relative;
            top: -99px; /* Adjust the top position to move the card body down */
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-head">
        <div id="page-title">
            <h1 class="page-header text-overflow" style="color: white;">General Ledger BAP</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="Home.aspx">Home</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Master<i class="fa fa-caret-right ml-2"></i></li>
            <li class="active">&nbsp;&nbsp;General Ledger</li>
        </ol>
    </div>

    <%--<asp:HiddenField ID="hlbID" runat="server" />
    <asp:HiddenField ID="lbNIK" runat="server" />--%>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of General Ledger BAP</legend>
                                    <div class="row">
                                      <div class='col-sm-12'>
                                           
                                         <asp:Button runat="server" ID="btNew" CssClass="btn  mb-1 buttonColor" Text="Add New" OnClick="btNew_Click" />

                                         </div>

                                        <div class='col-sm-12'>
                                            <!-- Table List of User Registration -->
                                         <div class="table-responsive">
                                                                              
                                            <asp:GridView ID="TableGLaccount" ClientIDMode="Static" runat="server" AutoGenerateColumns="False"
                                                              CssClass="table table-striped table-bordered zero-configuration text-nowrap"
                                                              EmptyDataText="No Record Found"
                                                              Style="width: 100%"
                                                              ShowHeaderWhenEmpty="true"
                                                              OnRowEditing="TableGLaccount_RowEditing"
                                                              OnRowCancelingEdit="TableGLaccount_RowCancelingEdit"
                                                              OnRowUpdating="TableGLaccount_RowUpdating"
                                                              OnRowDataBound="TableGLaccount_RowDataBound"
                                                              DataKeyNames="GL_id">

                                                    <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Action"  ItemStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <div Class="action-cell"> 
                                                                    <asp:LinkButton ID="btnEditGL"
                                                                                    runat="server"
                                                                                    CommandName="Edit"
                                                                                    CssClass="btn mb-1 buttonColor  edit-btn">
                                                                        <i class="fa fa-pencil"></i>
                                                                    </asp:LinkButton>
                                                                     <asp:LinkButton runat="server" ID="btnDeleteGL" 
                                                                     CommandName="Hapus" CommandArgument="<%# Container.DataItemIndex %>" 
                                                                     CssClass="btn mb-1 btn-danger ml-2 delete-btn" OnClick="btnDelete_Click"  OnClientClick="return confirmDelete(this);"  ToolTip="Remove"><i class="fa  fa-trash"></i> </asp:LinkButton>
                                                                </div>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="action-cell">
                                                                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn buttonColor"  OnClientClick="return validgl(this);" >
                                                                    <i class="fa fa-save"></i> 
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger ml-2">
                                                                    <i class="fa fa-times"></i> 
                                                                </asp:LinkButton>

                                                                </div>
                                                            </EditItemTemplate>

                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GL Name">
                                                            <ItemTemplate>
                                                                <%# Eval("GL_Name") %>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtGLName"
                                                                             runat="server"
                                                                             Text='<%# Bind("GL_Name") %>'
                                                                             CssClass="form-control form-control-sm custom-input"
                                                                             placeholder="Enter GL Name..." />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GL Account">
                                                            <ItemTemplate>
                                                                <%# Eval("GL_Accouunt") %>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtGLAccount"
                                                                             runat="server"
                                                                             Text='<%# Bind("GL_Accouunt") %>'
                                                                             CssClass="form-control form-control-sm custom-input"
                                                                             placeholder="Enter GL Account..." />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="GL Type" HeaderStyle-Width="200px" ItemStyle-Width="200px" >
                                                             <ItemTemplate>
                                                                 <%# Eval("Type_GL") %>
                                                             </ItemTemplate>
                                                             <EditItemTemplate>
                                                                 <div class="dropdown">
                                                                 <asp:DropDownList 
                                                                     ID="ddlTypeGL" 
                                                                     runat="server" 
                                                                     CssClass="selectpicker form-control table-dropdown"
                                                                     data-show-subtext="true" 
                                                                     data-live-search="true">
                                                                 <asp:ListItem Text="&lt;Select Type&gt;" Value=""></asp:ListItem>
                                                                 <asp:ListItem Text="BSH" Value="BSH"></asp:ListItem> 
                                                                 <asp:ListItem Text="P&L" Value="P&L"></asp:ListItem>
                                                                 </asp:DropDownList>
                                                                 </div>
                                                             </EditItemTemplate>
                                                         </asp:TemplateField>    
<%--                                                       <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" ReadOnly="True" DataFormatString="{0:dd-MMMM-yyyy hh:mm:ss tt}" />--%>
                                                    </Columns>
                                                </asp:GridView>
                                        </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>





 <!--**********************************
	Modal New User
***********************************-->
  <div class="modal fade bs-example-modal-lg" id="mdlAddGL" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
      <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content">

              <div class="modal-header">
                  <h4 class="modal-title" id="myModalLabel1">Add General Ledger</h4>
                  <button type="button" onclick="<%=btnCloseModalNew.ClientID %>.click()" class="close"><span>×</span></button>
                  <asp:Button runat="server" Style="display: none;" ID="btnCloseModalNew" OnClick="btnCloseModalNew_Click"></asp:Button>
              </div>
              <div class="modal-body">
                  <div class="x_content">
                      <div class="row">
                         <asp:HiddenField ID="GL_id" runat="server" />                        
                         <asp:HiddenField ID="txtTypeGL" runat="server" />
                         <asp:HiddenField ID="txtGLAccountbrf" runat="server" />
                         <%--<asp:HiddenField ID="hlbOid" runat="server" />--%>
            <div class='col-sm-12'>
                              GL Name 
			<div class="form-group" style="margin-top:5px;">
                      <div class='input-group'>
                          <input runat="server" id="txtGLName" name="NameBranch" data-validate-length-range="5,15" type="text" class="form-control input-rounded required-field" placeholder="Type GL Name" >
                      </div>
                </div>
           </div>
           <div class='col-sm-12'>
                              Gl Account
			<div class="form-group" style="margin-top:5px;">
                      <div class='input-group'>
                          <input runat="server" id="txtGLAccount" data-validate-length-range="5,15" type="text" class="form-control input-rounded required-field" placeholder="Type GL Account" >
                      </div>
                  </div>
              </div>
           <div class='col-sm-12'>
                                GL Type
                    <div class="form-group">
                        <div class='input-group'>
                            <asp:DropDownList ID="ddlTypeGL"
                                CssClass="selectpicker form-control full-width-dropdown  mt-2"
                                data-show-subtext="true" 
                                data-live-search="true"
                                AppendDataBoundItems="true"
                                runat="server"
                                >
                               <asp:ListItem Text="&lt;Select Type GL&gt;" Value="0"></asp:ListItem>   
                               <asp:ListItem Text="BSH" Value="BSH"></asp:ListItem> 
                               <asp:ListItem Text="P&L" Value="P&L"></asp:ListItem>
                            </asp:DropDownList>
                            
                        </div>
                    </div>
           </div>          
            </div>
                  </div>
              </div>
              <div class="modal-footer">
                  <asp:Button runat="server" ID="btnSubmit" CssClass="btn buttonColor" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
              </div>
          </div>
      </div>
  </div>

</asp:content>
