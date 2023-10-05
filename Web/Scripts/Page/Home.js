Codebase.helpersOnLoad(['cb-table-tools-checkable', 'cb-table-tools-sections', 'js-flatpickr', 'jq-datepicker', 'jq-colorpicker', 'jq-maxlength', 'jq-select2', 'jq-rangeslider', 'jq-masked-inputs', 'jq-pw-strength']);

$("document").ready(function () {
    Karyawanmasuk();
    UpdateterakhirKaryawanmasuk()
    KaryawanFit()
    UpdateterakhirKaryawaFit()
    KaryawanUnfit()
    UpdateterakhirKaryawaUnfit()
    sudahApproved()
    sudahApprovedDate()
    butuhApproval()
    butuhApprovalDate()
    retest()
    retestDate()
    tdkdptbekerja()
    tdkdptbekerjaDate()
    cham001()
    cham001Date()
    cham002()
    cham002Date()
    cham003()
    cham003Date()
    cham004()
    cham004Date()
    $("#example-flatpickr-range").flatpickr({
        mode: "range",
        onChange: function (selectedDates, dateStr, instance) {
            if (selectedDates.length === 2) {
                var startDate = selectedDates[0];
                var endDate = selectedDates[1];

                var startDateLocal = startDate.toLocaleDateString('en-CA');
                var endDateLocal = endDate.toLocaleDateString('en-CA');
                debugger

                updateHTMLElementsKaryawanMasuk(startDateLocal, endDateLocal);
                updateHTMLElementsUpdateTerakhirKaryawanMasuk(startDateLocal, endDateLocal);

                updateHTMLElementsKaryawanFit(startDateLocal, endDateLocal);
                updateHTMLElementsUpdateTerakhirKaryawanFit(startDateLocal, endDateLocal);
            }
        },
    });
})

//karyawan masuk
function Karyawanmasuk() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/TotalKaryawanMasuk/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#totalKaryawanMasuk").text(result.Total);
        }
    });
}

function updateHTMLElementsKaryawanMasuk(startDate, endDate) {
    //debugger
    // Panggil AJAX atau perhitungan lainnya sesuai dengan rentang tanggal yang dipilih
    // Kemudian, perbarui elemen HTML dengan hasil yang diperoleh

    // Contoh:
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/TotalKaryawanMasuk_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            // Perbarui elemen HTML dengan hasil yang diperoleh
            $("#totalKaryawanMasuk").text(result.Total);
        }
    });

    // Anda dapat melakukan pembaruan serupa untuk elemen lainnya
}

function UpdateterakhirKaryawanmasuk() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/UpdateterakhirKaryawanMasuk/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#updateTerakhirKaryawanMasuk").text(result.Tanggal);
        }
    });
}

function updateHTMLElementsUpdateTerakhirKaryawanMasuk(startDate, endDate) {
    //debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/UpdateterakhirKaryawanMasuk_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            $("#updateTerakhirKaryawanMasuk").text(result.Tanggal);
        }
    });
}


//karyawan fit
function KaryawanFit() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/TotalKaryawanFit/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#totalKaryawanFit").text(result.Total);
        }
    });
}

function updateHTMLElementsKaryawanFit(startDate, endDate) {
    //debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/TotalKaryawanFit_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            $("#totalKaryawanFit").text(result.Total);
        }
    });
}

function UpdateterakhirKaryawaFit() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/UpdateterakhirKaryawanFit/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#updateTerakhirKaryawanFit").text(result.Tanggal);
        }
    });
}

function updateHTMLElementsUpdateTerakhirKaryawanFit(startDate, endDate) {
    //debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/UpdateterakhirKaryawanFit_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            $("#updateTerakhirKaryawanFit").text(result.Tanggal);
        }
    });
}

//karyawan unfit
function KaryawanUnfit() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/TotalKaryawanUnfit/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#totalKaryawanUnfit").text(result.Total);
        }
    });
}

function UpdateterakhirKaryawaUnfit() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/UpdateterakhirKaryawanUnfit/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#updateTerakhirKaryawanUnfit").text(result.Tanggal);
        }
    });
}

//sudah approve
function sudahApproved() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/sudahapproved/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#sudahapproved").text(result.Total);
        }
    });
}

function sudahApprovedDate() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/sudahapprovedDate/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#sudahapprovedDate").text(result.Tanggal);
        }
    });
}

//butuh approve
function butuhApproval() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/butuhapproval/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#butuhapproval").text(result.Total);
        }
    });
}

function butuhApprovalDate() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/butuhapprovalDate/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#butuhapprovalDate").text(result.Tanggal);
        }
    });
}

//retest
function retest() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/retest/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#retest").text(result.Total);
        }
    });
}

function retestDate() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/retestDate/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#retestDate").text(result.Tanggal);
        }
    });
}

//tidak dapat bekerja
function tdkdptbekerja() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/tdkdptbekerja/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#tdkdptbekerja").text(result.Total);
        }
    });
}

function tdkdptbekerjaDate() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/tdkdptbekerjaDate/" + $("#hd_positid").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#tdkdptbekerjaDate").text(result.Tanggal);
        }
    });
}

//cham001
function cham001() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham001", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#cham001").text(result.USDTDY);
        }
    });
}

function cham001Date() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham001Date", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#cham001Date").text(result.Tanggal);
        }
    });
}

//cham002
function cham002() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham002", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#cham002").text(result.USDTDY);
        }
    });
}

function cham002Date() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham002Date", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#cham002Date").text(result.Tanggal);
        }
    });
}

//cham003
function cham003() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham003", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#cham003").text(result.USDTDY);
        }
    });
}

function cham003Date() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham003Date", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#cham003Date").text(result.Tanggal);
        }
    });
}

//cham004
function cham004() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham004", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#cham004").text(result.USDTDY);
        }
    });
}

function cham004Date() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham004Date", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#cham004Date").text(result.Tanggal);
        }
    });
}