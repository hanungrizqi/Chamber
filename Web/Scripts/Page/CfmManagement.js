Codebase.helpersOnLoad(['cb-table-tools-checkable', 'cb-table-tools-sections', 'js-flatpickr', 'jq-datepicker', 'jq-colorpicker', 'jq-maxlength', 'jq-select2', 'jq-rangeslider', 'jq-masked-inputs', 'jq-pw-strength']);

$("document").ready(function () {
    $("#example-flatpickr-range").flatpickr({
        mode: "range",
        onChange: function (selectedDates, dateStr, instance) {
            if (selectedDates.length === 2) {
                debugger
                var startDate = selectedDates[0];
                var endDate = selectedDates[1];

                var startDateLocal = startDate.toLocaleDateString('en-CA');
                var endDateLocal = endDate.toLocaleDateString('en-CA');

                table.ajax.url($("#web_link").val() + "/api/CfmManagement/Get_ListChambers_Daterange/" + startDateLocal + "/" + endDateLocal).load();
            }
        },
    });
})

$(document).on('click', '.action-link', function (e) {
    e.preventDefault();
    var approvalId = $(this).data('approvalid');
    var action = $(this).data('action');

    if (action === 'Edit') {
        // Mencari baris yang sesuai dengan approvalId
        var row = table.row($(this).closest('tr')).data();
        var chamber = row.ID_CHAMBER;
        var loc = row.LOKASI;

        ShowEditModal(approvalId, chamber, loc);
    }
});

var table = $("#tbl_cfmmngmnt").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/CfmManagement/Get_ListChambers",
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-nowrap", "targets": '_all' }
    ],
    scrollX: true,
    "searching": true,
    columns: [
        {
            "data": null,
            render: function (data, type, row, meta) {
                return '<input type="checkbox" class="row-checkbox" data-id="' + row.ID_CHAMBER + '">';
            }
        },
        { data: 'ID_CHAMBER' },
        { data: 'USDTDY' },
        { data: 'LOKASI' },
        {
            data: 'LSTUSD',
            render: function (data, type, row) {
                if (data !== null && data !== "") {
                    const tanggal = moment(data).format("DD/MM/YYYY");
                    return tanggal;
                } else {
                    return "";
                }
            }
        },
        {
            data: 'ID',
            targets: 'no-sort',
            orderable: false,
            render: function (data, type, row) {
                var actions = '<div class="btn-group">';
                actions += '<button class="btn btn-sm" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-ellipsis-vertical"></i></button>';
                actions += '<ul class="dropdown-menu">';
                actions += '<li><a class="dropdown-item action-link" data-approvalid="' + data + '" data-chamber="' + row.ID_CHAMBER + '" data-loc="' + row.LOKASI + '" data-action="Edit" href="#">Edit</a></li>';
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
            .columns(1)
            .every(function () {
                var column = this;
                var select = $('<select class="form-control form-control-sm" style="width:200px; display:inline-block; margin-left: 10px;"><option value="">-- CHAMBER --</option></select>')
                    .appendTo($("#tbl_cfmmngmnt_filter.dataTables_filter"))
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

$("#downloadButton").on("click", function () {
    debugger
    generatePDF();
});

$("#downloadButton2").on("click", function () {
    debugger
    generatePDF();
});

function generatePDF() {
    debugger
    var doc = new jsPDF();
    doc.autoTable({ html: '#tbl_cfmmngmnt' });
    doc.save('CfmManagement.pdf');
}

table.on('draw', function () {
    var visibleCheckboxes = document.querySelectorAll('#tbl_cfmmngmnt tbody .row-checkbox:checked');

    visibleCheckboxes.forEach(function (checkbox) {
        checkbox.checked = false;
    });
});

function Create() {
    let obj = new Object();
    obj.ID_CHAMBER = $('#txt_chamber').val();
    obj.LOKASI = $('#txt_loc').val();

    $.ajax({
        url: $("#web_link").val() + "/api/CfmManagement/Create_Chamber", //URI
        data: JSON.stringify(obj),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire(
                    'Saved!',
                    'Data has been Saved.',
                    'success'
                );
                $('#modal-insert').modal('hide');
                $('.select2-modal').val('').trigger('change');
                $('.form-control').val('');

                table.ajax.reload();
            } if (data.Remarks == false) {
                Swal.fire(
                    'Error!',
                    'Message : ' + data.Message,
                    'error'
                );
            }

        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    })
}

function ShowEditModal(approvalId, chamber, loc) {
    $('#txt_id').val(approvalId);
    $('#txt_chamber_update').val(chamber);
    $('#txt_loc_update').val(loc);

    $('#modal_update').modal('show');
}

function Update() {
    let id = $('#txt_id').val();
    let chamber = $('#txt_chamber_update').val();
    let loc = $('#txt_loc_update').val();

    let obj = {
        ID: id,
        ID_CHAMBER: chamber,
        LOKASI: loc
    };
    debugger

    $.ajax({
        url: $("#web_link").val() + "/api/CfmManagement/Update_Chamber", //URI
        data: JSON.stringify(obj),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire(
                    'Saved!',
                    'Data has been Saved.',
                    'success'
                );
                $('#modal_update').modal('hide');
                table.ajax.reload();
            } if (data.Remarks == false) {
                Swal.fire(
                    'Error!',
                    'Message : ' + data.Message,
                    'error'
                );
            }

        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    })
}