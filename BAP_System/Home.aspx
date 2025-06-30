<%@ Page Title="Home" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BAP_System.Home" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .page-head {
            background-color: #06183d; /* Change this to your desired background color */
            color: white; /* Text color for the header */
            padding: 33px 2.938rem; /* Optional padding for the header */
            height: 130px;
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
            <div class="pad-all text-center">
                <h1 style="color: white;">Welcome to BAP - YLID</h1>
              <%--  <p1>
                    Freight Forwarding Integrated System - YLID<p></p>
                </p1>--%>
            </div>
        </div>
    </div>
    <asp:Image ID="Image2" runat="server" Width="100%" Height="100%" ImageUrl="~/img/Background_Icon20210401.png" />

</asp:Content>