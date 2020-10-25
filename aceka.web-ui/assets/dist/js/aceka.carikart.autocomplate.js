/*
aceka.genel.js dosyasından gelecek
var apiUrl = 'http://92.45.23.86:9597/';
*/

/*
aceka.carikart.js dosyasından gelecek
var cari = {};
*/

var carikart = {};

carikart.autocomplate = {
    cariListeTuruBayiVeCari: function () {
        var metodUrl = 'api/cari/cari-liste-turu-bayi-ve-cari';
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonData.push({ 'id': obj.carikart_id, 'value': obj.cari_unvan });
                    });
                }
            }
        });
        return jsonData;
    },
    cariKartTipleri: function () {
        var metodUrl = 'api/parametre/cari-kart-tipleri';

        var jsonData = [];

        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {

                if (data != null) {
                    $.each(data, function (key, obj) {

                        jsonData.push({ 'id': obj.carikart_tipi_id, 'value': obj.carikart_tipi_adi });

                    });
                }
            }
        });

        return jsonData;
    },
    muhasebeKodlari: function () {
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
                        jsonData.push({ 'id': obj.muh_kod_id, 'value': obj.muh_kod_adi });
                    });
                }
            }
        });
        return jsonData;
    },
    bankaListesi: function () {
        var metodurl = 'api/parametre/banka-liste';
        var jsonDta = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonDta.push({ 'id': obj.banka_id, 'value': obj.banka_adi });
                    });
                }
            }
        });
        return jsonDta;
    },
    bankaSubeListesi: function (banka_id) {
        var metodurl = 'api/parametre/banka-subeler/' + banka_id;
        var jsonDta = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonDta.push({ 'id': obj.banka_sube_id, 'value': obj.sube_adi_kucuk, 'additionalValue': obj.banka_sube_kodu });
                    });
                }
            }
        });
        return jsonDta;
    },

    cariParametreListe: function () {
        var metodurl = 'api/cari/cari-parametreleri-getir';
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonData.push({ 'id': obj.parametre_id, 'value': obj.tanim });
                    });

                }
            }
        });
        return jsonData;
    },
    ozelAlanlarListe: function () {
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
                        jsonData.push({ 'id': obj.carikart_id, 'value': obj.cari_unvan_kucuk });
                    });
                }
            }
        });
        return jsonData;
    },
    cariSubeListesi: function (carikart_id) {
        var metodurl = 'api/cari/cari-sube-listesi/' + carikart_id;
        var jsonData = [];
        $.ajax({
            type: "GET",
            url: apiUrl + metodurl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonData.push({ 'id': obj.carikart_id, 'value': obj.cari_unvan });
                    });
                }
            }
        });
        return jsonData;
    },
    varsayilanSatici: function () {
        var metodUrl = 'api/stokkart/varsayilan_satici';

        var jsonData = [];

        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {

                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonData.push({ 'id': obj.carikart_id, 'value': obj.cari_unvan });
                    });
                }
            }
        });

        return jsonData;
    },
    vergiDaireleri: function () {
        var metodUrl = 'api/parametre/vergidaireleri';

        var jsonData = [];

        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {

                if (data != null) {
                    $.each(data, function (key, obj) {
                        jsonData.push({ 'id': obj.vergi_daire_no, 'value': obj.vergi_daire_adi });
                    });
                }
            }
        });
        return jsonData;
    }


};


