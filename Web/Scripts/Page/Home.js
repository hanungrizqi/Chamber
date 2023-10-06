Codebase.helpersOnLoad(['jq-sparkline', 'cb-table-tools-checkable', 'cb-table-tools-sections', 'js-flatpickr', 'jq-datepicker', 'jq-colorpicker', 'jq-maxlength', 'jq-select2', 'jq-rangeslider', 'jq-masked-inputs', 'jq-pw-strength']);

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
                //debugger

                updateHTMLElementsKaryawanMasuk(startDateLocal, endDateLocal);
                updateHTMLElementsUpdateTerakhirKaryawanMasuk(startDateLocal, endDateLocal);

                updateHTMLElementsKaryawanFit(startDateLocal, endDateLocal);
                updateHTMLElementsUpdateTerakhirKaryawanFit(startDateLocal, endDateLocal);

                updateHTMLElementsKaryawanUnfit(startDateLocal, endDateLocal);
                updateHTMLElementsUpdateTerakhirKaryawanUnfit(startDateLocal, endDateLocal);

                updateHTMLElementssudahApproved(startDateLocal, endDateLocal);
                updateHTMLElementssudahApprovedDate(startDateLocal, endDateLocal);

                updateHTMLElementsbutuhApproval(startDateLocal, endDateLocal);
                updateHTMLElementsbutuhApprovalDate(startDateLocal, endDateLocal);

                updateHTMLElementsretest(startDateLocal, endDateLocal);
                updateHTMLElementsretestDate(startDateLocal, endDateLocal);

                updateHTMLElementstdkdptbekerja(startDateLocal, endDateLocal);
                updateHTMLElementstdkdptbekerjaDate(startDateLocal, endDateLocal);

                updateHTMLElementscham001(startDateLocal, endDateLocal);
                updateHTMLElementscham001Date(startDateLocal, endDateLocal);

                updateHTMLElementscham002(startDateLocal, endDateLocal);
                updateHTMLElementscham002Date(startDateLocal, endDateLocal);

                updateHTMLElementscham003(startDateLocal, endDateLocal);
                updateHTMLElementscham003Date(startDateLocal, endDateLocal);

                updateHTMLElementscham004(startDateLocal, endDateLocal);
                updateHTMLElementscham004Date(startDateLocal, endDateLocal);

                // Panggil fungsi untuk mengupdate data grafik
                updateChartWithDateRange(startDateLocal, endDateLocal);
            }
        },
    });
    
    fetchData();
})

function destroyChartIfExists() {
    var existingChart = Chart.getChart("chartshome");
    if (existingChart) {
        existingChart.destroy();
    }
}

function initChart() {
    destroyChartIfExists();
    const data = {
        labels: [
            'Sudah Approved',
            'Retest',
            'Butuh Approval',
            'Tidak Dapat Bekerja'
        ],
        datasets: [{
            label: 'My First Dataset',
            data: [sudahApprovedValue, retestValue, butuhApprovalValue, tdkdptbekerjaValue],
            backgroundColor: [
                'rgb(255, 99, 132)',
                'rgb(75, 192, 192)',
                'rgb(255, 205, 86)',
                'rgb(54, 162, 235)'
            ],
        }]
    };

    const config = {
        type: 'polarArea',
        data: data,
        options: {
            scale: {
                pointLabels: {
                    fontSize: 14,
                    fontStyle: 'bold',
                }
            },
            plugins: {
                legend: {
                    display: true,
                    position: 'right',
                }
            },
        }
    };
    const myChart = new Chart(document.getElementById('chartshome'), config);
}

var sudahApprovedValue, butuhApprovalValue, retestValue, tdkdptbekerjaValue;

function fetchData() {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/sudahapproved/" + $("#hd_positid").val(),
        type: "GET",
        cache: false,
        success: function (result) {
            sudahApprovedValue = result.Total;
            checkDataAndInitChart();
        }
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/butuhapproval/" + $("#hd_positid").val(),
        type: "GET",
        cache: false,
        success: function (result) {
            butuhApprovalValue = result.Total;
            checkDataAndInitChart();
        }
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/retest/" + $("#hd_positid").val(),
        type: "GET",
        cache: false,
        success: function (result) {
            retestValue = result.Total;
            checkDataAndInitChart();
        }
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/tdkdptbekerja/" + $("#hd_positid").val(),
        type: "GET",
        cache: false,
        success: function (result) {
            tdkdptbekerjaValue = result.Total;
            checkDataAndInitChart();
        }
    });
}

function checkDataAndInitChart() {
    if (sudahApprovedValue !== undefined &&
        butuhApprovalValue !== undefined &&
        retestValue !== undefined &&
        tdkdptbekerjaValue !== undefined) {

        initChart();
    }
}

function updateChartWithDateRange(startDate, endDate) {
    //debugger
    initChart();
}


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

function updateHTMLElementsKaryawanUnfit(startDate, endDate) {
    //debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/TotalKaryawanUnfit_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
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

function updateHTMLElementsUpdateTerakhirKaryawanUnfit(startDate, endDate) {
    //debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/UpdateterakhirKaryawanUnfit_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
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

var isUpdatingChart = false;
function updateHTMLElementssudahApproved(startDate, endDate) {
    //debugger
    if (isUpdatingChart) {
        return;
    }

    // Set variabel penanda sebagai true untuk menandakan bahwa grafik sedang dalam proses pembaruan
    isUpdatingChart = true;
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/sudahapproved_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            $("#sudahapproved").text(result.Total);
            // Set ulang sudahApprovedValue ke result.Total
            sudahApprovedValue = result.Total;

            // Setelah mengupdate data, panggil fungsi untuk mengupdate grafik
            updateChartWithDateRange(startDate, endDate);

            // Set variabel penanda kembali ke false setelah pembaruan grafik selesai
            isUpdatingChart = false;
        },
        error: function () {
            // Jika ada kesalahan dalam AJAX, pastikan variabel penanda juga diatur kembali ke false
            isUpdatingChart = false;
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

function updateHTMLElementssudahApprovedDate(startDate, endDate) {
    //debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/sudahApprovedDate_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
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

var isUpdatingChart2 = false;
function updateHTMLElementsbutuhApproval(startDate, endDate) {
    //debugger
    if (isUpdatingChart2) {
        return;
    }
    isUpdatingChart2 = true;

    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/butuhApproval_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            $("#butuhapproval").text(result.Total);

            butuhApprovalValue = result.Total;
            updateChartWithDateRange(startDate, endDate);
            isUpdatingChart2 = false;
        }
    });
}

function updateHTMLElementsbutuhApprovalDate(startDate, endDate) {
    //debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/butuhApprovalDate_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
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

var isUpdatingChart3 = false;
function updateHTMLElementsretest(startDate, endDate) {
    if (isUpdatingChart3) {
        return;
    }
    isUpdatingChart3 = true;

    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/retest_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            $("#retest").text(result.Total);

            retestValue = result.Total;
            updateChartWithDateRange(startDate, endDate);
            isUpdatingChart3 = false;
        }
    });
}

function updateHTMLElementsretestDate(startDate, endDate) {
    //debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/retestDate_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
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

var isUpdatingChart4 = false;
function updateHTMLElementstdkdptbekerja(startDate, endDate) {
    if (isUpdatingChart4) {
        return;
    }
    isUpdatingChart4 = true;

    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/tdkdptbekerja_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            $("#tdkdptbekerja").text(result.Total);

            tdkdptbekerjaValue = result.Total;
            updateChartWithDateRange(startDate, endDate);
            isUpdatingChart4 = false;
        }
    });
}

function updateHTMLElementstdkdptbekerjaDate(startDate, endDate) {
    //debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/tdkdptbekerjaDate_Datepicker/" + $("#hd_positid").val() + "/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
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

function updateHTMLElementscham001(startDate, endDate) {
    //debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham001_Datepicker/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            debugger
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

function updateHTMLElementscham001Date(startDate, endDate) {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham001Date_Datepicker/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
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

function updateHTMLElementscham002(startDate, endDate) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham002_Datepicker/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#cham002").text(result.USDTDY);
        }
    });
}

function updateHTMLElementscham002Date(startDate, endDate) {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham002Date_Datepicker/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
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

function updateHTMLElementscham003(startDate, endDate) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham003_Datepicker/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#cham003").text(result.USDTDY);
        }
    });
}

function updateHTMLElementscham003Date(startDate, endDate) {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham003Date_Datepicker/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
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

function updateHTMLElementscham004(startDate, endDate) {
    debugger
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham004_Datepicker/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            //debugger
            $("#cham004").text(result.USDTDY);
        }
    });
}

function updateHTMLElementscham004Date(startDate, endDate) {
    $.ajax({
        url: $("#web_link").val() + "/api/Dashboard/cham004Date_Datepicker/" + startDate + "/" + endDate,
        type: "GET",
        cache: false,
        success: function (result) {
            $("#cham004Date").text(result.Tanggal);
        }
    });
}