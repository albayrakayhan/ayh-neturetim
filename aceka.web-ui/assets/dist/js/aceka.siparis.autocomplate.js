var siparissearch = {};

siparissearch.autocomplate = {

    SiparisNoListe: function (object) {
        var metodUrl = "";
        if (object.siparis_no != undefined && object.siparis_no != '')
            metodUrl = 'api/siparis/siparis-no?siparis_no=' + object.siparis_no;
        var jsonData = [];

        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonData.push({ 'id': obj.siparis_no, 'value': obj.siparis_no, 'code': obj.siparis_id });
                    });
                }
            }
        });

        return jsonData;
    },
    stokAdiTurListesi: function (object) {

        var metodUrl = "";

        if (object.stokkart_id != undefined) {

            if (object.stok_kodu != undefined && object.stok_kodu != '') {
                //api/stokkart/stok-tipadi-arama-stokkodu-ile/{stok_kodu},{stokkart_tur_id}
                metodUrl = 'api/stokkart/stok-tipadi-arama-stokkodu-ile/' + object.stok_kodu + ',' + object.stokkart_tur_id;
            }
            else if (object.stok_adi != undefined && object.stok_adi != '') {
                //api/stokkart/stok-tipadi-arama/{stok_adi},{stokkart_tur_id}
                //api/stokkart/stok-adi-arama/{stok_adi},{stokkart_tipi_id}
                metodUrl = 'api/stokkart/stok-adi-arama/' + object.stok_adi + ',' + object.stokkart_tur_id;
            }
            else {
                //api/stokkart/stok-adi-arama/{stokkart_tipi_id}
                metodUrl = 'api/stokkart/stok-tipadi-arama/' + object.stokkart_tur_id;
            }
        }
        var jsonData = [];

        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {

                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonData.push({ 'id': obj.stokkart_id, 'value': obj.stok_adi, 'code': obj.stok_kodu });
                    });
                }
            }
        });

        return jsonData;
    },
    stokAdiListesi: function (object) {

        var metodUrl = "";
        if (object.stokkart_tipi_id != undefined && object.stokkart_tipi_id != '') {
            if (object.stok_adi != undefined && object.stok_adi != '') {
                //api/stokkart/stok-adi-arama/{stok_adi},{stokkart_tipi_id}
                metodUrl = 'api/stokkart/stok-adi-arama/' + object.stok_adi + ',' + object.stokkarttipi;
            } else {
                //api/stokkart/stok-adi-arama/{stokkart_tipi_id}
                metodUrl = 'api/stokkart/stok-adi-arama/' + object.stokkart_tipi_id;
            }
        }
        var jsonData = [];

        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {

                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonData.push({ 'id': obj.stokkart_id, 'value': obj.stok_adi });

                    });
                }
            }
        });

        return jsonData;
    },
    stokKoduBul: function (object) {
        var metodUrl = "";
        if (object != undefined && object != '') {
            //api/stokkart/stok-kod-arama/{stok_kodu}
            metodUrl = 'api/stokkart/stok-kod-arama/' + object;
        }
        var jsonData = {};
        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {

                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonData.push({ 'id': obj.stokkart_id, 'value': obj.stok_adi });
                    });
                }
            }
        });
        return jsonData;
    }

}

