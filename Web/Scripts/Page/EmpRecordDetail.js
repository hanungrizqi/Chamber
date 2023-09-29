Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    Detail();
    Status()
})

function Detail() {
    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Detail/" + $("#idapprv").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            debugger
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
        }
    });
}

function Status() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Status", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_status').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.STATUS == $("#txt_status").val()) {
                    text += '<option selected value="' + val.STATUS + '">' + val.STATUS + '</option>';
                } else {
                    text += '<option value="' + val.STATUS + '">' + val.STATUS + '</option>';
                }
            });
            $("#txt_status").append(text);
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
        url: $("#web_link").val() + "/api/Approval/Approve",
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
                        window.location.href = "/Approval/Index";
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
        url: $("#web_link").val() + "/api/Approval/Approve",
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
                        window.location.href = "/Approval/Index";
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
        url: $("#web_link").val() + "/api/Approval/Approve",
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
                        window.location.href = "/Approval/Index";
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