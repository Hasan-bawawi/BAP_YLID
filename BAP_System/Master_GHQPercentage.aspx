
<%@ Page  Title="GHQ Percentage"  MasterPageFile="~/Layout.Master"  Language ="C#" AutoEventWireup="true" CodeBehind="Master_GHQPercentage.aspx.cs" Inherits="BAP_System.Master_GHQPercentage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Untuk file CSS -->
<link href="Styles/sweetalert.css" rel="stylesheet" />
<!-- Untuk file JavaScript -->
<script src="Scripts/sweetalert.min.js"></script>>
    <script type="text/javascript">

        function FuncSave(message) {
            swal(
            {
                title: 'Success',
                text: message,
                timer: 3000,
                type: 'success',
                showConfirmButton: false,
                html: true,
                customClass: 'swal-wide',
            },
                function redirect() {
                    window.location.href = 'Master_GHQPercentage.aspx';
                }
            );
        }


        function Errorsave(msg) {
            swal({
                title: 'Failed!',
                text: msg,
                type: 'error',
                showConfirmButton: false,
                timer: 3000,
                customClass: 'swal-wide',
            });
        }

    </script>

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

    </script>
  
   <style>

       .swal-wide{
            width:500px !important;
        }

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

         .importgrup .btn.same-height {
                height: 100%;
                white-space: nowrap; 
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
            <h1 class="page-header text-overflow" style="color: white;">GHQ Percentage
        </div>
        <ol class="breadcrumb">
            <li><a href="Home.aspx">Home</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Master<i class="fa fa-caret-right ml-2"></i></li>
            <li class="active">&nbsp;&nbsp;GHQ Percentage</li>
        </ol>
    </div>
    <div class="container-fluid">
        <div class="row">   
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List GHQ Percentage </legend>
                                    <div class="row">
                                        <div class='col-sm-12'>
                                            <!-- Table List of User Registration -->
                                            <div class="table-responsive">
                                                <asp:HiddenField ID="Id" runat="server" />
                                                <asp:HiddenField ID="GHQarm" runat="server" />
                                                <asp:HiddenField ID="GlobalHQ" runat="server" />
                                                <asp:GridView ID="TableGHQGen" runat="server" CssClass="table table-striped table-bordered zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowDataBound="TableGHQGen_RowDataBound" 
                                                    OnRowCommand="TableGHQGen_RowCommand" OnSelectedIndexChanged="TableGHQGen_SelectedIndexChanged"
                                                    OnRowEditing="TableGHQGen_RowEditing"
                                                    OnRowCancelingEdit="TableGHQGen_RowCancelingEdit"
                                                    OnRowUpdating="TableGHQGen_RowUpdating"
                                                    DataKeyNames="Id">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                              <asp:TemplateField HeaderText="Action" ItemStyle-Width="100px">
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

                                                                    <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn buttonColor"  Style="margin-right:5px" >
                                                                        <i class="fa fa-save"></i> 
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger">
                                                                        <i class="fa fa-times"></i> 
                                                                    </asp:LinkButton>
                                                                    </div>

                                                                </EditItemTemplate>
                                                            </asp:TemplateField>                                           
                                                        <asp:TemplateField HeaderText="GHQarm Percentage">
                                                                <ItemTemplate>
                                                                       <%# Eval("GHQarm", "{0}%") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox 
                                                                        ID="txtGHQarm" 
                                                                        runat="server" 
                                                                        Text='<%# Bind("GHQarm") %>' 
                                                                        CssClass="form-control form-control-sm custom-input" 
                                                                        placeholder="Enter percentage GHQarm..." 
                                                                        TextMode="Number"
                                                                        oninput="this.value = this.value.replace(/[^0-9]/g, '')" />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                       <asp:TemplateField HeaderText="GlobalHQ Percentage">
                                                                <ItemTemplate>
                                                                      <%# Eval("GlobalHQ", "{0}%") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox 
                                                                        ID="txtGlobalHQ" 
                                                                        runat="server" 
                                                                        Text='<%# Bind("GlobalHQ") %>' 
                                                                        CssClass="form-control form-control-sm custom-input" 
                                                                        placeholder="Enter percentage GHQarm..." 
                                                                        TextMode="Number"
                                                                        oninput="this.value = this.value.replace(/[^0-9]/g, '')" />
                                                                </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Periode" HeaderText="Periode" ReadOnly="True"  DataFormatString="{0:MMMM-yyyy}"/>
<%--                                                        <asp:BoundField DataField="CreatedDate" HeaderText="Create Date" ReadOnly="True" DataFormatString="{0:dd-MMMM-yyyy hh:mm:ss tt}"/>--%>
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


</asp:content>


