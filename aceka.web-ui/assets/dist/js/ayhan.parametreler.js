/*
aceka.genel.js dosyasından gelecek
var apiUrl = 'http://92.45.23.86:9597/';
*/
var parametreX = {};

/*
options 12 tane değer alır
Zorluk Grubu:zorlukGrubuControlId 
*/
parametreX.init = function (options) {
    //default yüklenecek metodları buraya yazabiliriz.


    /*Zorluk Grubu Start*/
    if (options.select.zorlukGrubuControlId != undefined) {
        parametreX.zorlukgrubu(options.select.zorlukGrubuControlId);
    }
    /*Zorluk Grubu End*/

    /*Şirket Listesi Start*/
    if (options.select.sirketListesiControlId != undefined) {
        parametreX.sirketListesi(options.select.sirketListesiControlId);
    }
    /*Şirket Listesi End*/

    /*Cari Kart Tür Start*/
    if (options.select.carikartTurControlId != undefined) {
        parametreX.carikartTurListesi(options.select.carikartTurControlId);
    }
    /*Cari Kart Tür End*/

    /*Cari Kart Tür Start*/
    if (options.select.stokkartTipControlId != undefined) {
        parametreX.stokkartTipListesi(options.select.stokkartTipControlId);
    }
    /*Cari Kart Tür End*/

    /*Kdv Start*/
    if (options.select.kdvControlId != undefined) {
        parametreX.kdvListesi(options.select.kdvControlId);
    }
    /*Kdv End*/

    /*Kdv Satiış Start*/
    if (options.select.kdvControlId != undefined) {
        parametreX.kdvListesi(options.select.kdvControlId);
    }
    /*Kdv Satış End*/

    /*Ölçü Birimi Start*/
    if (options.select.olcubirimControlId != undefined) {
        parametreX.olcubirimListesi(options.select.olcubirimControlId);
    }
    /*Ölçü Birimi End*/

    /*Fiyat Tipi Start*/
    if (options.select.fiyattipControlId != undefined) {
        parametreX.fiyattipListesi(options.select.fiyattipControlId);
    }
    /*Fiyat Tipi End*/

    /*Para Birimi Start*/
    if (options.select.parabirimControlId != undefined) {
        parametreX.parabirimListesi(options.select.parabirimControlId);
    }
    /*Para Birimi End*/
    /*Cari Ödeme  Start*/
    if (options.select.cariodemeControlId != undefined) {
        parametreX.cariodemeListesi(options.select.cariodemeControlId);
    }
    /*Cari Ödeme  End*/

};


parametreX.zorlukgrubu = function (controlId) {
    var metodUrl = 'api/parametreX.zorlukgrubu';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Zorluk Grupubu Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.zorlukgrubu_id + '">' + obj.tanim + '</option>');
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Zorluk Grupubu Seçin</option>');
            }
        }
    });
};


parametreX.sirketListesi = function (controlId) {
    var metodUrl = 'api/cari/sirket-listesi';

    $.ajax({
        type: "GET",
        url:apiUrl + metodUrl,
        dataType:"json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Şİrket Listesi Seçin</option>');

                $.each(data, function(key,obj){
                    $('#' + controlId).append('<option value="' + obj.sirket_id + '">' + obj.sirket_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Şİrket Listesi Seçin</option>');
            }
        }
    });
};


parametreX.carikartTurListesi = function (controlId) {
    var metodUrl = 'api/parametreX.cari-kart-turleri';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Cari Kart Tür Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.carikart_turu_id + '">' + obj.carikart_turu_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Cari Kart Tür Seçin</option>');
            }
        }
    });
};

parametreX.stokkartTipListesi = function (controlId) {
    var metodUrl = 'api/parametreX.stokkart-tipleri';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                //$('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="0">Stokkart Tipi Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.stokkarttipi + '">' + obj.tanim + '</option>')
                });
            }
            else {
                //$('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="0">Stokkart Tipi Seçin</option>');
            }
        }
    });
};

parametreX.kdvListesi = function (controlId) {
    var metodUrl = 'api/admin/kdv';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="0">Kdv Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.kod + '">' + obj.oran + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="0">Kdv Seçin</option>');
            }

         
           
        }
    });
};

parametreX.olcubirimListesi = function (controlId) {
    var metodUrl = 'api/admin/birimliste';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Birim Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.birim_id + '">' + obj.birim_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Birim Seçin</option>');
            }
        }
    });
};

parametreX.fiyattipListesi = function (controlId) {
    var metodUrl = 'api/parametreX.fiyat-tipleri';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Fiyat Tipi Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.fiyattipi + '">' + obj.fiyattipi_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Fiyat Tipi Seçin</option>');
            }
        }
    });
};

parametreX.parabirimListesi = function (controlId) {
    var metodUrl = 'api/admin/para-birimi';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Para Birimi Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.pb + '">' + obj.merkezbankasi_kodu + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Para Birimi Seçin</option>');
            }
        }
    });
};

parametreX.cariodemeListesi = function (controlId) {
    var metodUrl = 'api/parametreX.cari-odeme-sekilleri';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Cari Ödeme Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.cari_odeme_sekli_id + '">' + obj.cari_odeme_sekli + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Cari Ödeme Seçin</option>');
            }
        }
    });
};









