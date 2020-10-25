//var apiUrl = 'http://92.45.23.86:9597/';
var apiUrl = 'http://localhost:49629/';

var filePath = "/content/files/";

var genel = {};

genel.init = function () {
    //default yüklenecek metodları buraya yazabiliriz.
};

genel.filePath = function () {
    return filePath;
};

//Bu refresh yapınca ikisinde de vermedi.:)

genel.fileUploadPopup = function (object, id) {

    $.ajax({
        url: '/popup/fileupload?id=' + id,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {

        genel.modal("Dosya Yükleme", result, "yeni", object.event);

        $('#eklerFrom #hfId').val(object.stokkart_id);
        $('#eklerFrom #stok_adi').val($('#stok_adi').val());
        $('#eklerFrom #labelModelKodu1').text($('#stok_kodu').val());
        $('#eklerFrom #labelModelKodu2').text($('#orjinal_stok_kodu').val());
    });

};

genel.onayGecmisiGet = function (id) {

    $.ajax({
        url: '/popup/onaygecmisi',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Onay Geçmişi", result, "liste", "$('#myModal').modal('hide');");
        var metodUrl = 'api/stokkart/onay-loglari/' + id
        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data != null) {
                    $('#onayGecmisiTable').DataTable({
                        data: data,
                        paging: true,
                        destroy: true,
                        lengthChange: false,
                        searching: false,
                        ordering: false,
                        info: true,
                        autoWidth: true,
                        columns: [
                            { data: "onaylayan_cari" },
                            {
                                data: "onay_tarihi",
                                render: function (data, type, row) {
                                    if (type === 'display') {
                                        return genel.dateFormatWithTime(row.onay_tarihi);
                                    }
                                    return data;
                                }
                            },
                            { data: "iptal_eden_cari" },
                            {
                                data: "iptal_tarihi",
                                render: function (data, type, row) {
                                    if (type === 'display') {
                                        if (row.iptal_tarihi != null)
                                            return genel.dateFormatWithTime(row.iptal_tarihi);
                                        else return '';
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
                        checkbox();
                    });
                }
            }
        });
    });

}

genel.onayGecmisiPut = function (id, status) {
    var retVal = false;
    var metodUrl = "api/stokkart/onay";

    var postData = {
        "stokkart_id": parseInt(id),
        "genel_onay": status,
        "malzeme_onay": false,
        "yukleme_onay": false,
        "uretim_onay": false

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
    }).success(function (result) {
        if (result != undefined) {
            retVal = true;
        }
    });

    return retVal;

}

genel.varyantGet = function (object) {
    $.ajax({
        url: '/popup/varyant',
        //url: '/popup/varyantlar?id=' + id,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Varyantlar", result, "yeni", object.event);
        //Stokkart id bilgisi form içerisindeki hfId ye atanıyor. Bu sayede post metodu kullanılacak!
        genel.iCheck();
        $('#varyantFrom > #hfId').val(object.stokkart_id);

        if (object.data != undefined && object.data != null) {
            //içerideki alanlar doldurulacak


        }
    });
}

genel.ilkMaddeGet = function (object) {
    $.ajax({
        url: '/popup/IlkMadde',
        //url: '/popup/varyantlar?id=' + id,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("İlk Madde", result, "yeni", object.event);


        if (object.data != undefined && object.data != null) {
            //içerideki alanlar doldurulacak

        }
    });
}

genel.fiyatpopGet = function (object) {
    $.ajax({
        url: '/popup/stokkartFiyatPopUp',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Genel Fiyat Tipleri", result, "yeni", object.event);
        //Stokkart id bilgisi form içerisindeki hfId ye atanıyor. Bu sayede post metodu kullanılacak!
        genel.iCheck();
        $('#fiyatpopupForm > #hfId').val(object.stokkart_id);

        if (object.data != undefined && object.data != null) {
            //içerideki alanlar doldurulacak
            $('.modal-header  .modal-title').text('Fiyat Güncelle');
            $('#fiyatpopupForm  #fiyat').val(object.data.fiyat);
            $('#fiyatpopupForm  #fiyattipi').val(object.data.fiyattipi);
            $('#fiyatpopupForm  #pb').val(object.data.pb);
            $('#fiyatpopupForm  #tarih').val(object.data.tarih);

            $('#genericModalButton').text("Kaydet");

        }
    });
}

genel.bankaHesapPopGet = function (object) {
    $.ajax({
        url: '/popup/finansBankaHesap',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Banka Hesap Bilgileri", result, "yeni", object.event);
        //Cari k art id bilgisi form içerisindeki hfId ye atanıyor. Bu sayede post metodu kullanılacak!
        //genel.iCheck();
        $('#bankaHesapForm > #hfId').val(object.carikart_id);
        if (object.data != undefined && object.data != null) {
            $('.modal-header  .modal-title').text('Banka Hesap Güncelleme');
            $('#bankaHesapForm  #banka_id').val(object.data.banka_id);
            $('#bankaHesapForm  #banka_sube_id').val(object.data.banka_sube_id);
            $('#bankaHesapForm  #pb').val(object.data.pb);
            $('#bankaHesapForm  #ibanno').val(object.data.ibanno);
            $('#bankaHesapForm  #kredi_limiti_dbs').val(object.data.kredi_limiti_dbs);

            $('#genericModalButton').text("Kaydet");
        }
    });
}

genel.finansIletisimPopGet = function (object) {
    $.ajax({
        url: '/popup/finansIletisim',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("İletişim Bilgileri", result, "yeni", object.event);
        //Cari k art id bilgisi form içerisindeki hfId ye atanıyor. Bu sayede post metodu kullanılacak!
        //genel.iCheck();
        $('#finansIletisimForm > #hfId').val(object.carikart_id);
        if (object.data != undefined && object.data != null) {
            $('.modal-header  .modal-title').text('İletişim Bilgisi Güncelleme');
            $('#finansIletisimForm  #carikart_id').val(object.data.carikart_id);
            $('#finansIletisimForm  #yetkili_ad_soyad').val(object.data.yetkili_ad_soyad);
            $('#finansIletisimForm  #yetkili_tel').val(object.data.yetkili_tel);
            $('#finansIletisimForm  #websitesi').val(object.data.websitesi);
            $('#finansIletisimForm  #email').val(object.data.email);
            $('#finansIletisimForm  #carikart_adres_id').val(object.data.carikart_adres_id);

            $('#genericModalButton').text("Kaydet");
        }
    });
}

genel.finansNotlarPopGet = function (object) {
    $.ajax({
        url: '/popup/finansNotlar',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Not Bilgileri", result, "yeni", object.event);
        $('#finansNotlarFrm > #hfId').val(object.carikart_id);
        if (object.data != undefined && object.data != null) {
            $('.modal-header  .modal-title').text('Notlar Güncelleme');
            $('#finansNotlarFrm  #carikart_id').val(object.data.carikart_id);
            $('#finansNotlarFrm  #carikart_not_id').val(object.data.carikart_not_id);
            $('#finansNotlarFrm  #nereden').val(object.data.nereden);
            $('#finansNotlarFrm  #aciklama').val(object.data.aciklama);

            $('#genericModalButton').text("Kaydet");
        }
    });
}

genel.searchAksesuarPopup = function(object) {
    console.log("genel.searchAksesuarPopup");
    myWindow.kendoWindow({
        width: "960px",
        title: "Aksesuar Arama",
        content: "/popup/SearchAksesuarPopup",
        visible: false,
        position: {
            top: "100px", // or "100px"
            left: "20%"
        },
        actions: [
            "Pin",
            "Minimize",
            "Maximize",
            "Close"
        ]
        //close: onClose
    }).data("kendoWindow").open();
    //$.ajax({
    //    url: '/popup/SearchAksesuarPopup',
    //    contentType: 'application/html; charset=utf-8',
    //    type: 'GET',
    //    async: false,
    //    dataType: 'html',
    //}).success(function(result) {
    //    //genel.modal("Aksesuar Arama", result, "liste", object.event);
       
    //});
}

genel.finansSubePopGet = function (object) {
    $.ajax({
        url: '/popup/finansSube',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Şube Bilgileri", result, "yeni", object.event);
        $('#finansSubeForm > #hfId').val(object.carikart_id);
        if (object.data != undefined && object.data != null) {
            $('.modal-header  .modal-title').text('Notlar Güncelleme');
            $('#finansSubeForm  #carikart_id').val(object.data.carikart_id);
            $('#finansSubeForm  #ana_carikart_id').val(object.data.ana_carikart_id);
            //$('#finansNotlarForm  #nereden').val(object.data.nereden);
            //$('#finansNotlarForm  #aciklama').val(object.data.aciklama);

            $('#genericModalButton').text("Kaydet");
        }
    });
}

genel.depoFiyatPopup = function (object) {
    $.ajax({
        url: '/popup/depokartFiyatPopup',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Fiyat Tip Bilgileri", result, "yeni", object.event);
        $('#depoFiyForm > #hfId').val(object.carikart_id);
        if (object.data != undefined && object.data != null) {
            $('.modal-header  .modal-title').text('Fiyat Tipi Güncelleme');
            $('#depoFiyForm  #carikart_id').val(object.data.carikart_id);

            $('#genericModalButton').text("Kaydet");
        }
    });
}

genel.stokkartvaryantGet = function (object) {
    $.ajax({
        url: '/popup/stokkartvaryant',
        //url: 'api/parametre/renkler?renk_adi==' + id,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Renkler", result, "yeni", object.event);
        //Stokkart id bilgisi form içerisindeki hfId ye atanıyor. Bu sayede post metodu kullanılacak!
        //genel.iCheck();
        $('#varyantFrom > #hfId').val(object.stokkart_id);
        //$('#varyantFrom > #renk_id').val(object.renk_id);
        if (object.data != undefined && object.data != null) {
            //içerideki alanlar doldurulacak
        }
    });
}

genel.benzerPopupGet = function (object) {
    $.ajax({
        url: '/popup/BenzerPopup',
        //url: 'api/parametre/renkler?renk_adi==' + id,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Benzer Kartlar", result, "yeni", object.event);
        //Stokkart id bilgisi form içerisindeki hfId ye atanıyor. Bu sayede post metodu kullanılacak!
        //genel.iCheck();
        //$('#benzerstkFrom > #hfId').val(object.stokkart_id);
        if (object.data != undefined && object.data != null) {
            //içerideki alanlar doldurulacak
        }
    });
}

genel.modelkartSearchPopup = function (object) {
    $.ajax({
        url: '/popup/modelKartSecimPopup',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Model Kart/Kartları Seçiniz", result, "yeni", object.event);
        //$('#eventInsForm > #hfId').val(object.siparis_id);

        setTimeout(function () {

            var autocompleteControl = $('#myModal').parent().find('#stokKodu');

            autocompleteControl.autocomplete({
                source: function (request, response) {

                    var data = stokkart.autocomplate.stokAdiTurListesi({ stokkart_tur_id: 0, stok_kodu: request.term });
                    response($.map(data, function (item) {
                        return { label: item.code, value: item.code, id: item.id };
                    }));
                },
                select: function (event, ui) {
                    autocompleteControl.attr('data-id', ui.item.id);
                },
                minLength: 2

            });
        }, 100);

        if (object.data != undefined && object.data != null) {

            /*
            $('.modal-header  .modal-title').text('Notlar Güncelleme');
            $('#eventInsForm  #stokKodu').val(object.data.stokkart_id);
            $('#genericModalButton').text("Kaydet");
            */
        }
    });
}
/*
Parametreler: 
stokkart_id, 
event -> Popup açıldığında button verilecek event, 
sira_id -> Kayıt detayı çağırılacağı zaman kullanılacak id
*/
genel.talimatGet = function (object) {

    $.ajax({
        url: '/popup/talimatPopup',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Talimat", result, "yeni", object.event);
        //Stokkart id bilgisi form içerisindeki hfId ye atanıyor. Bu sayede post metodu kullanılacak!

        $('#talimatForm > #hfId').val(object.stokkart_id);

        if (object.data != undefined && object.data != null) {
            //içerideki alanlar dolduruluyor
            $('.modal-header  .modal-title').text('Talimat Güncelle');
            $('#talimatForm  #siparis_id').val(object.data.siparis_id);
            $('#talimatForm  #sira_id').val(object.data.sira_id);
            $('#talimatForm  #eski_sira_id').val(object.data.eski_sira_id);
            $('#talimatForm  #islem_sayisi').val(object.data.islem_sayisi);
            $('#talimatForm  #aciklama').val(object.data.aciklama);
            $('#talimatForm  #irstalimat').val(object.data.irstalimat);
            $('#talimatForm  #talimatturu_id1').val(object.data.talimatturu_id);
            $('#talimatForm  #fasoncu_carikart_id1').val(object.data.fasoncu_carikart_id);

            $('#genericModalButton').text("Kaydet");
        }
    });
}

genel.siparisNotlarPopGet = function (object) {
    $.ajax({
        url: '/popup/siparisNotPopup',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Not Bilgileri", result, "yeni", object.event);
        $('#finansNotlarForm > #hfId').val(object.carikart_id);
        if (object.data != undefined && object.data != null) {
            $('.modal-header  .modal-title').text('Sipariş Not Güncelleme');
            $('#siparisNotForm  #siparis_id').val(object.data.siparis_id);
            $('#siparisNotForm  #sira_id').val(object.data.sira_id);
            $('#siparisNotForm  #aciklama').val(object.data.aciklama);

            $('#genericModalButton').text("Kaydet");
        }
    });
}

genel.personelcalismaYeriPopGet = function (object) {
    $.ajax({
        url: '/popup/personelCalismaYeri',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
    }).success(function (result) {
        genel.modal("Çalışma Yeri", result, "yeni", object.event);
        $('#calismayeriForm > #hfId').val(object.carikart_id);
        if (object.data != undefined && object.data != null) {
            $('.modal-header  .modal-title').text('Çalışma Yeri Ekleme');
            $('#calismayeriPopup  #calismayeri').val(object.data.calismayeri);
            $('#calismayeriPopup  #gorev').val(object.data.gorev);
            $('#calismayeriPopup  #departman').val(object.data.departman);

            $('#genericModalButton').text("Kaydet");
        }
    });
}

genel.dateFormat = function (inputFormat) {
    if (inputFormat != null) {
        function pad(s) { return (s < 10) ? '0' + s : s; }
        var d = new Date(inputFormat);
        return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('.');
    }
}

genel.dateFormatWithTime = function (inputFormat) {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat);
    return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('.') + ' ' + d.getHours() + ':' + d.getMinutes();
}

///
//dateStr tarih değişkeni örn: "29.01.2017"
//splitChr split karakteri örn: "." yada "-"
///
genel.dateFormatByJavascript = function (dateStr, splitChr) {
    if (dateStr != "") {
        var parts = dateStr.split(splitChr);
        var date = new Date(parts[2], parts[1] - 1, parts[0], 0, 0, 0, 0);
        date.setDate(date.getDate() + 1);
        return date.toISOString();
    }

}


/*
modalTipi;
yeni : HTML olarak "yeni" ya da "update" işlemi yapılacak form çağırılır
liste : HTML olarak ekrana çıktı aldıracağımız zaman kullanılacak
sil: Silme işlemi onayı için kullanılacak popup.
uyarı: Standart uyarı popup ı.
onay: İşlem onaylama popup ı.
basarili: İşlem başarılı ise kullanılacak popup.
hata: Hatalı bir işlem var ise kullanılacak popup.

*/
genel.modal = function (title, message, modalTipi, event) {

    var customModal = $('#myModal');

    var genericModalButton = $('#genericModalButton');

    switch (modalTipi) {
        case 'yeni':
            customModal.removeClass();
            customModal.addClass('modal fade');

            customModal.find('.modal-body').find('p').html(message);

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-primary');
            genericModalButton.text("Ekle");
            break;
        case 'liste':
            customModal.removeClass();
            customModal.addClass('modal fade');

            customModal.find('.modal-body').find('p').html(message);

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-primary');
            genericModalButton.text("Kapat");
            break;
        case 'sil':
            customModal.removeClass();
            customModal.addClass('modal fade modal-danger');

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-outline');
            genericModalButton.text("Onayla");

            break;
        case 'uyari':
            customModal.removeClass();
            customModal.addClass('modal fade modal-warning');

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-outline');
            genericModalButton.text("Onayla");

            break;
        case 'onay':
            customModal.removeClass();
            customModal.addClass('modal fade modal-info');

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-outline');

            break;
        case 'basarili':
            customModal.removeClass();
            customModal.addClass('modal fade modal-info');

            genericModalButton.text("Kapat");

            break;
        case 'hata':
            customModal.removeClass();
            customModal.addClass('modal fade modal-danger');

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-outline');
            genericModalButton.text("Kapat");

            break;
        case 'kaydet':
            break;
        default:
    }

    if (event != '' && event != null) {

        genericModalButton.attr('onclick', event);
    }

    customModal.find('.modal-title').html(title);

    if (modalTipi != 'yeni') {
        customModal.find('.modal-body').find('p').html(message);
    }

    customModal.modal()                      // initialized with defaults
    customModal.modal({ keyboard: false })   // initialized with no keyboard
    customModal.modal('show')                // initializes and invokes show immediately
};

genel.modalWithoutBackground = function (title, message, modalTipi, event) {

    var customModal = null;

    customModal = $('#myModalWithoutBG');
    $('#myModalWithoutBG').css('background', 'none');
    $('#myModalWithoutBG .modal-content > .modal-body').find('p').html('');

    var genericModalButton = $('#genericModalButtonWithoutBG');

    switch (modalTipi) {
        case 'yeni':
            customModal.removeClass();
            customModal.addClass('modal fade');

            $('.modal-content > .modal-body').find('p').html(message);

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-primary');
            genericModalButton.text("Ekle");
            break;
        case 'liste':
            customModal.removeClass();
            customModal.addClass('modal fade');

            $('.modal-content > .modal-body').find('p').html(message);

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-primary');
            genericModalButton.text("Kapat");
            break;
        case 'sil':
            customModal.removeClass();
            customModal.addClass('modal fade modal-danger');

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-outline');
            genericModalButton.text("Onayla");

            break;
        case 'uyari':
            customModal.removeClass();
            customModal.addClass('modal fade modal-warning');

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-outline');
            genericModalButton.text("Onayla");

            break;
        case 'onay':
            customModal.removeClass();
            customModal.addClass('modal fade modal-info');

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-outline');

            break;
        case 'basarili':
            customModal.removeClass();
            customModal.addClass('modal fade modal-info');

            genericModalButton.text("Kapat");

            break;
        case 'hata':
            customModal.removeClass();
            customModal.addClass('modal fade modal-danger');

            genericModalButton.removeClass();
            genericModalButton.addClass('btn btn-outline');
            genericModalButton.text("Kapat");

            break;
        case 'kaydet':
            break;
        default:

    }

    if (event != '' && event != null) {
        genericModalButton.attr('onclick', event);
    }

    $('.modal-content > .modal-header > .modal-title').html(title);
    if (modalTipi != 'yeni') {
        $('.modal-content > .modal-body').find('p').html(message);
    }

    customModal.modal({
        backdrop: false,
        keyboard: false
    });
};

var myTimer;

genel.timer = function (milliseconds, event) {
    myTimer = setInterval(function () {
        if (event.length > 0) {
            eval(event);
        }
        stopTimer()
    }, milliseconds);
}

genel.form_isValid = function () {

    alert('Test_2');
}

//Mevcut URL in istediğimiz parametresinin değerini verir
// Örn URL : /cms/slider/gallery_detail/?id=14
// Sonuç : genel.getParameterByName('id') = değer
genel.getParameterByName = function (name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

genel.fileUpload = function (inputId) {

    var fileName = '';
    var input = document.getElementById(inputId);

    if (input.files.length > 0) {
        if (window.FormData !== undefined) {

            var retObj = [];
            var metodUrl = "api/FileUpload";

            var data = new FormData();
            for (var x = 0; x < input.files.length; x++) {
                data.append("file" + x, input.files[x]);
            }

            $.ajax({
                type: "POST",
                url: apiUrl + metodUrl,
                contentType: false,
                processData: false,
                async: false,
                data: data,
                success: function (result) {
                    retObj = result;
                },
                error: function (xhr, status, p3, p4) {
                    var err = "Error " + " " + status + " " + p3 + " " + p4;
                    if (xhr.responseText && xhr.responseText[0] == "{")
                        err = JSON.parse(xhr.responseText).Message;
                    console.log(err);
                }
            });
        } else {
            alert("This browser doesn't support HTML5 file uploads!");
        }
    }
    return retObj;
};

genel.fileDelete = function (files) {
    var retVal = false;
    var metodUrl = "api/modelkart/genel-ekler";
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
        data: JSON.stringify(files)
    }).success(function (data) {
        if (data != undefined && data.message == 'successful')
            retVal = true;
        else
            genel.modal("Hata!", "İşlemi yapılırken bir hata oluştu!", "hata", "$('#myModal').modal('hide');");

    }).error(function (jqXHR, exception) {
        var errorJson = JSON.parse(jqXHR.responseText);
        genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
    });

    return retVal;
};

//genel.fileUploadPopup = function (saveFunction) {
//    $.ajax({
//        dataType: 'json',
//        url: '/cms/FileUpload/SingleFile',
//        method: 'GET',
//        async: false,
//        dataType: 'html'
//    }).success(function (data) {
//        genel.modal("Upload a picture", data, "yeni", saveFunction);
//    }).error(function (xhr, status) {
//        alert(xhr + " -- " + status);
//    });
//};
/*
function form_validate(attr_id){
    var result = true;
    $('#'+attr_id).validator('validate');
    $('#'+attr_id+' .form-group').each(function(){
        if($(this).hasClass('has-error')){
            result = false;
            return false;
        }
    });
    return result;
}

*/

//var myVar = setInterval(function () { setColor() }, 300);
//function setColor() {
//    var x = document.body;
//    x.style.backgroundColor = x.style.backgroundColor == "yellow" ? "pink" : "yellow";
//}

function stopTimer() {
    clearInterval(myTimer);
}

/*Önemli bir fonksiyon. JSON array içerisinde kayıt arayacağız!
Adnan TURK 13.09.2017
*/
//genel.findInObject(TestObj, 'id', 'A'); 
genel.findInObject = function (obj, key, val) {
    var objects = [];
    for (var i in obj) {
        if (!obj.hasOwnProperty(i)) continue;
        if (typeof obj[i] == 'object') {
            objects = objects.concat(genel.findInObject(obj[i], key, val));
        } else if (i == key && obj[key] == val) {
            objects.push(obj);
        }
    }
    return objects;
}

genel.iCheck = function (control) {
    var input = 'input[type="checkbox"]';
    if (control != undefined && control != null && control != '') {
        input = control;
    }
    $(input).iCheck({
        checkboxClass: 'icheckbox_flat-blue',
        radioClass: 'iradio_flat-blue'
    });
}

// jquery fn prototype
$.fn.tarihComponent = function () {
    var id = $(this).attr('id');

    $("#" + id).datepicker({
        format: "dd.mm.yyyy",
        language: 'tr',
    });

    //$("#" + id).datepicker('setDate', new Date());
};

$.fn.tarihComponentKendo = function () {
    var id = $(this).attr('id');

    $("#" + id).kendoDatePicker({
        culture: "tr-TR",
        dateInput: true,
        value: new Date(),
        animation: {
            close: {
                effects: "zoom:out",
                duration: 300
            }
        }
    });
};
//$('.iCheck-helper').iCheck({
//    checkboxClass: 'icheckbox_flat-blue',
//    radioClass: 'iradio_flat-blue'
//});