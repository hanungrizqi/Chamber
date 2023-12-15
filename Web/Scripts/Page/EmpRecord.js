Codebase.helpersOnLoad(['cb-table-tools-checkable', 'cb-table-tools-sections', 'js-flatpickr']);

$("document").ready(function () {
    console.log($('#sD').val())
    console.log($('#eD').val())
    console.log($('#stats').val())
    if ($('#sD').val()) {
        //debugger
        table.ajax.url($("#web_link").val() + "/api/EmpRecord/Get_ListEmprecord_Daterange?posid=" + $("#hd_positid").val() + "&startDate=" + $('#sD').val() + "&endDate=" + $('#eD').val()).load();
        $("#example-flatpickr-range").flatpickr({
            mode: "range",
            defaultDate: [$('#sD').val(), $('#eD').val()],
            
        });
        initializeCustomFilter($('#stats').val())
    }
    //debugger
    $("#example-flatpickr-range").flatpickr({
        mode: "range",
        onChange: function (selectedDates, dateStr, instance) {
            if (selectedDates.length === 2) {
                var startDate = selectedDates[0];
                var endDate = selectedDates[1];

                var currentTime = new Date();
                var startDateFormatted = startDate.getFullYear() + '-' +
                    ('0' + (startDate.getMonth() + 1)).slice(-2) + '-' +
                    ('0' + startDate.getDate()).slice(-2) + ' ' +
                    '00' + ':' +
                    '00' + ':' +
                    '00' + '.' +
                    currentTime.getMilliseconds();

                var endDateFormatted = endDate.getFullYear() + '-' +
                    ('0' + (endDate.getMonth() + 1)).slice(-2) + '-' +
                    ('0' + endDate.getDate()).slice(-2) + ' ' +
                    ('0' + currentTime.getHours()).slice(-2) + ':' +
                    ('0' + currentTime.getMinutes()).slice(-2) + ':' +
                    ('0' + currentTime.getSeconds()).slice(-2) + '.' +
                    currentTime.getMilliseconds();
                //debugger
                if ($("#hd_idroles").val() == 3) {
                    table.ajax.url($("#web_link").val() + "/api/EmpRecord/Get_ListEmprecord_Daterange_Operator?nrp=" + $("#hd_nrp").val() + "&startDate=" + startDateFormatted + "&endDate=" + endDateFormatted).load();
                }
                else {
                    table.ajax.url($("#web_link").val() + "/api/EmpRecord/Get_ListEmprecord_Daterange?posid=" + $("#hd_positid").val() + "&startDate=" + startDateFormatted + "&endDate=" + endDateFormatted).load();

                }
                
            }
        },
    });
})

$(document).on('click', '.action-link', function (e) {
    e.preventDefault();
    var approvalId = $(this).data('approvalid');
    var action = $(this).data('action');

    if (action === 'Unfit') {
        Unfit(approvalId);
    } else if (action === 'Fit Need Rest Time') {
        Fit_Need_Rest_Time(approvalId);
    } else if (action === 'Unfit Butuh Paramedis') {
        UnfitBParam(approvalId);
    } else if (action === 'Retest') {
        Retest(approvalId);
    } else if (action === 'Istirahat') {
        Istirahat(approvalId);
    } else if (action === 'Berhenti Bekerja') {
        BerhentiBekerja(approvalId);
    } else if (action === 'Fit') {
        Fitt(approvalId);
    } else if (action === 'Temporary Unfit') {
        TempUnfit(approvalId);
    }
});

var ajaxConfig;
if ($("#hd_idroles").val() == 3) {
    ajaxConfig = {
        url: $("#web_link").val() + "/api/Emprecord/Get_ListEmprecord_Operator/" + $("#hd_nrp").val(),
        dataSrc: "Data"
    };
} else {
    ajaxConfig = {
        url: $("#web_link").val() + "/api/Emprecord/Get_ListEmprecord/" + $("#hd_positid").val(),
        dataSrc: "Data"
    };
}

var table = $("#tbl_empr").DataTable({
    ajax: ajaxConfig,
    fixedHeader: {
        header: true,
    },
    "columnDefs": [
        { "className": "dt-nowrap", "targets": '_all' },
        {
            "targets": '_all',
            "createdCell": function (td, cellData, rowData, row, col) {
                if (col === 0) {
                    $(td).parent().attr("id", "row_" + rowData.APPROVAL_ID);
                }
            }
        },
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
                var oxy = row.OXYGEN_SATURATION;
                var heart = row.HEART_RATE;
                var systo = row.SYSTOLIC;
                var diasto = row.DIASTOLIC;
                var tempra = row.TEMPRATURE;
                var note = row.NOTE;
                var text = '';

                if (note && typeof note === 'string')
                {
                    var noteValues = note.split(',');
                    for (var i = 0; i < noteValues.length; i++) {
                        var noteValue = noteValues[i].trim();
                        switch (noteValue) {
                            case 'heart_rate':
                                text += 'HEART = ' + heart;
                                break;
                            case 'systolic':
                                text += 'SYSTOLIC = ' + systo;
                                break;
                            case 'diastolic':
                                text += 'DIASTOLIC = ' + diasto;
                                break;
                            case 'temprature':
                                text += 'TEMPERATURE = ' + tempra;
                                break;
                            case 'oxygen_saturation':
                                text += 'OXYGEN = ' + oxy;
                                break;
                            default:
                            //do nothing
                        }
                        if (i < noteValues.length - 1) {
                            text += ', ';
                        }
                    }
                }

                if (text) {
                    return data + '<p class="fs-sm text-muted mb-0">' + text + '</p>';
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
                    text = `<span class="badge" style="background-color: #dfffde; color: #4ffe55; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px; margin-right: 4px;"></i>${data}</span>`;
                } else if (data == "Unfit") {
                    text = `<span class="badge" style="background-color: #ffdede; color: #fe4f4f; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px; margin-right: 4px;"></i>${data}</span>`;
                } else if (data == "Retest") {
                    text = `<span class="badge" style="background-color: #E3E3FF; color: #927FB3; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px; margin-right: 4px;"></i>${data}</span>`;
                } else if (data == "Fit Need Rest Time") {
                    text = `<span class="badge" style="background-color: #defeff; color: #4fecfe; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px; margin-right: 4px;"></i>${data}</span>`;
                } else if (data == "Unfit Butuh Paramedis") {
                    text = `<span class="badge" style="background-color: #ffdef3; color: #fe4f4f; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px; margin-right: 4px;"></i>${data}</span>`;
                } else if (data == "Istirahat") {
                    text = `<span class="badge" style="background-color: #fff0de; color: #feb24f; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px; margin-right: 4px;"></i>${data}</span>`;
                } else if (data == "Berhenti Bekerja") {
                    text = `<span class="badge" style="background-color: #ff9999; color: #ff1c1c; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px; margin-right: 4px;"></i>${data}</span>`;
                } else if (data == "Temporary Unfit") {
                    text = `<span class="badge" style="background-color: #fac6a5; color: #f78b1e; font-size: 12px; font-family: Poppins; font-weight: 500; word-wrap: break-word;"><i class="fa fa-circle" style="font-size: 6px; vertical-align: middle; margin-top: -2px; margin-right: 4px;"></i>${data}</span>`;
                } else {
                    text = `<span class="badge bg-info">${data}</span>`;
                }
                return text;
            }
        },
        {
            data: 'WAKTU_ABSEN',
            render: function (data, type, row) {
                if (type == 'display') {
                    return moment(data).format("DD/MM/YYYY HH:mm");
                }
                return data;
            },
            type: 'date',
        },
        { data: 'ID_CHAMBER' },
        {
            data: 'APPROVAL_ID',
            targets: 'no-sort',
            orderable: false,
            render: function (data, type, row) {
                var actions = '<div class="btn-group">';
                actions += '<button class="btn btn-sm" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-ellipsis-vertical"></i></button>';
                if ($("#hd_idroles").val() == 4 || $("#hd_idroles").val() == 3) {
                    //do nothing
                }
                else
                {
                    if (row.IS_LATEST == true && row.ID_STATUS == 5) {
                        actions += '<ul class="dropdown-menu dropdown-menu-right">';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Fit" href="#">Fit</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Unfit" href="#">Unfit</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Fit Need Rest Time" href="#">Fit Need Rest Time</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Unfit Butuh Paramedis" href="#">Unfit Butuh Paramedis</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Retest" href="#">Retest</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Istirahat" href="#">Istirahat</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Berhenti Bekerja" href="#">Berhenti Bekerja</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Temporary Unfit" href="#">Temporary Unfit</a></li>';
                        actions += '</ul>';
                    }
                    if (row.IS_LATEST == true && row.ID_STATUS != 5) {
                        actions += '<ul class="dropdown-menu dropdown-menu-right">';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Fit" href="#">Fit</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Unfit" href="#">Unfit</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Fit Need Rest Time" href="#">Fit Need Rest Time</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Unfit Butuh Paramedis" href="#">Unfit Butuh Paramedis</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Retest" href="#">Retest</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Istirahat" href="#">Istirahat</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Berhenti Bekerja" href="#">Berhenti Bekerja</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Temporary Unfit" href="#">Temporary Unfit</a></li>';
                        actions += '</ul>';
                    }
                    if (row.IS_LATEST == false && row.ID_STATUS == 5 && row.JUMLAH_APPROVAL_PERHARI == 1) {
                        actions += '<ul class="dropdown-menu dropdown-menu-right">';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Fit" href="#">Fit</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Unfit" href="#">Unfit</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Fit Need Rest Time" href="#">Fit Need Rest Time</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Unfit Butuh Paramedis" href="#">Unfit Butuh Paramedis</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Retest" href="#">Retest</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Istirahat" href="#">Istirahat</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Berhenti Bekerja" href="#">Berhenti Bekerja</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Temporary Unfit" href="#">Temporary Unfit</a></li>';
                        actions += '</ul>';
                    }
                    else if (row.IS_LATEST == false && row.ID_STATUS != 5) {
                        actions += '<ul class="dropdown-menu dropdown-menu-right">';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Fit" href="#">Fit</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Unfit" href="#">Unfit</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Fit Need Rest Time" href="#">Fit Need Rest Time</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Unfit Butuh Paramedis" href="#">Unfit Butuh Paramedis</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Retest" href="#">Retest</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Istirahat" href="#">Istirahat</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Berhenti Bekerja" href="#">Berhenti Bekerja</a></li>';
                        actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-action="Temporary Unfit" href="#">Temporary Unfit</a></li>';
                        actions += '</ul>';
                    }
                    else {
                        //DO NOTHING
                    }
                }
                actions += '</div>';
                return actions;
            }
        },
    ],
    initComplete: function () {
        console.log($('#tbl_empr').DataTable().rows().data().toArray())
        var headerCheckbox = document.getElementById('checkAll');
        var rowCheckboxes = document.getElementsByClassName('row-checkbox');
        headerCheckbox.addEventListener('change', function () {
            var isChecked = headerCheckbox.checked;
            for (var i = 0; i < rowCheckboxes.length; i++) {
                rowCheckboxes[i].checked = isChecked;
            }
        });
        this.api()
            .columns(3)
            .every(function () {
                var column = this;
                var select = $('<select class="form-control form-control-sm" style="width:200px; display:inline-block; margin-left: 10px;"><option value="">-- LAST STATUS --</option></select>')
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
        initializeCustomFilter($('#stats').val())
        $("#search").on("keyup", function () {
            table.search(this.value).draw();
        });

    },
});

function initializeCustomFilter(filter) {
    var selectDropdown = $("#tbl_empr_filter select");
    selectDropdown.val(filter);
    table.columns(3)
         .search(filter)
         .draw();
}

$("#downloadButton").on("click", function () {
    generatePDF();
});

$("#downloadButton2").on("click", function () {
    generatePDF();
});

function generatePDF() {
    var doc = new jsPDF();
    doc.autoTable({ html: '#tbl_empr' });
    doc.save('EmployeeRecord.pdf');
}

$('#tbl_empr tbody').on('click', 'tr', function (e) {
    var columnIndex = $(e.target).closest('td').index();
    if (columnIndex === 1 || columnIndex === 2 || columnIndex === 3 || columnIndex === 4 || columnIndex === 5) {
        var rowId = this.id; // Get the unique row ID
        var approvalId = rowId.split('_')[1]; // Extract the APPROVAL_ID
        var url = '/EmpRecord/Detail_ERecord?id=' + approvalId;
        window.location.href = url;
    }
});

table.on('draw', function () {
    var visibleCheckboxes = document.querySelectorAll('#tbl_empr tbody .row-checkbox:checked');

    visibleCheckboxes.forEach(function (checkbox) {
        checkbox.checked = false;
    });
});

function isClosestToCurrentDate(date) {
    var currentDate = new Date();
    var rowDate = new Date(date);
    var diff = Math.abs(currentDate - rowDate);
    var thresholdMilliseconds = 60 * 60 * 1000; // 1 hour
    return diff <= thresholdMilliseconds;
}

function Fitt(approvalId) {
    console.log('Fit', approvalId);
    debugger

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = approvalId;
    dataCFM.ID_STATUS = 1;
    dataCFM.APPROVER = $("#hd_nrp").val();

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/QuickActions",
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
        url: $("#web_link").val() + "/api/EmpRecord/QuickActions",
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

function TempUnfit(approvalId) {
    console.log('Temporary Unfit', approvalId);
    debugger

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = approvalId;
    dataCFM.ID_STATUS = 8;
    dataCFM.APPROVER = $("#hd_nrp").val();

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/QuickActions",
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
        url: $("#web_link").val() + "/api/EmpRecord/QuickActions",
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

function UnfitBParam(approvalId) {
    console.log('Unfit, Wait for GL Other Instruction', approvalId);
    debugger

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = approvalId;
    dataCFM.ID_STATUS = 4;
    dataCFM.APPROVER = $("#hd_nrp").val();

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/QuickActions",
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
        url: $("#web_link").val() + "/api/EmpRecord/QuickActions",
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
        url: $("#web_link").val() + "/api/EmpRecord/QuickActions",
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

function BerhentiBekerja(approvalId) {
    console.log('Istirahat', approvalId);
    debugger

    let dataCFM = new Object();
    dataCFM.APPROVAL_ID = approvalId;
    dataCFM.ID_STATUS = 7;
    dataCFM.APPROVER = $("#hd_nrp").val();

    $.ajax({
        url: $("#web_link").val() + "/api/EmpRecord/QuickActions",
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