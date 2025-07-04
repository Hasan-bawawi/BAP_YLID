<%@ Page  Title="Import BAP"  MasterPageFile="~/Layout.Master"  Language ="C#" AutoEventWireup="true" CodeBehind="Transaksi_ImportBAP.aspx.cs" Inherits="BAP_System.Transaksi_ImportBAP" %>

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
                    window.location.href = 'Transaksi_ImportBAP.aspx';
                }
            );
        }


        function Errorsave(msg) {
            swal({
                title: 'Failed!',
                text: msg,
                type: 'error',
                showConfirmButton: true,
                timer: 10000,
                customClass: 'swal-wide',
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
            <h1 class="page-header text-overflow" style="color: white;">Import BAP</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="Home.aspx">Home</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Transaksi<i class="fa fa-caret-right ml-2"></i></li>
            <li class="active">&nbsp;&nbsp;Import BAP</li>
        </ol>
    </div>
<%--    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List Summary BAP</legend>
                                    <div class="row">
                                        <div class="d-flex align-items-center gap-2">
                                       <div class='col-sm-5'>
                                         <button type="button" onclick="<%=btNImport.ClientID %>.click()" class="btn btn-lg  btn-success">                                             
                                               Import
                                             <i class="fa  fa-upload ml-1"></i>
                                         </button>
                                         <asp:Button runat="server" Style="display: none;" ID="btNImport" OnClick="btNImport_Click"></asp:Button>                                      
                                     </div>

                                     <div class='col-sm-12'>
                                         <button type="button" onclick="<%=btnMenu.ClientID %>.click()" class="btn  btn-lg buttonColor">                                             
                                               Export Menu
                                             <i class="fa  fa-arrow-right ml-1"></i>
                                         </button>
                                         <asp:Button runat="server" Style="display: none;" ID="btnMenu" OnClick="btNMenu_Click"></asp:Button>                                      
                                     </div>
                                       </div>
                                        <div class='col-sm-12'>
                                            <!-- Table List of User Registration -->
                                            <div class="table-responsive">
                                                <asp:GridView ID="TableImportBAP" runat="server" CssClass="table table-striped table-bordered zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowDataBound="TableImportBAP_RowDataBound" OnRowCommand="TableImportBAP_RowCommand" OnSelectedIndexChanged="TableImportBAP_SelectedIndexChanged"
                                                    DataKeyNames="id,Branch_id,BussinesMode_id,Direction_id,Departement_id,GL_id">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                        <asp:BoundField DataField="GLAccount" HeaderText="GL Account" />
                                                        <asp:BoundField DataField="GLName" HeaderText="GL Name" />
                                                        <asp:BoundField DataField="Type_GL" HeaderText="Type" />
                                                        <asp:BoundField DataField="CodeBranch" HeaderText="Branch" />
                                                        <asp:BoundField DataField="Codename" HeaderText="Bussines Mode" />
                                                        <asp:BoundField DataField="CodeDept" HeaderText="Departement" />
                                                        <asp:BoundField DataField="DtType" HeaderText="Direction" />
                                                        <asp:BoundField DataField="Activity" HeaderText="Activity" />
                                                        <asp:BoundField DataField="Periode" HeaderText="Period" ReadOnly="True" DataFormatString="{0:MMMM-yyyy}"/>
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
    </div>--%>


  <div class="container-fluid">
    <div class="row">
        <div class="col-md-12 col-sm-12">
            <div class="card custom-card-body">
                <div class="card-body">
                    <div class="row">
                        <div class="col-xl-12">
                            <div class="card border-left-primary shadow h-100 py-2">
                                <div class="card-body"> <!-- card 1  -->
                                    <div class="row no-gutters align-items-center">
                                        <div class="col-12">
                                            <div class="form-group">
                                                    <div class="mt-4 d-flex gap-3">
                                                        <div>
                                                            <button type="button" onclick="<%=btNImport.ClientID %>.click()" class="btn btn-lg btn-success px-5" style="margin-right:10px; height: 50px; line-height: 1.2;">                                             
                                                                Import
                                                                <i class="fa fa-upload ml-1"></i>
                                                            </button>
                                                            <asp:Button runat="server" Style="display: none;" ID="btNImport" OnClick="btNImport_Click"></asp:Button>                                      
                                                        </div>

                                                        <div>
                                                            <button type="button" onclick="<%=btnMenu.ClientID %>.click()" class="btn btn-lg buttonColor px-5" style="height: 50px; line-height: 1.2;">                                             
                                                                Report Menu
                                                                <i class="fa fa-arrow-right ml-1"></i>
                                                            </button>
                                                            <asp:Button runat="server" Style="display: none;" ID="btnMenu" OnClick="btNMenu_Click"></asp:Button>                                      
                                                        </div>
                                                    </div>

                                             </div>                                              
                                            </div>
                                        </div> <!-- /.col-12 -->
                                    </div> <!-- /.row no-gutters -->              
                                 </div> <!-- /.card-body -->
                           </div> <!-- /.card -->
                        </div> <!-- /.col-xl-12 -->
                    </div> <!-- /.row -->
                </div> <!-- /.card-body -->
            </div> <!-- /.card -->
        </div>
    </div>

    <div class="modal fade" id="mdlImport" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
    <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">

            <!-- Header -->
            <div class="modal-header">
                <h5 class="modal-title">Import BAP</h5>
                <button type="button" class="close" aria-label="Close"
                        onclick="<%=btnCloseModalNew.ClientID %>.click()">
                    <span aria-hidden="true">&times;</span>
                </button>
                <asp:Button runat="server" Style="display: none;" ID="btnCloseModalNew" OnClick="btnCloseModalNew_Click" />
            </div>

            <!-- Body -->
            <div class="modal-body">
                <div class="form-group">
                    <label for="FileUpload1">Pilih file Excel :</label>
                    <div class="input-group">
                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                        <div class="input-group-append">
                            <button type="button" class="btn buttonColor ml-2 rounded"
                                    onclick="document.getElementById('<%= btnUpload.ClientID %>').click();">
                                Upload <i class="fa fa-file-excel ml-1"></i>
                            </button>
                            <asp:Button ID="btnUpload" runat="server" Style="display: none;" OnClick="btnUpload_Click" />

                            <button type="button" class="btn btn-primary ml-2 rounded"
                                    onclick="document.getElementById('<%= btnDownloadTemplate.ClientID %>').click();">
                                Download file Template <i class="fas fa-file-download ml-1"></i>
                            </button>
                            <asp:Button ID="btnDownloadTemplate" runat="server" Style="display: none;" OnClick="btnDownloadTemplate_Click" />
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</div>
</asp:content>
