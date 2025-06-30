
<%@ Page  Title="Module Registration BAP"  MasterPageFile="~/Layout.Master"  Language ="C#" AutoEventWireup="true" CodeBehind="ModuleBAP.aspx.cs" Inherits="BAP_System.ModuleBAP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Untuk file CSS -->
<link href="Styles/sweetalert.css" rel="stylesheet" />
<!-- Untuk file JavaScript -->
<script src="Scripts/sweetalert.min.js"></script>
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">

        function FuncSave() {
            swal({
                title: 'Save Success',
                text:  'Module Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'ModuleBAP.aspx';
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
                text: 'Module Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'ModuleBAP.aspx';
                }
            );
        }

    </script>
   <!-- script baru  -->
  <script>


      //function setupCheckboxDisable() {
      //    debugger
      //    $('.edit-checkbox').off('change').on('change', function () {
      //        if ($(this).is(':checked')) {
      //            $('.edit-checkbox').not(this).prop('disabled', true);
      //        } else {
      //            $('.edit-checkbox').prop('disabled', false);
      //        }
      //    });
      //}


      function disableEditButtons() {
          var editButtons = document.querySelectorAll('.edit-btn');
          editButtons.forEach(function (btn) {
              btn.disabled = true;
              btn.classList.add('disabled');
              btn.style.pointerEvents = "none";
              btn.style.opacity = "0.5";
          });

          // Disable all delete buttons
          var deleteButtons = document.querySelectorAll('.delete-btn');
          deleteButtons.forEach(function (btn) {
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
          var deleteButtons = document.querySelectorAll('.delete-btn');
          deleteButtons.forEach(function (btn) {
              btn.disabled = false;
              btn.classList.remove('disabled');
              btn.style.pointerEvents = "";
              btn.style.opacity = "";
          });
      }




      function validateRow(btn) {

          var row = btn.closest("tr");

          var module = row.querySelector("#txtmoduleName");
          var aspxid = row.querySelector("#txtModuleASPXid");
          //var dropdown = row.querySelector("#ddlParent");
          //var type = row.querySelector('#txtType');
          var isValid = true;


          [module, aspxid].forEach(function (input) {
              if (input) input.style.border = "";
          });

          //if (dropdown) {
          //    dropdown.style.border = "";
          //    var btnVisual = row.querySelector(".bootstrap-select button");
          //    if (btnVisual) btnVisual.style.border = "";
          //}

          if (!module || module.value.trim() === "") {
              module.style.border = "2px solid red";
              isValid = false;
          }

          if (!aspxid || aspxid.value.trim() === "") {
              aspxid.style.border = "2px solid red";
              isValid = false;
          }

          //if(!dropdown || dropdown.value === "") {
          //    dropdown.style.border = "3px solid red";
          //    isValid = false;


          //    var btnVisual = row.querySelector(".bootstrap-select button");
          //    if (btnVisual) {
          //        btnVisual.style.border = "3px solid red";
          //    }
          //}
          

          return isValid;
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
          .center-header {
              text-align: center !important;
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
            <h1 class="page-header text-overflow" style="color: white;">Module Registration</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="Home.aspx">Home</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;User Management<i class="fa fa-caret-right ml-2"></i></li>
            <li class="active">&nbsp;&nbsp;Module Registration</li>
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
                                    <legend class="scheduler-border">List of Module BAP</legend>
                                    <div class="row">
                                       
                                      <div class='col-sm-12'>
<%--                                            <button type="button" onclick="<%=btNew.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                <i class="fa fa-plus"></i>
                                                  Add New
                                            </button>--%>
<%--                                            <asp:Button runat="server" Style="display: none;" ID="btNew" OnClick="btNew_Click"></asp:Button>--%>
                                            <asp:Button runat="server" ID="btNew" CssClass="btn buttonColor btn-icon" Text=" Add New" OnClick="btNew_Click" />

                                         </div>

                                        <div class='col-sm-12'>
                                            <!-- Table List of User Registration -->
                                         <div class="table-responsive">
                                         <asp:HiddenField ID="txtmoduleName" runat="server" />
                                         <asp:HiddenField ID="txtModuleASPXid" runat="server" />
                                         <asp:HiddenField ID="menu_id" runat="server" />
                                         <asp:HiddenField ID="Parent_id" runat="server" />
                                         <asp:GridView ID="TableModule" ClientIDMode="Static" runat="server" AutoGenerateColumns="False"
                                          CssClass="table table-striped table-bordered zero-configuration grid"
                                          EmptyDataText="No Record Found"
                                          Style="width: 100%"
                                          ShowHeaderWhenEmpty="true"
                                          OnRowEditing="TableModule_RowEditing"
                                          OnRowCancelingEdit="TableModule_RowCancelingEdit"
                                          OnRowUpdating="TableModule_RowUpdating"
                                          OnRowDataBound="TableModule_RowDataBound"
                                          DataKeyNames="menu_id,Parent_id">
                  
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
                                                       <asp:TemplateField HeaderText="Menu Name" ItemStyle-Width="100px" HeaderStyle-Width="200px">
                                                          <ItemTemplate>
                                                              <%# Eval("moduleName") %>
                                                          </ItemTemplate>
                                                          <EditItemTemplate>
                                                              <asp:TextBox 
                                                                  ID="txtmoduleName" 
                                                                  runat="server" 
                                                                  Text='<%# Bind("moduleName") %>' 
                                                                  CssClass="form-control form-control-sm custom-input" 
                                                                  placeholder="Enter Name Menu..." />
                                                          </EditItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Parent Name" ItemStyle-Width="100px" HeaderStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <%# Eval("ParentName") %>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="dropdown">
                                                                <asp:DropDownList 
                                                                    ID="ddlParent" 
                                                                    runat="server" 
                                                                    CssClass="selectpicker form-control table-dropdown"
                                                                    data-show-subtext="true" 
                                                                    data-live-search="true">
                                                                <asp:ListItem Text="&lt;Select Parent Menu&gt;" Value="0"></asp:ListItem>   
                                                                </asp:DropDownList>
                                                                </div>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>  
                                                      <asp:TemplateField HeaderText="Menu ASPXID" ItemStyle-Width="100px" HeaderStyle-Width="200px">
                                                          <ItemTemplate>
                                                              <%# Eval("ModuleASPXid") %>
                                                          </ItemTemplate>
                                                          <EditItemTemplate>
                                                              <asp:TextBox 
                                                                  ID="txtModuleASPXid" 
                                                                  runat="server" 
                                                                  Text='<%# Bind("ModuleASPXid") %>' 
                                                                  CssClass="form-control form-control-sm custom-input" 
                                                                  placeholder="Enter aspxid.." />
                                                          </EditItemTemplate>
                                                      </asp:TemplateField>  
                                                        <asp:TemplateField HeaderText="Sub Menu 1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="center-header" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div class="action-cell">
                                                                        <asp:CheckBox ID="Submenu1" runat="server" Checked='<%# Convert.ToBoolean(Eval("Submenu1")) %>' Enabled="false" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="action-cell">
                                                                        <asp:CheckBox ID="Submenu1" CssClass="edit-checkbox" runat="server" Checked='<%# Convert.ToBoolean(Eval("Submenu1")) %>' />
                                                                    </div>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sub Menu 2" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle CssClass="center-header" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div class="action-cell">
                                                                    <asp:CheckBox ID="Submenu2" runat="server" Checked='<%# Convert.ToBoolean(Eval("Submenu2")) %>' Enabled="false" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <EditItemTemplate >
                                                                    <div class="action-cell">
                                                                    <asp:CheckBox ID="Submenu2" CssClass="edit-checkbox" runat="server" Checked='<%# Convert.ToBoolean(Eval("Submenu2")) %>' />
                                                                    </div>
                                                                </EditItemTemplate >
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Sub Menu 3" HeaderStyle-HorizontalAlign="Center">
                                                                 <HeaderStyle CssClass="center-header" HorizontalAlign="Center" />
                                                                <ItemTemplate >
                                                                    <div class="action-cell">
                                                                    <asp:CheckBox ID="Submenu3" runat="server" Checked='<%# Convert.ToBoolean(Eval("Submenu3")) %>' Enabled="false" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <EditItemTemplate >
                                                                    <div class="action-cell">
                                                                    <asp:CheckBox ID="Submenu3" CssClass="edit-checkbox" runat="server" Checked='<%# Convert.ToBoolean(Eval("Submenu3")) %>' />
                                                                    </div>
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

 


   <script type="text/javascript">
       function bindCheckboxBehavior() {
          
         /*  console.log("bindCheckboxBehavior initialized");*/

           $(".edit-checkbox input[type=checkbox]").off("change").on("change", function () {
               var checkbox = $(this);
               //console.log("Checkbox changed:", checkbox.attr("id")); // Sekarang harus muncul

               var row = checkbox.closest("tr");
               var allCheckboxes = row.find("input[type=checkbox]");

               if (checkbox.is(":checked")) {
                   allCheckboxes.each(function () {
                       if (this !== checkbox[0]) {
                           $(this).prop("checked", false);
                           $(this).prop("disabled", true);
                       }
                   });
               } else {
                   var anyChecked = allCheckboxes.filter(":checked").length > 0;
                   if (!anyChecked) {
                       allCheckboxes.prop("disabled", false);
                   }
               }
           });
       }

       Sys.Application.add_load(function () {
           bindCheckboxBehavior();
       });
   </script>


</asp:content>



