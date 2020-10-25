var modelkart = [];
var table = null;
var talimatlarTable = null;
var varyantlarTable = null;
var eklerTable = null;
var defaultDataTableSettings = {
    paging: false, destroy: true, lengthChange: false, searching: false, ordering: false, info: false, autoWidth: true
}
var eklerCheckedValues = [];
var talimatCheckedValues = [];
var varyantCheckedValues = [];

modelkart.init = function () {
};
modelkart.data = {
    varyantlar: [],
    talimatlar: [],
    olculer: {}
};
modelkart.helper = {
    getStokkartId() {
        return $("#stokkart_id").val();
    }
};
modelkart.search = {
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
            minLength: 2,
            select: function (event, ui) {
                $("#stokKodu").attr('data-stokkart_id', ui.item.id); // seçilen stokodunun attributene "id" değerini atıyoruz.
            },

        });

        $("#satici_carikart_id").autocomplete({
            //source: carikart.autocomplate.varsayilanSatici(),
            source: function (request, response) {
                var data = carikart.autocomplate.varsayilanSatici({});
                response($.map(data, function (item) {
                    return { id: item.id, value: item.value };
                }));
            },
            minLength: 2,
            select: function (event, ui) {
                $("#satici_carikart_id").attr('data-id', ui.item.id);
            }
        });
        $("#satici_carikart_id").autocomplete({
            source: carikart.autocomplate.varsayilanSatici(),
            minLength: 2
        });

        $('.aramaBtn').on('click', function () {

            var metodUrl = "api/modelkart/arama";
            var parameter = '?i==1';

            var stokKodu = $("#stokKodu").val();
            var stokAdi = $("#stok_adi").val();
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

            table = null;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl + parameter,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {

                        table = $('#modelKart').DataTable({
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
                                            return '<input type="checkbox" name="stokkarttur" data-id="' + row.stokkart_id + '" class="iCheck-helper ">';
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
                                genel.iCheck('input[name="stokkarttur"]');
                            }
                        });

                        $('.paginate_button').on('click', function () {
                            genel.iCheck('input[name="stokkarttur"]');
                        });
                    } else {
                        genel.modal("Dikkat!", "Kayıt bulunamadı", "uyari", "$('#myModal').modal('hide');");
                    }
                }
            });

            $('#modelKart > tbody').on('dblclick', 'tr', function () {
                var data = table.row(this).data();
                location = '/model/detail/' + data.stokkart_id;
            });
        });
    }
};
modelkart.get = {
    init: function (id) {
        // alert('Model Kart No:' + id);
        $('.talimatBtn').attr("onclick", "modelkart.get.talimatGetPopup(" + id + ")");
        $('.varyantBtn').attr("onclick", "modelkart.get.varyantGetPopup(" + id + ")");
        $('.ilkMaddeBtn').attr("onclick", "modelkart.get.ilkMaddeGetPopup(" + id + ")");
        $('.talimatDeleteBtn').attr("onclick", "modelkart.delete.talimat(" + id + ")");
        $('.varyantDeleteBtn').attr("onclick", "modelkart.delete.varyant(" + id + ")");

        if (id > 0) // düzenleme ekranında aktif olması için 
        {
            $('.fileUploadButton').attr("onclick", "modelkart.get.eklerGetPopup(" + id + ")");
            $('.fileDeleteButton').attr("onclick", "modelkart.delete.ekler(" + id + ")");
        }

    },
    genelUst: function (id, secilecekParametreControlIds) {
        if (id > 0) {
            //api/modelkart/{stokkart_id}
            var metodUrl = "api/modelkart/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        //Veri tabanındaki değerleri html elementlerine atıyoruz. Get Metodları.
                        $('#stokkart_id').val(data.stokkart_id); //veri tabanındaki stokkart_id elemanına değerini atıyoruz.
                        $('#stokkart_tipi_id').val(data.stokkart_tipi_id);
                        $('#stok_kodu').val(data.stok_kodu);
                        $('#anastokkart_id').val(data.anastokkart_id);
                        $('#stok_adi').val(data.stok_adi);
                        $('#stok_adi_uzun').val(data.stokkart_ozel.stok_adi_uzun);
                        $('#orjinal_renk_kodu').val(data.stokkart_ozel.orjinal_renk_kodu);
                        $('#orjinal_renk_adi').val(data.stokkart_ozel.orjinal_renk_adi);
                        $('#orjinal_stok_kodu').val(data.stokkart_ozel.orjinal_stok_kodu);
                        $('#orjinal_stok_adi').val(data.stokkart_ozel.orjinal_stok_adi);

                        // if (true) {
                        $('#' + secilecekParametreControlIds.birim_id_1).val(data.birim_id_1);
                        $('#' + secilecekParametreControlIds.birim_id_2).val(data.birim_id_2);
                        $('#' + secilecekParametreControlIds.birim_id_3).val(data.birim_id_3);
                        // }

                        $('#' + secilecekParametreControlIds.kdv_alis_id).val(data.kdv_alis_id); //Kdv Alış ın id sine değerini atıyoruz. (detay sayfasındaki #kdv_alis_id)
                        $('#' + secilecekParametreControlIds.kdv_satis_id).val(data.kdv_satis_id);

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

                        //window.alert('#' + secilecekParametreControlIds.statu + data.statu);
                        $('#' + secilecekParametreControlIds.statu).val(data.statu.toString()); //true ya da false döndüğü için sonuna toString() eklendi. Çünkü select'in value su bir string dir.

                        //Log için Onay buton status == true ise butonun text i "Onay İptal" Olarak değiştiriliyor!
                        if (data.stokkart_onay.genel_onay) {
                            $('#onayButton').text('Onay İptal');
                            $('#onayButton').attr('data-status', 'false');
                        } else {
                            $('#onayButton').attr('data-status', 'true');
                        }
                    }
                }
            });
        }
    },
    genelAlt: function (id) {
        if (id > 0) {
            /*Ekler Listesi*/
            //api/modelkart/genel-ekler/{stokkart_id}
            var metodUrl = "api/modelkart/genel-ekler/" + id;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
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
                                    data: "ek_id",
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

                    if (data != null && data !== undefined) {
                        $('#' + secilecekParametreControlIds.stokalan_id_1).val(data.stokalan_id_1);
                    }
                    if (data !== undefined && data.stokalan_id_2 > 0) {
                        $('#' + secilecekParametreControlIds.stokalan_id_2).val(data.stokalan_id_2);
                    }
                    if (data !== undefined && data.stokalan_id_3 > 0) {
                        $('#' + secilecekParametreControlIds.stokalan_id_3).val(data.stokalan_id_3);
                    }
                    if (data !== undefined && data.stokalan_id_4 > 0) {
                        $('#' + secilecekParametreControlIds.stokalan_id_4).val(data.stokalan_id_4);
                    }
                    if (data !== undefined && data.stokalan_id_5 > 0) {
                        $('#' + secilecekParametreControlIds.stokalan_id_5).val(data.stokalan_id_5);
                    }
                    if (data !== undefined && data.stokalan_id_6 > 0) {
                        $('#' + secilecekParametreControlIds.stokalan_id_6).val(data.stokalan_id_6);
                    }
                    if (data !== undefined && data.stokalan_id_7 > 0) {
                        $('#' + secilecekParametreControlIds.stokalan_id_7).val(data.stokalan_id_7);
                    }
                    if (data !== undefined && data.stokalan_id_8 > 0) {
                        $('#' + secilecekParametreControlIds.stokalan_id_8).val(data.stokalan_id_8);
                    }
                    if (data !== undefined && data.stokalan_id_9 > 0) {
                        $('#' + secilecekParametreControlIds.stokalan_id_9).val(data.stokalan_id_9);
                    }
                    if (data !== undefined && data.uretimyeri_id > 0) {
                        $('#' + secilecekParametreControlIds.uretimyeri_id).val(data.uretimyeri_id);
                    }

                    if (data !== undefined && data.satici_carikart_id > 0) {

                        $('#' + secilecekParametreControlIds.satici_carikart_id).val(data.satici_cari_unvan);
                        $('#' + secilecekParametreControlIds.satici_carikart_id).attr("data-id", data.satici_carikart_id);

                    }

                }
            });
        }
    },
    varyantlar: function (id) {
        if (id > 0) {
            //api/modelkart/beden/{stokkart_id}
            var metodUrl = "api/modelkart/beden/" + id;
            var url = apiUrl + metodUrl;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != undefined && data != null) {

                        modelkart.data.varyantlar[id] = data;

                        varyantlarTable = $('#VaryantKart').DataTable({
                            data: data,
                            paging: false,
                            scrollY: "250px",
                            scrollCollapse: true,
                            destroy: true,
                            searching: true,
                            ordering: true,
                            columns: [
                                {
                                    data: "stokkart_id",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            return '<input type="checkbox" name="varyantCheckBox" data-sku_id="' + row.sku_id + '" data-sku_no="' + row.sku_no + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "beden_tanimi" },
                                { data: "bedengrubu" },
                                { data: "sku_no" },//Button 1
                                {
                                    data: "sku_no",
                                    render: function (data, type, row) {
                                        if (type == 'display') {
                                            return '<button type="button" class="btn btn-default btn-sm fright m-right-10">Tanımla</button>';
                                        }
                                        return data;

                                    }
                                },
                                {
                                    data: "sku_no",
                                    render: function (data, type, row) {
                                        if (type == 'display') {
                                            return '<button type="button" class="btn btn-default btn-sm fright m-right-10">Tanımla</button>';
                                        }
                                        return data;

                                    }
                                }
                                //Aktif Pasif Ekleme{
                                //    data: "statu",
                                //    render: function (data, type, row) {
                                //        if (type === 'display') {
                                //            if (row.statu == false) {
                                //                return '<span class="badge bg-red">Pasif</span>';
                                //            } else {
                                //                return '<span class="badge bg-green">Aktif</span>';
                                //            }
                                //        }
                                //        return data;
                                //    }
                                //    //,className: "dt-body-center"
                                //}
                            ],
                            language: {
                                "search": "Tabloda Ara :"
                            },
                            initComplete: function (settings, json) {
                                //var api = this.api();
                                genel.iCheck('input[name="varyantCheckBox"]');
                                //$('.dataTables_filter label').eq(0).text('Tabloda Ara');
                            }, drawCallback: function () {
                                $('.paginate_button')
                                    .on('click', function () {
                                        genel.iCheck('input[name="varyantCheckBox"]');
                                        var api = this.api();
                                    });
                            }
                        });

                        $('input[name="varyantCheckBox"]').on('ifChecked', function (event) {
                            var sku_id = event.target.attributes["data-sku_id"].nodeValue;
                            var sku_no = event.target.attributes["data-sku_no"].nodeValue;
                            varyantCheckedValues.push({ sku_id: sku_id, sku_no: sku_no });
                        });
                        $('input[name="varyantCheckBox"]').on('ifUnchecked', function (event) {
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
                            $('#VaryantKart > tbody').html('');
                        }
                    }
                }
            });
        }
    },
    //Kumaşlar tab'inin bilgilerini getiriyor.
    ilkMaddeModel: function (id, stokkart_tipi_id) {
        var result = [];
        if (id > 0) {
            // api/modelkart/ilk-madde-modeller-pivot/{stokkart_id},{stokkart_tipi_id}
            var metodUrl = "api/modelkart/ilk-madde-modeller-pivot/" + id + ',' + stokkart_tipi_id;
            var url = apiUrl + metodUrl;
            var stokkartTipi = "";
            switch (stokkart_tipi_id) {
                case 20:
                    stokkartTipi = "Kumaş"
                    break;
                case 21:
                    stokkartTipi = "Aksesuar"
                    break;
                case 22:
                    stokkartTipi = "İplik"
                    break;
            }
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        result = data;
                    }
                }
            });
        }
        return result;
    },

    ilkMadde: function (id, stokkart_tipi_id, tableName) {
        var result = [];
        if (id > 0) {
            // api/modelkart/ilk-madde-modeller-pivot/{stokkart_id},{stokkart_tipi_id}
            var metodUrl = "api/modelkart/ilk-madde-modeller-pivot/" + id + ',' + stokkart_tipi_id;
            var url = apiUrl + metodUrl;
            var stokkartTipi = "";
            switch (stokkart_tipi_id) {
                case 20:
                    stokkartTipi = "Kumaş"
                    break;
                case 21:
                    stokkartTipi = "Aksesuar"
                    break;
                case 22:
                    stokkartTipi = "İplik"
                    break;
            }
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        var dataSetIlkMadde = [];
                        $.each(data, function (key, value) {
                            var obj = {};
                            obj["Talimat Türü"] = value.talimat_tanim;
                            obj["Model Yeri"] = value.modelyeri;
                            obj["Açıklama"] = value.aciklama;
                            obj["İlk Madde"] = stokkartTipi;
                            obj["Renk"] = value.renk_adi;
                            obj["Birim"] = value.birim_adi;

                            $.each(value.pivotMatrixData, function (pivotKey, pivotVal) {
                                obj[' ' + pivotVal.name] = pivotVal.value != null ? pivotVal.value : 0;
                            });
                            dataSetIlkMadde.push(obj);
                            obj = null;
                        });
                        var my_columns = [];
                        $.each(dataSetIlkMadde[0], function (key, value) {
                            var my_item = {};
                            my_item.data = key;
                            my_item.title = key;
                            my_columns.push(my_item);
                        });
                        /*TextBox lar için render array i oluşturuluyor.*/
                        var dataColumnDefs = [];
                        var dataColumnDefCounter = 6; // Beden Matrix [6] dan itibaren başladığından "targets" 6 dan başlatılıyor
                        $.each(data[0].pivotMatrixData, function (pivotKey, pivotVal) {
                            objColumnDef = {
                                "targets": dataColumnDefCounter,
                                "data": pivotVal.name,
                                "render": function (data, type, full, meta) {
                                    return '<input type="text" style="width:30px;text-align:right;" onclick="$(this).select()" value="' + data + '" disabled="" />';
                                }
                            };
                            dataColumnDefCounter++;
                            dataColumnDefs.push(objColumnDef);
                            objColumnDef = null;
                        });
                        /*Table Basılıyor*/
                        $('#' + tableName).DataTable({
                            data: dataSetIlkMadde,
                            paging: false,
                            searching: false,
                            ordering: false,
                            "columns": my_columns,
                            "columnDefs": dataColumnDefs
                        });
                    }
                }
            });
        }
    },
    talimatlar: function (id) {
        if (id > 0) {
            //api/modelkart/talimatlar/{stokkart_id}
            var metodUrl = "api/modelkart/talimatlar/" + id;
            var url = apiUrl + metodUrl;
            // talimatlarTable = null;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {
                    modelkart.data.talimatlar[id] = data;
                    if (data != undefined && data != null) {
                        talimatlarTable = $('#talimatlar').DataTable({
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
                                            return '<input type="checkbox" name="talimatCheckBox" data-stokkart_id="' + row.stokkart_id + '" data-sira_id="' + row.sira_id + '" value="' + row.stokkart_id + '" class="iCheck-helper">';
                                        }
                                        return data;
                                    }
                                },
                                { data: "sira_id" },
                                {
                                    data: "talimat_adi",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            //return '<input type="text" name="talimatturu_id" data-talimatturu_id="' + row.talimatturu_id + '" value="' + row.talimat_adi + '>' + data + '</input>';
                                            return '<label style="width:8px;text-align:right;font-weight:100;" name="talimatturu_id" id="talimatturu_id" data-talimatturu_id="' + row.talimatturu_id + '" >' + data + '</label>';
                                        }
                                        return data;
                                    }
                                },
                                {
                                    data: "cari_unvan",
                                    render: function (data, type, row) {
                                        if (type === 'display') {
                                            //return '<input type="hidden" name="fasoncu_carikart_id" data-fasoncu_carikart_id="' + row.fasoncu_carikart_id + '" value="' + row.fasoncu_carikart_id + '>';
                                            return '<label style="width:8px;text-align:right;font-weight:100;" name="fasoncu_carikart_id" id="fasoncu_carikart_id" data-fasoncu_carikart_id="' + row.fasoncu_carikart_id + '" >' + data + '</label>';
                                        }
                                        return data;
                                    }
                                },
                                //{ data: "cari_unvan" },fasoncu adını almak için api ye ekledik.AA
                                { data: "aciklama" },
                                { data: "irstalimat" },
                                { data: "islem_sayisi" },
                            ],
                            initComplete: function (settings, json) {
                                genel.iCheck('input[name="talimatCheckBox"]');
                            }
                        });
                        $('.paginate_button').on('click', function () {
                        });
                        $('#talimatlar > tbody').on('dblclick', 'tr', function () {
                            var data = talimatlarTable.row(this).data();
                            var stokkart_id = data.stokkart_id;
                            genel.talimatGet({ stokkart_id: id, event: "modelkart.put.talimat();", data: data });
                        });
                        $('input[name="talimatCheckBox"]').on('ifChecked', function (event) {
                            var stokkart_id = event.target.value;
                            var sira_id = event.target.attributes["data-sira_id"].nodeValue;
                            talimatCheckedValues.push({ stokkart_id: stokkart_id, sira_id: sira_id });
                        });
                        $('input[name="talimatCheckBox"]').on('ifUnchecked', function (event) {
                            var stokkart_id = event.target.value;
                            var sira_id = event.target.attributes["data-sira_id"].nodeValue;
                            var tmpArray = [];
                            for (var item, i = 0; item = talimatCheckedValues[i++];) {
                                if (item.sira_id != sira_id && item.stokkart_id == stokkart_id) {
                                    tmpArray.push(item);
                                }
                            }
                            talimatCheckedValues = tmpArray;
                        });
                    } else {
                        //alert($('table#talimatlar tr:last').index() + 1); son 'tr' göre index ini alıyor.
                        if (talimatlarTable != null && ($('table#talimatlar tr:last').index() + 1) > 1) {
                            //alert($('#talimatlar tr').length > 0);
                            //alert($('table#talimatlar tr:last').index() + 1);
                            talimatlarTable.destroy();
                            $('#talimatlar > tbody').html('');
                        }
                    }
                }
            });
        }
    },
    fiyatlar: function (id) {
        if (id > 0) {
            //api/modelkart/fiyat/{stokkart_id}
            var metodUrl = "api/modelkart/fiyat/" + id;
            var url = apiUrl + metodUrl;
            table = null;
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        table = $('#fiyatlar').DataTable({
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
                                {
                                    data: "tarih",
                                    render: function (data, type, row) {
                                        if (type == 'display') {
                                            return genel.dateFormat(row.tarih);
                                        }
                                        return data;
                                    }
                                },
                                {
                                    data: "fiyat",
                                    render: function (data, type, row) {

                                        if (type == 'display') {
                                            if (row.fiyattipi == 'AF') {
                                                return row.fiyat
                                            } else {
                                                return '-'
                                            }
                                        }

                                        return data;
                                    }
                                },
                                {
                                    data: "fiyat",

                                    render: function (data, type, row) {

                                        if (type == 'display') {
                                            if (row.fiyattipi == 'SF') {
                                                return row.fiyat
                                            } else {
                                                return '-'
                                            }
                                        }

                                        return data;
                                    }
                                },
                                //{ data: "pb_kodu" },
                            ],
                            initComplete: function (settings, json) {

                            }
                        });
                        $('.paginate_button').on('click', function () {

                        });
                    }
                }
            });
        }
    },
    fiyatlarTalimatlar: function (id) {

        if (id > 0) {
            //api/modelkart/fiyat-talimatlar/{stokkart_id}
            var metodUrl = 'api/modelkart/fiyat-talimatlar/' + id;
            //var url = apiUrl + metodUrl;
            //        table
            //            .tables()
            //.header("Ayahn")
            //.to$()
            //.addClass('highlight');
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data != null) {
                        table = $('#talimatfiyat').DataTable({
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
                                    data: "tarih",
                                    render: function (data, type, row) {
                                        if (type == 'display') {
                                            return genel.dateFormat(row.tarih);
                                        }
                                        return data;
                                    }
                                },
                                {
                                    data: "talimat",

                                },
                                {
                                    data: "fiyat",
                                },

                            ],
                            initComplete: function (settings, json) {

                            }
                        });
                        $('.paginate_button').on('click', function () {

                        });
                    }
                }
            });

        }
    },
    olculer: function (id, successCallback) {
        if (id > 0) {
            // api/modelkart/olculer-pivot/{stokkart_id}
            var metodUrl = "api/modelkart/olculer-pivot/" + id;
            var url = apiUrl + metodUrl;

            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                dataType: "json",
                async: false,
                success: function (data) {
                    modelkart.data.olculer[id] = data;
                },
                error: function () {
                    console.log("Hata oluştu");
                }
            });

            return modelkart.data.olculer[id];
        }
    },
    fileUpload: function (id) {
        //genel.fileUploadPopup(id);
    },
    onayGecmisi: function (id) {
        genel.onayGecmisiGet(id);
    },
    varyantGetPopup: function (id) {
        genel.varyantGet({ stokkart_id: id, event: "modelkart.post.varyant();$('#myModal').modal('hide');", data: null });

        var getBedenGruplari = parametre.bedenler.bedenGrubuListesiData();
        if (getBedenGruplari != undefined && getBedenGruplari != null && getBedenGruplari.length > 0) {

            $.each(getBedenGruplari, function (key, obj) {
                $('.bedenGruplari').append('<li><label for="' + obj.bedengrubu + '">' + obj.bedengrubu + '</label><input type="checkbox" class="form-control bedenGruplariItem" value="' + obj.bedengrubu + '" name="' + obj.bedengrubu + '" id="' + obj.bedengrubu + '" /></li>')
            });

            genel.iCheck('.bedenGruplariItem');

            $('.bedenGruplariItem').on('ifChecked', function (event) {

                varyantTemplate();
            });

            $('.bedenGruplariItem').on('ifUnchecked', function (event) {

                varyantTemplate();
            });
        }
    },
    ilkMaddeGetPopup: function (id) {
        genel.ilkMaddeGet({ stokkart_id: id, event: "$('#myModal').modal('hide');", data: null });
    },
    talimatGetPopup: function (id) {
        genel.talimatGet({ stokkart_id: id, event: "modelkart.post.talimat();", data: null });
    },
    eklerGetPopup: function (id) {
        genel.fileUploadPopup({ stokkart_id: id, event: "modelkart.post.ekler();", data: null });
    }
};
modelkart.post = {
    init: function () {
    },
    send: function () {
        $("#modelKart").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                var metodUrl = "api/modelkart";
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

                var postData = {
                    "stokkart_id": null,
                    "statu": true,
                    "stokkart_tipi_id": dataObj['stokkart_tipi_id'],
                    //"stokkart_tur_id": 0,
                    "stok_kodu": dataObj['stok_kodu'],
                    "stok_adi": dataObj['stok_adi'],
                    "kdv_alis_id": dataObj['kdv_alis_id'],
                    "kdv_satis_id": dataObj['kdv_satis_id'],
                    "birim_id_1": dataObj['birim_id_1'],
                    "birim_id_2": dataObj['birim_id_2'],
                    "birim_id_2_zorunlu": dataObj['birim_id_2_zorunlu'],
                    "birim_id_3": dataObj['birim_id_3'],
                    "birim_id_3_zorunlu": dataObj['birim_id_3_zorunlu'],
                    "talimat": {
                        "talimatturu_id": 1,
                        "kod": "sample string 2"
                    },
                    "stok_talimat": {
                        "aciklama": "sample string 1"
                    },
                    "stokkart_ozel": {
                        "stok_adi_uzun": dataObj['stok_adi_uzun'],
                        "orjinal_stok_kodu": dataObj['orjinal_stok_kodu'],
                        "orjinal_stok_adi": dataObj['orjinal_stok_adi'],
                        "orjinal_renk_kodu": dataObj['orjinal_renk_kodu'],
                        "orjinal_renk_adi": dataObj['orjinal_renk_adi'],
                        "tek_varyant": null
                    },
                    "stokkart_onay": {
                        "genel_onay": true,
                        "malzeme_onay": true,
                        "yukleme_onay": true,
                        "uretim_onay": true
                    },
                    "stokkart_onay_log": {
                        "stokkart_id": 1,
                        "onay_alan_adi": "sample string 2",
                        "onay_tarihi": "2017-05-25T21:09:35.0866699+03:00",
                        "onay_carikart_id": 1,
                        "iptal_tarihi": "2017-05-25T21:09:35.0866699+03:00",
                        "iptal_carikart_id": 1
                    },
                    "gizsabit_stokkarttipi": {
                        "stokkarttipi": 64,
                        "tanim": "sample string 2",
                        "otostokkodu": "sample string 3",
                        "parametre_grubu": 64,
                        "stokkartturu": 64
                    }
                }

                $.ajax({
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

                        genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');location = '/model/detail/" + data.ret_val + "'");
                        /*Parametreler POST oluyor*/
                        modelkart.post.genelAltParametreler(data.ret_val);
                        localStorage.clear();

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
    varyant: function () {
        $("#varyantFrom").on('submit', function (e) {
            e.preventDefault();

            var metodUrl = "api/modelkart/beden";

            var dataArray = $('.bedenIdList *').serializeArray();
            var dataObj = {};

            var parametre_bedenler = [];
            $(dataArray).each(function (i, field) {
                parametre_bedenler.push({ "beden_id": field.value });
            });

            if (parametre_bedenler.length > 0) {
                var stokkart_id = $('#modelkart_id').val();

                var postData = {
                    "sku_no": $("#varyantFrom input[name='sku_no']").val(),
                    "stokkart_id": stokkart_id,
                    "parametre_bedenler": parametre_bedenler
                };

                $.ajax({
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

                    genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$('#myModal').modal('hide');");
                    genel.timer(300, 'modelkart.get.varyantlar(' + stokkart_id + ')');

                }).error(function (jqXHR, exception) {
                    genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                });
            }
        });

        $("#varyantFrom").trigger('submit');
    },
    talimat: function () {
        /*stokakrt_id ve sira_id unique alanlar. bir stokkart_id ye ait iki adet sira_id olamaz.*/
        $("#talimatForm").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                $("#myModal").modal("hide");
                var metodUrl = "api/modelkart/talimatlar";

                //var formData = $(this).serialize();
                var dataArray = $(this).serializeArray();
                var dataObj = {};
                $(dataArray).each(function (i, field) {
                    dataObj[field.name] = field.value;
                });
                var postData = {
                    "stokkart_id": dataObj["hfId"],
                    "sira_id": dataObj["sira_id"],
                    "talimatturu_id": dataObj["talimatturu_id1"],
                    "fasoncu_carikart_id": dataObj["fasoncu_carikart_id1"],
                    "islem_sayisi": dataObj["islem_sayisi"],
                    "aciklama": dataObj["aciklama"],
                    "irstalimat": dataObj["irstalimat"]
                }

                var stokkart_id = dataObj['hfId'];

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
                    if (data != undefined && data.message == 'Successful') { //Buradaki "Successful" api/modelkart/talimatlar daki apiden dönen message ın değeri.
                        genel.timer(300, 'genel.modal("Tebrikler!", "Yeni kayıt oluşturuldu", "basarili", "$(\'#myModal\').modal(\'hide\');");modelkart.get.talimatlar(' + stokkart_id + ');');
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
        $("#talimatForm").trigger('submit');
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
                                modelkart.get.genelAlt(formDataObj['hfId'])
                                //genel.modal("Tebrikler!", "Dosya yüklendi", "basarili", "$('#myModal').modal('hide');");

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
    genelAltParametreler: function (id) {
        var metodUrl = "api/modelkart/rapor-parametreler";
        var postData = {
            "stokkart_id": id,
            "satici_carikart_id": $("#satici_carikart_id").attr('data-id'),
            "uretimyeri_id": $('#uretimyeri_id').val(),
            "stokalan_id_1": $("#stokalan_id_1").val(),
            "stokalan_id_2": $("#stokalan_id_2").val(),
            "stokalan_id_3": $("#stokalan_id_3").val(),
            "stokalan_id_4": $("#stokalan_id_4").val(),
            "stokalan_id_5": $("#stokalan_id_5").val(),
            "stokalan_id_6": $("#stokalan_id_6").val(),
            "stokalan_id_7": $("#stokalan_id_7").val(),
            "stokalan_id_8": $("#stokalan_id_8").val(),
            "stokalan_id_9": $("#stokalan_id_9").val(),
            "stokalan_id_10": $("#stokalan_id_10").val()
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
};
modelkart.put = {
    send: function () {
        $("#modelKart").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {

                var metodUrl = "api/modelkart";

                //var formData = $(this).serialize();
                var dataArray = $(this).serializeArray();
                console.log(dataArray);

                //CheckBox lar serializeArray ile gelmediğinden farklı bir dizi içerisine kopyalandı
                var serializedCheckbox = $('input:checkbox').map(function () {
                    return { name: this.name, value: this.checked };
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
                    "stokkart_id": dataObj['modelkart_id'],
                    "statu": dataObj['statu'],
                    "stokkart_tipi_id": dataObj['stokkart_tipi_id'],
                    //"stokkart_tur_id": 0,
                    "stok_kodu": dataObj['stok_kodu'],
                    "stok_adi": dataObj['stok_adi'],
                    "kdv_alis_id": dataObj['kdv_alis_id'],
                    "kdv_satis_id": dataObj['kdv_satis_id'],
                    "birim_id_1": dataObj['birim_id_1'],
                    "birim_id_2": dataObj['birim_id_2'],
                    "birim_id_3": dataObj['birim_id_3'],
                    "birim_id_2_zorunlu": dataObj['birim_id_2_zorunlu'],
                    "birim_id_3_zorunlu": dataObj['birim_id_3_zorunlu'],
                    "talimat": {
                        "talimatturu_id": 0,
                        "kod": "Back End tarafında kapatılmış"
                    },
                    "stok_talimat": {
                        "aciklama": "yapılacak!"
                    },
                    "stokkart_ozel": {
                        "stok_adi_uzun": dataObj['stok_adi_uzun'],
                        "orjinal_stok_kodu": dataObj['orjinal_stok_kodu'],
                        "orjinal_stok_adi": dataObj['orjinal_stok_adi'],
                        "orjinal_renk_kodu": dataObj['orjinal_renk_kodu'],
                        "orjinal_renk_adi": dataObj['orjinal_renk_adi'],
                        "tek_varyant": null
                    }
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

                        /*Parametreler PUT oluyor*/
                        modelkart.put.genelAltParametreler();
                        modelkart.get.genelUst(modelkart.helper.getStokkartId(),
                            {
                                birim_id_1: 'birim_id_1',
                                birim_id_2: 'birim_id_2',
                                birim_id_3: 'birim_id_3',
                                kdv_alis_id: 'kdv_alis_id',
                                kdv_satis_id: 'kdv_satis_id',  //Kdv Alış select 'tinin seçili değerini getiriyoruz.
                                birim_id_2_zorunlu: 'birim_id_2_zorunlu',
                                birim_id_3_zorunlu: 'birim_id_3_zorunlu',
                                statu: 'statu',
                                stokkart_tipi_id: 'stokkart_tipi_id',
                                anastokkart_id: 'anastokkart_id',


                            })

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
    varyant: function () {
        $("#varyantFrom").on('submit', function (e) {
            e.preventDefault();
            var metodUrl = "api/modelkart/beden";
            var dataArray = $(this).serializeArray();
            var dataObj = {};
            $(dataArray).each(function (i, field) {
                dataObj[field.name] = field.value;
            });
            var postData = {
                "sku_no": dataObj["sku_no"],
                "stokkart_id": dataObj["hfId"],
                "parametre_bedenler": [
                    {
                        "beden_id": dataObj["beden_id"],
                    }
                ]
            }
            var stokkart_id = dataObj['hfId'];
        });
        $("#varyantFrom").trigger('submit');
    },
    talimat: function () {
        /*stokkart_id ve sira_id unique alanlar. bir stokkart_id ye ait iki adet sira_id olamaz.*/
        $("#talimatForm").on('submit', function (e) {
            e.preventDefault();
            if ($(this).valid()) {
                $("#myModal").modal("hide");
                var metodUrl = "api/modelkart/talimatlar";
                //var formData = $(this).serialize();
                var dataArray = $(this).serializeArray();
                // console.log(dataArray);
                var dataObj = {};
                $(dataArray).each(function (i, field) {
                    dataObj[field.name] = field.value;
                });
                var postData = {
                    "stokkart_id": dataObj["hfId"],
                    "sira_id": dataObj["sira_id"],
                    "talimatturu_id": dataObj["talimatturu_id1"],
                    "fasoncu_carikart_id": dataObj["fasoncu_carikart_id1"],
                    "islem_sayisi": dataObj["islem_sayisi"],
                    "aciklama": dataObj["aciklama"],
                    "irstalimat": dataObj["irstalimat"],
                    "eski_sira_id": dataObj["eski_sira_id"]
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
                    if (data != undefined && data.message == 'Successful') { //Buradaki "Successful" api/modelkart/talimatlar daki apiden dönen message ın değeri.
                        genel.timer(300, 'genel.modal("Tebrikler!", "Kayıt Güncellendi", "basarili", "$(\'#myModal\').modal(\'hide\');");modelkart.get.talimatlar(' + stokkart_id + ');');
                    } else {
                        genel.modal("Hata!", "Kayıt işlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");
                    }
                }).error(function (jqXHR, exception) {
                    var errorJson = JSON.parse(jqXHR.responseText);
                    genel.timer(300, 'genel.modal("Hata!", "' + errorJson.message + '", "hata", "$(\'#myModal\').modal(\'hide\');");');

                });
                //datatable refresh ediliyor
                // talimatlarTable.reload();
            }
        });
        $("#talimatForm").trigger('submit');
    },
    genelAltParametreler: function () {
        var metodUrl = "api/modelkart/rapor-parametreler";
        var postData = {
            "stokkart_id": $("#modelkart_id").val(),
            "satici_carikart_id": $("#satici_carikart_id").attr('data-id'),
            "uretimyeri_id": $('#uretimyeri_id').val(),
            "stokalan_id_1": $("#stokalan_id_1").val(),
            "stokalan_id_2": $("#stokalan_id_2").val(),
            "stokalan_id_3": $("#stokalan_id_3").val(),
            "stokalan_id_4": $("#stokalan_id_4").val(),
            "stokalan_id_5": $("#stokalan_id_5").val(),
            "stokalan_id_6": $("#stokalan_id_6").val(),
            "stokalan_id_7": $("#stokalan_id_7").val(),
            "stokalan_id_8": $("#stokalan_id_8").val(),
            "stokalan_id_9": $("#stokalan_id_9").val(),
            "stokalan_id_10": $("#stokalan_id_10").val()
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
    }
};
modelkart.delete = {
    ekler: function (id) {
        if (eklerCheckedValues.length > 0) {

            if (genel.fileDelete(eklerCheckedValues)) {
                eklerCheckedValues = [];
                modelkart.get.genelAlt(id);
            }
        }
    },
    talimat: function (id) {
        if (talimatCheckedValues.length > 0) {

            var metodUrl = "api/modelkart/talimatlar";
            var errCount = 0;
            var errorMessage = '';

            for (var i = 0; i < talimatCheckedValues.length; i++) {
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
                    data: JSON.stringify(talimatCheckedValues[i])
                }).success(function (data) {
                    if (data != undefined && data.message == 'Successful') { //Buradaki "Successful" api/modelkart/talimatlar daki apiden dönen message ın değeri.


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
            talimatCheckedValues = [];
            modelkart.get.talimatlar(id);
        }

    },
    varyant: function (id) {
        if (varyantCheckedValues.length > 0) {

            var metodUrl = "api/modelkart/beden";
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
                    if (data != undefined && data.message == 'successful') { //Buradaki "Successful" api/modelkart/talimatlar daki apiden dönen message ın değeri.

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
            modelkart.get.varyantlar(id);
        }

    }
};
function varyantTemplate() {
    var val = '';// event.target.value;

    var dataArray = $("ul.bedenGruplari *").serializeArray();
    for (var beden, i = 0; beden = dataArray[i++];) {
        val += beden.value + ',';
    }

    var getBedenDetayListesi = parametre.bedenler.bedenDetayListeData(val);

    $('.groupDetayList').html('');

    if (getBedenDetayListesi != undefined && getBedenDetayListesi != null && getBedenDetayListesi.length > 0) {

        var tmpBedenGrubu = '';
        var tmplate = "";

        for (var item, i = 0; item = getBedenDetayListesi[i++];) {
            var bedengrubu = item.bedengrubu;
            if (i == 1) {
                tmplate += '<div class="groupListTop w100">';
                tmplate += '<h2 class="border-bottom">' + bedengrubu + '</h2>';
                tmplate += '<ul class="groupList">';
                tmpBedenGrubu = bedengrubu;
            }

            tmplate += '<li class="bedenIdList"><label for="' + item.beden + '">' + item.beden + '</label><input type="checkbox" class="form-control bedenDetayListeiItem" name="' + item.beden + '" id="' + item.beden + '" value="' + item.beden_id + '" ></li>';

            if ((i == getBedenDetayListesi.length) || tmpBedenGrubu != bedengrubu) {
                tmplate += '</ul>';
                tmplate += '</div>';
                if (i < getBedenDetayListesi.length) {
                    tmplate += '<div class="groupListTop w100">';
                    tmplate += '<h2 class="border-bottom">' + bedengrubu + '</h2>';
                    tmplate += '<ul class="groupList">';
                }
                tmpBedenGrubu = bedengrubu;
            }
        }

        $('.groupDetayList').append(tmplate);

        /*Listelenen tüm bedenleri seçili hale getiriyoruz*/
        $('.bedenDetayListeiItem').prop("checked", true);

        /*iCheck aktif ediliyor*/
        genel.iCheck('.bedenDetayListeiItem');
    }
}
// Local Storage's Parts
var href = '/model/new';
// if Sorgusu Local Storage Kodlarının Sadece New Sayfasında Kullanılması İçin Belirtilmiştir. 
if (window.location.pathname == href) {
    //Sayfadaki Inputların Girilen Değerlerinin Local Storage Set İşleminin Yağıldığı Part
    // For All Input Elements Set Local Storage
    $("input").change(function () {
        let element = $(this);
        localStorage.setItem(element.attr("name"), element.val());
        localStorage[element.attr("name")] = element.val();
    });

    // For All Checkbox Elements Set Local Storage
    $("input[type=checkbox]").on('ifChecked ifUnchecked', function (event) {
        if (event.type == "ifChecked") {
            localStorage.setItem(event.target.name, 1);
        } else if (event.type == "ifUnchecked") {
            localStorage.removeItem(event.target.name);
        }
    });

    // For All Select Elements Set Local Storage
    $('select').change(function () {
        let element = $(this);
        localStorage.setItem(element.attr("name"), element.val());
    });

    // Sayfa Yenilendiğinde Local Storage Verilerini Input Nesnelerine İşlendiği Part
    $(window).load(function () {
        if (localStorage) {

            // For All Select Elements Get Local Storage
            $("select").each(function () {
                let element = $(this);
                var get = localStorage.getItem(element.attr("name"));
                $('select[name=' + element.attr("name") + '] option[value=' + get + ']').attr('selected', 'selected');
            });

            //        // For All Input Elements Get Local Storage
            $("input").each(function () {
                let element = $(this);
                var get = localStorage.getItem(element.attr("name"), element.val());
                $('input[name=' + element.attr("name") + ']').val(get);
            });

            //        // For All Checkbox Elements Get Local Storage
            $("input[type=checkbox]").each(function () {
                let element = $(this);
                var value = localStorage.getItem(element.attr("name"));
                if (value == 1) {
                    element.iCheck('check');
                }
            });
        }
    });
}