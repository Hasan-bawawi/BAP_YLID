<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BAP_System.Login" %>


<%@ MasterType VirtualPath="~/Layout.Master" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="/Scripts/style-boxlogin.css" rel="stylesheet" />    
    <link rel="icon" href="img/favicon.ico" type="image/x-icon">
    <script type="text/javascript" src="date/JS/jquery-1.10.2.min.js"></script>
    <link href="/Content/Ionicons/css/ionicons.min.css" rel="stylesheet" />
    <link href="/vendors/sweetalert.css" rel="stylesheet" />
    <link href="/css/sweetalert.css" rel="stylesheet" />
    <script src="/js/sweetalert.min.js"></script>

    <link href="/vendors/toastr/toastr.min.css" rel="stylesheet" />
    <script src="/vendors/toastr/toastr.min.js"></script>
    <script type="text/javascript">
        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": true,
            "positionClass": "toast-top-center",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "2000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    </script>


    <script type="text/javascript">
        function SuccessLogin() {
            swal({
                title: 'Login Success',
                text: '<span style=color:grey>WELCOME <label id=txtFullname runat="server"></label></span>',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'Home.aspx';
                }
            );
        }
    </script>

    <script type="text/javascript">
        function InfoBox() {
            swal({
                title: '<strong>Perhatian!</strong>',
                text: '<strong>Welcome to BAP  System</strong>' +
                    '<br/> ' +
                    '<br/> ' +
                    'Login sama dengan login windows pada PC/Laptop, ' +
                    '<br/> ' +
                    '<strong>Contoh -> username:YLID-NIK, password:(sama dengan password login windows pada pc/laptop).</strong>' +
                    '<br/> ' +
                    'Jika belum bisa login menggunakan login windows,silahkan hubungi IT' +
                    '<br/> ' +
                    '<strong>Ext. IT: 213 or 212</strong>',
                type: 'info',
                showConfirmButton: true,
                html: true
            }
            );
        }
    </script>


    <script type="text/javascript">
        function FailedLogin() {
            swal('Login Failed!', 'Username or Password incorrect, please try again', 'error');
        }
    </script>

    <style>
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
                background-color: #06183d;
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
    </style>

    <title>BAP - YLID | Login</title>

</head>

<body>
    <div id="container" class="container">
        <form runat="server" defaultbutton="btnlogin">

            <!-- FORM SECTION -->
            <div class="row">
                <!-- SIGN UP -->
                <div class="col align-items-center flex-col sign-up">
                    <div class="form-wrapper align-items-center">
                        <div class="form sign-up">
                            <div class="input-group">
                                <i class='bx bxs-user'></i>
                                <input type="text" placeholder="Username">
                            </div>
                            <div class="input-group">
                                <i class='bx bx-mail-send'></i>
                                <input type="email" placeholder="Email">
                            </div>
                            <div class="input-group">
                                <i class='bx bxs-lock-alt'></i>
                                <input type="password" placeholder="Password">
                            </div>
                            <div class="input-group">
                                <i class='bx bxs-lock-alt'></i>
                                <input type="password" placeholder="Confirm password">
                            </div>
                            <button>
                                Register
                            </button>
                            <p>
                                <span>Already have an account?
                                </span>
                                <b onclick="toggle()" class="pointer">Login here
                                </b>
                            </p>
                        </div>
                    </div>

                </div>
                <!-- END SIGN UP -->
                <!-- SIGN IN -->
                <div class="col align-items-center flex-col sign-in">

                    <div class="form-wrapper align-items-center">
                        <h1 style="color: white">BAP - YLID</h1>
                    </div>
                    <div class="form-wrapper align-items-center">
                        <a style="color: white">Budget And Performence</a>
                    </div>
                    <div class="form-wrapper align-items-center">
                        &nbsp;
                    </div>

       
                    <div class="form-wrapper align-items-center">
                        <div class="form sign-in">
                            <br />
                            <div>
                                <label class="field field_v1">
                                    <input class="field__input" id="txtusername" runat="server" type="text" placeholder="type your YLID-NIK ">
                                    <span class="field__label-wrap">
                                        <span class="field__label"><i class="fa fa-user"></i>&nbsp;&nbsp;Username (YLID-NIK)</span>
                                    </span>
                                </label>
                            </div>

                            <div>
                                <label class="field field_v1">
                                    <input class="field__input" id="txtpassword" runat="server" type="password" placeholder="type your password...">
                                    <span class="field__label-wrap">
                                        <span class="field__label"><i class="fa fa-key"></i>&nbsp;&nbsp;Password</span>
                                    </span>
                                </label>
                            </div>

                            <p>
                                <b>
                                    <button type="button" onclick="<%=btnlogin.ClientID %>.click()" class="btn mb-1 buttonColor">
                                        Login <span class="btn-icon-right">
                                            <i class="fa fa-sign-in"></i></span>
                                    </button>
                                    <asp:Button runat="server" Style="display: none;" ID="btnlogin" OnClick="btnlogin_Click"></asp:Button>

                                   
                                </b>
                            </p>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                    <div class="form-wrapper align-items-center">
                        &nbsp;
                    </div>
                    <div class="form-wrapper align-items-center">
                        <a style="color: white">©2025 All Rights Reserved. PT. Yusen Logistics Indonesia.</a>
                    </div>
                </div>
              
            </div>

            <!-- END FORM SECTION -->
        </form>
        <!-- CONTENT SECTION -->
        <div class="row content-row">
            <!-- SIGN IN CONTENT -->
            <div class="col align-items-center flex-col">
                <%--<div class="text sign-in">
					<h2>
						Welcome Back
					</h2>
	
				</div>--%>
                <div class="img sign-in">
                    <img src="img/img-log-yusen.png" class="image" alt="" />
                </div>
            </div>
            <!-- END SIGN IN CONTENT -->
            <!-- SIGN UP CONTENT -->
            <div class="col align-items-center flex-col">
                <div class="img sign-up">
                </div>
                <div class="text sign-up">
                </div>

            </div>
            <!-- END SIGN UP CONTENT -->
        </div>
        <!-- END CONTENT SECTION -->
    </div>

    <script src="/Scripts/style-login.js"></script>
    <script src="/vendors/sweetalert.js"></script>
    <script src="/vendors/sweetalert.min.js"></script>

</body>
</html>

