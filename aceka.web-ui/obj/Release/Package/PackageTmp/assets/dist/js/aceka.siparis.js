var siparis = [];


siparis.init = function () {
    //checkbox();
};

siparis.search = {
    init: function () {
        $('.searchFrom').submit(function (e) {
            e.preventDefault();
        });
        $('.aramaSiparisBtn').on('click', function () {

            var metodUrl = "api/siparis/arama";
            var parameter = '?i==1';

            var siparis_no = $("#siparis_no").val();
            var musteri_carikart_id = $("#carikart_id").val();
            var sezon_id = $("#sezon_id").val();

            //var stok_kodu = $("#stokKodu").val();
            //var stokKodu = $("#stokKodu").attr('data-id') // -> Burası atanamıyor! sorun bura ile ilgili! autocomplate tarasında data-id işlemi olmuyor
            if ($("#stokKodu").attr('data-id') != undefined && $("#stokKodu").attr('data-id') != null) {
                var modelno = $("#stokKodu").attr('data-id');
            }
            else {
                var modelno = $("#stokKodu").val();
            }
            var modeladi = $("#stok_adi").val();
            var siparis_tarihi = $('#siparis_tarihi').val();

            //SiparisAra(string siparis_no = "", long musteri_carikart_id = 0, byte sezon_id = 0, string modelno = "", string modeladi = "", string baslangic_tarihi = "", string bitis_tarihi = "") 
            //var url = apiUrl + metodUrl + '?siparis_no=' + siparis_no + '&musteri_carikart_id=' + musteri_carikart_id + '&sezon_id=' + sezon_id + '&stok_kodu=' + stok_kodu + '&stok_adi=' + stok_adi + '&siparis_tarihi=' + siparis_tarihi;

            table = null;

            if (siparis_no.length > 0)
                parameter += '&siparis_no=' + encodeURIComponent(siparis_no);
            if (musteri_carikart_id.length > 0)
                parameter += '&musteri_carikart_id=' + encodeURIComponent(musteri_carikart_id);
            if (sezon_id.length > 0)
                parameter += '&sezon_id=' + encodeURIComponent(sezon_id);
            //if (stok_kodu.length > 0)
            //    parameter += '&stok_kodu=' + encodeURIComponent(stok_kodu);
            //if (stok_adi.length > 0)
            //    parameter += '&stok_adi=' + encodeURIComponent(stok_adi);

            if (stok_kodu.length > 0) //Modelno
                parameter += '&stok_kodu=' + encodeURIComponent(stok_kodu);
            if (modeladi.length > 0)
                parameter += '&modeladi=' + encodeURIComponent(modeladi);
            //if (stokKodu != undefined && stokKodu.length > 0)
            //    parameter += '&stokkart_id=' + encodeURIComponent(stokKodu);

            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl + parameter,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        table = $('#siparisKart').DataTable({
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
                                    data: "siparis_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" data-id="' + row.siparis_id + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "siparis_no" },
                                { data: "musteri_carikart.cari_unvan" },
                                { data: "stok_kodu" },
                                { data: "stok_adi" },
                                 {
                                     data: "siparis_tarihi",
                                     render: function (data, type, row) {
                                         if (type === 'display') {
                                             return genel.dateFormat(row.siparis_tarihi);
                                         }
                                         return data;
                                     }
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

            $('#siparisKart > tbody').on('dblclick', 'tr', function () {
                var data = table.row(this).data();
                location = '/siparis/detail/' + data.siparis_id;

            });

        });
        //$("#stokKodu").autocomplete({
        //    source: function (request, response) {

        //        var data = stokkart.autocomplate.stokAdiTurListesi({ stokkart_tur_id: 0, stok_kodu: request.term });
        //        response($.map(data, function (item) {
        //            //return { label: item.code, value: item.value, id: item.id };
        //            return { label: item.code, value: item.code, id: item.id };
        //            $("#stokKodu").attr('data-id', item.id);
        //            $("#stokKodu").val();
        //        }));
        //    },
        //    //stokKodu nun data-id sine değer atıyoruz.
        //    select: function (event, ui) {
        //        $("#stokKodu").attr('data-id', ui.item.id);
        //    },
        //    minLength: 2

        //});
        $("#stok_kodu").autocomplete({
            source: function (request, response) {
                var data = stokkart.autocomplate.stokAdiTurListesi({ stokkart_tur_id: 0, stok_kodu: request.term });
                response($.map(data, function (item) {
                    //return { label: item.code, value: item.value, id: item.id };
                    return { label: item.code, value: item.code, id: item.id };
                    $("#stok_kodu").attr('data-stokkartid', item.id);
                    $("#stok_kodu").val();
                }));
            },
            //stokKodu nun data-id sine değer atıyoruz.
            select: function (event, ui) {
                $("#stok_kodu").attr('data-stokkartid', ui.item.id);
            },
            minLength: 2

        });
        $("#stok_adi").autocomplete({
            //source: ,
            source: function (request, response) {

                var data = stokkart.autocomplate.stokAdiTurListesi({ stokkart_tur_id: 0, stok_adi: request.term, stok_kodu: '' });
                response($.map(data, function (item) {
                    //return { label: item.code, value: item.value, id: item.id };
                    return { label: item.value, value: item.value, id: item.id };
                }));
            },
            minLength: 2,
            select: function (event, ui) {
                $("#stokKodu").attr('data-id', ui.item.id);
            },

        });
        $("#siparis_no").autocomplete({
            source: function (request, response) {
                var data = siparissearch.autocomplate.SiparisNoListe({ siparis_no: request.term });
                response($.map(data, function (item) {
                    return { label: item.value, value: item.value, id: item.id };
                    $("#siparis_no").attr('data-id', item.id);

                }));

            },
            minLength: 2,
            select: function (event, ui) {
                $("#siparis_no").attr('data-id', ui.item.id);
            }
        });
        $("#modelAdi").autocomplete({
            //source: ,
            source: function (request, response) {

                var data = stokkart.autocomplate.stokAdiTurListesi({ stokkart_tur_id: 0, stok_adi: request.term, stok_kodu: '' });
                response($.map(data, function (item) {
                    return { label: item.value, value: item.code, id: item.id };
                }));
            },
            minLength: 2
        });
    }
};

siparis.get = {
    genelUst: function (id, secilecekParametreControlIds) {
        if (id > 0) {
            //api/siparis/{siparis_id}
            var metodUrl = "api/siparis/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: 'Json',
                async: false,
                success: function (data) {
                    //inputlar dolduruluyor.
                    $('#siparis_id').val(data.siparis_id);
                    $('#siparis_no').val(data.siparis_no);
                    $('#siparis_tarihi').val(genel.dateFormat(data.siparis_tarihi));
                    $('#musteri_carikart_id').val(data.musteri_carikart_id);
                    $('#siparisturu_id').val(data.siparisturu_id);
                    $('#zorlukgrubu_id').val(data.zorlukgrubu_id);
                    $('#uretimyeri_id').val(data.uretimyeri_id);
                    $('#mense_uretimyeri_id').val(data.mense_uretimyeri_id);
                    $('#siparis_not').val(data.siparis_not);
                    $('#stok_kodu').val(data.stok_kodu).attr('data-stokkartid',data.stokkart_id);
                    $('#musterifazla').val(data.musterifazla);
                    $('#siparis_id').val(data.siparis_id);
                    $('#statu').val(data.statu);


                    //siparis_ozel
                    $('#isteme_tarihi').val(genel.dateFormat(data.siparis_ozel.isteme_tarihi));
                    $('#tahmini_uretim_tarihi').val(genel.dateFormat(data.siparis_ozel.tahmini_uretim_tarihi));
                    $('#ref_siparis_no').val(data.siparis_ozel.ref_siparis_no);
                    $('#ref_siparis_no2').val(data.siparis_ozel.ref_siparis_no2);
                    $('#sezon_id').val(data.siparis_ozel.sezon_id);

                    //true ya da false döndüğü için sonuna toString() eklendi. Çünkü select'in value su bir string dir.
                    $('#' + secilecekParametreControlIds.statu).val(data.statu.toString());

                    //Log için Onay buton status == true ise butonun text i "Onay İptal" Olarak değiştiriliyor!
                    //if (data.stokkart_onay.genel_onay) {
                    //    $('#onayButton').text('Onay İptal');
                    //    $('#onayButton').attr('data-status', 'false');
                    //} else {
                    //    $('#onayButton').attr('data-status', 'true');
                    //}
                }
            });
        }
    },
    talimatlar: function (id) {
        if (id > 0) {
            //api/siparis/talimatlar/{siparis_id}
            var metodUrl = "api/siparis/talimatlar/" + id;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    if (data != null) {
                        talimatlarsiparisTable = $('#talimats').DataTable({
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
                                    data: "siparis_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" name="siparis_id" data-siparis_id="' + row.siparis_id + '" value="' + row.siparis_id + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                 { data: "sira_id" },
                                { data: "talimat_adi" },
                                { data: "cari_unvan" },
                                {data:"aciklama"},
                                { data: "irstalimat" },
                                { data: "islem_sayisi" }
                            ]

                        });
                        $('.paginate_button').on('click', function () {
                            //checkbox();
                        });

                        $('#talimats > tbody').on('dblclick', 'tr', function () {
                            var data = talimatlarsiparisTable.row(this).data();

                            var siparis_id = data.siparis_id;
                            genel.talimatGet({ siparis_id: id, event: "modelkart.put.talimat();", data: data });
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
    siparisnotlar : function (id) {
    if (id > 0) {
        //api/siparis/siparis-notlar/{siparis_id}
        var metodUrl = "api/siparis/siparis-notlar/" + id;
        $.ajax({
            type: 'GET',
            url: apiUrl + metodUrl,
            async: false,
            success: function (data) {
                if (data != null) {
                    siparisnotlarTable = $('#sipnotlar').DataTable({
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
                                data: "siparis_id",
                                render: function (data, type, row) {
                                    if (type === 'display') {
                                        return '<input type="checkbox" name="siparis_id" data-siparis_id="' + row.siparis_id + '" value="' + row.siparis_id + '" class="iCheck-helper">';
                                    }
                                    return data;
                                }
                            },
                             { data: "sira_id" },
                            {data:"aciklama"},
                         
                        ]

                    });
                    $('.paginate_button').on('click', function () {
                        //checkbox();
                    });

                    $('#talimats > tbody').on('dblclick', 'tr', function () {
                        var data = siparisnotlarTable.row(this).data();

                        var siparis_id = data.siparis_id;
                       // Sonra yapılacak. genel.talimatGet({ siparis_id: id, event: "modelkart.put.talimat();", data: data });
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
}
};
siparis.post = {
        send: function () {
            $("#siparisKart").on('submit', function (e) {
                e.preventDefault();
                var metodUrl = "api/siparis";
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

                var postData = {
                    "statu": true,
                    "siparis_no": dataObj['siparis_no'],
                    "siparis_tarihi": dataObj['siparis_tarihi'],
                    "musteri_carikart_id": dataObj['musteri_carikart_id'],
                    "uretimyeri_id": dataObj['uretimyeri_id'],
                    "mense_uretimyeri_id": dataObj['mense_uretimyeri_id'],
                    "siparis_not": dataObj['siparis_not'],
                    "stok_kodu": dataObj['stok_kodu'],
                    "musterifazla": dataObj['musterifazla'],
                    "siparisturu_id": dataObj['siparisturu_id'],
                    "zorlukgrubu_id": dataObj['zorlukgrubu_id'],

                    "siparis_ozel": {
                        "isteme_tarihi": dataObj['isteme_tarihi'],
                        "tahmini_uretim_tarihi": dataObj['tahmini_uretim_tarihi'],
                        "ref_siparis_no": dataObj['ref_siparis_no'],
                        "ref_siparis_no2": dataObj['ref_siparis_no2'],
                        "sezon_id": dataObj['sezon_id'],
                        //"stokkart_id": stokkart_id,
                    }
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

                        genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');location = '/siparis/detail/" + data.ret_val + "'");
                        /*Parametreler PUT oluyor*/
                        siparis.get.genelUst('data.ret_val');
                        //stokkartilkMadde.put.genelAltParametreler(data.ret_val);
                        //stokkartilkMadde.post.uyarilar(data.ret_val);
                    } else {
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    var errorJson = JSON.parse(jqXHR.responseText);
                    genel.timer(500, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                });
                //if ($(this).valid()) {
                   

                //}

            });
        },
    };
siparis.put = {
    send: function () {
        $("#siparisKart").on('submit', function (e) {
            e.preventDefault();

            if ($(this).valid()) {

                var metodUrl = "api/siparis";
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
                //CheckBox lar da dataArray e atanıyor.
                $(serializedCheckbox).each(function (i, field) {
                    if ($.trim(field.name) != '') {
                        dataObj[field.name] = field.value;
                    }
                });
                var siparis_id = dataObj['hfsiparis_id'];
                var postData = {
                    "siparis_id": dataObj['hfsiparis_id'],
                    "statu": dataObj['statu'],
                    "siparis_no": dataObj['siparis_no'],
                    "siparis_tarihi": dataObj['siparis_tarihi'],
                    "musteri_carikart_id": dataObj['musteri_carikart_id'],
                    "uretimyeri_id": dataObj['uretimyeri_id'],
                    "mense_uretimyeri_id": dataObj['mense_uretimyeri_id'],
                    "siparis_not": dataObj['siparis_not'],
                    "stok_kodu": dataObj['stok_kodu'],
                    "musterifazla": dataObj['musterifazla'],
                    "siparisturu_id": dataObj['siparisturu_id'],
                    "zorlukgrubu_id": dataObj['zorlukgrubu_id'],
                    "siparis_ozel": {
                        "ref_siparis_no": dataObj['ref_siparis_no'],
                        "tahmini_uretim_tarihi": dataObj['tahmini_uretim_tarihi'],
                        "isteme_tarihi": dataObj['isteme_tarihi'], //dataObj['isteme_tarihi'],
                        "ref_siparis_no2": dataObj['ref_siparis_no2'],
                        "sezon_id": dataObj['sezon_id'],
                        "stokkart_id": $('#stok_kodu').attr('data-stokkartid')
                    }
                }
                //var stokkart_id = $('#stok_kodu').attr('data-stokkartid');
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
                    data: JSON.stringify(postData)
                }).success(function (data) {
                    if (data != undefined && data.message == 'successful') {
                        genel.modal("Tebrikler!", "Kayıt güncellendi", "basarili", "$('#myModal').modal('hide');");
                        
                        siparis.get.genelUst(siparis_id);

                    } else {
                        //genel.modal("Hata!", "Güncelleme işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                        genel.timer(300, 'genel.modal("Hata!", "\'"+errorMessage +"\'", "hata", "$(\'#myModal\').modal(\'hide\');");');
                    }
                }).error(function (jqXHR, exception) {
                    var errorJson = JSON.parse(jqXHR.responseText);
                    errorMessage = errorJson.message;
                    genel.timer(300, 'genel.modal("Hata!", "\'"+errorMessage +"\'", "hata", "$(\'#myModal\').modal(\'hide\');");');
                });
            }
        });
    },
};