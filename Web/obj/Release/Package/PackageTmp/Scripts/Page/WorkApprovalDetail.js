$("document").ready(function () {
    Detail(function () {
        updateDropdownVisibility();
    });
})
var statusz;
var jumlahperhari;
function Detail(callback) {
    $.ajax({
        url: $("#web_link").val() + "/api/Approval/Detail/" + $("#idapprv").val(), //URI,
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
            $("#txt_date").val(moment(data.WAKTU_ABSEN).format("YYYY-MM-DD"));
            $("#txt_oxygen").val(data.OXYGEN_SATURATION);
            $("#txt_heartrate").val(data.HEART_RATE);
            $("#txt_sys").val(data.SYSTOLIC);
            $("#txt_diastolic").val(data.DIASTOLIC);
            $("#txt_temp").val(data.TEMPRATURE);
            $("#txt_note").val(data.NOTE);
            statusz = data.STATUS;
            jumlahperhari = data.JUMLAH_APPROVAL_PERHARI;
            if (typeof callback === "function") {
                callback();
            }
        }
    });
}

function updateDropdownVisibility() {
    var txtStatus = statusz;
    var txtJumlahperhari = jumlahperhari;
    var approvalDropdown = document.getElementById("approvalDropdown");
    console.log(approvalDropdown);

    if (approvalDropdown) {
        var dropdownItems = approvalDropdown.querySelectorAll(".dropdown-item");
        for (var i = 0; i < dropdownItems.length; i++) {
            dropdownItems[i].style.display = "none";
        }
        debugger
        if (txtStatus === "Unfit") {
            if (txtJumlahperhari === 4) {
                var unfitItem = document.createElement("a");
                unfitItem.setAttribute("class", "dropdown-item");
                unfitItem.setAttribute("onclick", "UBParamedis()");
                unfitItem.innerHTML = '<i class="far fa-fw fa-copy opacity-50 me-1"></i>Unfit Butuh Paramedis';

                var bbkerja = document.createElement("a");
                bbkerja.setAttribute("class", "dropdown-item");
                bbkerja.setAttribute("onclick", "BerhentiBekerja()");
                bbkerja.innerHTML = '<i class="fa fa-fw fa-plane-slash opacity-50 me-1"></i>Berhenti Bekerja';

                approvalDropdown.appendChild(unfitItem);
                approvalDropdown.appendChild(bbkerja);
            }
            else {
                var unfitItem = document.createElement("a");
                unfitItem.setAttribute("class", "dropdown-item");
                unfitItem.setAttribute("onclick", "UBParamedis()");
                unfitItem.innerHTML = '<i class="far fa-fw fa-copy opacity-50 me-1"></i>Unfit Butuh Paramedis';

                var retestItem = document.createElement("a");
                retestItem.setAttribute("class", "dropdown-item");
                retestItem.setAttribute("onclick", "Retests()");
                retestItem.innerHTML = '<i class="fa fa-fw fa-pencil opacity-50 me-1"></i>Retest';

                var bbkerja = document.createElement("a");
                bbkerja.setAttribute("class", "dropdown-item");
                bbkerja.setAttribute("onclick", "BerhentiBekerja()");
                bbkerja.innerHTML = '<i class="fa fa-fw fa-plane-slash opacity-50 me-1"></i>Berhenti Bekerja';

                approvalDropdown.appendChild(unfitItem);
                approvalDropdown.appendChild(retestItem);
                approvalDropdown.appendChild(bbkerja);
            }
        }
        else if (txtStatus === "Fit Need Rest Time") {

            var fitItem = document.createElement("a");
            fitItem.setAttribute("class", "dropdown-item");
            fitItem.setAttribute("onclick", "Fits()");
            fitItem.innerHTML = '<i class="fa fa-fw fa-user-check opacity-50 me-1"></i>Fit';

            var istirahatItem = document.createElement("a");
            istirahatItem.setAttribute("class", "dropdown-item");
            istirahatItem.setAttribute("onclick", "Istirahat()");
            istirahatItem.innerHTML = '<i class="fa fa-fw fa-person-booth opacity-50 me-1"></i>Istirahat';

            approvalDropdown.appendChild(fitItem);
            approvalDropdown.appendChild(istirahatItem);
        }
        else if (txtStatus === "Unfit Butuh Paramedis") {

            var fitItem = document.createElement("a");
            fitItem.setAttribute("class", "dropdown-item");
            fitItem.setAttribute("onclick", "Fits()");
            fitItem.innerHTML = '<i class="fa fa-fw fa-user-check opacity-50 me-1"></i>Fit';

            var bbkerja = document.createElement("a");
            bbkerja.setAttribute("class", "dropdown-item");
            bbkerja.setAttribute("onclick", "BerhentiBekerja()");
            bbkerja.innerHTML = '<i class="fa fa-fw fa-plane-slash opacity-50 me-1"></i>Berhenti Bekerja';

            approvalDropdown.appendChild(fitItem);
            approvalDropdown.appendChild(bbkerja);
        } else if (txtStatus === "Temporary Unfit") {

            var fitItem = document.createElement("a");
            fitItem.setAttribute("class", "dropdown-item");
            fitItem.setAttribute("onclick", "Unfits()");
            fitItem.innerHTML = '<i class="fa fa-fw fa-copy opacity-50 me-1"></i>Unfit';

            var istirahatItem = document.createElement("a");
            istirahatItem.setAttribute("class", "dropdown-item");
            istirahatItem.setAttribute("onclick", "Istirahat()");
            istirahatItem.innerHTML = '<i class="fa fa-fw fa-person-booth opacity-50 me-1"></i>Istirahat';

            approvalDropdown.appendChild(fitItem);
            approvalDropdown.appendChild(istirahatItem);
        }
    } else {
        console.error("Element with ID 'approvalDropdown' not found.");
    }
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

function Retests() {

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = $("#idapprv").val();
    dataCFM.ID_STATUS = 5;
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

function BerhentiBekerja() {

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = $("#idapprv").val();
    dataCFM.ID_STATUS = 7;
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

function Fits() {

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = $("#idapprv").val();
    dataCFM.ID_STATUS = 1;
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

function Istirahat() {

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = $("#idapprv").val();
    dataCFM.ID_STATUS = 6;
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