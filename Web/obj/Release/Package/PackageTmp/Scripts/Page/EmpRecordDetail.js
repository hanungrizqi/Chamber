Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    Detail(function () {
        Status();
    });
    var idProfile = $("#hd_idroles").val();
    if (idProfile == 3 || idProfile == 4) {
        $("#tombolApprove").hide();
    } else {
        $("#tombolApprove").show();
    }

    //var jumlahApprovalPerhari = 0;
    //var approvalids = 0;
    //var combine_approvalids = 0;

    //Detail(function (jumlah) {

    //    var idProfile = $("#hd_idroles").val();
    //    if (idProfile == 3) {
    //        $("#tombolApprove").hide();
    //    } else {
    //        $("#tombolApprove").show();
    //    }

    //    jumlahApprovalPerhari = jumlah;
    //    approvalids = approvalid;
    //    combine_approvalids = combine_approvalid;
    //    Status();

    //    if (jumlahApprovalPerhari === 2 || jumlahApprovalPerhari === 3) {
    //        debugger
    //        if (approvalids === combine_approvalids) {
    //            // Disable the dropdown
    //            $("#txt_status").prop("disabled", true);
    //        }
    //    }
    //});
})

var approvalid;
var combine_approvalid;
var idstatss;
var data2;
function Detail(callback) {
    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Detail/" + $("#idapprv").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            var data = result.Data;
            $("#txt_nrp").val(data.NRP);
            $("#txt_name").val(data.NAME);
            $("#txt_chamber").val(data.ID_CHAMBER);
            $("#txt_postitle").val(data.POS_TITLE);
            $("#txt_atasan").val(data.ATASAN);
            $("#txt_status").val(data.STATUS);
            $("#txt_date").val(moment(data.DATE_FROM_CFC).format("YYYY-MM-DD"));
            $("#txt_oxygen").val(data.OXYGEN_SATURATION);
            $("#txt_heartrate").val(data.HEART_RATE);
            $("#txt_sys").val(data.SYSTOLIC);
            $("#txt_diastolic").val(data.DIASTOLIC);
            $("#txt_temp").val(data.TEMPRATURE);
            $("#txt_note").val(data.NOTE);
            data2 = data.STATUS;
            approvalid = data.APPROVAL_ID;
            idstatss = data.ID_STATUS;
            //if (typeof callback === "function") {
            //    callback();
            //}

            var jumlahApprovalPerhari = data.JUMLAH_APPROVAL_PERHARI;

            recordData(jumlahApprovalPerhari, data.NRP, moment(data.DATE_FROM_CFC).format("YYYY-MM-DD"));
            //debugger
            if (typeof callback === "function") {
                callback(/*jumlahApprovalPerhari, combine_approvalid*/);
            }

        }
    });
}

function recordData(jumlahApprovalPerhari, nrp, dateFromCFC) {
    debugger
    $.ajax({
        url: $("#web_link").val() + `/api/EmpRecord/RecordData?nrp=${nrp}&datefromcfc=${dateFromCFC}&jmlapprvlperhari=${jumlahApprovalPerhari}`,
        type: "GET",
        cache: false,
        success: function (result) {
            debugger
            var datas = result.Data;
            combine_approvalid = datas.APPROVAL_ID;

            //if (idstatss == 5) {
                if (jumlahApprovalPerhari >= 2) {
                    debugger
                    if (approvalid == datas.APPROVAL_ID /*combine_approvalid*/) {
                        $("#txt_status").prop("disabled", false);
                    }
                    else {
                        $("#txt_status").prop("disabled", true);
                    }
                }
            //}
        }
    });
}

function Status() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Status", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $('#txt_status').empty();
            text = '<option></option>';
            var dataStatus = data2;
            $.each(result.Data, function (key, val) {
                if (val.STATUS == dataStatus) {
                    text += '<option selected value="' + val.ID_STATUS + '">' + val.STATUS + '</option>';
                } else {
                    text += '<option value="' + val.ID_STATUS + '">' + val.STATUS + '</option>';
                }
            });
            $("#txt_status").append(text);
        }
    });
}

function Approves() {

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = $("#idapprv").val();
    dataCFM.ID_STATUS = $("#txt_status").val();
    dataCFM.APPROVER = $("#hd_nrp").val();
    dataCFM.NOTED = $("#txt_note").val();
    debugger

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Approve",
        data: JSON.stringify(dataCFM),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks) {
                Swal.fire({
                    title: 'Saved',
                    text: "Data has been Saved.",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/EmpRecord/Index";
                    }
                });
            } else {
                Swal.fire(
                    'Error!',
                    'Message: ' + data.Message,
                    'error'
                );
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function Unfits() {

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = $("#idapprv").val();
    dataCFM.ID_STATUS = 2;
    dataCFM.APPROVER = $("#hd_nrp").val();
    dataCFM.NOTED = $("#txt_note").val();
    debugger

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Approve",
        data: JSON.stringify(dataCFM),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks) {
                Swal.fire({
                    title: 'Saved',
                    text: "Data has been Saved.",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/EmpRecord/Index";
                    }
                });
            } else {
                Swal.fire(
                    'Error!',
                    'Message: ' + data.Message,
                    'error'
                );
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function FNRestime() {

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = $("#idapprv").val();
    dataCFM.ID_STATUS = 3;
    dataCFM.APPROVER = $("#hd_nrp").val();
    dataCFM.NOTED = $("#txt_note").val();
    debugger

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Approve",
        data: JSON.stringify(dataCFM),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks) {
                Swal.fire({
                    title: 'Saved',
                    text: "Data has been Saved.",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/EmpRecord/Index";
                    }
                });
            } else {
                Swal.fire(
                    'Error!',
                    'Message: ' + data.Message,
                    'error'
                );
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function UBParamedis() {

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = $("#idapprv").val();
    dataCFM.ID_STATUS = 4;
    dataCFM.APPROVER = $("#hd_nrp").val();
    dataCFM.NOTED = $("#txt_note").val();
    debugger

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Approve",
        data: JSON.stringify(dataCFM),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks) {
                Swal.fire({
                    title: 'Saved',
                    text: "Data has been Saved.",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/EmpRecord/Index";
                    }
                });
            } else {
                Swal.fire(
                    'Error!',
                    'Message: ' + data.Message,
                    'error'
                );
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function RTest() {

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = $("#idapprv").val();
    dataCFM.ID_STATUS = 5;
    dataCFM.APPROVER = $("#hd_nrp").val();
    dataCFM.NOTED = $("#txt_note").val();
    debugger

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Approve",
        data: JSON.stringify(dataCFM),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks) {
                Swal.fire({
                    title: 'Saved',
                    text: "Data has been Saved.",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/EmpRecord/Index";
                    }
                });
            } else {
                Swal.fire(
                    'Error!',
                    'Message: ' + data.Message,
                    'error'
                );
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function Istirahat() {

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = $("#idapprv").val();
    dataCFM.ID_STATUS = 6;
    dataCFM.APPROVER = $("#hd_nrp").val();
    dataCFM.NOTED = $("#txt_note").val();
    debugger

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Approve",
        data: JSON.stringify(dataCFM),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks) {
                Swal.fire({
                    title: 'Saved',
                    text: "Data has been Saved.",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/EmpRecord/Index";
                    }
                });
            } else {
                Swal.fire(
                    'Error!',
                    'Message: ' + data.Message,
                    'error'
                );
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function BKerja() {

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = $("#idapprv").val();
    dataCFM.ID_STATUS = 7;
    dataCFM.APPROVER = $("#hd_nrp").val();
    dataCFM.NOTED = $("#txt_note").val();
    debugger

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Approve",
        data: JSON.stringify(dataCFM),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks) {
                Swal.fire({
                    title: 'Saved',
                    text: "Data has been Saved.",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/EmpRecord/Index";
                    }
                });
            } else {
                Swal.fire(
                    'Error!',
                    'Message: ' + data.Message,
                    'error'
                );
            }
        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}