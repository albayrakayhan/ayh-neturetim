var stokkartilkMadde = [];
var eklerTable = null;
var eklerCheckedValues = [];
var varyantCheckedValues = [];
var benzerstkCheckedValues = [];
var fiyatlarTable = null;
var benzerTable = null;
var varyantlarTable = null;
stokkartilkMadde.init = function () {
};

stokkartilkMadde.search = {
    init: function () {

        $('#searchFrom').submit(function (e) {
            e.preventDefault();
        });

        $("#modelAdi").autocomplete({
            appendTo: document.getElementsByTagName('form'),
            source: function (request, response) {
                var data = stokkart.autocomplate.stokAdiTurListesi({ stokkart_tur_id: 2, stok_adi: encodeURIComponent(request.term), stok_kodu: '' });
                response($.map(data, function (item) {
                    return { label: item.value, value: item.value, id: item.id };
                }));
            },
            select: function (event, ui) {
                $("#modelAdi").attr('data-id', ui.item.id);
            },
            minLength: 2
        });

        $("#stokKodu").autocomplete({
            appendTo: document.getElementsByTagName('form'),
            source: function (request, response) {
                //stokkart_tur_id: 2 kumaş,aksesuar ve iplik için geçerli.
                console.log(request);
                var data = stokkart.autocomplate.stokAdiTurListesi({ stokkart_tur_id: 2, stok_kodu: encodeURIComponent(request.term) });
                response($.map(data, function (item) {
                    return { label: item.code, value: item.code, id: item.id };

                }));
            },
            minLength: 2
        });


        $("#satici_carikart_id").autocomplete({
            //source: carikart.autocomplate.varsayilanSatici(),
            source: function (request, response) {
                var data = carikart.autocomplate.varsayilanSatici();
                response($.map(data, function (item) {
                    return { id: item.id, value: item.value };
                }));
            },
            select: function (event, ui) {
                $("#satici_carikart_id").attr('data-id', ui.item.id);
            },
            minLength: 2
        });
        $("#satici_carikart_id").autocomplete({
            source: carikart.autocomplate.varsayilanSatici(),
            minLength: 2
        });

        $('.aramaBtn').on('click', function () {

            var metodUrl = "api/stokkart/arama";
            var parameter = '?1==1';

            var stokKodu = $("#stokKodu").val(); //  encodeURI($("#stokKodu").val()); $("#stokKodu").val();
            var stokAdi = $("#modelAdi").val();
            var stokKartTipi = $("#stokKartTipi").val();
            var orjNo = $('#orjinal_stok_kodu').val();

            if (stokKartTipi == null || stokKartTipi == '')
                stokKartTipi = 0;

            if (stokKodu != undefined && stokKodu.length > 0)
                parameter += '&stok_kodu=' + encodeURIComponent(stokKodu);
            if (stokAdi != undefined && stokAdi.length > 0)
                parameter += '&stok_adi=' + encodeURIComponent(stokAdi);
            if (stokKartTipi != undefined && stokKartTipi.length > 0)
                parameter += '&stokkart_tipi_id=' + encodeURIComponent(stokKartTipi);
            if (orjNo != undefined && orjNo.length > 0)
                parameter += '&orjinal_stok_kodu=' + encodeURIComponent(orjNo);


            //long stokkart_id = 0, string stok_adi = "", short stokkart_tur_id = 0, int stokkart_tipi_id = 0, string stok_kodu = "", byte stokkartturu = 0
            //var url = apiUrl + metodUrl + '?stok_adi=' + encodeURIComponent(stokAdi) + '&stok_kodu=' + encodeURIComponent(stokKodu) + '&stokkart_turu_id=' + stokKartTipi + '&orjinal_stok_kodu=' + orjNo;
            var table = null;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl + parameter,  //+ '?stok_adi=' + encodeURIComponent(stokAdi) + '&stok_kodu=' + encodeURIComponent(stokKodu) + '&stokkart_tipi_id=' + stokKartTipi + '&orjinal_stok_kodu=' + encodeURIComponent(orjNo),
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        table = $('#stokKart').DataTable({
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
                                    data: "stokkart_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" name="stokkart" data-id="' + row.stokkart_id + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "stok_kodu" },
                                { data: "stok_adi" },
                                { data: "stokkart_tipi" },
                                { data: "stokkart_turu" },
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
                                genel.iCheck('input[name="stokkart"]');
                            }
                        });
                        $('.paginate_button').on('click', function () {
                            genel.iCheck('input[name="stokkart"]');
                        });
                        $('#stokKart > tbody').on('dblclick', 'tr', function () {
                            var data = table.row(this).data();
                            if (data != 'undefined') {
                                location = '/stokkart/detail/' + data.stokkart_id;
                            }
                        });
                    }
                    else {
                        genel.modal("Dikkat!", "Kayıt bulunamadı", "uyari", "$('#myModal').modal('hide');");
                    }
                }
            });



        });

        $('.aramaBtn2').on('click', function () {

            var metodUrl = "api/stokkart/arama";
            var parameter = '?1==1';

            var stokKodu = $("#stokKodu").val(); //  encodeURI($("#stokKodu").val()); $("#stokKodu").val();
            var stokAdi = $("#modelAdi").val();
            var stokKartTipi = $("#stokKartTipi").val();
            var orjNo = $('#orjinal_stok_kodu').val();

            if (stokKartTipi == null || stokKartTipi == '')
                stokKartTipi = 0;

            if (stokKodu != undefined && stokKodu.length > 0)
                parameter += '&stok_kodu=' + encodeURIComponent(stokKodu);
            if (stokAdi != undefined && stokAdi.length > 0)
                parameter += '&stok_adi=' + encodeURIComponent(stokAdi);
            if (stokKartTipi != undefined && stokKartTipi.length > 0)
                parameter += '&stokkart_tipi_id=' + encodeURIComponent(stokKartTipi);
            if (orjNo != undefined && orjNo.length > 0)
                parameter += '&orjinal_stok_kodu=' + encodeURIComponent(orjNo);


            //long stokkart_id = 0, string stok_adi = "", short stokkart_tur_id = 0, int stokkart_tipi_id = 0, string stok_kodu = "", byte stokkartturu = 0
            //var url = apiUrl + metodUrl + '?stok_adi=' + encodeURIComponent(stokAdi) + '&stok_kodu=' + encodeURIComponent(stokKodu) + '&stokkart_turu_id=' + stokKartTipi + '&orjinal_stok_kodu=' + orjNo;
            var table = null;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl + parameter,  //+ '?stok_adi=' + encodeURIComponent(stokAdi) + '&stok_kodu=' + encodeURIComponent(stokKodu) + '&stokkart_tipi_id=' + stokKartTipi + '&orjinal_stok_kodu=' + encodeURIComponent(orjNo),
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        table = $('#stokKart').DataTable({
                            data: data,
                            paging: false,
                            destroy: true,
                            lengthChange: false,
                            searching: false,
                            ordering: true,
                            info: true,
                            autoWidth: true,
                            columns: [
                                { data: "stok_kodu" },
                                { data: "stok_adi" },
                                { data: "stokkart_tipi" },
                                { data: "stokkart_turu" },
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
                                },
                                {
                                    data: "islem",
                                    render: function (data, type, row) {
                                        return '<a class="btn btn-success" id="stokid-' + row.stokkart_id + '" onclick="stokkartilkMadde.put.AksesuarlaraEkle(' + row.stokkart_id + ');">Ekle</a>';
                                    }
                                }
                            ],
                            initComplete: function (settings, json) {
                                // genel.iCheck('input[name="stokkart"]');
                            }
                        });
                        $('.paginate_button').on('click', function () {
                            // genel.iCheck('input[name="stokkart"]');
                        });
                        $('#stokKart > tbody').on('dblclick', 'tr', function () {
                            var data = table.row(this).data();
                            console.log(data);
                            stokkartilkMadde.put.AksesuarlaraEkle(data.stokkart_id);
                        });
                    }
                    else {
                        genel.modal("Dikkat!", "Kayıt bulunamadı", "uyari", "$('#myModal').modal('hide');");
                    }
                }
            });



        });


        $("#renk_adi").autocomplete({
            source: function (request, response) {
                var data = stokkart.autocomplate.renklerListesi({ renk_adi: request.term });
                response($.map(data, function (item) {
                    return { label: item.value, value: item.value, id: item.id };
                    $("#renk_adi").attr('data-id', item.id);
                }));

            },
            minLength: 2,
            select: function (event, ui) {
                $("#renk_adi").attr('data-id', ui.item.id);
            }
        });
    }
};

stokkartilkMadde.get = {
    init: function (id) {
        $('.varyantBtn').attr("onclick", "stokkartilkMadde.get.varyantGetPopup(" + id + ")");
        $('.fileUploadButton').attr("onclick", "stokkartilkMadde.get.eklerGetPopup(" + id + ")");
        $('.fileDeleteButton').attr("onclick", "stokkartilkMadde.delete.ekler(" + id + ")");
        $('.varyantDeleteBtn').attr("onclick", "stokkartilkMadde.delete.varyant(" + id + ")");
        $('.benzerstkBtn').attr("onclick", "stokkartilkMadde.get.benzerGetPopup(" + id + ")");
        $('.benzerstkDeleteBtn').attr("onclick", "stokkartilkMadde.delete.benzerstkKartlar(" + id + ")");
        $('.fiyatstkBtn').attr("onclick", "stokkartilkMadde.get.FiyatlarGetPopup(" + id + ")");
        //$('.fiyatstkDeleteBtn').attr("onclick", "stokkartilkMadde.delete.benzerstkKartlar(" + id + ")");
    },
    genelUst: function (id, secilecekParametreControlIds) {
        if (id > 0) {
            //api/stokkart/{stokkart_id}
            var metodUrl = "api/stokkart/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: 'Json',
                async: false,
                success: function (data) {
                    //inputlar dolduruluyor.
                    $('#stokkart_id').val(data.stokkart_id);
                    $('#stokKart > #hfstokkart_tipi_id').val(data.stokkart_tipi_id);
                    $('#stokkart_tipi_id').val(data.stokkart_tipi_id);
                    $('#stok_kodu').val(data.stok_kodu);
                    $('#stok_adi').val(data.stok_adi);
                    $('#stok_adi_uzun').val(data.stokkart_ozel.stok_adi_uzun);
                    $('#orjinal_renk_kodu').val(data.stokkart_ozel.orjinal_renk_kodu);
                    $('#orjinal_renk_adi').val(data.stokkart_ozel.orjinal_renk_adi);
                    $('#orjinal_stok_kodu').val(data.stokkart_ozel.orjinal_stok_kodu);
                    $('#orjinal_stok_adi').val(data.stokkart_ozel.orjinal_stok_adi);
                    //Birim Selectleri Dolduruluyor.
                    $('#' + secilecekParametreControlIds.birim_id_1).val(data.birim_id_1);
                    $('#' + secilecekParametreControlIds.birim_id_2).val(data.birim_id_2);
                    $('#' + secilecekParametreControlIds.birim_id_3).val(data.birim_id_3);


                    if (data.birim_id_2_zorunlu) {
                        genel.iCheck($('#' + secilecekParametreControlIds.birim_id_2_zorunlu).prop('checked', true)); // Checkbox true ise check atıyoruz.
                    }
                    else {
                        genel.iCheck($('#' + secilecekParametreControlIds.birim_id_2_zorunlu).prop('checked', false));
                    }
                    if (data.birim_id_3_zorunlu) {
                        genel.iCheck($('#' + secilecekParametreControlIds.birim_id_3_zorunlu).prop('checked', true));
                    }
                    else {
                        genel.iCheck($('#' + secilecekParametreControlIds.birim_id_3_zorunlu).prop('checked', false));
                    }
                    //Kdv Selectleri Dolduruluyor.
                    $('#' + secilecekParametreControlIds.kdv_alis_id).val(data.kdv_alis_id); //Kdv Alış ın id sine değerini atıyoruz. (detay sayfasındaki #kdv_alis_id)
                    $('#' + secilecekParametreControlIds.kdv_satis_id).val(data.kdv_satis_id);

                    //window.alert('#' + secilecekParametreControlIds.statu + data.statu);
                    //true ya da false döndüğü için sonuna toString() eklendi. Çünkü select'in value su bir string dir.
                    $('#' + secilecekParametreControlIds.statu).val(data.statu.toString());

                    //talimatlar
                    $('#talimatturu_id').val(data.talimat.talimatturu_id);
                    $('#aciklama').val(data.stok_talimat.aciklama);

                    //Log için Onay buton status == true ise butonun text i "Onay İptal" Olarak değiştiriliyor!
                    if (data.stokkart_onay.genel_onay) {
                        $('#onayButton').text('Onay İptal');
                        $('#onayButton').attr('data-status', 'false');
                    } else {
                        $('#onayButton').attr('data-status', 'true');
                    }
                }
            });
        }
    },
    genelAlt: function (id) {
        if (id > 0) {
            //api/stokkart/genel-ekler/{stokkart_id}
            var metodUrl = "api/stokkart/genel-ekler/" + id;
            $.ajax({
                url: apiUrl + metodUrl,
                type: "GET",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != undefined && data != null) {
                        //eklerTable = null;
                        eklerTable = $('#imageDownload').DataTable({
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
                                    data: "ek_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<span id="span_' + row.ek_id + '" data-filePath="' + row.filepath + row.filename + '" >' + row.ekadi + '</span>';
                                        }
                                        return data;
                                    }
                                }, {
                                    data: "ekadi",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" data-id="' + row.ek_id + '" data-stokkart_id="' + row.stokkart_id + '" data-filename="' + row.filename + '" name="eklerCheckBox" value="' + row.ek_id + '" class="iCheck-helper eklerCheckBox">';
                                        }
                                        return data;
                                    }
                                },
                            ],
                            initComplete: function (settings, json) {
                                genel.iCheck('input[name="eklerCheckBox"]')
                            }
                        });
                        $('.paginate_button').on('click', function () {

                        });
                        $('#imageDownload > tbody').on('dblclick', 'tr', function () {
                            var data = eklerTable.row(this).data();

                            var filePath = data.filepath + data.filename;

                            var extension = data.filename.split('.')[1].toLowerCase();

                            switch (extension) {
                                case 'jpg':
                                case 'png':
                                case 'gif':
                                    $('.selectedImageSrc').attr('src', filePath);
                                    break;
                                default:
                                    $('.selectedImageSrc').attr('src', '~/assets/dist/img/photo2.png');
                                    break

                            }

                            $('.selectedImageHref').attr('href', filePath);

                        });
                        $('input[name="eklerCheckBox"]').on('ifChecked', function (event) {
                            var ek_id = event.target.value;
                            var stokkart_id = event.target.attributes["data-stokkart_id"].nodeValue;
                            var filename = event.target.attributes["data-filename"].nodeValue;

                            eklerCheckedValues.push({ ek_id: ek_id, stokkart_id: stokkart_id, filename: filename });

                            //alert(ek_id + ' ' + stokkart_id + ' ' + filename);
                        });
                        $('input[name="eklerCheckBox"]').on('ifUnchecked', function (event) {
                            var ek_id = event.target.value;
                            var tmpArray = [];

                            for (var item, i = 0; item = eklerCheckedValues[i++];) {
                                if (item.ek_id != ek_id) {
                                    tmpArray.push(item);
                                }
                            }

                            eklerCheckedValues = tmpArray;
                        });
                    } else {
                        if (eklerTable != null) {
                            eklerTable.destroy();
                            $('#imageDownload > tbody').html('');
                        }
                    }
                }
            });
            /*Ekler Listesi*/
        }
    },
    genelAltParametreler: function (id, secilecekParametreControlIds) {
        if (id > 0) {
            //api/modelkart/rapor-parametreler/{stokkart_id}
            var metodUrl = "api/modelkart/rapor-parametreler/" + id;

            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {

                    // if (data.stokalan_id_1 != null && data !== undefined) { 26.007.2017 de düzeltildi. AA
                    if (data.stokalan_id_1 > 0 && data !== undefined) {
                        $('#' + secilecekParametreControlIds.stokalan_id_1).val(data.stokalan_id_1);
                    }
                    if (data.stokalan_id_2 > 0 && data !== undefined) {
                        $('#' + secilecekParametreControlIds.stokalan_id_2).val(data.stokalan_id_2);
                    }
                    if (data.stokalan_id_3 > 0 && data !== undefined) {
                        $('#' + secilecekParametreControlIds.stokalan_id_3).val(data.stokalan_id_3);
                    }
                    if (data.stokalan_id_4 > 0 && data !== undefined) {
                        $('#' + secilecekParametreControlIds.stokalan_id_4).val(data.stokalan_id_4);
                    }
                    if (data.stokalan_id_5 > 0 && data !== undefined) {
                        $('#' + secilecekParametreControlIds.stokalan_id_5).val(data.stokalan_id_5);
                    }
                    if (data.stokalan_id_6 > 0 && data !== undefined) {
                        $('#' + secilecekParametreControlIds.stokalan_id_6).val(data.stokalan_id_6);
                    }
                    if (data.stokalan_id_7 > 0 && data !== undefined) {
                        $('#' + secilecekParametreControlIds.stokalan_id_7).val(data.stokalan_id_7);
                    }
                    if (data.stokalan_id_8 > 0 && data !== undefined) {
                        $('#' + secilecekParametreControlIds.stokalan_id_8).val(data.stokalan_id_8);
                    }
                    if (data.stokalan_id_9 > 0 && data !== undefined) {
                        $('#' + secilecekParametreControlIds.stokalan_id_9).val(data.stokalan_id_9);
                    }
                    if (data.uretimyeri_id > 0 && data !== undefined) {
                        $('#' + secilecekParametreControlIds.uretimyeri_id).val(data.uretimyeri_id);
                    }

                    if (data.satici_carikart_id > 0 && data !== undefined) {

                        $('#' + secilecekParametreControlIds.satici_carikart_id).val(data.satici_cari_unvan);
                        $('#' + secilecekParametreControlIds.satici_carikart_id).attr("data-id", data.satici_carikart_id);

                    }

                }
            });
        }
    },
    varyantlar: function (id) {
        if (id > 0) {
            //api/stokkart/varyant/{stokkart_id}
            var metodUrl = "api/stokkart/varyant/" + id;
            var url = apiUrl + metodUrl;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != undefined && data != null) {
                        varyantlarTable = $('#VaryantSkuKart').DataTable({
                            data: data,
                            paging: false,
                            scrollY: "250px",
                            scrollCollapse: true,
                            destroy: true,
                            searching: true,
                            ordering: true,
                            columns: [
                                {
                                    data: "sku_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" name="varyantSkuCheckBox" data-sku_id="' + row.sku_id + '" data-sku_no="' + row.sku_no + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "renk.renk_adi" },
                                { data: "renk.renk_kodu" },
                                { data: "sku_no" },
                                {
                                    data: "sku_id",
                                    render: function (data, type, row) {
                                        if (type == 'display') {
                                            return '<button class="badge modal-primary">Tanımla</button>';
                                        }
                                        return data;
                                    }
                                },
                                {
                                    data: "sku_id",
                                    render: function (data, type, row) {
                                        if (type == 'display') {
                                            return '<button class="badge modal-primary">Tanımla</button>';
                                        }
                                        return data;
                                    }
                                },
                                {
                                    data: "sku_id",
                                    render: function (data, type, row) {
                                        if (type == 'display') {
                                            return '<button class="badge modal-primary">Tanımla</button>';
                                        }
                                        return data;
                                    }
                                }
                            ],
                            language: {
                                "search": "Tabloda Ara :"
                            },
                            initComplete: function (settings, json) {
                                //var api = this.api();
                                genel.iCheck('input[name="varyantSkuCheckBox"]');
                                //$('.dataTables_filter label').eq(0).text('Tabloda Ara');
                            }, drawCallback: function () {
                                $('.paginate_button')
                                    .on('click', function () {
                                        genel.iCheck('input[name="varyantSkuCheckBox"]');
                                        var api = this.api();
                                    });
                            }
                        });

                        $('input[name="varyantSkuCheckBox"]').on('ifChecked', function (event) {
                            var sku_id = event.target.attributes["data-sku_id"].nodeValue;
                            var sku_no = event.target.attributes["data-sku_no"].nodeValue;
                            var stokkart_id = id;
                            varyantCheckedValues.push({ sku_id: sku_id, stokkart_id: stokkart_id, sku_no: sku_no });
                        });
                        $('input[name="varyantSkuCheckBox"]').on('ifUnchecked', function (event) {
                            var sku_id = event.target.attributes["data-sku_id"].nodeValue;
                            var tmpArray = [];
                            for (var item, i = 0; item = varyantCheckedValues[i++];) {
                                if (item.sku_id != sku_id) {
                                    tmpArray.push(item);
                                }
                            }
                            varyantCheckedValues = tmpArray;
                        });
                    } else {
                        if (varyantlarTable != null) {
                            varyantlarTable.destroy();
                            $('#VaryantSkuKart > tbody').html('');
                        }
                    }
                }
            });
        }
    },
    uyarilar: function (id) {
        if (id > 0) {
            //api/stokkart/uyari/{stokkart_id}
            var metodUrl = "api/stokkart/uyari/" + id;
            var url = apiUrl + metodUrl;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    if (data != null) {
                        // alert(data.eksi_stok_uyari);
                        $('#eksi_stok_uyari').prop('checked', data.eksi_stok_uyari);
                        $('#eksi_stok_izin').prop('checked', data.eksi_stok_izin);
                        $('#min_stok_uyari').prop('checked', data.min_stok_uyari);
                        $('#beden_bazli_kullanim').prop('checked', data.beden_bazli_kullanim);
                        $('#satin_alma_testi_gerekli_uyari').prop('checked', data.satin_alma_testi_gerekli_uyari);
                        $('#her_sezon_onay_gerekli').prop('checked', data.her_sezon_onay_gerekli);
                        $('#tedarik_edilemez').prop('checked', data.tedarik_edilemez);
                        $('#sezon_onayi_yok_uyarisi').prop('checked', data.sezon_onayi_yok_uyarisi);
                        //Bunu çalıştırmazsak yukarıdaki selectler gelmiyor.

                        $('.paginate_button').on('click', function () {

                        });
                    }
                }
            });
        }
    },
    fiyatlar: function (id) {
        if (id > 0) {
            //api/stokkart/fiyat/{stokkart_id}
            var metodUrl = "api/stokkart/fiyat/" + id;
            var url = apiUrl + metodUrl;
            //table = null;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    if (data != null) {
                        fiyatlarTable = $('#fiyatlarTab').DataTable({
                            data: data,
                            paging: true,
                            destroy: true,
                            lengthChange: false,
                            searching: false,
                            ordering: true,
                            info: true,
                            autoWidth: true,
                            columns: [
                                //{
                                //    data: "stokkart_id",
                                //    render: function (data, type, row) {
                                //        if (type === 'display') {
                                //            return '<input type="checkbox" data-id="' + row.stokkart_id + '" class="iCheck-helper">';
                                //        }
                                //        return data;
                                //    }
                                //},
                                { data: "fiyattipi_adi" },
                                {
                                    data: "tarih",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return genel.dateFormat(row.tarih);
                                        }
                                        return data;
                                    }
                                },
                                { data: "fiyat" },
                                { data: "pb_kodu" }
                            ],
                            initComplete: function (setting, json) {

                            }
                        });

                        $('.paginate_button').on('click', function () {

                        });
                        //Çift tıklama ile fiyat güncelleme popup ını açıyoruz.
                        $('#fiyatlarTab > tbody').on('dblclick', 'tr', function () {
                            var data = fiyatlarTable.row(this).data();

                            var stokkart_id = data.stokkart_id;
                            genel.fiyatpopGet({ stokkart_id: id, event: "stokkartilkMadde.put.fiyatlar();", data: data });
                        });

                    }
                }
            });
        }
    },
    sezonlar: function (id) {
        if (id >= 0) {
            //api/stokkart/sezon/{stokkart_id}
            var metodUrl = "api/stokkart/sezon/" + id;
            var url = apiUrl + metodUrl;
            table = null;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    if (data != null) {

                        table = $('#sezonTable').DataTable({
                            data: data,
                            paging: true,
                            destroy: true,
                            lengthChange: false,
                            searching: false,
                            ordering: true,
                            info: true,
                            autoWidth: true,
                            columns: [
                                { data: "sezon_kodu" },
                                { data: "sezon_adi" },
                                {
                                    data: "sezon_id",
                                    render: function (data, type, row) {
                                        if (type == 'display') {
                                            return '<button class="badge modal-primary">Tanımla</button>';
                                        }
                                        return data;
                                    }
                                },
                                {
                                    data: "statu",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            if (row.statu == true) {
                                                return '<input class="iCheck-helper" type="checkbox" name="sezon_id" id="sezon_id" value="' + row.sezon_id + '" checked="checked"></input>';
                                            } else {
                                                return '<input type="checkbox" id="sezon_id" name="sezon_id" value="' + row.sezon_id + '" class="iCheck-helper"></input>';
                                            }

                                        }
                                        return data;
                                    }
                                    //,className: "dt-body-center"
                                }

                            ],
                            initComplete: function (settings, json) {
                                //var api = this.api();
                                genel.iCheck('input[name="sezon_id"]');
                                //$('.dataTables_filter label').eq(0).text('Tabloda Ara');
                            }, drawCallback: function () {
                                $('.paginate_button')
                                    .on('click', function () {
                                        genel.iCheck('input[name="sezon_id"]');
                                        var api = this.api();
                                    });
                            }
                        });

                        $('.paginate_button').on('click', function () {
                        });
                    }
                }
            });
        }
    },
    onayGecmisi: function (id) {
        genel.onayGecmisiGet(id);
    },
    benzerkartlar: function (id) {
        if (id > 0) {
            //api/stokkart/muadil/{stokkart_id}
            var metodUrl = "api/stokkart/muadil/" + id;
            var url = apiUrl + metodUrl;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    if (data != null) {
                        benzerTable = $('#benzerKartlar').DataTable({
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
                                    data: "muadil_stokkart_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" name="bnzerCheckBox" data-muadil_stokkart_id="' + row.muadil_stokkart_id + '"  class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "muadil_stokkart_id" },
                                { data: "tanim" },
                            ],
                            initComplete: function (settings, json) {
                                genel.iCheck('input[name="bnzerCheckBox"]');
                            }, drawCallback: function () {
                                $('.paginate_button')
                                    .on('click', function () {
                                        genel.iCheck('input[name="bnzerCheckBox"]');
                                        var api = this.api();
                                    });
                            }
                        });
                        $('input[name="bnzerCheckBox"]').on('ifChecked', function (event) {

                            var muadil_stokkart_id = event.target.attributes["data-muadil_stokkart_id"].nodeValue;
                            var stokkart_id = id;

                            benzerstkCheckedValues.push({ muadil_stokkart_id: muadil_stokkart_id, stokkart_id: stokkart_id });

                        });
                        $('input[name="bnzerCheckBox"]').on('ifUnchecked', function (event) {
                            var muadil_stokkart_id = event.target.attributes["data-muadil_stokkart_id"].nodeValue;
                            var tmpArray = [];

                            for (var item, i = 0; item = benzerstkCheckedValues[i++];) {
                                if (item.muadil_stokkart_id != muadil_stokkart_id) {
                                    tmpArray.push(item);
                                }
                            }
                            benzerstkCheckedValues = tmpArray;
                        });
                    }
                    else {
                        if (benzerTable != null) {
                            benzerTable.destroy();
                            $('#benzerKartlar > tbody').html('');
                        }
                    }
                }
            });
        }
    },
    birimDonustur: function (id) {
        if (id > 0) {
            //api/stokkart/birim-donustur/{stokkart_id}
            var metodUrl = "api/stokkart/birim-donustur/" + id;
            var url = apiUrl + metodUrl;
            table = null;
            $.ajax({
                type: 'GET',
                url: apiUrl + metodUrl,
                async: false,
                success: function (data) {
                    $('#stokkart_id').val(data.stokkart_id);
                    $('#stokKart > #hfstokkart_tipi_id').val(data.stokkart_tipi_id);
                    $('#birim1x').val(data.birim1x);
                    $('#birim2x').val(data.birim2x);
                    $('#birim3x').val(data.birim3x);
                    $('#M2_gram').val(data.M2_gram);
                    $('#eni').val(data.eni);
                    $('#fyne').val(data.fyne);
                    $('#fire_orani').val(data.fire_orani);
                }
            });
        }
    },
    eklerGetPopup: function (id) {
        genel.fileUploadPopup({ stokkart_id: id, event: "stokkartilkMadde.post.ekler();", data: null });
    },
    varyantGetPopup: function (id) {
        genel.stokkartvaryantGet({ stokkart_id: id, event: "stokkartilkMadde.post.stokkartvaryant();$('#myModal').modal('hide');", data: null });
    },
    benzerGetPopup: function (id) {
        genel.benzerPopupGet({ stokkart_id: id, event: "stokkartilkMadde.post.benzerKartlar();$('#myModal').modal('hide');", data: null });
    },
    FiyatlarGetPopup: function (id) {
        genel.fiyatpopGet({ stokkart_id: id, event: "stokkartilkMadde.post.fiyatlar();$('#myModal').modal('hide');", data: null });
    }
};
stokkartilkMadde.post = {
    send: function () {
        $("#stokKart").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                var metodUrl = "api/stokkart";
                //var formData = $(this).serialize();
                var dataArray = $(this).serializeArray();
                //console.log(dataArray);

                //CheckBox lar serializeArray ile gelmediğinden farklı bir dizi içerisine kopyalandı
                var serializedCheckbox = $('input:checkbox').map(function () {
                    return { name: this.name, value: this.checked };// ? this.value : "false"
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
                //var tek_varyant = $('input[name=tek_varyant]').iCheck('update')[0].checked;
                var postData = {
                    "stokkart_id": null,
                    "statu": true,
                    "stokkart_tipi_id": dataObj['stokkart_tipi_id'],
                    //"stokkart_tur_id": 2,
                    "stok_kodu": dataObj['stok_kodu'],
                    "uretimyeri_id": dataObj['uretimyeri_id'],
                    "stok_adi": dataObj['stok_adi'],
                    "kdv_alis_id": dataObj['kdv_alis_id'],
                    "kdv_satis_id": dataObj['kdv_satis_id'],
                    "birim_id_1": dataObj['birim_id_1'],
                    "birim_id_2": dataObj['birim_id_2'],
                    "birim_id_3": dataObj['birim_id_3'],
                    "birim_id_2_zorunlu": dataObj['birim_id_2_zorunlu'],
                    "birim_id_3_zorunlu": dataObj['birim_id_3_zorunlu'],
                    "talimat": {
                        "talimatturu_id": dataObj['talimatturu_id'],
                        "kod": dataObj['kod']
                    },
                    "stok_talimat": {
                        "aciklama": dataObj['aciklama']
                    },
                    "stokkart_ozel": {
                        "stok_adi_uzun": dataObj['stok_adi_uzun'],
                        "orjinal_stok_kodu": dataObj['orjinal_stok_kodu'],
                        "orjinal_stok_adi": dataObj['orjinal_stok_adi'],
                        "tek_varyant": $('input[name=tek_varyant]').iCheck('update')[0].checked
                    },
                    "stokkart_onay": {
                        "genel_onay": false,
                        "malzeme_onay": false,
                        "yukleme_onay": false,
                        "uretim_onay": false
                    },
                    "stokkart_onay_log": {
                        "stokkart_id": 1,
                        "onay_alan_adi": "sample string 2",
                        "onay_tarihi": "2017-05-25",
                        "onay_carikart_id": 1,
                        "iptal_tarihi": "2017-05-25",
                        "iptal_carikart_id": 1
                    }
                    //"gizsabit_stokkarttipi": {
                    //    "stokkarttipi": 64,
                    //    "tanim": "sample string 2",
                    //    "otostokkodu": "sample string 3",
                    //    "parametre_grubu": 64,
                    //    "stokkartturu": 64
                    //},

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

                        genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');location = '/stokkart/detail/" + data.ret_val + "'");
                        /*Parametreler PUT oluyor*/
                        stokkartilkMadde.put.genelAltParametreler(data.ret_val);
                        stokkartilkMadde.post.uyarilar(data.ret_val);
                    } else {
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    var errorJson = JSON.parse(jqXHR.responseText);
                    genel.timer(500, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                });

            }
        });
    },
    ekler: function () {
        $("#eklerFrom").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                metodUrl = "api/modelkart/genel-ekler";
                var formDataArray = $(this).serializeArray();
                var formDataObj = {};
                $(formDataArray).each(function (i, field) {
                    formDataObj[field.name] = field.value;
                });
                var retObj = genel.fileUpload('dosyaSec');
                if (retObj != null) {
                    var postData = [];
                    for (var item, i = 0; item = retObj[i++];) {
                        /* fileUpload dan gelen obj un fieldları
                        ContentType
                        FileName
                        */
                        postData.push({ stokkart_id: formDataObj['hfId'], ekadi: item.FileName.split('_')[0], aciklama: formDataObj['aciklama'], filepath: genel.filePath(), filename: item.FileName, ekturu_id: formDataObj['ekturu_id'] });
                    }
                    if (postData.length > 0) {
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
                                genel.timer(300, '$(\'#myModal\').modal(\'hide\');');
                                stokkartilkMadde.get.genelAlt(formDataObj['hfId']);
                            } else {
                                genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                            }
                        }).error(function (jqXHR, exception) {
                            var errorJson = JSON.parse(jqXHR.responseText);
                            genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                        });
                    }
                }
            }
        });
        $("#eklerFrom").trigger('submit');
    },
    stokkartvaryant: function () {
        $("#varyantFrom").on('submit', function (e) {
            e.preventDefault();
            //api/stokkart/varyant/
            var metodUrl = "api/stokkart/varyant";
            var dataArray = $(this).serializeArray();

            var dataObj = {};
            $(dataArray).each(function (i, field) {
                dataObj[field.name] = field.value;
            });
            var postData = {
                "stokkart_id": dataObj["hfId"],
                "sku_no": dataObj["sku_no"],
                "renk": {
                    "renk_id": $("#renk_adi").attr('data-id')
                }
            }
            var stokkart_id = dataObj['hfId'];
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
                    genel.timer(300, 'stokkartilkMadde.get.varyantlar(' + stokkart_id + ')');
                } else {
                    genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                }
            }).error(function (jqXHR, exception) {
                genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            });

        });
        $("#varyantFrom").trigger('submit');
    },
    sezonlar: function () {
        //Sezonları silip tekrar ekliyoruz. O yüzden Sadece POST Metodu var.
        var metodUrl = "api/stokkart/sezon/";
        var dataArray = $("div.sezonArray *").serializeArray();
        var stokkart_id = $('#modelkart_id').val();
        var postData = [];
        for (var item, i = 0; item = dataArray[i++];) {
            postData.push({ stokkart_id: stokkart_id, sezon_id: item['value'] });
        }
        if (postData.length > 0) {
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
                    genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');");
                    genel.timer(300, 'stokkartilkMadde.get.sezonlar(' + stokkart_id + ')');
                } else {
                    genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                }
            }).error(function (jqXHR, exception) {
                genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                genel.timer(300, 'stokkartilkMadde.get.sezonlar(' + stokkart_id + ')');
            });
        }
    },
    fiyatlar: function (id) {
        $("#fiyatpopupForm").on('submit', function (e) {
            e.preventDefault();
            //api/stokkart/fiyat
            var metodUrl = "api/stokkart/fiyat";
            var dataArray = $(this).serializeArray();

            var dataObj = {};
            $(dataArray).each(function (i, field) {
                dataObj[field.name] = field.value;
            });
            var postData = {
                "stokkart_id": dataObj["hfId"],
                "fiyattipi": dataObj["fiyattipi"],
                "tarih": dataObj["tarih"],
                "fiyat": dataObj["fiyat"],
                "pb": dataObj["pb"],
                "pb_kodu": dataObj["pb_kodu"],
            }
            //var stokkart_id = dataObj['hfId'];
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
                    genel.timer(300, 'stokkartilkMadde.get.fiyatlar(' + dataObj["hfId"] + ')');
                } else {
                    genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                }
            }).error(function (jqXHR, exception) {
                genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            });

        });
        $("#fiyatpopupForm").trigger('submit');
    },
    benzerKartlar: function () {
        $("#benzerstkFrom").on('submit', function (e) {
            e.preventDefault();
            //api/stokkart/muadil/
            var metodUrl = "api/stokkart/muadil/";
            var dataArray = $(this).serializeArray();
            var dataObj = {};
            $(dataArray).each(function (i, field) {
                dataObj[field.name] = field.value;
            });
            var stokkart_id = $('#modelkart_id').val();
            var muadil_stokkart_id = $("#modelAdi").attr('data-id');
            var postData = {
                "stokkart_id": stokkart_id,
                "muadil_stokkart_id": muadil_stokkart_id,
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
            }).success(function (data) {
                if (data != undefined && data.message == 'successful') {
                    genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');");
                    genel.timer(300, 'stokkartilkMadde.get.benzerkartlar(' + stokkart_id + ')');
                } else {
                    genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                }
            }).error(function (jqXHR, exception) {
                genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            });

        });
        $("#benzerstkFrom").trigger('submit');

    },
    uyarilar: function (id) {
        var metodUrl = "api/stokkart/uyari";
        var dataArray = $("div.uyari *").serializeArray();
        var dataObj = {};
        $(dataArray).each(function (i, field) {
            dataObj[field.name] = field.value;
        });
        var postData = {
            "stokkart_id": id,
            "beden_bazli_kullanim": dataObj["beden_bazli_kullanim"],
            "eksi_stok_izin": dataObj["eksi_stok_izin"],
            "eksi_stok_uyari": dataObj["eksi_stok_uyari"],
            "her_sezon_onay_gerekli": dataObj["her_sezon_onay_gerekli"],
            "min_stok_uyari": dataObj["min_stok_uyari"],
            "musteri_siparisi_icin_acik": dataObj["musteri_siparisi_icin_acik"],
            "tedarik_edilemez": dataObj["tedarik_edilemez"],
            "satin_alma_testi_gerekli_uyari": dataObj["satin_alma_testi_gerekli_uyari"],
            "sezon_onayi_yok_uyarisi": dataObj["sezon_onayi_yok_uyarisi"],
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
        }).success(function (data) {
            if (data != undefined && data.message == 'successful') {
                //         genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');");
            } else {
                //       genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            }
        }).error(function (jqXHR, exception) {
            genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
        });
    }
};
stokkartilkMadde.put = {
    send: function () {
        $("#stokKart").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                var metodUrl = "api/stokkart";
                //var formData = $(this).serialize();
                var dataArray = $(this).serializeArray();
                // console.log(dataArray);
                //CheckBox lar serializeArray ile gelmediğinden farklı bir dizi içerisine kopyalandı
                var serializedCheckbox = $('input:checkbox').map(function () {
                    return { name: this.name, value: this.checked };// ? this.value : "false"
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
                var tek_varyant = $('input[name=tek_varyant]').iCheck('update')[0].checked;
                var postData = {
                    "stokkart_id": dataObj['modelkart_id'],
                    "statu": dataObj['statu'],
                    "stokkart_tipi_id": dataObj['stokkart_tipi_id'],
                    //"stokkart_tur_id": 0,
                    "stok_kodu": dataObj['stok_kodu'],
                    "uretimyeri_id": dataObj['uretimyeri_id'],
                    "stok_adi": dataObj['stok_adi'],
                    "kdv_alis_id": dataObj['kdv_alis_id'],
                    "kdv_satis_id": dataObj['kdv_satis_id'],
                    "birim_id_1": dataObj['birim_id_1'],
                    "birim_id_2": dataObj['birim_id_2'],
                    "birim_id_3": dataObj['birim_id_3'],
                    "birim_id_2_zorunlu": dataObj['birim_id_2_zorunlu'],
                    "birim_id_3_zorunlu": dataObj['birim_id_3_zorunlu'],
                    "talimat": {
                        "talimatturu_id": dataObj['talimatturu_id'],
                        "kod": dataObj['kod']
                    },
                    "stok_talimat": {
                        "aciklama": dataObj['aciklama']
                    },
                    "stokkart_ozel": {
                        "stok_adi_uzun": dataObj['stok_adi_uzun'],
                        "orjinal_stok_kodu": dataObj['orjinal_stok_kodu'],
                        "orjinal_stok_adi": dataObj['orjinal_stok_adi'],
                        "orjinal_renk_kodu": dataObj['orjinal_renk_kodu'],
                        "orjinal_renk_adi": dataObj['orjinal_renk_adi'],
                        "orjinal_renk_adi": dataObj['orjinal_renk_adi'],
                        //"birim1x": dataObj['birim1x'],
                        //"birim2x": dataObj['birim2x'],
                        //"birim3x": dataObj['birim3x'],
                        //"M2_gram": dataObj['M2_gram'],
                        //"eni": dataObj['eni'],
                        //"fyne": dataObj['fyne'],
                        //"fire_orani": dataObj['fire_orani'],
                        "tek_varyant": tek_varyant
                        //"tek_varyant": dataObj['tek_varyant'] == null ? 0 : 1
                    }
                    //,
                    //"stokkart_kontrol": { //Uyarılar PUT
                    //    "stokkart_id": dataObj['modelkart_id'],
                    //    "musteri_siparisi_icin_acik": dataObj['musteri_siparisi_icin_acik'],
                    //    "eksi_stok_izin": dataObj['eksi_stok_izin'],
                    //    "eksi_stok_uyari": dataObj['eksi_stok_uyari'],
                    //    "min_stok_uyari": dataObj['min_stok_uyari'],
                    //    "satin_alma_testi_gerekli_uyari": dataObj['satin_alma_testi_gerekli_uyari'],
                    //    "her_sezon_onay_gerekli": dataObj['her_sezon_onay_gerekli'],
                    //    "beden_bazli_kullanim": dataObj['beden_bazli_kullanim'],
                    //    "sezon_onayi_yok_uyarisi": dataObj['sezon_onayi_yok_uyarisi']
                    //}
                }

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
                        stokkartilkMadde.put.birimdonustur(dataObj['modelkart_id']);
                        stokkartilkMadde.put.genelAltParametreler(dataObj['modelkart_id']);
                        stokkartilkMadde.put.uyarilar(dataObj['modelkart_id']);

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
    //raporparametrelerinde hem POST hem de PUT var.
    genelAltParametreler: function (id) {
        var metodUrl = "api/stokkart/rapor-parametreler";
        if (id > 0) {
            var stokkart_id = id;
        }
        //alert($("#satici_carikart_id").attr('data-id'));
        //alert($("#modelkart_id").val());
        //alert($("#stokkart_id").val());
        //alert(stokkart_id);
        var postData = {
            //"stokkart_id"       : $('modelkart_id').val(),
            "uretimyeri_id": $('#uretimyeri_id').val(),
            "stokkart_id": stokkart_id > 0 ? stokkart_id : $('#modelkart_id').val(),
            "satici_carikart_id": $("#satici_carikart_id").attr('data-id'),
            "stokalan_id_1": $('#stokalan_id_1').val(),
            "stokalan_id_2": $('#stokalan_id_2').val(),
            "stokalan_id_3": $('#stokalan_id_3').val(),
            "stokalan_id_4": $('#stokalan_id_4').val(),
            "stokalan_id_5": $('#stokalan_id_5').val(),
            "stokalan_id_6": $('#stokalan_id_6').val(),
            "stokalan_id_7": $('#stokalan_id_7').val(),
            "stokalan_id_8": $('#stokalan_id_8').val(),
            "stokalan_id_9": $('#stokalan_id_9').val(),
            "stokalan_id_10": $('#stokalan_id_10').val(),

            //stokalan_id_11        : null,
            //stokalan_id_12        : null,
            //stokalan_id_13        : null,
            //stokalan_id_14        : null,
            //stokalan_id_15        : null,
            //stokalan_id_16        : null,
            //stokalan_id_17        : null,
            //stokalan_id_18        : null,
            //stokalan_id_19        : null,
            //stokalan_id_20        : null
        }
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
            if (data != undefined && data.message == 'Successful') { //Buradaki "Successful" api/modelkart/talimatlar daki apiden dönen message ın değeri.
                //genel.timer(300, 'genel.modal("Tebrikler!", "Kayıt Güncellendi", "basarili", "$(\'#myModal\').modal(\'hide\');");modelkart.get.talimatlar(' + stokkart_id + ');');


            } else {
                // genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            }
        }).error(function (jqXHR, exception) {

            var errorJson = JSON.parse(jqXHR.responseText);
            //genel.timer(300, 'genel.modal("Hata!", "' + errorJson.message + '", "hata", "$(\'#myModal\').modal(\'hide\');");');

        });
    },
    birimdonustur: function (id) {
        var metodUrl = "api/stokkart/birim-donustur";
        if (id > 0) {
            var stokkart_id = id;
        }
        //alert($("#satici_carikart_id").attr('data-id'));
        //alert($("#modelkart_id").val());
        //alert($("#stokkart_id").val());
        //alert(stokkart_id);
        var postData = {
            "stokkart_id": $('modelkart_id').val(),
            "birim1x": $('#birim1x').val(),
            "stokkart_id": stokkart_id > 0 ? stokkart_id : $('#modelkart_id').val(),
            "birim2x": $('#birim2x').val(),
            "birim3x": $('#birim3x').val(),
            "M2_gram": $('#M2_gram').val(),
            "eni": $('#eni').val(),
            "fyne": $('#fyne').val(),
            "fire_orani": $('#fire_orani').val(),
            "birim_gram": $('#birim_gram').val(),
        }
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
            if (data != undefined && data.message == 'Successful') {
                //Buradaki "Successful" api/stokkart/birim-donustur deki apiden dönen message ın değeri.
                //genel.timer(300, 'genel.modal("Tebrikler!", "Kayıt Güncellendi", "basarili", "$(\'#myModal\').modal(\'hide\');");modelkart.get.talimatlar(' + stokkart_id + ');');

            } else {
                // genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            }
        }).error(function (jqXHR, exception) {

            var errorJson = JSON.parse(jqXHR.responseText);
            //genel.timer(300, 'genel.modal("Hata!", "' + errorJson.message + '", "hata", "$(\'#myModal\').modal(\'hide\');");');

        });
    },
    fiyatlar: function (id) {
        $("#fiyatpopupForm").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                $("#myModal").modal("hide");
                //api/stokkart/fiyat
                var metodUrl = "api/stokkart/fiyat";
                var dataArray = $(this).serializeArray();
                var dataObj = {};

                $(dataArray).each(function (i, field) {
                    dataObj[field.name] = field.value;
                });

                var postData = {
                    "stokkart_id": dataObj["hfId"],
                    "fiyat": dataObj["fiyat"],
                    "fiyattipi": dataObj["fiyattipi"],
                    "pb": dataObj["pb"],
                    "tarih": dataObj["tarih"]
                }
                var stokkart_id = dataObj['hfId'];
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
                        genel.timer(300, 'genel.modal("Tebrikler!", "Kayıt Güncellendi", "basarili", "$(\'#myModal\').modal(\'hide\');");stokkartilkMadde.put.fiyatlar(' + stokkart_id + ');');
                        stokkartilkMadde.get.fiyatlar(stokkart_id);
                    } else {
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {

                    var errorJson = JSON.parse(jqXHR.responseText);
                    errorMessage = errorJson.message;
                    genel.timer(300, 'genel.modal("Hata!", "\'"+errorMessage +"\'", "hata", "$(\'#myModal\').modal(\'hide\');");');
                });

                //datatable refresh ediliyor
                // talimatlarTable.reload();
            }
        });
        $("#fiyatpopupForm").trigger('submit');
    },
    uyarilar: function (id) {
        var metodUrl = "api/stokkart/uyari";
        var dataArray = $("div.uyari *").serializeArray();
        var dataObj = {};
        $(dataArray).each(function (i, field) {
            dataObj[field.name] = field.value;
        });
        var postData = {
            "stokkart_id": id,
            "beden_bazli_kullanim": dataObj["beden_bazli_kullanim"],
            "eksi_stok_izin": dataObj["eksi_stok_izin"],
            "eksi_stok_uyari": dataObj["eksi_stok_uyari"],
            "her_sezon_onay_gerekli": dataObj["her_sezon_onay_gerekli"],
            "min_stok_uyari": dataObj["min_stok_uyari"],
            "musteri_siparisi_icin_acik": dataObj["musteri_siparisi_icin_acik"],
            "tedarik_edilemez": dataObj["tedarik_edilemez"],
            "satin_alma_testi_gerekli_uyari": dataObj["satin_alma_testi_gerekli_uyari"],
            "sezon_onayi_yok_uyarisi": dataObj["sezon_onayi_yok_uyarisi"],
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
        }).success(function (data) {
            if (data != undefined && data.message == 'successful') {
                //         genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');");
            } else {
                //       genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
            }
        }).error(function (jqXHR, exception) {
            genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
        });
    },
    //onay: function (id) {
    //    $("#genel_onay").on('click', function (e) {
    //        e.preventDefault();
    //        var metodUrl = "api/stokkart/onay";
    //        var dataArray = $(this).serializeArray();
    //        var postData = {
    //            "Stokkart_onay": {
    //            "stokkart_id": id,
    //            "genel_onay": true,
    //            "malzeme_onay": false,
    //            "yukleme_onay": false,
    //            "uretim_onay": false
    //            }
    //        }
    //        $.ajax({
    //            async: true,
    //            crossDomain: true,
    //            url: apiUrl + metodUrl,
    //            method: 'PUT',
    //            headers: {
    //                "content-type": "application/json",
    //                "cache-control": "no-cache"
    //            },
    //            processData: false,
    //            data: JSON.stringify(postData)
    //        }).success(function (data) {
    //            if (data != undefined && data.message == 'Successful') {
    //                //         genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');");
    //            } else {
    //                //       genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
    //            }
    //        }).error(function (jqXHR, exception) {
    //            genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
    //        });
    //    });
    //},
    onayGecmisi: function (buttonControl) {
        var status = $('#' + buttonControl.id).attr('data-status');
        var stokkart_id = $("#modelkart_id").val();

        var retVal = genel.onayGecmisiPut(stokkart_id, status);
        if (retVal) {
            if (status == 'true') {
                $('#' + buttonControl.id).text('Onay İptal');
                $('#' + buttonControl.id).attr('data-status', 'false');
            }
            else {
                $('#' + buttonControl.id).text('Onay');
                $('#' + buttonControl.id).attr('data-status', 'true');
            }
        } else {
            alert('error');
        }
    },
    AksesuarlaraEkle: function (stokid) {
        kendo.confirm("Eklemek istediğinize emin misiniz?").then(function () {
            kendo.alert("Evet-" + stokid);
            $("#aksesuarPopoup").data("kendoWindow").close();
        }, function () {
            kendo.alert("Hayır");
            $("#aksesuarPopoup").data("kendoWindow").close();
        });
    }
};
stokkartilkMadde.delete = {
    ekler: function (id) {
        if (eklerCheckedValues.length > 0) {
            if (genel.fileDelete(eklerCheckedValues)) {
                eklerCheckedValues = [];
                stokkartilkMadde.get.genelAlt(id);
            }
        }
    },
    varyant: function (id) {
        if (varyantCheckedValues.length > 0) {
            var metodUrl = "api/stokkart/varyant";
            var errCount = 0;
            var errorMessage = '';
            for (var i = 0; i < varyantCheckedValues.length; i++) {
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
                    data: JSON.stringify(varyantCheckedValues[i])
                }).success(function (data) {
                    if (data != undefined && data.message == 'successful') {
                        stokkartilkMadde.get.varyantlar(id);
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
            varyantCheckedValues = [];
            stokkartilkMadde.get.varyantlar(id);
        }
    },
    benzerstkKartlar: function (id) {
        if (benzerstkCheckedValues.length > 0) {
            var metodUrl = "api/stokkart/muadil";
            var errCount = 0;
            var errorMessage = '';
            for (var i = 0; i < benzerstkCheckedValues.length; i++) {
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
                    data: JSON.stringify(benzerstkCheckedValues[i])
                }).success(function (data) {
                    if (data != undefined && data.message == 'successful') {
                        //genel.timer(300, 'genel.modal("Tebrikler!", "Kayıt Silindi", "basarili", "$(\'#myModal\').modal(\'hide\');");stokkartilkMadde.get.benzerkartlar(' + id + ');');
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
            } else { }
            /*Hata olsun ya da olmasın checkboxlar uncheck ediliyor ve dizi boşaltılıyor*/
            benzerstkCheckedValues = [];
            stokkartilkMadde.get.benzerkartlar(id);
        }

    },
};

$('#tek_varyant').on('ifClicked', function (event) {
    $('#VaryantSkuKart').toggleClass('tableactive');
})