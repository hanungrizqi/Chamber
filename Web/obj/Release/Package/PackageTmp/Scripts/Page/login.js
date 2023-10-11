var e = Swal.mixin({ buttonsStyling: !1, customClass: { confirmButton: "btn btn-alt-success m-5", cancelButton: "btn btn-alt-danger m-5", input: "form-control" } });

$("document").ready(function () {
    /*addRoled();*/
    debugger
    $("#yourFormId").validate({
        rules: {
            "val-username": {
                required: true
            },
            "val-password": {
                required: true
            }
        },
        messages: {
            "val-username": {
                required: "Please enter your kNRP."
            },
            "val-password": {
                required: "Please enter your password."
            }
        },
        submitHandler: function (form) {
            debugger
            var obj = {
                Username: $("#val-username").val(),
                Password: $("#val-password").val()
            };

            $.ajax({
                url: $("#web_link").val() + "/api/Login/Get_Login",
                data: JSON.stringify(obj),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    $("#overlay").show();
                },
                success: function (data) {
                    if (data.Remarks == true) {
                        SearchRole(obj.Username);
                    } else {
                        swal.fire({
                            title: "Error!",
                            text: "Username or Password incorrect.",
                            icon: "error"
                        });
                        $("#overlay").hide();
                    }
                },
                error: function (xhr) {
                    swal.fire({
                        title: "Error!",
                        text: "Message: " + xhr.responseText,
                        icon: "error"
                    });
                }
            });
        }
    });
})

function PostLogin() {
    var obj = new Object();
    obj.Username = $("#val-username").val();
    obj.Password = $("#val-password").val();

    //if (obj.Password.trim() === '') {
    //    $('.btn.btn-success').css('display', 'none');
    //    $('#signInButton').css('display', 'block');
    //    return;
    //}

    $.ajax({
        url: $("#web_link").val() + "/api/Login/Get_Login", //URI
        data: JSON.stringify(obj),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            debugger
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                //MakeSession(obj.Username);
                SearchRole(obj.Username);
            }
            else {
                swal.fire({
                    title: "Error!",
                    text: "Username or Password incorrect.",
                    icon: 'error',
                });
                $("#overlay").hide();
            }

        },
        error: function (xhr) {
            swal.fire({
                title: "Error!",
                text: 'Message : ' + xhr.responseText,
                icon: 'error',
            });
        },
    })
}

//function PostLogin2() {
//    var obj = new Object();
//    obj.Username = $("#val-username").val();
//    obj.Password = $("#val-password").val();

//    if (obj.Password.trim() === '') {
//        $('.btn.btn-success').css('display', 'block');
//        $('#signInButton').css('display', 'none');
//        return;
//    }

//    $.ajax({
//        url: $("#web_link").val() + "/api/Login/Get_Login", //URI
//        data: JSON.stringify(obj),
//        dataType: "json",
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//        beforeSend: function () {
//            debugger
//            $("#overlay").show();
//        },
//        success: function (data) {
//            if (data.Remarks == true) {
//                SearchRole(obj.Username);
//            }
//            else {
//                swal.fire({
//                    title: "Error!",
//                    text: "Username or Password incorrect.",
//                    icon: 'error',
//                });
//                $("#overlay").hide();
//            }

//        },
//        error: function (xhr) {
//            swal.fire({
//                title: "Error!",
//                text: 'Message : ' + xhr.responseText,
//                icon: 'error',
//            });
//        },
//    })
//}

var rol;
function SearchRole(nrp) {
    debugger
    var obj = new Object();
    obj.Username = $("#val-username").val();
    obj.Password = $("#val-password").val();

    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_Roled/" + nrp, //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            if (result.Remarks == true) {
                debugger
                rol = result.Data.ID_Role;
                MakeSession(obj.Username, rol)
            }
            else {
                swal.fire({
                    title: "Error!",
                    text: "Maaf anda tidak memiliki akses ke CFM Web",
                    icon: 'error',
                });
                $("#overlay").hide();
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    });
}

function MakeSession(nrp, rol) {
    debugger
    var obj = {
        NRP: nrp,
    };

    $.ajax({
        type: "POST",
        url: "/Login/MakeSession", //URI
        dataType: "json",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                debugger
                if (rol != 3) {
                    window.location.href = "/Home/Index";
                }
                else if (rol == 3) {
                    window.location.href = "/EmpRecord/Index";
                }
                else {
                    window.location.href = "/Home/Index";
                }
            }
            else {
                swal.fire({
                    title: "Error!",
                    text: data.Message,
                    icon: 'error',
                });
                $("#overlay").hide();
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    });

}

function addJobsite() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_JobsiteByUsername?username=" + $("#login-username").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#jobSite').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.DSTRCT_CODE + '">' + val.DSTRCT_CODE + '</option>';
            });
            $("#jobSite").append(text);
        }
    });
}

function addRoled() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_Roled", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#roled').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.RoleName + '">' + val.RoleName + '</option>';
            });
            $("#roled").append(text);
        }
    });
}
