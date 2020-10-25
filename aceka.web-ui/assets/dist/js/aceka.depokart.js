/*
aceka.genel.js dosyasından gelecek
var apiUrl = 'http://92.45.23.86:9597/';
*/

var depokart = {};
var bankaCheckedValues = [];
var depoFiyatCheckedValues = [];
var finansIletisiCheckedValues = [];
var finansNotlarCheckedValues = [];
var finansSubeCheckedValues = [];
var fiyatlarTable = null;

depokart.search = {
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
                                //{ data: "para_birimi" },
                                { data: "cari_tipi_adi" },
                                //{ data: "parametre_parabirimi.pb_kodu" }, email
                                 { data: "telefon" },
                                { data: "email" },
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
                location = '/depo/detail/' + data.carikart_id;

            });

        });
    }
};
depokart.get = {
    init: function (id) {
        $('.depoBtn').attr("onclick", "depokart.get.depoFiyatPopup(" + id + ")");
        $('.depoDeleteBtn').attr("onclick", "depokart.delete.depoFiyatTipleri(" + id + ")");
        $('.finansiletisimDeleteBtn').attr("onclick", "depokart.delete.finansIletisim(" + id + ")");
        $('.finansiletisimBtn').attr("onclick", "depokart.get.finansIletisimPopup(" + id + ")");
        $('.bankahesapBtn').attr("onclick", "depokart.get.bankaHesapPopup(" + id + ")");
        $('.bankahesapDeleteBtn').attr("onclick", "depokart.delete.bankaHesap(" + id + ")");
        $('.subeBtn').attr("onclick", "depokart.get.finansSubePopup(" + id + ")");
        $('.subeDeleteBtn').attr("onclick", "depokart.delete.subeListesi(" + id + ")");

        //$('.ilkMaddeBtn').attr("onclick", "modelkart.get.ilkMaddeGetPopup(" + id + ")");
        //$('.fileUploadButton').attr("onclick", "modelkart.get.eklerGetPopup(" + id + ")");
        //$('.fileDeleteButton').attr("onclick", "modelkart.delete.ekler(" + id + ")");
        //$('.talimatDeleteBtn').attr("onclick", "modelkart.delete.talimat(" + id + ")");
        //$('.varyantDeleteBtn').attr("onclick", "modelkart.delete.varyant(" + id + ")");
    },
    genelUst: function (id, secilecekParametreControlIds) {
        if (id > 0) {
            //api/depo/genel/{carikart_id}
            var metodUrl = "api/depo/genel/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: 'Json',
                async: false,
                success: function (data) {
                    if (data != undefined && data != null) {

                        $('#carikart_id').val(data.carikart_id);
                        $('#statu').val(data.statu);
                        $('#ana_cari_unvan').val(data.baglistokyeri_unvan).attr('data-id', data.ana_carikart_id);
                        //$('#depoKartForm > #hfcarikart_id').val(data.carikart_id);
                        $('#carikart_tipi_id').val(data.carikart_tipi_id);
                        $('#carikart_turu_id').val(data.carikart_turu_id);
                        $('#muh_kod').val(data.muh_kod);
                        $('#masraf_merkezi_id').val(data.masraf_merkezi_id);
                        //$('#baglistokyeri_unvan').val(data.baglistokyeri_unvan);
                        $('#sirket_id').val(data.sirket_id);
                        $('#giz_yazilim_kodu').val(data.giz_yazilim_kodu);
                        $('#earsiv_seri').val(data.earsiv_seri);
                        $('#efatura_seri').val(data.efatura_seri);
                        $('#ozel_kod').val(data.ozel_kod);
                        $('#acilis_tarihi').val(genel.dateFormat(data.acilis_tarihi));
                        $('#kapanis_tarihi').val(genel.dateFormat(data.kapanis_tarihi));

                        $('#cari_unvan').val(data.cari_unvan);
                        $('#ozel_kod').val(data.ozel_kod);
                        //$('#transfer_depo_kullan').val(data.transfer_depo_kullan);

                        $("#acilis_tarihi").tarihComponent();
                        $("#kapanis_tarihi").tarihComponent();

                        //genel.iCheck($("#transfer_depo_kullan").prop("checked", data.transfer_depo_kullan != null ? data.transfer_depo_kullan.toString() : false));
                        genel.iCheck($("#transfer_depo_kullan").prop("checked", data.transfer_depo_kullan));
                        genel.iCheck($("#kapali").prop("checked", data.kapali));
                         
                        //$('#ana_cari_unvan').val(data.ana_cari_unvan).attr('data-id', data.ana_carikart_id);//ana_cari_unvan 'ın attributune ana_carikart_id nin değerini atadık.
                        //$('#sube_cari_unvan').val(data.sube_cari_unvan).attr('data-id', data.sube_carikart_id);
                        $('#' + secilecekParametreControlIds.statu).val(data.statu.toString());

                        //$('#satin_alma_sorumlu_cari_unvan').val(data.satin_alma_sorumlu_cari_unvan).attr('data-id', data.satin_alma_sorumlu_carikart_id);
                        //$('#satis_sorumlu_cari_unvan').val(data.satis_sorumlu_cari_unvan).attr('data-id', data.satis_sorumlu_carikart_id);
                        $('#finans_sorumlu_cari_unvan').val(data.finans_sorumlu_cari_unvan).attr('data-id', data.finans_sorumlu_carikart_id);

                        $('#' + secilecekParametreControlIds.ulke_id).val(data.ulke_id);
                        $('#' + secilecekParametreControlIds.sehir_id).val(data.sehir_id);
                        $('#' + secilecekParametreControlIds.ilce_id).val(data.ilce_id)

                        $('#' + secilecekParametreControlIds.semt_id).val(data.semt_id);
                    }
                }
            });
        }
    },
    depoRaporParametreleri: function (id, secilecekParametreControlIds) {
        if (id > 0) {
            //api/cari/cari-parametreleri-getir/{carikart_id} 
            var metodUrl = "api/cari/cari-parametreleri-getir/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: 'Json',
                async: false,
                success: function (data) {
                    if (data != undefined && data != null) {

                        $('#cari_parametre_1').val(data.cari_parametre_1);
                        $('#cari_parametre_2').val(data.cari_parametre_2);
                        $('#cari_parametre_3').val(data.cari_parametre_3);
                        $('#cari_parametre_4').val(data.cari_parametre_4);
                        $('#cari_parametre_5').val(data.cari_parametre_5);
                        $('#cari_parametre_6').val(data.cari_parametre_6);
                        $('#cari_parametre_7').val(data.cari_parametre_7);
                    }

                }
            });
        }
    },
    depoFiyatTipleri: function (Id) {
        //api/depo/fiyat-tipleri
        var metodUrl = "api/parametre/depo-fiyat-tipleri/" + Id;
        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != undefined && data != null) {
                    depoFiyatTable = $('#depofiyat').DataTable({
                        data: data,
                        paging: false,
                        searching: false,
                        //scrollY: "250px",
                        //scrollCollapse: true,
                        destroy: true,
                        ordering: true,
                        columns: [
                            //{ data: "fiyattipi" },
                                    { data: "fiyattipi_adi" },
                                    {
                                        data: "statu",
                                        render: function (data, type, row) {
                                            if (type === 'display') {
                                                if (row.statu == true) {
                                                    return '<input class="iCheck-helper" name="depoStatuCheckbox" type="checkbox"  id="statu"  value="' + row.statu + '"  checked="checked"  data-fiyattipi="' + row.fiyattipi + '" disabled ></input>';
                                                } else {
                                                    return '<input type="checkbox" id="statu"  name="depoStatuCheckbox"  value="' + row.statu + '" class="iCheck-helper" disabled ></input>';
                                                }

                                            }
                                            return data;
                                        }
                                    },
                                    {
                                        data: "varsayilan",
                                        render: function (data, type, row) {
                                            if (type === 'display') {
                                                if (row.varsayilan == true) {
                                                    return '<input class="iCheck-helper"  type="checkbox" name="varsayilan" id="varsayilan" value="' + row.varsayilan + '" checked="checked" disabled></input>';
                                                    //return '<input class="iCheck-helper" type="checkbox" name="varsayilan" id="varsayilan"  value="' + row.varsayilan + '" checked="checked" ></input>';
                                                } else {
                                                    return '<input type="checkbox" id="varsayilan" name="varsayilan"  value="' + row.varsayilan + '" class="iCheck-helper" disabled></input>';
                                                }

                                            }
                                            return data;
                                        }
                                    },
                        ],
                        //"columnDefs": [
                        //    {
                        //        "targets": [0], //ilk kolonu gizliyoruz.
                        //        "visible": false,
                        //        "searchable": false
                        //    }
                        //],
                        initComplete: function (settings, json) {

                            genel.iCheck('input[name="depofiyatCheckBox"]');
                            genel.iCheck('input[name="depoStatuCheckbox"]');
                            genel.iCheck('input[name="varsayilan"]');
                        }, drawCallback: function () {
                            $('.paginate_button')
                               .on('click', function () {
                                   genel.iCheck('input[name="depofiyatCheckBox"]');
                                   var api = this.api();
                               });
                        }
                    });
                    $('input[name="depofiyatCheckBox"]').on('ifChecked', function (event) {
                        var carikart_id = event.target.attributes["data-carikart_id"].nodeValue;
                        var fiyattipi = event.target.attributes["data-fiyattipi"].nodeValue;
                        var statu = event.target.attributes["data-statu"].nodeValue;
                        var varsayilan = event.target.attributes["data-varsayilan"].nodeValue;
                        var kayit_silindi = event.target.attributes["data-kayit_silindi"].nodeValue;
                        depoFiyatCheckedValues.push({ carikart_id: carikart_id, fiyattipi: fiyattipi, statu: statu, varsayilan: varsayilan, kayit_silindi: kayit_silindi });
                    });
                    $('input[name="depofiyatCheckBox"]').on('ifUnchecked', function (event) {
                        var carikart_id = event.target.attributes["data-carikart_id"].nodeValue;
                        var fiyattipi = event.target.attributes["data-fiyattipi"].nodeValue;
                        var statu = event.target.attributes["data-statu"].nodeValue;
                        var varsayilan = event.target.attributes["data-varsayilan"].nodeValue;
                        var kayitsilindi = event.target.attributes["data-kayit_silindi"].nodeValue;
                        var tmpArray = [];
                        for (var item, i = 0; item = depoFiyatCheckedValues[i++];) {
                            if (item.carikart_id != carikart_id && item.fiyattipi != fiyattipi) {
                                tmpArray.push(item);
                            }
                        }
                        depoFiyatCheckedValues = tmpArray;
                    });
                } else {
                    //if (depoFiyatTable != null) {
                    //    depoFiyatTable.destroy();
                    //    $('#depofiyat > tbody').html('');
                    //}
                }
            }
        });
    },
    depoiletisimBilgileri: function (id, secilecekParametreControlIds) {
        if (id > 0) {
            //api/depo/iletisim-bilgileri/{carikart_id}
            var metodUrl = "api/depo/iletisim-bilgileri/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: 'Json',
                async: false,
                success: function (data) {
                    if (data != null) {
                        //$('#ulke_id').val(data.ulke_id);
                        $('#adresunvan').val(data.adresunvan);
                        $('#yetkili_ad_soyad').val(data.yetkili_ad_soyad);
                        $('#yetkili_tel').val(data.yetkili_tel);
                        $('#email').val(data.email);
                        $('#websitesi').val(data.websitesi);

                        $('#postakodu').val(data.postakodu);
                        $('#adres').val(data.adres);
                        //$('#ulke_id').val(data.ulke_id);
                        //$('#sehir_id').val(data.sehir_id);
                        //$('#ilce_id').val(data.ilce_id);
                        //$('#semt_id').val(data.semt_id);
                        $('#tel1').val(data.tel1);
                        $('#tel2').val(data.tel2);
                        $('#fax').val(data.fax);
                        $('#email').val(data.email);
                        $('#websitesi').val(data.websitesi);
                        $('#yetkili_tel').val(data.yetkili_tel);
                        $('#carikart_adres_id').val(data.carikart_adres_id);
                        //$('#adres1').val(data.irsaliye_adresi.adres);

                        $('#' + secilecekParametreControlIds.ulke_id).val(data.ulke_id);
                        parametre.ulke_sehir_ilce_semt.sehirler(secilecekParametreControlIds.sehir_id, data.ulke_id);
                        $('#' + secilecekParametreControlIds.sehir_id).val(data.sehir_id);
                        parametre.ulke_sehir_ilce_semt.ilceler(secilecekParametreControlIds.ilce_id, data.sehir_id);
                        $('#' + secilecekParametreControlIds.ilce_id).val(data.ilce_id);
                        parametre.ulke_sehir_ilce_semt.semtler(secilecekParametreControlIds.semt_id, data.ilce_id);
                        $('#' + secilecekParametreControlIds.semt_id).val(data.semt_id);

                    }
                }
            });
        }
    },
    depoFiyatPopup: function (id) {
        //genel.finansIletisimPopGet({ carikart_id: id, event: "carikartlar.post.finansIletisim();$('#myModal').modal('hide');", data: null });
        genel.depoFiyatPopup({ carikart_id: id, event: "depokart.post.depoFiyatTipleri();$('#myModal').modal('hide');", data: null });

    },
};
depokart.put = {
    send: function () {
        $("#depoKartForm").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                var metodUrl = "api/depo/genel";
                var carikart_id = $('#hfcarikart_id').val();
                var carikart_tipi_id = $('#hfcarikart_tipi_id').val();
                var dataObj = {};
                var dataArray = $(this).serializeArray();
                // console.log(dataArray);
                //CheckBox lar serializeArray ile gelmediğinden farklı bir dizi içerisine kopyalandı
                var serializedCheckbox = $('input:checkbox').map(function () {
                    return { name: this.name, value: this.value == 'on' ? true : false };// ? this.value : "false"
                });
                $(dataArray).each(function (i, field) {
                    dataObj[field.name] = field.value;
                });
                //CheckBox lar da dataObj de düzeltiliyor. Burada "on" olan checkbox' lar ture ya da false oluyor.
                $(serializedCheckbox).each(function (i, field) {
                    if ($.trim(field.name) != '') {
                        dataObj[field.name] = field.value;
                    }
                });
                var postData = {
                    "carikart_id": carikart_id,
                    "cari_unvan": dataObj['cari_unvan'],
                    "statu": dataObj['statu'],
                    "carikart_tipi_id": dataObj['carikart_tipi_id'],
                    "carikart_turu_id": dataObj['carikart_turu_id'],
                    "ozel_kod": dataObj['ozel_kod'],
                    "muh_kod": dataObj['muh_kod'],
                    "masraf_merkezi_id": dataObj['masraf_merkezi_id'],
                    "ana_carikart_id": $(ana_cari_unvan).attr('data-id'),
                    //"baglistokyeri_unvan": dataObj['baglistokyeri_unvan'],
                    "sirket_id": dataObj['sirket_id'],
                    "giz_yazilim_kodu": dataObj['giz_yazilim_kodu'],
                    "earsiv_seri": dataObj['earsiv_seri'],
                    "efatura_seri": dataObj['efatura_seri'],
                    //"acilis_tarihi"         : dataObj['acilis_tarihi'],
                    "acilis_tarihi": dataObj['acilis_tarihi'] != null ? genel.dateFormatByJavascript(dataObj['acilis_tarihi'], '.') : null,
                    "kapanis_tarihi": dataObj['kapanis_tarihi'] != null ? genel.dateFormatByJavascript(dataObj['kapanis_tarihi'], '.') : null,
                    //"acilis_tarihi": $('#acilis_tarihi').datepicker({ dateFormat: 'mm-dd-yyyy' }).val(), //dataObj['kapanis_tarihi'],
                    //"kapanis_tarihi"        : $('#kapanis_tarihi').datepicker({ dateFormat: 'mm-dd-yyyy' }).val(), //dataObj['kapanis_tarihi'],
                    "transfer_depo_kullan": dataObj['transfer_depo_kullan'],
                    "kapali": dataObj['kapali'],

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
                        depokart.put.depoiletisimBilgileri(carikart_id);
                        depokart.put.depoRaporParametreleri(carikart_id);

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
    depoiletisimBilgileri: function (id) {
        if (id > 0) {
            //api/depo/iletisim-bilgileri
            var metodUrl = "api/depo/iletisim-bilgileri";
            var postData = {
                //İletişim Bilgileri
                "carikart_id": id,
                "adresunvan": $('#adresunvan').val(),
                "yetkili_ad_soyad": $('#yetkili_ad_soyad').val(),
                "yetkili_tel": $('#yetkili_tel').val(),
                "email": $('#email').val(),
                "ulke_id": $('#ulke_id').val(),
                "sehir_id": $('#sehir_id').val(),
                "ilce_id": $('#ilce_id').val(),
                "semt_id": $('#semt_id').val(),
                "postakodu": $('#postakodu').val(),
                "fax": $('#fax').val(),
                "tel1": $('#tel1').val(),
                "tel2": $('#tel2').val(),
                "adres": $('#adres').val(),
                "kayit_silindi": $('#kayit_silindi').val(),
                "carikart_adres_id": $('#carikart_adres_id').val(),
                "websitesi": $('#websitesi').val(),
            }
            $.ajax({
                async: false,
                crossDomain: true,
                url: apiUrl + metodUrl,
                method: 'PUT',
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                processData: false,
                data: JSON.stringify(postData)
            });
        }
    },
    depoRaporParametreleri: function (id) {
        if (id > 0) {
            //api/depo/iletisim-bilgileri
            var metodUrl = "api/depo/rapor-parametre";
            var postData = {
                //İletişim Bilgileri
                "carikart_id": id,
                "cari_parametre_1": $('#cari_parametre_1').val(),
                "cari_parametre_2": $('#cari_parametre_2').val(),
                "cari_parametre_3": $('#cari_parametre_3').val(),
                "cari_parametre_4": $('#cari_parametre_4').val(),
                "cari_parametre_5": $('#cari_parametre_5').val(),
                "cari_parametre_6": $('#cari_parametre_6').val(),
                "cari_parametre_7": $('#cari_parametre_7').val()
            }
            $.ajax({
                async: false,
                crossDomain: true,
                url: apiUrl + metodUrl,
                method: 'PUT',
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                processData: false,
                data: JSON.stringify(postData)
            });
        }
    }
};
depokart.post = {
    send: function () {
        $("#depoKartNew").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                var metodUrl = "api/depo/genel";
                var carikart_id = $('#hfcarikart_id').val();
                var carikart_tipi_id = $('#hfcarikart_tipi_id').val();
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
                    "carikart_tipi_id": dataObj['carikart_tipi_id'],
                    "carikart_turu_id": dataObj['carikart_turu_id'],
                    "ozel_kod": dataObj['ozel_kod'],
                    "muh_kod": dataObj['muh_kod'],
                    "masraf_merkezi_id": dataObj['masraf_merkezi_id'],
                    "ana_carikart_id": $(ana_cari_unvan).attr('data-id'),
                    "sirket_id": dataObj['sirket_id'],
                    "giz_yazilim_kodu": dataObj['giz_yazilim_kodu'],
                    "earsiv_seri": dataObj['earsiv_seri'],
                    "efatura_seri": dataObj['efatura_seri'],
                    "acilis_tarihi": dataObj['acilis_tarihi'] != null ? genel.dateFormatByJavascript(dataObj['acilis_tarihi'], '.') : null,
                    "kapanis_tarihi": dataObj['kapanis_tarihi'] != null ? genel.dateFormatByJavascript(dataObj['kapanis_tarihi'], '.') : null,
                    "transfer_depo_kullan": dataObj['transfer_depo_kullan'],
                    "kapali": dataObj['kapali'],

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
                        genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');location = '/depo/detail/" + data.ret_val + "'");
                        depokart.put.depoiletisimBilgileri(data.ret_val);
                        depokart.put.depoRaporParametreleri(data.ret_val);
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
    depoFiyatTipleri: function () {
        $("#depoFiyForm").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                $("#myModal").modal("hide");
                var metodUrl = "api/depo/fiyat-tipleri";
                var errorMessage = '';
                var dataArray = $(this).serializeArray();
                var dataObj = {};
                $(dataArray).each(function (i, field) {
                    dataObj[field.name] = field.value;
                });
                var postData = {
                    "carikart_id": $('#hfId').val(),
                    "fiyattipi": dataObj["fiyattipi"],
                    "statu": dataObj["statu"],
                    "varsayilan": dataObj["varsayilan"],
                    "kayit_silindi": true
                }
                var carikart_id = $('#hfId').val();
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
                        genel.timer(300, 'genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$(\'#myModal\').modal(\'hide\');");depokart.get.depoFiyatTipleri(' + carikart_id + ');');
                    } else {
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    var errorJson = JSON.parse(jqXHR.responseText);
                    [0]["startAt"]
                    genel.timer(300, 'genel.modal("Hata!", "Daha önce eklenmiş veya Silinmiş kayıt!", "hata", "$(\'#myModal\').modal(\'hide\');");');

                });
            }
        });
        $("#depoFiyForm").trigger('submit');
    },

    ////cari kart iletişim bilgilerindeki, "Fatura Adresi"  ve "İrsaliye Adresi" Bölümleri
    //finansFaturaIrsaliyeAdresi: function (id) {
    //    //api/cari/cari-iletisim-bilgileri
    //    var metodUrl = "api/cari/cari-iletisim-bilgileri";
    //    var postData = {
    //        //"carikart_id": id,
    //        "carikart_id": $('#carikart_id').val(),
    //        "iletisim_adres_tipi_id": $('#iletisim_adres_tipi_id').val(),
    //        fatura_adresi: {
    //            //Fatura Adres alanları.
    //            "carikart_adres_id": $('#carikart_adres_id').val(),
    //            "ulke_id": $('#ulke_id').val(),
    //            "sehir_id": $('#sehir_id').val(),
    //            "ilce_id": $('#ilce_id').val(),
    //            "semt_id": $('#semt_id').val(),
    //            "postakodu": $('#postakodu').val(),
    //            "adres": $('#adres').val(),
    //            "tel1": $('#tel1').val(),
    //            "tel2": $('#tel2').val(),
    //            "fax": $('#fax').val(),
    //            "email": $('#email').val(),
    //            "websitesi": $('#websitesi').val(),
    //            "yetkili_tel": $('#yetkili_tel').val(),
    //        },
    //        irsaliye_adresi: {
    //            //İrsaliye Adres alanları.
    //            carikart_adres_id: $('#irs_carikart_adres_id').val(),
    //            "ulke_id": $('#irs_ulke_id').val(),
    //            "sehir_id": $('#irs_sehir_id').val(),
    //            "ilce_id": $('#irs_ilce_id').val(),
    //            "semt_id": $('#irs_semt_id').val(),
    //            "postakodu": $('#irs_postakodu').val(),
    //            "adres": $('#irs_adres').val(),
    //            "tel1": $('#tel1').val(),
    //            "tel2": $('#tel2').val(),
    //            "fax": $('#fax').val(),
    //            "email": $('#email').val(),
    //            "websitesi": $('#websitesi').val(),
    //            "yetkili_tel": $('#yetkili_tel').val(),
    //        },
    //    }
    //    $.ajax({
    //        async: true,
    //        crossDomain: true,
    //        url: apiUrl + metodUrl,
    //        method: 'POST',
    //        headers: {
    //            "content-type": "application/json",
    //            "cache-control": "no-cache"
    //        },
    //        processData: false,
    //        data: JSON.stringify(postData)
    //    });
    //},
};
depokart.delete = {
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
            depokart.get.bankaHesapBilgileri(id);
        }
    },
    depoFiyatTipleri: function (id) {
        if (depoFiyatCheckedValues.length > 0) {

            var metodUrl = "api/depo/fiyat-tipleri";
            var errCount = 0;
            var errorMessage = '';

            for (var i = 0; i < depoFiyatCheckedValues.length; i++) {
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
                    data: JSON.stringify(depoFiyatCheckedValues[i])
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
            depoFiyatCheckedValues = [];
            depokart.get.depoFiyatTipleri(id);

        }

    },

};
