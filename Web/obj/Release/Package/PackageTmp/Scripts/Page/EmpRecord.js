Codebase.helpersOnLoad(['cb-table-tools-checkable', 'cb-table-tools-sections']);

$(document).on('click', '.action-link', function (e) {
    e.preventDefault();
    var approvalId = $(this).data('approvalid');
    var action = $(this).data('action');

    if (action === 'Fit') {
        debugger
        Fit(approvalId);
    } else if (action === 'Unfit') {
        Unfit(approvalId);
    } else if (action === 'Fit Need Rest Time') {
        Fit_Need_Rest_Time(approvalId);
    } else if (action === 'Unfit, Wait for GL Other Instruction') {
        Unfit_Wait_GL_Instruction(approvalId);
    } else if (action === 'Retest') {
        Retest(approvalId);
    } else if (action === 'Istirahat') {
        Istirahat(approvalId);
    }
});

var table = $("#tbl_empr").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Emprecord/Get_ListEmprecord",
        dataSrc: "Data",
    },
    fixedHeader: {
        header: true, // Aktifkan header tetap
    },
    "columnDefs": [
        { "className": "dt-nowrap", "targets": '_all' }
    ],
    "searching": true,
    scrollX: true,
    columns: [
        {
            "data": null,
            render: function (data, type, row, meta) {
                return '<input type="checkbox" class="row-checkbox" data-id="' + row.APPROVAL_ID + '">';
            }
        },
        {
            data: 'NAME',
            render: function (data, type, row) {
                var email = row.EMAIL;
                if (email) {
                    return data + '<p class="fs-sm text-muted mb-0">' + email + '</p>';
                } else {
                    return data;
                }
            }
        },
        { data: 'POS_TITLE' },
        {
            data: 'STATUS',
            render: function (data, type, row) {
                let text = '';
                if (data == "Fit") {
                    text = `<span class="badge" style="background-color: #dfffde; color: #4ffe55; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px;"></i> ${data}</span>`;
                } else if (data == "Unfit") {
                    text = `<span class="badge" style="background-color: #ffdede; color: #fe4f4f; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px;"></i> ${data}</span>`;
                } else if (data == "Retest") {
                    text = `<span class="badge" style="background-color: #E3E3FF; color: #927FB3; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px;"></i> ${data}</span>`;
                } else if (data == "Fit Need Rest Time") {
                    text = `<span class="badge" style="background-color: #defeff; color: #4fecfe; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px;"></i> ${data}</span>`;
                } else if (data == "Unfit, Wait for GL Other Instruction") {
                    text = `<span class="badge" style="background-color: #ffdef3; color: #fe4f4f; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px;"></i> ${data}</span>`;
                } else if (data == "Istirahat") {
                    text = `<span class="badge" style="background-color: #fff0de; color: #feb24f; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px;"></i> ${data}</span>`;
                } else {
                    text = `<span class="badge bg-info">${data}</span>`;
                }
                return text;
            }
        },
        {
            data: 'DATE_FROM_CFC',
            render: function (data, type, row) {
                const tanggal = moment(data).format("DD/MM/YYYY");
                return tanggal;
            }
        },
        { data: 'ID_CHAMBER' },
        {
            data: 'APPROVAL_ID',
            targets: 'no-sort',
            orderable: false,
            render: function (data, type, row) {
                var actions = '<div class="btn-group">';
                actions += '<button class="btn btn-sm" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-ellipsis-vertical"></i></button>';
                actions += '<ul class="dropdown-menu dropdown-menu-right">';
                actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Fit" href="#">Fit</a></li>';
                actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Unfit" href="#">Unfit</a></li>';
                actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Fit Need Rest Time" href="#">Fit Need Rest Time</a></li>';
                actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Unfit, Wait for GL Other Instruction" href="#">Unfit, Wait for GL Other Instruction</a></li>';
                actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Retest" href="#">Retest</a></li>';
                actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Istirahat" href="#">Istirahat</a></li>';
                actions += '</ul>';
                actions += '</div>';
                return actions;
            }
        },
    ],
    initComplete: function () {
        var headerCheckbox = document.getElementById('checkAll');
        var rowCheckboxes = document.getElementsByClassName('row-checkbox');
        headerCheckbox.addEventListener('change', function () {
            var isChecked = headerCheckbox.checked;
            for (var i = 0; i < rowCheckboxes.length; i++) {
                rowCheckboxes[i].checked = isChecked;
            }
        });
        this.api()
            .columns(5)
            .every(function () {
                var column = this;
                var select = $('<select class="form-control form-control-sm" style="width:200px; display:inline-block; margin-left: 10px;"><option value="">-- CHAMBER --</option></select>')
                    .appendTo($("#tbl_empr_filter.dataTables_filter"))
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex($(this).val());
                        column.search(val ? '^' + val + '$' : '', true, false).draw();
                    });
                column
                    .data()
                    .unique()
                    .sort()
                    .each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>');
                    });
            });
        $("#search").on("keyup", function () {
            table.search(this.value).draw();
        });
    },
});

table.on('draw', function () {
    var visibleCheckboxes = document.querySelectorAll('#tbl_empr tbody .row-checkbox:checked');

    visibleCheckboxes.forEach(function (checkbox) {
        checkbox.checked = false;
    });
});

function Fit(approvalId) {
    console.log('Fit', approvalId);
    debugger

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = approvalId;
    dataCFM.ID_STATUS = 1;
    dataCFM.APPROVER = $("#hd_nrp").val();

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Actions",
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

function Unfit(approvalId) {
    console.log('Unfit', approvalId);
    debugger

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = approvalId;
    dataCFM.ID_STATUS = 2;
    dataCFM.APPROVER = $("#hd_nrp").val();

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Actions",
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

function Fit_Need_Rest_Time(approvalId) {
    console.log('Fit Need Rest Time', approvalId);
    debugger

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = approvalId;
    dataCFM.ID_STATUS = 3;
    dataCFM.APPROVER = $("#hd_nrp").val();

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Actions",
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

function Unfit_Wait_GL_Instruction(approvalId) {
    console.log('Unfit, Wait for GL Other Instruction', approvalId);
    debugger

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = approvalId;
    dataCFM.ID_STATUS = 4;
    dataCFM.APPROVER = $("#hd_nrp").val();

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Actions",
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

function Retest(approvalId) {
    console.log('Retest', approvalId);
    debugger

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = approvalId;
    dataCFM.ID_STATUS = 5;
    dataCFM.APPROVER = $("#hd_nrp").val();

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Actions",
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

function Istirahat(approvalId) {
    console.log('Istirahat', approvalId);
    debugger

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = approvalId;
    dataCFM.ID_STATUS = 6;
    dataCFM.APPROVER = $("#hd_nrp").val();

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/Actions",
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