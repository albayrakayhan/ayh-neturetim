/*
aceka.genel.js dosyasından gelecek
var apiUrl = 'http://92.45.23.86:9597/';
Select işlemleri. combo box.
*/

var parametre = {};
parametre.init = function (options) {
    //default yüklenecek metodları buraya yazabiliriz.

    //Ülke, Şehir, İlçe, Semt Lookup Start
    if (options.select.ulkeControlId != undefined) {

        parametre.ulke_sehir_ilce_semt.ulkeler(options.select.ulkeControlId);

        $('#' + options.select.ulkeControlId).on('change', function () {
            var val = $(this).val();
            //console.log(val)
            if (val != '') {

                //Şehir select dolduruluyor
                parametre.ulke_sehir_ilce_semt.sehirler(options.select.sehirControlId, val);

                //İlçe select boşaltılıyor
                if (options.select.ilceControlId != undefined) {
                    parametre.ulke_sehir_ilce_semt.ilceler(options.select.ilceControlId, null);
                }
                //Semt select boşaltılıyor
                if (options.select.semtControlId != undefined) {
                    parametre.ulke_sehir_ilce_semt.semtler(options.select.semtControlId, null);
                }

            } else {

                //Şehir select boşaltılıyor
                parametre.ulke_sehir_ilce_semt.sehirler(options.select.sehirControlId, null);

                if (options.select.ilceControlId != undefined) {
                    parametre.ulke_sehir_ilce_semt.ilceler(options.select.ilceControlId, null);
                }
            }
        })
    }

    if (options.select.sehirControlId != undefined) {

        parametre.ulke_sehir_ilce_semt.sehirler(options.select.sehirControlId);

        $('#' + options.select.sehirControlId).on('change', function () {
            var val = $(this).val();
            if (val != '') {
                //İlçe select dolduruluyor. Şehir değiştiğinde ilçeleri de boşaltıyoruz.
                parametre.ulke_sehir_ilce_semt.ilceler(options.select.ilceControlId, val)
                //semtler select boşaltulıyor. Şehir değiştiğinde semtleri de boşaltıyoruz.
                parametre.ulke_sehir_ilce_semt.semtler(options.select.semtControlId, null)

            } else {
                //Şehir select boşaltılıyor
                parametre.ulke_sehir_ilce_semt.ilceler(options.select.ilceControlId, null)

            }
        })
    }

    if (options.select.ilceControlId != undefined) {

        parametre.ulke_sehir_ilce_semt.ilceler(options.select.ilceControlId);

        $('#' + options.select.ilceControlId).on('change', function () {
            var val = $(this).val();
            if (val != '') {
                //Semt select dolduruluyor
                parametre.ulke_sehir_ilce_semt.semtler(options.select.semtControlId, val)
            } else {
                //İlçe select boşaltılıyor
                parametre.ulke_sehir_ilce_semt.semtler(options.select.ilceControlId, null)
            }
        })
    }

    if (options.select.semtControlId != undefined) {
        parametre.ulke_sehir_ilce_semt.semtler(options.select.semtControlId);
    }
    //Ülke, Şehir, İlçe, Semt Lookup End

    //Adnan TURK 04.08.2017
    //2. Ülke, Şehir, Semt lookup için Start
    if (options.select.ulkeControl2Id != undefined) {

        parametre.ulke_sehir_ilce_semt.ulkeler(options.select.ulkeControl2Id);

        $('#' + options.select.ulkeControl2Id).on('change', function () {
            var val = $(this).val();
            //console.log(val)
            if (val != '') {

                //Şehir select dolduruluyor
                parametre.ulke_sehir_ilce_semt.sehirler(options.select.sehirControl2Id, val);

                //İlçe select boşaltılıyor
                if (options.select.ilceControl2Id != undefined) {
                    parametre.ulke_sehir_ilce_semt.ilceler(options.select.ilceControl2Id, null);
                }
                //Semt select boşaltılıyor
                if (options.select.semtControl2Id != undefined) {
                    parametre.ulke_sehir_ilce_semt.semtler(options.select.semtControl2Id, null);
                }

            } else {

                //Şehir select boşaltılıyor
                parametre.ulke_sehir_ilce_semt.sehirler(options.select.sehirControl2Id, null);

                if (options.select.ilceControl2Id != undefined) {
                    parametre.ulke_sehir_ilce_semt.ilceler(options.select.ilceControl2Id, null);
                }
            }
        })
    }

    if (options.select.sehirControl2Id != undefined) {

        parametre.ulke_sehir_ilce_semt.sehirler(options.select.sehirControl2Id);

        $('#' + options.select.sehirControl2Id).on('change', function () {
            var val = $(this).val();
            if (val != '') {
                //İlçe select dolduruluyor. Şehir değiştiğinde ilçeleri de boşaltıyoruz.
                parametre.ulke_sehir_ilce_semt.ilceler(options.select.ilceControl2Id, val)
                //semtler select boşaltulıyor. Şehir değiştiğinde semtleri de boşaltıyoruz.
                parametre.ulke_sehir_ilce_semt.semtler(options.select.semtControl2Id, null)

            } else {
                //Şehir select boşaltılıyor
                parametre.ulke_sehir_ilce_semt.ilceler(options.select.ilceControl2Id, null)

            }
        })
    }

    if (options.select.ilceControl2Id != undefined) {

        parametre.ulke_sehir_ilce_semt.ilceler(options.select.ilceControl2Id);

        $('#' + options.select.ilceControl2Id).on('change', function () {
            var val = $(this).val();
            if (val != '') {
                //Semt select dolduruluyor
                parametre.ulke_sehir_ilce_semt.semtler(options.select.semtControl2Id, val)
            } else {
                //İlçe select boşaltılıyor
                parametre.ulke_sehir_ilce_semt.semtler(options.select.ilceControl2Id, null)
            }
        })
    }

    if (options.select.semtControl2Id != undefined) {
        parametre.ulke_sehir_ilce_semt.semtler(options.select.semtControl2Id);
    }
    //2. Ülke, Şehir, Semt lookup için End

    /*Beden Grubu  Start*/
    if (options.select.bedenGrubuControlId != undefined) {
        parametre.bedenler.bedenGrubuListesi(options.select.bedenGrubuControlId);
        $('#' + options.select.bedenGrubuControlId).on('change', function () {
            var val = $(this).val();
            if (val != '') {
                //Bedenler select dolduruluyor. Beden Grubu değiştiğinde Bedenleri de dolduruyoruz.
                parametre.bedenler.bedenDetayListe(options.select.bedenDetayControlId, val)
            } else {
                //Beden Grubu select boşaltılıyor
                parametre.bedenler.bedenGrubuListesi(options.select.bedenGrubuControlId, null)

            }
        })
    }
    /*Beden Grubu  End*/

    /*Fiyat Tipleri  Start*/
    if (options.select.fiyattipiControlId != undefined) {
        parametre.fiyattipleri(options.select.fiyattipiControlId);
    }
    /*Fiyat Tipleri End*/

    /*Verigi Daireleri Start*/
    if (options.select.vergiDaireleriControlId != undefined) {
        parametre.vergidaireleri(options.select.vergiDaireleriControlId);
    }
    /*Verigi Daireleri End*/

    /*Sezonlar Start*/
    if (options.select.sezonControlId != undefined) {
        parametre.sezonlar(options.select.sezonControlId);
    }
    /*Sezonlar End*/

    /*Sipariş Türleri Start*/
    if (options.select.siparisturuControlId != undefined) {
        parametre.siparisTurleri(options.select.siparisturuControlId);
    }
    /*Sipariş Türleri End*/

    /*Zorluk Grupları Start*/
    if (options.select.zorlukGrubuControlId != undefined) {
        parametre.zorlukgrubu(options.select.zorlukGrubuControlId);
    }
    /*Zorluk Grupları End*/

    /*Şirket Listesi Start*/
    if (options.select.sirketListesiControlId != undefined) {
        parametre.sirketListesi(options.select.sirketListesiControlId);
    }
    /*Şirket Listesi End*/

    /*Cari Kart Tür Start*/
    if (options.select.carikartTurControlId != undefined) {
        parametre.carikartTurListesi(options.select.carikartTurControlId);
    }
    /*Cari Kart Tür End*/

    /*Cari Kart Tür Start*/
    if (options.select.stokkartTipControlId != undefined) {
        parametre.stokkartTipListesi(options.select.stokkartTipControlId,options.select.calback);
    }
    /*Cari Kart Tür End*/

    /*Kdv Start*/
    if (options.select.kdvControlId != undefined) {
        parametre.kdvListesi(options.select.kdvControlId);
    }
    /*Kdv End*/

    /*stokkart sku Start*/
    if (options.select.stokkartskuControlId != undefined) {
        parametre.stokkartskuListesi(options.select.stokkartskuControlId);
    }
    /*stokkart sku End*/

    /*Ölçü Birimi Start*/
    if (options.select.olcubirimControlId != undefined) {
        parametre.olcubirimListesi(options.select.olcubirimControlId);
    }
    /*Ölçü Birimi End*/

    /*Fiyat Tipi Start*/
    if (options.select.fiyattipControlId != undefined) {
        parametre.fiyattipListesi(options.select.fiyattipControlId);
    }
    /*Fiyat Tipi End*/

    /*Para Birimi Start*/
    if (options.select.parabirimControlId != undefined) {
        parametre.parabirimListesi(options.select.parabirimControlId);
    }
    /*Para Birimi End*/

    /*Bankalar Start*/
    if (options.select.bankaControlId != undefined) {
        parametre.bankaListesi(options.select.bankaControlId);
    }
    /*Bankalar End*/

    /*Şubeler Start*/
    //if (options.select.subeControlId != undefined) {
    //    parametre.subeListesi(options.select.subeControlId);
    //}
    /*Şubeler End*/

    /*Personel Görevler Start*/
    if (options.select.personelGorevControlId != undefined) {
        parametre.personelGorev(options.select.personelGorevControlId);
    }
    /*Personel Görevler End*/

    /*Personel Departman Start*/
    if (options.select.personelDepartmanControlId != undefined) {
        parametre.personelDepartman(options.select.personelDepartmanControlId);
    }
    /*Personel Departman End*/

    /*Personel Çalışma Yeri Start*/
    if (options.select.personelCalismaYeriControlId != undefined) {
        parametre.personelCalimaYeri(options.select.personelCalismaYeriControlId);
    }
    /*Personel Çalışma Yeri End*/

    /*Personel Şubeler Start*/
    if (options.select.personelSubeControlId != undefined) {
        parametre.personelsubeler(options.select.personelSubeControlId);
    }
    /*Personel Şubeler End*/

    /*Cari Ödeme  Start*/
    if (options.select.cariodemeControlId != undefined) {
        parametre.cariodemeListesi(options.select.cariodemeControlId);
    }
    /*Cari Ödeme  End*/

    /*Cari KArt Tipi  Start*/
    if (options.select.carikartTipControlId != undefined) {
        parametre.carikartTipListesi(options.select.carikartTipControlId,carikart_turu_id);
    }
    /*Cari KArt Tipi  End*/

    /*Rapor Parametreleri  Start*/
    if (options.select.raporParametreControlId != undefined) {
        parametre.raporParametreleri(options.select.raporParametreControlId);
    }
    /*Rapor Parametreleri  End*/

    /*Masraf merkezi   Start*/
    if (options.select.masrafMerkeziControlId != undefined) {
        parametre.masrafMerkeziListesi(options.select.masrafMerkeziControlId);
    }
    /*Masraf merkezi  End*/

    /*Talimat   Start*/
    if (options.select.talimatControlId != undefined) {
        parametre.talimatListesi(options.select.talimatControlId);
    }
    /*Talimat  End*/

    /*Fasoncu Listesi   Start*/
    if (options.select.fasoncuControlId != undefined) {
        parametre.fasoncuListesi(options.select.fasoncuControlId);
    }
    /*Fasoncu Listesi  End*/
    /*Üretim Yeri   Start*/
    if (options.select.uretimyeriControlId != undefined) {
        parametre.uretimYeriListesi(options.select.uretimyeriControlId);
    }
    /*Üretim Yeri  End*/

    /* Gerçek Üretim Yeri   Start*/
    if (options.select.gercekuretimyeriControlId != undefined) {
        parametre.gercekUretimYeriListesi(options.select.gercekuretimyeriControlId);
    }
    /*Gerçek Üretim Yeri  End*/

    /* Rapor Parametreleri Key Start*/
    if (options.select.keyListesiControlId != undefined) {
        parametre.keyListeleri(options.select.keyListesiControlId);
    }
    /*Rapor Parametreleri Key End*/

    /* Rapor Parametreleri Marka Start*/
    if (options.select.markaListesiControlId != undefined) {
        parametre.markaListeleri(options.select.markaListesiControlId);
    }
    /*Rapor Parametreleri Marka End*/

    /* Rapor Parametreleri Marka Start*/
    if (options.select.cinsListesiControlId != undefined) {
        parametre.cinsListeleri(options.select.cinsListesiControlId);
    }
    /*Rapor Parametreleri Marka End*/

    /* Rapor Parametreleri  Model Türü Start*/
    if (options.select.modelturuControlId != undefined) {
        parametre.modelTuruListeleri(options.select.modelturuControlId);
    }
    /*Rapor Parametreleri  Model Türü End*/

    /* Rapor Parametreleri  promo Start*/
    if (options.select.promoControlId != undefined) {
        parametre.promoListeleri(options.select.promoControlId);
    }
    /*Rapor Parametreleri  promo End*/

    /* Rapor Parametreleri  tür Start*/
    if (options.select.turControlId != undefined) {
        parametre.turListeleri(options.select.turControlId);
    }
    /*Rapor Parametreleri  tür End*/

    /* Rapor Parametreleri  Pelür Start*/
    if (options.select.pelurControlId != undefined) {
        parametre.pelurListeleri(options.select.pelurControlId);
    }
    /*Rapor Parametreleri  Pelür End*/

    /* Rapor Parametreleri  Pelür Start*/
    if (options.select.sponsorluControlId != undefined) {
        parametre.sponsorluListeleri(options.select.sponsorluControlId);
    }
    /*Rapor Parametreleri  Pelür End*/

    /* Ödeme Planı Start*/
    if (options.select.odemeplaniControlId != undefined) {
        parametre.odemePlani(options.select.odemeplaniControlId);
    }
    /*Ödeme Planı End*/

    /* Ödeme Planı Start*/
    if (options.select.muhasekodControlId != undefined) {
        parametre.muhasebekodlari(options.select.muhasekodControlId);
    }
    /*Ödeme Planı End*/

    /* Sene Listesi  Start*/
    if (options.select.seneControlId != undefined) {
        parametre.muhasebekodlari(options.select.seneControlId);
    }
    /*Sene Listesi End*/



    /*Beden Grubu Start*/
    //if (options.select.bedenGrubuControlId != undefined) {
    //    parametre.bedenler.bedenGrubuListesi(options.select.bedenGrubuControlId);
    //}
    /*Beden Grubu End*/
};
parametre.ulke_sehir_ilce_semt = {

    ulkeler: function (controlId) {
        var metodUrl = 'api/parametre/ulkeler';

        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            async: false,
            dataType: "json",
            success: function (data) {
                if (data != null) {

                    $('#' + controlId + ' option').remove();
                    $('#' + controlId).append('<option value="">Ülke Seçin</option>');

                    $.each(data, function (key, obj) {

                        $('#' + controlId).append('<option value="' + obj.ulke_id + '">' + obj.ulke_adi + '</option>');
                    });
                }
                else {
                    $('#' + controlId + ' option').remove();
                    $('#' + controlId).append('<option value="">Ülke Seçin</option>');
                }
            }
        });
    },

    sehirler: function (controlId, id) {
        var metodUrl = 'api/parametre/sehirler?ulke_id=' + id;
        if (id != null) {
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data != null) {

                        $('#' + controlId + ' option').remove();
                        $('#' + controlId).append('<option value="">Şehir Seçin</option>');

                        $.each(data, function (key, obj) {

                            $('#' + controlId).append('<option value="' + obj.sehir_id + '">' + obj.sehir_adi + '</option>');
                        });
                    }
                    else {
                        $('#' + controlId + ' option').remove();
                        $('#' + controlId).append('<option value="">Şehir Seçin</option>');
                    }
                }
            });
        } else {
            $('#' + controlId + ' option').remove();
            $('#' + controlId).append('<option value="">Şehir Seçin</option>');
        }

    },
    ilceler: function (controlId, id) {
        var metodUrl = 'api/parametre/ilceler?sehir_id=' + id;
        if (id != null) {
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data != null) {

                        $('#' + controlId + ' option').remove();
                        $('#' + controlId).append('<option value="">İlçe Seçin</option>');

                        $.each(data, function (key, obj) {

                            $('#' + controlId).append('<option value="' + obj.ilce_id + '">' + obj.ilce_adi + '</option>');
                        });
                    }
                    else {
                        $('#' + controlId + ' option').remove();
                        $('#' + controlId).append('<option value="">İlçe Seçin</option>');
                    }
                }
            });
        } else {
            $('#' + controlId + ' option').remove();
            $('#' + controlId).append('<option value="">İlçe Seçin</option>');
        }
    },
    semtler: function (controlId, id) {
        var metodUrl = 'api/parametre/semtler?ilce_id=' + id;
        if (id != null) {
            $.ajax({
                type: "GET",
                url: apiUrl + metodUrl,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data != null) {

                        $('#' + controlId + ' option').remove();
                        $('#' + controlId).append('<option value="">Semt Seçin</option>');

                        $.each(data, function (key, obj) {

                            $('#' + controlId).append('<option value="' + obj.semt_id + '">' + obj.semt_adi + '</option>');
                        });
                    }
                    else {
                        $('#' + controlId + ' option').remove();
                        $('#' + controlId).append('<option value="">Semt Seçin</option>');
                    }
                }
            });
        } else {
            $('#' + controlId + ' option').remove();
            $('#' + controlId).append('<option value="">Semt Seçin</option>');
        }
    }

};
parametre.fiyattipleri = function (controlId) {
    var metodurl = 'api/parametre/fiyat-tipleri';
    $.ajax({
        type: "GET",
        url: apiUrl + metodurl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Fiyat Tipi Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value= "' + obj.fiyattipi + '">' + obj.fiyattipi_adi + '</option>');
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Fiyat Tipi Seçin</option>');
            }
        }
    });
};
parametre.depoFiyatTipleri = function (controlId, carikart_id) {
    var metodUrl = 'api/parametre/depo-fiyat-tipleri/' + carikart_id;
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="0">Seçin</option>');
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.fiyattipi + '">' + obj.fiyattipi_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Seçin</option>');
            }
        }
    });
};
parametre.depoFiyatTips = function (controlId, carikart_id) {
    var metodUrl = 'api/parametre/depo-fiyat-tiplers/' + carikart_id;

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="0">Seçin</option>');
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.fiyattipi + '">' + obj.fiyattipi_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Seçin</option>');
            }
        }
    });
};
parametre.vergidaireleri = function (controlId) {
    var metodUrl = 'api/parametre/vergidaireleri';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        success: function (data) {
            if (data != null) {

                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Vergi Dairesi Seçin</option>');

                $.each(data, function (key, obj) {

                    $('#' + controlId).append('<option value="' + obj.vergi_daire_no + '">' + obj.vergi_daire_adi + '</option>');
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Vergi Dairesi Seçin</option>');
            }
        }
    });
};
parametre.zorlukgrubu = function (controlId) {
    var metodUrl = 'api/parametre/zorlukgrubu';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
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
parametre.sirketListesi = function (controlId) {
    var metodUrl = 'api/cari/sirket-listesi';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Şİrket Listesi Seçin</option>');

                $.each(data, function (key, obj) {
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
parametre.carikartTurListesi = function (controlId) {
    var metodUrl = 'api/parametre/cari-kart-turleri';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
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
parametre.personelGorev = function (controlId) {
    var metodUrl = 'api/personel/gorev-parametreler';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Görev Yeri Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.parametre_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Görev Yeri Seçin</option>');
            }
        }
    });
};
parametre.personelDepartman = function (controlId) {
    var metodUrl = 'api/personel/departman-parametreler';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Departman Seçin</option>');
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.parametre_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Departman Seçin</option>');
            }
        }
    });
};
parametre.personelCalimaYeri = function (controlId) {
    var metodUrl = 'api/personel/calismayerleri';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Çalışma Yeri Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.carikart_id + '">' + obj.cari_unvan + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Çalışma Yeri Seçin</option>');
            }
        }
    });
};
parametre.personelsubeler = function (controlId) {
    var metodUrl = 'api/cari/cari-liste-turu-bayi-ve-cari';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Şube Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.carikart_id + '">' + obj.cari_unvan + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Şube Seçin</option>');
            }
        }
    });
};
parametre.stokkartTipListesi = function (controlId, calback) {
    var metodUrl = 'api/parametre/stokkart-tipleri';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Stokkart Tipi Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.stokkarttipi + '">' + obj.tanim + '</option>')
                });

                if(typeof calback =='function'){
                    calback();
                }
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Stokkart Tipi Seçin</option>');
            }
        }
    });
};
parametre.bedenler = {

    bedenGrubuListesi: function (controlId) {
        var metodUrl = 'api/modelkart/parametre-beden';

        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            async: false,
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    $('#' + controlId + ' option').remove();
                    $.each(data, function (key, obj) {
                        $('#' + controlId).append('<option  type="checkbox" value="' + obj.bedengrubu + '">' + obj.bedengrubu + '</option>')
                    });
                }
                else {
                    $('#' + controlId + ' option').remove();
                    //$('#' + controlId).append('<option value="">Beden Grubu Seçin</option>');
                }
            }
        });
    },
    bedenDetayListe: function (controlId, id) {
        //api/parametre/bedenler?bedenGruplari={bedenGruplari}
        var metodUrl = 'api/parametre/bedenler?bedenGruplari=' + id;
        //if (id != null) {
        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            async: false,
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    $('#' + controlId + ' option').remove();
                    //$('#' + controlId).append('<option value="">Beden/Bedenleri Seçin</option>');

                    $.each(data, function (key, obj) {
                        $('#' + controlId).append('<option   type="checkbox" value="' + obj.beden_id + '">' + obj.beden_tanimi + '</option>');
                    });
                }
                else {
                    $('#' + controlId + ' option').remove();
                    $//('#' + controlId).append('<option value="">Beden Seçin</option>');
                }
            }
        });
        //} else {
        //    $('#' + controlId + ' option').remove();
        //    $('#' + controlId).append('<option value="">Beden Seçin</option>');
        //}

    },
    bedenGrubuListesiData: function () {
        var metodUrl = 'api/modelkart/parametre-beden';
        var retData = null;
        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            async: false,
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    retData = data;

                }
                else {
                    return null;
                }
            }
        });

        return retData;
    },
    bedenDetayListeData: function (id) {
        //api/parametre/bedenler?bedenGruplari={bedenGruplari}
        var metodUrl = 'api/parametre/bedenler?bedenGruplari=' + id;
        var retData = null;

        $.ajax({
            type: "GET",
            url: apiUrl + metodUrl,
            async: false,
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    retData = data;
                }
                else {

                }
            }
        });

        return retData;
    }

}
//parametre-beden
//parametre.bedenGrubuListesi = function (controlId) {
//    var metodUrl = 'api/modelkart/parametre-beden';
//    $.ajax({
//        type: "GET",
//        url: apiUrl + metodUrl,
//        async: false,
//        dataType: "json",
//        success: function (data) {
//            if (data != null) {
//                $('#' + controlId + ' option').remove();
//                $('#' + controlId).append('<option value="">Beden Grubu Seçin</option>');
//                $.each(data, function (key, obj) {
//                    $('#' + controlId).append('<option value="' + obj.bedengrubu + '">' + obj.bedengrubu + '</option>')
//                });
//            }
//            else {
//                $('#' + controlId + ' option').remove();
//                $('#' + controlId).append('<option value="">Beden Grubu Seçin</option>');
//            }
//        }
//    });
//};
parametre.kdvListesi = function (controlId) {
    var metodUrl = 'api/admin/kdv';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Kdv Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.kod + '">' + obj.oran + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Kdv Seçin</option>');
            }
        }
    });
};

parametre.olcubirimListesi = function (controlId) {
    var metodUrl = 'api/admin/birimliste';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
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
parametre.fiyattipListesi = function (controlId) {
    var metodUrl = 'api/parametre/fiyat-tipleri';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
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
parametre.parabirimListesi = function (controlId) {
    var metodUrl = 'api/admin/para-birimi';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Para Birimi</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.pb + '">' + obj.merkezbankasi_kodu + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Para Birimi</option>');
            }
        }
    });
};
parametre.bankaListesi = function (controlId) {
    var metodUrl = 'api/parametre/banka-liste';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="0">Banka Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.banka_id + '">' + obj.banka_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Banka Seçin</option>');
            }
        }
    });
};
parametre.subeListesi = function (controlId, id) {
    //api/parametre/banka-subeler/{banka_id}
    var metodUrl = 'api/parametre/banka-subeler/' + id;
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="0">Banka Şubesi Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.banka_sube_id + '">' + obj.banka_sube_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Banka Şubesi Seçin</option>');
            }
        }
    });
};
parametre.cariodemeListesi = function (controlId) {
    var metodUrl = 'api/parametre/cari-odeme-sekilleri';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="0">Cari Ödeme Seçin</option>');

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
parametre.carikartTipListesi = function (controlId, carikart_turu_id) {
    var metodUrl = 'api/parametre/cari-kart-tipleri/' + carikart_turu_id;

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="0">Cari Kart Tipi Seçin</option>');
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.carikart_tipi_id + '">' + obj.carikart_tipi_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Cari Kart Tipi Seçin</option>');
            }
        }
    });
};
parametre.depokartRaporParametreleri = function (controlId, parametre_grubu,parametre) {
    var metodUrl = 'api/parametre/depo-kart-liste/' + parametre_grubu  + ',' + parametre;
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="0">Seçin</option>');
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Seçin</option>');
            }
        }
    });
};
parametre.raporParametreleri = function (object) {
    var metodUrl = 'api/admin/cari-raporlar';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                var lookup = {};
                var labels = [];

                // tüm verileri labels a ekliyoruz.
                for (var item, i = 0; item = data[i++];) {
                    var parametre_adi = item.tanim;
                    var parametre = item.parametre
                    if (!(parametre in lookup)) {
                        lookup[parametre] = 1;
                        labels.push({ 'parametre_adi': parametre_adi, 'parametre': parametre });
                    }
                }
                // En son tüm Selectboxlar doluyor. Burada data-parametre deki değere göre selectler dolduruluyor.
                $.each(object.selectBoxes, function (key, obj) {
                    var parametre = $('#' + obj).attr('data-parametre');

                    $('#' + obj + ' option').remove();
                    $('#' + obj).append('<option value="0">Seçiniz...</option>')
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].parametre == parametre) {
                            $('#' + obj).append('<option value="' + data[i].parametre_id + '">' + data[i].tanim + '</option>')
                        }
                    }
                    if (obj == undefined) {
                        alert('Tanımsız' + Obj);
                    }
                })
            }
        }
    });
};
parametre.masrafMerkeziListesi = function (controlId) {
    var metodUrl = 'api/muhasebe/masraf-merkezleri';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Masraf Merkezi Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.masraf_merkezi_id + '">' + obj.masraf_merkezi_adi + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Masraf Merkezi Seçin</option>');
            }
        }
    });
};
parametre.talimatListesi = function (controlId) {
    var metodUrl = 'api/parametre/talimat-liste';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Talimat Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.talimatturu_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Talimat Seçin</option>');
            }
        }
    });
};
parametre.fasoncuListesi = function (controlId) {
    var metodUrl = 'api/modelkart/fasoncular';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Fasoncu Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.carikart_id + '">' + obj.cari_unvan + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Fasoncu Seçin</option>');
            }
        }
    });
};
parametre.uretimYeriListesi = function (controlId) {
    var metodUrl = 'api/parametre/uretimyerleri';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Üretim Yeri Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.uretimyeri_id + '">' + obj.uretimyeri_tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Üretim Yeri Seçin</option>');
            }
        }
    });
};
parametre.gercekUretimYeriListesi = function (controlId) {
    var metodUrl = 'api/parametre/uretimyerleri';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Gerçek Üretim Yeri Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.uretimyeri_id + '">' + obj.uretimyeri_tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Gerçek Üretim Yeri Seçin</option>');
            }
        }
    });
};
parametre.dosyaturleri = function (controlId) {
    //api/parametre/dosyaturleri
    var metodUrl = 'api/parametre/dosyaturleri';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.ekturu_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sponsor Seçin</option>');
            }
        }
    });

};
parametre.keyListeleri = function (controlId, parametregrubu, parametre) {
    //api/parametre/stokkart-rapor-parametre/{parametregrubu},{parametre}
    var metodUrl = 'api/parametre/stokkart-rapor-parametre/' + parametregrubu + ',' + parametre;

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Key Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Key Seçin</option>');
            }
        }
    });

};
parametre.markaListeleri = function (controlId, parametregrubu, parametre) {
    //api/parametre/stokkart-rapor-parametre/{parametregrubu},{parametre}
    var metodUrl = 'api/parametre/stokkart-rapor-parametre/' + parametregrubu + ',' + parametre;

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Marka Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Marka Seçin</option>');
            }
        }
    });

};
parametre.cinsListeleri = function (controlId, parametregrubu, parametre) {
    //api/parametre/stokkart-rapor-parametre/{parametregrubu},{parametre}
    var metodUrl = 'api/parametre/stokkart-rapor-parametre/' + parametregrubu + ',' + parametre;

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Cins Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Cins Seçin</option>');
            }
        }
    });

};
parametre.modelTuruListeleri = function (controlId, parametregrubu, parametre) {
    //api/parametre/stokkart-rapor-parametre/{parametregrubu},{parametre}
    var metodUrl = 'api/parametre/stokkart-rapor-parametre/' + parametregrubu + ',' + parametre;

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Model Türü Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Model Türü Seçin</option>');
            }
        }
    });

};
parametre.promoListeleri = function (controlId, parametregrubu, parametre) {
    //api/parametre/stokkart-rapor-parametre/{parametregrubu},{parametre}
    var metodUrl = 'api/parametre/stokkart-rapor-parametre/' + parametregrubu + ',' + parametre;

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Promo Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Promo Seçin</option>');
            }
        }
    });

};
parametre.turListeleri = function (controlId, parametregrubu, parametre) {
    //api/parametre/stokkart-rapor-parametre/{parametregrubu},{parametre}
    var metodUrl = 'api/parametre/stokkart-rapor-parametre/' + parametregrubu + ',' + parametre;

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Tür Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Tür Seçin</option>');
            }
        }
    });

};
parametre.pelurListeleri = function (controlId, parametregrubu, parametre) {
    //api/parametre/stokkart-rapor-parametre/{parametregrubu},{parametre}
    var metodUrl = 'api/parametre/stokkart-rapor-parametre/' + parametregrubu + ',' + parametre;

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Pelür Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Pelür Seçin</option>');
            }
        }
    });

};
parametre.stokkartskuListesi = function (controlId) {
    //api/parametre/stokkart-rapor-parametre/{parametregrubu},{parametre}
    var metodUrl = 'api/stokkart/tekvaryant';

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.sku_oto_field_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
            }
        }
    });

};
parametre.sponsorluListeleri = function (controlId, parametregrubu, parametre) {
    //api/parametre/stokkart-rapor-parametre/{parametregrubu},{parametre}
    var metodUrl = 'api/parametre/stokkart-rapor-parametre/' + parametregrubu + ',' + parametre;

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sponsor Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sponsor Seçin</option>');
            }
        }
    });

};
parametre.odemePlani = function (controlId) {
    var metodUrl = 'api/finans/odeme-planlari';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {

                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Ödeme Planı Seçin</option>');

                $.each(data, function (key, obj) {

                    $('#' + controlId).append('<option value="' + obj.odeme_plani_id + '">' + obj.odeme_plani_adi + '</option>');
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Ödeme Planı Seçin</option>');
            }
        }
    });
};
parametre.parametreListeler = function (controlId, parametregrubu, parametre) {
    //api/parametre/stokkart-rapor-parametre/{parametregrubu},{parametre}
    var metodUrl = 'api/parametre/stokkart-rapor-parametre/' + parametregrubu + ',' + parametre;

    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Seçiniz</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.parametre_id + '">' + obj.tanim + '</option>')
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Parametre Seçin</option>');
            }
        }
    });

};
/*
object parametregrubu,[selectBoxes] [Labels]
*/
parametre.parametreListeleri = function (object) {
    //api/parametre/stokkart-rapor-parametre/{parametregrubu}
    var metodUrl = 'api/parametre/stokkart-rapor-parametgrup/' + object.parametregrubu
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                var lookup = {};
                var labels = [];

                // tüm verileri labels a ekliyoruz.
                for (var item, i = 0; item = data[i++];) {
                    var parametre_adi = item.parametre_adi;
                    var parametre = item.parametre
                    if (!(parametre_adi in lookup)) {
                        lookup[parametre_adi] = 1;
                        labels.push({ 'parametre_adi': parametre_adi, 'parametre': parametre });
                    }
                }
                //Sonra tüm Labellar doluyor. Burada aynı isimli başlıkdan birden fazla var. 
                //Birden fazla olan kayıtları teke indiriyoruz.  Burada data-parametre deki değere göre label lar dolduruluyor. 
                if (labels.length > 0) {
                    $.each(object.labels, function (key, obj) {
                        var parametre = $('#' + obj).attr('data-parametre');

                        for (var i = 0; i < labels.length; i++) {
                            if (labels[i].parametre == parametre) {
                                $('#' + obj).text(labels[i].parametre_adi);
                            }
                        }
                    })
                }
                // En son tüm Selectboxlar doluyor. Burada data-parametre deki değere göre selectler dolduruluyor.
                $.each(object.selectBoxes, function (key, obj) {
                    var parametre = $('#' + obj).attr('data-parametre');

                    $('#' + obj + ' option').remove();
                    $('#' + obj).append('<option value="">Seçiniz...</option>')
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].parametre == parametre) {
                            $('#' + obj).append('<option value="' + data[i].parametre_id + '">' + data[i].tanim + '</option>')
                        }
                    }
                    if (obj == undefined) {
                        alert('Tanımsız' + Obj);
                    }
                })

            }
            else {
                //Yukarıdaki data null gelirse tüm label ların text değerlerini boşaltıyoruz.
                $.each(object.labels, function (key, obj) {
                    $('#' + obj).text('');
                })

                // En son tüm Selectboxların içini boşaltıyoruz.
                $.each(object.selectBoxes, function (key, obj) {
                    $('#' + obj + ' option').remove();
                })
            }
        }
    });

};
parametre.sezonlar = function (controlId) {
    var metodUrl = 'api/parametre/sezon';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {

                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sezon Seçin</option>');

                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.sezon_id + '">' + obj.sezon_adi + '</option>');
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sezon Seçin</option>');
            }
        }
    });
};
parametre.gtipBelgeList = function (controlId) {
    var metodUrl = 'api/genelayarlar/gtipBelgeler';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Belge Seçin</option>');
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.belge_id + '">' + obj.belgeno + ' - ' + obj.belge_tarihi + ' - ' + obj.bitis_tarihi + '</option>');
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sezon Seçin</option>');
            }
        }
    });
};
parametre.siparisTurleri = function (controlId) {
    var metodUrl = 'api/parametre/siparis-turu-liste';
    console.log(metodUrl);
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sipariş Türü Seçin</option>');
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.siparisturu_id + '">' + obj.siparisturu_tanim + '</option>');
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sipariş Türü Seçin</option>');
            }
        }
    });
};
parametre.muhasebekodlari = (function (controlId) {
    var metodurl = 'api/cari/muhasebe-kodlari';
    $.ajax({
        type: "GET",
        url: apiUrl + metodurl,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Muhasebe Kodu Seçin</option>');
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.muh_kod_id + '">' + obj.muh_kod_id + '-' + obj.muh_kod_adi + '</option>');
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Muhasebe Kodu Seçin</option>');
            }
        }
    });
});
parametre.seneler = (function (controlId) {
    var metodurl = 'api/kur/seneGetir';
    $.ajax({
        type: "GET",
        url: apiUrl + metodurl,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sene Seçin</option>');
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.sene + '">' + obj.sene + '</option>');
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sene Seçin</option>');
            }
        }
    });
});
parametre.aylar = (function (controlId) {
    var metodurl = 'api/kur/ayGetir';
    $.ajax({
        type: "GET",
        url: apiUrl + metodurl,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Ay Seçin</option>');
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.ay_no + '">' + obj.ay_adi + '</option>');
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Ay Seçin</option>');
            }
        }
    });
});
parametre.takvimseneler = (function (controlId) {
    var metodurl = 'api/GenelAyarlarSeneler';
    $.ajax({
        type: "GET",
        url: apiUrl + metodurl,
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sene Seçin</option>');
                $.each(data, function (key, obj) {
                    $('#' + controlId).append('<option value="' + obj.sene + '">' + obj.sene + '</option>');
                });
            }
            else {
                $('#' + controlId + ' option').remove();
                $('#' + controlId).append('<option value="">Sene Seçin</option>');
            }
        }
    });
});
/*
JQUERY PlugIn with Prototype
referanslar:
google search : https://www.google.com.tr/search?q=%24.fn+jquery&oq=%24.fn&aqs=chrome.1.69i57j0l5.2621j0j7&sourceid=chrome&ie=UTF-8
https://learn.jquery.com/plugins/basic-plugin-creation/
http://stackoverflow.com/questions/4083351/what-does-jquery-fn-mean
*/
$.fn.vergidaireleri = function () {

    var id = $(this).attr('id');

    var metodUrl = 'api/parametre/vergidaireleri';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        success: function (data) {
            if (data != null) {

                $('#' + id + ' option').remove();
                $('#' + id).append('<option value="">Vergi Dairesi Seçin</option>');

                $.each(data, function (key, obj) {

                    $('#' + id).append('<option value="' + obj.vergi_daire_no + '">' + obj.vergi_daire_adi + '</option>');
                });
            }
            else {
                $('#' + id + ' option').remove();
                $('#' + id).append('<option value="">Vergi Dairesi Seçin</option>');
            }
        }
    });
};
$.fn.sezonlar = function () {
    var id = $(this).attr('id');
    var metodUrl = 'api/parametre/sezon';
    $.ajax({
        type: "GET",
        url: apiUrl + metodUrl,
        dataType: "json",
        success: function (data) {
            if (data != null) {

                $('#' + id + ' option').remove();
                $('#' + id).append('<option value="s">Sezon Seçiniz</option>');

                $.each(data, function (key, obj) {

                    $('#' + id).append('<option value="' + obj.sezon_id + '">' + obj.sezon_adi + '</option>');
                });
            }
            else {
                $('#' + id + ' option').remove();
                $('#' + id).append('<option value="">Sezon Seçin</option>');
            }
        }
    });
};