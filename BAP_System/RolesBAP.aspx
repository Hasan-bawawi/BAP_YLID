
<%@ Page  Title="Role Registration BAP"  MasterPageFile="~/Layout.Master"  Language ="C#" AutoEventWireup="true" CodeBehind="RolesBAP.aspx.cs" Inherits="BAP_System.RolesBAP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Untuk file CSS -->
<link href="Styles/sweetalert.css" rel="stylesheet" />
<!-- Untuk file JavaScript -->
<script src="Scripts/sweetalert.min.js"></script>
    <script type="text/javascript">

        function FuncSave() {
            swal({
                title: 'Save Success',
                text:  'Role Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'RolesBAP.aspx';
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

        function FuncSaveRMA(message) {
            swal({
                title: 'Success',
                text: message,
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            });
        }


        function ErrorsaveRMA(message) {

            swal({
                title: 'Failed!',
                text: message,
                type: 'error',
                showConfirmButton: false,
                timer: 3000
            });


            //    .then(() => {
            //    location.reload();

            //});
        }


        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Role Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'RolesBAP.aspx';
                }
            );
        }

    </script>
   <!-- script baru  -->
  <script>


      function disableEditButtons() {

          var editButtons = document.querySelectorAll('.edit-btn');
          editButtons.forEach(function (btn) {
              btn.disabled = true;
              btn.classList.add('disabled');
              btn.style.pointerEvents = "none";
              btn.style.opacity = "0.5";
          });

      }

      function enableEditButtons() {

          var buttons = document.querySelectorAll('.edit-btn');
          buttons.forEach(function (btn) {
              btn.disabled = false;
              btn.classList.remove('disabled');
              btn.style.pointerEvents = "";
              btn.style.opacity = "";
          });
         
      }


      function validateRow(btn) {

          var row = btn.closest("tr");

          
          var Rolename = row.querySelector("#txtRoleName"); 


          var isValid = true;



          [Rolename].forEach(function (input) {
              if (input) {
                  input.style.border = "";

              }
          });

          if (!Rolename || Rolename.value.trim() === "") {
              Rolename.style.border = "2px solid red";
              isValid = false;
          }

          return isValid;
      }


      function validateRMA(btn) {

          debugger
          var row = btn.closest("tr");


          var dropdown = row.querySelector("#ddlModuleName");


          var isValid = true;


          if (dropdown) {
              dropdown.style.border = "";
              var btnVisual = row.querySelector(".bootstrap-select button");
              if (btnVisual) btnVisual.style.border = "";
          }


          [dropdown].forEach(function (input) {
              if (input) {
                  input.style.border = "";

              }
          });

      

          if (!dropdown || dropdown.value === "") {
              dropdown.style.border = "3px solid red";
              isValid = false;


              var btnVisual = row.querySelector(".bootstrap-select button");
              if (btnVisual) {
                  btnVisual.style.border = "3px solid red";
              }
          }



          return isValid;
      }






  </script>
   <style>
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

         .buttonColorx {
          background-color: #235acc;
          color: white;
            }

          .buttonColorx:hover {
              background-color: #1a49ab;
              color: white;
          }
          .center-header {
            text-align: center !important;
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
            <h1 class="page-header text-overflow" style="color: white;">Role Registration</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="Home.aspx">Home</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;User Management<i class="fa fa-caret-right ml-2"></i></li>
            <li class="active">&nbsp;&nbsp;Role Registtration</li>
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
                                    <legend class="scheduler-border">List of Role BAP</legend>
                                    <div class="row">
                                       
                                      <div class='col-sm-12'>
                                    <%-- <button type="button" onclick="<%=btNew.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                <i class="fa fa-plus"></i>
                                                  Add New
                                            </button>
                                            <asp:Button runat="server" Style="display: none;" ID="btNew" OnClick="btNew_Click"></asp:Button>--%> 
                                            <asp:Button runat="server" ID="btNew" CssClass="btn buttonColor btn-icon" Text=" Add New" OnClick="btNew_Click" />


                                         </div>

                                        <div class='col-sm-12'>
                                            <!-- Table List of User Registration -->
                                         <div class="table-responsive">
                                         <asp:HiddenField ID="txtRoleName" runat="server" />                                       
                                         <asp:HiddenField ID="Role_id" runat="server" />
                                         <asp:GridView ID="TableRole" ClientIDMode="Static" runat="server" AutoGenerateColumns="False"
                                          CssClass="table table-striped table-bordered zero-configuration grid"
                                          EmptyDataText="No Record Found"
                                          Style="width: 100%"
                                          ShowHeaderWhenEmpty="true"
                                          OnRowEditing="TableRole_RowEditing"
                                          OnRowCancelingEdit="TableRole_RowCancelingEdit"
                                          OnRowUpdating="TableRole_RowUpdating"
                                          OnRowDataBound="TableRole_RowDataBound"
                                          DataKeyNames="Role_id">
                  
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
                                                                      <asp:LinkButton 
                                                                        ID="btnView" 
                                                                        runat="server" 
                                                                        CommandName="View" 
                                                                        CssClass="btn mb-1 buttonColorx ml-2 edit-btn"
                                                                        OnClick="btnview_Click">
                                                                        <i class="fa fa-eye"></i> 
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
                                                        <asp:TemplateField HeaderText="Role Name">
                                                          <ItemTemplate>
                                                              <asp:Label ID="lblRoleNmae" runat="server" Text='<%# Eval("RoleName") %>' />                                                            
                                                          </ItemTemplate>
                                                          <EditItemTemplate>
                                                              <asp:TextBox 
                                                                  ID="txtRoleName" 
                                                                  runat="server" 
                                                                  Text='<%# Bind("RoleName") %>' 
                                                                  CssClass="form-control form-control-sm custom-input" 
                                                                  placeholder="Enter Name Role..." />
                                                          </EditItemTemplate>
                                                      </asp:TemplateField>
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
        Modal View
    ***********************************-->
<div class="modal fade bs-example-modal-lg" id="mdlViews" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false"  >
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" onclick="<%=btnCloseModalView.ClientID %>.click()" class="close"><span>×</span></button>
                <asp:Button runat="server" Style="display: none;" ID="btnCloseModalView" OnClick="btnCloseModalView_Click"></asp:Button>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class='col-sm-12'>
                        <fieldset class="scheduler-border">
                            <legend class="scheduler-border">Role Menu Action</legend>
                            <div class="control-group">
                                <div class="row">
                                    <div class='col-sm-12'>
                                        <%--<button type="button" onclick="<%=btNewRL.ClientID %>.click()" class="btn mb-1 buttonColor">
                                            <i class="fa fa-plus"></i>
                                            Add New
                                        </button>
                                        <asp:Button runat="server" Style="display: none;" ID="btNewRL" OnClick="btNewRL_Click"></asp:Button>--%>
                                        <asp:Button runat="server" ID="btNewRL" CssClass="btn  mb-1 buttonColor btn-icon" Text="Add New Role Action" OnClick="btNewRL_Click" />
                                    </div>
                                    <h2 ID ="txtheader" runat="server" style=" font-weight:bold; background-color:coral;" class=" mt-3 ml-3"></h2>
                                    <div class='col-sm-12'>
                                        <div class="table-responsive">
                                            <asp:HiddenField ID="Access_id" runat="server" />
                                            <asp:HiddenField ID="Menu_id" runat="server" />
                                            <asp:GridView ID="TableRMA" ClientIDMode="Static" runat="server" AutoGenerateColumns="False"
                                                              CssClass="table table-striped table-bordered zero-configuration text-nowrap"
                                                              EmptyDataText="No Record Found"
                                                              Style="width: 100%"
                                                              ShowHeaderWhenEmpty="true"
                                                              OnRowEditing="TableRMA_RowEditing"
                                                              OnRowCancelingEdit="TableRMA_RowCancelingEdit"
                                                              OnRowUpdating="TableRMA_RowUpdating"
                                                              OnRowDataBound="TableRMA_RowDataBound"
                                                              DataKeyNames="Access_id,Menu_id">

                                                    <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <div Class="action-cell">
                                                                    <asp:LinkButton runat="server" ID="btnDeleteGL" 
                                                                        CommandName="Hapus" CommandArgument="<%# Container.DataItemIndex %>" 
                                                                        CssClass="btn mb-1 btn-danger edit-btn" OnClick="btnDelete_Click" ToolTip="Remove"><i class="fa  fa-trash"></i> </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnEditGL"
                                                                                    runat="server"
                                                                                    CommandName="Edit"
                                                                                    CssClass="btn mb-1 buttonColor  ml-2 edit-btn">
                                                                        <i class="fa fa-pencil"></i>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="action-cell">
                                                                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn buttonColor"  OnClientClick="return validateRMA(this);" >
                                                                    <i class="fa fa-save"></i> 
                                                                </asp:LinkButton>
                                                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger ml-2">
                                                                    <i class="fa fa-times"></i> 
                                                                </asp:LinkButton>
                                                                </div>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>                                                
                                                         <asp:TemplateField HeaderText="Module Name" ItemStyle-Width="200px" HeaderStyle-Width="300px">
                                                            <ItemTemplate>
                                                                <%# Eval("ModuleName") %>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="dropdown">
                                                                <asp:DropDownList 
                                                                    ID="ddlModuleName" 
                                                                    runat="server" 
                                                                    CssClass="selectpicker form-control table-dropdown"
                                                                    data-show-subtext="true" 
                                                                    data-live-search="true"
                                                                   >
                                                                <asp:ListItem Text="&lt;Select Module&gt;" Value="0"></asp:ListItem>   
                                                                </asp:DropDownList>
                                                                </div>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--  <asp:TemplateField HeaderText="CanCreate" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <div class="action-cell">
                                                                    <asp:CheckBox ID="CanCreate" runat="server" Checked='<%# Convert.ToBoolean(Eval("CanCreate")) %>' Enabled="false" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <EditItemTemplate >
                                                                    <div class="action-cell">
                                                                    <asp:CheckBox ID="CanCreate" runat="server" Checked='<%# Convert.ToBoolean(Eval("CanCreate")) %>' />
                                                                    </div>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>--%>

                                                        <asp:TemplateField HeaderText="CanCreate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="center-header" HorizontalAlign="Center" />
                                                                <%--<ItemStyle HorizontalAlign="Center" />--%>
                                                                <ItemTemplate>
                                                                    <div class="action-cell">
                                                                        <asp:CheckBox ID="CanCreate" runat="server" Checked='<%# Convert.ToBoolean(Eval("CanCreate")) %>' Enabled="false" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="action-cell">
                                                                        <asp:CheckBox ID="CanCreate" runat="server" Checked='<%# Convert.ToBoolean(Eval("CanCreate")) %>' />
                                                                    </div>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="CanEdit" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="center-header" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div class="action-cell">
                                                                    <asp:CheckBox ID="CanEdit" runat="server" Checked='<%# Convert.ToBoolean(Eval("CanEdit")) %>' Enabled="false" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <EditItemTemplate >
                                                                    <div class="action-cell">
                                                                    <asp:CheckBox ID="CanEdit" runat="server" Checked='<%# Convert.ToBoolean(Eval("CanEdit")) %>' />
                                                                    </div>
                                                                </EditItemTemplate >
                                                            </asp:TemplateField>

                                                             <asp:TemplateField HeaderText="CanView" HeaderStyle-HorizontalAlign="Center">
                                                                 <HeaderStyle CssClass="center-header" HorizontalAlign="Center" />
                                                                <ItemTemplate >
                                                                    <div class="action-cell">
                                                                    <asp:CheckBox ID="CanView" runat="server" Checked='<%# Convert.ToBoolean(Eval("CanView")) %>' Enabled="false" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <EditItemTemplate >
                                                                    <div class="action-cell">
                                                                    <asp:CheckBox ID="CanView" runat="server" Checked='<%# Convert.ToBoolean(Eval("CanView")) %>' />
                                                                    </div>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>                                                       
                                                       <asp:BoundField DataField="CretedDateRMA" HeaderText="Created Date" ReadOnly="True" DataFormatString="{0:dd-MMMM-yyyy hh:mm:ss tt}" />
                                                    </Columns>
                                                </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
          <div class="modal-footer">
           </div>
        </div>
    </div>
</div>




</asp:content>
