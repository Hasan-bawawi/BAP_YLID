
<%@ Page  Title="Master DepartemenBAP"  MasterPageFile="~/Layout.Master"  Language ="C#" AutoEventWireup="true" CodeBehind="Master_DepartementBAP.aspx.cs" Inherits="BAP_System.Master_DepartementBAP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Untuk file CSS -->
<link href="Styles/sweetalert.css" rel="stylesheet" />
<!-- Untuk file JavaScript -->
<script src="Scripts/sweetalert.min.js"></script>
    <script type="text/javascript">

        function FuncSave() {
            swal({
                title: 'Save Success',
                text:  'Departement Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'Master_DepartementBAP.aspx';
                }
            );
        }

        function Errorsave() {
         
            swal({
                title: 'Save Failed!',
                text: 'Something went wrong while saving.',
                type: 'error',
                showConfirmButton: false,
                timer: 2000
            }).then(() => {
                location.reload(); 
            });


        }

        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Departement Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'Master_DepartementBAP.aspx';
                }
            );
        }

    </script>
   <!-- script baru  -->
  <script>
      document.addEventListener("DOMContentLoaded", function () {
          var btnSubmit = document.getElementById('<%= btnSubmit.ClientID %>');
      btnSubmit.addEventListener('click', function (e) {
        let isValid = true;

        // Validasi input yang wajib diisi
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
        var ddlBussines = document.getElementById('<%= ddlBussines.ClientID %>');
        if (ddlBussines.value === "0") {
            ddlBussines.classList.add('is-invalid');
            isValid = false;
        } else {
            ddlBussines.classList.remove('is-invalid');
        }

        if (!isValid) {
            e.preventDefault();
        }
    });
});


      function validateRow(btn) {

          var row = btn.closest("tr");

          var NameDept = row.querySelector("#txtNamaDept");
          var CodeDept = row.querySelector("#txtCodeDept");
          var dropdown = row.querySelector("#ddlBussines");

          var isValid = true;


          [NameDept, CodeDept, dropdown].forEach(function (input) {
              if (input) {
                  input.style.border = "";
                  //var errorSpan = input.parentElement.querySelector(".error-text");
                  //if (errorSpan) errorSpan.remove();
              }
          });


          if (dropdown) {
              dropdown.style.border = "";
              var btnVisual = row.querySelector(".bootstrap-select button");
              if (btnVisual) btnVisual.style.border = "";
          }

          if (!NameDept || NameDept.value.trim() === "") {
              NameDept.style.border = "2px solid red";
              isValid = false;
          }

          if (!CodeDept || CodeDept.value.trim() === "") {
              CodeDept.style.border = "2px solid red";
              isValid = false;
          }

        
          if (!dropdown || dropdown.value === "0") {
              dropdown.style.border = "3px solid red"; 
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

        /*.error-text {
            color: red;
            font-size: 11px;
            margin-top: 4px;
            position: absolute;
            display:block;*/

            /* Sesuaikan jaraknya di sini */
        /*}*/

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
            <h1 class="page-header text-overflow" style="color: white;">Departement BAP</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="Home.aspx">Home</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Master<i class="fa fa-caret-right ml-2"></i></li>
            <li class="active">&nbsp;&nbsp;Departement</li>
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
                                    <legend class="scheduler-border">List of Departement BAP</legend>
                                    <div class="row">
                                      <div class='col-sm-12'>
                                           <asp:Button runat="server" ID="btNew" CssClass="btn  mb-1 buttonColor" Text="Add New" OnClick="btNew_Click" />
                                    
                                         </div>

                                        <div class='col-sm-12'>
                                            <!-- Table List of User Registration -->
                                            <div class="table-responsive">
                                                <asp:GridView ID="TableDepartement" ClientIDMode="Static" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-striped table-bordered zero-configuration grid"
                                                EmptyDataText="No Record Found"
                                                Style="width: 100%"
                                                ShowHeaderWhenEmpty="true"
                                                OnRowEditing="TableDepartement_RowEditing"
                                                OnRowCancelingEdit="TableDepartement_RowCancelingEdit"
                                                OnRowUpdating="TableDepartement_RowUpdating"
                                                OnRowDataBound="TableDepartement_RowDataBound"
                                                DataKeyNames="Department_id,BussinesMode_id">

                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                         <%--   <asp:CommandField ShowEditButton="True"/>--%>

                                                            <asp:TemplateField HeaderText="Action" >
                                                            <ItemTemplate>
                                                                <div Class="action-cell">
                                                                     <asp:LinkButton 
                                                                         ID="btnEdit" 
                                                                         runat="server" 
                                                                         CommandName="Edit" 
                                                                         CssClass="btn mb-1 buttonColor edit-btn">
                                                                         <i class="fa fa-pencil"></i> 
                                                                     </asp:LinkButton>
                                                                </div>
                                                               
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="action-cell">

                                                                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn buttonColor"  OnClientClick="return validateRow(this);" Style="margin-right:5px" >
                                                                    <i class="fa fa-save"></i> 
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger">
                                                                    <i class="fa fa-times"></i> 
                                                                </asp:LinkButton>

                                                                </div>

                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                           <%-- <asp:BoundField DataField="AccountCode" HeaderText="Account Code" ReadOnly="False" />
                                                            <asp:BoundField DataField="AccountTitle" HeaderText="Account Title" ReadOnly="False" />--%>
                                                        <asp:TemplateField HeaderText="Departement Name">
                                                          <ItemTemplate>
                                                              <%# Eval("NamaDept") %>
                                                          </ItemTemplate>
                                                          <EditItemTemplate>
                                                              <asp:TextBox 
                                                                  ID="txtNamaDept" 
                                                                  runat="server" 
                                                                  Text='<%# Bind("NamaDept") %>' 
                                                                  CssClass="form-control form-control-sm custom-input" 
                                                                  placeholder="Enter Departement Name..." />
                                                          </EditItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Departement Code">
                                                          <ItemTemplate>
                                                              <%# Eval("CodeDept") %>
                                                          </ItemTemplate>
                                                          <EditItemTemplate>
                                                              <asp:TextBox 
                                                                  ID="txtCodeDept" 
                                                                  runat="server" 
                                                                  Text='<%# Bind("CodeDept") %>' 
                                                                  CssClass="form-control form-control-sm custom-input" 
                                                                  placeholder="Enter Code..." />
                                                          </EditItemTemplate>
                                                      </asp:TemplateField>  
                                                        <asp:TemplateField HeaderText="Bussines Name">
                                                            <ItemTemplate>
                                                                <%# Eval("BussinesName") %>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="dropdown">
                                                                <asp:DropDownList 
                                                                    ID="ddlBussines" 
                                                                    runat="server" 
                                                                    CssClass="selectpicker form-control table-dropdown"
                                                                    data-show-subtext="true" 
                                                                    data-live-search="true">
                                                                <asp:ListItem Text="&lt;Select Bussines&gt;" Value="0"></asp:ListItem>   
                                                                </asp:DropDownList>
                                                                </div>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>                                                          
<%--                                                         <asp:TemplateField HeaderText="Have Sub">
                                                             <ItemTemplate>
                                                                 <asp:CheckBox ID="havesub" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsHavesub")) %>' Enabled="false" />
                                                             </ItemTemplate>
                                                             <EditItemTemplate>
                                                                 <asp:CheckBox ID="havesubEdit" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsHavesub")) %>' />
                                                             </EditItemTemplate>
                                                         </asp:TemplateField>--%>
                                                            <asp:BoundField DataField="CreatedDate" HeaderText="Create Date" ReadOnly="True" DataFormatString="{0:dd-MMMM-yyyy hh:mm:ss tt}"/>
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
  <div class="modal fade bs-example-modal-lg" id="mdlAddNewDept" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
      <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content">

              <div class="modal-header">
                  <h4 class="modal-title" id="myModalLabel1">Add Departement</h4>
                  <button type="button" onclick="<%=btnCloseModalNew.ClientID %>.click()" class="close"><span>×</span></button>
                  <asp:Button runat="server" Style="display: none;" ID="btnCloseModalNew" OnClick="btnCloseModalNew_Click"></asp:Button>
              </div>
              <div class="modal-body">
                  <div class="x_content">
                      <div class="row">
                          <asp:HiddenField ID="Departement_id" runat="server" />
                          <asp:HiddenField ID="Busid" runat="server" />
                         <%--<asp:HiddenField ID="hlbOid" runat="server" />--%>
          <div class='col-sm-12'>
                                Bussines Name
                    <div class="form-group">
                        <div class='input-group'>
                            <asp:DropDownList ID="ddlBussines"
                                CssClass="selectpicker form-control full-width-dropdown  mt-2"
                                data-show-subtext="true" 
                                data-live-search="true"
                                AppendDataBoundItems="true"
                                runat="server"
                                OnSelectedIndexChanged="ddlBussines_SelectedIndexChanged">
                               <asp:ListItem Text="&lt;Select Bussines&gt;" Value="0"></asp:ListItem>   
                            </asp:DropDownList>
                            
                        </div>
                    </div>
           </div>
            <div class='col-sm-12'>
                              Departement Name 
			<div class="form-group" style="margin-top:5px;">
                      <div class='input-group'>
                          <input runat="server" id="txtNamaDeptadd" name="NamaDept" data-validate-length-range="5,15" type="text" class="form-control input-rounded required-field" placeholder="Type Branch Name" >
                      </div>
                </div>
           </div>
           <div class='col-sm-12'>
                              Departement Code
			<div class="form-group" style="margin-top:5px;">
                      <div class='input-group'>
                          <input runat="server" id="txtCodeDeptadd" data-validate-length-range="5,15" type="text" class="form-control input-rounded required-field" placeholder="Type Branch Code" >
                      </div>
                  </div>
            </div> 
            
<%--          <div class='col-sm-12'>
                <div class="d-flex align-items-center mb-3" style="gap: 3.5rem;"> 
                    <div class="d-flex align-items-center">
                        <label for="havesub" class="mb-0 me-2" style="margin-right:10px">Have Sub</label>
                        <div class="input-group-text">
                        <asp:CheckBox runat="server" ID="havesub" />
                        </div>
                    </div>
                </div>
            </div> --%>      
            </div>
                  </div>
              </div>
              <div class="modal-footer">
                  <asp:Button runat="server" ID="btnSubmit" CssClass="btn buttonColor" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
              </div>
          </div>
      </div>
  </div>

    
<%-- jquery input select2   --%>

</asp:content>

