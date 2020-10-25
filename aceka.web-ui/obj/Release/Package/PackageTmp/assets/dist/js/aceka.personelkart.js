/// <reference path="C:\TFS_ENES\ack-net-uretim\aceka.web-ui\Views/Personel/Detail.cshtml" />
/// <reference path="C:\TFS_ENES\ack-net-uretim\aceka.web-ui\Views/Personel/Detail.cshtml" />
var personelkart = [];


personelkart.search = {
    init: function () {

        //$('#searchFrom').submit(function (e) {
        //    e.preventDefault();
        //});

        //$('.searchBtn').on('click', function () {



        //    $('#personelKart').on('dblclick', 'tr', function () {
        //    //var data = table.row(this).data();
        //        location = '/personel/detail';// + data.carikart_id;

        //});

        //    //$('#modelKart > tbody').on('dblclick', 'tr', function () {
        //    //    var data = table.row(this).data();
        //    //    location = '/model/detail/' + data.stokkart_id;

        //    //});

        //});


        $('.searchBtn').on('click', function () {

            //api/personel/personel-bul?carikart_id={carikart_id}&cari_unvan={cari_unvan}&ozel_kod={ozel_kod}&carikart_tipi_id={carikart_tipi_id}&statu={statu}
            var metodUrl = "api/personel/personel-arama";

            //var ozel_kod = $("#ozelKod").val();
            var carikart_id = $("#carikart_id").val();
            var cari_unvan = $("#cari_unvan").val();
            //var carikart_tipi_id = $('#orjinal_stok_kodu').val();
            var statu = $('#statu').val();

            if (statu == 'Aktif')
                statu = '&statu=1';
            else if (statu == 'Pasif')
                statu = '&statu=0';
            else statu = '';

            var carikart_tipi_id = $('#carikart_tipi_id').val();

            if (carikart_tipi_id == null || carikart_tipi_id == '')
                carikart_tipi_id = 21;

            //url: apiUrl + metodUrl + '?carikart_id=' + carikart_id + '&cari_unvan=' + cari_unvan + '&ozel_kod=' + ozel_kod + '&carikart_tipi_id=' + carikart_tipi_id + '&statu=' + statu,
            var url = apiUrl + metodUrl + '?cari_unvan=' + cari_unvan + '&carikart_tipi_id=' + carikart_tipi_id + statu;
            
            var table = null;

            $.ajax({
                type: "GET",
                //url: apiUrl + metodUrl + '?carikart_id=' + carikart_id + '&cari_unvan=' + cari_unvan + '&ozel_kod=' + ozel_kod + '&carikart_tipi_id=' + carikart_tipi_id + '&statu=' + statu,
                url : apiUrl + metodUrl + '?cari_unvan=' + cari_unvan + '&carikart_tipi_id=' + carikart_tipi_id,
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
                                            return '<input type="checkbox" data-id="' + row.personel_no + '" class="iCheck-helper">';
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
                                            if (row.statu == false) {
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
                                checkbox();
                            }
                        });

                        $('.paginate_button').on('click', function () {
                            checkbox();
                        });
                    }
                }
            });
            //#personelKart : search deki table'ın idadı.( <table id="personelKart" class="table table-bordered table-hover">)
            $('#personelKart> tbody').on('dblclick', 'tr', function () {
                var data = table.row(this).data();
                location = '/personel/detail?id=' + data.personel_no;
            });
        });
    }
};


personelkart.get = {
    genelUst: function (id, secilecekParametreControlIds) {


                    $.ajax({
                        url:  apiUrl + "api/personel/genel/" + id,//100000000969",
                        type: 'Get',
                        dataType: 'JSON',
                        success: function (data) {
                            $('#carikart_id').val(data.carikart_id);
                            $('#giz_kullanici_sifre').val(data.giz_kullanici_sifre);
                            $('#giz_kullanici_adi').val(data.giz_kullanici_adi);
                            $('#cari_unvan').val(data.cari_unvan);
                            $('#ozel_kod').val(data.ozel_kod);

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
        });
    },


    calismayerleri: function (id, secilecekParametreControlIds) {
        $.ajax({
            url: apiUrl + "api/personel/calismayeri/" + id,//100000000969",
            type: 'Get',
            dataType: 'JSON',
            success: function (data) {
                $.each(data, function (key, obj) {
                    $('#calismayerleri').append('<tr> <td align="center"> <input type="checkbox"></td>  <td> ' + obj.stokyeri_carikart_adi + ' </td> <td> ' + obj.gorev_adi + ' </td> <td> ' + obj.departman_adi + ' </td> </tr>');
                })
            }
        });
    },

    raporparametreleri: function (id, secilecekParametreControlIds) {
    $.ajax({
        url: apiUrl + "api/personel/parametre/" + id,//100000000969",
        type: 'Get',
        dataType: 'JSON',
        success: function (data) {
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
    });
}
};

personelkart.Parametreler = {
    subeler: (function () {
        var metodurl = 'api/cari/cari-liste-turu-bayi-ve-cari';
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        $('#sube_carikart_id').append('<option value="' + obj.carikart_id + '">' + obj.carikart_id + '-' + obj.cari_unvan + '</option>');
                    });
                }
            }
        });
    }),
    muhasebekodlari: (function () {
        var metodurl = 'api/cari/muhasebe-kodlari';
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        $('#muh_kod').append('<option value="' + obj.muh_kod_id + '">' + obj.muh_kod_id + '-' + obj.muh_kod_adi + '</option>');
                    });
                }
            }
        });
    }),
    cariparametreler: (function (parametreid){
        var metodurl = 'api/personel/cariparametrelistesi';
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        if (obj.parametre == parametreid) {
                            $('#cari_parametre_' + parametreid).append('<option value="' + obj.parametre_id + '">' + obj.parametre_adi + '</option>');
                        }
                    });
                }
            }
        });
    }),
    gorevler: (function (){
        var metodurl = 'api/personel/gorev-parametreler';
        var jsonData = [];
    $.ajax({
        type: "GET",
        url: apiUrl + metodurl,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {
                $.each(data, function (key, obj) {
                        $('#gorev').append('<option value="' + obj.parametre_id + '">' + obj.parametre_adi + '</option>');
                });
            }
        }
    });
    }),
    departmanlar: (function () {
        var metodurl = 'api/personel/departman-parametreler';
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        $('#departman').append('<option value="' + obj.parametre_id + '">' + obj.parametre_adi + '</option>');
                    });
                }
            }
        });
    }),
    calismayeri: (function () {
        var metodurl = 'api/personel/calismayerleri';
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        $('#calismayeri').append('<option value="' + obj.carikart_id + '">' + obj.cari_unvan + '</option>');
                    });
                }
            }
        });
    })
};




personelkart.post = {
    init: function () {
    },
    send: function () {
        $("#newForm").on('submit', function (e) {
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
                var data;
                $.ajax({
                    async: true,
                    crossDomain: true,
                    url: apiUrl + metodUrl, //"http://localhost:49629/" + metodUrl,//apiUrl + metodUrl, //
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
                            genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');location = 'detail?id=" + data.ret_val + "'");

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
    }
};
personelkart.put = {
    init: function () {
    },
    send: function () {
        $("#detailForm").on('submit', function (e) {
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

            var putData = {
                "carikart_id": dataObj['carikart_id'],
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
            var data;
            $.ajax({
                async: true,
                crossDomain: true,
                url: apiUrl + metodUrl, //"http://localhost:49629/" + metodUrl,//apiUrl + metodUrl, //
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
                        genel.modal("Tebrikler!", "Güncelleme işlemi tamamlandı", "basarili", "$('#myModal').modal('hide');location = 'detail?id=" + dataObj['carikart_id'] + "'");
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

    init: function () {
    },
    send: function () {
        $("#detailForm").on('submit', function (e) {
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
    }
};

