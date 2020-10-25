/*
aceka.genel.js dosyasından gelecek
var apiUrl = 'http://92.45.23.86:9597/';
*/

var stokkart = {};


stokkart.autocomplate = {
    stokTipAdiArama: function (stokkart_turu_id) {
        var metodUrl = 'api/stokkart/stok-tipadi-arama/' + stokkart_turu_id;
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {

                if (data != null) {
                    $.each(data, function (key, obj) {

                        jsonData.push({ 'id': obj.stokkart_id, 'value': obj.stokkodu_kucuk });

                    });
                }
            }
        });

        return jsonData;
    },
    /*
    object 2 değer alabilir
    1. object.stokkart_tipi_id
    2. object.stok_adi

    Not: object.stokkart_tipi_id : 1->Mamul Model Kartı, 2->Hizmet Kartı, 3->Sarf Malzemesi, 4->Kıymetli Ürün, 13->Sabit Kıymet, 20->Kumaş Kartı, 21->Aksesuar Kartı,22->İplik Kartı

    */
    stokAdiListesi: function (object) {

        var metodUrl = "";
        if (object.stokkart_tipi_id != undefined && object.stokkart_tipi_id != '') {
            if (object.stok_adi != undefined && object.stok_adi != '') {
                //api/stokkart/stok-adi-arama/{stok_adi},{stokkart_tipi_id}
                metodUrl = 'api/stokkart/stok-adi-arama/' + object.stok_adi + ',' + object.stokkart_tipi_id;
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
    /*
    object 2 değer alabilir
    1. object.stokkart_tur_id
    2. object.stok_adi
    3. object.stok_kodu

    Not: object.stokkart_tur_id : 0->Mamul , 1-> Yarı Mamül , 2->İlk Madde (Kumaş,Aksesuar ve iplik), 9->Diğer

    */
    stokAdiTurListesi: function (object) {

        var metodUrl = "";

        if (object.stokkart_tur_id != undefined) {

            if (object.stok_kodu != undefined && object.stok_kodu != '') {
                //api/stokkart/stok-tipadi-arama-stokkodu-ile/{stok_kodu},{stokkart_tur_id}
                metodUrl = 'api/stokkart/stok-tipadi-arama-stokkodu-ile/' + object.stok_kodu + ',' + object.stokkart_tur_id;
            }
            else if (object.stok_adi != undefined && object.stok_adi != '') {
                //api/stokkart/stok-tipadi-arama/{stok_adi},{stokkart_tur_id}
                metodUrl = 'api/stokkart/stok-tipadi-arama/' + object.stok_adi + ',' + object.stokkart_tur_id;
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

    //pop up daki renk adını dolduruyoruz.
    renklerListesi: function (object) {

        var metodUrl = "";
        if (object.renk_adi != undefined && object.renk_adi != '')
            //api/parametre/renkler?renk_adi
            metodUrl = 'api/parametre/renkler?renk_adi=' + object.renk_adi;
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonData.push({ 'id': obj.renk_id, 'value': obj.renk_adi, 'code': obj.renk_rgb });
                    });
                }
            }
        });

        return jsonData;
    }



};






