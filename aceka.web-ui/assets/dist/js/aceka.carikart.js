/*
aceka.genel.js dosyasından gelecek
var apiUrl = 'http://92.45.23.86:9597/';
*/

var carikartlar = {};
var bankaCheckedValues = [];
var finansIletisiCheckedValues = [];
var finansNotlarCheckedValues = [];
var finansSubeCheckedValues = [];
var kosulListData = [];
var kosulListDistinctData = [];
var paramListData = [];
var operatorListData = [];
var cevapListeSqlData = [];
var myWindow = $("#aksesuarPopoup");

carikartlar.search = {
    init: function () {
        $('#searchFrom').submit(function (e) {
            e.preventDefault();
        });
        //Carikart Arama ekranındaki cari_unvan
        $("#cari_unvan").autocomplete({
            source: carikart.autocomplate.cariListeTuruBayiVeCari(),
            select: function (event, ui) {
                $("#cari_unvan").attr('data-id', ui.item.id);
            },
            minLength: 2
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
        $("#ana_carikart_id").autocomplete({
            source: carikart.autocomplate.cariListeTuruBayiVeCari(),
            select: function (event, ui) {
                $("#ana_carikart_id").attr('data-ana_carikart_id', ui.item.id);
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

        $("#vergi_dairesi").autocomplete({
            source: carikart.autocomplate.vergiDaireleri(),
            select: function (event, ui) {
                $("#vergi_dairesi").attr('data-id', ui.item.id);
            },
            minLength: 2
        });

        $('.aramaBtn').on('click', function () {
            var metodUrl = "api/cari/cari-bul-ozet";
            var parameter = '?i==1';

            var carikart_id = $("#carikart_id").val();
            var cari_unvan = $("#cari_unvan").val();
            var carikart_tipi_id = $("#carikart_tipi_id").val();
            var ozel_kod = $('#ozel_kod').val();

            //var url = apiUrl + metodUrl + '?cari_unvan=' + cari_unvan + '&carikart_id=' + carikart_id + '&carikart_tipi_id=' + carikart_tipi_id + '&ozel_kod=' + ozel_kod;

            table = null;
            if (carikart_id != undefined && carikart_id.length > 0)
                parameter += '&carikart_id=' + encodeURIComponent(carikart_id);
            if (cari_unvan != undefined && cari_unvan.length > 0)
                parameter += '&cari_unvan=' + encodeURIComponent(cari_unvan);
            if (carikart_tipi_id != undefined && carikart_tipi_id.length > 0)
                parameter += '&carikart_tipi_id=' + encodeURIComponent(carikart_tipi_id);
            if (ozel_kod != undefined && ozel_kod.length > 0)
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
                                            return '<input type="checkbox" name="caritur" data-id="' + row.carikart_id + '" class="iCheck-helper ">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "carikart_id" },
                                { data: "cari_unvan" },
                                { data: "para_birimi" },
                                { data: "cari_tipi_adi" },
                                //{ data: "parametre_parabirimi.pb_kodu" },
                                //{ data: "giz_sabit_carikart_tipi.carikart_tipi_adi" },
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
                            ]
                            ,
                            initComplete: function (settings, json) {
                                //checkbox();
                                genel.iCheck('input[name="caritur"]');
                            }, drawCallback: function () {
                                $('.paginate_button')
                                    .on('click', function () {
                                        genel.iCheck('input[name="caritur"]');
                                        var api = this.api();
                                    });
                            }
                        });
                        $('.paginate_button').on('click', function () {
                            //checkbox();
                            genel.iCheck('input[name="caritur"]');
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
        $('.notlarBtn').attr("onclick", "carikartlar.get.finansNotlarPopup(" + id + ")");
        $('.notlarDeleteBtn').attr("onclick", "carikartlar.delete.finansNotlar(" + id + ")");
        $('.finansiletisimDeleteBtn').attr("onclick", "carikartlar.delete.finansIletisim(" + id + ")");
        $('.finansiletisimBtn').attr("onclick", "carikartlar.get.finansIletisimPopup(" + id + ")");
        $('.bankahesapBtn').attr("onclick", "carikartlar.get.bankaHesapPopup(" + id + ")");
        $('.bankahesapDeleteBtn').attr("onclick", "carikartlar.delete.bankaHesap(" + id + ")");
        $('.subeBtn').attr("onclick", "carikartlar.get.finansSubePopup(" + id + ")");
        $('.subeDeleteBtn').attr("onclick", "carikartlar.delete.subeListesi(" + id + ")");

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
                    $('#carikart_id').val(data.carikart_id);
                    $('#statu').val(data.statu);
                    $('#CarikartTable > #hfcarikart_id').val(data.carikart_id);
                    $('#carikart_tipi_id').val(data.carikart_tipi_id);
                    $('#cari_unvan').val(data.cari_unvan);
                    $('#ozel_kod').val(data.ozel_kod);
                    $('#ana_cari_unvan').val(data.ana_cari_unvan).attr('data-id', data.ana_carikart_id); //ana_cari_unvan 'ın attributune ana_carikart_id nin değerini atadık.
                    $('#sube_cari_unvan').val(data.sube_cari_unvan).attr('data-id', data.sube_carikart_id);
                    $('#' + secilecekParametreControlIds.statu).val(data.statu.toString());

                    //$('#satin_alma_sorumlu_cari_unvan').val(data.satin_alma_sorumlu_cari_unvan).attr('data-id', data.satin_alma_sorumlu_carikart_id);
                    //$('#satis_sorumlu_cari_unvan').val(data.satis_sorumlu_cari_unvan).attr('data-id', data.satis_sorumlu_carikart_id);
                    $('#finans_sorumlu_cari_unvan').val(data.finans_sorumlu_cari_unvan).attr('data-id', data.finans_sorumlu_carikart_id);
                    //$('#' + secilecekParametreControlIds.faUlkeler).val(data.ulke_id);
                    //$('#' + secilecekParametreControlIds.ilce_id).val(data.ilce_id);
                    //$('#' + secilecekParametreControlIds.selectUlkeler).change();
                    //$('#' + secilecekParametreControlIds.selectSehirler).val(data.sehir_id);
                    //$('#' + secilecekParametreControlIds.selectSehirler).change();
                    //$('#' + secilecekParametreControlIds.selectIlceler).val(data.ilce_id);
                    //$('#' + secilecekParametreControlIds.selectIlceler).change();
                    //$('#' + secilecekParametreControlIds.selectSemtler).val(data.semt_id);
                    //console.log(data.ilce_id);
                }
            });
        }
    },
    subeListesi: function (id) {
        if (id > 0) {
            //api/cari/cari-sube-listesi/{carikart_id}
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
                                            //return '<input type="checkbox" name="anacarikartidCheckBox" data-ana_carikart_id="' + row.ana_carikart_id + '" value="' + row.ana_carikart_id + '" data-carikart_id="' + row.carikart_id + '" class="iCheck-helper">';
                                            return '<input type="checkbox" name="anacarikartidCheckBox" data-ana_carikart_id="' + row.carikart_id + '" value="' + row.carikart_id + '" data-carikart_id="' + row.ana_carikart_id + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "carikart_id" },
                                { data: "cari_unvan" },
                                { data: "carikart_turu_adi" },
                                { data: "carikart_tipi_adi" }
                            ]
                            ,
                            initComplete: function (settings, json) {
                                genel.iCheck('input[name="anacarikartidCheckBox"]');
                            }, drawCallback: function () {
                                $('.paginate_button')
                                    .on('click', function () {
                                        genel.iCheck('input[name="anacarikartidCheckBox"]');
                                        var api = this.api();
                                    });
                            }
                        });
                        $('.paginate_button').on('click', function () {
                            //checkbox();
                        });
                        $('input[name="anacarikartidCheckBox"]').on('ifChecked', function (event) {
                            var ana_carikart_id = event.target.attributes["data-ana_carikart_id"].nodeValue;
                            var carikart_id = event.target.attributes["data-carikart_id"].nodeValue;
                            var kayit_silindi = 'true';
                            finansSubeCheckedValues.push({ carikart_id: carikart_id, kayit_silindi: kayit_silindi, ana_carikart_id: ana_carikart_id });
                        });
                        $('input[name="anacarikartidCheckBox"]').on('ifUnchecked', function (event) {
                            var carikart_id = event.target.attributes["data-carikart_id"].nodeValue;
                            var ana_carikart_id = event.target.attributes["data-ana_carikart_id"].nodeValue;
                            var tmpArray = [];
                            for (var item, i = 0; item = finansSubeCheckedValues[i++];) {
                                if (item.carikart_id != carikart_id) {
                                    tmpArray.push(item);
                                }
                            }
                            finansSubeCheckedValues = tmpArray;
                        });
                        //Pop up açma.
                        //$('#carisubeTable > tbody').on('dblclick', 'tr', function () {
                        //    var data = carisubeTable.row(this).data();
                        //    var carikart_id = data.carikart_id;
                        //    genel.talimatGet({ carikart_id: id, event: "modelkart.put.talimat();", data: data });
                        //});

                    } else {
                        //if (carisubeTable != null) {
                        //    carisubeTable.destroy();
                        //    $('#carisubeTable > tbody').html('');
                        //}
                    }
                }
            });
        }
    },
    finansBilgileri: function (id, secilecekParametreControlIds) {
        //api/cari/finans-bilgileri/{carikart_id}
        var metodUrl = "api/cari/finans-bilgileri/" + id;
        var url = apiUrl + metodUrl;
        $.ajax({
            type: 'GET',
            url: apiUrl + metodUrl,
            async: false,
            success: function (data) {
                if (data != null) {
                    $('#carikart_id').val(data.carikart_id);
                    $('#tckimlikno').val(data.tckimlikno);
                    //$('#vergi_dairesi').val(data.vergi_dairesi);
                    $('#vergi_dairesi').val(data.vergi_dairesi).attr('data-id', data.vergi_dairesi);//vergi_dairesi 'ın attributune vergi_dairesi nin değerini atadık.
                    $('#vergi_no').val(data.vergi_no);
                    $('#diger_kod').val(data.diger_kod);
                    $('#muh_kod').val(data.muh_kod);
                    $('#tedarik_gunu').val(data.tedarik_gunu);
                    $('#swift_kodu').val(data.swift_kodu);
                    $('#iskonto_alis').val(data.iskonto_alis);
                    $('#iskonto_satis').val(data.iskonto_satis);
                    $('#odeme_sekli_id').val(data.odeme_sekli_id);
                    $('#masraf_merkezi_id').val(data.masraf_merkezi_id);
                    //$('#cari_odeme_sekli_id').val(data.cari_odeme_sekli_id);
                    $('#pb').val(data.pb);
                    $('#fiyattipi').val(data.fiyattipi);
                    $('#odeme_plani_id').val(data.odeme_plani_id);
                    $('#odeme_listesinde_cikmasin').val(data.odeme_listesinde_cikmasin);
                    $('#alacak_listesinde_cikmasin').val(data.alacak_listesinde_cikmasin);

                }
            }
        });

    },
    bankaHesapBilgileri: function (id) {
        //api/cari/cari-banka-hesaplari/{carikart_id}"
        if (id > 0) {
            var metodUrl = "api/cari/cari-banka-hesaplari/" + id;
            //var url = apiUrl + metodUrl;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    if (data != null) {
                        //html sayfadaki tablonun id'si 'bankaLst'
                        bankaHesapTable = $('#bankaLst').DataTable({
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
                                    data: "carikart_banka_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" name="carikartbankaidCheckBox" data-carikart_banka_id="' + row.carikart_banka_id + '" value="' + row.carikart_banka_id + '" data-carikart_id="' + row.carikart_id + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "banka_adi" },
                                { data: "banka_sube_adi" },
                                { data: "pb" },
                                { data: "ibanno" },
                                { data: "kredi_limiti_dbs" },
                                {
                                    data: "odemehesabi",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            if (row.odemehesabi == false) {
                                                return '<input type="checkbox" class="icheckbox_flat-blue" disabled="disabled" >';
                                            } else {
                                                return '<input type="checkbox" class="icheckbox_flat-blue" disabled="disabled"  checked >';
                                            }
                                        }
                                        return data;
                                    }
                                    , className: "dt-body-center"
                                }
                                //{ data: 'odemehesabi' }
                            ],
                            initComplete: function (settings, json) {
                                //var api = this.api();
                                genel.iCheck('input[name="carikartbankaidCheckBox"]');
                                //$('.dataTables_filter label').eq(0).text('Tabloda Ara');
                            }, drawCallback: function () {
                                $('.paginate_button')
                                    .on('click', function () {
                                        genel.iCheck('input[name="carikartbankaidCheckBox"]');
                                        var api = this.api();
                                    });
                            }

                        });
                        $('input[name="carikartbankaidCheckBox"]').on('ifChecked', function (event) {
                            var carikart_banka_id = event.target.attributes["data-carikart_banka_id"].nodeValue;
                            var carikart_id = event.target.attributes["data-carikart_id"].nodeValue;
                            var kayit_silindi = 'true';
                            //varyantCheckedValues.push({ sku_id: sku_id, stokkart_id: stokkart_id, sku_no: sku_no });
                            bankaCheckedValues.push({ carikart_id: carikart_id, carikart_banka_id: carikart_banka_id, kayit_silindi: kayit_silindi });
                        });
                        $('input[name="carikartbankaidCheckBox"]').on('ifUnchecked', function (event) {
                            var carikart_banka_id = event.target.attributes["data-carikart_banka_id"];
                            var tmpArray = [];
                            for (var item, i = 0; item = bankaCheckedValues[i++];) {
                                if (item.carikart_banka_id != carikart_banka_id) {
                                    tmpArray.push(item);
                                }
                            }
                            bankaCheckedValues = tmpArray;
                        });

                        $('.paginate_button').on('click', function () {
                            //checkbox();
                        });
                        // Double click kapatıldı. Update işlemi olmayacak.
                        //$('#bankaLst > tbody').on('dblclick', 'tr', function () {
                        //    var data = bankaHesapTable.row(this).data();
                        //    var carikart_banka_id = data.carikart_banka_id;
                        //    genel.talimatGet({ carikart_banka_id: id, event: "modelkart.put.talimat();", data: data });
                        //});

                    } else {
                        //if (bankaHesapTable != null) {
                        //    bankaHesapTable.destroy();
                        //    $('#bankaLst > tbody').html('');
                        //}

                    }
                }
            });

        }
    },
    finansFaturaIrsaliyeAdresi: function (id, faturaControlIds, irsaliyeControlIds) {
        if (id > 0) {
            //api/cari/cari-iletisim-bilgileri/{carikart_id}
            var metodUrl = "api/cari/cari-iletisim-bilgileri/" + id;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    $('#iletisim_adres_tipi_id').val(data.iletisim_adres_tipi_id);
                    //Fatura Adres alanları.
                    if (data.fatura_adresi != null) {
                        if (faturaControlIds != null) {
                            $('#' + faturaControlIds.ulke_id).val(data.fatura_adresi.ulke_id);
                            parametre.ulke_sehir_ilce_semt.sehirler(faturaControlIds.sehir_id, data.fatura_adresi.ulke_id);
                            $('#' + faturaControlIds.sehir_id).val(data.fatura_adresi.sehir_id);
                            parametre.ulke_sehir_ilce_semt.ilceler(faturaControlIds.ilce_id, data.fatura_adresi.sehir_id);
                            $('#' + faturaControlIds.ilce_id).val(data.fatura_adresi.ilce_id);
                            parametre.ulke_sehir_ilce_semt.semtler(faturaControlIds.semt_id, data.fatura_adresi.ilce_id);
                            $('#' + faturaControlIds.semt_id).val(data.fatura_adresi.semt_id);
                            // $('#carikart_id').val(data.carikart_id);
                            $('#carikart_adres_id').val(data.fatura_adresi.carikart_adres_id);
                            $('#postakodu').val(data.fatura_adresi.postakodu);
                            $('#adres').val(data.fatura_adresi.adres);
                            $('#tel1').val(data.fatura_adresi.tel1);
                            $('#tel2').val(data.fatura_adresi.tel2);
                            $('#fax').val(data.fatura_adresi.fax);
                            $('#email').val(data.fatura_adresi.email);
                            $('#websitesi').val(data.fatura_adresi.websitesi);
                            $('#yetkili_tel').val(data.fatura_adresi.yetkili_tel);
                        }
                    }
                    //İrsaliye Adres alanları.
                    if (data.irsaliye_adresi != null) {
                        if (irsaliyeControlIds != null) {
                            $('#' + irsaliyeControlIds.ulke_id).val(data.irsaliye_adresi.ulke_id);
                            parametre.ulke_sehir_ilce_semt.sehirler(irsaliyeControlIds.sehir_id, data.irsaliye_adresi.ulke_id);
                            $('#' + irsaliyeControlIds.sehir_id).val(data.irsaliye_adresi.sehir_id);
                            parametre.ulke_sehir_ilce_semt.ilceler(irsaliyeControlIds.ilce_id, data.irsaliye_adresi.sehir_id);
                            $('#' + irsaliyeControlIds.ilce_id).val(data.irsaliye_adresi.ilce_id);
                            parametre.ulke_sehir_ilce_semt.semtler(irsaliyeControlIds.semt_id, data.irsaliye_adresi.ilce_id);
                            $('#' + irsaliyeControlIds.semt_id).val(data.irsaliye_adresi.semt_id);
                            //İrsaliye Adres Alanları
                            $('#irs_carikart_adres_id').val(data.irsaliye_adresi.carikart_adres_id);
                            $('#irs_postakodu').val(data.irsaliye_adresi.postakodu);
                            $('#irs_adres').val(data.irsaliye_adresi.adres);
                            $('#irs_tel1').val(data.irsaliye_adresi.tel1);
                            $('#irs_tel2').val(data.irsaliye_adresi.tel2);
                            $('#irs_fax').val(data.irsaliye_adresi.fax);
                            $('#irs_email').val(data.irsaliye_adresi.email);
                            $('#irs_websitesi').val(data.irsaliye_adresi.websitesi);
                            $('#irs_yetkili_tel').val(data.irsaliye_adresi.yetkili_tel);
                        }
                    }



                }
            });

        }
    },
    //Finans 'ın altında Notlar sekmesi.
    finansCariNotlar: function (id) {
        if (id > 0) {
            //api/cari/cari-notlar/{carikart_id}
            var metodUrl = "api/cari/cari-notlar/" + id;
            //var url = apiUrl + metodUrl;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    if (data != null) {
                        cariNotlarTable = $('#notlarTable').DataTable({
                            data: data,
                            paging: true,
                            destroy: true,
                            lenghtChange: false,
                            searching: false,
                            ordering: true,
                            info: true,
                            autoWidth: true,
                            columns: [
                                {
                                    data: "carikart_not_id",
                                    render: function (data, type, row) {
                                        if (type == 'display') {
                                            return '<input type="checkbox" name="finansNotlarCheckBox" data-carikart_not_id="' + row.carikart_not_id + '" value="' + row.carikart_not_id + '" data-carikart_id="' + row.carikart_id + '" data-aciklama="' + row.aciklama + '" data-nereden="' + row.nereden + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: 'nereden' },
                                { data: 'aciklama' }
                            ],
                            initComplete: function (settings, json) {
                                genel.iCheck('input[name="finansNotlarCheckBox"]');
                            }, drawCallback: function () {
                                $('.paginate_button')
                                    .on('click', function () {
                                        genel.iCheck('input[name="finansNotlarCheckBox"]');
                                        var api = this.api();
                                    });
                            }
                        });
                        $('input[name="finansNotlarCheckBox"]').on('ifChecked', function (event) {
                            var carikart_not_id = event.target.attributes["data-carikart_not_id"].nodeValue;
                            var carikart_id = event.target.attributes["data-carikart_id"].nodeValue;
                            var kayit_silindi = 'true';
                            var aciklama = event.target.attributes["data-aciklama"].nodeValue;
                            var nereden = event.target.attributes["data-nereden"].nodeValue;
                            finansNotlarCheckedValues.push({ carikart_not_id: carikart_not_id, carikart_id: carikart_id, kayit_silindi: kayit_silindi, aciklama: aciklama, nereden: nereden });
                        });
                        $('input[name="finansNotlarCheckBox"]').on('ifUnchecked', function (event) {
                            var carikart_id = event.target.attributes["data-carikart_id"];
                            var tmpArray = [];
                            for (var item, i = 0; item = finansNotlarCheckedValues[i++];) {
                                if (item.carikart_id != carikart_id) {
                                    tmpArray.push(item);
                                }
                            }
                            finansNotlarCheckedValues = tmpArray;
                        });
                        $('.paginate_button').on('click', function () {
                            //checkbox();
                        });
                        $('#notlarTable > tbody').on('dblclick', 'tr', function () {
                            var data = cariNotlarTable.row(this).data();
                            var carikart_id = data.carikart_id;
                            genel.finansNotlarPopGet({ carikart_id: id, event: "carikartlar.post.finansNotlar();", data: data });
                        });
                    }
                }
            });
        }
    },
    finansIletisim: function (id) {
        if (id > 0) {
            //api/cari/cari-finans-iletisim/{carikart_id}
            var metodUrl = "api/cari/cari-finans-iletisim/" + id;
            var url = apiUrl + metodUrl;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    if (data != null) {
                        iletisimTbl = $('#iletisimTable').DataTable({
                            data: data,
                            paging: true,
                            destroy: true,
                            lenghtChange: false,
                            searching: false,
                            ordering: true,
                            info: true,
                            autoWidth: true,
                            columns: [
                                {
                                    data: "carikart_adres_id",
                                    render: function (data, type, row) {
                                        if (type == 'display') {
                                            //return '<input type="checkbox" name="carikartadresidCheckBox" data-carikart_adres_id="' + row.carikart_adres_id + ' value="' + row.carikart_adres_id + '" class="iCheck-helper">';
                                            return '<input type="checkbox" name="carikartadresidCheckBox" data-carikart_adres_id="' + row.carikart_adres_id + '" value="' + row.carikart_adres_id + '" data-carikart_id="' + row.carikart_id + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: 'yetkili_ad_soyad' },
                                { data: 'yetkili_tel' },
                                { data: 'email' }
                            ],
                            initComplete: function (settings, json) {
                                //var api = this.api();
                                genel.iCheck('input[name="carikartadresidCheckBox"]');
                                //$('.dataTables_filter label').eq(0).text('Tabloda Ara');
                            }, drawCallback: function () {
                                $('.paginate_button')
                                    .on('click', function () {
                                        genel.iCheck('input[name="carikartadresidCheckBox"]');
                                        var api = this.api();
                                    });
                            }

                        });
                        $('input[name="carikartadresidCheckBox"]').on('ifChecked', function (event) {
                            var carikart_adres_id = event.target.attributes["data-carikart_adres_id"].nodeValue;
                            var carikart_id = event.target.attributes["data-carikart_id"].nodeValue;
                            var kayit_silindi = 'true';
                            finansIletisiCheckedValues.push({ carikart_adres_id: carikart_adres_id, carikart_id: carikart_id, kayit_silindi: kayit_silindi });
                        });
                        $('input[name="carikartadresidCheckBox"]').on('ifUnchecked', function (event) {
                            var carikart_adres_id = event.target.attributes["data-carikart_adres_id"];
                            var tmpArray = [];
                            for (var item, i = 0; item = finansIletisiCheckedValues[i++];) {
                                if (item.carikart_adres_id != carikart_adres_id) {
                                    tmpArray.push(item);
                                }
                            }
                            finansIletisiCheckedValues = tmpArray;
                        });
                        $('.paginate_button').on('click', function () {
                            //checkbox();
                        });
                        $('#iletisimTable > tbody').on('dblclick', 'tr', function () {
                            var data = iletisimTbl.row(this).data();
                            var carikart_id = data.carikart_id;
                            genel.finansIletisimPopGet({ carikart_id: id, event: "carikartlar.post.finansIletisim();", data: data });
                        });
                    }
                }
            });
        }
    },
    ozelAlanlar: function (id) {
        if (id > 0) {
            //api/cari/ozel-alanlar/{carikart_id}
            var metodUrl = "api/cari/ozel-alanlar/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: 'Json',
                async: false,
                success: function (data) {
                    if (data != null) {
                        $('#satin_alma_sorumlu_cari_unvan').val(data.satin_alma_sorumlu_cari_unvan).attr('data-id', data.satin_alma_sorumlu_carikart_id);
                        $('#satis_sorumlu_cari_unvan').val(data.satis_sorumlu_cari_unvan).attr('data-id', data.satis_sorumlu_carikart_id);
                        $('#baslamatarihi').val(genel.dateFormat(data.baslamatarihi));
                        $('#ozel').val(data.ozel);
                    }
                }
            });
        }
    },
    raporParametreleri: function (Id) {
        //api/cari/cari-parametreleri-getir/{carikart_id}
        var metodUrl = "api/cari/cari-parametreleri-getir/" + Id;
        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {
                //$('#cari_parametre_1').val(data[0].cari_parametre_1);
                //$('#cari_parametre_2').val(data[0].cari_parametre_2);
                //$('#cari_parametre_3').val(data[0].cari_parametre_3);
                //$('#cari_parametre_4').val(data[0].cari_parametre_4);
                //$('#cari_parametre_5').val(data[0].cari_parametre_5);
                //$('#cari_parametre_6').val(data[0].cari_parametre_6);
                //$('#cari_parametre_7').val(data[0].cari_parametre_7);


                $('#cari_parametre_1').val(data.cari_parametre_1);
                $('#cari_parametre_2').val(data.cari_parametre_2);
                $('#cari_parametre_3').val(data.cari_parametre_3);
                $('#cari_parametre_4').val(data.cari_parametre_4);
                $('#cari_parametre_5').val(data.cari_parametre_5);
                $('#cari_parametre_6').val(data.cari_parametre_6);
                $('#cari_parametre_7').val(data.cari_parametre_7);

            }
        });
    },
    epostaGruplari: function (Id) {
        //api/cari/eposta-gruplari//{carikart_id}
        var metodUrl = "api/cari/eposta-gruplari/" + Id;
        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {
                //$('#carikart_id').val(data.carikart_id);
                $('#babs_formu_eposta').val(data.babs_formu_eposta);
                $('#cari_mutabakat_formu_eposta').val(data.cari_mutabakat_formu_eposta);
                $('#irsaliye_eposta').val(data.irsaliye_eposta);
                $('#odeme_hatirlatma_eposta').val(data.odeme_hatirlatma_eposta);
                $('#perakende_fatura_eposta').val(data.perakende_fatura_eposta);
                $('#siparis_formu_eposta').val(data.siparis_formu_eposta);
                $('#toptan_fatura_eposta').val(data.toptan_fatura_eposta);
                $('#degistiren_carikart_id').val(data.degistiren_carikart_id);
                $('#degistiren_tarih').val(data.degistiren_tarih);
            }
        });
    },
    iletisimBilgileri: function (id) {
        if (id > 0) {
            //api/cari/cari-iletisim-bilgileri/{carikart_id}
            var metodUrl = "api/cari/cari-iletisim-bilgileri/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: 'Json',
                async: false,
                success: function (data) {
                    if (data != null) {
                        //$('#carikart_id').val(data.carikart_id);
                        ////$('#iletisim_adres_tipi_id').val(data.iletisim_adres_tipi_id);
                        //$('#postakodu').val(data.fatura_adresi.postakodu);
                        //$('#adres').val(data.fatura_adresi.adres);
                        ////$('#ulke_id').val(data.fatura_adresi.ulke_id);
                        ////$('#sehir_id').val(data.fatura_adresi.sehir_id);
                        ////$('#ilce_id').val(data.fatura_adresi.ilce_id);
                        ////$('#semt_id').val(data.fatura_adresi.semt_id);
                        //$('#tel1').val(data.fatura_adresi.tel1);
                        //$('#tel2').val(data.fatura_adresi.tel2);
                        //$('#fax').val(data.fatura_adresi.fax);
                        //$('#email').val(data.fatura_adresi.email);
                        //$('#websitesi').val(data.fatura_adresi.websitesi);
                        //$('#yetkili_tel').val(data.fatura_adresi.yetkili_tel);
                        //$('#carikart_adres_id').val(data.irsaliye_adresi.carikart_adres_id);
                        //$('#postakodu1').val(data.irsaliye_adresi.postakodu);
                        //$('#adres1').val(data.irsaliye_adresi.adres);

                    }
                }
            });
        }
    },
    bankaHesapPopup: function (id) {
        genel.bankaHesapPopGet({ carikart_id: id, event: "carikartlar.post.bankaHesap();$('#myModal').modal('hide');", data: null });
    },
    finansIletisimPopup: function (id) {
        //genel.finansIletisimPopGet({ carikart_id: id, event: "carikartlar.post.finansIletisim();$('#myModal').modal('hide');", data: null });
        genel.finansIletisimPopGet({ carikart_id: id, event: "carikartlar.post.finansIletisim();$('#myModal').modal('hide');", data: null });

    },
    finansNotlarPopup: function (id) {
        //genel.finansIletisimPopGet({ carikart_id: id, event: "carikartlar.post.finansIletisim();$('#myModal').modal('hide');", data: null });
        genel.finansNotlarPopGet({ carikart_id: id, event: "carikartlar.post.finansNotlar();$('#myModal').modal('hide');", data: null });

    },
    finansSubePopup: function (id) {
        //genel.finansIletisimPopGet({ carikart_id: id, event: "carikartlar.post.finansIletisim();$('#myModal').modal('hide');", data: null });
        genel.finansSubePopGet({ carikart_id: id, event: "carikartlar.post.subeListesi();$('#myModal').modal('hide');", data: null });

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

                        $('#' + controlId).append('<option value="' + obj.carikart_id + '">' + obj.cari_unvan + '</option>');
                    });
                }
                else {
                    $('#' + controlId + ' option').remove();
                    $('#' + controlId).append('<option value="">Firma/Bayi Seçiniz</option>');
                }
            }
        });
        return jsonData;
    },
    //Firma Bayi Listesi End

    // Otomatik Aksesuar Ekleme Star
    otomatikAksesuar: {
        init: function () {
            carikartlar.get.otomatikAksesuar.otomatikAksesuarGrid("aksesuar");
            //carikartlar.get.otomatikAksesuar.searchPopupYukle();
        },
        otomatikAksesuarGrid: function (element) {
            var columns = [];
            //columns.push({
            //    title: "Uygulama Koşulu",
            //    template: "<button type='button' class='btn btn-info w80' onclick='carikartlar.get.uygulamaKosulu();'>Koşulu Ekle</button>"
            //});
            columns.push({ title: "Aksesuar Kodu", field: "stok_kodu", type: "string", editor: carikartlar.get.setAksesuarKodText });
            columns.push({ title: "Aksesuar Orjinal Kod", field: "orjinal_stok_kodu", type: "string" });
            columns.push({ title: "Aksesuar Adı", field: "stok_adi", type: "string" });
            columns.push({ title: "Renk", field: "renk_adi", type: "string" });
            columns.push({ title: "Birim", field: "birim_adi", type: "string" });
            columns.push({ title: "Miktar", field: "miktar", type: "string" });
            columns.push({
                command:
                [
                    { name: "edit", text: { edit: "Düzenle", update: "Güncelle", cancel: "Kapat" } },
                    { name: "destroy", text: "Sil" }
                ],
                title: "İşlemler"
            });
            $("#aksesuar").kendoGrid({
                dataBound: onDataBound,
                columns: columns,
                detailInit: detailInit,
                dataSource: {
                    transport: {
                        read: function (e) {
                            $.ajax({
                                url: apiUrl + "api/cari/aksesuar-denetim-listesi/100000000002,0",
                                method: 'get',
                                headers: {
                                    "content-type": "application/json",
                                    "cache-control": "no-cache"
                                },
                                type: "json",
                                dataType: "json",
                            }).success(function (data) {
                                console.log(data, "success");
                                e.success(data);
                            }).error(function (jqXHR, exception) {
                                //console.log(exception);
                                var errorJson = JSON.parse(jqXHR.responseText);
                                genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                            });
                            // .always(function () {
                            // kendo.ui.progress($("#aksesuar"), false);
                            //});
                        },

                        update: function (e) {
                            console.log("update", e);
                        }
                    },
                    pageSize: 20,

                    schema: {
                        model: {
                            id: "carikart_id"
                        }
                    }
                },
                toolbar: [{ name: "create", text: "Yeni Otomatik Aksesuar Ekle" }],
                //sortable: true,
                //height: 575,                
                pageable: {
                    refresh: true,
                    pageSizes: [10, 20, 50, 100, 200],
                    buttonCount: 5
                },
                editable: "inline"
            });
            $("#aksesuar").kendoTooltip({
                filter: "td", //this filter selects the second column's cells
                position: "right",
                content: function (e) {
                    return e.target.closest("td").context.innerHTML;
                }
            }).data("kendoTooltip");
            function detailInit(e) {
                $("<div/>").appendTo(e.detailCell).kendoGrid({
                    dataSource: {
                        type: "json",
                        transport: {
                            read: function (e) {
                                // carikartlar.get.kosulListesi;
                                $.ajax({
                                    type: "GET",
                                    url: apiUrl + "api/cari/aksesuar-denetim-listesi-kosullar",
                                    dataType: "json",
                                    async: false,
                                    success: function (data) {
                                        kosulListData = [];
                                        kosulListData.push(data);
                                        var kosulListDistinctData = [];
                                        $.each(data, function (i) {
                                            if (i == 0) {
                                                kosulListDistinctData.push({
                                                    "cevap_liste_sql": "",
                                                    "direkt_kosul": this.direkt_kosul,
                                                    "grup_adi": this.grup_adi,
                                                    "operator_liste": "",
                                                    "param_field_name": this.param_field_name,
                                                    "param_tanim": "",
                                                    "sira": this.sira,
                                                    "tip": this.tip
                                                });
                                            }
                                            else if (data[i].grup_adi != data[i - 1].grup_adi) {
                                                kosulListDistinctData.push({
                                                    "cevap_liste_sql": "",
                                                    "direkt_kosul": this.direkt_kosul,
                                                    "grup_adi": this.grup_adi,
                                                    "operator_liste": "",
                                                    "param_field_name": this.param_field_name,
                                                    "param_tanim": "",
                                                    "sira": this.sira,
                                                    "tip": this.tip
                                                });
                                            }
                                            e.success(kosulListDistinctData);

                                        })
                                    }
                                });
                                //
                            },

                        },
                        serverPaging: true,
                        serverSorting: true,
                        serverFiltering: true,
                        pageSize: 10,
                        filter: {
                            field: "tip", operator: "eq", value: e.data.tip,
                            field: "sira", operator: "eq", value: e.data.sira,
                        },
                        schema: {
                            model: {
                                fields: {
                                    grup_adi: { editable: false },
                                    param_tanim: { type: "string" },
                                    siparisturu_id: { type: "number" }
                                }
                            }
                        }
                    },
                    scrollable: false,
                    sortable: true,
                    pageable: true,
                    editable: true,
                    columns: [
                        { field: "grup_adi", title: "Grup" },
                        {
                            field: "param_field_name", title: "param_tanim", width: 200, editor: carikartlar.get.parametreDropDownEditor,
                            //template: ""
                            template: "#=data.param_tanim#"
                        },
                        {
                            field: "operator_liste", title: "operator_liste", editor: carikartlar.get.operatorListDropDown//,
                            //template: ""
                            // template: "#= carikartlar.get.getFirstOperator(data.operator_liste) #"
                        },
                        {
                            field: "siparisturu_id", title: "siparisturu_tanim", editor: carikartlar.get.aksesuarKosul,
                            template: "#= carikartlar.get.getSql(data) #"
                        }
                    ]
                });
            }
            function onDataBound() {
                var grid = $("#aksesuar").data("kendoGrid");
                for (var i = 0; i < grid.columns.length; i++) {
                    grid.autoFitColumn(i);
                }
            }
        },
        //searchPopupYukle: function() {
        //    myWindow.kendoWindow({
        //        width: "960px",
        //        title: "Aksesuar Arama",
        //        content: "/popup/SearchAksesuarPopup",
        //        visible: false,
        //        actions: [
        //            "Pin",
        //            "Minimize",
        //            "Maximize",
        //            "Close"
        //        ]
        //        //close: onClose
        //    }).data("kendoWindow");
        //}
    },
    setAksesuarKodText: function (container, options) {
        $('<input type="text" class="k-input k-textbox" name="stok_kodu"  ondblclick="carikartlar.get.aksesuarPopup(this);" data-bind="value:stok_kodu">')
            .appendTo(container);
    },
    aksesuarPopup: function (e) {//search popup
        genel.searchAksesuarPopup({ grup_adi: "grup", event: "$('#myModal').modal('hide');", data: null });
        // myWindow.data("kendoWindow").open().center().open();
    },
    parametreDropDownEditor: function (container, options) {
        paramListData = [];
        $.each(kosulListData[0], function (i) {
            if (options.model.grup_adi == this.grup_adi) {
                paramListData.push({ "param_tanim": this.param_tanim, "param_field_name": this.param_field_name });
            }
        })
        $('<input required id="' + options.field + '" name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: true,
                dataTextField: "param_tanim",
                dataValueField: "param_field_name",
                dataSource: {
                    data: paramListData,
                    schema: {
                        model: {
                            id: "grup_adi"
                        }
                    }
                },
                select: function (e) {
                    let grid = container.parents(".k-grid").data("kendoGrid");
                    let trIndex = container.parents("tr").index();

                    let row = grid.dataSource.at(trIndex);
                    row.set("param_tanim", e.dataItem.param_tanim);
                    row.set("param_field_name", e.dataItem.param_field_name);
                },
                dataBound: function () {
                    this.value(options.model.param_tanim);
                }
            });
    },
    operatorListDropDown: function (container, options) {
        $.each(kosulListData[0], function (i) {
            if (options.model.grup_adi == this.grup_adi) {
                operatorListData = [];
                for (var k = 0; k < this.operator_liste.split(';').length; k++) {
                    operatorListData.push({ "operator_liste": this.operator_liste.split(';')[k] });
                }
                i = kosulListData[0].length;
            }
        })
        $('<input required id="' + options.field + '" name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                // autoBind: true,
                dataTextField: "operator_liste",
                dataValueField: "operator_liste",
                dataSource: {
                    data: operatorListData,
                    schema: {
                        model: {
                            id: "grup_adi"
                        }
                    }
                },
                select: function (e) {
                    let grid = container.parents(".k-grid").data("kendoGrid");
                    let trIndex = container.parents("tr").index();

                    let row = grid.dataSource.at(trIndex);
                    row.set("operator_liste", e.dataItem.operator_liste);
                    row.set("operator_liste", e.dataItem.operator_liste);
                    //carikartlar.get.aksesuarKosul(container, options);
                },
                dataBound: function () {
                    this.value(options.model.operator_liste);
                }
            });
    },
    //getFirstOperator: function(e) {
    //    return e.split(';').length > 0 ? e.split(';')[0] : e;
    //},
    getCevapListSql: function (e) {
        return e.split(';').length > 0 ? e.split(';')[0] : e;
    },
    getSql: function (e) {
        console.log(e);
        return e.siparisturu_tanim == undefined ? "" : e.siparisturu_tanim;
    },
    cevapListDropDown: function (container, options) {
        cevapListeSqlData = [];
        $.each(kosulListData[0], function () {
            if (options.model.grup_adi == this.grup_adi && options.model.sira == this.sira) {
                cevapListeSqlData.push({ "cevap_liste_sql": this.cevap_liste_sql, "cevap_liste_sql": this.cevap_liste_sql });
            }
        })
        console.log(cevapListeSqlData);
        $('<input required id="' + options.field + '" name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                // autoBind: true,
                dataTextField: "cevap_liste_sql",
                dataValueField: "cevap_liste_sql",
                dataSource: {
                    data: cevapListeSqlData,
                    //transport: {
                    //    dataType: "json",
                    //    read: apiUrl + "api/cari/aksesuar-denetim-listesi-kosullar",
                    //},
                    schema: {
                        model: {
                            id: "grup_adi"
                        }
                    }
                },
                select: function (e) {
                    let grid = container.parents(".k-grid").data("kendoGrid");
                    let trIndex = container.parents("tr").index();

                    let row = grid.dataSource.at(trIndex);
                    row.set("cevap_liste_sql", e.dataItem.cevap_liste_sql);
                    row.set("cevap_liste_sql", e.dataItem.cevap_liste_sql);
                },
                dataBound: function () {
                    //console.log("renk bound")
                    this.value(options.model.cevap_liste_sql);
                    //this.text("item adı")
                }

            });
    },

    uygulamaKosuluGet: function (carikart_id, tip) {
        $.ajax({
            url: '/popup/otomatikAksesuarPopup',
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            async: false,
            dataType: 'html',
        }).success(function (result) {
            console.log("OTomatik Aksesuar ekleme", result);
            genel.modal("Uygulama Koşulu Ekle", result, "liste", "$('#myModal').modal('hide');");
        });
    },
    uygulamaKosulu: function (id) {
        carikartlar.get.uygulamaKosuluGet(id);
    },
    aksesuarKosul: function (container, options) {
        var grup_adi = encodeURIComponent(options.model.grup_adi);
        var sira = options.model.sira;
        $.ajax({
            url: apiUrl + 'api/cari/aksesuar-denetim-kosullar/' + sira + ',' + grup_adi,
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            async: false,
            dataType: 'json',
        }).success(function (result) {
            var datasource = eval(result[0]);
            console.log(datasource);
            $('<input required id="' + options.field + '" name="' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    // autoBind: true,
                    dataTextField: "siparisturu_tanim",
                    dataValueField: "siparisturu_id",
                    dataSource: {
                        data: datasource,
                        //transport: {
                        //    dataType: "json",
                        //    read: apiUrl + "api/cari/aksesuar-denetim-listesi-kosullar",
                        //},
                        schema: {
                            model: {
                                id: "grup_adi"
                            }
                        }
                    },
                    select: function (e) {
                        let grid = container.parents(".k-grid").data("kendoGrid");
                        let trIndex = container.parents("tr").index();

                        let row = grid.dataSource.at(trIndex);
                        row.set("siparisturu_tanim", e.dataItem.siparisturu_tanim);
                        row.set("siparisturu_id", e.dataItem.siparisturu_id);
                    },
                    dataBound: function () {
                        this.value(options.model.siparisturu_tanim);
                    }

                });
        });

    }
    // Otomatik Aksesuar Ekle End
};
carikartlar.put = {
    send: function () {
        $("#CarikartTable").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                var metodUrl = "api/cari";
                var carikart_id = $('#hfcarikart_id').val();
                var dataArray = $(this).serializeArray();
                // console.log(dataArray);
                //CheckBox lar serializeArray ile gelmediğinden farklı bir dizi içerisine kopyalandı
                var serializedCheckbox = $('input:checkbox').map(function () {
                    return { name: this.name, value: this.value == 'on' ? true : false };// ? this.value : "false"
                });
                var dataObj = {};
                $(dataArray).each(function (i, field) {
                    dataObj[field.name] = field.value;
                });
                //alert($(satin_alma_sorumlu_cari_unvan).attr('data-id'));
                var postData = {
                    "carikart_id": carikart_id,
                    "cari_unvan": dataObj['cari_unvan'],
                    "statu": dataObj['statu'],
                    //"carikart_turu_id": dataObj['carikart_turu_id'],
                    "carikart_tipi_id": dataObj['carikart_tipi_id'],
                    "ozel_kod": dataObj['ozel_kod'],
                    "fiyattipi": dataObj['fiyattipi'],
                    //"transfer_depo_id": dataObj['transfer_depo_id'],
                    "sube_carikart_id": $(sube_cari_unvan).attr('data-id'),
                    "ana_carikart_id": $(ana_cari_unvan).attr('data-id'),
                    "finans_sorumlu_carikart_id": $(finans_sorumlu_cari_unvan).attr('data-id'),
                    //"satin_alma_sorumlu_carikart_id": $(satin_alma_sorumlu_cari_unvan).attr('data-id'),
                    //"satis_sorumlu_carikart_id": $(satis_sorumlu_cari_unvan).attr('data-id'),
                    //RAPOR PArametreleri Sekmesi.
                    "cari_parametre_1": dataObj['cari_parametre_1'],
                    "cari_parametre_2": dataObj['cari_parametre_2'],
                    "cari_parametre_3": dataObj['cari_parametre_3'],
                    "cari_parametre_4": dataObj['cari_parametre_4'],
                    "cari_parametre_5": dataObj['cari_parametre_5'],
                    "cari_parametre_6": dataObj['cari_parametre_6'],
                    "cari_parametre_7": dataObj['cari_parametre_7'],

                }
                $.ajax({
                    async: true,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'POST',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(postData)
                }).success(function (data) {
                    if (data != undefined && data.message == 'successful') {
                        genel.modal("Tebrikler!", "Kayıt güncellendi", "basarili", "$('#myModal').modal('hide');");
                        carikartlar.put.finansBilgileri(carikart_id);
                        carikartlar.put.ozelAlanlar(carikart_id);
                        carikartlar.put.epostaGruplari(carikart_id);
                        carikartlar.post.finansFaturaIrsaliyeAdresi();

                    } else {
                        genel.modal("Hata!", "Güncelleme işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    var errorJson = JSON.parse(jqXHR.responseText);
                    genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                });
            }
        });
    },
    epostaGruplari: function (id) {
        var metodUrl = "api/cari/eposta-gruplari";
        var postData = {
            "carikart_id": id, //$(hfcarikart_id).val(),
            "babs_formu_eposta": $(babs_formu_eposta).val(),
            "cari_mutabakat_formu_eposta": $(cari_mutabakat_formu_eposta).val(),
            "irsaliye_eposta": $(irsaliye_eposta).val(),
            "odeme_hatirlatma_eposta": $(odeme_hatirlatma_eposta).val(),
            "perakende_fatura_eposta": $(perakende_fatura_eposta).val(),
            "siparis_formu_eposta": $(siparis_formu_eposta).val(),
            "toptan_fatura_eposta": $(toptan_fatura_eposta).val(),
        }
        $.ajax({
            async: true,
            crossDomain: true,
            url: apiUrl + metodUrl,
            method: 'POST',
            headers: {
                "content-type": "application/json",
                "cache-control": "no-cache"
            },
            processData: false,
            data: JSON.stringify(postData)
        });

    },
    ozelAlanlar: function (id) {
        if (id > 0) {
            //api/cari/ozel-alanlar
            var metodUrl = "api/cari/ozel-alanlar";
            var dataArray = $("div.ozelaln *").serializeArray();
            var postData = {
                "carikart_id": id,
                "baslamatarihi": $('#baslamatarihi').datepicker({ dateFormat: 'mm-dd-yyyy' }).val(), // $(baslamatarihi).val(),
                "ozel": $(ozel).val(),
                "satin_alma_sorumlu_carikart_id": $(satin_alma_sorumlu_cari_unvan).attr('data-id'),
                "satis_sorumlu_carikart_id": $(satis_sorumlu_cari_unvan).attr('data-id'),
            }
            $.ajax({
                async: false,
                crossDomain: true,
                url: apiUrl + metodUrl,
                method: 'POST',
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                processData: false,
                data: JSON.stringify(postData)

            });
        }
    },
    finansBilgileri: function (id) {
        //api/cari/finans-bilgileri/{carikart_id}
        var metodUrl = "api/cari/finans-bilgileri";
        var carikart_id = $('#hfcarikart_id').val();
        var url = apiUrl + metodUrl;
        var dataObj = {};
        var postData = {
            "carikart_id": carikart_id,
            "tckimlikno": $(tckimlikno).val(),
            "vergi_dairesi": $(vergi_dairesi).val(),
            "masraf_merkezi_id": $(masraf_merkezi_id).val(),
            "vergi_no": $(vergi_no).val(),
            "diger_kod": $(diger_kod).val(),
            "muh_kod": $(muh_kod).val(),
            "tedarik_gunu": $(tedarik_gunu).val(),
            "swift_kodu": $(swift_kodu).val(),
            "iskonto_alis": $(iskonto_alis).val(),
            "iskonto_satis": $(iskonto_satis).val(),
            "odeme_sekli_id": $(odeme_sekli_id).val(),
            "odeme_listesinde_cikmasin": $(odeme_listesinde_cikmasin).val(),
            "finans_sorumlu_carikart_id": $(finans_sorumlu_cari_unvan).attr('data-id'),
            "alacak_listesinde_cikmasin": $(alacak_listesinde_cikmasin).val(),
            "odeme_plani_id": $(odeme_plani_id).val(),
            "pb": $(pb).val(),
            "fiyattipi": $(fiyattipi).val(),

        }
        $.ajax({
            async: false,
            crossDomain: true,
            url: apiUrl + metodUrl,
            method: 'POST',
            headers: {
                "content-type": "application/json",
                "cache-control": "no-cache"
            },
            processData: false,
            data: JSON.stringify(postData)
        });

    },
};
carikartlar.post = {
    send: function () {
        $("#cariKartNew").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                var metodUrl = "api/cari";
                var carikart_id = $('#hfcarikart_id').val();
                var dataArray = $(this).serializeArray();
                var serializedCheckbox = $('input:checkbox').map(function () {
                    return { name: this.name, value: this.value == 'on' ? true : false };// ? this.value : "false"
                });
                var dataObj = {};
                $(dataArray).each(function (i, field) {
                    dataObj[field.name] = field.value;
                });
                var postData = {
                    "carikart_id": carikart_id,
                    "cari_unvan": dataObj['cari_unvan'],
                    "statu": dataObj['statu'],

                    //"carikart_turu_id": dataObj['carikart_turu_id'],
                    "carikart_tipi_id": dataObj['carikart_tipi_id'],
                    "ozel_kod": dataObj['ozel_kod'],
                    "fiyattipi": dataObj['fiyattipi'],
                    //"transfer_depo_id": dataObj['transfer_depo_id'],

                    "sube_carikart_id": $(sube_cari_unvan).attr('data-id'),
                    "ana_carikart_id": $(ana_cari_unvan).attr('data-id'),
                    "finans_sorumlu_carikart_id": $(finans_sorumlu_cari_unvan).attr('data-id'),
                    //"satin_alma_sorumlu_carikart_id": $(satin_alma_sorumlu_cari_unvan).attr('data-id'),
                    //"satis_sorumlu_carikart_id": $(satis_sorumlu_cari_unvan).attr('data-id'),
                    //RAPOR PArametreleri Sekmesi.
                    "cari_parametre_1": dataObj['cari_parametre_1'],
                    "cari_parametre_2": dataObj['cari_parametre_2'],
                    "cari_parametre_3": dataObj['cari_parametre_3'],
                    "cari_parametre_4": dataObj['cari_parametre_4'],
                    "cari_parametre_5": dataObj['cari_parametre_5'],
                    "cari_parametre_6": dataObj['cari_parametre_6'],
                    "cari_parametre_7": dataObj['cari_parametre_7'],

                }
                $.ajax({
                    async: true,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'POST',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(postData)
                }).success(function (data) {
                    if (data != undefined && data.message == 'successful') {
                        genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');location = '/carikart/detail/" + data.ret_val + "'");
                        carikartlar.put.finansBilgileri(carikart_id);
                        carikartlar.put.ozelAlanlar(carikart_id);
                        carikartlar.put.epostaGruplari(carikart_id);
                        if (data.ret_val != null) {
                            carikartlar.post.finansFaturaIrsaliyeAdresi(data.ret_val);
                        }
                        else {
                            carikartlar.post.finansFaturaIrsaliyeAdresi(carikart_id);
                        }
                    } else {
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    var errorJson = JSON.parse(jqXHR.responseText);
                    genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                });
            }
        });
    },
    bankaHesap: function () {
        $("#bankaHesapForm").on('submit', function (e) {
            e.preventDefault();
            //api/stokkart/varyant/
            var metodUrl = "api/cari/cari-banka-hesabi";
            var dataArray = $(this).serializeArray();

            var dataObj = {};
            $(dataArray).each(function (i, field) {
                dataObj[field.name] = field.value;
            });
            var postData = {
                "carikart_id": dataObj["hfId"],
                //"ulke_id": dataObj["ulke_id"],
                "banka_id": dataObj["banka_id"],
                "banka_sube_id": dataObj["banka_sube_id"],
                "ibanno": dataObj["ibanno"],
                "pb": dataObj["pb1"],
                "ebanka": dataObj["ebanka"],
                //"odemehesabi": dataObj["odemehesabi"],
                "kredi_limiti_dbs": dataObj["kredi_limiti_dbs"],
                //"kayit_silindi": dataObj["kayit_silindi"],

            }
            var carikart_id = dataObj['hfId'];
            $.ajax({
                async: false,
                crossDomain: true,
                url: apiUrl + metodUrl,
                method: 'POST',
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                processData: false,
                data: JSON.stringify(postData)
            }).success(function (data) {
                if (data != undefined && data.message == 'successful') {
                    genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');");
                    genel.timer(300, 'carikartlar.get.bankaHesapBilgileri(' + carikart_id + ')');
                } else {
                    genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                }
            }).error(function (jqXHR, exception) {
                genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            });

        });
        $("#bankaHesapForm").trigger('submit');
    },
    finansIletisim: function () {
        $("#finansIletisimForm").on('submit', function (e) {
            e.preventDefault();
            //api/stokkart/varyant/
            $("#myModal").modal("hide");
            var metodUrl = "api/cari/cari-finans-iletisim";
            var dataArray = $(this).serializeArray();

            var dataObj = {};
            $(dataArray).each(function (i, field) {
                dataObj[field.name] = field.value;
            });
            var postData = {
                "carikart_id": dataObj["hfId"],
                "carikart_adres_id": dataObj["carikart_adres_id"],
                "email": dataObj["email"],
                "websitesi": dataObj["websitesi"],
                "yetkili_ad_soyad": dataObj["yetkili_ad_soyad"],
                "yetkili_tel": dataObj["yetkili_tel"],
                //"kayit_silindi": dataObj["kayit_silindi"], Gerekirse eklerim AA.

            }
            var carikart_id = dataObj['hfId'];
            $.ajax({
                async: false,
                crossDomain: true,
                url: apiUrl + metodUrl,
                method: 'POST',
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                processData: false,
                data: JSON.stringify(postData)
            }).success(function (data) {
                if (data != undefined && data.message == 'successful') {
                    //genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');");
                    carikartlar.get.finansIletisim(carikart_id);
                    //genel.timer(300, 'carikartlar.get.finansIletisim(' + carikart_id + ')');
                } else {
                    genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                }
            }).error(function (jqXHR, exception) {
                //genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            });

        });
        $("#finansIletisimForm").trigger('submit');
    },
    finansNotlar: function () {
        $("#finansNotlarFrm").on('submit', function (e) {
            e.preventDefault();
            $("#myModal").modal("hide");
            var metodUrl = "api/cari/cari-notlar/";
            //var dataArray = $("div.finanscarint *").serializeArray(); Sadece finanscarint clasındaki bilgileri alıyor.
            var dataArray = $(this).serializeArray();

            var dataObj = {};
            $(dataArray).each(function (i, field) {
                dataObj[field.name] = field.value;
            });
            var postData = {
                "carikart_id": dataObj["hfId"],
                "carikart_not_id": dataObj["carikart_not_id"],
                "aciklama": dataObj["aciklama"],
                "nereden": dataObj["nereden"],
            }
            var carikart_id = dataObj['hfId'];
            $.ajax({
                async: false,
                crossDomain: true,
                url: apiUrl + metodUrl,
                method: 'POST',
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                processData: false,
                data: JSON.stringify(postData)
            }).success(function (data) {
                if (data != undefined && (data.message == 'successful' || data.message == 'Successful')) {
                    carikartlar.get.finansCariNotlar(carikart_id);
                } else {
                    genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                }
            }).error(function (jqXHR, exception) {
                //genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            });

        });
        $("#finansNotlarFrm").trigger('submit');
    },
    subeListesi: function () {
        $("#finansSubeForm").on('submit', function (e) {
            e.preventDefault();
            $("#myModal").modal("hide");
            var metodUrl = "api/cari/cari-sube-listesi";
            var dataArray = $(this).serializeArray();

            var dataObj = {};
            $(dataArray).each(function (i, field) {
                dataObj[field.name] = field.value;
            });

            var postData = {
                "carikart_id": dataObj["hfcarikart_id"], //$(hfId).val(),
                "ana_carikart_id": $('#ana_carikart_id').attr('data-ana_carikart_id'),
                //"carikart_tipi_id": dataObj["carikart_tipi_id"],
                //"carikart_turu_id": dataObj["carikart_turu_id"],
            }
            var carikart_id = dataObj['hfcarikart_id'];
            $.ajax({
                async: false,
                crossDomain: true,
                url: apiUrl + metodUrl,
                method: 'POST',
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                processData: false,
                data: JSON.stringify(postData)
            }).success(function (data) {
                if (data != undefined && (data.message == 'successful' || data.message == 'Successful')) {
                    carikartlar.get.subeListesi(carikart_id);
                } else {
                    genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                }
            }).error(function (jqXHR, exception) {
                //genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            });

        });
        $("#finansSubeForm").trigger('submit');
    },
    finansFaturaIrsaliyeAdresi: function (id) {
        //api/cari/cari-iletisim-bilgileri
        var metodUrl = "api/cari/cari-iletisim-bilgileri";
        var postData = {
            //"carikart_id": id,

            "carikart_id": $('#carikart_id').val(),
            "iletisim_adres_tipi_id": $('#iletisim_adres_tipi_id').val(),
            fatura_adresi: {
                //Fatura Adres alanları.
                "carikart_adres_id": $('#carikart_adres_id').val(),
                "ulke_id": $('#ulke_id').val(),
                "sehir_id": $('#sehir_id').val(),
                "ilce_id": $('#ilce_id').val(),
                "semt_id": $('#semt_id').val(),
                "postakodu": $('#postakodu').val(),
                "adres": $('#adres').val(),
                "tel1": $('#tel1').val(),
                "tel2": $('#tel2').val(),
                "fax": $('#fax').val(),
                "email": $('#email').val(),
                "websitesi": $('#websitesi').val(),
                "yetkili_tel": $('#yetkili_tel').val(),
            },
            irsaliye_adresi: {
                //İrsaliye Adres alanları.
                carikart_adres_id: $('#irs_carikart_adres_id').val(),
                "ulke_id": $('#irs_ulke_id').val(),
                "sehir_id": $('#irs_sehir_id').val(),
                "ilce_id": $('#irs_ilce_id').val(),
                "semt_id": $('#irs_semt_id').val(),
                "postakodu": $('#irs_postakodu').val(),
                "adres": $('#irs_adres').val(),
                "tel1": $('#tel1').val(),
                "tel2": $('#tel2').val(),
                "fax": $('#fax').val(),
                "email": $('#email').val(),
                "websitesi": $('#websitesi').val(),
                "yetkili_tel": $('#yetkili_tel').val(),
            },
        }
        $.ajax({
            async: true,
            crossDomain: true,
            url: apiUrl + metodUrl,
            method: 'POST',
            headers: {
                "content-type": "application/json",
                "cache-control": "no-cache"
            },
            processData: false,
            data: JSON.stringify(postData)
        });
    },
};
carikartlar.delete = {
    bankaHesap: function (id) {
        if (bankaCheckedValues.length > 0) {
            var metodUrl = "api/cari/cari-banka-hesabi";
            var errCount = 0;
            var errorMessage = '';

            for (var i = 0; i < bankaCheckedValues.length; i++) {
                $.ajax({
                    async: false,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'POST',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(bankaCheckedValues[i])
                }).success(function (data) {
                    if (data != undefined && data.message == 'successful') {

                    } else {
                        errCount++;
                        errorMessage = "Kayıt işlemi yapılırken bir hata oluştu!";
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    errCount++;
                    var errorJson = JSON.parse(jqXHR.responseText);
                    errorMessage = errorJson.message;
                });
                if (errCount > 0) {
                    break;
                }
            }
            if (errCount > 0) {
                genel.timer(300, 'genel.modal("Hata!", "' + errorMessage + '", "hata", "$(\'#myModal\').modal(\'hide\');");');
            } else {

            }
            /*Hata olsun ya da olmasın checkboxlar uncheck ediliyor ve dizi boşaltılıyor*/
            bankaCheckedValues = [];
            carikartlar.get.bankaHesapBilgileri(id);
        }
    },
    finansIletisim: function (id) {
        if (finansIletisiCheckedValues.length > 0) {
            var metodUrl = "api/cari/cari-finans-iletisim";
            var errCount = 0;
            var errorMessage = '';

            for (var i = 0; i < finansIletisiCheckedValues.length; i++) {
                $.ajax({
                    async: false,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'POST',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(finansIletisiCheckedValues[i])
                }).success(function (data) {
                    if (data != undefined && (data.message == 'successful' || data.message == 'Successful')) {

                    } else {
                        errCount++;
                        errorMessage = "Kayıt işlemi yapılırken bir hata oluştu!";

                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    errCount++;
                    var errorJson = JSON.parse(jqXHR.responseText);
                    errorMessage = errorJson.message;
                });

                if (errCount > 0) {
                    break;
                }
            }

            if (errCount > 0) {
                genel.timer(300, 'genel.modal("Hata!", "' + errorMessage + '", "hata", "$(\'#myModal\').modal(\'hide\');");');
            } else {

            }

            /*Hata olsun ya da olmasın checkboxlar uncheck ediliyor ve dizi boşaltılıyor*/
            finansIletisiCheckedValues = [];
            carikartlar.get.finansIletisim(id);
        }
    },
    finansNotlar: function () {
        if (finansNotlarCheckedValues.length > 0) {
            var metodUrl = "api/cari/cari-notlar";
            var errCount = 0;
            var errorMessage = '';
            for (var i = 0; i < finansNotlarCheckedValues.length; i++) {
                $.ajax({
                    async: false,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'POST',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(finansNotlarCheckedValues[i])
                }).success(function (data) {
                    if (data != undefined && (data.message == 'successful' || data.message == 'Successful')) {
                    } else {
                        errCount++;
                        errorMessage = "Kayıt işlemi yapılırken bir hata oluştu!";
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    errCount++;
                    var errorJson = JSON.parse(jqXHR.responseText);
                    errorMessage = errorJson.message;
                });

                if (errCount > 0) {
                    break;
                }
            }
            if (errCount > 0) {
                genel.timer(300, 'genel.modal("Hata!", "' + errorMessage + '", "hata", "$(\'#myModal\').modal(\'hide\');");');
            } else {
            }
            /*Hata olsun ya da olmasın checkboxlar uncheck ediliyor ve dizi boşaltılıyor*/
            carikartlar.get.finansCariNotlar(finansNotlarCheckedValues[0].carikart_id);
            finansNotlarCheckedValues = [];
        }
    },
    subeListesi: function (id) {
        if (finansSubeCheckedValues.length > 0) {
            var metodUrl = "api/cari/cari-sube-listesi";
            var errCount = 0;
            var errorMessage = '';

            for (var i = 0; i < finansSubeCheckedValues.length; i++) {
                $.ajax({
                    async: false,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'POST',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(finansSubeCheckedValues[i])
                }).success(function (data) {
                    if (data != undefined && data.message == 'successful') {

                    } else {
                        errCount++;
                        errorMessage = "Kayıt işlemi yapılırken bir hata oluştu!";
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    errCount++;
                    var errorJson = JSON.parse(jqXHR.responseText);
                    errorMessage = errorJson.message;
                });
                if (errCount > 0) {
                    break;
                }
            }
            if (errCount > 0) {
                genel.timer(300, 'genel.modal("Hata!", "' + errorMessage + '", "hata", "$(\'#myModal\').modal(\'hide\');");');
            } else {

            }
            /*Hata olsun ya da olmasın checkboxlar uncheck ediliyor ve dizi boşaltılıyor*/
            finansSubeCheckedValues = [];
            carikartlar.get.subeListesi(id);
        }
    }
};
