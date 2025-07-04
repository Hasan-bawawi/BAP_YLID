<%@ Page  Title="Master BranchBAP"  MasterPageFile="~/Layout.Master"  Language ="C#" AutoEventWireup="true" CodeBehind="Master_BranchBAP.aspx.cs" Inherits="BAP_System.Master_BranchBAP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Untuk file CSS -->
<link href="Styles/sweetalert.css" rel="stylesheet" />
<!-- Untuk file JavaScript -->
<script src="Scripts/sweetalert.min.js"></script>
    <script type="text/javascript">

        function FuncSave() {
            swal({
                title: 'Save Success',
                text:  'Branch Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'Master_BranchBAP.aspx';
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

        function FuncRemove() {
            swal({
                title: 'Remove Success',
                text: 'Successfully Removed',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            }
            );
        }


        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Branch Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'Master_BranchBAP.aspx';
                }
            );
        }

        function confirmDelete(linkButton) {


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

          if (!isValid) {
              e.preventDefault();
            }
        });
      });



      function validateRow(btn) {
          debugger

          var row = btn.closest("tr");

          var Name = row.querySelector("#txtBranchName");
          var Code = row.querySelector("#txtCodeBranch");
          var Address = row.querySelector("#txtAddress");

          var isValid = true;


          [Name, Code, Address].forEach(function (input) {
              if (input) input.style.border = "";
          });

          if (!Name || Name.value.trim() === "") {
              Name.style.border = "2px solid red";
              isValid = false;
          }

          if (!Code || Code.value.trim() === "") {
              Code.style.border = "2px solid red";
              isValid = false;
          }

          if (!Address || Address.value.trim() === "") {
              Address.style.border = "2px solid red";
              isValid = false;
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
            <h1 class="page-header text-overflow" style="color: white;">Branch BAP</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="Home.aspx">Home</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Master<i class="fa fa-caret-right ml-2"></i></li>
            <li class="active">&nbsp;&nbsp;Branch</li>
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
                                    <legend class="scheduler-border">List of Branch BAP</legend>
                                    <div class="row">
                                      <div class='col-sm-12'>
                                           
                                            <asp:Button runat="server" ID="btNew" CssClass="btn  mb-1 buttonColor" Text="Add New" OnClick="btNew_Click" />

                                         </div>

                                        <div class='col-sm-12'>
                                            <!-- Table List of User Registration -->
                                            <div class="table-responsive">
                                                <asp:GridView ID="TableBranch" ClientIDMode="Static" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-striped table-bordered zero-configuration grid"
                                                EmptyDataText="No Record Found"
                                                Style="width: 100%"
                                                ShowHeaderWhenEmpty="true"
                                                OnRowEditing="TableBranch_RowEditing"
                                                OnRowCancelingEdit="TableBranch_RowCancelingEdit"
                                                OnRowUpdating="TableBranch_RowUpdating"
                                                OnRowDataBound="TableBranch_RowDataBound"
                                                DataKeyNames="Branch_id">

                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                         <%--   <asp:CommandField ShowEditButton="True"/>--%>

                                                          <asp:TemplateField HeaderText="Action"  ItemStyle-Width="100px">
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
                                                                             ID="btnDelete" 
                                                                             runat="server" 
                                                                             CommandName="Hapus" 
                                                                             CommandArgument="<%# Container.DataItemIndex %>" 
                                                                             CssClass="btn mb-1 btn-danger edit-btn ml-2"
                                                                             OnClick="btnDelete_Click" 
                                                                             OnClientClick="return confirmDelete(this);" 
                                                                             ToolTip="Remove">
                                                                            <i class="fa  fa-trash"></i>
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
                                                        <asp:TemplateField HeaderText="Branch Name">
                                                          <ItemTemplate>
                                                              <%# Eval("NameBranch") %>
                                                          </ItemTemplate>
                                                          <EditItemTemplate>
                                                              <asp:TextBox 
                                                                  ID="txtBranchName" 
                                                                  runat="server" 
                                                                  Text='<%# Bind("NameBranch") %>' 
                                                                  CssClass="form-control form-control-sm custom-input" 
                                                                  placeholder="Enter Name Branch..." />
                                                          </EditItemTemplate>
                                                      </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Code Branch">
                                                          <ItemTemplate>
                                                              <%# Eval("CodeBranch") %>
                                                          </ItemTemplate>
                                                          <EditItemTemplate>
                                                              <asp:TextBox 
                                                                  ID="txtCodeBranch" 
                                                                  runat="server" 
                                                                  Text='<%# Bind("CodeBranch") %>' 
                                                                  CssClass="form-control form-control-sm custom-input" 
                                                                  placeholder="Enter Code Branch.." />
                                                          </EditItemTemplate>
                                                      </asp:TemplateField>  
                                                       <asp:TemplateField HeaderText="Address">
                                                          <ItemTemplate>
                                                              <%# Eval("Address") %>
                                                          </ItemTemplate>
                                                          <EditItemTemplate>
                                                              <asp:TextBox 
                                                                  ID="txtAddress" 
                                                                  runat="server" 
                                                                  Text='<%# Bind("Address") %>' 
                                                                  CssClass="form-control form-control-sm custom-input" 
                                                                  placeholder="Enter Address..." />
                                                          </EditItemTemplate>
                                                      </asp:TemplateField>            
<%--                                                            <asp:BoundField DataField="CreatedDate" HeaderText="Create Date" ReadOnly="True" DataFormatString="{0:dd-MMMM-yyyy hh:mm:ss tt}"/>--%>
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
  <div class="modal fade bs-example-modal-lg" id="mdlAddNewBranch" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
      <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content">

              <div class="modal-header">
                  <h4 class="modal-title" id="myModalLabel1">Add Branch</h4>
                  <button type="button" onclick="<%=btnCloseModalNew.ClientID %>.click()" class="close"><span>×</span></button>
                  <asp:Button runat="server" Style="display: none;" ID="btnCloseModalNew" OnClick="btnCloseModalNew_Click"></asp:Button>
              </div>
              <div class="modal-body">
                  <div class="x_content">
                      <div class="row">
                          <asp:HiddenField ID="Branch_id" runat="server" />
                         <%--<asp:HiddenField ID="hlbOid" runat="server" />--%>
            <div class='col-sm-12'>
                              Branch Name 
			<div class="form-group" style="margin-top:5px;">
                      <div class='input-group'>
                          <input runat="server" id="txtBranchNameadd" name="NameBranch" data-validate-length-range="5,15" type="text" class="form-control input-rounded required-field" placeholder="Type Branch Name" >
                      </div>
                </div>
           </div>
           <div class='col-sm-12'>
                              Code Branch
			<div class="form-group" style="margin-top:5px;">
                      <div class='input-group'>
                          <input runat="server" id="txtCodeBranchadd" data-validate-length-range="5,15" type="text" class="form-control input-rounded required-field" placeholder="Type Branch Code" >
                      </div>
                  </div>
              </div>
<%--             <div class='col-sm-12'>
                             Address
			<div class="form-group" style="margin-top:5px;">
                      <div class='input-group'>
                          <input runat="server" id="txtAddressadd" data-validate-length-range="5,15" type="text" class="form-control input-rounded required-field" placeholder="Type Address" >
                      </div>
                  </div>
              </div>  --%> 


            <div class='col-sm-12'>
                Address
                <div class="form-group" style="margin-top:5px;">
                    <%--<div class='input-group'>--%>
                        <textarea runat="server"
                               id="txtAddressadd"
                               data-validate-length-range="5,15"
                               type="text"
                               class="form-control required-field"
                               placeholder="Type Address"
                               TextMode="MultiLine"
                               style="height: 100px; width: 100%; border-radius: 0; vertical-align: top; padding-top: 5px; line-height: 1.4; resize: vertical"  wrap="true"  />
                   <%-- </div>--%>

                  
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
