var ajaxHelper = function () {
    function get(url, successCallback, failureCallBack) {
        $.ajax({
            url: url,
            dataType: "json",
            type: "GET",
            contentType: 'application/json; charset=utf-8',
            async: true,
            processData: false,
            cache: false,
            success: successCallback,
            error: failureCallBack
        });
    }

    function post(url, postData, successCallback, failureCallBack) {
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(postData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: successCallback,
            failure: failureCallBack
        });
    }
    function postPdf(url, postData, successCallback, failureCallBack) {
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(postData),
            contentType: "application/pdf",
            success: successCallback,
            failure: failureCallBack
        });
    }
    return {
        get: get,
        post: post,
        postPdf: postPdf
    }
}();

function creazaReteta(nume_medic, nume_pacient, prenume_pacient, diagnostic, medicament_1, cant_1, medicament_2, cant_2, medicament_3, cant_3, medicament_4, cant_4, medicament_5, cant_5, tip_reteta) {
    var postData = new Object();
    postData.Nume_medic = nume_medic;
    postData.Nume_pacient = nume_pacient;
    postData.Prenume_pacient = prenume_pacient;
    postData.Diagnostic = diagnostic;
    postData.Medicament1 = medicament_1;
    postData.Cant1 = cant_1;
    postData.Medicament2 = medicament_2;
    postData.Cant2 = cant_2;
    postData.Medicament3 = medicament_3;
    postData.Cant3 = cant_3;
    postData.Medicament4 = medicament_4;
    postData.Cant4 = cant_4;
    postData.Medicament5 = medicament_5;
    postData.Cant5 = cant_5;
    postData.TipReteta = tip_reteta;

    ajaxHelper.post("/Home/CautaMedicamente",
        postData,
        function (data) {
            $("#listFarmacii").css('display', 'block')
            $(".farmacie").remove();
            data.forEach(function (entry) {
                $("#listFarmacii").append('<a href="#" class="list-group-item list-group-item-action farmacie" style="text-align: left">' + entry.Nume_farmacie + '<span class="badge badge-primary badge-pill">' + entry.Distanta + 'km</span></a>')
            });
        },
        function () { console.log("Ceva nu a mers") })
}
function salvareConsultatie(nume_medic, nume_pacient, prenume_pacient, diagnostic) {
    var postData = new Object();
    postData.Nume_medic = nume_medic;
    postData.Nume_pacient = nume_pacient;
    postData.Prenume_pacient = prenume_pacient;
    postData.Diagnostic = diagnostic;

    ajaxHelper.post("/Home/SalvareConsultatie",
        postData,
        function (data) {
            $('#consultationModal').hide();
            $('.modal-backdrop').hide();
        },
        function () { console.log("Ceva nu a mers") })
}
function generareRaportReteta() {
    $('#myModal').hide();
    $('.modal-backdrop').hide();
    $("#actiuni").append("<a href=/Home/GenereazaReteta?numeMedic=" + $('#numeMedic').val() + "&numePacient=" + $('#numePacient').val() + "&prenumePacient=" + $('#prenumePacient').val() + "&diagnostic=" + $('#diagnostic').val() + "&medicament1=" + $('#medicament1').val() + "&cant1=" + $('#cant1').val() + "&medicament2=" + $('#medicament2').val() + "&cant2=" + $('#cant2').val() + "&medicament3=" + $('#medicament3').val() + "&cant3=" + $('#cant3').val() + "&medicament4=" + $('#medicament4').val() + "&cant4=" + $('#cant4').val() + "&medicament5" + $('#medicament5').val() + "&cant5=" + $('#cant5').val() + "&tipReteta=" + $('#tipReteta').val() + " style='display:none' id='buton-generat'></a>")
    window.location = $("#buton-generat").attr("href")
    $("#buton-generat").remove()
}