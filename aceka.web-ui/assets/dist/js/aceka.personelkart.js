var personelkart = [];
var calismaYeriCheckedValues = [];


personelkart.search = {
    init: function () {

        $('.searchBtn').on('click', function () {

            //api/personel/personel-bul?carikart_id={carikart_id}&cari_unvan={cari_unvan}&ozel_kod={ozel_kod}&carikart_tipi_id={carikart_tipi_id}&statu={statu}
            var metodUrl = "api/personel/personel-arama";
            //var ozel_kod = $("#ozelKod").val();
            var carikart_id = $("#carikart_id").val();
            var cari_unvan = $("#cari_unvan").val();
            //var carikart_tipi_id = $('#orjinal_stok_kodu').val();
            var statu = $('#statu').val();

            //if (statu == 'Aktif')
            //    statu = '&statu=1';
            //else if (statu == 'Pasif')
            //    statu = '&statu=0';
            //else statu = '';

            var carikart_tipi_id = $('#carikart_tipi_id').val();
            if (carikart_tipi_id == null || carikart_tipi_id == '')
                carikart_tipi_id = 21;

            //url: apiUrl + metodUrl + '?carikart_id=' + carikart_id + '&cari_unvan=' + cari_unvan + '&ozel_kod=' + ozel_kod + '&carikart_tipi_id=' + carikart_tipi_id + '&statu=' + statu,
           // var url = apiUrl + metodUrl + '?cari_unvan=' + cari_unvan + '&carikart_tipi_id=' + carikart_tipi_id + '&statu=' + statu;

            var table = null;
            $.ajax({
                type: "GET",
                //url: apiUrl + metodUrl + '?carikart_id=' + carikart_id + '&cari_unvan=' + cari_unvan + '&ozel_kod=' + ozel_kod + '&carikart_tipi_id=' + carikart_tipi_id + '&statu=' + statu,
                url: apiUrl + metodUrl + '?cari_unvan=' + cari_unvan + '&carikart_tipi_id=' + carikart_tipi_id + '&statu=' + statu,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        table = $('#personelKart').DataTable({
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
                                    data: "personel_no",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" name="personel" data-id="' + row.personel_no + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "tanim" },
                                 { data: "personel_kod" },
                                {
                                    data: "statu",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            if (row.statu == "pasif") {
                                                return '<span class="badge bg-red">Pasif</span>';
                                            } else {
                                                return '<span class="badge bg-green">Aktif</span>';
                                            }
                                        }
                                        return data;
                                    }
                                    //,className: "dt-body-center"
                                }
                            ],
                            initComplete: function (settings, json) {
                               // checkbox();
                                genel.iCheck('input[name="personel"]');
                            }
                        });

                        $('.paginate_button').on('click', function () {
                           // checkbox();
                            genel.iCheck('input[name="personel"]');
                        });
                    }
                    else {
                        genel.modal("Dikkat!", "Kayıt bulunamadı", "uyari", "$('#myModal').modal('hide');");
                    }
                }
            });
            $('#personelKart> tbody').on('dblclick', 'tr', function () {
                var data = table.row(this).data();
                location = '/personel/detail/' + data.personel_no;
            });
        });
    }
};
personelkart.get = {
    init: function (id) {

        $('.perCalismaYeriBtn').attr("onclick", "personelkart.get.personelcalismaPopup(" + id + ")");
        $('.perCalismaYeriDeleteBtn').attr("onclick", "personelkart.delete.calismaYeri(" + id + ")");
    },
    genelUst: function (id, secilecekParametreControlIds) {
        $.ajax({
            url: apiUrl + "api/personel/genel/" + id,//100000000969",
            type: 'Get',
            dataType: 'JSON',
            success: function (data) {
                $('#carikart_id').val(data.carikart_id);
                $('#giz_kullanici_sifre').val(data.giz_kullanici_sifre);
                $('#giz_kullanici_adi').val(data.giz_kullanici_adi);
                $('#cari_unvan').val(data.cari_unvan);
                $('#ozel_kod').val(data.ozel_kod);

                $('#' + secilecekParametreControlIds.statu).val(data.statu.toString());

                if (data.muh_masraf.muh_kod > 0) $('#' + secilecekParametreControlIds.muh_kod).val(data.muh_masraf.muh_kod);
                else $('#' + secilecekParametreControlIds.muh_kod).val(0);
                if (data.sube_carikart_id > 0) $('#' + secilecekParametreControlIds.sube_carikart_id).val(data.sube_carikart_id);
                else $('#' + secilecekParametreControlIds.sube_carikart_id).val(0);
            }
        });
    },
    iletisim: function (id, secilecekParametreControlIds) {
        $.ajax({
            url: apiUrl + "api/personel/iletisim/" + id,//100000000969",
            type: 'Get',
            dataType: 'JSON',
            success: function (data) {
                if (typeof data != 'undefined') {
                    $('#yetkili_ad_soyad').val(data.yetkili_ad_soyad);
                    $('#yetkili_tel').val(data.yetkili_tel);
                    $('#email').val(data.email);
                    $('#websitesi').val(data.websitesi);
                    $('#postakodu').val(data.postakodu);
                    $('#fax').val(data.fax);
                    $('#tel1').val(data.tel1);
                    $('#tel2').val(data.tel2);
                    $('#adres').val(data.adres);

                    $('#' + secilecekParametreControlIds.selectUlkeler).val(data.ulke_id);
                    $('#' + secilecekParametreControlIds.selectUlkeler).change();

                    $('#' + secilecekParametreControlIds.selectSehirler).val(data.sehir_id);
                    $('#' + secilecekParametreControlIds.selectSehirler).change();

                    $('#' + secilecekParametreControlIds.selectIlceler).val(data.ilce_id);
                    $('#' + secilecekParametreControlIds.selectIlceler).change();

                    $('#' + secilecekParametreControlIds.selectSemtler).val(data.semt_id);
                    //console.log(data.ilce_id);
                }
               
            }
        });
    },
    calismayerleri: function (id) {
        if (id > 0) {
            //api/siparis/talimatlar/{siparis_id}
            var metodUrl = "api/personel/calismayeri/" + id;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    if (data != null) {
                        calismaYeriTable = $('#calismayerleri').DataTable({
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
                                    data: "stokyeri_carikart_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" name="calismayerleriCheckBox" data-carikart_id="' + row.carikart_id + '"value="' + row.stokyeri_carikart_id + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                 { data: "stokyeri_carikart_adi" },
                                { data: "gorev_adi" },
                                { data: "departman_adi" }
                            ],
                            initComplete: function (settings, json) {
                                genel.iCheck('input[name="calismayerleriCheckBox"]');
                            }, drawCallback: function () {
                                $('.paginate_button')
                                   .on('click', function () {
                                       genel.iCheck('input[name="calismayerleriCheckBox"]');
                                       var api = this.api();
                                   });
                            }
                        });
                        $('.paginate_button').on('click', function () {
                            //checkbox();
                        });
                        //$('#calismayerleri > tbody').on('dblclick', 'tr', function () {
                        //    var data = calismaYeriTable.row(this).data();
                        //    var siparis_id = data.siparis_id;
                        //    genel.talimatGet({ siparis_id: id, event: "siparis.put.talimatlar();", data: data });
                        //});
                        $('input[name="calismayerleriCheckBox"]').on('ifChecked', function (event) {
                            var stokyeri_carikart_id = event.target.value;
                            var carikart_id = event.target.attributes["data-carikart_id"].nodeValue;
                            calismaYeriCheckedValues.push({ stokyeri_carikart_id: stokyeri_carikart_id, carikart_id: carikart_id });
                        });
                        $('input[name="calismayerleriCheckBox"]').on('ifUnchecked', function (event) {
                            var stokyeri_carikart_id = event.target.value;
                            var carikart_id = event.target.attributes["data-carikart_id"].nodeValue;
                            var tmpArray = [];
                            for (var item, i = 0; item = calismaYeriCheckedValues[i++];) {
                                if (item.stokyeri_carikart_id == stokyeri_carikart_id && item.carikart_id == carikart_id) {
                                    tmpArray.push(item);
                                }
                            }
                            calismaYeriCheckedValues = tmpArray;
                        });

                    } else {
                        //if (talimatlarTable != null) {
                        //    talimatlarTable.destroy();
                        //    $('#talimatlar > tbody').html('');
                        //}
                    }
                }
            });
        }

    },
    raporparametreleri: function (id, secilecekParametreControlIds) {
        $.ajax({
            url: apiUrl + "api/personel/parametre/" + id,
            type: 'Get',
            dataType: 'JSON',
            success: function (data) {

                if (typeof data != 'undefined') {
                    if (data.cari_parametre_1 > 0) $('#' + secilecekParametreControlIds.cari_parametre_1).val(data.cari_parametre_1);
                    else $('#' + secilecekParametreControlIds.cari_parametre_1).val(0);

                    if (data.cari_parametre_2 > 0) $('#' + secilecekParametreControlIds.cari_parametre_2).val(data.cari_parametre_2);
                    else $('#' + secilecekParametreControlIds.cari_parametre_2).val(0);

                    if (data.cari_parametre_3 > 0) $('#' + secilecekParametreControlIds.cari_parametre_3).val(data.cari_parametre_3);
                    else $('#' + secilecekParametreControlIds.cari_parametre_3).val(0);

                    if (data.cari_parametre_4 > 0) $('#' + secilecekParametreControlIds.cari_parametre_4).val(data.cari_parametre_4);
                    else $('#' + secilecekParametreControlIds.cari_parametre_4).val(0);

                    if (data.cari_parametre_5 > 0) $('#' + secilecekParametreControlIds.cari_parametre_5).val(data.cari_parametre_5);
                    else $('#' + secilecekParametreControlIds.cari_parametre_5).val(0);

                    if (data.cari_parametre_6 > 0) $('#' + secilecekParametreControlIds.cari_parametre_6).val(data.cari_parametre_6);
                    else $('#' + secilecekParametreControlIds.cari_parametre_6).val(0);

                    if (data.cari_parametre_7 > 0) $('#' + secilecekParametreControlIds.cari_parametre_7).val(data.cari_parametre_7);
                    else $('#' + secilecekParametreControlIds.cari_parametre_7).val(0);
                }

            }
        });
    },
    personelcalismaPopup: function (id) {
        genel.personelcalismaYeriPopGet({ carikart_id: id, event: "personelkart.post.calismaYeri();", data: null });
    },
};
personelkart.Parametreler = {
    //GEreksiz yazılmış kodlar. Parametrelerde zaten vardı. AA.
    //subeler: (function () {
    //    var metodurl = 'api/cari/cari-liste-turu-bayi-ve-cari';
    //    var jsonData = [];
    //    $.ajax({
    //        type: "GET",
    //        url: apiUrl + metodurl,
    //        dataType: "json",
    //        async: false,
    //        success: function (data) {
    //            if (data != null) {
    //                $.each(data, function (key, obj) {
    //                    $('#sube_carikart_id').append('<option value="' + obj.carikart_id + '">' + obj.cari_unvan + '</option>');
    //                    //$('#sube_carikart_id').append('<option value="' + obj.carikart_id + '">' + obj.carikart_id + '-' + obj.cari_unvan + '</option>');
    //                });
    //            }
    //        }
    //    });
    //}),
    //muhasebekodlari: (function () {
    //    var metodurl = 'api/cari/muhasebe-kodlari';
    //    var jsonData = [];
    //    $.ajax({
    //        type: "GET",
    //        url: apiUrl + metodurl,
    //        dataType: "json",
    //        async: false,
    //        success: function (data) {
    //            if (data != null) {
    //                $.each(data, function (key, obj) {
    //                    $('#muh_kod').append('<option value="' + obj.muh_kod_id + '">' + obj.muh_kod_adi + '</option>');
    //                    //$('#muh_kod').append('<option value="' + obj.muh_kod_id + '">' + obj.muh_kod_id + '-' + obj.muh_kod_adi + '</option>');
    //                });
    //            }
    //        }
    //    });
    //}),
    //cariparametreler: (function (parametreid) {
    //    var metodurl = 'api/personel/cariparametrelistesi';
    //    var jsonData = [];
    //    $.ajax({
    //        type: "GET",
    //        url: apiUrl + metodurl,
    //        dataType: "json",
    //        async: false,
    //        success: function (data) {
    //            if (data != null) {
    //                $.each(data, function (key, obj) {
    //                    if (obj.parametre == parametreid) {
    //                        $('#cari_parametre_' + parametreid).append('<option value="' + obj.parametre_id + '">' + obj.parametre_adi + '</option>');
    //                    }
    //                });
    //            }
    //        }
    //    });
    //}),
    //aceka.parametreye alındı. A.A
    //gorevler: (function () {
    //    var metodurl = 'api/personel/gorev-parametreler';
    //    var jsonData = [];
    //    $.ajax({
    //        type: "GET",
    //        url: apiUrl + metodurl,
    //        dataType: "json",
    //        async: false,
    //        success: function (data) {
    //            if (data != null) {
    //                $.each(data, function (key, obj) {
    //                    $('#gorev').append('<option value="' + obj.parametre_id + '">' + obj.parametre_adi + '</option>');
    //                });
    //            }
    //        }
    //    });
    //}),
    //departmanlar: (function () {
    //    var metodurl = 'api/personel/departman-parametreler';
    //    var jsonData = [];
    //    $.ajax({
    //        type: "GET",
    //        url: apiUrl + metodurl,
    //        dataType: "json",
    //        async: false,
    //        success: function (data) {
    //            if (data != null) {
    //                $.each(data, function (key, obj) {
    //                    $('#departman').append('<option value="' + obj.parametre_id + '">' + obj.parametre_adi + '</option>');
    //                });
    //            }
    //        }
    //    });
    //}),
    //calismayeri: (function () {
    //    var metodurl = 'api/personel/calismayerleri';
    //    var jsonData = [];
    //    $.ajax({
    //        type: "GET",
    //        url: apiUrl + metodurl,
    //        dataType: "json",
    //        async: false,
    //        success: function (data) {
    //            if (data != null) {
    //                $.each(data, function (key, obj) {
    //                    $('#calismayeri').append('<option value="' + obj.carikart_id + '">' + obj.cari_unvan + '</option>');
    //                });
    //            }
    //        }
    //    });
    //})
};
personelkart.post = {
    init: function () {
    },
    send: function () {
        $("#detailPersonelForm").on('submit', function (e) {
            e.preventDefault();
            var metodUrl = "api/personel/genel";
            //var formData = $(this).serialize();
            var dataArray = $(this).serializeArray();
            //console.log(dataArray);

            //CheckBox lar serializeArray ile gelmediğinden farklı bir dizi içerisine kopyalandı
            var serializedCheckbox = $('input:checkbox').map(function () {
                return { name: this.name, value: this.value == 'on' ? true : false };// ? this.value : "false"
            });

            var dataObj = {};

            $(dataArray).each(function (i, field) {
                dataObj[field.name] = field.value;
            });

            //CheckBox lar da dataArray e atanıyor.
            $(serializedCheckbox).each(function (i, field) {
                if ($.trim(field.name) != '') {
                    dataObj[field.name] = field.value;
                }
            });

            var postData = {
                "carikart_id": 0,
                "statu": true,
                "kayit_silindi": false,
                "carikart_turu_id": 2,//personel
                "carikart_tipi_id": 21,
                "cari_unvan": dataObj['cari_unvan'],
                "ozel_kod": dataObj['ozel_kod'],
                "fiyat_tipi": "",
                "giz_yazilim_kodu": "",
                "sube_carikart_id": dataObj['sube_carikart_id'],
                "giz_kullanici_adi": dataObj['giz_kullanici_adi'],
                "giz_kullanici_sifre": dataObj['giz_kullanici_sifre'],
                "iletisim": {
                    "yetkili_ad_soyad": dataObj['yetkili_ad_soyad'],
                    "yetkili_tel": dataObj['yetkili_tel'],
                    "email": dataObj['email'],
                    "websitesi": dataObj['websitesi'],
                    "ulke_id": dataObj['ulke_id'],
                    "sehir_id": dataObj['sehir_id'],
                    "ilce_id": dataObj['ilce_id'],
                    "semt_id": dataObj['semt_id'],
                    "postakodu": dataObj['postakodu'],
                    "tel1": dataObj['tel1'],
                    "tel2": dataObj['tel2'],
                    "fax": dataObj['fax'],
                    "adres": dataObj['adres']
                },
                "parametre": {
                    "cari_parametre_1": dataObj['cari_parametre_1'],
                    "cari_parametre_2": dataObj['cari_parametre_2'],
                    "cari_parametre_3": dataObj['cari_parametre_3'],
                    "cari_parametre_4": dataObj['cari_parametre_4'],
                    "cari_parametre_5": dataObj['cari_parametre_5'],
                    "cari_parametre_6": dataObj['cari_parametre_6'],
                    "cari_parametre_7": dataObj['cari_parametre_7']
                },
                "muh_masraf": {
                    "muh_kod": dataObj['muh_kod']
                }
            }
            //var data;
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
                data: JSON.stringify(postData),
                success: function (data) {
                    //line added to save ajax response in var result
                    if (data != undefined && data.message == 'successful') {
                        genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');location = 'detail/" + data.ret_val + "'");

                        //genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');");
                        //$(location).attr('href', 'detail?id=' + data.ret_val + '');
                        //genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');location = '/personel/detail?id=" + data.ret_val + "'");

                    } else {
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                },
                error: function () {
                    alert('Error occured');
                }
            })
        });
    },
    calismaYeri: function () {
        $("#calismayeriForm").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                $("#myModal").modal("hide");
                var metodUrl = "api/personel/calismayeri";
                var dataArray = $(this).serializeArray();
                var dataObj = {};
                $(dataArray).each(function (i, field) {
                    dataObj[field.name] = field.value;
                });
                var postData = {
                    "carikart_id": dataObj["hfId"],
                    "departman_id": dataObj["departman"],
                    "gorev_id": dataObj["gorev"],
                    "stokyeri_carikart_id": dataObj["calismayeri"]
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
                    if (data != undefined && data.message == 'successful') { //Buradaki "Successful" api/modelkart/talimatlar daki apiden dönen message ın değeri.
                        genel.timer(300, 'genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$(\'#myModal\').modal(\'hide\');");personelkart.get.calismayerleri(' + carikart_id + ');');
                        //datatable refresh ediliyor
                        //talimatlarTable.reload();

                    } else {
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {

                    var errorJson = JSON.parse(jqXHR.responseText);
                    genel.timer(300, 'genel.modal("Hata!", "' + errorJson.message + '", "hata", "$(\'#myModal\').modal(\'hide\');");');

                });
            }
        });
        $("#calismayeriForm").trigger('submit');
    }
};
personelkart.put = {
    init: function () {
    },
    send: function () {
        $("#detailPersonelForm").on('submit', function (e) {
            e.preventDefault();
            var metodUrl = "api/personel/genel";

            //var formData = $(this).serialize();
            var dataArray = $(this).serializeArray();
            //console.log(dataArray);

            //CheckBox lar serializeArray ile gelmediğinden farklı bir dizi içerisine kopyalandı
            var serializedCheckbox = $('input:checkbox').map(function () {
                return { name: this.name, value: this.value == 'on' ? true : false };// ? this.value : "false"
            });

            var dataObj = {};
            $(dataArray).each(function (i, field) {
                dataObj[field.name] = field.value;
            });

            //CheckBox lar da dataArray e atanıyor.
            $(serializedCheckbox).each(function (i, field) {
                if ($.trim(field.name) != '') {
                    dataObj[field.name] = field.value;
                }
            });

            var putData = {
                "carikart_id": dataObj['carikart_id'],
                "statu": dataObj['statu'],
                "kayit_silindi": false,
                "carikart_turu_id": 2,//personel
                "carikart_tipi_id": 21,
                "cari_unvan": dataObj['cari_unvan'],
                "ozel_kod": dataObj['ozel_kod'],
                "fiyat_tipi": "",
                "giz_yazilim_kodu": "",
                "sube_carikart_id": dataObj['sube_carikart_id'],
                "giz_kullanici_adi": dataObj['giz_kullanici_adi'],
                "giz_kullanici_sifre": dataObj['giz_kullanici_sifre'],
                "iletisim": {
                    "yetkili_ad_soyad": dataObj['yetkili_ad_soyad'],
                    "yetkili_tel": dataObj['yetkili_tel'],
                    "email": dataObj['email'],
                    "websitesi": dataObj['websitesi'],
                    "ulke_id": dataObj['ulke_id'],
                    "sehir_id": dataObj['sehir_id'],
                    "ilce_id": dataObj['ilce_id'],
                    "semt_id": dataObj['semt_id'],
                    "postakodu": dataObj['postakodu'],
                    "tel1": dataObj['tel1'],
                    "tel2": dataObj['tel2'],
                    "fax": dataObj['fax'],
                    "adres": dataObj['adres']
                },
                "parametre": {
                    "cari_parametre_1": dataObj['cari_parametre_1'],
                    "cari_parametre_2": dataObj['cari_parametre_2'],
                    "cari_parametre_3": dataObj['cari_parametre_3'],
                    "cari_parametre_4": dataObj['cari_parametre_4'],
                    "cari_parametre_5": dataObj['cari_parametre_5'],
                    "cari_parametre_6": dataObj['cari_parametre_6'],
                    "cari_parametre_7": dataObj['cari_parametre_7']
                },
                "muh_masraf": {
                    "muh_kod": dataObj['muh_kod']
                }
            }
            //var data;
            $.ajax({
                async: true,
                crossDomain: true,
                url: apiUrl + metodUrl,
                method: 'PUT',
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                processData: false,
                data: JSON.stringify(putData),
                success: function (data) {
                    //line added to save ajax response in var result
                    if (data != undefined && data.message == 'successful') {
                        genel.modal("Tebrikler!", "Kayıt güncellendi", "basarili", "$('#myModal').modal('hide');");
                        //alert(dataObj['carikart_id']);
                        personelkart.get.iletisim(dataObj['carikart_id']);
                        //$(location).attr('href', 'detail?id=' + dataObj['carikart_id'] + '');
                        //genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');location = '/personel/detail?id=" + data.ret_val + "'");

                    } else {
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                },
                error: function () {
                    alert('Error occured');
                }
            })
        });
    }
};
personelkart.delete = {

    //init: function () {
    //},
    send: function () {
        $("#detailPersonelForm").on('submit', function (e) {
            e.preventDefault();



            var metodUrl = "api/personel/genel";

            //var formData = $(this).serialize();
            var dataArray = $(this).serializeArray();
            console.log(dataArray);

            //CheckBox lar serializeArray ile gelmediğinden farklı bir dizi içerisine kopyalandı
            var serializedCheckbox = $('input:checkbox').map(function () {
                return { name: this.name, value: this.value == 'on' ? true : false };// ? this.value : "false"
            });

            var dataObj = {};

            $(dataArray).each(function (i, field) {
                dataObj[field.name] = field.value;
            });

            //CheckBox lar da dataArray e atanıyor.
            $(serializedCheckbox).each(function (i, field) {
                if ($.trim(field.name) != '') {
                    dataObj[field.name] = field.value;
                }
            });

            var deleteData = {
                "carikart_id": dataObj['carikart_id'],
                "statu": true,
                "kayit_silindi": true
            }
            var data;
            $.ajax({
                async: true,
                crossDomain: true,
                url: apiUrl + metodUrl, //"http://localhost:49629/" + metodUrl,//apiUrl + metodUrl, //
                method: 'DELETE',
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                processData: false,
                data: JSON.stringify(deleteData),
                success: function (data) {
                    //line added to save ajax response in var result
                    if (data != undefined && data.message == 'successful') {
                        genel.modal("Tebrikler!", "Kayıt başarıyla pasif edilmiştir", "basarili", "$('#myModal').modal('hide');location = 'search'");
                        //$(location).attr('href', 'detail?id=' + dataObj['carikart_id'] + '');
                        //genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');location = '/personel/detail?id=" + data.ret_val + "'");

                    } else {
                        genel.modal("Hata!", "Silme işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                },
                error: function () {
                    alert('Error occured');
                }
            })
        });
    },
    calismaYeri: function (id) {
        //calismayerleriCheckBox
        if (calismaYeriCheckedValues.length > 0) {
            var metodUrl = "api/personel/calismayeri";
            var errCount = 0;
            var errorMessage = '';

            for (var i = 0; i < calismaYeriCheckedValues.length; i++) {
                $.ajax({
                    async: false,
                    crossDomain: true,
                    url: apiUrl + metodUrl,
                    method: 'DELETE',
                    headers: {
                        "content-type": "application/json",
                        "cache-control": "no-cache"
                    },
                    processData: false,
                    data: JSON.stringify(calismaYeriCheckedValues[i])
                }).success(function (data) {
                    if (data != undefined && data.message == 'successful') {
                        genel.timer(300, 'genel.modal("Kayıt Silme!", "Kayıt Silindi", "basarili", "$(\'#myModal\').modal(\'hide\');")');

                    } else {
                        errCount++;
                        errorMessage = "Silme işlemi yapılırken bir hata oluştu!";
                        genel.modal("Hata!", "Silme işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
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
            calismaYeriCheckedValues = [];
            personelkart.get.calismayerleri(id);
        }
    }
};

