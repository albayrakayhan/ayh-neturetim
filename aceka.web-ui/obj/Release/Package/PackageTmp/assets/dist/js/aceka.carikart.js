/*
aceka.genel.js dosyasından gelecek
var apiUrl = 'http://92.45.23.86:9597/';
*/

var carikartlar = {};


carikartlar.search = {
    init: function () {
        $('#searchFrom').submit(function (e) {
            e.preventDefault();
        });
        $("#modelAdi").autocomplete({
            //source: ,
            source: function (request, response) {

                var data = stokkart.autocomplate.stokAdiTurListesi({ stokkart_tur_id: 0, stok_adi: request.term, stok_kodu: '' });
                response($.map(data, function (item) {
                    return { label: item.value, value: item.value, id: item.id };
                }));
            },
            minLength: 2
        });
        $("#stokKodu").autocomplete({
            //source: ,
            source: function (request, response) {

                var data = stokkart.autocomplate.stokAdiTurListesi({ stokkart_tur_id: 0, stok_kodu: request.term });
                response($.map(data, function (item) {
                    return { label: item.code, value: item.code, id: item.id };
                }));
            },
            minLength: 2

        });
        $("#sube_cari_unvan").autocomplete({
            source: carikart.autocomplate.cariListeTuruBayiVeCari(),
            select: function (event, ui) {
                $("#sube_cari_unvan").attr('data-id', ui.item.id);
            }
        });
        $("#finans_sorumlu_cari_unvan").autocomplete({
            source: carikart.autocomplate.cariListeTuruBayiVeCari(),
            select: function (event, ui) {
                $("#finans_sorumlu_cari_unvan").attr('data-id', ui.item.id);
            }
        });
        $("#satis_sorumlu_cari_unvan").autocomplete({
            source: carikart.autocomplate.cariListeTuruBayiVeCari(),
            select: function (event, ui) {
                $("#satis_sorumlu_cari_unvan").attr('data-id', ui.item.id);
            }
        });
        $("#satin_alma_sorumlu_cari_unvan").autocomplete({
            source: carikart.autocomplate.cariListeTuruBayiVeCari(),
            select: function (event, ui) {
                $("#satin_alma_sorumlu_cari_unvan").attr('data-id', ui.item.id);
            }
        });
        $("#ana_cari_unvan").autocomplete({
            source: carikart.autocomplate.cariListeTuruBayiVeCari(),
            select: function (event, ui) {
                $("#ana_cari_unvan").attr('data-id', ui.item.id);
            }
        });

        $('.aramaBtn').on('click', function () {
            var metodUrl = "api/cari/cari-bul";
            var parameter = '?i==1';

            var carikart_id = $("#carikart_id").val();
            var cari_unvan = $("#cari_unvan").val();
            var carikart_tipi_id = $("#carikart_tipi_id").val();
            var ozel_kod = $('#ozel_kod').val();

            //var url = apiUrl + metodUrl + '?cari_unvan=' + cari_unvan + '&carikart_id=' + carikart_id + '&carikart_tipi_id=' + carikart_tipi_id + '&ozel_kod=' + ozel_kod;

            table = null;
            if (carikart_id.length > 0)
                parameter += '&carikart_id=' + encodeURIComponent(carikart_id);
            if (cari_unvan.length > 0)
                parameter += '&cari_unvan=' + encodeURIComponent(cari_unvan);
            if (carikart_tipi_id.length > 0)
                parameter += '&carikart_tipi_id=' + encodeURIComponent(carikart_tipi_id);
            if (ozel_kod.length > 0)
                parameter += '&ozel_kod=' + encodeURIComponent(ozel_kod);
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl + parameter,
                dataType: "json",
                async: false,
                success: function (data) {

                    if (data != null) {

                        table = $('#CarikartTable').DataTable({
                            data: data,
                            paging: true,
                            destroy: true,
                            lengthChange: false,
                            searching: false,
                            ordering: true,
                            info: true,
                            autoWidth: true,
                            columns: [
                                {
                                    data: "carikart_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" data-id="' + row.carikart_id + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "carikart_id" },
                                { data: "cari_unvan" },
                                { data: "parametre_parabirimi.pb_adi" },
                                 { data: "giz_sabit_carikart_tipi.carikart_tipi_adi" },
                                //{ data: "stokkart_turu" },
                                {
                                    data: "statu",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            if (row.statu == false) {
                                                return '<span class="badge bg-red">Pasif</span>';
                                            } else {
                                                return '<span class="badge bg-green">Aktif</span>';
                                            }

                                        }
                                        return data;
                                    }
                                    , className: "dt-body-center"
                                }
                            ],
                            initComplete: function (settings, json) {
                                //checkbox();
                            }
                        });

                        $('.paginate_button').on('click', function () {
                            //checkbox();
                        });
                    } else {
                        genel.modal("Dikkat!", "Kayıt bulunamadı", "uyari", "$('#myModal').modal('hide');");
                    }
                }
            });

            $('#CarikartTable > tbody').on('dblclick', 'tr', function () {
                var data = table.row(this).data();
                location = '/carikart/detail/' + data.carikart_id;

            });

        });
    }

};
carikartlar.get = {
    init: function (id) {
        //$('.talimatBtn').attr("onclick", "modelkart.get.talimatGetPopup(" + id + ")");
        //$('.varyantBtn').attr("onclick", "modelkart.get.varyantGetPopup(" + id + ")");
        //$('.ilkMaddeBtn').attr("onclick", "modelkart.get.ilkMaddeGetPopup(" + id + ")");
        //$('.fileUploadButton').attr("onclick", "modelkart.get.eklerGetPopup(" + id + ")");
        //$('.fileDeleteButton').attr("onclick", "modelkart.delete.ekler(" + id + ")");
        //$('.talimatDeleteBtn').attr("onclick", "modelkart.delete.talimat(" + id + ")");
        //$('.varyantDeleteBtn').attr("onclick", "modelkart.delete.varyant(" + id + ")");
    },
    genelUst: function (id, secilecekParametreControlIds) {
        if (id > 0) {
            //api/cari/{carikart_id}
            var metodUrl = "api/cari/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: 'Json',
                async: false,
                success: function (data) {
                    //inputlar dolduruluyor.
                    $('#carikart_id').val(data.carikart_id);
                    $('#CarikartTable > #hfcarikart_tipi_id').val(data.carikart_tipi_id);
                    $('#carikart_tipi_id').val(data.carikart_tipi_id);
                    $('#cari_unvan').val(data.cari_unvan);
                    $('#ozel_kod').val(data.ozel_kod);
                    //$('#ana_carikart_id').val(data.ana_carikart_id);
                    $('#ana_cari_unvan').val(data.ana_cari_unvan).attr('data-id', data.ana_carikart_id);
                    //$('#sube_carikart_id').val(data.sube_carikart_id);
                    $('#sube_cari_unvan').val(data.sube_cari_unvan).attr('data-id', data.sube_carikart_id);
                    //$('#satin_alma_sorumlu_carikart_id').val(data.satin_alma_sorumlu_carikart_id);
                    // $('#satis_sorumlu_carikart_id').val(data.satis_sorumlu_carikart_id);
                    $('#satin_alma_sorumlu_cari_unvan').val(data.satin_alma_sorumlu_cari_unvan).attr('data-id', satin_alma_sorumlu_carikart_id);
                    $('#satis_sorumlu_cari_unvan').val(data.satis_sorumlu_cari_unvan).attr('data-id', data.satis_sorumlu_carikart_id);
                    $('#finans_sorumlu_cari_unvan').val(data.finans_sorumlu_cari_unvan).attr('data-id', data.finans_sorumlu_carikart_id);

                    //$('#' + secilecekParametreControlIds.carikart_tipi_id).val(data.carikart_tipi_id);

                }
            });
        }
    },
    subeListesi: function (id) {
        if (id > 0) {
            var metodUrl = "api/cari/cari-sube-listesi/" + id;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    if (data != null) {
                        carisubeTable = $('#subeList').DataTable({
                            data: data,
                            paging: true,
                            destroy: true,
                            lengthChange: false,
                            searching: false,
                            ordering: true,
                            info: true,
                            autoWidth: true,
                            columns: [
                                {
                                    data: "carikart_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" name="carikart_id" data-carikart_id="' + row.carikart_id + '" value="' + row.carikart_id + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "carikart_id" },
                                { data: "cari_unvan" },
                                { data: "carikart_turu_adi" },
                                { data: "carikart_tipi_adi" }
                            ]

                        });
                        $('.paginate_button').on('click', function () {
                            //checkbox();
                        });

                        $('#subeList > tbody').on('dblclick', 'tr', function () {
                            var data = carisubeTable.row(this).data();

                            var carikart_id = data.carikart_id;
                            genel.talimatGet({ carikart_id: id, event: "modelkart.put.talimat();", data: data });
                        });

                    } else {
                        //if (varyantlarTable != null) {
                        //    varyantlarTable.destroy();
                        //    $('#VaryantSkuKart > tbody').html('');
                        //}
                    }
                }
            });
        }

    },
    //E-Posta GRupları Star
    epostaBabsformListe: function (carikart_id) {
        var metodurl = 'api/cari/eposta-gruplari/' + carikart_id;
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    jsonData = data.babs_formu_eposta;
                }
            }
        });
        return jsonData;
    },
    cariMutabakatform: function (carikart_id) {
        var metodurl = 'api/cari/eposta-gruplari/' + carikart_id;
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    jsonData = data.cari_mutabakat_formu_eposta;
                }
            }
        });
        return jsonData;
    },
    irsaliyeEposta: function (carikart_id) {
        var metodurl = 'api/cari/eposta-gruplari/' + carikart_id;
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    jsonData = data.irsaliye_eposta;
                }
            }
        });
        return jsonData;
    },
    odemeHatırlatmaEposta: function (carikart_id) {
        var metodurl = 'api/cari/eposta-gruplari/' + carikart_id;
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    jsonData = data.odeme_hatirlatma_eposta;
                }
            }
        });
        return jsonData;
    },
    perakendeFaturaEposta: function (carikart_id) {
        var metodurl = 'api/cari/eposta-gruplari/' + carikart_id;
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    jsonData = data.perakende_fatura_eposta;
                }
            }
        });
        return jsonData;
    },
    siparisFormuEposta: function (carikart_id) {
        var metodurl = 'api/cari/eposta-gruplari/' + carikart_id;
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    jsonData = data.siparis_formu_eposta;
                }
            }
        });
        return jsonData;
    },
    toptanFaturaEposta: function (carikart_id) {
        var metodurl = 'api/cari/eposta-gruplari/' + carikart_id;
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    jsonData = data.toptan_fatura_eposta;
                }
            }
        });
        return jsonData;
    },
    //E-Posta GRupları End

    //Müşteri Listesi Star
    musteriListesi: function (controlId) {
        //var id = $(this).attr('id');
        var metodurl = 'api/cari/musteri-listesi';
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {

                    $('#' + controlId + ' option').remove();
                    $('#' + controlId).append('<option value="">Müşteri Seçiniz</option>');

                    $.each(data, function (key, obj) {

                        $('#' + controlId).append('<option value="' + obj.carikart_id + '">' + obj.cari_unvan + '</option>');
                    });
                }
                else {
                    $('#' + controlId + ' option').remove();
                    $('#' + controlId).append('<option value="">Müşteri Seçiniz</option>');
                }
            }
        });
        return jsonData;
    },
    //Müşteri Listesi End

    //Firma Bayi Star
    firmaBayiListesi: function (controlId) {
        //var id = $(this).attr('id');
        var metodurl = 'api/cari/firma-bayi-listesi';
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {

                    $('#' + controlId + ' option').remove();
                    $('#' + controlId).append('<option value="">Firma/Bayi Seçiniz</option>');

                    $.each(data, function (key, obj) {

                        $('#' + controlId).append('<option value="' + obj.firmacarikart_id + '">' + obj.cari_unvan + '</option>');
                    });
                }
                else {
                    $('#' + controlId + ' option').remove();
                    $('#' + controlId).append('<option value="">Firma/Bayi Seçiniz</option>');
                }
            }
        });
        return jsonData;
    }
    //Firma Bayi Listesi End





};
